using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Configuration;
using Newtonsoft.Json;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json.Linq;

namespace FAD.API.SDK
{
    public enum formatType
    {
        JSON,
        XML
    }
    public class ForeignAssistanceActivityData
    {
        public static List<ForeignAssistanceActivity> GetData(string type, string filter, string option, int year)
        {
            formatType format = formatType.JSON;
            List<ForeignAssistanceActivity> activities = new List<ForeignAssistanceActivity>();
            RootObject rootJSON = new RootObject();
            string url = string.Format("https://fedweb.pwc.com/dashboardserviceapi/dashboardserviceapi.svc/{0}/GetData?type={1}&filter={2}&option={3}&year={4}", format, type, filter, option, year);
            WebRequest request = WebRequest.Create(url);

            // Get the response.
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            // Get the stream containing content returned by the server.
            Stream dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();
            // Display the content.
            //Console.WriteLine(responseFromServer);
            switch (format)
            {
                case formatType.JSON:
                    rootJSON = JsonConvert.DeserializeObject<RootObject>(responseFromServer);
                    if (rootJSON.JSONDataResult.Count > 0 || rootJSON != null)
                    {
                        foreach (var item in rootJSON.JSONDataResult)
                        {
                            ForeignAssistanceActivity activity = new ForeignAssistanceActivity();

                            activity.AgencyName = item.AgencyName;
                            activity.Amount = item.Amount;
                            activity.Category = item.Category;
                            activity.FiscalYear = item.FiscalYear;
                            activity.OperatingUnit = item.OperatingUnit;
                            activity.Sector = item.Sector;

                            activities.Add(activity);
                        }
                    }
                    break;
                case formatType.XML:
                    var readers = new StringReader(responseFromServer);
                    var serializer = new XmlSerializer(typeof(List<ForeignAssistanceActivity>));

                    activities = (List<ForeignAssistanceActivity>)serializer.Deserialize(readers);
                    break;
                default:
                    break;
            }
            // Cleanup the streams and the response.
            reader.Close();
            dataStream.Close();
            response.Close();

            return activities;
        }
    }

    [JsonConverter(typeof(ForeignAssistanceActivityModelConverter))]
    public class ForeignAssistanceActivityModel
    {
        public int Page { get; set; }
        public int Pages { get; set; }
        public int Per_Page { get; set; }
        public int Total { get; set; }

        public List<ForeignAssistanceActivity> ForeignAssistanceActivities { get; set; }
    }
    public class ForeignAssistanceActivityModelConverter : JsonConverter
    {

        public override bool CanConvert(Type objectType)
        {
            if (objectType == typeof(ForeignAssistanceActivityModel))
            {
                return true;
            }

            return false;
        }

        public override object ReadJson(JsonReader reader, Type objectType
            , object existingValue, JsonSerializer serializer)
        {
            reader.Read(); //start array
            //reader.Read(); //start object
            JObject obj = (JObject)serializer.Deserialize(reader);

            //{"page":1,"pages":1,"per_page":"50","total":35}
            var model = new ForeignAssistanceActivityModel();

            model.Page = Convert.ToInt32(((JValue)obj["page"]).Value);
            model.Pages = Convert.ToInt32(((JValue)obj["pages"]).Value);
            model.Per_Page = Int32.Parse((string)((JValue)obj["per_page"]).Value);
            model.Total = Convert.ToInt32(((JValue)obj["total"]).Value);

            reader.Read(); //end object

            model.ForeignAssistanceActivities = serializer.Deserialize<List<ForeignAssistanceActivity>>(reader);

            reader.Read(); //end array

            return model;
        }

        public override void WriteJson(JsonWriter writer, object value
            , JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace FAD.API.SDK
{
    [XmlRoot("ForeignAssistanceActivity")]
    public class ForeignAssistanceActivity
    {
        [XmlAttribute("FiscalYear")]
        public string FiscalYear { get; set; }
        [XmlAttribute("AgencyName")]
        public string AgencyName { get; set; }
        [XmlAttribute("OperatingUnit")]
        public string OperatingUnit { get; set; }
        [XmlAttribute("Category")]
        public string Category { get; set; }
        [XmlAttribute("Sector")]
        public string Sector { get; set; }
        [XmlAttribute("Amount")]
        public string Amount { get; set; }
        [XmlAttribute("Text")]
        public string Text { get; set; }
    }

    public class JSONDataResult
    {
        public string AgencyName { get; set; }
        public string Amount { get; set; }
        public string Category { get; set; }
        public string FiscalYear { get; set; }
        public string OperatingUnit { get; set; }
        public string Sector { get; set; }
        public object Text { get; set; }
    }

    public class RootObject
    {
        public List<JSONDataResult> JSONDataResult { get; set; }
    }
}

using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using GlassDemo.Project.Demo;
using System;

namespace demo.portal.Areas.basic.Models
{
    [SitecoreType(TemplateId = Templates.FlightDeal.Id)]
    public class BaseDeal
    {
        [SitecoreId]
        public Guid Id { get; set; }

        [SitecoreInfo(SitecoreInfoType.Name)]
        public string Name { get; set; }

        [SitecoreField(FieldId = "{FD452AAD-537C-4D10-87C1-CF0707E84BA9}")]
        public int StartPrice { get; set; }

        [SitecoreField(FieldId = "{69AAF7C1-2C2D-469F-9741-59883227327A}")]
        public string PriceIncludes { get; set; }

        [SitecoreField(FieldId = "{B277F7B6-C806-41C3-B524-05EC23B72ED8}")]
        public Image HeaderImage { get; set; }
    }
}
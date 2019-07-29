using Glass.Mapper.Sc.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace demo.portal.Areas.basic.Models.Configurations
{
    public class PageModelMap : SitecoreGlassMap<PageModel>
    {
        public override void Configure()
        {
            Map(config =>
            {
                config.AutoMap();
                config.Id(f => f.Id);
                config.Field(f => f.Header).FieldId("{C4C6F216-5FA2-41E8-BF5D-748EBC7B2A60}");
            });
        }
    }
}
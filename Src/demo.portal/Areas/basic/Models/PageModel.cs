using Glass.Mapper.Sc.Fields;
using System;

namespace demo.portal.Areas.basic.Models
{

    public class PageModel : ItemBase
    {
        public virtual string Header { get; set; }

        public virtual string Body { get; set; }

        public virtual DateTime Date { get; set; }

        // public virtual ImageModel Image { get; set; }

        public virtual Image Image { get; set; }
    } 

}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace demo.portal.Areas.basic.Models
{
    public class ImageModel
    {
        public virtual string Src { get; set; }

        public virtual string Alt { get; set; }

        public virtual int Height { get; set; }

        public virtual int Width { get; set; }

        public virtual string Title { get; set; }

        public virtual Guid MediaId { get; set; }
    }

}
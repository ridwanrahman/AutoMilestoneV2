using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoMilestoneV2.Models
{
    public class StaffCarUploadModel
    {        
        public string name { get; set; }
        public string model { get; set; }

        public HttpPostedFileBase carPicture { get; set; }
    }
}
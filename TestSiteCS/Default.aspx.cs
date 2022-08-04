using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TestSiteCS
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLoadImage_Click(object sender, EventArgs e)
        {
            webPaint.DisplayImage("https://picsum.photos/200/300");
            webPaint.Visible = true;
        }

        protected void btnSaveImage_Click(object sender, EventArgs e)
        {
            string path = Server.MapPath("~/Images");
            string fileName = DateTime.Now.ToString().Replace("/", "-").Replace(" ", "- ").Replace(":", "") + ".png";
            string fullPath = path + "\\" + fileName;
            webPaint.UploadImage(fullPath);
            webPaint.Visible = false;
        }
    }
}
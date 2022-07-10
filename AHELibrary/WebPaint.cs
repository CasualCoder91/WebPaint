using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AHELibrary
{
    [DefaultProperty("OnClientResponseReceived")]
    [ToolboxData("<{0}:WebPaint runat=server></{0}:WebPaint>")]
    public class WebPaint : CompositeControl
    {
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]

        //private Image image;
        private string imageUrl;
        private Button button;

        [Bindable(true), Category("Appearance"), DefaultValue("")]
        public string ImageUrl
        {
            set
            {
                this.imageUrl = value;
            }
        }

        //public override int Width
        //{
        //    get { return (int)ViewState["Width"]; }
        //    set { ViewState["Width"] = value; }
        //}

        protected override void CreateChildControls()
        {
            this.Controls.Clear();
            //image = new Image();
            //image.Width = 500;
            //image.Height = 500;
            //image.BorderColor = System.Drawing.Color.Black;
            //image.BorderWidth = 4;
            //image.ID = "cropImage";
            //image.ImageUrl = imageUrl;
            //image.Visible = true;
            //this.Controls.Add(image);

            button = new Button();
            button.Text = "press";
            button.ID = "pressme";
            button.Width = 100;
            button.Height = 100;
            this.Controls.Add(button);


            base.CreateChildControls();
        }

        public override void RenderControl(HtmlTextWriter writer)
        {
            //image.RenderControl(writer);
            RenderContents(writer);
        }

        protected override void RenderContents(HtmlTextWriter output)
        {
            if (this.Visible)
            {
                output.AddAttribute(HtmlTextWriterAttribute.Id, "imageCanvas");
                output.AddAttribute(HtmlTextWriterAttribute.Width, this.Width.ToString());
                output.AddAttribute(HtmlTextWriterAttribute.Height, this.Height.ToString());
                output.AddAttribute("runat", "server");
                output.AddAttribute(HtmlTextWriterAttribute.Style, "border: 2px solid black");
                output.RenderBeginTag("canvas");
                output.RenderEndTag();
                output.RenderBeginTag("br");
                output.RenderEndTag();
                button.Attributes.Add("onclick", "return test()");
                button.RenderControl(output);
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            Page.ClientScript.RegisterClientScriptResource(typeof(WebPaint), "AHELibrary.Scripts.WebPaint.js");
        }

        public void DisplayImage(string imageURL)
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "id", $"drawImage(\"{imageURL}\")", true);
        }
    }
}

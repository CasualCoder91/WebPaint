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

        struct LineWidth
        {
            public uint Size { get; set; }
            public string Name { get; set; }

            public LineWidth(uint size)
            {
                this.Size = size;
                this.Name = $"{size} px";
            }
        }

        private static readonly List<LineWidth> lineWidths = new List<LineWidth> { 
            new LineWidth(1), 
            new LineWidth(2), 
            new LineWidth(3),
            new LineWidth(5), 
            new LineWidth(8),
            new LineWidth(12),
            new LineWidth(25),
            new LineWidth(35),
        };

        //private Image image;
        private string imageUrl;
        private Button button;
        private DropDownList toolSelectionDDL;
        private DropDownList sizesDDL;

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


        private void CreateCustomChildControls()
        {

            toolSelectionDDL = new DropDownList() { AutoPostBack = false };
            toolSelectionDDL.Items.Add(new ListItem("Rechteck"));
            toolSelectionDDL.Items.Add(new ListItem("Zuschneiden"));
            toolSelectionDDL.SelectedIndex = 0;
            toolSelectionDDL.Attributes.Add("onchange", $"setAction(this);");
            base.Controls.Add(toolSelectionDDL);

            sizesDDL = new DropDownList
            {
                DataSource = lineWidths,
                DataTextField = "Name",
                DataValueField = "Size",
            };
            sizesDDL.Attributes.Add("onchange", $"setLineWidth(this);");
            base.Controls.Add(sizesDDL);

            button = new Button();
            button.Text = "press";
            button.ID = "pressme";
            button.Width = 100;
            button.Height = 100;
            base.Controls.Add(button);

        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            CreateCustomChildControls();
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
                // stores currently drawn Shape during mouse movement
                output.AddAttribute(HtmlTextWriterAttribute.Id, "tempCanvas");
                output.AddAttribute(HtmlTextWriterAttribute.Width, this.Width.ToString());
                output.AddAttribute(HtmlTextWriterAttribute.Height, this.Height.ToString());
                output.AddAttribute("runat", "server");
                output.AddAttribute(HtmlTextWriterAttribute.Style, "display: none;");
                output.RenderBeginTag("canvas");
                output.RenderEndTag();

                // displayed Canvas. Here we render the current image
                output.AddAttribute(HtmlTextWriterAttribute.Id, "renderCanvas");
                output.AddAttribute(HtmlTextWriterAttribute.Width, this.Width.ToString());
                output.AddAttribute(HtmlTextWriterAttribute.Height, this.Height.ToString());
                output.AddAttribute("runat", "server");
                output.AddAttribute(HtmlTextWriterAttribute.Style, "border: 2px solid black");
                output.RenderBeginTag("canvas");
                output.RenderEndTag();
                output.RenderBeginTag("br");
                output.RenderEndTag();

                //
                toolSelectionDDL.DataBind();
                toolSelectionDDL.RenderControl(output);
                sizesDDL.DataBind();
                sizesDDL.RenderControl(output);

                // Testbutton
                button.Attributes.Add("onclick", "test(); return false;"); // "return false;" to avoid postback
                button.RenderControl(output);
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            Page.ClientScript.RegisterClientScriptResource(typeof(WebPaint), "AHELibrary.Scripts.WebPaint.js");
            Page.ClientScript.RegisterStartupScript(GetType(), "init", "init();", true);
        }

        public void DisplayImage(string imageURL)
        {
            Page.ClientScript.RegisterStartupScript(this.Page.GetType(), Guid.NewGuid().ToString(), $"drawImage(\"{imageURL}\");", true);
        }
    }
}

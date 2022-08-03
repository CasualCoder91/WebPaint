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

        class Action
        {
            public string Name { get; set; }
            public string DisplayName { get; set; }

            public Action(string name, string displayName)
            {
                this.Name = name;
                this.DisplayName = displayName;
            }

            public Action(string name)
            {
                this.Name = name;
                this.DisplayName = name;
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

        private List<Action> actions = new List<Action>
        {
            new Action("Rechteck"),
            new Action("Zuschneiden"),
        };

        //private Image image;
        private ImageButton rotateButton;
        private ImageButton undoButton;
        private DropDownList toolSelectionDDL;
        private DropDownList sizesDDL;

        public string Language
        {
            get { return (string)ViewState["Language"]; }
            set { ViewState["Language"] = value; }
        }


        private void CreateCustomChildControls()
        {
            // Textanzeige sprachsensitiv
            if (Language == "EN")
            {
                actions.ElementAt(0).DisplayName = "Rectangle";
                actions.ElementAt(1).DisplayName = "Trim";
            }

            toolSelectionDDL = new DropDownList() 
            {
                AutoPostBack = false,
                DataSource = actions,
                DataTextField = "DisplayName",
                DataValueField = "Name"
            };
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

            rotateButton = new ImageButton
            {
                ImageUrl = Page.ClientScript.GetWebResourceUrl(typeof(AHELibrary.WebPaint), "AHELibrary.Img.rotate.png"),
                ID = "rotateButton",
                ClientIDMode = ClientIDMode.Static,
            };
            base.Controls.Add(rotateButton);

            undoButton = new ImageButton
            {
                ImageUrl = Page.ClientScript.GetWebResourceUrl(typeof(AHELibrary.WebPaint), "AHELibrary.Img.undo.png"),
                ID = "undoButton",
                ClientIDMode = ClientIDMode.Static,
            };
            base.Controls.Add(undoButton);

        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            string css = "<link href=\"" + Page.ClientScript.GetWebResourceUrl(this.GetType(), "AHELibrary.WebPaint.css") + "\" type=\"text/css\" rel=\"stylesheet\" />";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "cssFile", css, false);

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
                output.AddAttribute(HtmlTextWriterAttribute.Id, "master");
                output.RenderBeginTag("div"); //Masterdiv

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
                //output.AddAttribute(HtmlTextWriterAttribute.Style, "border: 2px solid black");
                output.RenderBeginTag("canvas");
                output.RenderEndTag();
                output.RenderBeginTag("br");
                output.RenderEndTag();

                output.AddAttribute(HtmlTextWriterAttribute.Id, "menubar");
                output.RenderBeginTag("div"); // Menubar

                // Color selection
                output.AddAttribute(HtmlTextWriterAttribute.Id, "colorChoice");
                output.AddAttribute(HtmlTextWriterAttribute.Name, "colorChoice");
                output.AddAttribute(HtmlTextWriterAttribute.Value, "#ff0000");
                output.AddAttribute(HtmlTextWriterAttribute.Type, "color");
                output.AddAttribute(HtmlTextWriterAttribute.Onchange, "setColor(this);");
                output.RenderBeginTag("input");
                output.RenderEndTag();

                //
                toolSelectionDDL.DataBind();
                toolSelectionDDL.RenderControl(output);
                sizesDDL.DataBind();
                sizesDDL.RenderControl(output);

                rotateButton.Attributes.Add("onclick", "rotate(); return false;"); // "return false;" to avoid postback
                rotateButton.RenderControl(output);

                undoButton.RenderControl(output);

                output.RenderEndTag(); // close Menubar div

                output.RenderEndTag(); // close Master div
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            Page.ClientScript.RegisterClientScriptResource(typeof(WebPaint), "AHELibrary.Scripts.WebPaint.js");
            Page.ClientScript.RegisterStartupScript(GetType(), "init", "init();", true);
        }

        public void DisplayImage(string imageURL)
        {
            Page.ClientScript.RegisterStartupScript(this.Page.GetType(), Guid.NewGuid().ToString(), $"loadImage(\"{imageURL}\");", true);
        }
    }
}

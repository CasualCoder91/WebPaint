using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Alarich
{
    [ToolboxItem(true)]
    [System.Drawing.ToolboxBitmap(typeof(WebPaint), "Alarich.WebPaint.bmp")]
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

        private ImageButton rotateButton;
        private ImageButton undoButton;
        private DropDownList toolSelectionDDL;
        private DropDownList sizesDDL;

        /// <summary>
        /// Stores the currently displayed image as a Base64 string
        /// </summary>
        public HiddenField ImageData
        {
            get
            {
                HiddenField imageData = (HiddenField)ViewState["ImageData"];
                return imageData ?? new HiddenField();
            }
            private set { ViewState["ImageData"] = value;}
        }

        public string Language
        {
            get 
            {
                string text = (string)ViewState["Language"];
                return text ?? "EN";
            }
            set { ViewState["Language"] = value; }
        }

        public string SavePath
        {
            get { return (string)ViewState["SavePath"]; }
            set { ViewState["SavePath"] = value; }
        }

        public string DefaultColor
        {
            get 
            {
                string text = (string)ViewState["DefaultColor"];
                return text ?? "#000000";
            }
            set { ViewState["DefaultColor"] = value; }
        }

        public string CSSLink
        {
            get 
            {
                string text = (string)ViewState["CSSLink"];
                return text ?? Page.ClientScript.GetWebResourceUrl(this.GetType(), "Alarich.WebPaint.WebPaint.css");
            }
            set { ViewState["CSSLink"] = value; }
        }

        private void CreateCustomChildControls()
        {
            // Multi language support
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
                ImageUrl = Page.ClientScript.GetWebResourceUrl(typeof(Alarich.WebPaint), "Alarich.Resources.rotate.png"),
                ID = "rotateButton",
                ClientIDMode = ClientIDMode.Static,
            };
            base.Controls.Add(rotateButton);

            undoButton = new ImageButton
            {
                ImageUrl = Page.ClientScript.GetWebResourceUrl(typeof(Alarich.WebPaint), "Alarich.Resources.undo.png"),
                ID = "undoButton",
                ClientIDMode = ClientIDMode.Static,
            };
            base.Controls.Add(undoButton);

            ImageData = new HiddenField
            {
                ID = "imageData",
                ClientIDMode = ClientIDMode.Static,
            };
            base.Controls.Add(ImageData);

        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            string css = $"<link href=\"{CSSLink}\" type=\"text/css\" rel=\"stylesheet\" />";
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
                output.AddAttribute(HtmlTextWriterAttribute.Value, DefaultColor);
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

                undoButton.Attributes.Add("onclick", "undo(); return false;"); // "return false;" to avoid postback
                undoButton.RenderControl(output);

                output.RenderEndTag(); // close Menubar div

                output.RenderEndTag(); // close Master div

                ImageData.RenderControl(output);
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            Page.ClientScript.RegisterClientScriptResource(typeof(WebPaint), "Alarich.WebPaint.Scripts.WebPaint.js");
            Page.ClientScript.RegisterStartupScript(GetType(), "init", $"init(\"{DefaultColor}\");", true);
        }

        public void DisplayImage(string imageURL)
        {
            Page.ClientScript.RegisterStartupScript(this.Page.GetType(), Guid.NewGuid().ToString(), $"loadImage(\"{imageURL}\");", true);
        }

        public bool UploadImage(string path)
        {
            if (string.IsNullOrEmpty(ImageData.Value))
            {
                return false;
            }
            using (System.IO.FileStream fs = new System.IO.FileStream(path, System.IO.FileMode.Create))
            {
                using (System.IO.BinaryWriter bw = new System.IO.BinaryWriter(fs))
                {
                    byte[] data = Convert.FromBase64String(ImageData.Value);
                    bw.Write(data);
                    bw.Close();
                }
            }
            return true;
        }
    }
}

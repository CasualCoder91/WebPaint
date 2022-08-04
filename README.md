[![NuGet version (Alarich.WebPaint)](https://img.shields.io/nuget/v/Alarich.WebPaint.svg?style=flat-square)](https://www.nuget.org/packages/Alarich.WebPaint/)

# WebPaint

A .NET Framework 4.8 custom server control for image manipulation.

## Current Capabilities

- Load
- Save
- Scale
- Rotate
- Draw rectanges

### Demonstration

![](doc/demonstration.gif)

## Getting Started

1. Download and install the <a href="https://www.nuget.org/packages/Alarich.WebPaint/">NuGet Package</a>.
3. Add the Control to Your website of choice:
   ```aspx
   //...
   <%@ Register Assembly="Alarich.WebPaint" Namespace="Alarich" TagPrefix="AHE" %>
   //...
   <AHE:WebPaint ID="webPaint" runat="server" Width="500" Height="500" Visible="false" Language="EN" />
   ```
3. Add Buttons for loading an image and saving the result:
   ```aspx
   <asp:Button ID="btnLoadImage" runat="server" OnClick="btnLoadImage_Click" Text="Load" />
   <asp:Button ID="btnSaveImage" runat="server" OnClientClick="saveImageData();" OnClick="btnSaveImage_Click" Text="Save" />  
   ```
   ```cs
   protected void btnLoadImage_Click(object sender, EventArgs e)
   {
      webPaint.DisplayImage("https://www.simplilearn.com/ice9/free_resources_article_thumb/what_is_image_Processing.jpg");
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
   ```   
4. In case steps 2-3 fail: Refer to the exaple projects "TestSite" (vb) or "TestSiteCS" (C#).

## Usage

1. Load an image and make the control visible:
   ```cs
   webPaint.DisplayImage("https://www.simplilearn.com/ice9/free_resources_article_thumb/what_is_image_Processing.jpg");
   webPaint.Visible = true;
   ```
2. The user interacts with the control (rotate image, scale image, add rectangles)
3. Save the image:
   The js function `saveImageData()` stores the currently displayed image as a Base64 string into the hidden field "imageData".
   The function `UploadImage(string path)` can be used to store that string as an image on the server.

### Postbacks
If a postback is unavoidable make sure to save the image data first by calling the js function `saveImageData()`, then save the image itself and display it again by calling the method `DisplayImage(string imageURL)`.

### Resolution

The properties `Width` and `Height` refer to the resolution of the displayed image. An uploaded image is automatically scaled if neccesary while keeping its resolution. When using the `trim` tool the given resolution is enforced aswell.

## License

Distributed under the MIT License. See `LICENSE.txt` for more information.

<p align="right">(<a href="#readme-top">back to top</a>)</p>

## Roadmap

- [ ] Handle onmouseout
- [ ] Add Customization options
  - [ ] custom css
  - [ ] disable proportion enforcement
- [ ] test ...

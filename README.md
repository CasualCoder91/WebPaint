[![NuGet version (Alarich.WebPaint)](https://img.shields.io/nuget/v/Alarich.WebPaint.svg?style=flat-square)](https://www.nuget.org/packages/Alarich.WebPaint/)

# WebPaint

A NET Framework 4.8-based Custom ASP.NET Server Control for image manipulation.

## Current Capabilities

- Load
- Save
- Scale
- Rotate
- Draw rectanges
- Draw arrows

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
   ```   
4. In case steps 2-3 fail: Refer to the exaple projects "TestSite" (vb) or "TestSiteCS" (C#).

## Usage

1. Load an image and make the control visible:
   ```cs
   webPaint.DisplayImage("https://picsum.photos/200/300");
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

### Customization

 - Use the Property `CSSLink` to link a custom css file.
   ```ASP.NET
   CSSLink="/CustomWebPaint.css"
   ```
   Refer to [WebPaint.css](/AHELibrary/WebPaint/WebPaint.css) for available options.

 - The property `DefaultColor` can be set to any hex value to change the default color.
   ```html
   DefaultColor="#FFFFFF"
   ```
## License

Distributed under the MIT License. See `LICENSE.txt` for more information.

<p align="right">(<a href="#readme-top">back to top</a>)</p>

## Roadmap

- [x] Handle onmouseout
- [ ] Add Customization options
  - [x] custom css
  - [ ] disable proportion enforcement
- [ ] test ...

## Changelog

### [1.0.0.5] - 2022-10-12

New Action: Draw Arrows.

### [1.0.0.4] - 2022-09-05

New Option: Load Image from Byte Array.

### [1.0.0.3] - 2022-08-30

The HiddenField "ImageData" is now public to allow for custom save methods.

### [1.0.0.2] - 2022-08-06

Properties for default color and custom css file added.

### [1.0.0.1] - 2022-08-05

Rework resulting in better results regarding "rotation" and "undo" functions.

### [1.0.0] - 2022-08-04

Initial Release

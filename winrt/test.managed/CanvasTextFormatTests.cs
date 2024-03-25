using System;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Text;
using Microsoft.UI;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace test.managed;

[TestClass]
public class CanvasTextFormatTests
{
    [TestMethod]
    public void LoadCanvasTextFormatTests()
    {
        var ret = CanvasFontSet.AddFontFileToNewSet("C:\\Users\\gjt\\Downloads\\amatic\\sample.ttf");
        var textFormat = CanvasTextFormat.CreateTextFormatWithCustomFontSet(ret, "Amatic SC");
        textFormat.FontSize = 44;

        CanvasDevice device = CanvasDevice.GetSharedDevice();
        CanvasRenderTarget offscreen = new CanvasRenderTarget(device, 300, 300, 96);

        using (CanvasDrawingSession ds = offscreen.CreateDrawingSession())
        {
            ds.Clear(Colors.Black);
           
            ds.DrawText("Hello, World!", 100, 100, Colors.White, textFormat);
        }
        offscreen.SaveAsync("C:\\\\Users\\\\gjt\\\\Downloads\\\\adine-kirnberg\\\\sample.png", CanvasBitmapFileFormat.Png).GetAwaiter().GetResult();
        Assert.IsNotNull(textFormat);
    }
}
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Graphics.Canvas.Text;

namespace test.managed;

[TestClass]
public class CanvasTextFormatTests
{
    [TestMethod]
    public void LoadCanvasTextFormatTests()
    {
        var ret = CanvasFontSet.AddFontFileToNewSet("C:\\\\Users\\\\gjt\\\\Downloads\\\\adine-kirnberg\\\\sample.ttf");
        var textFormat = CanvasTextFormat.CreateTextFormatWithCustomFontSet(ret);
        var set=textFormat.get_CustomFontSet();
        Assert.IsNotNull(textFormat);
    }
}
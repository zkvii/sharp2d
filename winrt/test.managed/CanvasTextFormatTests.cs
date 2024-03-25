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
        var textFormat = new CanvasTextFormat()
        {
            FontFamily = "Arial",
        };
        Assert.AreEqual("Arial", textFormat.FontFamily);
    }
}
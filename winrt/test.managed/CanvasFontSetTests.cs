using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Graphics.Canvas.Text;

namespace test.managed
{
    [TestClass]
    public class CanvasFontSetTests
    {
        [TestMethod]
        public void LoadFontFileToSet()
        {
            // var ret=CanvasFontSet.AddFontFileToSet("C:\\Users\\gjt\\Downloads\\adine-kirnberg\\sample.ttf");
            var ret = CanvasFontSet.AddFontFileToNewSet("C:\\\\Users\\\\gjt\\\\Downloads\\\\adine-kirnberg\\\\sample.ttf");
           
            ret.Fonts.ToList().ForEach(f => Debug.WriteLine(f.FamilyNames));
            
           var res= CanvasFontSet.AddFontFileToSet("C:\\Users\\gjt\\Downloads\\amatic\\AmaticSC-Regular.ttf", ret);

            ret.Fonts.ToList().ForEach(f => Debug.WriteLine(f.FamilyNames));
            Debug.WriteLine("Font set count: " + ret.Fonts.Count);
        }
    }
}

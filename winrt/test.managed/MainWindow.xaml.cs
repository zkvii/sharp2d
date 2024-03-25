// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.Graphics.Canvas.UI;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.UI;
using System.Diagnostics;
using System.Numerics;
using Microsoft.Graphics.Canvas.Text;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace test.managed
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            // PdfCanvas.CreateResources += PdfCanvas_CreateResources;
            // PdfCanvas.RegionsInvalidated += PdfCanvas_RegionUpdate;
        }

        private void PdfCanvas_RegionUpdate(CanvasVirtualControl sender, CanvasRegionsInvalidatedEventArgs args)
        {
            // foreach (var region in args.InvalidatedRegions)
            // {
            //     var ds = PdfCanvas.CreateDrawingSession(region);
            //
            //     // var page = ViewModel.PdfPageList[i];
            //
            //     // var image = CanvasBitmap.LoadAsync(sender, stream).AsTask().Result;
            //     //  var rect = new Rect(0, ViewModel.PageRanges[i], ViewModel.PdfWidth,
            //     //      ViewModel.PageRanges[i] + ViewModel.PdfHeight);
            //     // var size = new Size(ViewModel.PageWidth, ViewModel.PdfHeight);
            //     // var cl=new WinNoteCpp.Class();
            //
            //     // await FontHelpers.LoadFontAsync();
            //     var canvasSet = CanvasFontSet.AddFontFileToNewSet(@"C:\Users\gjt\Downloads\adine-kirnberg\sample.ttf");
            //
            //     var textFormat = CanvasTextFormat.CreateTextFormatWithCustomFontSet(canvasSet);
            //     textFormat.FontFamily = "Adine Kirnberg";
            //
            //     var cs = textFormat.CustomFontSet;
            //
            //     var canvasTextLayout = new CanvasTextLayout(sender, "Hello", textFormat, 100, 100);
            //     ds.DrawTextLayout(canvasTextLayout, new Vector2(100, 100), Colors.Black);
            //
            //
            //     Debug.WriteLine("fucked");
            //
            //
            //     // WinNoteCore.Helper.showMessage();
            //     ds.Dispose();
            // }
        }

        private void PdfCanvas_CreateResources(CanvasVirtualControl sender, CanvasCreateResourcesEventArgs args)
        {
        }
    }
}
using System;
using System.Numerics;
using Windows.UI;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Graphics.Canvas.Text;
using Microsoft.UI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Threading.Tasks;

namespace test.managed;

[TestClass]
public class CommandListMultiThreadDrawingSession
{
    [TestMethod]
    public void SingleThreadTest()
    {
        CanvasDevice device = CanvasDevice.GetSharedDevice();
        // CanvasRenderTarget offscreen = new CanvasRenderTarget(device, 600,800 , 144);

        var commandList = new CanvasCommandList(device);
        Stopwatch stopwatch = Stopwatch.StartNew();
        for (var i = 0; i < 2000; i++)
        {
           AddPathToCommandList(commandList,false);
        }
        var offscreen = new CanvasRenderTarget(device, 600, 800, 144);
        using CanvasDrawingSession ds = offscreen.CreateDrawingSession();
        ds.Clear(Colors.Black);
        ds.DrawImage(commandList);
        // 计时结束
        stopwatch.Stop();

        // 输出执行时间
        Debug.WriteLine($"################################Execution time: {stopwatch.ElapsedMilliseconds} milliseconds #######################");
    }

    // [TestMethod]
    // public async Task MultiThreadTest()
    // {
    //     CanvasDevice device = CanvasDevice.GetSharedDevice();
    //     // CanvasRenderTarget offscreen = new CanvasRenderTarget(device, 600,800 , 144);
    //
    //     var commandList = new CanvasCommandList(device);
    //     Stopwatch stopwatch = Stopwatch.StartNew();
    //     await Task.WhenAll(
    //         Task.Run(() => AddPathToCommandListAsync(commandList, 0, 50)),
    //         Task.Run(() => AddPathToCommandListAsync(commandList, 50, 100)),
    //         Task.Run(() => AddPathToCommandListAsync(commandList, 100, 150)),
    //         Task.Run(() => AddPathToCommandListAsync(commandList, 150, 200))
    //     );
    //     var offscreen = new CanvasRenderTarget(device, 600, 800, 144);
    //     using CanvasDrawingSession ds = offscreen.CreateDrawingSession();
    //     ds.Clear(Colors.Black);
    //     ds.DrawImage(commandList);
    //     // 计时结束
    //     stopwatch.Stop();
    //
    //     // 输出执行时间
    //     Debug.WriteLine($"################################Execution time: {stopwatch.ElapsedMilliseconds} milliseconds #######################");
    // }
    //
    // private async Task AddPathToCommandListAsync(CanvasCommandList commandList, int start, int end)
    // {
    //     for (int i = start; i < end; i++)
    //     {
    //         await Task.Run(() => AddPathToCommandList(commandList,true));
    //     }
    // }

    public void AddPathToCommandList(CanvasCommandList commandList,bool isMultiThreaded=false)
    {
        using var ds = commandList.CreateDrawingSessionMultiThread(true);
        Random random = new Random();
        CanvasPathBuilder pathBuilder = new CanvasPathBuilder(commandList.Device);

        // 随机生成线条
        for (int i = 0; i < 10; i++)
        {
            CanvasPathBuilder subPathBuilder = new CanvasPathBuilder(commandList.Device);
            float x1 = (float)random.NextDouble() * 100;
            float y1 = (float)random.NextDouble() * 100;
            float x2 = (float)random.NextDouble() * 100;
            float y2 = (float)random.NextDouble() * 100;

            // ds.DrawLine(x1, y1, x2, y2, GetRandomColor(random));
            subPathBuilder.BeginFigure(x1, y1);
            subPathBuilder.AddLine(x2, y2);
            subPathBuilder.EndFigure(CanvasFigureLoop.Closed);
            pathBuilder.AddGeometry(CanvasGeometry.CreatePath(subPathBuilder));
        }

        // 随机生成贝塞尔曲线
        for (int i = 0; i < 5; i++)
        {
            float x1 = (float)random.NextDouble() * 100;
            float y1 = (float)random.NextDouble() * 100;
            float x2 = (float)random.NextDouble() * 100;
            float y2 = (float)random.NextDouble() * 100;
            float x3 = (float)random.NextDouble() * 100;
            float y3 = (float)random.NextDouble() * 100;
            float x4 = (float)random.NextDouble() * 100;
            float y4 = (float)random.NextDouble() * 100;
            CanvasPathBuilder subPathBuilder = new CanvasPathBuilder(commandList.Device);
            subPathBuilder.BeginFigure(x1, y1);
            subPathBuilder.AddCubicBezier(new Vector2(x2, y2), new Vector2(x3, y3), new Vector2(x4, y4));
            subPathBuilder.EndFigure(CanvasFigureLoop.Closed);
            pathBuilder.AddGeometry(CanvasGeometry.CreatePath(subPathBuilder));

        }
        for(int i=0;i < 5; i++)
        {
            float x1 = (float)random.NextDouble() * 100;
            float y1 = (float)random.NextDouble() * 100;
          
            float width = (float)random.NextDouble() * 100;
            float height = (float)random.NextDouble() * 100;

            pathBuilder.AddGeometry(CanvasGeometry.CreateRectangle(commandList.Device, x1, y1, width, height));
        }
        
        ds.DrawGeometry(CanvasGeometry.CreatePath(pathBuilder), GetRandomColor(random), 1);
    }

    // 获取随机颜色
    private Color GetRandomColor(Random random)
    {
        byte r = (byte)random.Next(0, 256);
        byte g = (byte)random.Next(0, 256);
        byte b = (byte)random.Next(0, 256);
        return Color.FromArgb(255, r, g, b);
    }
}
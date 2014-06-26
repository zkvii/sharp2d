// Copyright (c) Microsoft Corporation.  All rights reserved

#include "pch.h"

using namespace Microsoft::Graphics::Canvas;

TEST_CLASS(CanvasStrokeStyleTests)
{
    TEST_METHOD(CanvasStrokeStyleTests_Construction)
    {
        CanvasStrokeStyle^ canvasStrokeStyle = ref new CanvasStrokeStyle();
    }

    TEST_METHOD(CanvasStrokeStyleTests_Interop_FactoryMixing)
    {
        // This should yield an exception on deletion of the drawing session,
        // but right now it does not, seemingly due to a pre-existing issue.
        RunOnUIThread(
            []()
            {
                // Create a drawing session on one factory.
                CanvasDrawingSession^ drawingSession;
                CanvasSolidColorBrush^ brush;
                {
                    auto canvasDevice = ref new CanvasDevice();
                    auto canvasImageSource = ref new CanvasImageSource(
                        canvasDevice,
                        1,
                        1);
                    brush = ref new CanvasSolidColorBrush(canvasDevice, Windows::UI::Colors::Red);
                    drawingSession = canvasImageSource->CreateDrawingSession();
                }

                // ...Create a stroke style on another.
                CanvasStrokeStyle^ strokeStyle;
                {
                    ComPtr<ID2D1Factory1> d2dFactory;
                    ThrowIfFailed(D2D1CreateFactory(D2D1_FACTORY_TYPE_MULTI_THREADED, d2dFactory.GetAddressOf()));
                    ComPtr<ID2D1StrokeStyle1> d2dStrokeStyle;
                    D2D1_STROKE_STYLE_PROPERTIES1 strokeStyleProperties = D2D1::StrokeStyleProperties1();
                    ThrowIfFailed(d2dFactory->CreateStrokeStyle(&strokeStyleProperties, NULL, 0, &d2dStrokeStyle));
                    strokeStyle = GetOrCreate<CanvasStrokeStyle>(d2dStrokeStyle.Get());
                }

                Windows::Foundation::Point point(1, 1);
                drawingSession->DrawLine(point, point, brush, 5, strokeStyle);

                delete drawingSession;
            });
    }

    TEST_METHOD(CanvasStrokeStyleTests_DrawForcesRealization)
    {
        RunOnUIThread(
            []()
            {
                // Create a drawing session on one factory.
                CanvasDrawingSession^ drawingSession;
                CanvasSolidColorBrush^ brush;
                auto canvasDevice = ref new CanvasDevice();
                auto canvasImageSource = ref new CanvasImageSource(
                    canvasDevice,
                    1,
                    1);
                brush = ref new CanvasSolidColorBrush(canvasDevice, Windows::UI::Colors::Red);
                drawingSession = canvasImageSource->CreateDrawingSession();

                CanvasStrokeStyle^ strokeStyle = ref new CanvasStrokeStyle();

                Windows::Foundation::Point point(1, 1);
                drawingSession->DrawLine(point, point, brush, 5, strokeStyle);

                delete drawingSession;
            });
    }

    TEST_METHOD(CanvasStrokeStyleTests_Interop_DefaultDeviceContext)
    {
        CanvasDevice^ canvasDevice = ref new CanvasDevice();
        auto d2dDevice = GetWrappedResource<ID2D1Device1>(canvasDevice);

        ComPtr<ID2D1Factory> d2dFactoryBase;
        d2dDevice->GetFactory(&d2dFactoryBase);

        ComPtr<ID2D1Factory1> d2dFactory;
        ThrowIfFailed(d2dFactoryBase.As(&d2dFactory));

        ComPtr<ID2D1StrokeStyle1> d2dStrokeStyle;
        D2D1_STROKE_STYLE_PROPERTIES1 strokeStyleProperties = D2D1::StrokeStyleProperties1();
        strokeStyleProperties.startCap = D2D1_CAP_STYLE_SQUARE;
        strokeStyleProperties.endCap = D2D1_CAP_STYLE_ROUND;
        strokeStyleProperties.dashCap = D2D1_CAP_STYLE_TRIANGLE;
        strokeStyleProperties.lineJoin = D2D1_LINE_JOIN_BEVEL;
        strokeStyleProperties.miterLimit = 2;
        strokeStyleProperties.dashStyle = D2D1_DASH_STYLE_CUSTOM;
        strokeStyleProperties.dashOffset = 6;
        strokeStyleProperties.transformType = D2D1_STROKE_TRANSFORM_TYPE_HAIRLINE;
        float customDashArray[] = {2, 4, 6, 8};
        ThrowIfFailed(d2dFactory->CreateStrokeStyle(&strokeStyleProperties, customDashArray, 4, &d2dStrokeStyle));

        // Create a wrapper and ensure it has the correct properties.
        auto canvasStrokeStyle = GetOrCreate<CanvasStrokeStyle>(d2dStrokeStyle.Get());

        Assert::AreEqual(CanvasCapStyle::Square, canvasStrokeStyle->StartCap);
        Assert::AreEqual(CanvasCapStyle::Round, canvasStrokeStyle->EndCap);
        Assert::AreEqual(CanvasCapStyle::Triangle, canvasStrokeStyle->DashCap);
        Assert::AreEqual(CanvasLineJoin::Bevel, canvasStrokeStyle->LineJoin);
        Assert::AreEqual(2.0f, canvasStrokeStyle->MiterLimit);

        // This getter won't ever return dash style CUSTOM, even if the wrapped resource had type CUSTOM.
        // Instead, by convention, it will return SOLID.
        Assert::AreEqual(CanvasDashStyle::Solid, canvasStrokeStyle->DashStyle);
        Assert::AreEqual(6.0f, canvasStrokeStyle->DashOffset);
        Assert::AreEqual(CanvasStrokeTransformBehavior::Hairline, canvasStrokeStyle->TransformBehavior);

        auto customDashStyle = canvasStrokeStyle->CustomDashStyle;
        Assert::AreEqual(4u, customDashStyle->Length);
        Assert::AreEqual(2.0f, customDashStyle[0]);
        Assert::AreEqual(4.0f, customDashStyle[1]);
        Assert::AreEqual(6.0f, customDashStyle[2]);
        Assert::AreEqual(8.0f, customDashStyle[3]);
    }

    TEST_METHOD(CanvasStrokeStyleTests_Interop_NativeDeviceContext)
    {
        ComPtr<ID2D1Factory1> d2dFactory;
        ThrowIfFailed(D2D1CreateFactory(D2D1_FACTORY_TYPE_MULTI_THREADED, d2dFactory.GetAddressOf()));

        ComPtr<ID3D11Device> d3dDevice = CreateD3DDevice();
        ComPtr<IDXGIDevice> dxgiDevice;
        ThrowIfFailed(d3dDevice.As(&dxgiDevice));

        ComPtr<ID2D1Device> d2dDevice;
        ThrowIfFailed(d2dFactory->CreateDevice(dxgiDevice.Get(), &d2dDevice));

        ComPtr<ID2D1DeviceContext> d2dDeviceContext;
        d2dDevice->CreateDeviceContext(D2D1_DEVICE_CONTEXT_OPTIONS_NONE, &d2dDeviceContext);
        auto drawingSession = GetOrCreate<CanvasDrawingSession>(d2dDeviceContext.Get());

        ComPtr<ID2D1SolidColorBrush> d2dSolidColorBrush;
        ThrowIfFailed(d2dDeviceContext->CreateSolidColorBrush(D2D1::ColorF(1, 0, 0), &d2dSolidColorBrush));
        auto canvasBrush = GetOrCreate<CanvasSolidColorBrush>(d2dSolidColorBrush.Get());

        ComPtr<ID2D1StrokeStyle1> d2dStrokeStyle;
        D2D1_STROKE_STYLE_PROPERTIES1 strokeStyleProperties = D2D1::StrokeStyleProperties1();
        strokeStyleProperties.startCap = D2D1_CAP_STYLE_SQUARE;
        ThrowIfFailed(d2dFactory->CreateStrokeStyle(&strokeStyleProperties, NULL, 0, &d2dStrokeStyle));
        auto canvasStrokeStyle = GetOrCreate<CanvasStrokeStyle>(d2dStrokeStyle.Get());

        Windows::Foundation::Point point(0, 1);
        drawingSession->DrawLine(point, point, canvasBrush, 5.0f, canvasStrokeStyle);

        delete drawingSession;
    }

    TEST_METHOD(CanvasStrokeStyle_CustomDashStyle)
    {
        //
        // This tests the following edge case:
        // A native stroke style has a non-custom D2D1_DASH_STYLE.
        // But it has a dash array. Stroke style is interopped.
        //
        // The Canvas stroke style should have the non-CUSTOM dash.
        // It should NOT have the dash array.
        //

        ComPtr<ID2D1Factory1> d2dFactory;
        ThrowIfFailed(D2D1CreateFactory(D2D1_FACTORY_TYPE_MULTI_THREADED, d2dFactory.GetAddressOf()));
        ComPtr<ID2D1StrokeStyle1> d2dStrokeStyle;
        D2D1_STROKE_STYLE_PROPERTIES1 strokeStyleProperties = D2D1::StrokeStyleProperties1();
        strokeStyleProperties.dashStyle = D2D1_DASH_STYLE_DASH;

        ThrowIfFailed(d2dFactory->CreateStrokeStyle(&strokeStyleProperties, NULL, 0, &d2dStrokeStyle));

        CanvasStrokeStyle^ strokeStyle = GetOrCreate<CanvasStrokeStyle>(d2dStrokeStyle.Get());
        Assert::AreEqual(CanvasDashStyle::Dash, strokeStyle->DashStyle);
        
        auto dashArray = strokeStyle->CustomDashStyle;
        Assert::AreEqual(0u, dashArray->Length);
    }
};

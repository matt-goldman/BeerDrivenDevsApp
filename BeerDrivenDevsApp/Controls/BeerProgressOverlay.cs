using SkiaSharp;
using SkiaSharp.Extended.UI.Controls;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;

namespace BeerDrivenDevsApp.Controls;

public class BeerProgressOverlayView : SKCanvasView
{
    public static readonly BindableProperty ProgressProperty =
        BindableProperty.Create(nameof(Progress), typeof(double), typeof(BeerProgressOverlayView), 0.0, propertyChanged: OnProgressChanged);

    
    public double Progress
    {
        get => (double)GetValue(ProgressProperty);
        set => SetValue(ProgressProperty, value);
    }

    public event EventHandler? AnimationCompleted;

    private bool _isFoamAnimating = false;

    private SKColor _gradientStart;
    private SKColor _gradientEnd;

    public BeerProgressOverlayView()
    {
        EnableTouchEvents = false;
        PaintSurface += OnPaintSurface;

        // Get Amber400 and Yellow300 static resource colors from the app resources
        var amber400 = (Color)Application.Current!.Resources["Amber400pc15"];
        var yellow300 = (Color)Application.Current!.Resources["Yellow300pc15"];

        _gradientStart = amber400.ToSKColor();
        _gradientEnd = yellow300.ToSKColor();
    }

    private static void OnProgressChanged(BindableObject bindable, object oldVal, object newVal)
    {
        var view = (BeerProgressOverlayView)bindable;
        view.InvalidateSurface();

        if ((double)newVal >= 1.0 && !view._isFoamAnimating)
        {
            view.StartFoamAnimation();
        }
    }

    private void OnPaintSurface(object? sender, SKPaintSurfaceEventArgs e)
    {
        var canvas = e.Surface.Canvas;
        var info = e.Info;
        canvas.Clear();

        var progressY = (float)(info.Height * (1 - Progress));

        // Gray muted overlay
        using var grayPaint = new SKPaint { Color = new SKColor(128, 128, 128, 38) }; // ~15% opacity
        var grayRect = new SKRect(0, 0, info.Width, progressY);
        canvas.DrawRect(grayRect, grayPaint);

        
        // Beer gradient overlay
        var beerRect = new SKRect(0, progressY, info.Width, info.Height);
        using var beerPaint = new SKPaint
        {
            Shader = SKShader.CreateLinearGradient(
                new SKPoint(0, beerRect.Top),
                new SKPoint(0, beerRect.Bottom),
                [_gradientStart, _gradientEnd],
                null,
                SKShaderTileMode.Clamp)
        };
        canvas.DrawRect(beerRect, beerPaint);
    }

    private void StartFoamAnimation()
    {
        _isFoamAnimating = true;
        AnimationCompleted?.Invoke(this, EventArgs.Empty);
    }
}

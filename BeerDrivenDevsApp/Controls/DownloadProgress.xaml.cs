using SkiaSharp.Extended.UI.Controls;

namespace BeerDrivenDevsApp.Controls;

public partial class DownloadProgress : ContentView
{
    private double _viewHeight = 0.0;

    public static readonly BindableProperty ProgressProperty =
        BindableProperty.Create(nameof(Progress), typeof(double), typeof(DownloadProgress), 0.0, propertyChanged: OnProgressChanged);

    public double Progress
    {
        get => (double)GetValue(ProgressProperty);
        set => SetValue(ProgressProperty, value);
    }

    private static void OnProgressChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is DownloadProgress downloadProgress)
        {
            downloadProgress.SetBubblesHeight();
        }
    }

    private SKConfettiSystem _regularBubbleConfettiSystem;
    private SKConfettiSystem _foamConfettiSystem;

    public DownloadProgress()
    {
        InitializeComponent();

        _regularBubbleConfettiSystem = new SKConfettiSystem
        {
            EmitterBounds   = SKConfettiEmitterBounds.Bottom,
            Emitter         = SKConfettiEmitter.Infinite(100, -1),
            Shapes          = [new SKConfettiCircleShape()],
            Colors          = [new Color(255, 255, 255, 60)],
            Lifetime        = 2,
            Physics         = [new SKConfettiPhysics(10, 50), new SKConfettiPhysics(5, 10), new SKConfettiPhysics(2, 20)]
        };

        _foamConfettiSystem = new SKConfettiSystem
        {
            EmitterBounds   = SKConfettiEmitterBounds.Bottom,
            Emitter         = SKConfettiEmitter.Infinite(200, -1),
            Shapes          = [new SKConfettiCircleShape()],
            Colors          = [new Color(255, 255, 255, 180)],
            Lifetime        = 2,
            Physics         = [
                new SKConfettiPhysics(10, 50),
                new SKConfettiPhysics(5, 10),
                new SKConfettiPhysics(2, 20),
                new SKConfettiPhysics(30, 50),
                new SKConfettiPhysics(50, 10),
                new SKConfettiPhysics(20, 20)]
        };

        Confetti.Systems = [_regularBubbleConfettiSystem];
    }

    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);

        if (height > 0)
        {
            _viewHeight = height;
        }
    }

    public void SetBubblesHeight()
    {
        Confetti.MaximumHeightRequest = Progress * _viewHeight;
    }

    private async void BeerProgressOverlayView_AnimationCompleted(object sender, EventArgs e)
    {
        if (Progress >= 1.0)
        {
            Confetti.Systems = [_foamConfettiSystem];
            _foamConfettiSystem.IsAnimationEnabled = true;
            Confetti.IsAnimationEnabled = true;
            await Task.Delay(500); // allow some foam to be visible
            Confetti.FadeTo(0, 500, Easing.CubicInOut);
            BeerOverlay.FadeTo(0, 500, Easing.CubicInOut);

        }
        else
        {
            Confetti.Systems = [_regularBubbleConfettiSystem];
        }
    }
}
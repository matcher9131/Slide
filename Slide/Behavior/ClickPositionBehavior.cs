using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Input;

namespace Slide.Behavior
{
    public enum ClickPosition
    {
        LeftQuarter,
        Middle,
        RightQuarter,
    }

    public class ClickPositionBehavior : Behavior<FrameworkElement>
    {
        private const double Tolerance = 5.0;
        private const double SideRatio = 0.25;

        private Point? downPosition;

        public ICommand Click
        {
            get => (ICommand)GetValue(ClickProperty);
            set => SetValue(ClickProperty, value);
        }

        public static readonly DependencyProperty ClickProperty = DependencyProperty.Register(
            nameof(Click), 
            typeof(ICommand), 
            typeof(ClickPositionBehavior), 
            new FrameworkPropertyMetadata()
        );

        private void OnMouseLeftDown(object sender, MouseButtonEventArgs e)
        {
            this.downPosition = e.GetPosition(AssociatedObject);
        }

        private void OnMouseLeftUp(object sender, MouseButtonEventArgs e)
        {
            if (this.downPosition is not Point downPosition) return;
            var upPosition = e.GetPosition(AssociatedObject);
            double dx = upPosition.X - downPosition.X;
            double dy = upPosition.Y - downPosition.Y;
            if (dx * dx + dy * dy < Tolerance * Tolerance)
            {
                var width = AssociatedObject.ActualWidth;
                var clickPosition = upPosition.X < width * SideRatio ? ClickPosition.LeftQuarter
                    : upPosition.X > width * (1 - SideRatio) ? ClickPosition.RightQuarter
                    : ClickPosition.Middle;
                if (this.Click != null && this.Click.CanExecute(clickPosition))
                {
                    this.Click.Execute(clickPosition);
                }
            }
            this.downPosition = null;
        }

        private void OnMouseLeave(object sender, MouseEventArgs e)
        {
            this.downPosition = null;
        }

        protected override void OnAttached()
        {
            AssociatedObject.MouseLeftButtonDown += this.OnMouseLeftDown;
            AssociatedObject.MouseLeftButtonUp += this.OnMouseLeftUp;
            AssociatedObject.MouseLeave += this.OnMouseLeave;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.MouseLeftButtonDown -= this.OnMouseLeftDown;
            AssociatedObject.MouseLeftButtonUp -= this.OnMouseLeftUp;
            AssociatedObject.MouseLeave -= this.OnMouseLeave;
        }
    }
}

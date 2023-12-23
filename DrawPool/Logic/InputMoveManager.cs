using DrawPool.Properties;
using Hearthstone_Deck_Tracker;
using System;
using System.Windows;
using System.Windows.Controls;
using Core = Hearthstone_Deck_Tracker.API.Core;

namespace DrawPool.Logic
{
    public class InputMoveManager
    {
        private User32.MouseInput _mouseInput;
        private bool _selected = false;
        private StackPanel _StackPanel;

        public InputMoveManager(StackPanel player)
        {
            _StackPanel = player;
        }

        private void MouseInputOnLmbDown(object sender, EventArgs eventArgs)
        {
            var pos = User32.GetMousePos();
            var _mousePos = new Point(pos.X, pos.Y);
            if (PointInsideControl(_mousePos, _StackPanel))
            {
                _selected = true;
            }
            else
            {
                _selected = false;
            }
        }

        private void MouseInputOnLmbUp(object sender, EventArgs eventArgs)
        {
            var pos = User32.GetMousePos();

            var _mousePos = new Point(pos.X, pos.Y);
            if (_selected)
            {
                var p = Core.OverlayCanvas.PointFromScreen(new Point(pos.X, pos.Y));

                Settings.Default.DrawPoolTop = p.Y;
                Settings.Default.DrawPoolLeft = p.X;
            }

            _selected = false;
        }

        private void MouseInputOnMouseMoved(object sender, EventArgs eventArgs)
        {
            if (_selected == false)
            {
                return;
            }

            var pos = User32.GetMousePos();
            var p = Core.OverlayCanvas.PointFromScreen(new Point(pos.X, pos.Y));

            Canvas.SetTop(_StackPanel, p.Y);
            Canvas.SetLeft(_StackPanel, p.X);
        }

        private bool PointInsideControl(Point p, FrameworkElement control)
        {
            var pos = control.PointFromScreen(p);
            return pos.X > 0 && pos.X < control.ActualWidth && pos.Y > 0 && pos.Y < control.ActualHeight;
        }

        public void Dispose()
        {
            _mouseInput?.Dispose();
            _mouseInput = null;
        }

        public bool Toggle()
        {
            if (Hearthstone_Deck_Tracker.Core.Game.IsRunning && _mouseInput == null)
            {
                _mouseInput = new User32.MouseInput();
                _mouseInput.LmbDown += MouseInputOnLmbDown;
                _mouseInput.LmbUp += MouseInputOnLmbUp;
                _mouseInput.MouseMoved += MouseInputOnMouseMoved;
                return true;
            }
            Dispose();
            return false;
        }
    }
}
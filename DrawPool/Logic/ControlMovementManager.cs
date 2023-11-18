using DrawPool.Controls;
using DrawPool.DrawLogic;
using DrawPool.Properties;
using Hearthstone_Deck_Tracker;
using System;
using System.Windows;
using Core = Hearthstone_Deck_Tracker.API.Core;

namespace DrawPool.Logic
{
    public class ControlMovementManager
    {
        private readonly PoolView _PoolView;
        private User32.MouseInput _mouseInput;
        private bool _selected;
        private MinstrelPool poolView;
        private double xMouseDeltaFromCard;
        private double yMouseDeltaFromCard;

        public ControlMovementManager(PoolView poolView, bool isUnlocked)
        {
            _PoolView = poolView;
            IsUnlocked = isUnlocked;
            if (IsUnlocked)
            {
                ToggleLockState();
            }
        }

        public bool IsUnlocked { get; private set; }

        private void MouseInputOnLmbDown(object sender, EventArgs eventArgs)
        {
            var pos = User32.GetMousePos();
            var _mousePos = new Point(pos.X, pos.Y);
            if (PointInsideControl(_mousePos, _PoolView))
            {
                _selected = true;
            }
        }

        private void MouseInputOnLmbUp(object sender, EventArgs eventArgs)
        {
            _selected = false;
        }

        private void MouseInputOnMouseMoved(object sender, EventArgs eventArgs)
        {
            if (!_selected)
            {
                return;
            }

            var pos = User32.GetMousePos();
            double mouseVerticalPositionAdjust = (yMouseDeltaFromCard * Settings.Default.DrawPoolScale / 100);
            double mouseHorizontalPositionAdjust = (xMouseDeltaFromCard * Settings.Default.DrawPoolScale / 100);
            var p = Core.OverlayCanvas.PointFromScreen(new Point(pos.X - mouseHorizontalPositionAdjust, pos.Y - mouseVerticalPositionAdjust));

            Settings.Default.DrawPoolTop = p.Y;
            Settings.Default.DrawPoolLeft = p.X;
        }

        private bool PointInsideControl(Point p, FrameworkElement control)
        {
            var pos = control.PointFromScreen(p);
            xMouseDeltaFromCard = pos.X;
            yMouseDeltaFromCard = pos.Y;

            return pos.X > 0 && pos.X < control.ActualWidth && pos.Y > 0 && pos.Y > control.ActualHeight;
        }

        public void Dispose()
        {
            _mouseInput?.Dispose();
            _mouseInput = null;
            _selected = false;
        }

        public bool isPositionLocked()
        {
            return _mouseInput == null;
        }

        public bool ToggleLockState()
        {
            if (Hearthstone_Deck_Tracker.Core.Game.IsRunning && _mouseInput == null)
            {
                _mouseInput = new User32.MouseInput();
                _mouseInput.LmbDown += MouseInputOnLmbDown;
                _mouseInput.LmbUp += MouseInputOnLmbUp;
                _mouseInput.MouseMoved += MouseInputOnMouseMoved;
                IsUnlocked = true;
            }
            else
            {
                Dispose();
                IsUnlocked = false;
            }

            return IsUnlocked;
        }
    }
}
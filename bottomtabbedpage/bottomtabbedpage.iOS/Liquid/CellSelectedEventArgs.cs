using System;

namespace bottomtabbedpage.iOS.Liquid
{
    public class CellSelectedEventArgs : EventArgs
    {
        public LiquidFloatingCell Cell { get; set; }

        public int Index { get; set; }

        public CellSelectedEventArgs(LiquidFloatingCell cell, int index)
        {
            Cell = cell;
            Index = index;
        }
    }

    public class FloatingMenuOpenEventArgs : EventArgs
    {
        public bool IsOpen { get; }

        public FloatingMenuOpenEventArgs(bool isOpen)
        {
            IsOpen = isOpen;
        }
    }

    public enum AnimateStyle
    {
        Up,
        Right,
        Left,
        Down
    }
}
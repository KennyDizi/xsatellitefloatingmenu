// Decompiled with JetBrains decompiler
// Type: SatelliteMenu.SatelliteMenuButtonItem
// Assembly: SatelliteMenu, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 193A7BB1-5276-4CC6-8511-0A490974AD2E
// Assembly location: C:\Users\trungdc\Desktop\satellite-menu-1.2.1.0\lib\android\SatelliteMenu.dll

using System;
using Android.Graphics.Drawables;
using Android.Widget;

namespace SatelliteMenu
{
    /// <summary>
    /// Popout menu button item definition, used to initiate the menu button.
    /// </summary>
    public class SatelliteMenuButtonItem
    {
        public int Tag { get; set; }

        public int ImgResourceId { get; set; }

        public Drawable ImgDrawable { get; set; }

        internal ImageView View { get; set; }

        internal ImageView CloneView { get; set; }

        internal Android.Views.Animations.Animation OutAnimation { get; set; }

        internal Android.Views.Animations.Animation InAnimation { get; set; }

        internal Android.Views.Animations.Animation ClickAnimation { get; set; }

        internal int FinalX { get; set; }

        internal int FinalY { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:SatelliteMenu.SatelliteMenuButtonItem" /> class.
        /// </summary>
        /// <param name="tag">Identifier.</param>
        /// <param name="imgResourceId">Image resource identifier.</param>
        public SatelliteMenuButtonItem(int tag, int imgResourceId)
        {
            this.ImgResourceId = imgResourceId;
            this.Tag = tag;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:SatelliteMenu.SatelliteMenuButtonItem" /> class.
        /// </summary>
        /// <param name="tag">Identifier.</param>
        /// <param name="imgDrawable">Image drawable.</param>
        public SatelliteMenuButtonItem(int tag, Drawable imgDrawable)
        {
            this.ImgDrawable = imgDrawable;
            this.Tag = tag;
        }
    }

    public class SatelliteMenuItemEventArgs : EventArgs
    {
        public SatelliteMenuButtonItem MenuItem { get; set; }
    }
}

using System;
using Android.Graphics.Drawables;
using Android.Widget;

namespace bottomtabbedpage.Droid.Satellite
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
        /// Initializes a new instance of the <see cref="T:bottomtabbedpage.Droid.Satellite.SatelliteMenuButtonItem" /> class.
        /// </summary>
        /// <param name="tag">Identifier.</param>
        /// <param name="imgResourceId">Image resource identifier.</param>
        public SatelliteMenuButtonItem(int tag, int imgResourceId)
        {
            ImgResourceId = imgResourceId;
            Tag = tag;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:bottomtabbedpage.Droid.Satellite.SatelliteMenuButtonItem" /> class.
        /// </summary>
        /// <param name="tag">Identifier.</param>
        /// <param name="imgDrawable">Image drawable.</param>
        public SatelliteMenuButtonItem(int tag, Drawable imgDrawable)
        {
            ImgDrawable = imgDrawable;
            Tag = tag;
        }
    }

    public class SatelliteMenuItemEventArgs : EventArgs
    {
        public SatelliteMenuButtonItem MenuItem { get; set; }
    }
}

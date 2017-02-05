using System;
using UIKit;

namespace bottomtabbedpage.iOS.Satellite
{
    public class SatelliteMenuButtonItem
    {
        private UIImage _itemImage;

        public UIImage ItemImage
        {
            get { return _itemImage; }
            set
            {
                _itemImage = value;
                ImageChanged?.Invoke();
            }
        }

        internal Action ImageChanged { get; set; }

        public int Tag { get; set; }

        public string Name { get; set; }

        public event EventHandler Click;

        public SatelliteMenuButtonItem(UIImage itemImage)
        {
            ItemImage = itemImage;
        }

        public SatelliteMenuButtonItem(UIImage itemImage, int tag)
        {
            ItemImage = itemImage;
            Tag = tag;
        }

        public SatelliteMenuButtonItem(UIImage itemImage, string name)
        {
            ItemImage = itemImage;
            Name = name;
        }

        public SatelliteMenuButtonItem(UIImage itemImage, int tag, string name)
        {
            ItemImage = itemImage;
            Tag = tag;
            Name = name;
        }

        internal void FireClick(object sender, EventArgs args)
        {
            var click = Click;
            click?.Invoke(sender, args);
        }
    }
}
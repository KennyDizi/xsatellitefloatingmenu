using System.Drawing;
using System.Linq;
using bottomtabbedpage.iOS.Satellite;
using bottomtabbedpage.SourceCode;
using CoreGraphics;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(XSatelliteFloatingMenu), typeof(XSatelliteFloatingMenuRenderer))]

namespace bottomtabbedpage.iOS.Satellite
{
    public class XSatelliteFloatingMenuRenderer : ViewRenderer<XSatelliteFloatingMenu, SatelliteMenuButton>
    {
        private SatelliteMenuButton _menuFab;
        const int BUTTON_SIZE = 44;
        const int MARGIN = 10;

        protected override void OnElementChanged(ElementChangedEventArgs<XSatelliteFloatingMenu> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                //init navcontrol
                var bounds = UIScreen.MainScreen.Bounds;
                var width = bounds.Width;
                var height = bounds.Height;
                var scale = UIScreen.MainScreen.Scale;

                var frame = new CGRect(width - 100 / scale, height - 100 / scale, 56 / scale, 56 / scale);
                var buttons =
                    Element.Children.Select(
                        x => new SatelliteMenuButtonItem(itemImage: UIImage.FromBundle(x.ImageName), tag: x.ClickId));
                // create the menu button
                var menu = new SatelliteMenuButton(Control.Superview, UIImage.FromBundle("menu.png"), frame);
                menu.AddItems(buttons);
                _menuFab = menu;
                
                SetNativeControl(_menuFab);
            }

            if (e.NewElement != null)
            {
                //register handler
                // register for the menu item selection event here
                _menuFab.MenuItemClick += OnItemClick;
            }

            if (e.OldElement != null)
            {
                //unregister handler
                // unregister for the menu item selection event here
                _menuFab.MenuItemClick -= OnItemClick;
            }
        }

        private void OnItemClick(object sender, SatelliteMenuItemEventArgs eventArgs)
        {
            var tag = eventArgs.MenuItem.Tag;
            Element.RaiseSelectIndexChanged(tag);
        }
    }
}
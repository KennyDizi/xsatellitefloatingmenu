using System;
using bottomtabbedpage.iOS;
using bottomtabbedpage.SourceCode;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(XBottomTabbedPage), typeof(BottomBarPageRenderer))]

namespace bottomtabbedpage.iOS
{
    public class BottomBarPageRenderer : TabbedRenderer
    {
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
            {
                Tabbed.CurrentPageChanged += CurrentPageChanged;
            }

            if (e.OldElement != null)
            {
                Tabbed.CurrentPageChanged -= CurrentPageChanged;
            }
        }

        private void CurrentPageChanged(object sender, EventArgs e)
        {
            //every page change
            var page = Tabbed.CurrentPage;
            var tabColor = page.GetTabColor();
            if (tabColor != null)
            {
                TabBar.BarTintColor = ((Color)tabColor).ToUIColor();
                var navPage = (NavigationPage)page;
                if (navPage != null)
                {
                    navPage.BarBackgroundColor = (Color)tabColor;
                }
                //change app theme color
               //you can change your app theme here
            }
        }

        public override void ViewWillAppear(bool animated)
        {
            if (TabBar?.Items == null) return;
            
            if (Tabbed != null)
            {
                #region for tab color

                //first page change
                var page = Tabbed.CurrentPage;
                var tabColor = page.GetTabColor();
                if (tabColor != null)
                {
                    TabBar.BarTintColor = ((Color)tabColor).ToUIColor();
                    var navPage = (NavigationPage) page;
                    if (navPage != null)
                    {
                        navPage.BarBackgroundColor = (Color) tabColor;
                    }

                    //change app theme color
                    //you can change your app theme here
                }

                #endregion

                #region for selected image

                for (var i = 0; i < TabBar.Items.Length; i++)
                {
                    UpdateItem(TabBar.Items[i], Tabbed.Children[i].Icon);
                }

                #endregion
            }

            base.ViewWillAppear(animated);
        }

        private static void UpdateItem(UITabBarItem item, string icon)
        {
            if (item == null) return;
            try
            {
                icon = icon.Replace(".png", "_selected.png");
                if (item.SelectedImage?.AccessibilityIdentifier == icon) return;
                item.SelectedImage = UIImage.FromBundle(icon);
                item.SelectedImage.AccessibilityIdentifier = icon;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Unable to set selected icon: " + ex.Message);
            }
        }
    }
}
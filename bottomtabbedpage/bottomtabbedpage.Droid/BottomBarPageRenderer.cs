using System.Collections.Generic;
using System.Linq;
using Android.Content.Res;
using Android.Views;
using bottomtabbedpage.Droid;
using bottomtabbedpage.SourceCode;
using BottomNavigationBar;
using BottomNavigationBar.Listeners;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(XBottomTabbedPage), typeof(BottomBarPageRenderer))]

namespace bottomtabbedpage.Droid
{
    internal class BottomBarPageRenderer : VisualElementRenderer<XBottomTabbedPage>, IOnTabClickListener
    {
        private BottomBar _bottomBar;

        private Page _currentPage;

        private int _lastSelectedTabIndex = -1;

        public BottomBarPageRenderer()
        {
            // Required to say packager to not to add child pages automatically
            AutoPackage = false;
        }

        public void OnTabSelected(int position)
        {
            LoadPageContent(position);
        }

        public void OnTabReSelected(int position)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<XBottomTabbedPage> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                ClearElement();
            }

            if (e.NewElement != null)
            {
                InitializeElement(e.NewElement);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ClearElement();
            }

            base.Dispose(disposing);
        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            if (Element == null)
            {
                return;
            }

            var width = r - l;
            var height = b - t;

            _bottomBar.Measure(
                MeasureSpec.MakeMeasureSpec(width, MeasureSpecMode.Exactly),
                MeasureSpec.MakeMeasureSpec(height, MeasureSpecMode.AtMost));

            // We need to call measure one more time with measured sizes 
            // in order to layout the bottom bar properly
            _bottomBar.Measure(
                MeasureSpec.MakeMeasureSpec(width, MeasureSpecMode.Exactly),
                MeasureSpec.MakeMeasureSpec(_bottomBar.ItemContainer.MeasuredHeight, MeasureSpecMode.Exactly));

            var barHeight = _bottomBar.ItemContainer.MeasuredHeight;
            _bottomBar.Layout(0, height - barHeight, width, b);

            var density = Resources.System.DisplayMetrics.Density;

            double contentWidthConstraint = width / density;
            double contentHeightConstraint = (height - barHeight) / density;

            if (_currentPage != null)
            {
                var renderer = Platform.GetRenderer(_currentPage);

                renderer.Element.Measure(contentWidthConstraint, contentHeightConstraint);
                renderer.Element.Layout(new Rectangle(0, 0, contentWidthConstraint, contentHeightConstraint));

                renderer.UpdateLayout();
            }
        }

        private void InitializeElement(XBottomTabbedPage element)
        {
            PopulateChildren(element);
        }

        private void PopulateChildren(XBottomTabbedPage element)
        {
            // Unfortunately bottom bar can not be reused so we have to
            // remove it and create the new instance
            _bottomBar?.RemoveFromParent();
            _bottomBar = CreateBottomBar(element.Children);

            //set color
            SetTabColors();
            UpdateBarBackgroundColor();
            UpdateBarTextColor();
            //end set color

            AddView(_bottomBar);
            LoadPageContent(0);
        }

        private void ClearElement()
        {
            if (_currentPage != null)
            {
                var renderer = Platform.GetRenderer(_currentPage);

                if (renderer != null)
                {
                    renderer.ViewGroup.RemoveFromParent();
                    renderer.ViewGroup.Dispose();
                    renderer.Dispose();

                    _currentPage = null;
                }

                if (_bottomBar != null)
                {
                    _bottomBar.RemoveFromParent();
                    _bottomBar.Dispose();
                    _bottomBar = null;
                }
            }
        }

        private BottomBar CreateBottomBar(IEnumerable<Page> pageIntents)
        {
            var bar = new BottomBar(Context);
            var bottomBarPage = Element;
            // TODO: Configure the bottom bar here according to your needs
            bar.NoTabletGoodness();
            if (bottomBarPage.FixedMode)
            {
                bar.UseFixedMode();
            }
            
            bar.SetOnTabClickListener(this);
            PopulateBottomBarItems(bar, pageIntents);

            return bar;
        }

        private void UpdateBarBackgroundColor()
        {
            var bgColor = Element.BarBackgroundColor.ToAndroid();
            _bottomBar?.SetBackgroundColor(bgColor);
        }

        private void UpdateBarTextColor()
        {
            var activeColor = Element.BarTextColor.ToAndroid();
            _bottomBar?.SetActiveTabColor(activeColor);
            // The problem SetActiveTabColor does only work in fiexed mode // haven't found yet how to set text color for tab items on_bottomBar, doesn't seem to have a direct way
        }

        private void SetTabColors()
        {
            for (var i = 0; i < Element.Children.Count; ++i)
            {
                var page = Element.Children[i];
                var tabColor = page.GetTabColor();
                if (tabColor != null)
                {
                    _bottomBar?.MapColorForTab(i, tabColor.Value.ToAndroid());
                }
            }
        }

        private void PopulateBottomBarItems(BottomBar bar, IEnumerable<Page> pages)
        {
            var barItems = pages.Select(x => new BottomBarTab(Context.Resources.GetDrawable(x.Icon), x.Title));
            bar.SetItems(barItems.ToArray());
        }

        private void LoadPageContent(int position)
        {
            ShowPage(position);
        }

        private void ShowPage(int position)
        {
            if (position != _lastSelectedTabIndex)
            {
                Element.CurrentPage = Element.Children[position];
                if (Element.CurrentPage != null)
                {
                    LoadPageContent(Element.CurrentPage);
                }
            }

            _lastSelectedTabIndex = position;
        }

        private void LoadPageContent(Page page)
        {
            UnloadCurrentPage();
            _currentPage = page;
            LoadCurrentPage();
            Element.CurrentPage = _currentPage;
        }

        private void LoadCurrentPage()
        {
            var renderer = Platform.GetRenderer(_currentPage);
            if (renderer == null)
            {
                renderer = Platform.CreateRenderer(_currentPage);
                Platform.SetRenderer(_currentPage, renderer);
                AddView(renderer.ViewGroup);
            }
            else
            {
                // As we show and hide pages manually OnAppearing and OnDisappearing
                // workflow methods won't be called by the framework. Calling them manually...
                var basePage = _currentPage as XNativeBasePageView;
                basePage?.SendAppearing();
            }
            renderer.ViewGroup.Visibility = ViewStates.Visible;

            #region change theme

            var tabColor = _currentPage.GetTabColor();
            if (tabColor != null)
            {
                //change app theme color
                //you can change your theme here
            }

            #endregion
        }

        private void UnloadCurrentPage()
        {
            if (_currentPage != null)
            {
                var basePage = _currentPage as XNativeBasePageView;
                basePage?.SendDisappearing();
                var renderer = Platform.GetRenderer(_currentPage);
                if (renderer != null)
                {
                    renderer.ViewGroup.Visibility = ViewStates.Invisible;
                }
            }
        }
    }
}
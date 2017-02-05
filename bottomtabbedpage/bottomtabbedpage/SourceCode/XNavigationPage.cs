using Xamarin.Forms;

namespace bottomtabbedpage.SourceCode
{
    public class XNavigationPage : NavigationPage
    {
        public XNavigationPage(Page root) : base(root)
        {
            Init();
            Title = root.Title;
            Icon = root.Icon;
        }

        public XNavigationPage()
        {
            Init();
        }

        private void Init()
        {
            BarTextColor = Color.White;
        }
    }
}
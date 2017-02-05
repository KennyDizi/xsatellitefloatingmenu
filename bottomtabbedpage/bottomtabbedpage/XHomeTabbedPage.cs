using bottomtabbedpage.SourceCode;
using Xamarin.Forms;

namespace bottomtabbedpage
{
    public class XHomeTabbedPage : XBottomTabbedPage
    {
        public XHomeTabbedPage()
        {
            var viewRootPage1 = new Page1(cityName:"HaNoi - VietNam", image:"", icon: Device.OnPlatform(iOS: "tab_about.png", Android: "menu_hacks.png", WinPhone: ""));
            var navPage1 = new XNavigationPage(root: viewRootPage1) {BarTextColor = Color.White};
            // set tab color
            navPage1.SetTabColor(Color.FromHex("#E11657"));
            Children.Add(navPage1);

            var viewRootPage2 = new Page1(cityName: "HaiPhong - VietNam", image: "", icon: Device.OnPlatform(iOS: "tab_feed.png", Android: "menu_info.png", WinPhone: ""));
            var navPage2 = new XNavigationPage(root: viewRootPage2) { BarTextColor = Color.White };
            // set tab color
            navPage2.SetTabColor(Color.FromHex("#7B1FA2"));
            Children.Add(navPage2);

            var viewRootPage3 = new Page1(cityName: "HoChiMinh - VietNam", image: "", icon: Device.OnPlatform(iOS: "tab_minihacks.png", Android: "menu_sessions.png", WinPhone: ""));
            var navPage3 = new XNavigationPage(root: viewRootPage3) { BarTextColor = Color.White };
            // set tab color
            navPage1.SetTabColor(Color.FromHex("#FF9800"));
            Children.Add(navPage3);

            var viewRootPage4 = new Page1(cityName: "DaNang - VietNam", image: "", icon: Device.OnPlatform(iOS: "tab_sessions.png", Android: "menu_settings.png", WinPhone: ""));
            var navPage4 = new XNavigationPage(root: viewRootPage4) { BarTextColor = Color.White };
            // set tab color
            navPage4.SetTabColor(Color.FromHex("#FF5252"));
            Children.Add(navPage4);

            #region them and color

            BarBackgroundColor = Color.FromHex("#f3f3f3");
            BarTextColor = Color.Blue;

            #endregion
        }
    }
}
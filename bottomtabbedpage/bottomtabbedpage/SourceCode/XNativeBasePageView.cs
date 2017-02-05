using Xamarin.Forms;

namespace bottomtabbedpage.SourceCode
{
    public class XNativeBasePageView : ContentPage
    {
        #region for bottom bar page

        public void SendAppearing()
        {
            OnAppearing();
        }

        public void SendDisappearing()
        {
            OnDisappearing();
        }

        #endregion
    }
}

namespace bottomtabbedpage
{
    public partial class Page1
    {
        public Page1(string cityName, string image, string icon)
        {
            InitializeComponent();
            Title = cityName;
            Icon = icon;
            ImageCity.Source = image;
        }
    }
}
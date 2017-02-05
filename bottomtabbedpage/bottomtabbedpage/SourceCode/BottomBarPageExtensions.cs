using Xamarin.Forms;

namespace bottomtabbedpage.SourceCode
{
	public static class BottomBarPageExtensions
	{
		#region TabColorProperty

		public static readonly BindableProperty TabColorProperty = BindableProperty.CreateAttached (
				propertyName: "TabColor",
				returnType: typeof (Color?),
				declaringType: typeof (Page),
				defaultValue: null);

		public static void SetTabColor (this Page page, Color? color)
		{
			page.SetValue (TabColorProperty, color);
		}

		public static Color? GetTabColor (this Page page)
		{
			return (Color?)page.GetValue (TabColorProperty);
		}

		#endregion
	}
}
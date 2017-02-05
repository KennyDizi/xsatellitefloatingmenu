using Xamarin.Forms;

namespace bottomtabbedpage.SourceCode
{
	public class XBottomTabbedPage : TabbedPage
	{
		public bool FixedMode { get; set; }

		public void RaiseCurrentPageChanged()
		{
			OnCurrentPageChanged();
		}
	}
}
using Xamanimation;
using Xamarin.Forms.Xaml;

namespace AppTFG.Walkthrough
{
	public partial class BikingHardView : IAnimatedView
	{
		public BikingHardView ()
		{
			InitializeComponent ();
		}

		public void StartAnimation()
		{
			if (Resources["InfoPanelAnimation"] is StoryBoard animation)
			{
				animation.Begin();
			}
		}
	}
}
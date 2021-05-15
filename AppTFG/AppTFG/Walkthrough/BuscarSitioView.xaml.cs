using Xamanimation;
using Xamarin.Forms.Xaml;

namespace AppTFG.Walkthrough
{
	public partial class BuscarSitioView : IAnimatedView
	{
		public BuscarSitioView()
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
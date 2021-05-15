using Xamanimation;

namespace AppTFG.Walkthrough
{
	public partial class PaseoView : IAnimatedView
	{
		public PaseoView()
		{
			InitializeComponent();
		}

		public void StartAnimation()
		{
			if (Resources["BackgroundColorAnimation"] is ColorAnimation backgroundColorAnimation)
			{
				backgroundColorAnimation.Begin();
			}

			if (Resources["InfoPanelAnimation"] is StoryBoard infoPanelAnimation)
			{
				infoPanelAnimation.Begin();
			}
		}
	}
}
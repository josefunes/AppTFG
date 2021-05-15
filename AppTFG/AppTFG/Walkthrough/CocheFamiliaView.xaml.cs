using Xamanimation;

namespace AppTFG.Walkthrough
{
	public partial class CocheFamiliaView : IAnimatedView
	{
		public CocheFamiliaView()
		{
			InitializeComponent ();
		}

		public void StartAnimation()
		{
			if (Resources["BackgroundColorAnimation"] is ColorAnimation backgroundColorAnimation)
			{
				backgroundColorAnimation.Begin();
			}

			if (Resources["InfoPanelAnimation"] is StoryBoard animation)
			{
				animation.Begin();
			}
		}
	}
}
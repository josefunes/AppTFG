using AppTFG.Modelos;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppTFG.Walkthrough
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class WalkthroughView : ContentPage
	{
		private View[] _views;
		private Saltar Saltar;
		public WalkthroughView()
		{
			InitializeComponent();
			_views = new View[]
			{
				new BuscarSitioView(),
				new PaseoView(),
				new CocheFamiliaView()
			};

			Carousel.ItemsSource = _views;
		}

		private void OnCarouselPositionSelected(object sender, CarouselView.FormsPlugin.Abstractions.PositionSelectedEventArgs e)
		{
			var currentView = _views[e.NewValue];

			if (currentView is IAnimatedView animatedView)
			{
				animatedView.StartAnimation();
			}
		}

		async void SaltarClick(object sender, EventArgs e)
		{
			Saltar = new Saltar();
			await App.Database.SaveSaltarAsync(Saltar);
			Application.Current.MainPage = new AppShell();
		}
	}
}
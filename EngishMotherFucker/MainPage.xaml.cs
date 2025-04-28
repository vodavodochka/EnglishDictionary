#if ANDROID
using Android.Views;
using static Android.Provider.UserDictionary;
#endif

namespace EngishMotherFucker
{
    public partial class MainPage : ContentPage
    {
        private MainPageViewModel ViewModel => BindingContext as MainPageViewModel;
        public MainPage()
        {
            InitializeComponent();
            ViewModel.RequestAddWordPage += OnRequestAddWordPage;
#if ANDROID
            var window = Microsoft.Maui.ApplicationModel.Platform.CurrentActivity?.Window;
            if (window != null)
            {
                window.SetStatusBarColor(Android.Graphics.Color.ParseColor("#1f1f1f"));
            }
#endif
        }
        private async void OnRequestAddWordPage()
        {
            await Navigation.PushAsync(new AddWordPage());
        }
    }
}

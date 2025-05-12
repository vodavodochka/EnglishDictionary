#if ANDROID
using Android.Views;
using static Android.Provider.UserDictionary;
#endif

using EngishMotherFucker.Models;
using EngishMotherFucker.ViewModels;

namespace EngishMotherFucker
{
    public partial class MainPage : ContentPage
    {
        private MainPageViewModel ViewModel => BindingContext as MainPageViewModel;
        public MainPage()
        {
            InitializeComponent();
            ViewModel.RequestAddWordPage += OnRequestAddWordPage;
            ViewModel.RequestStartTrainer += OnStartTrainer;
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

        private async void OnStartTrainer()
        {
            await Navigation.PushAsync(new TrainerStartPage());
        }

        private async void OnWordSelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is WordModel selectedWord)
            {
                // Переход на страницу редактирования
                await Navigation.PushAsync(new EditWordPage(selectedWord, ViewModel));
            }

            ((CollectionView)sender).SelectedItem = null;
        }

        private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            // Всё обрабатывается в ViewModel-и
        }

    }
}

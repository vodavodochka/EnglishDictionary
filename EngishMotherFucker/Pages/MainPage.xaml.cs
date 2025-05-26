#if ANDROID
using Android.Views;
using static Android.Provider.UserDictionary;
#endif

using CommunityToolkit.Mvvm.Messaging;
using EngishMotherFucker.Models;
using EngishMotherFucker.Pages;
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
            ViewModel.RequestOpenSettings += OnOpenSettings;
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
            await Navigation.PushAsync(new AddWordPage(ViewModel));
        }


        private async void OnStartTrainer()
        {
            await Navigation.PushAsync(new TrainerStartPage());
        }

        private async void OnOpenSettings()
        {
            await Navigation.PushAsync(new SettingsPage());
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

        //protected override void OnDisappearing()
        //{
        //    base.OnDisappearing();
        //    if (BindingContext is MainPageViewModel vm)
        //        WeakReferenceMessenger.Default.UnregisterAll(vm);
        //}

    }
}

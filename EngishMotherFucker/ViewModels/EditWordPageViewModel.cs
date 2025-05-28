using System.Windows.Input;
using EngishMotherFucker.Models;

namespace EngishMotherFucker.ViewModels
{
    public class EditWordPageViewModel : BaseViewModel
    {
        private WordModel _word;
        private readonly MainPageViewModel _mainPageViewModel;

        public WordModel Word
        {
            get => _word;
            set => SetProperty(ref _word, value);
        }

        public ICommand DeleteCommand { get; }
        public ICommand SaveCommand { get; }

        public EditWordPageViewModel(WordModel word, MainPageViewModel mainPageViewModel)
        {
            Word = word;
            _mainPageViewModel = mainPageViewModel;

            SaveCommand = new Command(async () => await OnSaveAsync());
            DeleteCommand = new Command(async () => await OnDeleteAsync());
        }

        private async Task OnSaveAsync()
        {
            await App.Database.SaveWordAsync(Word);
            await MainPageViewModel.Instance?.ReloadWordsFromDatabaseAsync();
            await Application.Current.MainPage.Navigation.PopAsync();
        }


        private async Task OnDeleteAsync()
        {
            var itemToRemove = _mainPageViewModel.Words.FirstOrDefault(w => w.Id == Word.Id);
            if (itemToRemove != null)
            {
                _mainPageViewModel.Words.Remove(itemToRemove);
                await App.Database.DeleteWordAsync(Word);
                _mainPageViewModel.ApplyFilter();
            }

            await Application.Current.MainPage.Navigation.PopAsync();
        }
    }
}
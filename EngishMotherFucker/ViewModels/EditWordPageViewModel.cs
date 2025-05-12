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

            SaveCommand = new Command(OnSave);
            DeleteCommand = new Command(OnDelete);
        }

        private void OnSave()
        {
            var index = _mainPageViewModel.Words.IndexOf(
                _mainPageViewModel.Words.FirstOrDefault(w => w.Word == Word.Word));

            if (index >= 0)
            {
                _mainPageViewModel.Words[index] = Word;
            }

            _mainPageViewModel.ApplyFilter();
            Application.Current.MainPage.Navigation.PopAsync();
        }

        private void OnDelete()
        {
            var index = _mainPageViewModel.Words.IndexOf(
                _mainPageViewModel.Words.FirstOrDefault(w => w.Word == Word.Word));
            if (index >= 0)
            {
                _mainPageViewModel.Words.RemoveAt(index);
            }

            _mainPageViewModel.ApplyFilter();
            Application.Current.MainPage.Navigation.PopAsync();
        }
    }
}
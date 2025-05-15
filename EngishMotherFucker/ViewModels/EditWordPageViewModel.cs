using System.Windows.Input;
using EngishMotherFucker.Models;
using EngishMotherFucker.Shared;

namespace EngishMotherFucker.ViewModels
{
    public class EditWordPageViewModel : BaseViewModel
    {
        private WordModel _word;
        private readonly MainPageViewModel _mainPageViewModel;
        private readonly SqliteConnectionFactory _connectionFactory;

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
            // Тут какой-то баг, связанный с первым словом.
            // Если его как либо изменить, то отображение не обновится
            // пока мы не изменим другое слово

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
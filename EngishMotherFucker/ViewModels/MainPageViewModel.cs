using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Messaging;
using EngishMotherFucker.Models;
using EngishMotherFucker.Utils;


namespace EngishMotherFucker.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        public event Action RequestAddWordPage;
        public event Action RequestStartTrainer;
        public event Action RequestOpenSettings;
        public event Action RequestDeleteWord;

        public ObservableCollection<WordModel> Words { get; } = new();
        public ObservableCollection<WordModel> FilteredWords { get; } = new();

        public ICommand AddWordCommand { get; }
        public ICommand StartTrainerCommand { get; }
        public ICommand OpenSettingsCommand { get; }

        private string searchText;
        public string SearchText
        {
            get => searchText;
            set
            {
                if (SetProperty(ref searchText, value))
                {
                    ApplyFilter();
                }
            }
        }

        private string emptyMessage;
        public string EmptyMessage
        {
            get => emptyMessage;
            set => SetProperty(ref emptyMessage, value);
        }


        public MainPageViewModel()
        {
            AddWordCommand = new Command(OnAddWord);
            StartTrainerCommand = new Command(OnStartTrainer);
            OpenSettingsCommand = new Command(OnOpenSettings);

            // Пример
            Words.Add(new WordModel { Word = "Apple", Translation = "Яблоко" });
            Words.Add(new WordModel { Word = "Run", Translation = "Бежать" });

            //for (int i = 0; i < 100; i++)
            //{
            //    Words.Add(new WordModel { Word = "Run", DefinitionEn = "move fast" });
            //}

            ApplyFilter();

            WeakReferenceMessenger.Default.Register<WordAddedMessage>(this, (r, m) =>
            {
                Words.Add(m.Value);
                ApplyFilter();
            });
        }

        public void ApplyFilter()
        {
            FilteredWords.Clear();

            var lower = SearchText?.ToLower() ?? "";

            var matchedWords = Words.Where(w =>
                w.Word.ToLower().Contains(lower) ||
                w.Translation.ToLower().Contains(lower)).ToList();

            foreach (var word in matchedWords)
                FilteredWords.Add(word);

            if (Words.Count == 0)
            {
                EmptyMessage = "Нет слов. Добавьте первое слово!";
            }
            else if (matchedWords.Count == 0)
            {
                EmptyMessage = "Ничего не найдено по запросу.";
            }
            else
            {
                EmptyMessage = string.Empty;
            }
        }

        public void AddWord(WordModel word)
        {
            Words.Add(word);
            ApplyFilter();
        }

        private async void OnAddWord()
        {
            RequestAddWordPage?.Invoke();
        }

        private void OnStartTrainer()
        {
            RequestStartTrainer?.Invoke();
        }

        private void OnOpenSettings()
        {
            // TODO: Открытие настроек
        }
    }
}

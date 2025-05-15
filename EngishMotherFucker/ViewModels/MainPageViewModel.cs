using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Messaging;
using EngishMotherFucker.Models;
using EngishMotherFucker.Shared;
using EngishMotherFucker.Utils;


namespace EngishMotherFucker.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        public event Action RequestAddWordPage;
        public event Action RequestStartTrainer;
        public event Action RequestOpenSettings;
        public event Action RequestDeleteWord;

        private readonly SqliteConnectionFactory _connectionFactory;
        public ObservableCollection<WordModel> Words { get; } = [];
        public ObservableCollection<WordModel> FilteredWords { get; } = [];

        public List<SearchCriterionOption> SearchCriterionOptions { get; } =
        [
            new() { Criterion = SearchCriterion.Word, DisplayName = "Слово" },
            new() { Criterion = SearchCriterion.Translation, DisplayName = "Перевод" },
            new() { Criterion = SearchCriterion.PartOfSpeech, DisplayName = "Часть речи" },
            new() { Criterion = SearchCriterion.Topic, DisplayName = "Тема" }
        ];

        public ICommand AddWordCommand { get; }
        public ICommand StartTrainerCommand { get; }
        public ICommand OpenSettingsCommand { get; }
        public ICommand SetCriterionCommand { get; }

        private SearchCriterion _selectedCriterion = SearchCriterion.Word;
        public SearchCriterion SelectedCriterion
        {
            get => _selectedCriterion;
            set
            {
                if (SetProperty(ref _selectedCriterion, value))
                    ApplyFilter();
            }
        }

        private SearchCriterionOption _selectedOption;
        public SearchCriterionOption SelectedOption
        {
            get => _selectedOption;
            set
            {
                if (SetProperty(ref _selectedOption, value))
                {
                    SelectedCriterion = value.Criterion;
                    ApplyFilter();
                }
            }
        }

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
            SetCriterionCommand = new Command<string>(OnSetCriterion);

            SelectedOption = SearchCriterionOptions.First();

            // Пример
            Words.Add(new WordModel { Word = "Computer", Translation = "Компьютер", PartOfSpeech="Существительное", Topic="Технологии" });
            Words.Add(new WordModel { Word = "Run", Translation = "Бежать", PartOfSpeech="Глагол", Topic="Действия" });
            Words.Add(new WordModel { Word = "Motherboard", Translation = "Материнская плата", PartOfSpeech="Существительное", Topic="Технологии" });
            Words.Add(new WordModel { Word = "Sleep", Translation = "Спать", PartOfSpeech="Глагол", Topic="Действия" });

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
            var criterion = SelectedOption?.Criterion.ToString() ?? "Word";

            IEnumerable<WordModel> matchedWords = Words;

            if (!string.IsNullOrWhiteSpace(lower))
            {
                matchedWords = Words.Where(w =>
                {
                    return criterion switch
                    {
                        "Word" => (w.Word ?? "").ToLower().Contains(lower),
                        "Translation" => (w.Translation ?? "").ToLower().Contains(lower),
                        "PartOfSpeech" => (w.PartOfSpeech ?? "").ToLower().Contains(lower),
                        "Topic" => (w.Topic ?? "").ToLower().Contains(lower),
                        _ => false
                    };
                });
            }

            foreach (var word in matchedWords)
                FilteredWords.Add(word);

            if (Words.Count == 0)
            {
                EmptyMessage = "Нет слов. Добавьте первое слово!";
            }
            else if (FilteredWords.Count == 0)
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
            RequestOpenSettings?.Invoke();
        }

        private void OnSetCriterion(string parameter)
        {
            if (Enum.TryParse(parameter, out SearchCriterion parsed))
            {
                var option = SearchCriterionOptions.FirstOrDefault(o => o.Criterion == parsed);
                if (option != null)
                    SelectedOption = option;
            }
        }
    }
}

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

        public ObservableCollection<WordModel> Words { get; private set; } = [];
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

            _ = LoadWordsFromDatabase();

            //WeakReferenceMessenger.Default.Register<WordAddedMessage>(this, async (r, m) =>
            //{
            //    if (Words == null)
            //        return;

            //    if (!Words.Any(w => w.Word == m.Value.Word && w.Translation == m.Value.Translation))
            //    {
            //        await App.Database.SaveWordAsync(m.Value);
            //        Words.Add(m.Value);
            //        ApplyFilter();
            //    }
            //});

        }

        ~MainPageViewModel()
        {
            WeakReferenceMessenger.Default.UnregisterAll(this);
        }

        private async Task LoadWordsFromDatabase()
        {
            var saved = await App.Database.GetWordsAsync();
            Words = new ObservableCollection<WordModel>(saved);
            ApplyFilter();
        }

        public async Task AddWordAsync(WordModel word)
        {
            if (!Words.Any(w => w.Word == word.Word && w.Translation == word.Translation))
            {
                await App.Database.SaveWordAsync(word);
                Words.Add(word);
                ApplyFilter();
            }
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

        private void OnStartTrainer()
        {
            RequestStartTrainer?.Invoke();
        }

        private void OnOpenSettings()
        {
            RequestOpenSettings?.Invoke();
        }

        private void OnAddWord()
        {
            RequestAddWordPage?.Invoke();
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

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

        public ICommand AddWordCommand { get; }
        public ICommand StartTrainerCommand { get; }
        public ICommand OpenSettingsCommand { get; }

        public MainPageViewModel()
        {
            AddWordCommand = new Command(OnAddWord);
            StartTrainerCommand = new Command(OnStartTrainer);
            OpenSettingsCommand = new Command(OnOpenSettings);

            // Пример
            Words.Add(new WordModel { Word= "Apple" , DefinitionEn = "red or green and sweet fruit" });
            //for (int i = 0; i < 100; i++)
            //{
            //    Words.Add(new WordModel { Word = "Run", DefinitionEn = "move fast" });
            //}
            WeakReferenceMessenger.Default.Register<WordAddedMessage>(this, (r, m) =>
            {
                Words.Add(m.Value);
            });
        }

        public void AddWord(WordModel word)
        {
            Words.Add(word);
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

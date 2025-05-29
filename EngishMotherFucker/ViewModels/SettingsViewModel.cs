using EngishMotherFucker.Models;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Xml;

namespace EngishMotherFucker.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        private int _questionCount;
        private string _selectedPrinciple;

        public ICommand ExportCommand { get; }
        public ICommand ImportCommand { get; }
        public ICommand ClearDatabaseCommand { get; }

        public ObservableCollection<string> AvailablePrinciples { get; }

        public int QuestionCount
        {
            get => _questionCount;
            set
            {
                if (SetProperty(ref _questionCount, value))
                {
                    Preferences.Set(nameof(QuestionCount), value);
                }
            }
        }

        public string SelectedPrinciple
        {
            get => _selectedPrinciple;
            set
            {
                if (SetProperty(ref _selectedPrinciple, value))
                {
                    Preferences.Set(nameof(SelectedPrinciple), value);
                }
            }
        }

        public SettingsViewModel()
        {
            ExportCommand = new Command(OnExport);
            ImportCommand = new Command(OnImport);
            ClearDatabaseCommand = new Command(async () => await OnClearDatabaseAsync());

            AvailablePrinciples = new ObservableCollection<string>
            {
                "Перевод EN > RU",
                "Перевод RU > EN",
                "Определение RU",
                "Определение EN",
            };

            QuestionCount = Preferences.Get(nameof(QuestionCount), 10);
            SelectedPrinciple = Preferences.Get(nameof(SelectedPrinciple), AvailablePrinciples[0]);
        }

        private async void OnExport()
        {
            try
            {
                string fileName = $"dictionary_export_{DateTime.Now:yyyyMMddHHmmss}.json";
                string directory = FileSystem.Current.AppDataDirectory;
                string filePath = Path.Combine(directory, fileName);

                var words = await App.Database.GetWordsAsync();
                string json = JsonConvert.SerializeObject(words, Newtonsoft.Json.Formatting.Indented);

                await File.WriteAllTextAsync(filePath, json);

                await Share.Default.RequestAsync(new ShareFileRequest
                {
                    Title = "Экспорт словаря",
                    File = new ShareFile(filePath)
                });
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", $"Не удалось экспортировать: {ex.Message}", "OK");
            }
        }


        private async void OnImport()
        {
            try
            {
                var fileResult = await FilePicker.Default.PickAsync(new PickOptions
                {
                    PickerTitle = "Выберите файл для импорта",
                    FileTypes = new FilePickerFileType(
                        new Dictionary<DevicePlatform, IEnumerable<string>>
                        {
                    { DevicePlatform.WinUI, new[] { ".json" } },
                    { DevicePlatform.Android, new[] { "application/json" } }
                        })
                });

                if (fileResult == null) return;

                string json = await File.ReadAllTextAsync(fileResult.FullPath);
                var words = JsonConvert.DeserializeObject<List<WordModel>>(json);

                if (words == null || words.Count == 0)
                {
                    await Application.Current.MainPage.DisplayAlert("Ошибка", "Файл пуст или повреждён", "OK");
                    return;
                }

                var db = App.Database;
                var existingWords = await db.GetWordsAsync();

                int addedCount = 0;

                foreach (var word in words)
                {
                    bool alreadyExists = existingWords.Any(w =>
                        w.Word == word.Word && w.Translation == word.Translation);

                    if (!alreadyExists)
                    {
                        word.Id = 0; // сбросим Id для нового добавления
                        await db.SaveWordAsync(word);
                        addedCount++;
                    }
                }

                await MainPageViewModel.Instance?.ReloadWordsFromDatabaseAsync();

                await Application.Current.MainPage.DisplayAlert(
                    "Импорт завершён",
                    $"Импортировано {addedCount} новых слов. Повторяющиеся пропущены.",
                    "OK");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", $"Не удалось импортировать: {ex.Message}", "OK");
            }
        }

        private async Task OnClearDatabaseAsync()
        {
            bool confirm = await Application.Current.MainPage.DisplayAlert(
                "Confirm",
                "Are you sure you want to delete all words from the database?",
                "Yes", "No");

            if (!confirm) return;

            await App.Database.DeleteAllWordsAsync();
            await MainPageViewModel.Instance?.ReloadWordsFromDatabaseAsync();

            await Application.Current.MainPage.DisplayAlert(
                "Success",
                "All words have been deleted.",
                "OK");
        }
    }
}

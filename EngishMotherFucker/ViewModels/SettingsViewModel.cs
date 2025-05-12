using System.Collections.ObjectModel;
using System.Windows.Input;
using EngishMotherFucker.ViewModels;

public class SettingsViewModel : BaseViewModel
{
    private int _questionCount;
    private string _selectedPrinciple;

    public ICommand ExportCommand { get; }
    public ICommand ImportCommand { get; }

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

        AvailablePrinciples =
        [
            "Перевод EN > RU",
            "Перевод RU > EN",
            "Определение RU",
            "Определение EN",
        ];

        // Загружаем сохранённые значения или берём стандартные
        QuestionCount = Preferences.Get(nameof(QuestionCount), 10);
        SelectedPrinciple = Preferences.Get(nameof(SelectedPrinciple), AvailablePrinciples[0]);
    }

    private async void OnExport()
    {
        await Application.Current.MainPage.DisplayAlert("Экспорт", "Экспорт базы в файл (заглушка)", "OK");
    }

    private async void OnImport()
    {
        await Application.Current.MainPage.DisplayAlert("Импорт", "Импорт базы из файла (заглушка)", "OK");
    }
}

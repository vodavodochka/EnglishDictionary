using System.Windows.Input;

namespace EngishMotherFucker.ViewModels
{
    public class TrainerStartViewModel : BaseViewModel
    {
        public int QuestionCount => Preferences.Get("QuestionCount", 10);

        public string SelectedPrinciple => Preferences.Get("SelectedPrinciple", "Перевод EN > RU");

        public ICommand StartTrainerCommand { get; }

        public TrainerStartViewModel()
        {
            StartTrainerCommand = new Command(StartTrainer);
        }

        private async void StartTrainer()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new TrainerQuestionPage());
        }
    }
}

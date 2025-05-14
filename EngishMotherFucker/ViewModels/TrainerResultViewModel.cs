using System.Windows.Input;

namespace EngishMotherFucker.ViewModels
{
    public class TrainerResultViewModel : BaseViewModel
    {
        public int CorrectAnswers { get; }
        public int TotalQuestions { get; }

        public string ResultText => $"Вы ответили правильно на {CorrectAnswers} из {TotalQuestions} вопросов";

        public ICommand GoBackCommand => new Command(async () =>
        {
            await Application.Current.MainPage.Navigation.PopToRootAsync();
        });

        public TrainerResultViewModel(int correctAnswers, int totalQuestions)
        {
            CorrectAnswers = correctAnswers;
            TotalQuestions = totalQuestions;
        }
    }
}

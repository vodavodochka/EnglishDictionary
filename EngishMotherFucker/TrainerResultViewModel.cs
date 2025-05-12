using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EngishMotherFucker
{
    public class TrainerResultViewModel : BaseViewModel
    {
        public int CorrectAnswers { get; }
        public int TotalQuestions { get; }

        public string ResultText => $"Вы ответили правильно на {CorrectAnswers} из {TotalQuestions}";

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

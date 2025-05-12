using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EngishMotherFucker
{
    public class TrainerStartViewModel : BaseViewModel
    {
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

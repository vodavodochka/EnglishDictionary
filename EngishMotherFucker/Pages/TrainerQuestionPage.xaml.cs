using EngishMotherFucker.ViewModels;

namespace EngishMotherFucker
{
    public partial class TrainerQuestionPage : ContentPage
    {
    	public TrainerQuestionPage()
    	{
    		InitializeComponent();
    		BindingContext = new TrainerQuestionViewModel();
    	}

        private void OnOptionSelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is string selected)
            {
                (BindingContext as TrainerQuestionViewModel)?.CheckAnswer(selected);
            }
        }

    }
}
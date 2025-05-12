namespace EngishMotherFucker;

public partial class TrainerResultPage : ContentPage
{
	public TrainerResultPage(int correctAnswers, int totalQuestions)
	{
		InitializeComponent();
		BindingContext = new TrainerResultViewModel(correctAnswers, totalQuestions);
	}
}
using EngishMotherFucker.ViewModels;

namespace EngishMotherFucker
{
    public partial class TrainerStartPage : ContentPage
    {
    	public TrainerStartPage()
    	{
    		InitializeComponent();
    		BindingContext = new TrainerStartViewModel();
    	}
    }
}
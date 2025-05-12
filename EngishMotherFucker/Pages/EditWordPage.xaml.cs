using EngishMotherFucker.Models;
using EngishMotherFucker.ViewModels;

namespace EngishMotherFucker
{
    public partial class EditWordPage : ContentPage
    {
        public EditWordPage(WordModel word, MainPageViewModel mainPageViewModel)
        {
            InitializeComponent();
            BindingContext = new EditWordPageViewModel(word, mainPageViewModel);
        }
    }
}

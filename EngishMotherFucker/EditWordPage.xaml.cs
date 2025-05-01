using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

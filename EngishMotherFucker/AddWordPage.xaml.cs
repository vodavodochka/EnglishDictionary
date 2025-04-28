using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngishMotherFucker
{
    public partial class AddWordPage : ContentPage
    {
        public AddWordPage()
        {
            InitializeComponent();   
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(WordEntry.Text))
            {
                await DisplayAlert("Ошибка", "Пожалуйста, введите слово.", "ОК");
                return;
            }

            var newWord = new WordModel
            {
                Word = WordEntry.Text.Trim(),
                Translation = TranslationEntry.Text?.Trim(),
                Transcription = TranscriptionEntry.Text?.Trim(),
                DefinitionEn = DefinitionEnEntry.Text?.Trim(),
                DefinitionRu = DefinitionRuEntry.Text?.Trim(),
                Topic = TopicEntry.Text?.Trim()
            };

            WeakReferenceMessenger.Default.Send(new WordAddedMessage(newWord));

            await Navigation.PopAsync();
        }
    }
}

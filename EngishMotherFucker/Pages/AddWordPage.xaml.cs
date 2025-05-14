using CommunityToolkit.Mvvm.Messaging;
using EngishMotherFucker.Models;
using EngishMotherFucker.Utils;

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
                PartOfSpeech = PartOfSpeechEntry.Text?.Trim(),
                DefinitionEn = DefinitionEnEntry.Text?.Trim(),
                DefinitionRu = DefinitionRuEntry.Text?.Trim(),
                Examples = ExamplesEntry.Text?.Trim(),
                Topic = TopicEntry.Text?.Trim()
            };

            WeakReferenceMessenger.Default.Send(new WordAddedMessage(newWord));

            await Navigation.PopAsync();
        }
    }
}

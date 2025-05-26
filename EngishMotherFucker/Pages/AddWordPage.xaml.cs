using CommunityToolkit.Mvvm.Messaging;
using EngishMotherFucker.Models;
using EngishMotherFucker.Utils;
using EngishMotherFucker.ViewModels;

namespace EngishMotherFucker
{
    public partial class AddWordPage : ContentPage
    {
        private readonly MainPageViewModel _viewModel;

        public AddWordPage(MainPageViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
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

            await _viewModel.AddWordAsync(newWord); // напрямую добавляем

            await Navigation.PopAsync();
        }

        private async void OnCancelClicked(object sender, EventArgs e)
        {
            bool confirm = await DisplayAlert("Отмена", "Вы уверены, что хотите отменить добавление слова?", "Да", "Нет");
            if (confirm)
            {
                await Navigation.PopAsync();
            }
        }
    }
}
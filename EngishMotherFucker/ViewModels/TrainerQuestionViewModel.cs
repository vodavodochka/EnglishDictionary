using System.Collections.ObjectModel;
using EngishMotherFucker.Models;
using EngishMotherFucker.Utils;

namespace EngishMotherFucker.ViewModels
{
    public class TrainerQuestionViewModel : BaseViewModel
    {
        private int currentIndex = 0;
        private int correctAnswers = 0;

        public string CurrentQuestionText { get; set; }
        public ObservableCollection<string> CurrentOptions { get; set; } = new();

        public string FeedbackText { get; set; }

        private List<QuestionModel> Questions { get; set; } = new();

        private bool areOptionsEnabled = true;
        public bool AreOptionsEnabled
        {
            get => areOptionsEnabled;
            set
            {
                if (areOptionsEnabled != value)
                {
                    areOptionsEnabled = value;
                    OnPropertyChanged();
                }
            }
        }

        public TrainerQuestionViewModel()
        {
            LoadQuestionsAsync();
        }

        private async void LoadQuestionsAsync()
        {
            var baseWords = await App.Database.GetWordsAsync();
            var selectedPrinciple = Preferences.Get("SelectedPrinciple", "Перевод EN > RU");
            var questionCount = Preferences.Get("QuestionCount", 10);

            try
            {
                Questions = TrainerQuestionGenerator.Generate(baseWords, selectedPrinciple, questionCount);

                if (Questions.Count == 0)
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Недостаточно слов",
                        "Для выбранного режима тренировки недостаточно слов. Пожалуйста, добавьте хотя бы одно подходящее слово.",
                        "Ок");

                    await Application.Current.MainPage.Navigation.PopAsync(); // вернуться назад
                    return;
                }

                LoadNextQuestion();
            }
            catch (InvalidOperationException ex)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Ошибка",
                    ex.Message,
                    "Ок");

                await Application.Current.MainPage.Navigation.PopAsync(); // вернуться назад
            }
        }

        private async void LoadNextQuestion()
        {
            if (currentIndex >= Questions.Count)
            {
                await Application.Current.MainPage.Navigation.PushAsync(
                    new TrainerResultPage(correctAnswers, Questions.Count)
                );

                return;
            }

            var q = Questions[currentIndex];
            CurrentQuestionText = q.QuestionText;
            CurrentOptions.Clear();
            foreach (var opt in q.Options) CurrentOptions.Add(opt);
            FeedbackText = string.Empty;

            AreOptionsEnabled = true;

            OnPropertyChanged(nameof(CurrentQuestionText));
            OnPropertyChanged(nameof(CurrentOptions));
            OnPropertyChanged(nameof(FeedbackText));
        }

        public void CheckAnswer(string selected)
        {
            if (!AreOptionsEnabled) return;

            AreOptionsEnabled = false;

            var correct = Questions[currentIndex].CorrectAnswer;
            if (selected == correct)
            {
                FeedbackText = "✅ Верно!";
                correctAnswers++;
            }
            else
            {
                FeedbackText = $"❌ Неверно! Правильный ответ: {correct}";
            }

            OnPropertyChanged(nameof(FeedbackText));
            currentIndex++;

            Device.StartTimer(TimeSpan.FromSeconds(2), () =>
            {
                LoadNextQuestion();
                return false;
            });
        }
    }
}
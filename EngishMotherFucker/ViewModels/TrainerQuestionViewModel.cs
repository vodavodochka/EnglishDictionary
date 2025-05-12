using System.Collections.ObjectModel;
using EngishMotherFucker.Models;

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
            LoadQuestions(); // Заглушка
            LoadNextQuestion();
        }

        private void LoadQuestions()
        {
            Questions =
            [
                new("Translate: Apple", ["Яблоко", "Апельсин", "Банан", "Груша"], "Яблоко"),
                new("Translate: Dog", ["Кошка", "Собака", "Лошадь", "Мышь"], "Собака"),
                // Здесь будете генерить и грузить вопросы
            ];
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

            AreOptionsEnabled = true; // Разрешаем выбор

            OnPropertyChanged(nameof(CurrentQuestionText));
            OnPropertyChanged(nameof(CurrentOptions));
            OnPropertyChanged(nameof(FeedbackText));
        }

        public void CheckAnswer(string selected)
        {
            if (!AreOptionsEnabled) return;

            AreOptionsEnabled = false; // Блокируем выбор после первого ответа

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

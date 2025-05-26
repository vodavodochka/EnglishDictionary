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
            LoadQuestionsAsync();
        }

        private async void LoadQuestionsAsync()
        {
            var baseWords = await App.Database.GetWordsAsync();

            var selectedPrinciple = Preferences.Get("SelectedPrinciple", "Перевод EN > RU");
            var questionCount = Preferences.Get("QuestionCount", 10);

            var rng = new Random();
            var shuffled = baseWords.OrderBy(_ => rng.Next()).Take(questionCount).ToList();

            List<QuestionModel> generatedQuestions = new();

            foreach (var word in shuffled)
            {
                List<string> options;
                string questionText = "";
                string correctAnswer = "";

                List<string> GetDistractors(Func<WordModel, string> selector)
                {
                    return baseWords
                        .Where(w => selector(w) != selector(word))
                        .Select(selector)
                        .Distinct()
                        .OrderBy(_ => rng.Next())
                        .Take(3)
                        .ToList();
                }

                switch (selectedPrinciple)
                {
                    case "Перевод EN > RU":
                        correctAnswer = word.Translation;
                        options = GetDistractors(w => w.Translation);
                        options.Add(correctAnswer);
                        options = options.OrderBy(_ => rng.Next()).ToList();
                        questionText = $"Переведи слово: {word.Word}";
                        break;

                    case "Перевод RU > EN":
                        correctAnswer = word.Word;
                        options = GetDistractors(w => w.Word);
                        options.Add(correctAnswer);
                        options = options.OrderBy(_ => rng.Next()).ToList();
                        questionText = $"Переведи слово: {word.Translation}";
                        break;

                    case "Определение RU":
                        correctAnswer = word.Translation;
                        options = GetDistractors(w => w.Translation);
                        options.Add(correctAnswer);
                        options = options.OrderBy(_ => rng.Next()).ToList();
                        questionText = $"Что означает: {word.DefinitionRu}";
                        break;

                    case "Определение EN":
                        correctAnswer = word.Word;
                        options = GetDistractors(w => w.Word);
                        options.Add(correctAnswer);
                        options = options.OrderBy(_ => rng.Next()).ToList();
                        questionText = $"What does it mean: {word.DefinitionEn}";
                        break;

                    default:
                        continue;
                }

                generatedQuestions.Add(new QuestionModel(questionText, options.ToArray(), correctAnswer));
            }

            Questions = generatedQuestions;
            LoadNextQuestion();
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
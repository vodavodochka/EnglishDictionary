using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngishMotherFucker.Models;

namespace EngishMotherFucker.Utils
{
    public static class TrainerQuestionGenerator
    {
        public static List<QuestionModel> Generate(List<WordModel> words, string principle, int count)
        {
            var rng = new Random();

            // Отфильтровать по режиму
            List<WordModel> eligibleWords = principle switch
            {
                "Определение RU" => words.Where(w => !string.IsNullOrWhiteSpace(w.DefinitionRu)).ToList(),
                "Определение EN" => words.Where(w => !string.IsNullOrWhiteSpace(w.DefinitionEn)).ToList(),
                "Перевод RU > EN" => words.Where(w => !string.IsNullOrEmpty(w.Translation)).ToList(),
                "Перевод EN > RU" => words.Where(w => !string.IsNullOrEmpty(w.Translation)).ToList(),
                _ => words
            };

            if (eligibleWords.Count == 0)
                throw new InvalidOperationException("Нет подходящих слов для выбранного режима.");

            if (eligibleWords.Count < count)
                count = eligibleWords.Count;

            var selectedWords = eligibleWords.OrderBy(_ => rng.Next()).Take(count).ToList();
            var questions = new List<QuestionModel>();

            foreach (var word in selectedWords)
            {
                string questionText = "";
                string correctAnswer = "";
                List<string> options;

                List<string> GetDistractors(Func<WordModel, string> selector)
                {
                    return eligibleWords
                        .Where(w => selector(w) != selector(word))
                        .Select(selector)
                        .Distinct()
                        .OrderBy(_ => rng.Next())
                        .Take(3)
                        .ToList();
                }

                switch (principle)
                {
                    case "Перевод EN > RU":
                        correctAnswer = word.Translation;
                        options = GetDistractors(w => w.Translation);
                        questionText = $"Переведи слово: {word.Word}";
                        break;

                    case "Перевод RU > EN":
                        correctAnswer = word.Word;
                        options = GetDistractors(w => w.Word);
                        questionText = $"Переведи слово: {word.Translation}";
                        break;

                    case "Определение RU":
                        correctAnswer = word.Word;
                        options = GetDistractors(w => w.Word);
                        questionText = $"Что это: {word.DefinitionRu}";
                        break;

                    case "Определение EN":
                        correctAnswer = word.Translation;
                        options = GetDistractors(w => w.Translation);
                        questionText = $"Угадай слово: {word.DefinitionEn}";
                        break;

                    default:
                        continue;
                }

                options.Add(correctAnswer);
                options = options.OrderBy(_ => rng.Next()).ToList();

                questions.Add(new QuestionModel(questionText, options.ToArray(), correctAnswer));
            }

            return questions;
        }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngishMotherFucker
{
    public class QuestionModel
    {
        public string QuestionText { get; }
        public string[] Options { get; }
        public string CorrectAnswer { get; }

        public QuestionModel(string questionText, string[] options, string correctAnswer)
        {
            QuestionText = questionText;
            Options = options;
            CorrectAnswer = correctAnswer;
        }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngishMotherFucker
{
    public class WordModel : BaseViewModel
    {
        public string Word { get; set; } // Слово на английском
        public string Translation { get; set; } // Перевод на русский
        public string Transcription { get; set; } // Транскрипция (необязательно)
        public string DefinitionRu { get; set; } // Определение на русском
        public string DefinitionEn { get; set; } // Определение на английском
        public string Topic { get; set; } // Тема (категория слова)
    }

}

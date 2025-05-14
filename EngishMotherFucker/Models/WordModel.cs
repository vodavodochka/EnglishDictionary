namespace EngishMotherFucker.Models
{
    public class WordModel
    {
        public string Word { get; set; } // Слово на английском
        public string Translation { get; set; } // Перевод на русский
        public string Transcription { get; set; } // Транскрипция (необязательно)
        public string PartOfSpeech { get; set; } // Часть речи (существительное, глагол и т.д.)
        public string DefinitionRu { get; set; } // Определение на русском
        public string DefinitionEn { get; set; } // Определение на английском
        public string Examples { get; set; } // Примеры использования
        public string Topic { get; set; } // Тема (категория слова)
    }
}

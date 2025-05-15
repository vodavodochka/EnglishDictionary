using System.Text.Json;

namespace EngishMotherFucker.Models
{
    public static class WordStorage
    {
        private const string WordsKey = "SavedWords";

        public static void SaveWords(List<WordModel> words)
        {
            var json = JsonSerializer.Serialize(words);
            Preferences.Set(WordsKey, json);
        }

        public static List<WordModel> LoadWords()
        {
            var json = Preferences.Get(WordsKey, null);
            if (string.IsNullOrWhiteSpace(json)) return null;

            try
            {
                return JsonSerializer.Deserialize<List<WordModel>>(json);
            }
            catch
            {
                return null;
            }
        }
    }

}


namespace EngishMotherFucker.Models
{
    public class SearchCriterionOption
    {
        public SearchCriterion Criterion { get; set; }
        public string DisplayName { get; set; }
    }

    public enum SearchCriterion
    {
        Word,
        Translation,
        PartOfSpeech,
        Topic
    }
}

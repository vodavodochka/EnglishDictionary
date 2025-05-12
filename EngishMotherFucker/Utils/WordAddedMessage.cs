using CommunityToolkit.Mvvm.Messaging.Messages;
using EngishMotherFucker.Models;

namespace EngishMotherFucker.Utils
{
    public class WordAddedMessage : ValueChangedMessage<WordModel>
    {
        public WordAddedMessage(WordModel value) : base(value)
        {
        }
    }
}

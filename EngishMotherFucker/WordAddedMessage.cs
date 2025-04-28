using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace EngishMotherFucker
{
    public class WordAddedMessage : ValueChangedMessage<WordModel>
    {
        public WordAddedMessage(WordModel value) : base(value)
        {
        }
    }
}

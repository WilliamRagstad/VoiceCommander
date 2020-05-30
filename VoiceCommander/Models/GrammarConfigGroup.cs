using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoiceCommander
{
    class GrammarConfigGroup
    {
        public string Phrase;
        public GrammarConfigElement[] Elements;
        public GrammarConfigGroup[] Groups;

        public GrammarConfigGroup(string phrase, params GrammarConfigElement[] elements) : this(phrase, elements, null) { }
        public GrammarConfigGroup(string phrase, params GrammarConfigGroup[] groups) : this(phrase, null, groups) { }
        [JsonConstructor]
        public GrammarConfigGroup(string phrase, GrammarConfigElement[] elements, GrammarConfigGroup[] groups)
        {
            if (!string.IsNullOrEmpty(phrase)) Phrase = phrase;
            this.Elements = elements;
            this.Groups = groups;
        }
    }
}

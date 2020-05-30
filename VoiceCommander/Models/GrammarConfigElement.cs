using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoiceCommander
{
    class GrammarConfigElement
    {
        public string Phrase;
        public string Action;
        public string[] ActionData;
        [JsonConstructor]
        public GrammarConfigElement(string phrase, GrammarConfigAction action, params string[] actionData)
        {
            Phrase = phrase;
            Action = Enum.GetName(typeof(GrammarConfigAction), action);
            ActionData = actionData;
        }

        public GrammarConfigAction GetAction()
        {
            return (GrammarConfigAction)Enum.Parse(typeof(GrammarConfigAction), Action);
        }
    }
}

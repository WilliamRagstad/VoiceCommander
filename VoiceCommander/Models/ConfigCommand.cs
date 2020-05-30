using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoiceCommander
{
    class ConfigCommand
    {
        public ConfigCommand(string phrase, GrammarConfigAction action, string[] data)
        {
            Phrase = phrase;
            Action = action;
            Data = data;
        }

        public string Phrase { get; }
        public GrammarConfigAction Action { get; }
        public string[] Data { get; }
    }
}

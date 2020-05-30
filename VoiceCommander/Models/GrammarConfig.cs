using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoiceCommander
{
    class GrammarConfig : GrammarConfigGroup
    {
        public static readonly GrammarConfig Default = new GrammarConfig(
            "Example Config",
            "Example configuration created 2020 by William.",
            new[] {
                new GrammarConfigElement("Hello there", GrammarConfigAction.Say, "General Kenobi"),
                new GrammarConfigElement("What's the time", GrammarConfigAction.SayCode, "\"The time is \" + DateTime.Now.ToString(\"hh:mm tt\")"),
                new GrammarConfigElement("who am I?", GrammarConfigAction.Code, "System.Console.WriteLine(\"Hello, You are \" + System.Environment.UserName)"),
                new GrammarConfigElement("who are you?", GrammarConfigAction.Code, "System.Windows.Forms.MessageBox.Show(\"Hello, I am \" + \"VoiceCommander\")"),
                new GrammarConfigElement("whats potato?", GrammarConfigAction.Code,
                    "HtmlAgilityPack.HtmlWeb web = new HtmlAgilityPack.HtmlWeb();",
                    "HtmlAgilityPack.HtmlDocument htmlDoc = web.Load(\"https://en.wikipedia.org/wiki/Potato\");",
                    "HtmlAgilityPack.HtmlNode node = htmlDoc.DocumentNode.SelectSingleNode(\"//*[@id=\\\"mw-content-text\\\"]/div/p[3]\");",
                    "System.Console.WriteLine(node.InnerText.Split('&')[0]);"
                    ),
                new GrammarConfigElement("tell me what a potato is?", GrammarConfigAction.SayCode,
                    "(((new HtmlAgilityPack.HtmlWeb()).Load(\"https://en.wikipedia.org/wiki/Potato\")).DocumentNode.SelectSingleNode(\"//*[@id=\\\"mw-content-text\\\"]/div/p[3]\")).InnerText"),
                new GrammarConfigElement("say hi to me", GrammarConfigAction.PowerShell, "Write-Host \"Hello there mister!\" -f Yellow"),
                new GrammarConfigElement("hello", GrammarConfigAction.Shell, "dir & echo Hi!")
            },
            new[] {
                new GrammarConfigGroup("open", new[] {
                    new GrammarConfigElement("notepad", GrammarConfigAction.OpenFile, "notepad.exe")
                }),
                new GrammarConfigGroup("hack", new[] {
                    new GrammarConfigElement("FBI", GrammarConfigAction.SayRandom, 
                        "Pisst, That's childsplay",
                        "Sorry kiddo, I have to tell your parents about this",
                        "Oh, I've already prepared a backdoor before you asked :)",
                        "Hack compleated! CIA has been hacked! Oh wait, fuck"),
                    new GrammarConfigElement("NASA", GrammarConfigAction.Say, "Okay, hacking in progress... Man, you will never belive what I found out exists!"),
                    new GrammarConfigElement("Pentagon", GrammarConfigAction.Say, "Maybe you should be more original...")
                })
            }
            );

        public string Name;
        public string Description;

        public GrammarConfig(string name, string description, params GrammarConfigElement[] elements) : this(name, description, elements, null) { }
        public GrammarConfig(string name, string description, params GrammarConfigGroup[] groups) : this(name, description, null, groups) { }
        [JsonConstructor]
        public GrammarConfig(string name, string description, GrammarConfigElement[] elements, GrammarConfigGroup[] groups) : base(null, elements, groups)
        {
            Name = name;
            Description = description;
            Groups = groups;
        }
    }
}
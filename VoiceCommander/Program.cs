using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Recognition;
using System.IO;
using Newtonsoft.Json;
using System.Speech.Synthesis;

namespace VoiceCommander
{
    class Program
    {
        public static VoiceCommander VoiceCommander;
        static void Main(string[] args)
        {
            Console.WriteLine(Settings.TitleThumbnail);

            Console.WriteLine("---- Startup");
            SetupFileStructure();
            RecognizerInfo recognizer = GetRecognizer();
            VoiceInfo voice = GetVoice();
            if (recognizer != null && voice != null)
            {
                Console.WriteLine($"Found Compatible Recognizer: {recognizer.Name} ({recognizer.Culture.Name}) - {recognizer.Description}");
                Console.WriteLine($"Found Voice Synthesizer: {voice.Name} - {voice.Description}");

                VoiceCommander = new VoiceCommander(recognizer, voice);
                VoiceCommander.Start();
            }
            while (true) Console.ReadKey(true);
        }

        private static VoiceInfo GetVoice()
        {
            var voices = (new SpeechSynthesizer()).GetInstalledVoices();
            if (voices.Count > 1)
            {
                Console.WriteLine("Please select one of the following voice synthesizers to use:");
                for (int i = 0; i < voices.Count; i++) Console.WriteLine($"  {i}: {voices[i].VoiceInfo.Name} - {voices[i].VoiceInfo.Description}");

                int choice = -1;
                bool validChoice = false;
                while (!validChoice)
                {
                    Console.Write("Select: ");
                    validChoice = int.TryParse(Console.ReadLine(), out choice) && choice >= 0 && choice < voices.Count;
                    if (!validChoice) Console.WriteLine("Please select a valid index from the list.");
                }

                return voices[choice].VoiceInfo;
            }
            if (voices.Count == 1) return voices[0].VoiceInfo;
            else return null;
        }

        static RecognizerInfo GetRecognizer()
        {
            var recognizers = SpeechRecognitionEngine.InstalledRecognizers();
            if (recognizers.Count > 0)
            {
                List<RecognizerInfo> compatibleRecognizers = new List<RecognizerInfo>();
                for (int i = 0; i < recognizers.Count; i++)
                    //if (recognizers[i].Culture.Name == Settings.CompatibleCultureInfo)
                    if (recognizers[i].Culture.TwoLetterISOLanguageName == Settings.CompatibleISOLanguageName)
                            compatibleRecognizers.Add(recognizers[i]);

                if (compatibleRecognizers.Count > 1)
                {
                    Console.WriteLine("Please select one of the following recognizers to use:");
                    for (int i = 0; i < compatibleRecognizers.Count; i++) Console.WriteLine($"  {i}: {compatibleRecognizers[i].Name} - {compatibleRecognizers[i].Description}");

                    int choice = -1;
                    bool validChoice = false;
                    while(!validChoice)
                    {
                        Console.Write("Select: ");
                        validChoice = int.TryParse(Console.ReadLine(), out choice) && choice >= 0 && choice < compatibleRecognizers.Count;
                        if (!validChoice) Console.WriteLine("Please select a valid index from the list.");
                    }

                    return compatibleRecognizers[choice];
                }
                if (compatibleRecognizers.Count == 1) return compatibleRecognizers[0];
                else
                    Console.WriteLine("Some recognizers were found, but no one was compatible with VoiceCommander... (Please install one for English)");
            }
            else
                Console.WriteLine("No recognizers found... (Please install one for English)");
            return null;
        }

        static void SetupFileStructure()
        {
            if (!Directory.Exists(Settings.HomePath)) Directory.CreateDirectory(Settings.HomePath);
            if (!Directory.Exists(Settings.ConfigFilesPath))
            {
                Directory.CreateDirectory(Settings.ConfigFilesPath);

                if (!File.Exists(Settings.ExampleConfigPath))
                    File.WriteAllText(Settings.ExampleConfigPath, JsonConvert.SerializeObject(GrammarConfig.Default, Formatting.Indented, new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                    }));
            }
        }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Speech.Recognition;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace VoiceCommander
{
    class CommandController
    {
        private Random random;
        private VoiceCommander voiceCommander;
        private List<ConfigCommand> commands;
        //private Dictionary<string, KeyValuePair<GrammarConfigAction, string[]>> commands;

        public CommandController(VoiceCommander voiceCommander)
        {
            this.voiceCommander = voiceCommander;
            //commands = new Dictionary<string, KeyValuePair<GrammarConfigAction, string[]>>();
            commands = new List<ConfigCommand>();
            random = new Random((int)(DateTime.Now.Ticks % int.MaxValue));
            Console.WriteLine("\n--- Configs");
            loadConfigs();
        }

        private void loadConfigs()
        {
            if (commands.Count > 0) commands.Clear();
            foreach (string configFile in Directory.GetFiles(Settings.ConfigFilesPath)) parseConfig(configFile);
        }

        /**
         * Load grammar and actions from a configuration file
         * and add them to the total commands grammar.
         */
        private void parseConfig(string configFile)
        {
            if (!File.Exists(configFile)) Console.WriteLine($"Config: {configFile} does not exist!");

            string rawJSON = File.ReadAllText(configFile);
            GrammarConfig config = null;
            try
            {
                config = JsonConvert.DeserializeObject<GrammarConfig>(rawJSON);
                Console.WriteLine($"Loading Config: {config.Name} - {config.Description}");

                RegisterElementsIn(config);
            }
            catch(Exception e)
            {
                Console.WriteLine($"Failed to Load Config '{configFile}':\n\t{e.Message}");
            }
        }

        private void RegisterElementsIn(GrammarConfigGroup group, string rawCombinedPhrase = null)
        {
            string combinedPhrase = (string.IsNullOrEmpty(rawCombinedPhrase) ? "" : rawCombinedPhrase + " ");
            if (group.Elements != null)
                foreach (GrammarConfigElement e in group.Elements)
                    commands.Add(new ConfigCommand(combinedPhrase + e.Phrase.ToLower(), e.GetAction(), e.ActionData));
            if (group.Groups != null)
                foreach (GrammarConfigGroup g in group.Groups) RegisterElementsIn(g, combinedPhrase + g.Phrase.ToLower());
        }

        public void InjectGrammar()
        {
            List<string> phrases = new List<string>();
            for (int i = 0; i < commands.Count; i++) if (!phrases.Contains(commands[i].Phrase)) phrases.Add(commands[i].Phrase);

            voiceCommander.SpeechRecognizer.LoadGrammarAsync(
            new Grammar(
                new GrammarBuilder(new Choices(phrases.ToArray())) { Culture = new System.Globalization.CultureInfo(Settings.CompatibleCultureInfo) }
                )
            );
        }

        public async void HandleCommandAsync(string phrase)
        {
            for (int i = 0; i < commands.Count; i++)
            {
                if (commands[i].Phrase == phrase)
                {
                    string[] data = commands[i].Data;
                    switch (commands[i].Action)
                    {
                        case GrammarConfigAction.Say:
                            for (int j = 0; j < data.Length; j++)
                            {
                                Console.WriteLine("Said: " + data[j]);
                                voiceCommander.SpeechSynthesizer.Speak(data[j]);
                            }
                            break;
                        case GrammarConfigAction.OpenFile:
                            if (data == null || data.Length == 0) break;
                            Process.Start(data[0]);
                            Console.WriteLine("Opened: " + data[0]);
                            break;
                        case GrammarConfigAction.Shell:
                            if (data == null || data.Length == 0) break;
                            ProcessStartInfo startInfoShell = new ProcessStartInfo("cmd.exe", "/C " + ArrayAsArguments(data));
                            startInfoShell.UseShellExecute = false;
                            // startInfoShell.RedirectStandardOutput = true;
                            // startInfoShell.RedirectStandardError = true;
                            // startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                            // startInfo.CreateNoWindow = true;
                            Process p1 = new Process() { StartInfo = startInfoShell };
                            p1.Start();
                            break;
                        case GrammarConfigAction.PowerShell:
                            if (data == null || data.Length == 0) break;
                            ProcessStartInfo startInfoPowershell = new ProcessStartInfo("powershell.exe", "/C " + ArrayAsArguments(data));
                            startInfoPowershell.UseShellExecute = false;
                            // startInfoPowershell.RedirectStandardOutput = true;
                            // startInfoPowershell.RedirectStandardError = true;
                            // startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                            // startInfo.CreateNoWindow = true;
                            Process p2 = new Process() { StartInfo = startInfoPowershell };
                            p2.Start();
                            break;
                        case GrammarConfigAction.SayRandom:
                            if (data == null || data.Length == 0) break;
                            string randomString = data[random.Next(0, data.Length)];
                            Console.WriteLine("Said: " + randomString);
                            voiceCommander.SpeechSynthesizer.Speak(randomString);
                            break;
                        case GrammarConfigAction.SayCode:
                            try
                            {
                                var result = await EvaluateAsync(data[0].TrimEnd(';'));
                                Console.WriteLine("Said: " + result);
                                voiceCommander.SpeechSynthesizer.Speak(result.ToString());
                            }
                            catch(Exception e)
                            {
                                Console.WriteLine("Syntax " + e.Message);
                            }
                            break;
                        case GrammarConfigAction.Code:
                            try
                            {
                                string code = "";
                                for (int line = 0; line < data.Length; line++)
                                {
                                    code += data[line];
                                    if (line != data.Length - 1) code += '\n';
                                }

                                await ExecuteAsync(code);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("Syntax Error: " + e.Message);
                            }
                            break;
                        case GrammarConfigAction.CodeFileCS:
                            try
                            {
                                if (data.Length > 0)
                                {
                                    string file = data[0];
                                    if (!File.Exists(file)) {
                                        if (File.Exists(Path.Combine(Settings.ConfigFilesPath, file))) file = Path.Combine(Settings.ConfigFilesPath, file);
                                        else
                                        {
                                            Console.WriteLine("Provided file does not exist!");
                                            break;
                                        }
                                    }

                                    if (Path.GetExtension(file) == ".cs")
                                    {
                                        await ExecuteAsync(File.ReadAllText(file));
                                    }
                                    else
                                        Console.WriteLine("Provided file is not a C# (.cs) source file!");

                                }
                                else
                                    Console.WriteLine("Missing C# (.cs) source file!");
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("Syntax Error: " + e.Message);
                            }
                            break;
                    }
                }
            }
        }

        private string ArrayAsArguments(string[] array)
        {
            string result = "";
            for (int i = 0; i < array.Length; i++)
            {
                result += '"' + array[i] + '"';
                if (i != array.Length - 1) result += ' ';
            }
            return result;
        }

        // https://github.com/dotnet/roslyn/wiki/Scripting-API-Samples
        // https://github.com/dotnet/roslyn/wiki/Interactive-Window
        // https://stackoverflow.com/questions/4629/how-can-i-evaluate-c-sharp-code-dynamically
        ScriptOptions scriptOptions = ScriptOptions.Default
                .WithImports("System", "System.IO", "System.Math")
                .WithReferences(
                    typeof(System.Windows.Forms.MessageBox).Assembly, // System.Windows.Forms.dll 
                    typeof(HtmlAgilityPack.HtmlWeb).Assembly          // HtmlAgilityPack.dll
                    );
        private async System.Threading.Tasks.Task<object> EvaluateAsync(string expression)
        {
            return await CSharpScript.EvaluateAsync(expression, scriptOptions);
        }

        private async System.Threading.Tasks.Task<object> ExecuteAsync(string code)
        {
            await CSharpScript.RunAsync(code, scriptOptions);
            return null;
        }
    }
}

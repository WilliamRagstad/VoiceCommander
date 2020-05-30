using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;

namespace VoiceCommander
{
    // https://www.youtube.com/watch?v=AB9lfHDOe5U
    class VoiceCommander
    {
        private readonly RecognizerInfo recognizer;
        private readonly CommandController commandController;
        public SpeechRecognitionEngine SpeechRecognizer;
        public SpeechSynthesizer SpeechSynthesizer;
        public VoiceCommander(RecognizerInfo recognizer, VoiceInfo synthesizerVoice = null)
        {
            SpeechSynthesizer = new SpeechSynthesizer();
            if (synthesizerVoice != null) SpeechSynthesizer.SelectVoice(synthesizerVoice.Name);

            this.recognizer = recognizer;
            this.SpeechRecognizer = new SpeechRecognitionEngine(recognizer);

            this.commandController = new CommandController(this);
            this.commandController.InjectGrammar();

            SpeechRecognizer.SetInputToDefaultAudioDevice();
            SpeechRecognizer.SpeechRecognized += SpeechRecognized;
            SpeechRecognizer.RecognizeCompleted += RecognizeCompleted;
        }

        public void Start()
        {
            Console.WriteLine("\n---- Output");
            SpeechRecognizer.RecognizeAsync(RecognizeMode.Multiple);
        }

        public void Stop()
        {
            SpeechRecognizer.RecognizeAsyncStop();
        }

        public void Dispose()
        {
            Stop();
            SpeechRecognizer.Dispose();
        }

        private void SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result.Confidence > 0.75)
            {
                Console.WriteLine("You Said: " + e.Result.Text);
                commandController.HandleCommandAsync(e.Result.Text);
            }
        }

        private void RecognizeCompleted(object sender, RecognizeCompletedEventArgs e)
        {
            if (e.Error != null) Console.WriteLine("Recognize Compleated Error: " + e.Error);
            else Console.WriteLine("Recognize Compleated: " + e.Result.Text);
        }
    }
}

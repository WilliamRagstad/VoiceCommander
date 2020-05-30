using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoiceCommander
{
    enum GrammarConfigAction
    {
        OpenFile,           // Opens a file
        Shell,              // Runs a shellcommand as if it was runned from the terminal
        PowerShell,         // Runs a powershell command as if it was runned from the terminal
        Code,               // Evaluates and runs C# code
        CodeFileCS,           // Evaluates and runs a .cs C# project file
        SayCode,            // Say the evaluated resulting string from C# code to audio output
        SayRandom,          // Say on of a set of random strings to audio output
        Say,                // Say something to audio output
    }
}

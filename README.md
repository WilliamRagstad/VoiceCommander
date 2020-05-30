<div align=center>
 <img alt="VoiceCommander" src="logo.png" width="30%">
 <br>
 <img alt="GitHub release (latest by date)" src="https://img.shields.io/github/v/release/WilliamRagstad/VoiceCommander">
</div>

# VoiceCommander
An expandable tool for voice activated commands with infinite possibilities!

# Usage

## Introduction

**VoiceCommander** is a tool that provides fullt customizable, programmable and extendable real-time voice recognition to command execution. It has support for a voice selector if you have multiple voices installed, and therefore configs in  different languages. Everything is interpreted in **English** at the moment, but if it is requested this can be broadened.

When you start the program, you are first prompted to select a voice synthesizer and voice recognizer if your system has more than 1 installed. Then all [configs](#Configs) will be loaded. After that, you are good to go! Try to say something from the installed configs command palette.

## Configs

All configs are [JSON](https://en.wikipedia.org/wiki/JSON) formatted and follow the structure below:

```json
{
  "Name": "Example Title",
  "Description": "Example configuration created 2020 by John.",
  "Elements": [
    {
      "Phrase": "Phrase",
      "Action": "Action",
      "ActionData": [
        "Arg1",
        "Arg2",
        "..."
      ]
    }
  ],
  "Groups": [
    {
      "Phrase": "Group phrase",
      "Elements": [ ],
      "Groups": [ ]
    }
  ]
}
```

### Elements

Provides a new configuration which holds all data about the voice command.

#### Phrase

Which phrase to be linked with the command. Multiple commands can share phrases which creates a chain of executions.
This must be pronounceable in any language and is combined with any parent group phrases (to create a "sentence").

#### Action

This decides which action to be made at execution time, the currently available actions are:

> #### Say 
> Say something to audio output.
>
> ActionData: `text`, `text`, `...`.

> #### SayRandom 
> Say one of a set of random strings.
>
> ActionData: `text`, `text`, `...`.

> #### OpenFile
> Opens a file on the local system.
>
> ActionData: `Filepath`.

> #### Shell
> Runs a shellcommand as if it was runned from the terminal.
>
> ActionData: `command`, `command`, `...`.

> #### PowerShell
> Runs a powershell command as if it was runned from the terminal.
>
> ActionData: `command`, `command`, `...`.

> #### SayCode 
> Say evaluated C# code result to audio output.
>
> ActionData: `expression`.

> #### Code
> Evaluates and runs C# code.
>
> ActionData: `code line`, `code line`, `...`.

> #### CodeFileCS
> Evaluates and runs a .cs C# project file.
>
> ActionData: `source file`.


#### ActionData

This is essentially the arguments used by the action. Often the **First** argument is most important to provide.

### Groups

Holds child elements and/or groups, together with any phrases (can be left out).

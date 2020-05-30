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
  "Name": "Example Config",
  "Description": "Example configuration created 2020 by William.",
  "Elements": [
    {
      "Phrase": "#Phrase#",
      "Action": "#Action#",
      "ActionData": [
        "#Arg1#",
        "#Arg2#",
        "#...#"
      ]
    }
  ],
  "Groups": [
    "Phrase": "#Group phrase#",
    "Elements": [ ... ],
    "Groups": [ ... ]
  ]
```

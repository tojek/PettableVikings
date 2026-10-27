# Pettable Vikings

You can now pet other vikings!! - on Valheim 1.0.

Look at another player, press the use key, and a heart effect and sound play on their character while a short message shows on your screen. Other players with the mod installed see and hear the effect too.

## Config

Settings are in `com.valheim.pettablevikings.cfg`, created the first time you run the game. They can also be changed live with BepInEx Configuration Manager.

| Setting | Default | What it does |
| --- | --- | --- |
| `1_Enabled` | `true` | Turns the mod on or off. |
| `2_HoverText` | `Pet @viking` | Prompt shown when you look at a player. |
| `3_Message` | `@viking loves you` | Message shown on your screen after petting. |
| `4_Distance` | `2.0` | Maximum distance in meters. |
| `5_Cooldown` | `1.0` | Seconds before you can pet again. |
| `1_ShowHearts` | `true` | Show the heart particles. |
| `2_PlaySound` | `true` | Play the pet sound. |

`@viking` in the hover text or message is replaced with the other player's name. You can change both texts to anything, such as `@viking Smells funny...`

## Installation

Install with Gale or another Thunderstore-compatible mod manager. BepInExPack Valheim is pulled in as a dependency.

Manual install: install [BepInExPack Valheim](https://thunderstore.io/c/valheim/p/denikson/BepInExPack_Valheim/), then put `PettableVikings.dll` in `BepInEx/plugins/`.

## Notes

The mod runs on the client. You only see the effects on players who also have it installed. Dedicated servers work without the mod installed on the server.

Source and issues: https://github.com/tojek/PettableVikings

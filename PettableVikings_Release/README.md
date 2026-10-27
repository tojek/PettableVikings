# Pettable Vikings

Have you ever wanted to show some appreciation to your fellow Vikings after a hard day of chopping wood and slaying Trolls? Now you can! **Pettable Vikings** is a fully customizable, multiplayer-ready mod that allows players to "pet" each other in Valheim.

Updated for **Valheim 1.0** (Deep North).

## ✨ Features

* **Multiplayer Synced:** Uses native RPC. When you pet someone, everyone nearby sees the hearts and hears the sound!
* **Fully Customizable Text:** Change the crosshair text and the on-screen message to anything you want (e.g., Hug, High-five, Slap).
* **Dynamic Names:** Use the `@viking` tag in your configuration, and the mod will automatically replace it with the target player's name!
* **Toggleable Effects:** Don't like the pink hearts or the sound? You can easily turn them off in the config.
* **Configurable Limits:** Set your own custom cooldown times and interaction distances.
* **Live Updates:** Fully compatible with BepInEx Configuration Manager. Tweak settings in-game and see changes instantly without restarting!

## ⚙️ Configuration Options

After running the game once, a configuration file (`com.valheim.pettablevikings.cfg`) will be generated. You can change:

### 1 - General
* **1_Enabled:** Toggle the entire mod on or off.
* **2_HoverText:** What shows up on your crosshair. (Default: `Pet @viking`)
* **3_Message:** What shows up on the screen after interaction. (Default: `@viking loves you`)
* **4_Distance:** Maximum interaction range in meters. (Default: `2.0`)
* **5_Cooldown:** Delay between interactions in seconds. (Default: `1.0`)

### 2 - Visuals & Audio
* **1_ShowHearts:** Spawns the lovely pink heart particles. (Default: `true`)
* **2_PlaySound:** Plays the adorable pet sound. (Default: `true`)

*(Pro tip: Use the `@viking` tag in HoverText or Message, and the mod will swap it out dynamically!)*

## 🛠️ Installation

### Using Mod Manager (Recommended)
Install with [Gale](https://hexium.gg/mod-manager) or any Thunderstore-compatible mod manager. BepInExPack Valheim is installed automatically as a dependency.

### Manual Installation
1. Install [BepInExPack Valheim](https://thunderstore.io/c/valheim/p/denikson/BepInExPack_Valheim/).
2. Download the `.zip` file for this mod.
3. Extract `PettableVikings.dll` into your `Valheim/BepInEx/plugins/` folder.

## 🤝 Compatibility
This mod is completely **Client-Side**, but to see the effects on other players, they need to have the mod installed as well. It works perfectly on dedicated servers.
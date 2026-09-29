# Pettable Vikings

Have you ever wanted to show some appreciation to your fellow Vikings after a hard day of chopping wood and slaying Trolls? Now you can! **Pettable Vikings** is a fully customizable, multiplayer-ready mod that allows players to "pet" each other in Valheim.

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
* **Enabled:** Toggle the entire mod on or off.
* **HoverText:** What shows up on your crosshair. (Default: `Pet @viking`)
* **Message:** What shows up on the screen after interaction. (Default: `@viking loves you`)
* **Distance:** Maximum interaction range in meters. (Default: `2.0`)
* **Cooldown:** Delay between interactions in seconds. (Default: `1.0`)

### 2 - Visuals & Audio
* **ShowHearts:** Spawns the lovely pink heart particles. (Default: `true`)
* **PlaySound:** Plays the adorable pet sound. (Default: `true`)

*(Pro tip: Use the `@viking` tag in HoverText or Message, and the mod will swap it out dynamically!)*

## 🛠️ Installation

### Using Mod Manager (Recommended)
Simply install via [r2modman](https://valheim.thunderstore.io/package/ebkr/r2modman/) or the Thunderstore app. All dependencies will be handled automatically!

### Manual Installation
1. Install [BepInExPack Valheim](https://valheim.thunderstore.io/package/denikson/BepInExPack_Valheim/).
2. Download the `.zip` file for this mod.
3. Extract `PettableVikings.dll` into your `Valheim/BepInEx/plugins/` folder.

## 🤝 Compatibility
This mod is completely **Client-Side**, but to see the effects on other players, they need to have the mod installed as well. It works perfectly on dedicated servers.
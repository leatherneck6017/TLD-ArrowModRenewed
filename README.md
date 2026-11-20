![Arrow Mod Renewed Banner](AMRbanner.jpg)

## Overview
* Arrow Mod Renewed updates the classic [Arrow Mod](https://github.com/ttr/tld-ArrowMod) to work seamlessly with modern versions of The Long Dark (v2.58+) and MelonLoader. Customize arrow and arrowhead crafting times, craft arrows anywhere without a workbench (with an adjustable field-crafting time penalty), and unlock a mid to late-game recipe to carve arrow shafts from hardwood.

## Features
* Customizable Crafting Times: Adjust the time required to craft arrowheads and simple arrows.
* Craft Anywhere (with a time penalty): Allows crafting arrows away from a workbench, with an adjustable additional time penalty (15–60 min) per arrow.
* Hardwood Arrow Shafts (mid-late Game): Carve 6 arrow shafts at a workbench, using hardwood (Fir) and a knife, once you reach the required Archery skill level.

## What's changed from the original mod?
* Updated for The Long Dark v2.58+: Migrated to the current versions of MelonLoader and ModSettings.
* Streamlined Crafting Recipes: Removed the birch bark Fletching variants and global 'Use line' requirement so standard feather arrows remain untouched.
* No-Workbench Time Penalty: Added an adjustable time penalty per arrow (15–60 min) when crafting arrows away from a workbench.
* Distinct Crafting Menu Names & Descriptions: Added a custom title and description in the crafting UI (**Arrow Shaft (Hardwood)**) so the modded recipe is easy to tell apart from vanilla items.
* Live Workbench & Setting Updates: Toggling workbench requirements or recipe options now updates immediately in-game without requiring a game restart.
* Cleaner ModSettings UI: Replaced On/Off toggles with Yes/No selectors, with conditional sub-options that appear only when relevant, and included a **Reset to defaults?** option.

## Requirements/Dependencies
* The Long Dark (v2.58+)
* MelonLoader (v0.7.2)
* ModSettings

## Installation
1. Ensure **MelonLoader** and **ModSettings** are installed in your The Long Dark directory.
2. Download `ArrowModRenewed.dll` from the [Latest Release](https://github.com/leatherneck6017/TLD-ArrowModRenewed/releases/latest) page.
3. Place `ArrowModRenewed.dll` into your `TheLongDark\Mods` folder.
4. Launch the game and configure your preferences under **Options > Mod Settings > Arrow Mod Renewed Settings**.

## Configuration Options
| Setting | Default | Recommended | Description |
| :--- | :---: | :---: | :--- |
| Arrowhead crafting time: | 60 min | 20-40 min | Minutes required to craft (2) arrowheads. |
| Arrow crafting time: | 90 min | 30-60 min | Minutes required to craft (1) arrow. |
| Workbench required for arrows? | Yes | Player preference | Whether a workbench is required for crafting arrows (Vanilla is Yes). |
| ... additional arrow crafting time: | 30 min | 15-60 min | Additional time, in minutes, per arrow when crafting without a workbench (15–60). |
| Craft arrow shafts from hardwood? | No | — | Unlocks crafting 6 hardwood arrow shafts from 1 pieace of Fir at a workbench. |
| ... archery skill level needed? | 3 | 3 | Archery skill level required to craft arrow shafts from hardwood (0–5). |
| Reset to defaults? | No | — | Set to Yes and click 'Confirm' to reset all settings above to their default values. |

## Removal
* The mod does not change game or save files. You are free to install/remove at any time. Simply delete `ArrowModRenewed.dll` from your `TheLongDark\Mods` folder.

## Issues
* If you encounter any, please report them on the [GitHub Issues](https://github.com/leatherneck6017/TLD-ArrowModRenewed/issues) page or contact me directly on the [TLD Modding Discord](https://discord.gg/tldmodding) (@leatherneck6017).

## AI Disclaimer
* AI tools were consulted during the development of this mod as part of my learning process for exploring *The Long Dark*'s IL2CPP APIs, Harmony patching, and troubleshooting. All code was tested and verified in-game.

## Credits
* Original mod by [**ttr**](https://github.com/ttr) - updated and maintained by [leatherneck6017](https://github.com/leatherneck6017).
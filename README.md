# School Capacity Balancer Mod
 
Tweaks the capacity of schools to make the number of schools required more sane/realistic.

Exact stat tweaks:
* Elementary School & extension wing capacity and upkeep are doubled
* College extension wing capacity and upkeep are doubled
* Regular university capacity and upkeep are increased by 25%
    

## Installation - Thunderstore
If you wish, you can download and install the mod with a modloader from [Thunderstore.io](https://thunderstore.io/c/cities-skylines-ii/p/Wayzware/SchoolCapacityBalancer/).

## Installation - Manual
1. Install [BepInEx 6 Bleeding Edge build](https://builds.bepinex.dev/projects/bepinex_be) or BepInEx 5. 

   The pre-release version of BepInEx 6 available on their GitHub release page is quite outdated (dated August 2022) and may not support loading this mod. Please download the Bleeding Edge version from [their website](https://builds.bepinex.dev/projects/bepinex_be)

   * Download `BepInEx-Unity.Mono-win-x64-6.0.0-be.674+82077ec.zip` (or a newer version), and unzip all of its contents into the game's installation directory, typically `C:/Program Files (x86)/Steam/steamapps/common/Cities Skylines II`

   * The installation directory should now have the `BepInEx` folder, the `doorstop_config.ini` file, and the `winhttp.dll` file

2. Run the game once, then close it. You can close it when the main menu appears

3. Download the mod from the [release page](https://github.com/Wayzware/EducationBalancer/releases). Make sure you select the download that is compatible with your version of BepInEx. Unzip it into the `Cities Skylines II/BepInEx/plugins` folder.

4. Launch the game, and your mods should be loaded automatically

## Configuration
You can change the stat tweaks yourself if desired, or even add stat tweaks for other school buildings not already modified! (currently restricted to upkeep and student capacity changes only)

You will need to edit the config file located at `C:\Users\YOUR_USERNAME_HERE\AppData\LocalLow\Colossal Order\Cities Skylines II\ModSettings\SchoolCapacityBalancer_Wayz`. **You must run the game with the mod installed at least once for this file to be generated**.

## Compiling the Mod Yourself
You will need to add references to Unity yourself if you wish to compile the project.

In the .csproj, you can set the location of your game install and enable the PostBuild install step, to automatically install the mod after build.

## Thank You
* optimus-code for their [template mod repo](https://github.com/optimus-code/Cities2Modding/tree/main), especially the .csproj
* [slyh](https://github.com/slyh) for their installation instructions
* The creators of Harmony and BepInEx
* The Cities Skylines II modding discord in general

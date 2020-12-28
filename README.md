# BannerlordTestingEssentials
Bannerlord Testing Essentials is a Mount & Blade II: Bannerlord mod testing utility.

**Features**
* Create commands in the game instance and execute them from an independent test
* Retrieve the current game state
* Respond to commands from the game instance


**There are two different components to these tools**

* ### [BannerlordTestingLibrary](https://github.com/Bannerlord-Coop-Team/BannerlordTestingEssentials/wiki/BannerlordTestingLibrary)

  * This component is directly used in the tests you create.

* ### [BannerlordTestingFramework](https://github.com/Bannerlord-Coop-Team/BannerlordTestingEssentials/wiki/BannerlordTestingFramework)

  * This component is used directly with your mod to drive functionality from the game and report it to your tests.

## Installation
Under [Releases](https://github.com/Bannerlord-Coop-Team/BannerlordTestingEssentials/releases) download the BannerlordTestingEssentials and unzip them in your mods working directory. Then you can reference the associated DLLs.

Note: The distribution of Taleworlds assemblies is most likely prohibited, so we are unable to include some required assemblies.<br/>
[BannerlordTestingFramework](https://github.com/Bannerlord-Coop-Team/BannerlordTestingEssentials/wiki/BannerlordTestingFramework) requires references to:
* Taleworlds.MountAndBlade.dll
* Taleworlds.Core.dll
* Taleworlds.ObjectSystem.dll

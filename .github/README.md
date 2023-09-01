# UnityDevConsole
This asset provides an in-game developer console (debug console) for Unity projects, allowing developers or players to execute commands!

**Links:** [Discord](https://discord.gg/t3H9NMFTEc)

# Setup
Simply import the package into your project and you're good to go. No additional setup is required.

Download directly from the releases tab & import in Unity (Assets>Import Package).
Import via the Unity package manager (Window>Package Manager).
Git URL: https://github.com/sexeee/UnityDevConsole.git
<img src="/.github/install.png" alt="Package manager install" width="25%"></src>

#### Custom commands
```cs
    [ConsoleCommandAttribute("addhp", "Add hp to player", "addhp", ParameterType.INT)]
    public static void Add(int parameter)
    {
        hp += parameter;
    }
```
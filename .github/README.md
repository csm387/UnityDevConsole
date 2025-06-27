# 🎮 UnityDevConsole

**UnityDevConsole** is a lightweight, in-game developer console for Unity. It allows developers — or even players — to execute debug commands easily during runtime.

> ✅ No configuration required — just import and go!

---

## 📦 Installation

### Option 1: Unity Package Manager  
1. Open **Unity > Window > Package Manager**  
2. Click the **+** button → choose **"Add package from Git URL…"**  
3. Paste the URL: https://github.com/sexeee/UnityDevConsole.git

### Option 2: Manual Import  
1. Download the latest `.unitypackage` from the [Releases tab](https://github.com/sexeee/UnityDevConsole/releases)  
2. In Unity: **Assets > Import Package > Custom Package…**  
3. Select the downloaded file and import it

<img src="/.github/install.png" alt="Package manager install" width="25%">

---

## 🧪 Usage

Once imported, press <kbd>~</kbd> (tilde) at runtime to open the console and enter commands.

---

## ⚙️ Creating Custom Commands

You can easily register your own functions as console commands using the `[ConsoleCommand]` attribute.

### ✍️ Example:
```csharp
[ConsoleCommand("addhp", "Add hp to player", "addhp", ParameterType.INT)]
public static void AddHp(int value)
{
 hp += value;
 Debug.Log($"Player HP increased by {value}");
}
```

Attribute Field	Description

"addhp"	Command name
"Add hp to player"	Description for help menu
"addhp"	Usage example
ParameterType.INT	Type of parameter (INT, FLOAT, etc.)


---

🧑‍💻 Developer Notes

Works in both Editor and Builds

Built for minimal overhead and high extensibility

Supports primitive parameter types: int, float, bool, string
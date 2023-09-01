using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;
using System.Reflection;
using System;

namespace sexee.DevConsole
{
    public class DevConsole : MonoBehaviour
    {
        public KeyCode showConsoleKey;

        public  bool showConsole = false;
        public  bool IsEnabled = true;

        public GameObject console;
        public TMP_InputField inputField;
        string input;

        public static List<object> commandList;

        static GameObject outputparent;
        static GameObject outputText;
        public GameObject _outputparent;
        public GameObject _outputText;

        public GameObject autoCompleteBg;
        public TMP_Text autoComplete;
        private void Start()
        {
            console.SetActive(showConsole);

            outputparent = _outputparent;
            outputText = _outputText;
        }

        private void Awake()
        {
            AddCommandsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
        }
        public void AddCommandsFromAssemblies(Assembly[] assemblies)
        {
            commandList = new List<object>();

            foreach (var assembly in assemblies)
            {
                foreach (var type in assembly.GetTypes())
                {
                    //object instance = Activator.CreateInstance(type);
                    MethodInfo[] methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
                    foreach (MethodInfo method in methods)
                    {
                        ConsoleCommandAttribute attribute = method.GetCustomAttribute<ConsoleCommandAttribute>();

                        if (attribute != null)
                        {                       
                            if (method.GetParameters().Length == 1 || method.GetParameters().Length == 0)
                            {
                                if (method.IsStatic)
                                {
                                    switch (attribute.Parameter)
                                    {
                                        case ParameterType.NULL:
                                            ConsoleCommand nullcommand = new ConsoleCommand(attribute.Id, attribute.Description, attribute.Format, () =>
                                            {
                                                method.Invoke(null, null);
                                            });

                                            commandList.Add(nullcommand);
                                            break;
                                        case ParameterType.INT:
                                            ConsoleCommand<int> intcommand = new ConsoleCommand<int>(attribute.Id, attribute.Description, attribute.Format, (int parameter) =>
                                            {
                                                method.Invoke(null, new object[] { parameter });
                                            });

                                            commandList.Add(intcommand);
                                            break;
                                        case ParameterType.FLOAT:
                                            ConsoleCommand<float> floatcommand = new ConsoleCommand<float>(attribute.Id, attribute.Description, attribute.Format, (float parameter) =>
                                            {
                                                method.Invoke(null, new object[] { parameter });
                                            });

                                            commandList.Add(floatcommand);
                                            break;
                                        case ParameterType.BOOL:
                                            ConsoleCommand<bool> boolcommand = new ConsoleCommand<bool>(attribute.Id, attribute.Description, attribute.Format, (bool parameter) =>
                                            {
                                                method.Invoke(null, new object[] { parameter });
                                            });

                                            commandList.Add(boolcommand);
                                            break;
                                        case ParameterType.STRING:
                                            ConsoleCommand<string> stringcommand = new ConsoleCommand<string>(attribute.Id, attribute.Description, attribute.Format, (string parameter) =>
                                            {
                                                method.Invoke(null, new object[] { parameter });
                                            });

                                            commandList.Add(stringcommand);
                                            break;
                                    default:
                                        Debug.LogError($"[CONSOLE] Unexpected ParameterType for command with Id {attribute.Id}: {attribute.Parameter}");
                                        break;
                                }
                                }
                                else
                                {
                                    Debug.LogError($"[CONSOLE] Non-static method '{method.Name}' in class '{type.FullName}' cannot be used as a console command. Skipping.");
                                    #region Working... 
                                    //object instance = Activator.CreateInstance(type);
                                    //switch (attribute.Parameter)
                                    //{
                                    //    case ParameterType.NULL:
                                    //        ConsoleCommand nullcommand = new ConsoleCommand(attribute.Id, attribute.Description, attribute.Format, () =>
                                    //        {
                                    //            method.Invoke(instance, null);
                                    //        });

                                    //        commandList.Add(nullcommand);
                                    //        break;
                                    //    case ParameterType.INT:
                                    //        ConsoleCommand<int> intcommand = new ConsoleCommand<int>(attribute.Id, attribute.Description, attribute.Format, (int parameter) =>
                                    //        {
                                    //            method.Invoke(instance, new object[] { parameter });
                                    //        });

                                    //        commandList.Add(intcommand);
                                    //        break;
                                    //    case ParameterType.FLOAT:
                                    //        ConsoleCommand<float> floatcommand = new ConsoleCommand<float>(attribute.Id, attribute.Description, attribute.Format, (float parameter) =>
                                    //        {
                                    //            method.Invoke(instance, new object[] { parameter });
                                    //        });

                                    //        commandList.Add(floatcommand);
                                    //        break;
                                    //    case ParameterType.BOOL:
                                    //        ConsoleCommand<bool> boolcommand = new ConsoleCommand<bool>(attribute.Id, attribute.Description, attribute.Format, (bool parameter) =>
                                    //        {
                                    //            method.Invoke(instance, new object[] { parameter });
                                    //        });

                                    //        commandList.Add(boolcommand);
                                    //        break;
                                    //    case ParameterType.STRING:
                                    //        ConsoleCommand<string> stringcommand = new ConsoleCommand<string>(attribute.Id, attribute.Description, attribute.Format, (string parameter) =>
                                    //        {
                                    //            method.Invoke(instance, new object[] { parameter });
                                    //        });

                                    //        commandList.Add(stringcommand);
                                    //        break;
                                    //    //default:
                                    //    //    Debug.LogError($"[CONSOLE] Unexpected ParameterType for command with Id {attribute.Id}: {attribute.Parameter}");
                                    //    //    break;
                                    //}
                                    #endregion
                                }
                            }
                            else
                            {
                                Debug.LogError("[CONSOLE] Commands don`t support more that one parameter");
                            }
                        }
                    }
                }
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(showConsoleKey) && IsEnabled)
            {
                showConsole = !showConsole;
                console.SetActive(showConsole);
            }

            if (showConsole)
            {
                input = inputField.text;              
            }
        }

        public void HandleInput()
        {
            if (input != "")
            {
                string[] properties = input.Split(' ');
                bool commandFound = false;
                for (int i = 0; i < commandList.Count; i++)
                {
                    ConsoleCommandBase commandBase = commandList[i] as ConsoleCommandBase;
                    if (input.StartsWith(commandBase.CommandId))
                    {
                        commandFound = true;

                        if (commandList[i] as ConsoleCommand != null)
                        {
                            (commandList[i] as ConsoleCommand).Invoke();
                        }
                        else if (commandList[i] as ConsoleCommand<int> != null && properties.Length > 1)
                        {
                            if (int.TryParse(properties[1], out int proprieti))
                            {
                                (commandList[i] as ConsoleCommand<int>).Invoke(proprieti);
                            }
                            else
                            {
                                ErrorLog("Invalid integer value provided: " + properties[1]);
                            }                        
                        }  
                        else if (commandList[i] as ConsoleCommand<string> != null && properties.Length > 1)
                        {
                            (commandList[i] as ConsoleCommand<string>).Invoke(properties[1]);
                        }
                        else if (commandList[i] as ConsoleCommand<bool> != null && properties.Length > 1)
                        {
                            if (bool.TryParse(properties[1], out bool proprieti))
                            {
                                (commandList[i] as ConsoleCommand<bool>).Invoke(proprieti);
                            }
                            else
                            {
                                ErrorLog("Invalid bool value provided: " + properties[1]);
                            }             
                        }
                        else if (commandList[i] as ConsoleCommand<float> != null && properties.Length > 1)
                        {
                            if (float.TryParse(properties[1], out float proprieti))
                            {
                                (commandList[i] as ConsoleCommand<float>).Invoke(proprieti);
                            }
                            else
                            {
                                ErrorLog("Invalid float value provided: " + properties[1]);
                            }
                        }
                    }
                }

                if (!commandFound)
                {
                    WarningLog("Command not found.");
                }

                input = "";
                inputField.text = "";
                autoComplete.text = "";
            }
        }

        public void Autocomplete()
        {
            if (string.IsNullOrEmpty(input)) 
            {
                autoComplete.text = "";
                autoCompleteBg.SetActive(false);
                return; 
            }
            string matchingCommandsText = GetMatchingCommandsText(input);
            autoComplete.text = matchingCommandsText;
        }

        public string GetMatchingCommandsText(string input)
        {
            StringBuilder sb = new StringBuilder();

            if (string.IsNullOrEmpty(input))
            {
                return "No input provided.";
            }

            bool foundMatchingCommands = false;
            autoCompleteBg.SetActive(false);
            for (int i = 0; i < commandList.Count; i++)
            {
                ConsoleCommandBase command = commandList[i] as ConsoleCommandBase;
                if (command.CommandId.Contains(input))
                {
                    foundMatchingCommands = true;
                    sb.AppendLine(command.CommandId + " " + command.CurrentValue); // Append the matching command to the StringBuilder
                }
            }

            if (foundMatchingCommands)
            {
                autoCompleteBg.SetActive(true);
                return sb.ToString();
            }
            else
            {
                autoCompleteBg.SetActive(false);
                return "No matching commands found.";
            }
        }
        [ConsoleCommandAttribute("help", "show all commands", "help", ParameterType.NULL)]
        public static void Help()
        {
            GameObject help = Instantiate(outputText, outputparent.transform);
            string helplabel = "-- HELP --";

            help.GetComponentInChildren<TMP_Text>().text = helplabel;
            help.GetComponentInChildren<TMP_Text>().alignment = TextAlignmentOptions.Center;

            for (int i = 0; i < commandList.Count; i++)
            {
                ConsoleCommandBase commandBase = commandList[i] as ConsoleCommandBase;
                GameObject go = Instantiate(outputText, outputparent.transform);
                string label = $"<color=#fafa37>{commandBase.CommandFormat}</color> <color=#ffffff> - {commandBase.CommandDescription}</color>";

                go.GetComponentInChildren<TMP_Text>().text = label;
            }
        }

        public void Log(object log)
        {
            GameObject go = Instantiate(outputText, outputparent.transform);
            string label = log.ToString();

            go.GetComponentInChildren<TMP_Text>().text = label;
        }
        public void WarningLog(object log)
        {
            GameObject go = Instantiate(outputText, outputparent.transform);
            string label = log.ToString();

            go.GetComponentInChildren<TMP_Text>().text = $"<color=#fafa37>{log}</color>";
        }    
        public void ErrorLog(object log)
        {
            GameObject go = Instantiate(outputText, outputparent.transform);
            string label = log.ToString();

            go.GetComponentInChildren<TMP_Text>().text = $"<color=#FF0000>{log}</color>";
        }

        [ConsoleCommandAttribute("clear", "claer console", "clear", ParameterType.NULL)]
        public static void Clear()
        {
            for (int i = 0; i < outputparent.transform.childCount; i++)
            {
                Destroy(outputparent.transform.GetChild(i).gameObject);
            }
        }
    }
}
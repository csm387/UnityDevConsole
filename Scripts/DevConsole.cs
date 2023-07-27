using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System.Text;

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

        public List<object> commandList;

        public GameObject outputparent;
        public GameObject outputText;
        public GameObject autoCompleteBg;
        public TMP_Text autoComplete;
        public static ConsoleCommand HELP_COMMAND;
        public static ConsoleCommand CLEAR_COMMAND;
        public static ConsoleCommand<string> LOG_COMMAND;
        private void Start()
        {
            console.SetActive(showConsole);
        }

        private void Awake()
        {
            HELP_COMMAND = new ConsoleCommand("help", "Shows a list of commands", "help", () =>
            {
                ShowHelp();
            });
            CLEAR_COMMAND = new ConsoleCommand("clear", "Clear the console", "clear", () =>
            {
                Clear();
            });   
            LOG_COMMAND = new ConsoleCommand<string>("log", "log a message", "log <message>", (value) =>
            {
                    Log(value);
            });   

            commandList = new List<object>
            {
                HELP_COMMAND,
                CLEAR_COMMAND,
                LOG_COMMAND,
            };

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
                        else if (commandList[i] as ConsoleCommand<string> != null && properties.Length > 1)
                        {
                            (commandList[i] as ConsoleCommand<string>).Invoke(properties[1]);
                        }
                        else if (commandList[i] as ConsoleCommand<int> != null && properties.Length == 1)
                        {
                            (commandList[i] as ConsoleCommand<int>).Invoke(int.Parse(properties[1]));
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

            for (int i = 0; i < commandList.Count; i++)
            {
                ConsoleCommandBase command = commandList[i] as ConsoleCommandBase;
                if (command.CommandId.Contains(input))
                {
                    foundMatchingCommands = true;
                    sb.AppendLine(command.CommandId); // Append the matching command to the StringBuilder
                }
            }

            if (foundMatchingCommands)
            {
                return sb.ToString();
            }
            else
            {
                return "No matching commands found.";
            }
        }

        void ShowHelp()
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

        void Log(object log)
        {
            GameObject go = Instantiate(outputText, outputparent.transform);
            string label = log.ToString();

            go.GetComponentInChildren<TMP_Text>().text = label;
        }  
        void WarningLog(object log)
        {
            GameObject go = Instantiate(outputText, outputparent.transform);
            string label = log.ToString();

            go.GetComponentInChildren<TMP_Text>().text = $"<color=#fafa37>{log}</color>";
        }    

        void Clear()
        {
            for (int i = 0; i < outputparent.transform.childCount; i++)
            {
                Destroy(outputparent.transform.GetChild(i).gameObject);
            }
        }
    }
}
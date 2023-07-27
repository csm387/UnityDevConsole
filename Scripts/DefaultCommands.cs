using sexee.DevConsole;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultCommands : MonoBehaviour
{
    public static ConsoleCommand TEST_COMMAND;
    public static ConsoleCommand<string> DEBUG_COMMAND;
    public static ConsoleCommand<int> SET_INT;


    public int testint;
    private void Awake()
    {
        TEST_COMMAND = new ConsoleCommand("test", "test coomand", "test", () =>
        {
            Test();
        });

        DEBUG_COMMAND = new ConsoleCommand<string>("log", "log coomand", "log", (value) =>
        {
            DebugLog(value);
        });    
        SET_INT = new ConsoleCommand<int>("setint", "int coomand", "setint <ammount>", (value) =>
        {
            setInt(value);
        });
    }

    void Test()
    {
        Debug.Log("Test");
    }

    void DebugLog(string value)
    {
        Debug.Log(value);
    }  
    void setInt(int value)
    {
       testint = value;
       Debug.Log(value);
    }
}

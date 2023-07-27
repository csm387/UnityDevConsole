using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsoleCommandBase
{
    private string _commandId;
    private string _commandDescription;
    private string _commandFormat;
    // New property to hold the current value
    private object _currentValue;

    public object CurrentValue
    {
        get { return _currentValue; }
        set { _currentValue = value; }
    }

    public string CommandId { get { return _commandId; } }
    public string CommandDescription { get { return _commandDescription; } }
    public string CommandFormat { get { return _commandFormat; } }

    public ConsoleCommandBase(string id, string description, string format)
    {
        _commandId = id;
        _commandDescription = description;
        _commandFormat = format;
    }
}
public class ConsoleCommand : ConsoleCommandBase
{
    private Action command;

    public ConsoleCommand(string id, string description, string format, Action command) : base(id, description, format)
    {
        this.command = command;
    }

    public void Invoke()
    {
        command.Invoke();
    }
}

public class ConsoleCommand<T1> : ConsoleCommandBase
{
    private Action<T1> command;

    public ConsoleCommand(string id, string description, string format, Action<T1> command) : base(id, description, format)
    {
        this.command = command;
    }

    public void Invoke(T1 value)
    {
        // Set the current value to be used later if needed
        CurrentValue = value;

        command.Invoke(value);
    }
}
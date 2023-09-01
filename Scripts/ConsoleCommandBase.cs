using System;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class ConsoleCommandAttribute : Attribute
{
    public string Id { get; }
    public string Description { get; }
    public string Format { get; }
    public ParameterType Parameter { get; }

    public ConsoleCommandAttribute(string id, string description, string format, ParameterType parameter = ParameterType.NULL)
    {
        Id = id;
        Description = description;
        Format = format;
        Parameter = parameter;
    }
}

public enum ParameterType
{
    NULL,
    INT,
    FLOAT,
    BOOL,
    STRING
}

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
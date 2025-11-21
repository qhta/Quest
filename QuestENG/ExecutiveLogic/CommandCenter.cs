namespace Qhta.MVVM;

/// <summary>
/// Static class enables dependency injection and centralized command management.
/// </summary>
public static class CommandCenter
{
  private record CommandWithParameter(ICommand Command, object? Parameter)
  {
  }

  private static readonly Dictionary<object, CommandWithParameter> _commands = new();

  /// <summary>
  /// Registers a command with the specified ID.
  /// </summary>
  /// <param name="commandID">Identifier of the command (any type)</param>
  /// <param name="command">Command to register</param>
  /// <param name="parameter">Optional command parameter to register</param>
  public static void RegisterCommand(object commandID, ICommand command, object? parameter = null)
  {
    if (!_commands.TryAdd(commandID, new (command, parameter)))
    {
      throw new InvalidOperationException($"A command {commandID} is already registered.");
    }
  }

  /// <summary>
  /// Gets a registered command by ID.
  /// </summary>
  /// <param name="commandID"></param>
  /// <returns></returns>
  public static ICommand? GetCommand(object commandID)
  {
    _commands.TryGetValue(commandID, out var item);
    return item?.Command;
  }

  /// <summary>
  /// Execute a registered command by ID with a given parameter.
  /// First a command CanExecute method is called. If the result is true, then Execute method is invoked.
  /// </summary>
  /// <param name="commandID"></param>
  /// <param name="parameter"></param>
  /// <returns></returns>
  public static void ExecuteCommand(object commandID, object? parameter)
  {
    if (!_commands.TryGetValue(commandID, out var item))
      throw new InvalidOperationException($"A command {commandID} is not registered.");
    var command = item.Command;
    if (command.CanExecute(parameter))
      command.Execute(parameter);
  }

  /// <summary>
  /// Notifies all listeners that a specific command could have changed it's CanExecute state.
  /// </summary>
  /// <param name="commandID"></param>
  /// <exception cref="InvalidOperationException"></exception>
  public static void NotifyCanExecuteChanged(object commandID)
  {
    if (!_commands.TryGetValue(commandID, out var item))
      throw new InvalidOperationException($"A command {commandID} is not registered.");
    var command = item.Command;
    if (command is not Command commandChanged)
      throw new InvalidOperationException($"A command {commandID} does not implement NotifyCanExecuteChanged method.");
    commandChanged.NotifyCanExecuteChanged();

  }

}
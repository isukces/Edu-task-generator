using System.Windows.Input;

namespace XEducation.Modules;

public class RelayCommand : ICommand
{
    public RelayCommand(Action<object?> execute, Func<object?, bool>? canExecute = null)
    {
        _execute    = execute;
        _canExecute = canExecute;
    }

    public static ICommand Disabled { get; } = new RelayCommand(_ => { }, _ => false);

    public bool CanExecute(object? parameter)
    {
        if (_isUnder)
            return false;
        if (_canExecute is null)
            return true;
        return _canExecute(parameter);
    }
    

    public void Execute(object? parameter)
    {
        _isUnder = true;
        CommandManager.InvalidateRequerySuggested();
        try
        {
            _execute(parameter);
        }
        finally
        {
            _isUnder = false;
            CommandManager.InvalidateRequerySuggested();
        }
    }

    EventHandler? _canExecuteChanged;
    public event EventHandler? CanExecuteChanged
    {
        add
        {
            CommandManager.RequerySuggested += value;
            _canExecuteChanged+= value;
        }
        remove
        {
            CommandManager.RequerySuggested -= value;
            _canExecuteChanged              -= value;
        }
    }

    #region Fields

    private readonly Func<object, bool>? _canExecute;
    private readonly Action<object>      _execute;
    private          bool                _isUnder;

    #endregion
}
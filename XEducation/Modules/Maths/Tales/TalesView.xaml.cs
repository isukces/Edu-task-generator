using System.Collections.Immutable;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
//using XEducation.Modules.Biz.Deposits;
using XEducation.Ui;

namespace XEducation.Modules.Maths.Tales;

using Self = TalesView;

public partial class TalesView : IRefreshable, IMultipleTasks, IPrintable, ISolvable
{
    public TalesView()
    {
        printCommand = new RelayCommand(
            _ => { FlowDocumentHelper.TryPrint(GetCurrentReader()); },
            _ => _details.Length > 0 && GetCurrentReader() is not null);
        _solveCommand = new RelayCommand(
            _ => { UpdateSolutionContent(); },
            _ => SecondPage.Content is null && _details.Length > 0);

        Count = 3;
        InitializeComponent();
        _canRefresh = true;
    }

    private static void DependencyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        (d as Self)?.DependencyPropertyChanged(e);
    }

    private void DependencyPropertyChanged(DependencyPropertyChangedEventArgs e)
    {
        UpdateContent();
    }

    private FlowDocumentReader? GetCurrentReader()
    {
        return (MainControl.SelectedItem as TabItem)?.Content as FlowDocumentReader;
    }

    public ICommand GetPrintCommand()
    {
        return printCommand;
    }

    public ICommand GetRefreshCommand()
    {
        return new RelayCommand(_ => { UpdateContent(); });
    }

    public ICommand GetSolveCommand()
    {
        return _solveCommand;
    }

    private void MainControl_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        CommandManager.InvalidateRequerySuggested();
    }

    public void SetTaskCount(int count)
    {
        Count = count;
    }

    private void UpdateContent()
    {
        if (!_canRefresh) return;
        var count = Math.Max(1, Count);

        _details = TalesTask.Create(count);

        var helper = new FlowDocumentHelper();
        FirstPage.Content = new FlowDocumentReader
        {
            Document    = helper.Document,
            ViewingMode = FlowDocumentReaderViewingMode.Scroll
        };

        for (var i = 0; i < count; i++)
        {
            helper.AddHeader($"Zadanie {i + 1}");
            _details[i].FlushTask(helper);
        }

        SecondPage.Content        = null;
        MainControl.SelectedIndex = 0;
        CommandManager.InvalidateRequerySuggested();
    }

    private void UpdateSolutionContent()
    {
        var helper = new FlowDocumentHelper();
        SecondPage.Content = new FlowDocumentReader
        {
            Document    = helper.Document,
            ViewingMode = FlowDocumentReaderViewingMode.Scroll
        };

        var count = _details.Length;
        for (var i = 0; i < count; i++)
        {
            helper.AddHeader($"Rozwiązanie zadania {i + 1}");
            _details[i].FlushTaskSolution(helper);
        }
        MainControl.SelectedIndex = 1;
        CommandManager.InvalidateRequerySuggested();
    }


    public int Count
    {
        get => (int)GetValue(CountProperty);
        set => SetValue(CountProperty, value);
    }

    #region Fields

    public static readonly DependencyProperty CountProperty =
        DependencyProperty.Register(nameof(Count), typeof(int), typeof(Self),
            new PropertyMetadata(10, DependencyPropertyChanged));

    private readonly bool                                   _canRefresh;
    private          ImmutableArray<TalesTask> _details = [];
    private readonly RelayCommand                           _solveCommand;

    private readonly RelayCommand printCommand;

    #endregion
}
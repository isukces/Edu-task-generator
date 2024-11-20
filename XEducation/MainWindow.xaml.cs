using System.Windows;
using System.Windows.Controls.Ribbon;
using System.Windows.Input;
using System.Windows.Media;
using XEducation.Modules;
using XEducation.Modules.Biz.Deposits;
using XEducation.Modules.Maths.Tales;
using XEducation.Modules.PolishLang.Renaissance;

namespace XEducation;

using Self = MainWindow;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : IChallengeOwner
{
    public MainWindow()
    {
        RefreshCommand = RelayCommand.Disabled;
        PrintCommand   = RelayCommand.Disabled;
        SolveCommand   = RelayCommand.Disabled;

        InitializeComponent();
        TaskCount = 5;

        {
            var ints = new[] { 1, 5, 20, 50 };
            foreach (var i in ints)
            {
                var item = new RibbonMenuItem
                {
                    Header = i.ToString(),
                    Tag    = i
                };
                item.Click += NumberItemClick;
                TaskCountSplitButton.Items.Add(item);
            }

            void NumberItemClick(object sender, RoutedEventArgs e)
            {
                if (sender is not RibbonMenuItem { Tag: int selectedNumber })
                    return;
                TaskCount = selectedNumber;
            }
        }

        var challenges = new IEducationTaskInfo[]
        {
            new TalesTaskInfo(),
            new DepositCalculationTaskInfo(),
            new RenaissanceTaskInfo()
        };
        foreach (var challenge in challenges)
            challenge.Register(this);
    }

    private static void DependencyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        (d as Self)?.DependencyPropertyChanged(e);
    }


    void IChallengeOwner.AddButton(string label, ICommand command, ImageSource? iconSrc,
        string hint)
    {
        var button = new RibbonButton
        {
            Label            = label,
            Command          = command,
            LargeImageSource = iconSrc,
            SmallImageSource = iconSrc,
            ToolTip          = hint
        };
        GroupZadania.Items.Add(button);
    }

    private void DependencyPropertyChanged(DependencyPropertyChangedEventArgs e)
    {
        if (e.Property == TaskCountProperty)
        {
            TaskCountSplitButton.Label = $"Ilość zadań: {TaskCount}";
            (Content.Content as IMultipleTasks)?.SetTaskCount(TaskCount);
        }
    }

    void IChallengeOwner.SetContent(object content)
    {
        Content.Content             = content;
        RefreshCommand              = (content as IRefreshable)?.GetRefreshCommand() ?? RelayCommand.Disabled;
        PrintCommand                = (content as IPrintable)?.GetPrintCommand() ?? RelayCommand.Disabled;
        SolveCommand                = (content as ISolvable)?.GetSolveCommand() ?? RelayCommand.Disabled;
        TaskCountSplitButtonEnabled = content is IMultipleTasks;
        (Content.Content as IMultipleTasks)?.SetTaskCount(TaskCount);
    }


    public int TaskCount
    {
        get => (int)GetValue(TaskCountProperty);
        set => SetValue(TaskCountProperty, value);
    }


    public ICommand SolveCommand
    {
        get => (ICommand)GetValue(SolveCommandProperty);
        set => SetValue(SolveCommandProperty, value);
    }


    public ICommand PrintCommand
    {
        get => (ICommand)GetValue(PrintCommandProperty);
        set => SetValue(PrintCommandProperty, value);
    }

    public ICommand RefreshCommand
    {
        get => (ICommand)GetValue(RefreshCommandProperty);
        set => SetValue(RefreshCommandProperty, value);
    }


    public bool TaskCountSplitButtonEnabled
    {
        get => (bool)GetValue(TaskCountSplitButtonEnabledProperty);
        set => SetValue(TaskCountSplitButtonEnabledProperty, value);
    }

    #region Fields

    public static readonly DependencyProperty SolveCommandProperty =
        DependencyProperty.Register(nameof(SolveCommand), typeof(ICommand), typeof(Self));

    public static readonly DependencyProperty PrintCommandProperty =
        DependencyProperty.Register(nameof(PrintCommand), typeof(ICommand), typeof(Self));

    public static readonly DependencyProperty TaskCountSplitButtonEnabledProperty =
        DependencyProperty.Register(nameof(TaskCountSplitButtonEnabled), typeof(bool), typeof(Self));

    public static readonly DependencyProperty TaskCountProperty =
        DependencyProperty.Register(nameof(TaskCount), typeof(int), typeof(Self),
            new PropertyMetadata(DependencyPropertyChanged));

    public static readonly DependencyProperty RefreshCommandProperty =
        DependencyProperty.Register(nameof(RefreshCommand), typeof(ICommand), typeof(Self));

    #endregion
}
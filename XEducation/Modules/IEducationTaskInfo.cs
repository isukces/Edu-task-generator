using System.Windows.Input;
using System.Windows.Media;

namespace XEducation.Modules;

public interface IEducationTaskInfo
{
    void Register(IChallengeOwner challenge);
}

public interface IChallengeOwner
{
    void AddButton(string content, ICommand command, ImageSource? iconSrc, string hint);

    void SetContent(object content);
}

public interface IRefreshable
{
    ICommand GetRefreshCommand();
}

public interface IPrintable
{
    ICommand GetPrintCommand();
}

public interface IMultipleTasks
{
    void SetTaskCount(int count);
}

public interface ISolvable
{
    ICommand GetSolveCommand();
}
using System.Windows.Input;
using System.Windows.Media;
using MahApps.Metro.IconPacks;
using XEducation.Modules.Biz.Deposits;
using XEducation.Ui;

namespace XEducation.Modules.Maths.Tales;

public class TalesTaskInfo : IEducationTaskInfo
{
    public void Register(IChallengeOwner challenge)
    {
        ICommand command = new RelayCommand(_ =>
        {
            challenge.SetContent(new TalesView());
        });
        var iconSrc = PackIconImageSourceConverter.ConvertIconKind(PackIconVaadinIconsKind.Lines, Brushes.DarkBlue); 
        challenge.AddButton("Tales", command, iconSrc, "Zadania z twierdzeniem Talesa");
    }
}
using System.Windows.Input;
using System.Windows.Media;
using MahApps.Metro.IconPacks;
using XEducation.Modules.Biz.Deposits;
using XEducation.Ui;

namespace XEducation.Modules.PolishLang.Renaissance;

public class RenaissanceTaskInfo : IEducationTaskInfo
{
    public void Register(IChallengeOwner challenge)
    {
        ICommand command = new RelayCommand(_ => { challenge.SetContent(new RenaissanceView()); });
        var iconSrc =
            PackIconImageSourceConverter.ConvertIconKind(PackIconVaadinIconsKind.Book, Brushes.CornflowerBlue);
        challenge.AddButton("Renesans", command, iconSrc, "Literatura renesansu");
    }
}

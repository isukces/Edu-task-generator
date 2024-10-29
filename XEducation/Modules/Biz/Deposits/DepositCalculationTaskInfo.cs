using System.Windows.Input;
using System.Windows.Media;
using MahApps.Metro.IconPacks;
using XEducation.Ui;

namespace XEducation.Modules.Biz.Deposits;

public class DepositCalculationTaskInfo : IEducationTaskInfo
{
    public void Register(IChallengeOwner challenge)
    {
        ICommand command = new RelayCommand(_ =>
        {
            challenge.SetContent(new DepositView());
        });
        var iconSrc = PackIconImageSourceConverter.ConvertIconKind(PackIconPhosphorIconsKind.PiggyBankThin, Brushes.LimeGreen); 
        challenge.AddButton("Lokaty", command, iconSrc, "Nauka obliczania odsetek od lokat");
    }
}
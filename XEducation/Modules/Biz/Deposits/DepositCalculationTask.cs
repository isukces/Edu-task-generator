using System.Collections.Immutable;
using System.Windows.Documents;
using System.Xml.Linq;
using iSukces.Translation;
using XEducation.Ui;

namespace XEducation.Modules.Biz.Deposits;

public class DepositCalculationTask:DepositCalculationTaskBase
{
    public static ImmutableArray<DepositCalculationTask> Create(int count)
    {
        var b = ImmutableArray.CreateBuilder<DepositCalculationTask>(count);
        for (var i = 0; i < count; i++)
            b.Add(Random());
        return b.ToImmutable();
    }

    private static string MonthsNoun(int count)
    {
        return PolishGrammar.NounForm(count, "miesiąc", "miesiące", "miesięcy");
    }

    private static DepositCalculationTask Random()
    {
        var t = new DepositCalculationTask
        {
            InterestRates        = [..GetInterestRates()],
            CapitalizationMonths = RandomHelper.FromOptions(1, 2, 3, 4, 6, 12),
            Tax                  = RandomHelper.FromOptions(2m, 19m, 32m),
            Currency             = RandomHelper.FromOptions("zł", "€", "$"),
            StartingValue        = RandomHelper.RangeIncludingInt(1, 200) * 1000
        };
        t.Length = RandomHelper.RangeIncludingInt(2, 3) * t.CapitalizationMonths;
        {
            var a = t.GetRates(0, t.Length - 1).ToArray();
            a[^1]           = a[^1] with { Months = 0 };
            t.InterestRates = [..a];
        }
        return t;

        IEnumerable<InterestRate> GetInterestRates()
        {
            var rates = GetInterestRates2().OrderByDescending(a => a).ToArray();
            var count = rates.Length;
            for (var i = 0; i < count; i++)
            {
                var rate = rates[i];
                if (i == count - 1)
                    yield return new InterestRate(rate, 0);
                else
                    yield return new InterestRate(rate, RandomHelper.RangeIncludingInt(1, 5));
            }
        }

        IEnumerable<decimal> GetInterestRates2()
        {
            var count = RandomHelper.RangeIncludingInt(1, 4);
            var set   = new HashSet<decimal>();
            while (set.Count < count)
            {
                var rate = RandomHelper.RangeIncludingInt(1, 84) * 0.12m;
                set.Add(rate);
            }

            return set;
        }
    }

    private void AddInterestRatesList(FlowDocumentHelper helper)
    {
        if (InterestRates.Length < 2) return;
        var list  = helper.AddList();
        var first = true;
        foreach (var rate in InterestRates)
        {
            var rate1     = rate.GetText(false, first);
            var paragraph = new Paragraph();
            paragraph.Inlines.AddRange(HtmlHelper.HtmlToRuns(Bold(rate1.Value), " " + rate1.Description));
            list.ListItems.Add(new ListItem(paragraph));
            first = false;
        }
    }



    public void FlushTask(FlowDocumentHelper helper)
    {
        helper.AddParagraphX(GetStartingText());
        AddInterestRatesList(helper);
        helper.AddParagraphX(
            "Kapitalizacja następuje co ",
            Number(CapitalizationMonths, 0),
            $" {MonthsNoun(CapitalizationMonths)}. ",
            "Długość lokaty to ",
            Number(Length, 0),
            $" {MonthsNoun(Length)}."
        );
    }

    public void FlushTaskSolution(FlowDocumentHelper helper)
    {
        helper.AddParagraphX("Kwota początkowa lokaty: ", 
            Number(StartingValue, 2),
            Currency,
            Br(),
            "Podatek od lokat: ", Number(Tax, 1), "%",
            Br(),
            "Długość lokaty: ", Number(Length, 0), " " + MonthsNoun(Length),
            Br(),
            "Kapitalizacja: ", Number(CapitalizationMonths,0), " " + MonthsNoun(CapitalizationMonths)
        );

        if (InterestRates.Length > 1)
        {
            helper.AddParagraph(new Run("Oprocentowanie:"));
            AddInterestRatesList(helper);
        }
        else
        {
            var rate = InterestRates[0].GetText(true, true);
            helper.AddParagraphX("Oprocentowanie: ", Bold(rate.Value), " " + rate.Description);
        }

        var periods = Length / CapitalizationMonths;
        var current = StartingValue;
        for (var okres = 0; okres < periods; okres++)
        {
            helper.AddParagraphX(Underline($"Okres {okres + 1}:"));

            List<object> solutionItems = new();

            decimal oprocentowanie;
            {
                var rates = GetRates(okres * CapitalizationMonths, (okres + 1) * CapitalizationMonths - 1).ToArray();
                
                oprocentowanie = rates.Select(a => a.ValuePercent * a.Months / 12m).Sum();
                oprocentowanie = Math.Round(oprocentowanie, 5);
                Append(GetItems1());

                IEnumerable<object> GetItems1()
                {
                    yield return "Oprocentowanie za okres = ";
                    for (var index = 0; index < rates.Length; index++)
                    {
                        if (index > 0)
                            yield return " + ";
                        var rate = rates[index];
                        yield return NumberTrim(rate.ValuePercent, 2);
                        yield return "% / ";
                        yield return Number(12, 0);
                        yield return Times;
                        yield return Number(rate.Months, 0);
                    }

                    yield return " = ";
                    yield return NumberTrim(oprocentowanie, 5);
                    yield return "%";
                }
            }
            decimal kwotaOdsetek;
            {
                kwotaOdsetek = Math.Round(current * oprocentowanie * 0.01m, 2);
                Append(
                        "kwota odsetek = ",
                        Number(current, 2),
                        Currency,
                        Times,
                        NumberTrim(oprocentowanie, 5),
                        "% = ", 
                        Bold(kwotaOdsetek.ToString("N2")), 
                        Currency);
            }
            decimal podatek;
            {
                podatek = Math.Round(kwotaOdsetek * Tax * 0.01m, 2);
                Append(
                        "wartość podatku = ",
                        Number(kwotaOdsetek, 2),
                        Currency,
                        Times,
                        NumberTrim(Tax, 5),
                        "% = ", 
                        Number(podatek, 2), 
                        Currency);
            }
            {
                var kwotaKapitalizacji = kwotaOdsetek - podatek;
                Append(
                    "kwota kapitalizacji = ",
                    Number(kwotaOdsetek, 2), Currency,
                    " - ",
                    Number(podatek, 2), Currency,
                    " = ",
                    Number(kwotaKapitalizacji, 2), Currency);

                var before = current;
                current += kwotaKapitalizacji;
                Append(
                    "Kwota lokaty = ",
                    Number(before, 2), Currency + " + ",
                    Number(kwotaKapitalizacji, 2), Currency + " = ",
                    Bold(current.ToString("N2")), Currency);
            }
            helper.AddParagraphX(solutionItems.ToArray());
            continue;

            void Append(params object[] items)
            {
                if (solutionItems.Count > 0)
                    solutionItems.Add(Br());
                solutionItems.AddRange(items);
            }
        }

        
    }

    /*private string FormatedValue()
    {
        return $"{StartingValue:N2} {Currency}";
    }*/


    private IEnumerable<InterestRate> GetRates(int start, int end)
    {
        var tmp   = GetRates().Skip(start).Take(end - start + 1).ToArray();
        var count = 0;
        var last  = decimal.MinValue;
        foreach (var r in tmp)
        {
            if (count == 0 || last != r)
            {
                if (count > 0)
                    yield return new InterestRate(last, count);
                last  = r;
                count = 1;
                continue;
            }

            count++;
        }

        if (count > 0)
            yield return new InterestRate(last, count);
    }


    private IEnumerable<decimal> GetRates()
    {
        foreach (var interestRate in InterestRates)
        {
            var m = interestRate.Months;
            if (m > 0)
                for (var i = 0; i < m; i++)
                    yield return interestRate.ValuePercent;
            else
                while (true)
                    yield return interestRate.ValuePercent;
        }
    }

    private IEnumerable<object> GetStartingText()
    {
        yield return
            "Oblicz wartość końcową lokaty, wartości kolejnych kapitalizacji i kolejnych podatków dla lokaty o wartości ";
        yield return Number(StartingValue, 2);
        yield return Currency;

        if (InterestRates.Length == 1)
        {
            yield return " z oprocentowaniem ";
            var rate = InterestRates[0].GetText(true, true);
            yield return Bold(rate.Value);
            yield return $" {rate.Description}.";
        }
        else
        {
            yield return " z oprocentowaniem:";
        }
    }

    private XElement Underline(string text)
    {
        return new XElement("u", text);
    }

    public string Currency { get; set; }

    public decimal StartingValue { get; set; }

    public decimal Tax { get; set; }

    public int Length { get; set; }

    public int CapitalizationMonths { get; set; }

    public ImmutableArray<InterestRate> InterestRates { get; set; }

    public record InterestRate(decimal ValuePercent, int Months)
    {
        public (string Value, string Description) GetText(bool isSingle, bool isFirst)
        {
            var perc = $"{ValuePercent:N2}%";

            if (Months > 0)
            {
                var adj  = PolishGrammar.NounForm(Months, "pierwszy", "pierwsze", "pierwszych");
                var noun = MonthsNoun(Months);
                if (isFirst)
                    return (perc, $"przez {adj} {Months} {noun}");
                return (perc, $"przez {Months} {noun}");
            }


            return isSingle
                ? (perc, "przez cały okres")
                : (perc, "przez resztę okresu");
        }
    }
}
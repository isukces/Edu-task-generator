using System.Xml.Linq;

namespace XEducation.Modules.Biz.Deposits;

public class DepositCalculationTaskBase
{
    public XElement Bold(object text)
    {
        return new XElement("b", text);
    }

    public XElement Br()
    {
        return new XElement("br");
    }

    public XElement Number(string text)
    {
        return new XElement("b", text);
    }

    public XElement Number(decimal numer, int deci)
    {
        var text = numer.ToString($"N{deci}");
        return Number(text);
    }

    public XElement NumberTrim(decimal numer, int deci)
    {
        var text = numer.ToString($"N{deci}").TrimEnd('0').TrimEnd(',');
        return Number(text);
    }

    public string Times => " × ";
}
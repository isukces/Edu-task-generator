using System.Xml.Linq;

namespace XEducation.Modules.Biz.Deposits;

public class DepositCalculationTaskBase
{
    
    public XElement Number(string text)
    {
        //return new XElement("font", text, new XAttribute("name", "Times New Roman"));
        return new XElement("b", text);
    }
    
    public XElement Number(Decimal numer, int deci)
    {
        var text = numer.ToString($"N{deci}");
        return Number(text);
    }
    
    
    public XElement NumberTrim(Decimal numer, int deci)
    {
        var text = numer.ToString($"N{deci}").TrimEnd('0').TrimEnd(',');
        return Number(text);
    }

    public string Times => " × "; 
    
    public XElement Bold(object text)
    {
        return new XElement("b", text);
    }

    public XElement Br()
    {
        return new XElement("br");
    }
}
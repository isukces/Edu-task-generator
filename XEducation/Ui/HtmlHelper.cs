using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Xml.Linq;

namespace XEducation.Ui;

class HtmlHelper
{
    public static Inline[] HtmlToRuns(IEnumerable<object> nodes)
    {
        var el = new XElement("root", nodes);
        Debug.WriteLine(el);
        return HtmlToRuns(el);
    }
public static Inline[] HtmlToRuns(params object[] nodes)
    {
        var el = new XElement("root", nodes);
        return HtmlToRuns(el);
    }

    public static Inline[] HtmlToRuns(string x)
    {
        return HtmlToRuns(XElement.Parse($"<root>{x}</root>"));
    }

    public static Inline[] HtmlToRuns(XElement x)
    {
        var sb   = new StringBuilder();
        var bold = false;
        var underline = false;
        var italic = false;
        string fontName = "";
        var list = new List<Inline>();
        Add(x);
        Flush();
        return list.ToArray();

        void Add(XElement el)
        {
            foreach (var n in el.Nodes())
                if (n is XText t)
                {
                    Append(t.Value);
                }
                else if (n is XElement ee)
                {
                    switch (ee.Name.LocalName)
                    {
                        case "b":
                            Set1(ref bold, "bold", ee);
                            break;
                        case "u":
                            Set1(ref underline, "underline", ee);
                            break;
                        case "i":
                            Set1(ref italic, "italic", ee);
                            break;
                        case "br":
                            Flush();
                            list.Add(new LineBreak());
                            break;
                        case "font":
                            string name = ee.Attribute("name")?.Value ?? "";
                            if (fontName == name)
                            {
                                Add(ee);
                            }
                            else
                            {
                                var backup = fontName;
                                fontName = name;
                                Flush();
                                Add(ee);
                                Flush();
                                fontName = backup;
                            }
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                }
        }

        void Set1(ref bool flag, string flagName, XElement ee)
        {
            if (flag)
                throw new Exception("Nested " + flagName);
            Flush();
            flag = true;
            Add(ee);
            Flush();
            flag = false;
        }

        void Append(string text)
        {
            sb.Append(text);
        }

        void Flush()
        {
            if (sb.Length == 0)
                return;
            var r                            = new Run(sb.ToString());
            if (bold) r.FontWeight           = FontWeights.Bold;
            if (underline) r.TextDecorations = TextDecorations.Underline;
            if (italic) r.FontStyle = FontStyles.Italic;
            if (!string.IsNullOrEmpty(fontName)) r.FontFamily = new FontFamily(fontName);
            list.Add(r);
            sb.Clear();
        }
    }
}
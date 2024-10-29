using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace XEducation.Ui;

public class FlowDocumentHelper
{
    public FlowDocumentHelper(FlowDocument document)
    {
        Document = document;
    }

    public FlowDocumentHelper()
    {
        Document = new FlowDocument
        {
            PageHeight  = 1056,
            PageWidth   = 816,
            ColumnWidth = double.PositiveInfinity,
            PagePadding = new Thickness(50)
        };
    }

    public static void TryPrint(FlowDocumentReader? content)
    {
        var document = content?.Document;
        if (document is null)
            return;


        // Drukujemy dokument
        var paginator = ((IDocumentPaginatorSource)document).DocumentPaginator;
        // Tworzymy nowy PrintDialog
        var printDialog = new PrintDialog
        {
            MinPage              = 1,
            MaxPage              = (uint)paginator.PageCount,
            UserPageRangeEnabled = true,
            SelectedPagesEnabled = true,
            CurrentPageEnabled   = true
        };


        // Sprawdzamy, czy użytkownik kliknął "Drukuj"
        if (printDialog.ShowDialog() == true)
            // Ustawienie domyślnego rozmiaru strony na A4
            printDialog.PrintDocument(paginator, "Drukowanie FlowDocument");
    }

    public void AddBlock(Block block)
    {
        Document.Blocks.Add(block);
    }

    public T AddBlock<T>() where T : Block, new()
    {
        var p = new T();
        AddBlock(p);
        p.FontFamily = new FontFamily(UiSettings.FontFamilyFlowDocument);
        return p;
    }

    public void AddHeader(string text)
    {
        Document.Blocks.Add(new Paragraph(new Run(text))
        {
            FontSize = 18,
            //FontWeight = FontWeights.Bold,
            TextDecorations = TextDecorations.Underline,
            FontStyle       = FontStyles.Italic,
            FontFamily      = new FontFamily(UiSettings.FontFamily)
        });
    }

    public List AddList()
    {
        return AddBlock<List>();
    }

    public Paragraph AddParagraph(params Run[] runs)
    {
        var p = AddBlock<Paragraph>();
        p.Inlines.AddRange(runs);
        return p;
    }
    public Paragraph AddParagraphX(IEnumerable<object> objects)
    {
        var p = AddBlock<Paragraph>();
        p.Inlines.AddRange(HtmlHelper.HtmlToRuns(objects));
        return p;
    }

    public Paragraph AddParagraphX(params object[] objects)
    {
        var p = AddBlock<Paragraph>();
        p.Inlines.AddRange(HtmlHelper.HtmlToRuns(objects));
        return p;
    }

    public FlowDocument Document { get; }
}
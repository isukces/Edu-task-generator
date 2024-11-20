using System.Collections.Immutable;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Xml.Linq;
using XEducation.Ui;

namespace XEducation.Modules.PolishLang.Renaissance;

internal class RenaissanceTask
{
    public static ImmutableArray<RenaissanceTask> Create(int count)
    {
        count += 10000;
        var                   all    = LoadAll().ToList();
        List<RenaissanceTask> result = [];
        while (count > 0 && all.Count > 0)
        {
            var i = Random.Shared.Next(0, all.Count);
            result.Add(all[i]);
            all.RemoveAt(i);
            count--;
        }

        return [..result];
    }

    private static IEnumerable<RenaissanceTask> LoadAll()
    {
        var                    assembly = typeof(RenaissanceTask).Assembly;
        var                    a        = assembly.GetManifestResourceNames();
        RenaissanceTaskBuilder taskB    = new();
        foreach (var i in a)
        {
            if (!IsValidSource(i)) continue;
            var lines = Load(i);
            foreach (var line in lines)
            {
                if (taskB.Add(line, out var task))
                    yield return task!;
            }

            if (taskB.Finish(out var task1))
                yield return task1!;
        }

        yield break;

        bool IsValidSource(string name)
        {
            return name.StartsWith(
                       "XEducation.Modules.PolishLang.Renaissance.Resources.part",
                       StringComparison.Ordinal)
                   && name.EndsWith(".md", StringComparison.Ordinal);
        }

        string[] Load(string s)
        {
            using var stream = assembly.GetManifestResourceStream(s);
            if (stream is null)
                return [];
            using var ms = new MemoryStream();
            stream.CopyTo(ms);
            var text = Encoding.UTF8.GetString(ms.ToArray());
            return text.Split('\n', '\r')
                .Select(a => a.Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToArray();
        }
    }

    public void FlushTask(FlowDocumentHelper helper, int number, bool isAsnswer)
    {
        helper.AddHeader($"{number}. {Question}");
        var p = helper.AddBlock<List>();
        p.MarkerStyle = TextMarkerStyle.LowerLatin;
        for (var index = 0; index < Answers.Count; index++)
        {
            var answer = Answers[index];
            var run    = new Run(answer.Text);
            if (answer.IsCorrect && isAsnswer)
                run.FontWeight = FontWeights.Bold;

            ListItem listItem  = new();
            var      paragraph = new Paragraph();
            listItem.Blocks.Add(paragraph);
            if (isAsnswer)
            {
                /*var c = new InlineUIContainer
                {
                    Child = answer.IsCorrect
                        ? new TextBlock { Text = "✓", Foreground = Brushes.Green }
                        : new TextBlock { Text = "✗", Foreground = Brushes.Red }
                };
                // <iconPacks:PackIconCoolicons Kind="SquareCheck" />
                // <iconPacks:PathIconCoolicons Kind="CheckboxCheck" />*/
                var c = answer.IsCorrect
                    ? new Run { Text = "✓ ", Foreground = Brushes.Green }
                    : new Run { Text = "✗ ", Foreground = Brushes.Red };
                paragraph.Inlines.Add(c);
                paragraph.Inlines.Add(run);
            }
            else
            {
                paragraph.Inlines.Add(run);
            }

            p.ListItems.Add(listItem);
        }
    }


    public void ShuffleAnswers()
    {
        var answers = Answers.ToList();
        Answers.Clear();
        while (answers.Count > 1)
        {
            var i = Random.Shared.Next(0, answers.Count);
            var a = answers[i];
            answers.RemoveAt(i);
            Answers.Add(a);
        }

        Answers.AddRange(answers);
    }

    #region Properties

    public required string       Question { get; init; }
    public          List<Answer> Answers  { get; } = new();

    #endregion
}

public record Answer(string Text, bool IsCorrect);

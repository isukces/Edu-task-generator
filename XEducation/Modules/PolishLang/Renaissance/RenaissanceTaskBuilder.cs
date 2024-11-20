using System.Text.RegularExpressions;

namespace XEducation.Modules.PolishLang.Renaissance;

internal class RenaissanceTaskBuilder
{
    private static T Swap<T>(ref T field, T value)
    {
        var old = field;
        field = value;
        return old;
    }

    public bool Add(string line, out RenaissanceTask? task)
    {
        task = Add(line);
        task?.ShuffleAnswers();
        return task != null;
    }

    private RenaissanceTask? Add(string line)
    {
        var m = QuestionRegex.Match(line);
        if (m.Success)
            return Swap(ref _task, new RenaissanceTask
            {
                Question = Strip(m.Groups[1].Value)
            });

        m = AnswerRegex.Match(line);
        if (m.Success)
        {
            if (_task is null)
                throw new InvalidOperationException("No question");
            _task.Answers.Add(new Answer(Strip(m.Groups[1].Value), false));
            return null;
        }

        m = CorrectAnswersRegex.Match(line);
        if (m.Success)
        {
            if (_task is null)
                throw new InvalidOperationException("No question");
            var list = m.Groups[1].Value.Trim().Split(',')
                .Select(a => a.Trim())
                .Where(a => a.Length > 0)
                .Select(a => a.ToLower()[0] - 'a')
                .Distinct()
                .ToList();
            foreach (var i in list)
            {
                _task.Answers[i] = _task.Answers[i] with { IsCorrect = true };
            }

            return Swap(ref _task, null);
        }

        return null;
    }

    const string StripMarkdownStarsFilter = @"(\*+)([^*]+)(\*+)";
    static Regex StripMarkdownStarsRegex = new Regex(StripMarkdownStarsFilter, RegexOptions.IgnoreCase | RegexOptions.Compiled);

    private static string Strip(string text)
    {
        return StripMarkdownStarsRegex.Replace(text.Trim(), m =>
        {
            var s1 = m.Groups[1].Value;
            var s2 = m.Groups[3].Value;
            if (s1 != s2)
                throw new InvalidOperationException("Invalid markdown");
                //return m.Groups[0].Value;
            var value = m.Groups[2].Value;
            if (s1 == "*")
            {
                const char quote = '\"';
                return quote + value.TrimStart(quote).TrimEnd(quote) + quote;
            }

            return value;

        });
    }

    public bool Finish(out RenaissanceTask? task)
    {
        task = Swap(ref _task, null);
        task?.ShuffleAnswers();
        return task != null;
    }

    #region Fields

    private const string QuestionFilter = @"^####\s+\d+\.(.*)$";


    private const string AnswerFilter = @"^[abcd]\)(.*)$";
    private const string CorrectAnswersFilter = @"^\**Prawidłowa odpowiedź:\**(.*)$";

    private RenaissanceTask? _task;

    private static readonly Regex QuestionRegex =
        new Regex(QuestionFilter, RegexOptions.IgnoreCase | RegexOptions.Compiled);

    private static readonly Regex
        AnswerRegex = new Regex(AnswerFilter, RegexOptions.IgnoreCase | RegexOptions.Compiled);

    private static readonly Regex CorrectAnswersRegex =
        new Regex(CorrectAnswersFilter, RegexOptions.IgnoreCase | RegexOptions.Compiled);

    #endregion
}

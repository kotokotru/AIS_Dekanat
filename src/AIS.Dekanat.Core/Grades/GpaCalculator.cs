namespace AIS.Dekanat.Core.Grades;

/// <summary>
/// Расчёт среднего балла (GPA) студента за сессию.
/// GPA — средневзвешенная пятибалльная оценка по зачётным единицам.
/// Незачёты без оценки (Credit) в GPA не входят, по каждой дисциплине берётся последняя попытка.
/// </summary>
public sealed class GpaCalculator
{
    public decimal Calculate(IEnumerable<GradeRecord> grades)
    {
        ArgumentNullException.ThrowIfNull(grades);

        var graded = LastAttempts(grades)
            .Where(g => g.Type != AssessmentType.Credit)
            .ToList();

        if (graded.Count == 0)
            return 0m;

        var totalCredits = graded.Sum(g => g.Credits);
        if (totalCredits <= 0m)
            throw new InvalidOperationException("Сумма зачётных единиц должна быть больше нуля.");

        var weighted = graded.Sum(g => BrsScale.ToMark(g.Points) * g.Credits);
        return Math.Round(weighted / totalCredits, 2, MidpointRounding.AwayFromZero);
    }

    public static IEnumerable<GradeRecord> LastAttempts(IEnumerable<GradeRecord> grades) =>
        grades
            .GroupBy(g => g.Discipline)
            .Select(group => group.MaxBy(g => g.Attempt)!);
}

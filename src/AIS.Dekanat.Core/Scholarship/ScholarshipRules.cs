using AIS.Dekanat.Core.Grades;

namespace AIS.Dekanat.Core.Scholarship;

/// <summary>
/// Право на академическую стипендию по итогам сессии:
/// все дисциплины сданы без пересдач, нет оценок «удовлетворительно».
/// Повышенная стипендия — если все оценки «отлично».
/// </summary>
public sealed class ScholarshipRules
{
    public ScholarshipLevel Evaluate(IEnumerable<GradeRecord> grades)
    {
        ArgumentNullException.ThrowIfNull(grades);
        var all = grades.ToList();
        if (all.Count == 0)
            return ScholarshipLevel.None;

        if (all.Any(g => g.Attempt > 1))
            return ScholarshipLevel.None;

        if (all.Any(g => !BrsScale.IsPassed(g.Points)))
            return ScholarshipLevel.None;

        var marks = all
            .Where(g => g.Type != AssessmentType.Credit)
            .Select(g => BrsScale.ToMark(g.Points))
            .ToList();

        if (marks.Any(m => m < 4))
            return ScholarshipLevel.None;

        return marks.Count > 0 && marks.All(m => m == 5)
            ? ScholarshipLevel.Increased
            : ScholarshipLevel.Regular;
    }
}

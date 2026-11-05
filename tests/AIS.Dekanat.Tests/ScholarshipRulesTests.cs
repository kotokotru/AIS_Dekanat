using AIS.Dekanat.Core.Grades;
using AIS.Dekanat.Core.Scholarship;

namespace AIS.Dekanat.Tests;

public class ScholarshipRulesTests
{
    private readonly ScholarshipRules _rules = new();

    [Fact]
    public void AllExcellent_GivesIncreasedScholarship()
    {
        var grades = new[]
        {
            new GradeRecord("Базы данных", 6m, AssessmentType.Exam, 92m),
            new GradeRecord("Математика", 4m, AssessmentType.Exam, 88m),
            new GradeRecord("Физкультура", 2m, AssessmentType.Credit, 70m)
        };

        Assert.Equal(ScholarshipLevel.Increased, _rules.Evaluate(grades));
    }

    [Fact]
    public void GoodAndExcellent_GivesRegularScholarship()
    {
        var grades = new[]
        {
            new GradeRecord("Базы данных", 6m, AssessmentType.Exam, 92m),
            new GradeRecord("Математика", 4m, AssessmentType.Exam, 75m)
        };

        Assert.Equal(ScholarshipLevel.Regular, _rules.Evaluate(grades));
    }

    [Fact]
    public void SatisfactoryMark_NoScholarship()
    {
        var grades = new[]
        {
            new GradeRecord("Базы данных", 6m, AssessmentType.Exam, 92m),
            new GradeRecord("Математика", 4m, AssessmentType.Exam, 60m)
        };

        Assert.Equal(ScholarshipLevel.None, _rules.Evaluate(grades));
    }

    [Fact]
    public void Retake_NoScholarship_EvenIfPassedOnExcellent()
    {
        var grades = new[]
        {
            new GradeRecord("Математика", 4m, AssessmentType.Exam, 40m, Attempt: 1),
            new GradeRecord("Математика", 4m, AssessmentType.Exam, 95m, Attempt: 2)
        };

        Assert.Equal(ScholarshipLevel.None, _rules.Evaluate(grades));
    }

    [Fact]
    public void FailedCredit_NoScholarship()
    {
        var grades = new[]
        {
            new GradeRecord("Базы данных", 6m, AssessmentType.Exam, 95m),
            new GradeRecord("Физкультура", 2m, AssessmentType.Credit, 30m)
        };

        Assert.Equal(ScholarshipLevel.None, _rules.Evaluate(grades));
    }
}

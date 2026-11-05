using AIS.Dekanat.Core.Grades;

namespace AIS.Dekanat.Tests;

public class GpaCalculatorTests
{
    private readonly GpaCalculator _calculator = new();

    [Theory]
    [InlineData(100, 5)]
    [InlineData(86, 5)]
    [InlineData(85, 4)]
    [InlineData(71, 4)]
    [InlineData(70, 3)]
    [InlineData(56, 3)]
    [InlineData(55, 2)]
    [InlineData(0, 2)]
    public void BrsScale_ConvertsPointsOnBoundaries(int points, int expectedMark)
    {
        Assert.Equal(expectedMark, BrsScale.ToMark(points));
    }

    [Fact]
    public void BrsScale_PointsOutOfRange_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => BrsScale.ToMark(101m));
    }

    [Fact]
    public void Calculate_WeightsMarksByCredits()
    {
        var grades = new[]
        {
            new GradeRecord("Базы данных", 6m, AssessmentType.Exam, 90m),
            new GradeRecord("Математика", 4m, AssessmentType.Exam, 75m),
            new GradeRecord("История", 2m, AssessmentType.GradedCredit, 60m)
        };

        Assert.Equal(4.33m, _calculator.Calculate(grades));
    }

    [Fact]
    public void Calculate_IgnoresCreditsWithoutMark()
    {
        var grades = new[]
        {
            new GradeRecord("Программирование", 5m, AssessmentType.Exam, 95m),
            new GradeRecord("Физкультура", 2m, AssessmentType.Credit, 60m)
        };

        Assert.Equal(5.00m, _calculator.Calculate(grades));
    }

    [Fact]
    public void Calculate_UsesLastRetakeAttempt()
    {
        var grades = new[]
        {
            new GradeRecord("Математика", 4m, AssessmentType.Exam, 40m, Attempt: 1),
            new GradeRecord("Математика", 4m, AssessmentType.Exam, 72m, Attempt: 2)
        };

        Assert.Equal(4.00m, _calculator.Calculate(grades));
    }

    [Fact]
    public void Calculate_RoundsHalfAwayFromZero()
    {
        var grades = new[]
        {
            new GradeRecord("А", 1m, AssessmentType.Exam, 90m),
            new GradeRecord("Б", 1m, AssessmentType.Exam, 80m),
            new GradeRecord("В", 1m, AssessmentType.Exam, 80m),
            new GradeRecord("Г", 1m, AssessmentType.Exam, 80m),
            new GradeRecord("Д", 1m, AssessmentType.Exam, 80m),
            new GradeRecord("Е", 1m, AssessmentType.Exam, 80m),
            new GradeRecord("Ж", 1m, AssessmentType.Exam, 80m),
            new GradeRecord("З", 1m, AssessmentType.Exam, 80m)
        };

        // (5 + 7 * 4) / 8 = 4.125 -> 4.13
        Assert.Equal(4.13m, _calculator.Calculate(grades));
    }

    [Fact]
    public void Calculate_EmptySession_ReturnsZero()
    {
        Assert.Equal(0m, _calculator.Calculate(Array.Empty<GradeRecord>()));
    }
}

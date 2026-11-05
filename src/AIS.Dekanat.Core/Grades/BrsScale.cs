namespace AIS.Dekanat.Core.Grades;

/// <summary>Перевод баллов БРС в пятибалльную оценку по положению о БРС.</summary>
public static class BrsScale
{
    public const decimal PassThreshold = 56m;

    public static int ToMark(decimal points)
    {
        if (points < 0m || points > 100m)
            throw new ArgumentOutOfRangeException(nameof(points), points, "Балл БРС должен быть от 0 до 100.");

        return points switch
        {
            >= 86m => 5,
            >= 71m => 4,
            >= PassThreshold => 3,
            _ => 2
        };
    }

    public static bool IsPassed(decimal points) => points >= PassThreshold;
}

namespace AIS.Dekanat.Core.Grades;

/// <summary>Результат студента по дисциплине из БРС.</summary>
/// <param name="Discipline">Название дисциплины.</param>
/// <param name="Credits">Трудоёмкость в зачётных единицах.</param>
/// <param name="Type">Форма контроля.</param>
/// <param name="Points">Рейтинговый балл БРС (0–100).</param>
/// <param name="Attempt">Номер попытки: 1 — основная сдача, 2 и далее — пересдачи.</param>
public sealed record GradeRecord(
    string Discipline,
    decimal Credits,
    AssessmentType Type,
    decimal Points,
    int Attempt = 1);

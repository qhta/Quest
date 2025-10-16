namespace Quest;

/// <summary>
/// Observable collection of <see cref="QualityGrade"/> objects.
/// </summary>
public class QualityScale : ObservableList<QualityGrade>
{
  /// <summary>
  /// Initializing constructor.
  /// </summary>
  public QualityScale() : base([])
  {
  }

  //public void Add(QualityGrade item)
  //{
  //  if (item is QualityGrade grade)
  //   Items = Items.Add(grade);
  //  else
  //    throw new ArgumentException("Item must be of type QualityGrade", nameof(item));
  //}
}
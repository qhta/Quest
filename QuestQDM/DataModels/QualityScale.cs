namespace Quest;

/// <summary>
/// Observable collection of <see cref="QualityGrade"/> objects.
/// </summary>
public class QualityScale : ObservableList<QualityGrade>
{
  /// <summary>
  /// Initializing constructor.
  /// </summary>
  /// <param name="items">Collection of entities to add their view models.</param>
  public QualityScale(IEnumerable<QualityGrade> items) : base(items)
  {
  }
}
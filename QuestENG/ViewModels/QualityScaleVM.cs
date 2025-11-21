namespace Quest;

/// <summary>
/// Observable collection of <see cref="QualityGradeVM"/> objects.
/// </summary>
public class QualityScaleVM : ObservableList<QualityGradeVM>, IChangeable
{
  /// <summary>
  /// Parent view model
  /// </summary>
  public ProjectQualityVM Parent { get; set; }

  /// <summary>
  /// Initializes a new instance of the <see cref="QualityScaleVM"/> class with a list of <see cref="QualityGrade"/>.
  /// </summary>
  /// <param name="parent">Parent view model</param>
  /// <param name="items">Collection of entities to add their view models.</param>
  public QualityScaleVM(ProjectQualityVM parent,IEnumerable<QualityGrade> items) : base(items.Select(item => new QualityGradeVM(item)))
  {
    Parent = parent;
  }

  /// <summary>
  /// Finds a grade by its text representation.
  /// </summary>
  /// <param name="name"></param>
  /// <returns></returns>
  /// <exception cref="KeyNotFoundException"></exception>
  public QualityGradeVM? GetGradeByName(string name) => this.FirstOrDefault(g => g.Text == name);

  /// <summary>
  /// Gets or sets a value indicating whether any item in the collection has been modified.
  /// </summary>
  /// <remarks>Setting this property to false updates the <c>IsChanged</c> state of all items in the collection to false.</remarks>
  public bool? IsChanged
  {
    get => this.Any(g => g.IsChanged == true);
    set { if (value == false) this.ForEach(g => g.IsChanged = value); }
  }
}
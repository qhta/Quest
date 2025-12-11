using System.ComponentModel;

namespace Quest;

/// <summary>
/// ViewModel for a quality factor assessment
/// </summary>
public class QualityMetricsVM : QualityNodeVM<QualityMetrics>, IQualityNodeVM
{
  /// <summary>
  /// Mandatory constructor
  /// </summary>
  /// <param name="parent">Required parent View Model</param>
  /// <param name="collection">Collection to which this node belongs.</param>
  /// <param name="model">Required data entity</param>
  public QualityMetricsVM(IQualityObjectVM parent, IList collection, QualityMetrics model) : base(model)
  {
    Parent = parent;
    Collection = collection;
    Children = new QualityNodeVMCollection(this, model.Children?.Cast<QualityNode>() ?? []);
    Children.PropertyChanged += Children_PropertyChanged;
  }

  private void Children_PropertyChanged(object? sender, PropertyChangedEventArgs e)
  {
    if (e.PropertyName == nameof(Children.Value))
      Value = Children.Value;
  }


  /// <summary>
  /// Access to the model's factor type
  /// </summary>
  public override QualityFactorType? FactorType
  {
    get
    {
      object? parent = Parent as IQualityNodeVM;
      while (parent is IQualityNodeVM node && parent is not QualityFactorVM)
        parent = node.Parent;
      if (parent is QualityFactorVM factorVM)
        return factorVM.FactorType;
      return null;
    }
    set { }
  }

  /// <summary>
  /// Gets a display name for the metrics.
  /// </summary>
  public override string? DisplayName
  {
    [DebuggerStepThrough]
    get
    {
      var text = Model.Text;
      var name = Model.Name;
      if (name != null && name != text)
        text += $" ({name})";
      return text;
    }
  }

  /// <summary>
  /// Gets the background color for display purposes.
  /// </summary>
  public override string? BackgroundColor
  {
    get
    {
      var colors = FactorType?.Colors?.Split(',', ';');
      if (colors == null || !colors.Any())
        return null;
      if (colors.Count() > Level)
      {
        return colors[Level - 1];
      }
      return colors[colors.Count() - 1];
    }
  }

  /// <summary>
  /// Gets the collection of child nodes associated with this node.
  /// </summary>
  /// <remarks>This property provides access to the hierarchical structure of nodes. It is read-only and cannot
  /// be null.</remarks>
  public QualityNodeVMCollection Children { get; }

  /// <summary>
  /// Evaluates the value of the children collection.
  /// </summary>
  /// <returns>double value or null if evaluation is not possible</returns>
  public override double? Evaluate()
  {
    if (Children.Count != 0)
    {
      Value = Children.EvaluateValue(true);
      return Value;
    }
    Value = null;
    return null;
  }

}

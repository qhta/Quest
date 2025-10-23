namespace Quest;

/// <summary>
/// Quality metrics node in a hierarchical structure of quality nodes.
/// </summary>
[XmlContentProperty("Children")]
public class QualityMetricsNode: QualityNode
{
  /// <summary>
  /// Gets or sets the collection of child nodes associated with this node.
  /// </summary>
  public QualityNodeCollection? Children
  {
    get => _Children;
    set
    {
      if (_Children != value)
      {
        _Children = value;
        if (_Children != null)
        {
          _Children.Parent ??= this;
          foreach (var child in _Children)
            child.Parent = this;
          _Children.CollectionChanged += Children_CollectionChanged;
        }
      }
    }
  }

  private QualityNodeCollection? _Children;

  /// <summary>
  /// Evaluated value computed from child nodes.
  /// </summary>
  public double? Value { get; set; }

  public void Add(QualityNode childNode)
  {
    if (Children == null)
    {
      Children = new (this);
      Children.CollectionChanged += Children_CollectionChanged;
    }
    Children.Add(childNode);
  }

  private void Children_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
  {
    if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add && e.NewItems != null)
    {
      foreach (QualityNode node in e.NewItems)
      {
        node.Parent = this;
      }
    }
  }
}
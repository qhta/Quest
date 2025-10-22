namespace Quest.DataModels;

/// <summary>
/// Observable collection of <see cref="QualityFactorType"/> objects.
/// </summary>
[XmlCollection]
[HideInheritedMembers]
public class QualityFactorTypeCollection : ObservableList<QualityFactorType>
{
  /// <summary>
  /// Default constructor.
  /// </summary>
  public QualityFactorTypeCollection() : base([])
  {
    // ReSharper disable once VirtualMemberCallInConstructor
    CollectionChanged += QualityFactorTypeCollection_CollectionChanged;
  }

  /// <summary>
  /// Initializing constructor.
  /// </summary>
  /// <param name="items"></param>
  public QualityFactorTypeCollection(IEnumerable<QualityFactorType> items): base(items)  
  {
    // ReSharper disable once VirtualMemberCallInConstructor
    CollectionChanged += QualityFactorTypeCollection_CollectionChanged;
  }

  private void QualityFactorTypeCollection_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
  {
    if (e.Action == NotifyCollectionChangedAction.Add)
    {
      if (e.NewItems != null)
        foreach (var item in e.NewItems.OfType<QualityFactorType>())
        {
          if (item.Id == 0)
          {
            var maxId = this.Any() ? this.Max(g => g.Id) : 0;
            item.Id = maxId + 1;
          }
        }
    }
  }
}

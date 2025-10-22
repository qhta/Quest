namespace Quest;

/// <summary>
/// Observable collection of <see cref="QualityGrade"/> objects.
/// </summary>
[XmlCollection]
[HideInheritedMembers]
public class QualityScale : ObservableList<QualityGrade>
{
  /// <summary>
  /// Default.
  /// </summary>
  public QualityScale() : base([])
  {
    // ReSharper disable once VirtualMemberCallInConstructor
    CollectionChanged += QualityScale_CollectionChanged;
  }

  private void QualityScale_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
  {
    if (e.Action == NotifyCollectionChangedAction.Add)
    {
      if (e.NewItems != null)
        foreach (var item in e.NewItems.OfType<QualityGrade>())
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
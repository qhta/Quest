using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Quest;

/// <summary>
/// ViewModel for a project quality assessment
/// </summary>
public class ProjectQualityVM : ViewModel<ProjectQuality>, IQualityObjectVM
{
  /// <summary>
  /// Mandatory constructor
  /// </summary>
  /// <param name="model"></param>
  public ProjectQualityVM(ProjectQuality model) : base(model)
  {
    RootNode = [this];
    ViewItems = new QuestItemsCollection(this);
    Scale = new QualityScaleVM(this, model.Scale ?? []);
    FactorTypes = new QualityFactorTypeVMCollection(this, model.FactorTypes ?? []);
    FactorTypes.CollectionChanged += FactorTypes_CollectionChanged;
    DocumentQualities = new DocumentQualityVMCollection(this, model.DocumentQualities?.Cast<DocumentQuality>() ?? []);
    foreach (var documentQualityVM in DocumentQualities)
    {
      if (FactorTypes != null)
        ViewItems.Add(new QuestItemViewModel { Header = documentQualityVM.DocumentType, Content = documentQualityVM });
    }
    DocumentQualities.CollectionChanged += DocumentQualities_CollectionChanged;
  }

  ///// <summary>
  ///// Gets the quality object associated with the model.
  ///// </summary>
  //public QualityObject QualityObject => Model;

  /// <summary>
  /// Filename associated with the project quality.
  /// </summary>
  public string? FileName { get; set; }



  private bool InClear;
  private void FactorTypes_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
  {
    if (sender == FactorTypes)
    {
      if (e.Action == NotifyCollectionChangedAction.Add)
        foreach (var qualityFactorTypeVM in e.NewItems?.OfType<QualityFactorTypeVM>() ?? [])
        {
          FactorTypes?.Add(new QualityFactorTypeVM(qualityFactorTypeVM.Model));
        }
      else if (e.Action == NotifyCollectionChangedAction.Remove)
        foreach (var qualityFactorTypeVM in e.OldItems?.OfType<QualityFactorTypeVM>() ?? [])
        {
          var toRemove = FactorTypes?.FirstOrDefault(qft => qft.Model == qualityFactorTypeVM.Model);
          if (toRemove != null)
            FactorTypes?.Remove(toRemove);
        }
      else if (e.Action == NotifyCollectionChangedAction.Reset)
      {
        if (!InClear)
        {
          InClear = true;
          FactorTypes?.Clear();
        }
      }
    }
  }

  private void DocumentQualities_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
  {
    if (sender == DocumentQualities && FactorTypes != null)
    {
      if (e.Action == NotifyCollectionChangedAction.Add)
        foreach (var documentQualityVM in e.NewItems?.OfType<DocumentQualityVM>() ?? [])
        {
          documentQualityVM.BindFactorTypes(FactorTypes);
          ViewItems.Add(new QuestItemViewModel { Header = documentQualityVM.DocumentType, Content = documentQualityVM });
        }
    }
  }

  /// <summary>
  /// Root node for the quality tree (always this instance)
  /// </summary>
  public IEnumerable<ProjectQualityVM> RootNode { get; }

  /// <summary>
  /// Project title from the model
  /// </summary>
  public string? ProjectTitle
  {
    [DebuggerStepThrough]
    get => Model.ProjectTitle;
    set
    {
      if (Model.ProjectTitle != value)
      {
        Model.ProjectTitle = value;
        NotifyPropertyChanged(nameof(ProjectTitle));
      }
    }
  }

  /// <summary>
  /// Individual grade scale definition within the project
  /// </summary>
  public QualityScaleVM? Scale
  {
    [DebuggerStepThrough]
    get => _scale;
    set
    {
      if (_scale != value)
      {
        _scale = value;
        NotifyPropertyChanged(nameof(Scale));
        if (ViewItems == null)
          ViewItems = new QuestItemsCollection(this);
        if (ViewItems.FirstOrDefault()?.Content is QualityScaleVM)
          ViewItems.RemoveAt(0);
        ViewItems.Add(new QuestItemViewModel { Header = QuestViewStrings.Scale, Content = value });
      }
    }
  }
  private QualityScaleVM? _scale = null!;

  /// <summary>
  /// Collection of quality factor types used in the project.
  /// </summary>
  public QualityFactorTypeVMCollection? FactorTypes
  {
    [DebuggerStepThrough]
    get => _qualityTypes;
    set
    {
      if (_qualityTypes != value)
      {
        _qualityTypes = value;
        NotifyPropertyChanged(nameof(FactorTypes));
      }
    }
  }
  private QualityFactorTypeVMCollection? _qualityTypes = null!;

  /// <summary>
  /// Individual document qualities within the project.
  /// </summary>
  public DocumentQualityVMCollection? DocumentQualities
  {
    [DebuggerStepThrough]
    get => _documentQualities;
    set
    {
      if (_documentQualities != value)
      {
        _documentQualities = value;
        NotifyPropertyChanged(nameof(DocumentQualities));
      }
    }
  }
  private DocumentQualityVMCollection? _documentQualities = null!;


  /// <summary>
  /// Evaluates the value of the children collection.
  /// </summary>
  /// <returns>double value or null if evaluation is not possible</returns>
  public double? EvaluateValue()
  {
    if (DocumentQualities != null && DocumentQualities.Count != 0)
    {
      return DocumentQualities.EvaluateValue(true);
    }
    return null;
  }

  /// <summary>
  /// Determines whether the project quality is expanded (when used in TreeView).
  /// </summary>
  public bool IsExpanded
  {
    [DebuggerStepThrough]
    get => _isExpanded;
    set
    {
      if (_isExpanded != value)
      {
        _isExpanded = value;
        NotifyPropertyChanged(nameof(IsExpanded));
      }
    }
  }
  private bool _isExpanded;

  #region Loading State Properties
  /// <summary>
  /// Determines whether the workbook is currently in a loading state.
  /// </summary>
  public bool IsLoading
  {
    [DebuggerStepThrough]
    get => _isLoading;
    set
    {
      if (_isLoading != value)
      {
        _isLoading = value;
        NotifyPropertyChanged(nameof(IsLoading));
      }
    }
  }
  private bool _isLoading;

  /// <summary>
  /// Determines the count of worksheets to load.
  /// </summary>
  public int TotalCount
  {
    [DebuggerStepThrough]
    get => _totalCount;
    set
    {
      if (_totalCount != value)
      {
        _totalCount = value;
        NotifyPropertyChanged(nameof(TotalCount));
      }
    }
  }
  private int _totalCount;

  /// <summary>
  /// Currently loaded worksheets count
  /// </summary>
  public int LoadedCount
  {
    [DebuggerStepThrough]
    get => _loadedCount;
    set
    {
      if (_loadedCount != value)
      {
        _loadedCount = value;
        NotifyPropertyChanged(nameof(LoadedCount));
      }
    }
  }
  private int _loadedCount;
  #endregion

  /// <summary>
  /// Collection of quest items. First item is quality scale, followed by phase qualities and document qualities.
  /// </summary>
  public QuestItemsCollection? ViewItems { get; private set; }

  /// <summary>
  /// Evaluates whether all items are loaded.
  /// </summary>
  public new bool IsLoaded
  {
    [DebuggerStepThrough]
    get => _IsLoaded;
    set
    {
      if (_IsLoaded != value)
      {
        _IsLoaded = value;
        NotifyPropertyChanged(nameof(IsLoaded));
        EvaluateValue();
        IsChanged = false;
      }
    }
  }
  private bool _IsLoaded;

  ///  <inheritdoc/>
  public override bool? IsChanged
  {
    get => base.IsChanged == true || Scale?.IsChanged == true || FactorTypes?.IsChanged == true;
    set
    {
      base.IsChanged = value;
      if (Scale != null)
        Scale.IsChanged = value;
      if (FactorTypes != null)
        FactorTypes.IsChanged = value;
    }
  }
  /// <summary>
  /// Gets the quality object associated with the model.
  /// </summary>
  public QualityObject QualityObject => Model;
};

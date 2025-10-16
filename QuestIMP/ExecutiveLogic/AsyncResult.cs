namespace QuestIMP;

/// <summary>
/// Class representing the result of an asynchronous operation.
/// </summary>
public class AsyncResult : IAsyncResult
{
  /// <summary>
  /// Gets the user-defined object that qualifies or contains information about an asynchronous operation.  
  /// </summary>
  public object AsyncState { get; }

  /// <summary>
  /// Gets a <see cref="WaitHandle"/> that can be used to wait for the asynchronous operation to complete.
  /// </summary>
  /// <remarks>The returned <see cref="WaitHandle"/> can be used in scenarios where blocking is required to wait
  /// for the completion of an asynchronous operation. It is the caller's responsibility to ensure proper disposal of
  /// the <see cref="WaitHandle"/> if necessary.</remarks>
  public WaitHandle AsyncWaitHandle => null!;

  /// <summary>
  /// Gets a value indicating whether the operation completed synchronously.
  /// </summary>
  public bool CompletedSynchronously => false;

  /// <summary>
  /// Gets a value indicating whether the operation has been completed.
  /// </summary>
  public bool IsCompleted { get; }

  /// <summary>
  /// Initializes a new instance of the <see cref="AsyncResult"/> class.
  /// </summary>
  /// <param name="isCompleted"></param>
  /// <param name="asyncState"></param>
  public AsyncResult(bool isCompleted, object asyncState)
  {
    IsCompleted = isCompleted;
    AsyncState = asyncState;
  }
}
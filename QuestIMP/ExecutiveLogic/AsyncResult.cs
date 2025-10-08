namespace QuestIMP;

public class AsyncResult : IAsyncResult
{
    public object AsyncState { get; }
    public WaitHandle AsyncWaitHandle => null!;
    public bool CompletedSynchronously => false;
    public bool IsCompleted { get; }

    public AsyncResult(bool isCompleted, object asyncState)
    {
        IsCompleted = isCompleted;
        AsyncState = asyncState;
    }
}
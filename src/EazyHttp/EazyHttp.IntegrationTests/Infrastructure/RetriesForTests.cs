namespace EazyHttp.IntegrationTests.Infrastructure;

public interface IRetriesState
{
  int Attempts { get; }

  void IncrementAttempts();
  void ResetAttempts();
}

internal sealed class RetriesState : IRetriesState
{
  private int _attempts = 0;

  public int Attempts => Volatile.Read(ref _attempts);

  public void IncrementAttempts()
  {
    _ = Interlocked.Increment(ref _attempts);
  }

  public void ResetAttempts()
  {
    _ = Interlocked.Exchange(ref _attempts, 0);
  }
}

using UnityEngine;

public interface IPlayerAction 
{
    int Priority { get; }
    bool Processing { get; }
    bool LockMove { get; }
    bool TryStart(IActorIntent a);
    void Begin();
    void Tick();
    void Cancel();
}

using UnityEngine;

public interface IActorIntent
{
    Vector2 Move { get; }
    bool Run { get; }
    bool JumpPressed { get; }
    bool AttackPressed { get; }
}

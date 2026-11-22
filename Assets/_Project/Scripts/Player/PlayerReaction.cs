using System;
using UnityEngine;

public class PlayerReaction : MonoBehaviour
{
    private Health health;
    public event Action Attacked;
    public event Action Dying;
    private PlayerAnimation playerAnimation;

    private void Awake()
    {
        playerAnimation = GetComponentInChildren<PlayerAnimation>();
        health = GetComponent<Health>();
    }
    private void OnEnable()
    {
        health.Damaged += Damaged;
        health.Died += Died;
        health.Damaged += playerAnimation.PlayerAttacked;
        health.Died += playerAnimation.PlayerDied;
    }
    private void OnDisable()
    {
        health.Damaged -= Damaged;
        health.Died -= Died;
        health.Damaged -= playerAnimation.PlayerAttacked;
        health.Died -= playerAnimation.PlayerDied;
    }
    private void Damaged() => Attacked?.Invoke();
    private void Died() => Dying?.Invoke();
}

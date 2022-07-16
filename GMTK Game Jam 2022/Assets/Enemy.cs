using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] int maxHp;
    [SerializeField] int currentHp;

    public int MaxHp { get => maxHp; }
    public int CurrentHp { get => currentHp; }

    public void InflictDamage(int dmg)
    {
        currentHp -= dmg;
        if (currentHp <= 0) Die();
    }

    public void Die()
    {
        throw new System.NotImplementedException();
    }
}

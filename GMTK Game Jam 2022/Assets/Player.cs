using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int maxHP;
    int currentHp;

    public List<PlayableDie> diceInventory;

    public void InflictDamage(int dmg)
    {
        currentHp -= dmg;
        if (currentHp <= 0) Die();
    }

    public int Heal(int healNum)
    {
        int healAmount = Mathf.Min(healNum, maxHP - currentHp);
        currentHp += healAmount;
        return healAmount;
    }

    public void AddDie(PlayableDie die)
    {
        diceInventory.Add(die);
    }

    public void Die()
    {
        throw new System.NotImplementedException();
    }
}

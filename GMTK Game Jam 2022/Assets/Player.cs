using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    
    public int maxHP;
    public int currentHP;
    public int energyLevel = 1;
    public int maxEnergyLevel = 1;

    public List<PlayableDie> diceInventory;

    public void InflictDamage(int dmg)
    {
        currentHP -= dmg;
        if (currentHP <= 0) Die();
    }

    public int Heal(int healNum)
    {
        int healAmount = Mathf.Min(healNum, maxHP - currentHP);
        currentHP += healAmount;
        CombatManager.Instance.UpdateCombatReportText("You healed for " + healAmount + "HP");
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

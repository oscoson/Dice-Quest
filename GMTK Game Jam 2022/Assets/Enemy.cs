using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected int maxHp;
    [HideInInspector] protected int currentHp;

    public int MaxHp { get => maxHp; }
    public int CurrentHp { get => currentHp; }

    private void Start()
    {
        currentHp = maxHp;
        //EnemyHealthBar.Instance.Init(currentHp, maxHp);
    }

    public void InflictDamage(int dmg)
    {
        currentHp -= dmg;
        CombatManager.Instance.combatReport.text = "You dealed " + dmg + " damage!";
        //EnemyHealthBar.Instance.currentHealth = currentHp;
        if (currentHp <= 0) Die();
    }

    public void Die()
    {
        Debug.Log("Die");
        CombatManager.Instance.combatReport.text = "Good job!";
        CombatManager.Instance.EndCombat();
        Destroy(gameObject);
    }
}

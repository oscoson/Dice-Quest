using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    
    public int maxHP;
    public int currentHP;
    public int energyLevel = 3;
    public int maxEnergyLevel = 3;

    public List<PlayableDie> diceInventory;
    private List<PlayableDie> initialInventory;
    private ShakeUI shaker;

    private void Awake()
    {
        shaker = FindObjectOfType<ShakeUI>();
        for (int i = 0; i < diceInventory.Count; i++)
        {
            GameObject go = Instantiate(diceInventory[i].gameObject, transform);
            diceInventory[i] = go.GetComponent<PlayableDie>();
        }
        initialInventory = new(diceInventory);
    }

    public void Init()
    {
        currentHP = maxHP;
        diceInventory = new List<PlayableDie>(initialInventory);
        diceInventory.ForEach( d => d.ResetValues());
    }

    public void InflictDamage(int dmg)
    {
        currentHP -= dmg;
        CombatManager.Instance.playAudio.Play("DamagePlayer");
        StartCoroutine(shaker.Shake(0.15f, 4f));
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
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                 Application.Quit();
        #endif
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    
    public int maxHP;
    public int currentHP;
    public int energyLevel = 3;
    public int maxEnergyLevel = 3;
    public float blockValue = 0;

    public List<PlayableDie> diceInventory;
    private ShakeUI shaker;

    private void Awake()
    {
        shaker = FindObjectOfType<ShakeUI>();
        for (int i = 0; i < diceInventory.Count; i++)
        {
            GameObject go = Instantiate(diceInventory[i].gameObject, transform);
            diceInventory[i] = go.GetComponent<PlayableDie>();
        }

    }

    public int InflictDamage(int dmg)
    {
        int finaldmg = (int)(dmg * (1 - blockValue));
        Debug.Log(finaldmg);
        currentHP -= finaldmg;
        blockValue = 0;
        CombatManager.Instance.playAudio.Play("DamagePlayer");
        StartCoroutine(shaker.Shake(0.15f, 4f));
        if (currentHP <= 0) Die();
        return finaldmg;
    }

    public int Heal(int healNum)
    {
        int healAmount = Mathf.Min(healNum, maxHP - currentHP);
        currentHP += healAmount;
        CombatManager.Instance.UpdateCombatReportText("You healed for " + healAmount + "HP");
        return healAmount;
    }
    
    public void Block()
    {
        blockValue += 0.2f;
        CombatManager.Instance.UpdateCombatReportText("You prepare to block " + (blockValue * 100) + "% of damage");
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

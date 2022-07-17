using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        if (blockValue > 1)
        {
            blockValue = 1;
        }
        int finaldmg = (int)(dmg * (1 - blockValue));
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
        CombatManager.Instance.UpdateCombatReportText("You rolled to heal " + healAmount + " HP!");
        return healAmount;
    }
    
    public void Block(int minBlockVal, int maxBlockVal)
    {
        blockValue += Random.Range(minBlockVal * 0.01f, maxBlockVal * 0.01f);
        CombatManager.Instance.UpdateCombatReportText("You rolled to block " + Mathf.Round(blockValue * 100) + "% of damage!");
    }

    public void ExtraTurn()
    {
        CombatManager.Instance.UpdateCombatReportText("TIME WARPS! You take an extra turn!");
        StartCoroutine(CombatManager.Instance.PlayerWaitingTime(1.2f));
    }

    public void AddDie(PlayableDie die)
    {
        diceInventory.Add(die);
    }

    public void Die()
    {
        SceneManager.LoadScene(0);
        //#if UNITY_EDITOR
                //UnityEditor.EditorApplication.isPlaying = false;
        //#else
                 //Application.Quit();
        //#endif
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H)) { currentHP += 10; currentHP = Mathf.Min(maxHP, currentHP); }
    }
}

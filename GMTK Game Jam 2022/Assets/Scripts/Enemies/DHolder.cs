using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DHolder : Enemy
{
    int damageTaken;
    bool tripleDamage = false;
    bool blockFull = false;

    public override void PerformAction()
    {
        Debug.Assert(player != null, "Need to call Init!!!!");

        int randNum = Random.Range(1, 5);

        if (tripleDamage) randNum = 0;
        blockFull = false;
        switch (randNum)
        {
            case 0:
                int dmg = Random.Range(25, 51) * (tripleDamage ? 3 : 1);
                int actualDamage = player.InflictDamage(dmg);
                CombatManager.Instance.UpdateCombatReportText($"{Name}'s super attack does " + actualDamage.ToString() + " HP");
                tripleDamage = false;
                damageTaken = 0;
                break;
            case 1:
                damageTaken = damageTaken / 2;
                int returnedDamage = player.InflictDamage(damageTaken);
                CombatManager.Instance.UpdateCombatReportText($"{Name} returns half its damage taken! You take {returnedDamage.ToString()} damage!");
                damageTaken = 0;
                break;
            case 2:
                CombatManager.Instance.UpdateCombatReportText($"{Name} CHARGES up a super attack for next turn!");
                tripleDamage = true;
                damageTaken = 0;
                break;
            case 3:
                int smallAttack = Random.Range(10, 21);
                int newDamage = player.InflictDamage(smallAttack);
                CombatManager.Instance.UpdateCombatReportText($"{Name} deals {newDamage} and intends to FULL BLOCK this turn!");
                blockFull = true;
                damageTaken = 0;
                break;
            
            case 4:
                int normalAttack = Random.Range(40, 51); 
                int returnDamage = player.InflictDamage(normalAttack);
                CombatManager.Instance.UpdateCombatReportText($"{Name} rolls an attack! You take {returnDamage} damage!");
                damageTaken = 0;
                break;

        }

        int damage = Random.Range(1, 50);

    }

    public override void InflictDamage(int dmg)
    {
        if (!blockFull)
        {
            currentHp -= dmg;
            damageTaken += dmg;
        }
        CombatManager.Instance.enemyAnimation.SetBool("isHit", true);
        if (!blockFull) { CombatManager.Instance.UpdateCombatReportText("You dealed " + dmg + " damage!"); }
        else { CombatManager.Instance.UpdateCombatReportText($"{Name} blocked your damage!"); }
        StartCoroutine(hitAnimCancel(0.5f));
        //EnemyHealthBar.Instance.currentHealth = currentHp;
        if (currentHp <= 0) 
        {
            CombatManager.Instance.playAudio.StopLoop("BossTheme");
            Die();
        }

    }
}

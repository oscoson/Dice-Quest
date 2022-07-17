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

        int randNum = Random.Range(0, 4);

        if (tripleDamage) randNum = 0;
        blockFull = false;
        switch (randNum)
        {
            case 0:
                int dmg = Random.Range(1, 50) * (tripleDamage ? 3 : 1);
                tripleDamage = false;
                int actualDamage = player.InflictDamage(dmg);
                CombatManager.Instance.UpdateCombatReportText($"{Name} attacks and damages you for " + actualDamage.ToString() + " HP");
                break;
            case 1:
                int returnedDamage = player.InflictDamage(damageTaken);
                CombatManager.Instance.UpdateCombatReportText($"{Name} returns damage taken!    You are dealt {returnedDamage.ToString()} HP");
                damageTaken = 0;
                break;
            case 2:
                CombatManager.Instance.UpdateCombatReportText($"{Name} intends to deal triple damage next turn!");
                tripleDamage = true;
                break;
            case 3:
                CombatManager.Instance.UpdateCombatReportText($"{Name} intends to full block next turn!");
                blockFull = true;
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
        if (currentHp <= 0) Die();
    }
}

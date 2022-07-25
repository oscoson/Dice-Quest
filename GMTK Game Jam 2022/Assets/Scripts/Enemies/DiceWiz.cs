using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceWiz : Enemy
{
    int strength = 1;
    public override void PerformAction()
    {
        Debug.Assert(player != null, "Need to call Init!!!!");
        StartCoroutine(DoDamage());
    }

    IEnumerator DoDamage()
    {
        for (int i = 0; i < 5; i++)
        {
            if(CombatManager.Instance.healthBar.currentHealth > 0)
            {
                int damage = strength;
                int actualDamage = player.InflictDamage(damage);
                CombatManager.Instance.UpdateCombatReportText($"{Name} attacks and damages you for " + actualDamage.ToString() + " HP");
                yield return new WaitForSeconds(0.1f);
            }
            else
            {
                CombatManager.Instance.UpdateCombatReportText("You have died!");
            }
        }
        strength = 1;
    }

    public override void InflictDamage(int dmg)
    {
        strength++;
        currentHp -= dmg;
        CombatManager.Instance.enemyAnimation.SetBool("isHit", true);
        CombatManager.Instance.UpdateCombatReportText($"You dealed {dmg} damage!\n{Name}'s strength increased by 1!");
        StartCoroutine(hitAnimCancel(0.5f));
        //EnemyHealthBar.Instance.currentHealth = currentHp;
        if (currentHp <= 0) Die();
    }
}

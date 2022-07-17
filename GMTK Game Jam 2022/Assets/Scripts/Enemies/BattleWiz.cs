using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleWiz : Enemy
{
    public override void PerformAction()
    {
        Debug.Assert(player != null, "Need to call Init!!!!");
        int damage = Random.Range(20, 30);
        int actualDamage = player.InflictDamage(damage);
        CombatManager.Instance.UpdateCombatReportText($"{Name} attacks and damages you for " + actualDamage.ToString() + " HP");
    }

    public override void InflictDamage(int dmg)
    {
        float blockRatio = Random.Range(0.0f, 1.0f);
        int blockedDamage = (int) ((float) dmg * blockRatio);
        int actualDamage = dmg - blockedDamage;
        currentHp -= actualDamage;
        CombatManager.Instance.enemyAnimation.SetBool("isHit", true);
        CombatManager.Instance.UpdateCombatReportText($"{Name} blocked {blockedDamage} damage...     you dealt {actualDamage} damage!");
        StartCoroutine(hitAnimCancel(0.5f));
        //EnemyHealthBar.Instance.currentHealth = currentHp;
        if (currentHp <= 0) Die();
    }
}

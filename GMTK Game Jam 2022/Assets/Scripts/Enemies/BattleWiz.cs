using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleWiz : Enemy
{
    float blockRatio = 0.5f;
    public override void PerformAction()
    {
        Debug.Assert(player != null, "Need to call Init!!!!");
        blockRatio = 0.5f;
        int damage = Random.Range(20, 30);
        int actualDamage = player.InflictDamage(damage);
        CombatManager.Instance.UpdateCombatReportText($"{Name} attacks and damages you for " + actualDamage.ToString() + " HP");
    }

    public override void InflictDamage(int dmg)
    {
        int blockedDamage = (int) ((float) dmg * blockRatio);
        blockRatio *= 0.5f;
        int actualDamage = dmg - blockedDamage;
        currentHp -= actualDamage;
        CombatManager.Instance.enemyAnimation.SetBool("isHit", true);
        CombatManager.Instance.UpdateCombatReportText($"{Name} blocked {blockedDamage} damage!\nYou dealt {actualDamage} damage!\n{Name}'s defense decreased by 50%!");
        StartCoroutine(hitAnimCancel(0.5f));
        //EnemyHealthBar.Instance.currentHealth = currentHp;
        if (currentHp <= 0) Die();
    }
}

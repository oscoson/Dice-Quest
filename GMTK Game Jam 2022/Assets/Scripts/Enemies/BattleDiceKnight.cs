using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleDiceKnight : Enemy
{
    bool doDoubleDamage = false;

    public override void PerformAction()
    {
        Debug.Assert(player != null, "Need to call Init!!!!");

        int randNum = Random.Range(0, 2);

        switch (randNum)
        {
            case 0:
                int damage = Random.Range(40, 60) * (doDoubleDamage ? 2 : 1);
                int actualDamage = player.InflictDamage(damage);
                CombatManager.Instance.UpdateCombatReportText($"{Name} pierces you for " + actualDamage.ToString() + " HP");
                break;
            case 1:
                int pokeDmg = Random.Range(1, 11);
                int actPokeDamage = player.InflictDamage(pokeDmg);
                CombatManager.Instance.UpdateCombatReportText($"{Name} pokes you for " + actPokeDamage.ToString() + " HP");
                break;
        }
    }

    public override void InflictDamage(int dmg)
    {
        currentHp -= dmg;
        CombatManager.Instance.enemyAnimation.SetBool("isHit", true);
        if(!doDoubleDamage) CombatManager.Instance.UpdateCombatReportText("You dealed " + dmg + " damage...  and angered the knight! Intends to deal double damage!");
        else CombatManager.Instance.UpdateCombatReportText("You dealed " + dmg + " damage and calmed down the knight!");
        doDoubleDamage = !doDoubleDamage;
            StartCoroutine(hitAnimCancel(0.5f));
        //EnemyHealthBar.Instance.currentHealth = currentHp;
        if (currentHp <= 0) Die();
    }
}

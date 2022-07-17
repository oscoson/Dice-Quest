using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleDarcherTower : Enemy
{
    public override void PerformAction()
    {
        Debug.Assert(player != null, "Need to call Init!!!!");
        int damage = Random.Range(5, 15);
        int actualDamage = player.InflictDamage(damage);
        CombatManager.Instance.UpdateCombatReportText($"{Name} attacks and damages you for " + actualDamage.ToString() + " HP");
    }
}

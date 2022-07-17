using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DHolder : Enemy
{
    public override void PerformAction()
    {
        Debug.Assert(player != null, "Need to call Init!!!!");
        int damage = Random.Range(30, 31);
        int actualDamage = player.InflictDamage(damage);
        CombatManager.Instance.UpdateCombatReportText($"{Name} attacks and damages you for " + actualDamage.ToString() + " HP");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicDie : PlayableDie
{
    //private Player player
    void Start()
    {
        //player = FindObjectWithType<Player>();
    }

    public override void Roll()
    {
        CombatManager.Instance.playAudio.Play("Magic");
        enemy.InflictDamage(Random.Range(MinDiceVal, MaxDiceVal + 1));
    }
}

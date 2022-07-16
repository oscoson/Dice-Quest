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
        enemy.InflictDamage(Random.Range(diceData.minDiceVal, diceData.maxDiceVal + 1));
    }
}

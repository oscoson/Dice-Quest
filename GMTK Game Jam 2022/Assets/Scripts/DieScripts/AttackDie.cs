using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDie : PlayableDie
{
    void Start()
    {
        //player = FindObjectWithType<Player>();
        //minVal = diceData.minDiceVal;
        //maxVal = diceData.maxDiceVal;
    }
    public override void Roll()
    {
        enemy.InflictDamage(Random.Range(diceData.minDiceVal, diceData.maxDiceVal + 1));
    }
}

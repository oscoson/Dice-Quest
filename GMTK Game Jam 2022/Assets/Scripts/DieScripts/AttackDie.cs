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
        CombatManager.Instance.playAudio.Play("Attack");
        enemy.InflictDamage(Random.Range(MinDiceVal, MaxDiceVal + 1));
    }
}

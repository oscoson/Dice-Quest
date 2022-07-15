using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDie : PlayableDie
{
    public float minVal;
    public float maxVal;
    void Start()
    {
        //player = FindObjectWithType<Player>();
        //minVal = diceData.minDiceVal;
        //maxVal = diceData.maxDiceVal;
    }
    public override void Roll()
    {
        throw new System.NotImplementedException();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockDie : PlayableDie
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override void Roll()
    {
        CombatManager.Instance.playAudio.Play("Block");
        player.Block(MinDiceVal, MaxDiceVal);
    }

    public override void Upgrade()
    {
        if(MaxDiceVal < 50)
        {
            MinDiceVal += Random.Range(1,5);
            MaxDiceVal += Random.Range(4,7);
        }
    }
}

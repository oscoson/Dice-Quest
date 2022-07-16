using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoveryDie : PlayableDie
{
    

    //private Player player
    void Start()
    {
        //player = FindObjectWithType<Player>();
    }

    public override void Roll()
    {
        Debug.Assert(hasInit);
        CombatManager.Instance.playAudio.Play("Heal");
        player.Heal(Random.Range(MinDiceVal,MaxDiceVal + 1));
    }
}

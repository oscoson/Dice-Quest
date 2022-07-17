using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraTurnDie : PlayableDie
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override void Roll()
    {
        CombatManager.Instance.playAudio.Play("ExtraTurn");
        player.ExtraTurn();
    }
}

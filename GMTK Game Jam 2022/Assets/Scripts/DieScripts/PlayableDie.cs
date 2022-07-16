using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayableDie : MonoBehaviour
{
    public DiceSO diceData;

    protected Player player;
    protected Enemy enemy;

    protected bool hasInit = false;

    public void Init(Player p, Enemy e)
    {
        player = p;
        enemy = e;
        hasInit = true;
    }

    public abstract void Roll();
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayableDie : MonoBehaviour
{
    [SerializeField] DiceSO diceData;

    protected Player player;
    protected Enemy enemy;

    protected bool hasInit = false;
    protected bool hasFirstInit = false; // for DiceSO info loading

    public int MinDiceVal { get; protected set; }
    public int MaxDiceVal { get; protected set; }
    public Sprite DiceSprite { get; protected set; }
    public string DiceType { get; protected set; }

    private void Awake()
    {
            LoadCardInfo();

    }

    public void Init(Player p, Enemy e)
    {
        player = p;
        enemy = e;
        hasInit = true;
    }

    private void LoadCardInfo()
    {
        MinDiceVal = diceData.minDiceVal;
        MaxDiceVal = diceData.maxDiceVal;
        DiceSprite = diceData.diceSprite;
        DiceType = diceData.diceType;
    }

    public abstract void Roll();

    public virtual void Upgrade()
    {
        MaxDiceVal += Random.Range(1, 6);
        MinDiceVal += Mathf.Min(Random.Range(1, 6), MaxDiceVal - MinDiceVal);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dice", menuName = "New Dice")]
public class DiceSO : ScriptableObject
{
    public int minDiceVal;
    public int maxDiceVal;
    public Sprite diceSprite;
    public string diceType;
    

}

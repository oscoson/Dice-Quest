using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayableDie : MonoBehaviour
{
    [SerializeField] DiceSO diceData;

    public abstract void Roll();
}

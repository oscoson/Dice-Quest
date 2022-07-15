using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDie : MonoBehaviour
{
    public DiceSO attackDieSO;
    public float minVal;
    public float maxVal;
    void Start()
    {
        //player = FindObjectWithType<Player>();
        minVal = attackDieSO.minDiceVal;
        maxVal = attackDieSO.maxDiceVal;
    }

}

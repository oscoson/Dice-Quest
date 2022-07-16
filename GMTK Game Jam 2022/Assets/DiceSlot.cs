using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceSlot : MonoBehaviour
{
    [SerializeField] int id;

    public System.Action<int> OnDiePlay;

    public void OnClick()
    {
        OnDiePlay?.Invoke(id);
    }
}

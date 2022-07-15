using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance;
    [SerializeField] private Canvas battleCanvas;

    private void Awake()
    {
        if (Instance != null)
        {
            return;
        }
        else
        {
            Instance = this;
        }
    }

    public void StartCombat()
    {
        battleCanvas.enabled = true;

    }

    // Update is called once per frame
    void Update()
    {

    }
}

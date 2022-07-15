using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance;
    [SerializeField] private Canvas battleCanvas;
    private Player player;
    public TextMeshProUGUI combatReport;

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

    private void Start()
    {
        player = FindObjectOfType<Player>();
    }
    public void StartCombat()
    {
        battleCanvas.enabled = true;
        DiceDrawSystem.Instance.Init(player.diceInventory);
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}

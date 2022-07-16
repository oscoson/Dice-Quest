using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance;
    public GameObject battleCanvas;
    private Player player;
    [SerializeField] List<Enemy> enemies;
    
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

    public void StartCombat(int index)
    {
        battleCanvas.SetActive(true);
        DiceDrawSystem.Instance.Init(player.diceInventory, player, enemies[index]);
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}

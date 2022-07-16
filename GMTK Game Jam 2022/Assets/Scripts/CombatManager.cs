using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance;
    public GameObject battleCanvas;
    [SerializeField] Player player;
    [SerializeField] List<Enemy> enemies;

    readonly int maxDicePlay = 3;
    int dicePlayLeft = 3;
    bool playerTurn = true;
    
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
        DiceDrawSystem.Instance.OnDrawDie += UpdateDieScreen;
    }

    void UpdateDieScreen(PlayableDie die)
    {

    }

    public void StartCombat(int index)
    {
        playerTurn = true;
        battleCanvas.SetActive(true);
        DiceDrawSystem.Instance.Init(player.diceInventory, player, enemies[index]);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void EndTurn()
    {
        playerTurn = !playerTurn;
    }





}

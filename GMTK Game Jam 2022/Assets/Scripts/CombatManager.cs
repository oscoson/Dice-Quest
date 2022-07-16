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
    [SerializeField] List<Image> diceSlots;
    public TextMeshProUGUI combatReport;
    private Image diceSlotsImage;




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
        battleCanvas.SetActive(true);
        DiceDrawSystem.Instance.Init(player.diceInventory, player, enemies[index]);
        DrawDice();
    }

    public void DrawDice()
    {
        DiceDrawSystem.Instance.DrawDice();
        for (int i = 0; i < 5; i = i + 1) 
        {
            diceSlots[i].sprite = DiceDrawSystem.Instance.playPile[i].diceData.diceSprite;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }






}

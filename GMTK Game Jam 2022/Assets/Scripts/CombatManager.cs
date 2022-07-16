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
    [SerializeField] List<GameObject> diceSlots;
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
        for (int i = 0; i < diceSlots.Count; i++)
        {
            Debug.Log(diceSlots[i]);
            diceSlots[i].GetComponent<DiceSlot>().OnDiePlay += PlayDie;
        }
    }

    void PlayDie(int id)
    {
        if(id < DiceDrawSystem.Instance.playPile.Count)
        {
            DiceDrawSystem.Instance.PlayDie(id);
        }
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
        Debug.Log(DiceDrawSystem.Instance.playPile.Count);
        for (int i = 0; i < DiceDrawSystem.Instance.playPile.Count; i = i + 1) 
        {
            diceSlots[i].GetComponent<Image>().sprite = DiceDrawSystem.Instance.playPile[i].diceData.diceSprite;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }






}

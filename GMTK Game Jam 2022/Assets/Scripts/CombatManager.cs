using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance;
    [Header("Canvas & Other Objects")]
    public GameObject battleCanvas;
    [SerializeField] Sprite emptySquare;
    [SerializeField] Player player;
    [Header("Lists")]
    [SerializeField] List<GameObject> enemies;
    [SerializeField] List<GameObject> diceSlots;
    [SerializeField] List<TextMeshProUGUI> diceValues;
    [Header("Text Elements")]
    public TextMeshProUGUI combatReport;
    [SerializeField] TextMeshProUGUI availableDiceNum;
    [SerializeField] TextMeshProUGUI graveyardDiceNum;
    private Image diceSlotsImage;

    public System.Action OnCombatEnd;


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        for (int i = 0; i < diceSlots.Count; i++)
        {
            diceSlots[i].GetComponent<DiceSlot>().OnDiePlay += PlayDie;
        }
    }


    void PlayDie(int id)
    {
        if (id < DiceDrawSystem.Instance.playPile.Count)
        {
            if (DiceDrawSystem.Instance.playPile[id] == null) return;
            DiceDrawSystem.Instance.PlayDie(id);
            UpdateDiceDisplay();
        }
    }


    public void StartCombat(int index)
    {
        battleCanvas.SetActive(true);
        DiceDrawSystem.Instance.Init(player.diceInventory, player, Instantiate(enemies[index]).GetComponent<Enemy>());
        DiceDrawSystem.Instance.ShuffleDrawPile();
        DrawDice();
    }

    public void EndCombat()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            battleCanvas.SetActive(false);
            OnCombatEnd?.Invoke();
        }
    }

    public void DrawDice()
    {
        DiceDrawSystem.Instance.DrawDice();
        UpdateDiceDisplay();
    }

    public void UpdateDiceDisplay()
    {
        for (int i = 0; i < DiceDrawSystem.Instance.playPile.Count; i = i + 1)
        {
            if (DiceDrawSystem.Instance.playPile[i] != null)
                diceSlots[i].GetComponent<Image>().sprite = DiceDrawSystem.Instance.playPile[i].diceData.diceSprite;
            else
                diceSlots[i].GetComponent<Image>().sprite = emptySquare;
        }
    }

    // Update is called once per frame
    void Update()
    {
        EndCombat();
        for (int i = 0; i < DiceDrawSystem.Instance.playPile.Count; i = i + 1)
        {
            if (DiceDrawSystem.Instance.playPile[i] != null && DiceDrawSystem.Instance.playPile[i].diceData.diceType == "Damage")
                diceValues[i].text = DiceDrawSystem.Instance.playPile[i].diceData.minDiceVal.ToString() + "-" 
                + DiceDrawSystem.Instance.playPile[i].diceData.maxDiceVal.ToString() + " DMG";
            else if (DiceDrawSystem.Instance.playPile[i] != null && DiceDrawSystem.Instance.playPile[i].diceData.diceType == "Health")
            {
                diceValues[i].text = DiceDrawSystem.Instance.playPile[i].diceData.minDiceVal.ToString() + "-" 
                + DiceDrawSystem.Instance.playPile[i].diceData.maxDiceVal.ToString() + " HP";
            }
            else
                diceValues[i].text = "";
        }
    }

    private void FixedUpdate()
    {
        availableDiceNum.text = DiceDrawSystem.Instance.drawBag.Count.ToString();
        graveyardDiceNum.text = DiceDrawSystem.Instance.discardBag.Count.ToString();
    }







}

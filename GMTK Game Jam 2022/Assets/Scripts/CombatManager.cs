using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance;
    public GameObject battleCanvas;
    [SerializeField] Sprite emptySquare;
    [Header("Player")]
    [SerializeField] Player player;
    [SerializeField] bool playerTurn = true;
    [Header("Lists")]
    [SerializeField] List<GameObject> enemies;
    [SerializeField] List<GameObject> diceSlots;
    public TextMeshProUGUI combatReport;
    [SerializeField] TextMeshProUGUI availableDiceNum;
    [SerializeField] TextMeshProUGUI graveyardDiceNum;
    [SerializeField] TextMeshProUGUI energyAmount;
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
        if (id < DiceDrawSystem.Instance.playPile.Count && player.energyLevel > 0)
        {
            if (DiceDrawSystem.Instance.playPile[id] == null) return;
            DiceDrawSystem.Instance.PlayDie(id);
            player.energyLevel--;
            UpdateDiceDisplay();
        }
    }


    public void StartCombat(int index)
    {
        playerTurn = true;
        player = FindObjectOfType<Player>();
        battleCanvas.SetActive(true);
        DiceDrawSystem.Instance.Init(player.diceInventory, player, Instantiate(enemies[index]).GetComponent<Enemy>());
        DiceDrawSystem.Instance.ShuffleDrawPile();
        BeginTurn();
    }

    public void BeginTurn()
    {
        DrawDice();
    }
    public void EndTurn()
    {
        
    }

    public void EndCombat()
    {
        battleCanvas.SetActive(false);
        OnCombatEnd?.Invoke();
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            EndCombat();
        }
        for (int i = 0; i < DiceDrawSystem.Instance.playPile.Count; i = i + 1)
        {
            battleCanvas.SetActive(false);
            OnCombatEnd?.Invoke();
        }
    }

    private void FixedUpdate()
    {
        availableDiceNum.text = DiceDrawSystem.Instance.drawBag.Count.ToString();
        graveyardDiceNum.text = DiceDrawSystem.Instance.discardBag.Count.ToString();
        Debug.Log(player);
        energyAmount.text = player.energyLevel.ToString() + "/" + player.maxEnergyLevel.ToString();

    }







}

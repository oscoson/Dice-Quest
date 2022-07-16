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
    [Header("Waiting Times")] // I aint gonna lie brian this is pretty scuffed LOL
    [Header("Player")]
    [SerializeField] Player player;
    private Enemy enemy;
    [SerializeField] bool playerTurn = true;
    [SerializeField] float playerWaitTime = 1.5f;
    [SerializeField] float endWaitTime = 2f;
    [Header("Enemy")]
    [SerializeField] private int currentEnemyIndex;
    [Header("Lists")]
    [SerializeField] List<GameObject> enemies;
    [SerializeField] List<GameObject> diceSlots;
    [SerializeField] List<TextMeshProUGUI> diceValues;
    [Header("Text Elements")]
    public TextMeshProUGUI combatReport;
    [SerializeField] TextMeshProUGUI availableDiceNum;
    [SerializeField] TextMeshProUGUI graveyardDiceNum;
    [SerializeField] TextMeshProUGUI energyAmount;
    [SerializeField] EnemyHealthBar enemyHealthBar;
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
            DiceDrawSystem.Instance.idTracker(id);
            player.energyLevel--;
            UpdateDiceDisplay();

            enemyHealthBar.SetHealth(enemy.CurrentHp);
        }
    }


    public void StartCombat(int index)
    {
        playerTurn = true;
        player = FindObjectOfType<Player>();
        currentEnemyIndex = index;
        battleCanvas.SetActive(true);
        GameObject enemyGo = Instantiate(enemies[index]);
        enemy = enemyGo.GetComponent<Enemy>();
        enemyHealthBar.Init(enemy.CurrentHp, enemy.MaxHp);
        DiceDrawSystem.Instance.Init(player.diceInventory, player, enemy);
        DiceDrawSystem.Instance.ShuffleDrawPile();
        DiceDrawSystem.Instance.firstTurn = true;
        // Since we're taking into account unique enemy situations + moves -> several conditionals for what kind of enemy you will be facing
        if(currentEnemyIndex == 0)
        {
            UpdateCombatReportText("Dicewiz blocks your way!");
            DrawDice();
        }
    }

    public void BeginTurn()
    {
        UpdateCombatReportText("What will you do?");
        DrawDice();
    }
    public void EndTurn()
    {
        EnemyTurn();
    }

    public void EnemyTurn()
    {
        if(currentEnemyIndex == 0)
        {
            int damage = Random.Range(4, 10);
            player.InflictDamage(damage);
            UpdateCombatReportText("Dicewiz attacks and damages you for " + damage.ToString() + " HP");
            StartCoroutine(playerWaitingTime(playerWaitTime));
        }
    }

    public void EndCombat()
    {
        battleCanvas.SetActive(false);
        OnCombatEnd?.Invoke();
    }

    IEnumerator playerWaitingTime(float waitTime)
    {
        yield return new WaitForSecondsRealtime(waitTime);
        BeginTurn();

    }

    public void DrawDice()
    {
        DiceDrawSystem.Instance.DrawDice();
        player.energyLevel = player.maxEnergyLevel;
        UpdateDiceDisplay();
    }

    public void UpdateDiceDisplay()
    {
        for (int i = 0; i < DiceDrawSystem.Instance.playPile.Count; i = i + 1)
        {
            Debug.Log(i);
            if (DiceDrawSystem.Instance.playPile[i] != null)
            {
                diceSlots[i].GetComponent<Image>().sprite = DiceDrawSystem.Instance.playPile[i].diceData.diceSprite;
            }
            else
            {
                diceSlots[i].GetComponent<Image>().sprite = emptySquare;
            }
        }
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

    public void UpdateCombatReportText(string report)
    {
        combatReport.text = report;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            EndCombat();
        }
    }

    private void FixedUpdate()
    {
        availableDiceNum.text = DiceDrawSystem.Instance.drawBag.Count.ToString();
        graveyardDiceNum.text = DiceDrawSystem.Instance.discardBag.Count.ToString();
        energyAmount.text = player.energyLevel.ToString() + "/" + player.maxEnergyLevel.ToString();

    }







}

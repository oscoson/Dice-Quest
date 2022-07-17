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
    public AudioManager playAudio;
    [SerializeField] Sprite emptySquare;
    

    [Header("Player")]
    [SerializeField] Player player;
    
    [SerializeField] bool playerTurn = true;

    [Header("Waiting Times")] // I aint gonna lie brian this is pretty scuffed LOL
    [SerializeField] float playerWaitTime = 1.5f;
    //[SerializeField] float endWaitTime = 2f;
    [Header("Enemy")]
    public Animator enemyAnimation;
    private Enemy enemy;
    [SerializeField] private Image enemyImage;
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
        //Player assign
        playerTurn = true;
        player = FindObjectOfType<Player>();
        //canvas set
        battleCanvas.SetActive(true);
        //Enemy assign
        currentEnemyIndex = index;
        GameObject enemyGo = Instantiate(enemies[index]);
        enemy = enemyGo.GetComponent<Enemy>();
        enemy.Init(player);
        enemyImage.sprite = enemy.sprite;
        enemyHealthBar.Init(enemy.CurrentHp, enemy.MaxHp);
        //DiceDrawSystem Calls
        DiceDrawSystem.Instance.Init(player.diceInventory, player, enemy);
        DiceDrawSystem.Instance.ShuffleDrawPile();
        DiceDrawSystem.Instance.firstTurn = true;
        //Play Audio
        playAudio.Play("Encounter");
        playAudio.Play("BattleTheme");
        UpdateCombatReportText($"{enemy.Name} blocks your way!");
        DrawDice();
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
        enemy.PerformAction();
        StartCoroutine(playerWaitingTime(playerWaitTime));
    }

    public void EndCombat()
    {
        player.diceInventory.ForEach(d => d.Upgrade());
        Destroy(enemy.gameObject);
        playAudio.StopLoop("BattleTheme");
        battleCanvas.SetActive(false);
        OnCombatEnd?.Invoke();
    }

    public IEnumerator playerWaitingTime(float waitTime)
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
            if (DiceDrawSystem.Instance.playPile[i] != null)
            {
                diceSlots[i].GetComponent<Image>().sprite = DiceDrawSystem.Instance.playPile[i].DiceSprite;
            }
            else
            {
                diceSlots[i].GetComponent<Image>().sprite = emptySquare;
            }
        }
        for (int i = 0; i < DiceDrawSystem.Instance.playPile.Count; i = i + 1)
        {
            if (DiceDrawSystem.Instance.playPile[i] != null && DiceDrawSystem.Instance.playPile[i].DiceType == "Damage")
                diceValues[i].text = DiceDrawSystem.Instance.playPile[i].MinDiceVal.ToString() + "-" 
                + DiceDrawSystem.Instance.playPile[i].MaxDiceVal.ToString() + " DMG";
            else if (DiceDrawSystem.Instance.playPile[i] != null && DiceDrawSystem.Instance.playPile[i].DiceType == "Health")
            {
                diceValues[i].text = DiceDrawSystem.Instance.playPile[i].MinDiceVal.ToString() + "-" 
                + DiceDrawSystem.Instance.playPile[i].MaxDiceVal.ToString() + " HP";
            }
            else if (DiceDrawSystem.Instance.playPile[i] != null && DiceDrawSystem.Instance.playPile[i].DiceType == "Block")
            {
                diceValues[i].text = "Block " + DiceDrawSystem.Instance.playPile[i].MinDiceVal.ToString() + "-" + DiceDrawSystem.Instance.playPile[i].MaxDiceVal.ToString() +" DMG";
            }
            else if (DiceDrawSystem.Instance.playPile[i] != null && DiceDrawSystem.Instance.playPile[i].DiceType == "ExtraTurn")
            {
                diceValues[i].text = "Skip Enemy Turn";
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

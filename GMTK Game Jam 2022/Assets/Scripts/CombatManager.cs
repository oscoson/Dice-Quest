using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance;
    [Header("Canvas & Other Objects")]
    public GameObject battleCanvas;
    public GameObject deathCanvas;
    public AudioManager playAudio;
    public Button endButton;
    [SerializeField] Sprite emptySquare;
    

    [Header("Player")]
    [SerializeField] Player player;
    [SerializeField] bool playerTurn = true;
    [SerializeField] float playerWaitTime = 1.5f;
    [SerializeField] bool playerCombatEndScreen = false;
    public PlayerHealthBar healthBar;
    //[SerializeField] float endWaitTime = 2f;
    [Header("Enemy")]
    public Animator enemyAnimation;
    private Enemy enemy;
    [SerializeField] EnemyHealthBar enemyHealthBar;
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
    private List<Sprite> animationFrames;

    private Coroutine currentAnimationCoroutine;

    int spriteIndex = 0;



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
        playAudio.Play("OverworldTheme");
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
        spriteIndex = 0;

        //Player assign
        playerTurn = true;
        player = FindObjectOfType<Player>();
        //canvas set
        battleCanvas.SetActive(true);
        endButton.interactable = true;
        enemyHealthBar.healthBarBackground.enabled = true;
        //Enemy assign
        currentEnemyIndex = index;
        GameObject enemyGo = Instantiate(enemies[index]);
        enemy = enemyGo.GetComponent<Enemy>();
        enemy.Init(player);

        animationFrames = new(enemy.sprites);
        enemyImage.sprite = animationFrames[0];

        currentAnimationCoroutine = StartCoroutine(IncrementSpriteIndex());

        enemyHealthBar.Init(enemy.CurrentHp, enemy.MaxHp);
        //DiceDrawSystem Calls
        DiceDrawSystem.Instance.Init(player.diceInventory, player, enemy);
        DiceDrawSystem.Instance.ShuffleDrawPile();
        DiceDrawSystem.Instance.firstTurn = true;
        //renenable dice slots
        for(int i = 0; i < diceSlots.Count; i++)
        {
            diceSlots[i].GetComponent<Button>().interactable = true;
        }
        //Play Audio
        playAudio.Pause("OverworldTheme");
        playAudio.Play("Encounter");
        playAudio.Play("BattleTheme");
        UpdateCombatReportText($"{enemy.Name} blocks your way!");
        DrawDice();
    }

    public void BeginTurn()
    {
        for(int i = 0; i < diceSlots.Count; i++)
        {
            diceSlots[i].GetComponent<Button>().interactable = true;
        }
        UpdateCombatReportText("What will you do?");
        DrawDice();
    }

    public void EndTurn()
    {
        EnemyTurn();
    }

    public void EnemyTurn()
    {
        for(int i = 0; i < diceSlots.Count; i++)
        {
            diceSlots[i].GetComponent<Button>().interactable = false;
        }
        endButton.interactable = false;
        enemy.PerformAction();
        StartCoroutine(PlayerWaitingTime(2f));
    }

    public void EndCombat()
    {
        for(int i = 0; i < diceSlots.Count; i++)
        {
            diceSlots[i].GetComponent<Button>().interactable = false;
        }
        endButton.interactable = false;
        enemyHealthBar.healthBarBackground.enabled = false;
        enemyAnimation.SetBool("isDead", true);
        player.diceInventory.ForEach(d => d.Upgrade());
        playAudio.StopLoop("BattleTheme");
        playAudio.Play("VictoryTheme");
        UpdateCombatReportText(enemy.Name + " has been defeated!");
        Destroy(enemy.gameObject);
        StartCoroutine(EnemyDefeated(3f));
    }

    public void LoseCombat()
    {
        for(int i = 0; i < diceSlots.Count; i++)
        {
            diceSlots[i].GetComponent<Button>().interactable = false;
        }
        endButton.interactable = false;
        healthBar.secondaryHealthBar.fillAmount = 0;
        UpdateCombatReportText("You have died!");
        Time.timeScale = 0;
        playAudio.StopLoop("BattleTheme");
        playAudio.Play("DeathTheme");
        deathCanvas.SetActive(true);

    }
    public void LoseCombatTwo()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
                #else
                Application.Quit();
                #endif
    }

    public void EndPostScreen()
    {
        playerCombatEndScreen = false;
        playAudio.StopLoop("VictoryTheme");
        playAudio.UnPause("OverworldTheme");
        battleCanvas.SetActive(false);
        OnCombatEnd?.Invoke();
        StopCoroutine(currentAnimationCoroutine);
    }

    public IEnumerator PlayerWaitingTime(float waitTime)
    {
        for(int i = 0; i < diceSlots.Count; i++)
        {
            diceSlots[i].GetComponent<Button>().interactable = false;
        }
        endButton.interactable = false;
        yield return new WaitForSecondsRealtime(waitTime);
        endButton.interactable = true;
        BeginTurn();

    }

    public IEnumerator EnemyDefeated(float waitTime)
    {
        yield return new WaitForSecondsRealtime(waitTime);
        UpdateCombatReportText("Your Dice have been upgraded!");
        playerCombatEndScreen = true;
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
                diceValues[i].text = "Roll " + DiceDrawSystem.Instance.playPile[i].MinDiceVal.ToString() + "-" 
                + DiceDrawSystem.Instance.playPile[i].MaxDiceVal.ToString() + " DMG";
            else if (DiceDrawSystem.Instance.playPile[i] != null && DiceDrawSystem.Instance.playPile[i].DiceType == "Health")
            {
                diceValues[i].text = "Roll " +  DiceDrawSystem.Instance.playPile[i].MinDiceVal.ToString() + "-" 
                + DiceDrawSystem.Instance.playPile[i].MaxDiceVal.ToString() + " HP";
            }
            else if (DiceDrawSystem.Instance.playPile[i] != null && DiceDrawSystem.Instance.playPile[i].DiceType == "Block")
            {
                diceValues[i].text = "Roll " + DiceDrawSystem.Instance.playPile[i].MinDiceVal.ToString() + "-" + DiceDrawSystem.Instance.playPile[i].MaxDiceVal.ToString() +"% DEF";
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

    public void RestartButton()
    {
        playAudio.StopLoop("DeathTheme");
        battleCanvas.SetActive(false);
        deathCanvas.SetActive(false);
        Time.timeScale = 1;
        player.currentHP = 100;
        playAudio.Play("OverworldTheme");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void MainMenuButton()
    {
        playAudio.StopLoop("DeathTheme");
        battleCanvas.SetActive(false);
        deathCanvas.SetActive(false);
        Time.timeScale = 1;
        player.currentHP = 100;
        SceneManager.LoadScene(0);
    }
    
    

    // Update is called once per frame
    void Update()
    {

        if(Input.GetButton("Fire1") && playerCombatEndScreen)
        {
            EndPostScreen();
        }
    }

    private void FixedUpdate()
    {
        availableDiceNum.text = DiceDrawSystem.Instance.drawBag.Count.ToString();
        graveyardDiceNum.text = DiceDrawSystem.Instance.discardBag.Count.ToString();
        energyAmount.text = player.energyLevel.ToString() + "/" + player.maxEnergyLevel.ToString();
    }

    IEnumerator IncrementSpriteIndex()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.15f);
            spriteIndex++;
            spriteIndex %= animationFrames.Count;
            //spriteIndex = Random.Range(0, animationFrames.Count);
            enemyImage.sprite = animationFrames[spriteIndex];
        }
    }

}

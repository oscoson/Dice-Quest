using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DiceDrawSystem : MonoBehaviour
{
    public static DiceDrawSystem Instance;
    List<PlayableDie> dice;

    public List<PlayableDie> drawBag;
    public List<PlayableDie> playPile;
    public List<PlayableDie> discardBag;
    public bool firstTurn = false;
    public int drawAmount;
    [SerializeField] private List<int> emptyDiceSlots = new List<int>(5);

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.RightArrow))
    //    {
    //        DrawDice();
    //    }
    //    else if (Input.GetKeyDown(KeyCode.LeftArrow))
    //    {
    //        PlayDie(0);
    //    }
    //    else if (Input.GetKeyDown(KeyCode.UpArrow))
    //    {
    //        DiscardDice();
    //    }
    //    else if (Input.GetKeyDown(KeyCode.DownArrow))
    //    {
    //        Reshuffle();
    //    }
    //}
    public void Init(List<PlayableDie> diceList, Player player, Enemy enemy)
    {
        dice = new List<PlayableDie>(diceList);
        dice.ForEach(d => d.Init(player, enemy));
        drawBag = new List<PlayableDie>(dice);
        playPile = new List<PlayableDie>(5);
        discardBag = new List<PlayableDie>(dice.Count);
    }

    public void ShuffleDrawPile()
    {
        int n = drawBag.Count;
        for (int i = 0; i < (n - 1); i++)
        {
            int r = i + Random.Range(0, n - i);
            PlayableDie die = drawBag[r];
            drawBag[r] = drawBag[i];
            drawBag[i] = die;
        }
    }

    public void Reshuffle()
    {
        drawBag.AddRange(discardBag);
        discardBag.Clear();

        ShuffleDrawPile();
    }

    private void DrawDie(int emptyDicePos)
    {
        Debug.Log("Drawing Die!");
        //Debug.Assert(drawBag.Count > 0, "DRAWBAG COUNT NEEDS TO BE GREATER THAN ZERO!!!!@#!@#)!@#@!#!");
        int index = Random.Range(0, drawBag.Count);
        //playPile.Add(drawBag[index]);
        // Adds new Dice at the position of the selected dice from previous turn
        playPile[emptyDiceSlots[emptyDicePos]] = drawBag[index];
        emptyDiceSlots.RemoveAt(emptyDicePos);
        drawBag.RemoveAt(index);
    }

    private void firstDraw()
    {
        int index = Random.Range(0, drawBag.Count);
        playPile.Add(drawBag[index]);
        drawBag.RemoveAt(index);
    }

    public void idTracker(int id)
    {

        //Tracks ids of selected dies
        Debug.Log("ID Track: " + id);
        emptyDiceSlots.Add(id);
        Debug.Log("ids = " + emptyDiceSlots.Count());
        drawAmount++;
    }

    public void emptyIDTracker()
    {
        emptyDiceSlots.RemoveAt(0);
    }
    public void DrawDice()
    {
        if (firstTurn)
        {
            if(emptyDiceSlots.Count == 1)
            {
                emptyIDTracker();
            }
            drawAmount = 5;
            firstTurn = false;
            for (int i = 0; i < drawAmount; i++)
            {
                firstDraw();
            }
            drawAmount = 0;
            return;
        }
        Debug.Log("Drawing Dice!");
        int drawsNeeded = drawAmount;
        int drawBagSize = drawBag.Count;
        // If the amount of draws needed for empty dice exceeds the bag size, refill bag -> draw amount needed
        if (drawsNeeded > drawBagSize)
        {
            Reshuffle();
            Debug.Log("More Die!");
            for (int i = 0; i < drawsNeeded; drawsNeeded--)
            {
                DrawDie(i);
            }
            drawAmount = 0;
            return;
        }
        else
        {
            // else, draw the amount needed
            for (int i = 0; i < drawsNeeded; drawsNeeded--)
            {
                Debug.Log("We have Enough!");
                DrawDie(i);
            }
            drawAmount = 0;
            return;
        }
    }

    public void PlayDie(int ind)
    {
        playPile[ind].Roll();
        discardBag.Add(playPile[ind]);
        playPile[ind] = null;
    }

    private void DiscardDice()
    {
        for (int i = 0; i < playPile.Count; i++)
        {
            if (playPile[i] != null)
                discardBag.Add(playPile[i]);
        }
        playPile.Clear();
    }



}

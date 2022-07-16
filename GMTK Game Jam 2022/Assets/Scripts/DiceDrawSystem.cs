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

    readonly int drawAmount = 5;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else
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

    private void DrawDie()
    {
        Debug.Assert(drawBag.Count > 0, "DRAWBAG COUNT NEEDS TO BE GREATER THAN ZERO!!!!@#!@#)!@#@!#!");
        int index = Random.Range(0, drawBag.Count);
        playPile.Add(drawBag[index]);
        drawBag.RemoveAt(index);
    }

    public void DrawDice()
    {
        int drawLeft = drawAmount;
        int drawBagSize = drawBag.Count;
        if(drawLeft > drawBagSize)
        {
            drawLeft -= drawBagSize;
            for(int i = 0; i < drawBagSize; i++)
            {
                DrawDie();
            }
        }

        Reshuffle();
        drawBagSize = drawBag.Count;
        for(int i = 0; i < Mathf.Min(drawLeft, drawBagSize); i++)
        {
            DrawDie();
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

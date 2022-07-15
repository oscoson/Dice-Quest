using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DiceDrawSystem : MonoBehaviour
{
    [SerializeField] List<PlayableDie> dice;

    List<PlayableDie> drawBag;
    List<PlayableDie> playPile;
    List<PlayableDie> discardBag;

    readonly int drawAmount = 6;

    // Start is called before the first frame update
    void Awake()
    {
        drawBag = new List<PlayableDie>(dice);
        playPile = new List<PlayableDie>(5);
        discardBag = new List<PlayableDie>(dice.Count);
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
        playPile.RemoveAt(ind);
    }

    private void DiscardDice()
    {
        discardBag.AddRange(playPile);
        playPile.Clear();
    }



}

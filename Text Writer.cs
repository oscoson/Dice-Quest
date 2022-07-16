using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// attach to UI Text component (with the full text already there)

public class SlowTyping: MonoBehaviour
{

    Text txt;
    string dialogue;

    void Awake(string SomeRandomTextPulledFromSomewhere)
    {
        dialogue = SomeRandomTextPulledFromSomewhere
        Text txt = "";
        StartCoroutine("WriteText");
    }

    IEnumerator WriteText()
    {
        foreach (char letter in dialogue)
        {
            txt.Text += letter;
            yield return new WaitForSeconds(0.2f);
        }
    }
}
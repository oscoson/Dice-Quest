using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealthBar : MonoBehaviour
{
    public Image mainHealthBar;
    public Image secondaryHealthBar;
    public float currentHealth;
    public float maxHealth;
    [SerializeField] private TextMeshProUGUI text;
    private Player player;

    private void Start()
    {
        player = FindObjectOfType<Player>();
        currentHealth = player.currentHP;
        maxHealth = player.maxHP;
        mainHealthBar.fillAmount = currentHealth / maxHealth;
        secondaryHealthBar.fillAmount = mainHealthBar.fillAmount;
    }

    public void ResetSecondaryHealth()
    {
        secondaryHealthBar.fillAmount = 1;
    }

    public void HealthBarSecondaryUpdate(int healAmount)
    {
        // only called when player is healed
        secondaryHealthBar.fillAmount += healAmount * 0.01f;
    }

    private void Update()
    {
        currentHealth = player.currentHP;
        maxHealth = player.maxHP;
        mainHealthBar.fillAmount = currentHealth / maxHealth;
        if(currentHealth >= 0.9)
        {
            text.text = Mathf.Round((currentHealth * 10.0f) * 0.1f).ToString() + "/" + maxHealth.ToString();
        }
        else if(currentHealth < 0)
        {
            text.text = "0/100";
        }
        else
        {
            text.text = ((currentHealth * 10.0f) * 0.1f).ToString() + "/" + maxHealth.ToString();
        }

    }

    private void FixedUpdate()
    {
        if (secondaryHealthBar.fillAmount > mainHealthBar.fillAmount)
        {
            secondaryHealthBar.fillAmount -= 0.0035f;
        }
    }
}

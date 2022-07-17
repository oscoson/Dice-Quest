using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealthBar : MonoBehaviour
{
    [SerializeField] private Image healthBar;
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
        healthBar.fillAmount = currentHealth / maxHealth;
        secondaryHealthBar.fillAmount = healthBar.fillAmount;
    }

    public void ResetSecondaryHealth()
    {
        secondaryHealthBar.fillAmount = 1;
    }

    private void Update()
    {
        currentHealth = player.currentHP;
        maxHealth = player.maxHP;
        healthBar.fillAmount = currentHealth / maxHealth;
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
        if (secondaryHealthBar.fillAmount > healthBar.fillAmount)
        {
            secondaryHealthBar.fillAmount -= 0.0035f;
        }
    }
}

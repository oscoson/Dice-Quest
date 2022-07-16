using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealthBar : MonoBehaviour
{
    [SerializeField] private Image healthBar;
    [SerializeField] private Image secondaryHealthBar;
    public float currentHealth;
    public float maxHealth;
    [SerializeField] private TextMeshProUGUI text;
    private Player player;

    private void Start()
    {
        player = FindObjectOfType<Player>();
    }

    private void Update()
    {
        currentHealth = player.currentHP;
        maxHealth = player.maxHP;
        healthBar.fillAmount = currentHealth / maxHealth;

    }

    private void FixedUpdate()
    {
        if(currentHealth >= 0.6)
        {
            text.text = Mathf.Round((currentHealth * 10.0f) * 0.1f).ToString() + "/" + maxHealth.ToString();
        }
        else
        {
            text.text = ((currentHealth * 10.0f) * 0.1f).ToString() + "/" + maxHealth.ToString();
        }
        if (secondaryHealthBar.fillAmount > healthBar.fillAmount)
        {
            secondaryHealthBar.fillAmount -= 0.0035f;
        }
    }
}

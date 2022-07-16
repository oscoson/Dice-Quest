using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public static EnemyHealthBar Instance;
    [SerializeField] private Image healthBar;
    [SerializeField] private Image secondaryHealthBar;
    public float currentHealth;
    public float maxHealth;
    private Enemy enemy;

    public void Init(int curHealth, int max)
    {
        currentHealth = curHealth;
        maxHealth = max;
        healthBar.fillAmount = currentHealth/maxHealth;
        secondaryHealthBar.fillAmount = currentHealth / maxHealth;
    }

    private void Start()
    {

    }

    private void Update()
    {
        //currentHealth = player.currentHP;
        //maxHealth = player.maxHP;
        healthBar.fillAmount = currentHealth / maxHealth;
    }

    public void SetHealth(float health)
    {
        currentHealth = health;
    }

    private void FixedUpdate()
    {
        if (secondaryHealthBar.fillAmount > healthBar.fillAmount)
        {
            secondaryHealthBar.fillAmount -= 0.0035f;
        }
    }
}

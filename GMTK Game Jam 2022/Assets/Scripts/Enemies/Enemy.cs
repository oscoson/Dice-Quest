using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected int maxHp;
    [HideInInspector] protected int currentHp;
    public List<Sprite> sprites;

    protected Player player;
    public int MaxHp { get => maxHp; }
    public int CurrentHp { get => currentHp; }

    [SerializeField] private new string name;
    public string Name { get => name; }

    private void Awake()
    {
        currentHp = maxHp;
        //EnemyHealthBar.Instance.Init(currentHp, maxHp);
    }

    public void Init(Player player)
    {
        this.player = player;
        currentHp = maxHp;
    }

    public virtual void InflictDamage(int dmg)
    {
        currentHp -= dmg;
        CombatManager.Instance.enemyAnimation.SetBool("isHit", true);
        CombatManager.Instance.UpdateCombatReportText("You dealed " + dmg + " damage!");
        StartCoroutine(hitAnimCancel(0.5f));
        //EnemyHealthBar.Instance.currentHealth = currentHp;
        if (currentHp <= 0) Die();
    }

    public void Die()
    {
        Debug.Log("Die");
        CombatManager.Instance.UpdateCombatReportText("Good job!");
        CombatManager.Instance.EndCombat();
    }

    protected IEnumerator hitAnimCancel(float animTime)
    {
        yield return new WaitForSecondsRealtime(animTime);
        CombatManager.Instance.enemyAnimation.SetBool("isHit", false);
    }

    public abstract void PerformAction();
}

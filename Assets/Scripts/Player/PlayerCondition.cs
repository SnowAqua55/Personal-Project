using System;
using UnityEngine;
using UnityEngine.XR;

public interface IDamagable
{
    void TakePhysicalDamage(int damage);
}

public class PlayerCondition : MonoBehaviour, IDamagable
{
	public UICondition uiCondition;

	Condition health { get { return uiCondition.health; } }
    Condition hunger { get { return uiCondition.hunger; } }
    Condition stamina { get { return uiCondition.stamina; } }

    public float noHungerDamage;

    public event Action onTakeDamage;

    private void Update()
    {
        hunger.Subtract(hunger.passiveValue * Time.deltaTime);
        stamina.Add(stamina.passiveValue * Time.deltaTime);

        if (hunger.curValue <= 0f)
        {
            health.Subtract(noHungerDamage * Time.deltaTime);
        }

        if (health.curValue <= 0f)
        {
            Die();
        }

        if (hunger.curValue > hunger.maxValue * (70f / 100f))
        {
            health.Add(health.maxValue * (1f / 100f));
        }
    }

    public void Heal(float amount)
    {
        health.Add(amount);
    }

    public void Eat(float amount)
    {
        hunger.Add(amount);
    }

    public void Die()
    {
        Debug.Log("아; 디졌다;;;");
    }

    public void TakePhysicalDamage(int damage)
    {
        health.Subtract(damage);
        onTakeDamage?.Invoke();
    }

    public bool UseStamina(float amount)
    {
        if (stamina.curValue - amount < 0f)
            return false;

        stamina.Subtract(amount);
        return true;
    }
}
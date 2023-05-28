using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bait : MonoBehaviour
{
    [Header("Основные параметры")]
    public float protection;
    public float health;


    [Header("Необьходимые компоненты")]
    public GameObject HealthBar;

    public float currentHealth;


    private void Start()
    {
        currentHealth = health;
    }

    private void Update()
    {
        
        updateHealthBar();
    }

    public void getDamage(float incomingDamage)
    {
        float currentDamage = incomingDamage - (incomingDamage / 100 * protection);
        float finalDamage = (currentDamage < 1) ? 1 : currentDamage;
        currentHealth -= finalDamage;
        trackHealth();
    }

    public void getHealth(float incomingHealth)
    {
        float finalHealth = incomingHealth;
        currentHealth += finalHealth;
        trackHealth();
    }

    public void trackHealth()
    {
        currentHealth = currentHealth < 0 ? 0 : currentHealth > health ? health : currentHealth;
        if (currentHealth == 0) Destroy(gameObject);
    }


    public void updateHealthBar()
    {
        HealthBar.SetActive(currentHealth < health);
        HealthBar.GetComponent<Slider>().maxValue = health;
        HealthBar.GetComponent<Slider>().value = currentHealth;
    }
}

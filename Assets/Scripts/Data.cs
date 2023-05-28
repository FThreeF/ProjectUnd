using System.Collections;
using System.Collections.Generic;
using UnityEngine;







public class Data : MonoBehaviour
{
    [Header("Основные параметры")]

    public float protection;
    public float health;
    public float currentHealth;
    public float damage;
    public float penetration;
    public float crit;
    public float critChance;
    public float speed;
    
    

    public void Destruction(float currentDamage, float currentPenetration = 0)
    {
        float mainProtection = (protection - currentPenetration < 0) ? 0 : protection - currentPenetration;
        float mainDamage = (currentDamage - mainProtection < 1) ? 1 : currentDamage - mainProtection;
        currentHealth -= mainDamage;
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void Recovery(float currentHealing)
    {
        float mainHealing = currentHealing;
        currentHealth += mainHealing;
        if (currentHealth > health) currentHealth = health;
    }
}

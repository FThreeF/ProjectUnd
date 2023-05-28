using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Turret : MonoBehaviour
{
    public float protection;
    public float health;
    public float damage;
    public float penetration;
    public float attackSpeed;
    public float crit;
    public float critChance;
    public float turningSpeed;
    public float degreeAttack;
    public float scatter;

    public float rangeAttack;
    public LayerMask target;


    [Header("Необьходимые обьекты")]
    public GameObject Ammo;
    public Transform AttackPoint;
    public GameObject Body;
    public Transform ShotPoint;
    public GameObject HealthBar;

    public AudioSource AudioGun;

    /*Hide*/
    [HideInInspector] public float currentHealth;
    [HideInInspector] public GameObject creator;


    /*Private*/
    private float currentAttackSpeed;
    public float currentTurningSpeed;
    public float finalRotZ = 0;
    public float rotZ;

    private void Start()
    {
        currentHealth = health;
        
    }


    private void Update()
    {
        updateHealthBar();
        updateAmmoParameter();
        
        Collider2D[] attackTarget = Physics2D.OverlapCircleAll(AttackPoint.position, rangeAttack, target);

        

        var priorityTarget = attackTarget.OrderBy(x => Vector3.Distance(transform.position, x.transform.position)).FirstOrDefault();


        AudioGun.mute = attackTarget.Length == 0;
        if (attackTarget.Length != 0)
        {
            Vector3 difference = priorityTarget.transform.position - transform.position;
            rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            //Body.transform.rotation = Quaternion.Euler(0f, 0f, rotZ + 90);
            Body.transform.rotation = Quaternion.Lerp(Body.transform.rotation, Quaternion.Euler(0f, 0f, rotZ + 90), Time.deltaTime * turningSpeed);
            SpawnBullet();


            /*if (currentTurningSpeed <= 0)
            {
                

                \

                
                Body.transform.rotation = Quaternion.Euler(0f, 0f, finalRotZ + 90);
                currentTurningSpeed = turningSpeed;
            }
            else
            {
                currentTurningSpeed -= Time.deltaTime;
            }

            if ((finalRotZ >= 0 && rotZ >= 0) || (finalRotZ < 0 && rotZ < 0))
            {
                if (Mathf.Abs(finalRotZ - rotZ) <= degreeAttack)
                {
                    SpawnBullet();
                }
            }
            else if (finalRotZ >= 0 && rotZ < 0)
            {
                if (Mathf.Abs(finalRotZ - Mathf.Abs(rotZ)) <= degreeAttack)
                {
                    SpawnBullet();
                }
            }
            else if (finalRotZ < 0 && rotZ >= 0)
            {
                if (Mathf.Abs(Mathf.Abs(finalRotZ) - rotZ) <= degreeAttack)
                {
                    SpawnBullet();
                }
            }*/



        }
    }

    public void SpawnBullet()
    {
        if (currentAttackSpeed <= 0)
        {
            var l = Body.transform.rotation;
            l.z = Random.Range(Body.transform.rotation.z - scatter / 2, Body.transform.rotation.z + scatter / 2);
            Instantiate(Ammo, ShotPoint.position, l);
            currentAttackSpeed = attackSpeed;
        }
        else
        {
            currentAttackSpeed -= Time.deltaTime;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(AttackPoint.position, rangeAttack);
    }

    public void updateAmmoParameter()
    {
        Ammo.GetComponent<Ammo>().creator = (creator) ? creator : gameObject;
        Ammo.GetComponent<Ammo>().damage = damage;
        Ammo.GetComponent<Ammo>().target = target;
    }


    public void getDamage(float incomingDamage, float incomingPenetration = 0)
    {
        float finalProtection = (protection - incomingPenetration < 0) ? 0 : protection - incomingPenetration;
        float finalDamge = (incomingDamage - finalProtection < 1) ? 1 : incomingDamage - finalProtection;
        currentHealth -= finalDamge;
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

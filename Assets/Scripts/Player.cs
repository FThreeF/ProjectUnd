using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Data.SqlTypes;
using Unity.Collections;
using Unity.VisualScripting;
using System;

public class Player : MonoBehaviour
{
    [Header("Основные параметры")]
    public float protection;
    public float health;
    public float damage;
    public float regeneration;
    public float attackSpeed;
    public float speed;
    public float scatter;

    [Header("Дополнительные параметры")]

    public int ammo;
    public float reloading;
    public LayerMask attackTarget;
    public float chip;
    

    [Header("Выбор типа управления")]
    public ControlType controlType;
    public enum ControlType { PC, Android }

    [Header("Необходимые объекты")]
    public Slider HealthBar;
    public Text TextHealthBar;
    public GameObject Ammo;
    public Slider AmmoReloadBar;
    public Text TextAmmoReloadBar;
    public Transform ShotPoint;
    public Rigidbody2D MainRigidBody;
    public GameObject Body;
    public Joystick JoystickMove;
    public Joystick JoystickAttack;
    public Text TextWaves;
    public Text TextNumberEnemy;
    public Text TextTimeWaves;
    public Text TextChipAmount;
    public Text TextHealth;
    public Text TextProtection;
    public Text TextDamage;
    public Text TextRegeneration;

    public Text TextDescriptionTurret;
    public Text TextDescriptionBait;
    public Text TextPriceDamage;
    public Text TextPriceHealth;
    public Text TextPriceRegeneration;
    public Text TextPriceProtection;
    public Text TextEvent;

    public GameObject Engineer;
    public Text TextChipAmountTwo;

    public AudioSource AudioWalk;
    public AudioSource AudioGun;
    public Text TextC;
    
     
    public GameObject Turret;
    public GameObject Bait;

    [HideInInspector] public float currentHealth;

    /*Private*/
    private Vector2 moveInput;
    private Vector2 moveVelocity;
    private float rotZ;
    private float currentReloading;
    private int currentAmmo;
    private float currentAttackSpeed;
    private float currentPriceDamage = 2;
    private float currentPriceHealth = 1;
    private float currentPriceRegeneration = 5;
    private float currentPriceProtection = 10;

    private float timeRegeneration = 0;



    

    

    private void Start()
    {
        currentAmmo = ammo;
        currentHealth = health;
        
    }

    private void Update()
    {
        updateHealthBar();
        updateAmmoReloadBar();
        updateAmmoParameter();
        updateTextChipAmount();
        updateTextParameters();
        updateParametersTurret();
        updateParametersBait();
        updateTextTurret();
        updateTextBait();
        updateParametersEngineer();

        TextC.text = MainRigidBody.position.x + ":" + MainRigidBody.position.y;

        if (currentHealth < health)
        {
            if (timeRegeneration >= 1)
            {
                currentHealth += regeneration;
                timeRegeneration = 0;
                if (currentHealth > health) currentHealth = health;
            }
            else
            {
                timeRegeneration += Time.deltaTime;
            } 
        }

        TextPriceDamage.text = currentPriceDamage.ToString();
        TextPriceHealth.text = currentPriceHealth.ToString();
        TextPriceRegeneration.text = currentPriceRegeneration.ToString();
        TextPriceProtection.text = currentPriceProtection.ToString();

       




        if (currentAmmo > 0)
        {
            if (currentAttackSpeed <= 0)
            {
                
                var l = Body.transform.rotation;
                l.z = UnityEngine.Random.Range(Body.transform.rotation.z - scatter/2, Body.transform.rotation.z + scatter/2);
                if (JoystickAttack.Horizontal != 0 || JoystickAttack.Vertical != 0)
                {
                    AudioGun.mute = false;
                    Instantiate(Ammo, ShotPoint.position, l);
                    currentAttackSpeed = attackSpeed;
                    currentAmmo--;
                }
                else
                {
                    AudioGun.mute = true;
                }
            }
            else
            {
                currentAttackSpeed -= Time.deltaTime;
            }
        }
        else
        {
            AudioGun.mute = true;
            if (currentReloading >= reloading)
            {
                currentAmmo = ammo;
                currentReloading = 0;
            }
            else
            {
                currentReloading += Time.deltaTime;
            }
        }



        if (JoystickAttack.Horizontal != 0 || JoystickAttack.Vertical != 0)
        {
            rotZ = Mathf.Atan2(JoystickAttack.Vertical, JoystickAttack.Horizontal) * Mathf.Rad2Deg;
            Body.transform.rotation = Quaternion.Euler(0f, 0f, rotZ + 90);

        }
        else if (JoystickMove.Horizontal != 0 || JoystickMove.Vertical != 0)
        {   
            rotZ = Mathf.Atan2(JoystickMove.Vertical, JoystickMove.Horizontal) * Mathf.Rad2Deg;
            Body.transform.rotation = Quaternion.Euler(0f, 0f, rotZ + 90);
        }
        



        if (controlType == ControlType.PC)
        {
            

            moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }
        else if (controlType == ControlType.Android)
        {
            moveInput = new Vector2(JoystickMove.Horizontal, JoystickMove.Vertical);
        }


        moveVelocity = moveInput.normalized * speed;
    }

    private void FixedUpdate()
    {
        MainRigidBody.MovePosition(MainRigidBody.position + moveVelocity * Time.fixedDeltaTime);

        AudioWalk.mute = (moveVelocity.x == 0 && moveVelocity.y == 0);

    }







    /*Взаимодействие со здоровьем*/

    public void getDamage(float incomingDamage)
    {
        float finalDamage = (incomingDamage - protection < 1) ? 1 : incomingDamage - protection;
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
        if (currentHealth == 0) UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
        if (currentHealth == 0) Destroy(gameObject);
    }




    public void updateAmmoReloadBar()
    {
        TextAmmoReloadBar.text = currentAmmo + "/" + ammo;
        AmmoReloadBar.maxValue = reloading;
        AmmoReloadBar.value = currentReloading;
    }


    public void updateAmmoParameter()
    {
        Ammo.GetComponent<Ammo>().creator = gameObject;
        Ammo.GetComponent<Ammo>().damage = damage;

        Ammo.GetComponent<Ammo>().target = attackTarget;
    }

    public void updateHealthBar()
    {
        HealthBar.maxValue = health;
        HealthBar.value = currentHealth;
        TextHealthBar.text = Mathf.Ceil(currentHealth) + "/" + health;
    }

    public void getChip(float incomingChip)
    {
        chip += incomingChip;
       
    }



    public void updateTextTurret()
    {
        TextDescriptionTurret.text = "Прочность: 25% от хп ("
            + Turret.GetComponent<Turret>().health + ")\nУрон: 15% от силы ("
            + Turret.GetComponent<Turret>().damage + ")";

    }

    public void updateTextBait()
    {
        TextDescriptionBait.text = "Прочность: 350% от хп (" + Bait.GetComponent<Bait>().health + ")";
    }

    public void updateParametersTurret()
    {
        Turret.GetComponent<Turret>().health = (health / 100) * 25;
        Turret.GetComponent<Turret>().damage = (damage / 100) * 15;
    }

    public void updateParametersBait()
    {
        Bait.GetComponent<Bait>().health = (health / 100) * 350;
    }

    public void updateParametersEngineer()
    {
        Engineer.GetComponent<Bait>().health = (health / 100) * 5000;
    }

    public void updateTextParameters()
    {
        TextHealth.text = health.ToString();
        TextProtection.text = protection.ToString() + "%";
        TextDamage.text = damage.ToString();
        TextRegeneration.text = Convert.ToDouble(regeneration).ToString();
    }

    public void upgradeParameters(string name)
    {
        switch (name)
        {
            case "health":
                {
                    if (chip >= currentPriceHealth)
                    {
                        chip -= currentPriceHealth;
                        health += 10;
                        currentHealth += 10;
                        currentPriceHealth++;
                    }

                }
                break;
            case "damage":
                {
                    if (chip >= currentPriceDamage)
                    {
                        chip -= currentPriceDamage;
                        currentPriceDamage += 2;
                        damage++;
                    }

                }
                break;
            case "regeneration":
                {
                    if (chip >= currentPriceRegeneration)
                    {
                        chip -= currentPriceRegeneration;
                        currentPriceRegeneration += 5;
                        regeneration += 0.5f;
                    }
                }
                break;
            case "protection":
                {
                    if (chip >= currentPriceProtection)
                    {
                        chip -= currentPriceProtection;
                        currentPriceProtection += 10;
                        protection += 0.5f;
                    }
                }
                break;
        }
    }
    public void onHelp()
    {
        gameObject.transform.position = new Vector3(0, 0, 0);
    }

    public void updateTextChipAmount()
    {
        TextChipAmount.text = chip.ToString();
        TextChipAmountTwo.text = chip.ToString();
    }

    public void spawnObj(string name)
    {
        if (name == "turret")
        {
            if (chip >= 10)
            {
                chip -= 10;
                Turret.GetComponent<Turret>().creator = gameObject;
                Instantiate(Turret, gameObject.transform.position, Quaternion.Euler(0f, 0f, 0f));
            }
        }
        else if (name == "bait")
        {
            if (chip >= 5)
            {
                chip -= 5;
                Instantiate(Bait, gameObject.transform.position, Quaternion.Euler(0f, 0f, 0f));
            }
        }
        else if (name == "damage") damage += 50;
    }
}

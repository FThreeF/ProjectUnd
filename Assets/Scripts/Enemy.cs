using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header("Основные параметры")]
    public float health;
    public float damage;
    public float attackSpeed;
    public float speed;
    public float chip;
    public float addHealthWaves; 

    [Header("Настройка коллайдеров")]
    public Transform pointMove;
    public float rangeMove;
    public Transform pointAttack;
    public float rangeAttack;
    public Transform pointStopMove;
    public float rangeStopMove;
    public LayerMask target;


    [Header("Передача елементов")]
    public GameObject body;
    public GameObject HealthBar;
    public GameObject HealthBarInteractive;
    public GameObject deadBody;

    public AudioSource AudioWalk;


    /*Private*/
    private float currentHealth;
    private float currentAttackSpeed;
    private Vector3 difference;
    private float rotZ;
    private bool checkHealthBarInteractive = false;

    public void setValue()
    {
        currentHealth = health;
        currentAttackSpeed = attackSpeed;
        HealthBarInteractive.GetComponent<Slider>().maxValue = health;
        HealthBarInteractive.GetComponent<Slider>().value = currentHealth;
        
    }

    private void Start()
    {
        setValue();        
        updateHealthBar();
    }

    private void Update()
    {




        

        Collider2D[] moveTarget = Physics2D.OverlapCircleAll(pointMove.position, rangeMove, target);
        
        Collider2D[] stopMoveTarget = Physics2D.OverlapCircleAll(pointStopMove.position, rangeStopMove, target);
        AudioWalk.mute = (moveTarget.Length == 0 || stopMoveTarget.Length != 0);

        Collider2D[] attackTarget = Physics2D.OverlapCircleAll(pointAttack.position, rangeAttack, target);


        var priorityMoveTarget = moveTarget.OrderBy(x => Vector3.Distance(transform.position, x.transform.position)).FirstOrDefault();
        if (moveTarget.Length != 0)
        {
            if (stopMoveTarget.Length == 0)
            {
                transform.position = Vector2.MoveTowards(transform.position, priorityMoveTarget.transform.position, speed * Time.deltaTime);
            }
            difference = priorityMoveTarget.transform.position - transform.position;
            rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            body.transform.rotation = Quaternion.Euler(0f, 0f, rotZ + 90);
        }

        var priorityAttackTarget = attackTarget.OrderBy(x => Vector3.Distance(transform.position, x.transform.position)).FirstOrDefault();
        if (currentAttackSpeed <= 0)
        {
            if (attackTarget.Length != 0)
            {
                if (priorityAttackTarget.GetComponent<Player>() != null)
                {
                    priorityAttackTarget.GetComponent<Player>().getDamage(damage);
                }
                else if (priorityAttackTarget.GetComponent<Turret>() != null)
                {
                    priorityAttackTarget.GetComponent<Turret>().getDamage(damage);
                }
                else if (priorityAttackTarget.GetComponent<Bait>() != null)
                {
                    priorityAttackTarget.GetComponent<Bait>().getDamage(damage);
                }  
                

                
                currentAttackSpeed = attackSpeed;
            }
        }
        else
        {
            currentAttackSpeed -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        if (checkHealthBarInteractive) updateHealthBarInteractive();
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(pointMove.position, rangeMove);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(pointStopMove.position, rangeStopMove);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(pointAttack.position, rangeAttack);
    }



    /*Function*/

    public void getDamage(float incomingDamage, GameObject attaking = null)
    {
        currentHealth -= incomingDamage;
        if (attaking)
        {
            if (attaking.GetComponent<Player>())
            {
                if (currentHealth <= 0) attaking.GetComponent<Player>().getChip(chip);
                
            }    
        }
        if (currentHealth <= 0)
            Instantiate(deadBody, gameObject.transform.position, body.transform.rotation);
        trackHealth();
    }

    public void getHealth(float incomingHealth)
    {
        float finalHealth = incomingHealth;
        currentHealth += finalHealth;
        trackHealth();
    }

    public void trackHealth(GameObject attaking = null)
    {
        currentHealth = currentHealth < 0 ? 0 : currentHealth > health ? health : currentHealth;
        if (currentHealth == 0) Destroy(gameObject);
        updateHealthBar();
    }

    public void updateHealthBar()
    {
        HealthBar.SetActive(currentHealth < health);
        HealthBar.GetComponent<Slider>().maxValue = health;
        HealthBar.GetComponent<Slider>().value = currentHealth;
        checkHealthBarInteractive = true;
    }

    public void updateHealthBarInteractive()
    {
        if (HealthBarInteractive.GetComponent<Slider>().value > HealthBar.GetComponent<Slider>().value)
        {
            HealthBarInteractive.GetComponent<Slider>().value -= (health / 200);
        }
        else
        {
            checkHealthBarInteractive = false;
        }
    }
}

using UnityEngine;

public class Rage : DataRage
{
    public Enemy dataEnemy;
    public float maxSpeed;
    public float mainSpeed;

    public float maxDamage;
    public float mainDamage;

    public float maxProtection;
    public float mainProtection;

    public bool checkBonusSpeed = false;
    public bool checkBonusDamage = false;
    public bool checkBonusProtection = false;
    public bool checkBonusRegeneration = false;
    public bool checkBonusRegionRegeneration = false;

    [Header("Регенерация")]
    public float regionRegeneration;
    public Transform pointRegeneration;
    public float rangeRegeneration;
    public LayerMask targer;
    private float currentRegenerationTime;
    private Collider2D[] regenerationTarget;
    private void Start()
    {
        mainSpeed = dataEnemy.speed;
        mainDamage = dataEnemy.damage;


        mainTimeRage = Random.Range(minTimeRage, maxTimeRage);
        mainDurationRage = Random.Range(minDurationRage, maxDurationRage);
        currentDurationRage = mainDurationRage;
        currentTimeRage = Random.Range(0, mainTimeRage);

    }
    private void Update()
    {

  


        if (currentTimeRage <= 0)
        {
            dataEnemy.body.GetComponent<SpriteRenderer>().sprite = spriteRage;

            if (checkBonusSpeed) dataEnemy.speed = maxSpeed;
            if (checkBonusDamage) dataEnemy.damage = maxDamage;

            if (checkBonusRegionRegeneration)
            {
                regenerationTarget = Physics2D.OverlapCircleAll(pointRegeneration.position, rangeRegeneration, targer);
                if (currentRegenerationTime <= 0)
                {
                    foreach (var obj in regenerationTarget)
                    {
                        if (obj.GetComponent<Enemy>())
                        {
                            obj.GetComponent<Enemy>().getHealth(regionRegeneration);
                        }
                    }
                    currentRegenerationTime = 0.01f;
                }
                else
                {
                    currentRegenerationTime -= Time.fixedDeltaTime;
                }
            }



            if (currentDurationRage <= 0)
            {
                dataEnemy.body.GetComponent<SpriteRenderer>().sprite = spriteNormal;
                
                dataEnemy.speed = mainSpeed;
                dataEnemy.damage = mainDamage;



                currentTimeRage = mainTimeRage;
                currentDurationRage = mainDurationRage;
            }
            else
            {
                
                currentDurationRage -= Time.fixedDeltaTime;
            }

        }
        else
        {
            currentTimeRage -= Time.fixedDeltaTime;
        }



        

 
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(pointRegeneration.position, rangeRegeneration);
    }
}

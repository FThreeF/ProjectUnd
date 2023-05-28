using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Runtime.ExceptionServices;
using Unity.Mathematics;
using UnityEngine;




[System.Serializable]
public class SpawnEnemy
{
    public SpawnObject scriptSpawnObject;
    public int startWave; //Появляется на волне х
    public int startEnemy; //Количество врагов на первом появлении
    public float addition; //Прирост врага с волной
    public int maxEnemy; //Максимальное количетсво врага
    public int peredicitySpawn; //Переодичность появления врага
    public int maxWaves; //Переломная волна на которой количество врага сменяется на константу
    public int constEnemy; //Константа появления врага

    [Header("Параметры")]
    public float wavesHealth;
    public float wavesDamage;

    public float chance = 0f;
    public string message;
}


public class WavesManager : MonoBehaviour
{
    public Transform pointCollider;
    public float rangeCollider;
    public LayerMask maskPlayer;
    public LayerMask maskEnemy;

    public float breakTime;
    private float currentBreakTime;
    private int waves;

    public List<SpawnEnemy> listSpawnEnemy;

    private Collider2D[] findObjectTrigger;
    private Collider2D[] findEnemy;
    

    private void Start()
    {
        Application.targetFrameRate = 60;
        currentBreakTime = breakTime;
        foreach (var element in listSpawnEnemy)
        {
            if (element.wavesHealth != 0)
            {
                element.scriptSpawnObject.objectSpawn.GetComponent<Enemy>().health = element.wavesHealth * waves;
            }
            if (element.wavesDamage != 0)
            {
                element.scriptSpawnObject.objectSpawn.GetComponent<Enemy>().damage = element.wavesDamage * waves;
            }
        }
    }

    private void FixedUpdate()
    {

        findObjectTrigger = Physics2D.OverlapCircleAll(pointCollider.position, rangeCollider, maskPlayer);
        findEnemy = Physics2D.OverlapCircleAll(pointCollider.position, rangeCollider, maskEnemy);

        UpdatePlayerText();

        
      
        

     

        if (currentBreakTime <= 0)
        {
            waves++;
            takeMoney();
            currentBreakTime = breakTime;

            foreach (var element in listSpawnEnemy)
            {
                
                if (element.wavesHealth != 0)
                {
                    element.scriptSpawnObject.objectSpawn.GetComponent<Enemy>().health = element.wavesHealth * waves;
                }
                if (element.wavesDamage != 0)
                {
                    element.scriptSpawnObject.objectSpawn.GetComponent<Enemy>().damage = element.wavesDamage * waves;
                }
                
                
            }
            UpdateSpawnObject();
        }
        else
        {
            if (findEnemy.Length == 0)
            {
                currentBreakTime -= Time.fixedDeltaTime;
               
               
            }
        }



        




    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(pointCollider.position, rangeCollider);
    }


    public void UpdateSpawnObject()
    {
        foreach (var element in listSpawnEnemy)
        {
            if (waves >= element.startWave)
            {
                if (element.chance == 0)
                {
                    if (waves % element.peredicitySpawn == 0)
                    {
                        float count = element.startEnemy + (element.addition * Mathf.Floor((waves - element.startWave) / element.peredicitySpawn));
                        //if (count > element.maxEnemy) count = element.maxEnemy;
                        if (waves >= element.maxWaves) count = element.constEnemy;
                        element.scriptSpawnObject.CountCreateObject(Convert.ToInt32(Math.Floor(count)));
                    }
                }
                else if (element.chance != 0)
                {
                    var currentSpawnChance = UnityEngine.Random.Range(0, 100);
                    if (element.chance > currentSpawnChance)
                    {
                        float count = element.startEnemy + (element.addition * Mathf.Floor((waves - element.startWave) / element.peredicitySpawn));
                        if (waves >= element.maxWaves) count = element.constEnemy;
                        element.scriptSpawnObject.CountCreateObject(Convert.ToInt32(Math.Floor(count)));



                        if (findObjectTrigger.Length != 0)
                        {
                            foreach (var obj in findObjectTrigger)
                            {
                                if (obj.GetComponent<Player>().TextEvent.text == "")
                                {
                                    obj.GetComponent<Player>().TextEvent.text += element.message;
                                }
                                else
                                {
                                    obj.GetComponent<Player>().TextEvent.text += "\n" + element.message;
                                }
                                
                            }
                        }
                    }
                }
                
            }
        }
    }
    public void UpdatePlayerText()
    {
        if (findObjectTrigger.Length != 0)
        {
            foreach (var obj in findObjectTrigger)
            {
                obj.GetComponent<Player>().TextNumberEnemy.text = findEnemy.Length.ToString();
                obj.GetComponent<Player>().TextWaves.text = waves.ToString();
                if (findEnemy.Length == 0 && Mathf.Round(currentBreakTime) != 0)
                {
                    obj.GetComponent<Player>().TextTimeWaves.text = Mathf.Round(currentBreakTime).ToString();
                    obj.GetComponent<Player>().TextEvent.text = "";
                }
                else
                {
                    obj.GetComponent<Player>().TextTimeWaves.text = "";
                }

            }
        }
    }

    public void takeMoney()
    {
        if (findObjectTrigger.Length != 0)
        {
            foreach (var obj in findObjectTrigger)
            {
                obj.GetComponent<Player>().chip += waves * 1;
            }
        }            
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EN_0 : MonoBehaviour
{
    public float minTimePassive;
    public float maxTimePassive;
    public float timeActive;
    public float timeAgression;

    public float speed;

    public List<Sprite> sprites;
    public Enemy objectData;
    public SpriteRenderer objectSprite;
    public SpriteRenderer effectSprite;

    private float currentTimePassive;
    private float currentTimeActive;
    private float currentTimeAgression;
    private float speedMain;
    private float timePassive;

    private void Start()
    {
        timePassive = Random.Range(minTimePassive, maxTimePassive);
        currentTimePassive = Random.Range(0, timePassive);
        speedMain = objectData.speed;
    }

    private void FixedUpdate()
    {
        if (currentTimePassive >= timePassive)
        {
            objectSprite.sprite = sprites[1];

            if (currentTimeActive >= timeActive)
            {
                objectSprite.sprite = sprites[2];
                effectSprite.sprite = sprites[3];
                objectData.speed = speed;
                if (currentTimeAgression >= timeAgression)
                {
                    timePassive = Random.Range(minTimePassive, maxTimePassive);
                    objectSprite.sprite = sprites[0];
                    effectSprite.sprite = null;
                    objectData.speed = speedMain;
                    currentTimePassive = 0;
                    currentTimeActive = 0;
                    currentTimeAgression = 0;
                }
                else
                {
                    currentTimeAgression += Time.fixedDeltaTime;
                }
            }
            else
            {
                currentTimeActive += Time.fixedDeltaTime;
            }

            
        }
        else
        {
            currentTimePassive += Time.fixedDeltaTime;
        }
    }
}

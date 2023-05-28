using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    [Header("Основные параметры")]
    public float speed;
    public float lifeTime;
    public float rangeCollider;

    [Header("Необходимые объекты")]
    public GameObject boxCollider;
    public Transform pointColliderOne;
    public Transform pointColliderTwo;
    public LayerMask currentTarget;

    /*Hide parameter*/
    [HideInInspector] public GameObject creator;
    [HideInInspector] public float damage;
    [HideInInspector] public LayerMask target;

    /*Private*/
    private Collider2D foundTarget;


    private void Start()
    {
       
    }



    private void FixedUpdate()
    {
        doMove();
        trackLifeTime();
        trackTarget();
    }

    /*Отрисовка коллайдеров*/
    private void OnDrawGizmosSelected()
    {
/*        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(boxCollider.transform.position, rangeCollider);*/

        Gizmos.color = Color.red;
        Gizmos.DrawLine(pointColliderOne.position, pointColliderTwo.position);
    }

    /*Function*/
    private void doMove()
    {
        transform.Translate(Vector2.down * speed * Time.fixedDeltaTime);
    }
    private void trackTarget()
    {
        findTarget();
        if (foundTarget == null) return;
        if (foundTarget.GetComponent<Enemy>()) foundTarget.GetComponent<Enemy>().getDamage(damage, creator);
        Destroy(gameObject);
    }
    private void findTarget()
    {
        /*foundTarget = Physics2D.OverlapCircle(boxCollider.transform.position, rangeCollider, target);*/
        foundTarget = Physics2D.OverlapArea(pointColliderOne.position, pointColliderTwo.position, currentTarget);
    }
    private void trackLifeTime()
    {
        if (lifeTime <= 0) Destroy(gameObject);
        else lifeTime -= Time.deltaTime;
    }
}

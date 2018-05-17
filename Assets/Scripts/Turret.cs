using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{

    public Transform target;
    public Transform rotate;
    public static Turret instance; 
    [Header("Attributtes")]


    public float range = 15f;
    public float turnSpeed = 5f;
    public float fireRate = 2f;
    public float fireCtdwn = 1f;

    public GameObject bulletPrefab;
    public GameObject alert;
    public Transform firePoint;

    // Use this for initialization
    void Start()
    {
        instance = this;
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    public void TurretReset()
    {
        fireCtdwn = 1;
    }

    //Search objects mareked as  "". Mark closests, check in range and set target = that object 
    //Not done every frame because highly computational
    //Fixed basis
    void UpdateTarget()
    {
        //Find gameobj with tag "Player"
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        float shortestDist = Mathf.Infinity;

        //checking distance between turrent and player
        float distToEnemy = Vector3.Distance(transform.position, player.transform.position);

        if (distToEnemy < shortestDist)
        {
            shortestDist = distToEnemy;
        }

        if (shortestDist<=range)
        {
            target = player.transform;
        }

        else
        {
            target = null;
        }

    }



    // Update is called once per frame
    void Update()
    {
        if(target==null)
        {
            alert.SetActive(false);
            return;
        }

        //Find direction between player position to target position
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(rotate.rotation,lookRotation,Time.deltaTime*turnSpeed).eulerAngles;
        rotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);

        alert.SetActive(true);

        if(fireCtdwn>0)
        {
            Shoot();
        }

        fireCtdwn--;

    }

    void Shoot()
    {
        GameObject bGo= (GameObject)Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);  //Object Casting
        Bullet bullet = bGo.GetComponent<Bullet>();

        if(bullet!=null)
        {
            bullet.Chase(target);
        }

    }


    //Draw range finder gizmo. Select the turrent to view the range
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}

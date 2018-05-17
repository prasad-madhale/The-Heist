using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    private Transform target;
    public float timePenalty;

    public float bSpeed = 50f;
    public void Chase(Transform _target)
    {
        target = _target;
    }
	// Update is called once per frame
	void Update () {
		if(target==null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = bSpeed * Time.deltaTime;

        if(dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
        }

        transform.Translate(dir.normalized * distanceThisFrame,Space.World);

	}

    void HitTarget()
    {
        GameObject time = GameObject.Find("Timer");
        Timer timer = time.GetComponent<Timer>();
        timer.timelimit -= timePenalty;
        Destroy(gameObject);
    }
}

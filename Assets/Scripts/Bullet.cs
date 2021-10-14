using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Transform target;
    
    [Header("Attributes")]
    public float speed = 70f;
    public int damage = 50;
    // TODO: add particle effect (e5)

    public void Seek(Transform _target)
    {
        target = _target;
    }
    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }
        
        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        transform.LookAt(target);

    }
    
    // TODO: find which video this code is relevant, might remove before alpha build
    void HitTarget ()
    {
        // GameObject effectIns = (GameObject)Instantiate(impactEffect, transform.position, transform.rotation);
        // Destroy(effectIns, 5f);
        //
        // if (explosionRadius > 0f)
        // {
        //     Explode();
        // } else
        // {
        Damage(target);
        // }

        Destroy(gameObject);
    }
    
    // TODO: connect with enemy code
    void Damage (Transform enemy)
    {
        // Enemy e = enemy.GetComponent<Enemy>();
        //
        // if (e != null)
        // {
        //     e.TakeDamage(damage);
        // }
    }
}

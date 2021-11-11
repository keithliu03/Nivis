using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretLaser : MonoBehaviour
{

    private Transform target;

    [Header("Attributes")]

    public float range = 15f;
    public int damageOverTime = 10;
    public float fireRate = 5f;

    public Transform firePoint;
    public LineRenderer lineRenderer;

    public bool disabled = false;
    public MeshRenderer mRend;
    public Color defaultColor;
    private float disableCountdown = 10f;

    [Header("Unity Setup Fields")]

    // TODO: set enemy prefab with the enemy tag
    public string enemyTag = "Enemy";

    // TODO: logic to rotate the turret when it sees an enemy (e4)
    void Start()
    {
        InvokeRepeating("UpdateClosestTarget", 0f, 0.5f);
        defaultColor = mRend.material.color;
    }

    void Update()
    {
        if (disabled == true)
        {
            disableCountdown -= Time.deltaTime;
            if (disableCountdown <= 0)
            {
                Enable();
            }
        }

        if (target == null)
        {
            if (lineRenderer.enabled)
            {
                lineRenderer.enabled = false;
            }
            return;
        }

        LockOnTarget();

        if (disabled != true)
        {
            Laser();
        }
    }

    void LockOnTarget()
    {
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        //Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime);
        //partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }

    void Laser()
    {
        target.GetComponent<Enemy>().TakeDamage(damageOverTime * Time.deltaTime * fireRate);

        if (!lineRenderer.enabled)
        {
            lineRenderer.enabled = true;
        }

        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, target.position);
    }

    void UpdateClosestTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }

        }

        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
        }
        else
        {
            target = null;
        }
    }

    public void Disable()
    {
        disabled = true;
        mRend.material.SetColor("_Color", Color.red);
        disableCountdown = 10f;
    }

    public void Enable()
    {
        disabled = false;
        mRend.material.color = defaultColor;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}

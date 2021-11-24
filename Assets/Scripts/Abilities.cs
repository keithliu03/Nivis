using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Abilities : MonoBehaviour
{
    public BuildManager buildManager;
    public Generator generator;

    public int reenableTurretCost = 100, 
               tempShieldCost = 200, 
               stunAreaCost = 250;

    public float tempShieldDuration = 10;
    public float tempShieldDurationTimeStamp;
    public int tempShieldHP = 30;

    //CD are in seconds
    public float reenableTurretCD = 10,
                 tempShieldCD = 100,
                 stunAreaCD = 100;

    private float reenableTurretTimeStamp,
                  tempShieldTimeStamp,
                  stunAreaTimeStamp;

    private bool reenableTurretOnCD = false,
                 tempShieldOnCD = false,
                 stunAreaOnCD = false;

    public GameObject shieldIcon;
    public GameObject shieldMod;

    void Start()
    {
        shieldIcon.SetActive(false);
        shieldMod.SetActive(false);
    }

    void Update()
    {
        if (reenableTurretOnCD == true)
        {
            if (reenableTurretTimeStamp <= Time.time)
            {
                reenableTurretOnCD = false;
            }
        }
        
        if (tempShieldOnCD == true) 
        {
            if (tempShieldTimeStamp <= Time.time)
            {
                tempShieldOnCD = false;
                shieldIcon.SetActive(false);
                shieldMod.SetActive(false);
            }

            int shieldModVal = (int)((float)generator.shieldHealth
                / (float)tempShieldHP * 100.0f);
            if (shieldModVal <= 0 || tempShieldDurationTimeStamp <= Time.time)
            {
                generator.shieldHealth = 0;
                shieldIcon.SetActive(false);
                shieldMod.SetActive(false);
            }

            shieldMod.GetComponent<Text>().text = "+" + shieldModVal + "%";
        }

        if (stunAreaOnCD == true)
        {
            if (stunAreaTimeStamp <= Time.time)
            {
                stunAreaOnCD = false;
            }
        }
        
    }

    public void reenableTurret()
    {
        GameObject turret = buildManager.GetSelectedNode().turret;
        if (turret != null)
        {
            if (reenableTurretOnCD == false)
            {
                if (turret.GetComponent<Turret>().disabled == true)
                {
                    turret.GetComponent<Turret>().Enable();
                    buildManager.drill.currentMoney -= reenableTurretCost;
                    reenableTurretTimeStamp = Time.time + reenableTurretCD;
                    reenableTurretOnCD = true;
                }
                else
                {
                    Debug.Log("Turret not disabled!");
                }
            }
            else
            {
                Debug.Log("Ability on CD!");
            }
        }
    }

    public void tempShield()
    {
        if(tempShieldOnCD == false)
        {
            generator.shieldHealth = tempShieldHP;
            buildManager.drill.currentMoney -= tempShieldCost;
            tempShieldDurationTimeStamp = Time.time + tempShieldDuration;
            tempShieldTimeStamp = Time.time + tempShieldCD;
            tempShieldOnCD = true;
            shieldIcon.SetActive(true);
            shieldMod.SetActive(true);
            Debug.Log("HP: " + generator.totalHealth + " Shield: " + generator.shieldHealth);
        }
        else
        {
            Debug.Log("Ability on CD!");
        }
    }        

    public void stunArea()
    {

    }
}

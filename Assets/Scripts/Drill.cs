﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Drill : MonoBehaviour
{
    public float currentMoney = 3000f;
    public float moneyRate = 5.5f;
    public int drillLvl = 1;
    public float upgradeCost = 400f;

    public Text totalMoneyText/*, upgradeCostText*/;
    public GameObject generator;
    Generator gen;

    void Awake()
    {
        gen = generator.GetComponent<Generator>();
    }

    void Update()
    {
        currentMoney += moneyRate * Time.deltaTime;
        totalMoneyText.text = "$"+currentMoney.ToString("0") + "/Lvl: " + drillLvl.ToString();//sector 3 digits by ,
        //upgradeCostText.text = "$" + upgradeCost.ToString();
    }

    public void Upgrade()
    {
        if (drillLvl < 4)
        {
            if (currentMoney > upgradeCost &&
                gen.totalHealth > 30)
            {
                gen.TakeDamage(30);
                currentMoney -= upgradeCost;
                drillLvl++;
                upgradeCost = drillLvl * 300;
                moneyRate = drillLvl * 5f;
            }
        }
    }


}

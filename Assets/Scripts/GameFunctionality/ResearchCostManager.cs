using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResearchCostManager : MonoBehaviour
{
    public int killCount = 100;
    public int[,,] researchCost;
    private void Start()
    {
        researchCost = getResearchCosts();//page,branch,upgrades
    }
    
    void Update()
    {
        Text killCountText = GameObject.Find("KillCount").GetComponent<Text>();
        killCountText.text = "Kills: " + killCount;
    }

    public static int[,,] getResearchCosts()
    {
        return new int[,,]
        {
            {   // turret 1, path 1, upgrades 1-3
                {100, 150, 200},
                // turret 1, path 2, upgrades 1-3
                {100, 150, 200}
            },
            {   // turret 2, path 1, upgrades 1-3
                {100, 150, 200},
                // turret 2, path 2, upgrades 1-3
                {100, 150, 200}
            },
            {   // turret 3, path 1, upgrades 1-3
                {100, 150, 200},
                // turret 3, path 2, upgrades 1-3
                {100, 150, 200}
            },
        };
    }
    public bool checkIfCanResearch(int turret, int path, int tier)
    {
        if (researchCost[turret, path, tier] > killCount)
        {
            Debug.Log("Not enough kills to research tech");
            return false;
        }
        else
        {
            Debug.Log("Enough kills to research tech");
            killCount -= researchCost[turret, path, tier];
            ResearchStation.researched[turret, path, tier] = true;
            return true;
        }
    }
}
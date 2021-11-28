using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A container for the unlocked upgrades of:
/// -standard turret
/// -missile launcher
/// -radiator tower
/// </summary>
public class ResearchStation : MonoBehaviour
{
    GameObject leftBtn, rightBtn, btn2, btn3;
    GameObject pageName;
    GameObject[] b1Img, b2Img;
    GameObject[] b1, b2;
    int page = 0;
    const int LAST_PAGE = 2;
    Text price1Text, price2Text, price3Text;

    public static bool[,,] researched;
    public ResearchCostManager rcm;//manager for the cost of each upgrade
    public OptionDescriptor optionDescriptor;

    void Awake()
    {
        leftBtn = GameObject.Find("Canvas/ResearchStation/InnerEl/Row1/Btn1");
        rightBtn = GameObject.Find("Canvas/ResearchStation/InnerEl/Row1/Btn4");
        pageName = GameObject.Find("Canvas/ResearchStation/InnerEl/Row1/PageName");
        b1Img = new GameObject[3];
        b2Img = new GameObject[3];
        b1 = new GameObject[3];
        b2 = new GameObject[3];
        b1Img[0] = GameObject.Find("Canvas/ResearchStation/InnerEl/Row2/1a/Image");
        b1Img[1] = GameObject.Find("Canvas/ResearchStation/InnerEl/Row3/1a/Image");
        b1Img[2] = GameObject.Find("Canvas/ResearchStation/InnerEl/Row4/1a/Image");
        b2Img[0] = GameObject.Find("Canvas/ResearchStation/InnerEl/Row2/1b/Image");
        b2Img[1] = GameObject.Find("Canvas/ResearchStation/InnerEl/Row3/1b/Image");
        b2Img[2] = GameObject.Find("Canvas/ResearchStation/InnerEl/Row4/1b/Image");
        b1[0] = GameObject.Find("Canvas/ResearchStation/InnerEl/Row2/1a");
        b1[1] = GameObject.Find("Canvas/ResearchStation/InnerEl/Row3/1a");
        b1[2] = GameObject.Find("Canvas/ResearchStation/InnerEl/Row4/1a");
        b2[0] = GameObject.Find("Canvas/ResearchStation/InnerEl/Row2/1b");
        b2[1] = GameObject.Find("Canvas/ResearchStation/InnerEl/Row3/1b");
        b2[2] = GameObject.Find("Canvas/ResearchStation/InnerEl/Row4/1b");

        researched = new bool[LAST_PAGE + 1, 2, 3];//page,branch,upgrades
        for (int i = 0; i < LAST_PAGE + 1; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                researched[i, 0, j] = false;
                researched[i, 1, j] = false;
            }
        }

        for(int i = 0; i < 3; i++)
        {
            b1[i].gameObject.transform.position = b1[i].gameObject.transform.position
                + new Vector3(-1000, 0, 0);
            b2[i].gameObject.transform.position = b2[i].gameObject.transform.position
                + new Vector3(-200, 0, 0);
        }
    }

    void Start()
    {
        price1Text = GameObject.Find("Canvas/ResearchStation/InnerEl/Row2/Cost").GetComponent<Text>();
        price2Text = GameObject.Find("Canvas/ResearchStation/InnerEl/Row3/Cost").GetComponent<Text>();
        price3Text = GameObject.Find("Canvas/ResearchStation/InnerEl/Row4/Cost").GetComponent<Text>();
        UpdatePage(0);
    }

    /// <summary>
    /// Updates page and upgrades when a page is selected.
    /// </summary>
    /// <param name="page"></param>
    public void UpdatePage(int page)
    {
        this.page = page;
        Text pageName = this.pageName.GetComponent<Text>();

        switch (page)
        {
            case 0:
                pageName.text = "Standard\nUpgrades";
                break;
            case 1:
                pageName.text = "Missile\nUpgrades";
                break;
            case 2:
            default:
                pageName.text = "Radiator\nUpgrades";
                break;
        }

        for (int i = 0; i < 3; i++)
        {
            if (researched[page, 0, i])
                b1Img[i].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            else
                b1Img[i].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.25f);

            if (researched[page, 1, i])
                b2Img[i].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            else
                b2Img[i].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.25f);
        }

        int[,,] researchCost = ResearchCostManager.getResearchCosts();
        price1Text.text = "Kills: " + researchCost[page, 0, 0].ToString("0");
        price2Text.text = "Kills: " + researchCost[page, 0, 1].ToString("0");
        price3Text.text = "Kills: " + researchCost[page, 0, 2].ToString("0");
    }

    public void NextPage()
    {
        int p = page + 1;
        if (p > LAST_PAGE)
            p = 0;
        UpdatePage(p);
    }

    public void PrevPage()
    {
        int p = page - 1;
        if (p < 0)
            p = LAST_PAGE;
        UpdatePage(p);
    }

    /// <summary>
    /// Onclick for unlocking an upgrade.
    /// </summary>
    /// <param name="upgradeNo"></param>
    public void UpgradeClicked(int upgradeNo)
    {
        int i;
        bool primaryBranch = upgradeNo < 3;
        switch (upgradeNo)
        {
            case 0:
            case 3:
                i = 0;
                break;
            case 1:
            case 4:
                i = 1;
                break;
            case 2:
            case 5:
            default:
                i = 2;
                break;
        }

        if (primaryBranch)
        {
            if (i > 0)
            {
                for (int n = i - 1; n > -1; n--)
                    if (!researched[page, 0, n])
                    {
                        Debug.Log("You must upgrade from the start of the tree before upgrading further.");
                        return;
                    }

                rcm.checkIfCanResearch(page, 0, i);
            }
            else
            {
                rcm.checkIfCanResearch(page, 0, 0);
            }
        }
        else
        {
            if (i > 0)
            {
                for (int n = i - 1; n > -1; n--)
                    if (!researched[page, 1, n])
                    {
                        Debug.Log("You must upgrade from the start of the tree before upgrading further.");
                        return;
                    }

                rcm.checkIfCanResearch(page, 1, i);
            }
            else
            {
                rcm.checkIfCanResearch(page, 1, i);
            }
        }

        UpdatePage(page);
    }

    /// <summary>
    /// Activates the option descriptor to describe the upgrade type.
    /// </summary>
    /// <param name="ability"></param>
    public void AbilityHovered(int ability)
    {
        if (ability <= 2)
            optionDescriptor.SetPosition(b1Img[ability].transform.position.x, b1Img[ability].transform.position.y - 100);
        else
            optionDescriptor.SetPosition(b2Img[ability - 3].transform.position.x, b2Img[ability - 3].transform.position.y - 100);
        string power = "NULL";
        switch (page)
        {
            case 0:
                if (ability == 0)
                    power = "sniper";
                else if (ability == 1)
                    power = "+damage";
                else if (ability == 2)
                    power = "stun";
                else if (ability == 3)
                    power = "laser";
                else if (ability == 4)
                    power = "+fire rate";
                else
                    power = "freeze chance";
                break;
            case 1:
                if (ability == 0)
                    power = "nuke";
                else if (ability == 1)
                    power = "+fire rate";
                else if (ability == 2)
                    power = "radiation";
                else if (ability == 3)
                    power = "fire";
                else if (ability == 4)
                    power = "+damage";
                else
                    power = "fire spread";
                break;
            case 2:
            default:
                if (ability == 0)
                    power = "fire rate";
                else if (ability == 1)
                    power = "2x kill counts";
                else if (ability == 2)
                    power = "instant kill%";
                else if (ability == 3)
                    power = "range";
                else if (ability == 4)
                    power = "loot collecting";
                else
                    power = "+damage";
                break;
        }

        optionDescriptor.SetText("Purchase the\n" + power + "\nupgrade");
    }

    /// <summary>
    /// Disables the option descriptor.
    /// </summary>
    public void AbilityExited()
    {
        optionDescriptor.Disable();
    }
}
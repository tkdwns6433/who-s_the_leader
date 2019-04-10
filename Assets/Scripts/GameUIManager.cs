﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameUIManager : MonoBehaviour
{
    private static GameUIManager instance;
    public static GameUIManager Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

    }

    Unit selectedUnit = null;
    public GameObject unitPanel;
    public Text unitNameText;
    public Text goldText;
    public void SelectUnit(Unit selectUnit)
    {

        if (selectedUnit == selectUnit) //같은 유닛을 두번 누르면 panel 없어지게하기
        {
            unitPanel.SetActive(false);         
            selectedUnit = null;
            
        }
        else
        {
            unitPanel.SetActive(true);
            //UnitData 출력
            unitNameText.text = selectUnit.unitID.ToString();
            selectedUnit = selectUnit;
        }

    }
    public void ChangeGoldText(int gold)
    {
        goldText.text = gold.ToString();
    }

}

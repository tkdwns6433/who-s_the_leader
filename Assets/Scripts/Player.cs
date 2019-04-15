using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Player : MonoBehaviour
{
    public int totalActPower;
    public int currentActPower;
    public List<Unit> unitList;
    public int gold;

    public int Gold
    {
        get { return gold; }
        set
        {
            gold = value;
            GameUIManager.Instance.ChangeGoldText(gold);
        }
    }


    public void resetActPower()
    {
        currentActPower = totalActPower;
    }
    public void useActPower(int power)
    {
        currentActPower -= power;
    }
    
    public void addTotalActPower(int power)
    {
        totalActPower += power;
    }

    public bool checkActPower(int power)
    {
        return currentActPower - power >= 0;
    }
}

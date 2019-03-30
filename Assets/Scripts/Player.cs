using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int totalActPower;
    public int currentActPower;
    public List<Unit> unitList;
    public int gold;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public int unitID;
    public int x;
    public int y;
    public int curHP;
    private UnitData m_unitData;
    public UnitData unitData
    {
        get { return m_unitData; }
        set { m_unitData = value; }
    }
    
    UnitType unitType;

    public bool isInPos(int _x, int _y)
    {
        return _x == x && _y == y;
    }
    
    public void getDamage(int damage)
    {
        curHP -= damage;
        //맞는 애니메이션 사용 (꼭 비동기적인 코루틴 사용해야함)
        if(curHP <= 0)
        {
            GameManager.GetInstance().deleteUnit(unitID);
            //죽는 애니메이션 사용 (꼭 비동기적인 코루틴 사용해야함)
            Destroy(this);
        }
        
    }
}

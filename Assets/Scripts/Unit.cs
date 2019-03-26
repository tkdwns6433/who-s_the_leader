using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    UnitType unitType;
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
    //unity 폴더 Resources/Units folder에 UnitType과 똑같은 이름으로 png 또는 jpg file로 존재해야 sprite불러올 수 있음
    public void intiateUnit(UnitType ut)
    {
        unitType = ut;
        m_unitData = GameData.getUnitData(ut);
        GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Units/" + m_unitData.unitType.ToString());
        this.name = m_unitData.unitType.ToString();
    }
    


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

    public void unitMove(int _x, int _y)
    {
        //성현씨 유닛 움직이는 함수 추가 바랍니다. 코루틴으로 하면 좋아요
        //코드 만들곳
        //아래는 네트워크 관련 함수입니다.
        if(GameManager.GetInstance().myTurn)
        {
            var m_network = GameObject.FindWithTag("Network").GetComponent<Network>();
            UnitMoveData data = new UnitMoveData();
            data.unitId = this.unitID;
            data.x = _x;
            data.y = _y;
            UnitMovePacket movePacket = new UnitMovePacket(data);
            m_network.SendReliable(movePacket);
        }
    }
}

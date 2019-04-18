using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{
    UnitType unitType;
    bool bCheckcol;
    bool bMovedirection;  //true = right, false = left;
    bool bMove;
    public int unitID;
    public float x;
    public float y;
    public int curHP;
    private UnitData m_unitData;
    public PLAYER control_player;
    
    public UnitData unitData
    {
        get { return m_unitData; }
        set { m_unitData = value; }
    }

    private void Start()
    {

    }

    //unity 폴더 Resources/Units folder에 UnitType과 똑같은 이름으로 png 또는 jpg file로 존재해야 sprite불러올 수 있음
    public void initiateUnit(UnitType ut, float _x, float _y, PLAYER player)
    {
        control_player = player;
        unitType = ut;
        m_unitData = GameData.getUnitData(ut);
        GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Units/" + m_unitData.unitType.ToString());
        this.name = m_unitData.unitType.ToString();
        unitID = GameManager.GetInstance.giveID();
        curHP = unitData.hp;
        switch (player)
        {
            case PLAYER.PLAYER1:
                tag = "Player1Unit";
                break;
            case PLAYER.PLAYER2:
                tag = "Player2Unit";
                break;
            case PLAYER.NONE:
                break;
            default:
                break;
        }
        setPos(_x, _y);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision.transform.tag == "Player1Unit" || collision.transform.tag == "Player2Unit")
        //{
        //    Debug.Log("!!");
        //    if (bMove == false) // 첫생성시 시작위치 지정하기 위한것
        //    {
        //        if (bCheckcol == false)
        //        {
        //            setPos(transform.position.x + 120, transform.position.y);
        //            bCheckcol = true;
        //        }
        //        else if (bCheckcol == true)
        //        {
        //            setPos(transform.position.x - 120, transform.position.y);
        //        }
        //    }
        //    else
        //    {
        //        if (bMovedirection == true)
        //        {
        //            setPos(transform.position.x - 120, transform.position.y);
        //        }
        //        else
        //        {
        //            setPos(transform.position.x + 120, transform.position.y);
        //        }
        //    }
        //}
        Debug.Log("!");
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
            GameManager.GetInstance.deleteUnit(unitID);
            //죽는 애니메이션 사용 (꼭 비동기적인 코루틴 사용해야함)
            Destroy(this);
        }   
    }

    public void setPos(float _x, float _y)
    {
        //게임 상 위치 지정해주는 클라이언트 코드
        x = _x;
        y = _y;
        this.transform.position = new Vector2(_x, _y);
    }

    public void ClientUnitMove(float _x, float _y)
    {
        unitMove(_x, _y);
        //클라이언트 코드
    }

    public void unitMove(float _x, float _y)
    {
        x = _x;
        y = _y;
        if(GameManager.GetInstance.myTurn)
        {
            var m_network = GameObject.FindWithTag("Network").GetComponent<Network>();
            UnitMoveData data = new UnitMoveData();
            data.unitId = this.unitID;
            //data.x = _x;
            //data.y = _y;  //네트워크 플롯
            UnitMovePacket movePacket = new UnitMovePacket(data);
            m_network.SendReliable(movePacket);
        }
    }

    public void attackUnit(int defender)
    {
    }

    public void OnMouseDown()
    {       
        GameUIManager.Instance.SelectUnit(this);
        
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }
}

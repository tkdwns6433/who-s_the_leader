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
    public int movespeed;
    public int curHP;
    private UnitData m_unitData;
    public PLAYER control_player;
    IEnumerator Checkienun;
    bool bTiledcheck;
    bool check;                  //마우스 클릭 체크 확인
    bool movecheck;
    bool firstClick;            //처음 클릭 확인
    static bool currentUnit;   //현재 유닛이 무엇인지 체크
    public bool clickCheck;    //클릭하엿는지 체크(타일 연동을 위한 변수)
    public int blockRange;     //이동범위
    public int attackRange;    //공격범위
    public bool attackCheck;   //공격실행 체크

    UnitAttack m_unitAttack;

    public UnitData unitData
    {
        get { return m_unitData; }
        set { m_unitData = value; }
    }

    private void Start()
    {
        blockRange = 3;
        attackRange = 3;//임시
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
        m_unitAttack = new UnitAttack(unitID);
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
        this.transform.position = Vector2.MoveTowards(transform.position, new Vector2(_x, transform.position.y), movespeed *Time.deltaTime);

        if ((this.transform.position.x - tempPos.x)%120 == 0)
            movecheck = false;

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
        clickCheck = !clickCheck;
        if (currentUnit == false)
        {
            GameUIManager.Instance.SelectUnit(this);
            Checkienun = CheckTiled();
            check = !check;
            StartCoroutine(Checkienun);
            currentUnit = true;
            firstClick = false;
        }
        

    }

    RaycastHit2D rayhit;
    Vector2 tempPos;

    IEnumerator CheckTiled()
    {
       
        while (check)
        {

            if (!movecheck)
            {//Debug.Log("!");
                if (Input.GetMouseButtonDown(0))
                {
                    
                    Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Ray2D ray = new Ray2D(pos, Vector2.zero);
                    rayhit = Physics2D.Raycast(ray.origin, ray.direction);

                    if (rayhit.collider != null)
                    {
                        if (rayhit.collider.gameObject.tag == "Tiled")
                        {
                            if (rayhit.collider.GetComponent<SpriteRenderer>().color == Color.blue)
                            {
                                tempPos = transform.position;
                                movecheck = true;

                                //setPos(rayhit.collider.transform.position.x, y);
                                //네트워크 지정필요
                            }
                        }
                        else
                        {
                            if (firstClick)  //임시작업중 다른곳 클릭스 선택해제
                            {
                                Debug.Log("다른곳 클릭");
                                GameUIManager.Instance.SelectUnit(this);
                                currentUnit = false;
                                clickCheck = !clickCheck;
                            }
                            Debug.Log("다른곳 클릭");
                            firstClick = true;
                        }
                    }

                }
            }

            if(movecheck)
            {
                Debug.Log("!!");
                ClientUnitMove(rayhit.collider.transform.position.x, y);
            }
            yield return null;
        }

        
        Debug.Log("오류");
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

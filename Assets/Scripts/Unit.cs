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
    public int maxdamage;
    public int mindamage;
    private UnitData m_unitData;
    public PLAYER control_player;
    IEnumerator Checkienun;
    bool bTiledcheck;
    bool check;                  //마우스 클릭 체크 확인
    bool movecheck;
    bool firstClick;            //처음 클릭 확인
    public bool clickCheck;    //클릭하엿는지 체크(타일 연동을 위한 변수)
    public int blockRange;     //이동범위
    public int attackRange;    //공격범위
    public bool attackCheck;   //공격실행 체크
    public bool enemyattackCheck; //적공격 실행 체크

    public UnitAttack m_unitAttack;

    public UnitData unitData
    {
        get { return m_unitData; }
        set { m_unitData = value; }
    }

    private void Start()
    {
        blockRange = 4;
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
        if (control_player == PLAYER.PLAYER1)
        {
            GameManager.GetInstance.player1.unitList.Add(this);
        }
        else if (control_player == PLAYER.PLAYER2)
        {
            GameManager.GetInstance.player2.unitList.Add(this);
        }
        curHP = unitData.hp;
        m_unitAttack.InitiateUnitAttack(unitID);
        mindamage = unitData.min_atk;
        maxdamage = unitData.max_atk;
        switch (player)
        {
            case PLAYER.PLAYER1:
                break;
            case PLAYER.PLAYER2:
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


        //float dis = Vector2.Distance(transform.position, collision.transform.position); //-->물체사이 거리를 통해 콜라이더를 바꾸려했으나 실패
        //if (transform.tag == "Player1Unit")
        //{
        //    if (collision.transform.tag == "Player2" || collision.transform.tag == "Player2Unit")
        //    {
        //        attackCheck = !attackCheck;
        //    }
        //}
        //else if (transform.tag == "Player2Unit")
        //{
        //    if (collision.transform.tag == "Player1" || collision.transform.tag == "Player1Unit")
        //    {
        //        attackCheck = !attackCheck;
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

        Debug.Log(unitID + "히트 -hp : " + curHP);
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

        if ((this.transform.position.x - tempPos.x) % 120 == 0)
        {
            movecheck = false;
            clickCheck = true;
        }

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

    private void OnMouseDown()
    {
        
    }

    public void OnMouseUp()
    {
        if (GameManager.GetInstance.currentUnit == false && enemyattackCheck == false)
        {
            //Debug.Log("!");
            gameObject.layer = 8;
            clickCheck = !clickCheck;
            GameUIManager.Instance.SelectUnit(this);
            Checkienun = CheckTiled();
            check = !check;
            StartCoroutine(Checkienun);
            GameManager.GetInstance.currentUnit = true;
            firstClick = false;
        }
        

    }

    RaycastHit2D rayhit;
    Vector2 tempPos;
    Collider2D rightEnemy;
    Collider2D leftEnemy;

    IEnumerator CheckTiled()
    {

        while (check)
        {
            if (!movecheck)
            {
               
                firstClick = true;
                if (Input.GetMouseButtonDown(0))
                {

                    Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Ray2D ray = new Ray2D(pos, Vector2.zero);
                    rayhit = Physics2D.Raycast(ray.origin, ray.direction);

                    if (rayhit.collider != null)
                    {
                        if (attackCheck == false)
                        {
                            if (rayhit.collider.gameObject.tag == "Tiled")
                            {
                                if (rayhit.collider.GetComponent<Tiledcontrol1>().tiledUnitCheck == false) //자신의 유닛 위치로 이동못하게 막아주는 변수
                                {
                                    if (rayhit.collider.GetComponent<SpriteRenderer>().color == Color.blue)
                                    {
                                        tempPos = transform.position;
                                        movecheck = true;
                                        //setPos(rayhit.collider.transform.position.x, y);
                                        //네트워크 지정필요
                                    }
                                }
                            }
                            else if (rayhit.collider.gameObject.layer == 0) //임시작업중 다른곳 클릭스 선택해제
                            {
                                Debug.Log("다른곳 클릭1");
                                GameUIManager.Instance.SelectUnit(this);
                                clickCheck = false;
                                attackCheck = false;
                                check = false;
                                gameObject.layer = 0;
                                GameManager.GetInstance.currentUnit = false;
                                rightEnemy = null;
                                leftEnemy = null;
                            }
                        }
                        else if (attackCheck)
                        {
                        //    Vector2 aPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        //    Ray2D aRay = new Ray2D(aPos, Vector2.zero);
                        //    rayhit = Physics2D.Raycast(aRay.origin, aRay.direction);
                            if (transform.GetComponent<Unit>().control_player == PLAYER.PLAYER1 && rayhit.collider.GetComponent<Unit>().control_player == PLAYER.PLAYER2
                                || transform.GetComponent<Unit>().control_player == PLAYER.PLAYER2 && rayhit.collider.GetComponent<Unit>().control_player == PLAYER.PLAYER1)
                            {
                                if (rayhit.collider.GetComponent<Unit>().enemyattackCheck)
                                {
                                    Debug.Log(rayhit.collider.GetComponent<Unit>().unitID);
                                    m_unitAttack.DoAttack(rayhit.collider.GetComponent<Unit>().unitID);
                                }
                            }
                        }
                    }
                    else
                    {

                        if (firstClick)  //임시작업중 다른곳 클릭스 선택해제
                        {
                            Debug.Log("다른곳 클릭");
                            GameUIManager.Instance.SelectUnit(this);
                            GameManager.GetInstance.currentUnit = false;
                            clickCheck = false;
                            attackCheck = false;
                            check = false;
                            rightEnemy = null;
                            leftEnemy = null;
                            gameObject.layer = 0;
                        }

                        

                    }

                   
                }
                else if (Input.GetKeyDown(KeyCode.A))
                {
                    attackCheck = !attackCheck;
                }
            }
            else if (movecheck)
            {
                clickCheck = false;
                ClientUnitMove(rayhit.collider.transform.position.x, y);
            }

            
            yield return null;
        }
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

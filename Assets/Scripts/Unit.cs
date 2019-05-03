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
    //static bool currentUnit;   //현재 유닛이 무엇인지 체크
    public bool clickCheck;    //클릭하엿는지 체크(타일 연동을 위한 변수)
    public int blockRange;     //이동범위
    public int attackRange;    //공격범위
    public bool attackCheck;   //공격실행 체크
    List<Collider2D> tempcol; //닿은 물체 임시저장

    public UnitData unitData
    {
        get { return m_unitData; }
        set { m_unitData = value; }
    }

    private void Start()
    {
        tempcol = new List<Collider2D>();
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
        

        float dis = Vector2.Distance(transform.position, collision.transform.position); //-->물체사이 거리를 통해 콜라이더를 바꾸려했으나 실패
        if (transform.tag == "Player1Unit")
        {
            if (collision.transform.tag == "Player2" || collision.transform.tag == "Player2Unit")
            {
                tempcol.Add(collision);

            }
        }
        else if (transform.tag == "Player2Unit")
        {
            if (collision.transform.tag == "Player1" || collision.transform.tag == "Player1Unit")
            {
                tempcol.Add(collision);
                
            }
        }
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

    public void OnMouseUp()
    {
        Debug.Log("!");
        
        if (GameManager.GetInstance.currentUnit == false)
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
                bool rightenemycheck = false;
                bool leftenemycheck = false;
                firstClick = true;
                

                if (Input.GetMouseButtonDown(0))
                {
                    
                    Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Ray2D ray = new Ray2D(pos, Vector2.zero);
                    rayhit = Physics2D.Raycast(ray.origin, ray.direction);
                    if (tempcol.Count != 0)
                    {
                        for (int i = 0; i < tempcol.Count; i++)
                        {
                            if (i >= 1)
                            {
                                if (tempcol[i].transform.position == tempcol[i - 1].transform.position)
                                {
                                    tempcol.RemoveAt(i);
                                }
                            }
                        }

                        for (int j = 0; j < tempcol.Count; j++) //좌우 적 체크후 이동
                        {
                            if (transform.position.x < tempcol[j].transform.position.x)// && tempcol[j].transform.position.x > rayhit.collider.transform.position.x)
                            {
                                rightenemycheck = true;
                                rightEnemy = tempcol[j];  // 한쪽 방향에만 적이 있을때
                            }
                            else if (transform.position.x > tempcol[j].transform.position.x)// && tempcol[j].transform.position.x < rayhit.collider.transform.position.x)
                            {
                                leftenemycheck = true;
                                leftEnemy = tempcol[j];
                            }
                        }
                    }

                    if (rayhit.collider != null)
                    {

                        if (rayhit.collider.gameObject.tag == "Tiled")
                        {

                            if (tempcol.Count == 0)
                            {
                                if (rayhit.collider.GetComponent<SpriteRenderer>().color == Color.blue)
                                {
                                    tempPos = transform.position;
                                    movecheck = true;
                                    //setPos(rayhit.collider.transform.position.x, y);
                                    //네트워크 지정필요
                                }
                            }
                            else if (rightenemycheck && leftenemycheck)
                            {
                                if (transform.position.x > leftEnemy.transform.position.x && leftEnemy.transform.position.x < rayhit.collider.transform.position.x
                                    && transform.position.x < rightEnemy.transform.position.x && rightEnemy.transform.position.x > rayhit.collider.transform.position.x)
                                {
                                    if (rayhit.collider.GetComponent<SpriteRenderer>().color == Color.blue)
                                    {
                                        Debug.Log("!!!");
                                        tempPos = transform.position;
                                        movecheck = true;
                                        //setPos(rayhit.collider.transform.position.x, y);
                                        //네트워크 지정필요
                                    }
                                }
                            }
                            else if (leftenemycheck && !rightenemycheck)
                            {
                                if (transform.position.x > leftEnemy.transform.position.x && leftEnemy.transform.position.x < rayhit.collider.transform.position.x)
                                {
                                    if (rayhit.collider.GetComponent<SpriteRenderer>().color == Color.blue)
                                    {
                                        tempPos = transform.position;
                                        movecheck = true;
                                        leftenemycheck = false;
                                        //setPos(rayhit.collider.transform.position.x, y);
                                        //네트워크 지정필요
                                    }
                                }
                            }
                            else if (rightenemycheck && !leftenemycheck)
                            {
                                if (transform.position.x < rightEnemy.transform.position.x && rightEnemy.transform.position.x > rayhit.collider.transform.position.x)
                                {
                                    Debug.Log("check");
                                    if (rayhit.collider.GetComponent<SpriteRenderer>().color == Color.blue)
                                    {
                                        tempPos = transform.position;
                                        movecheck = true;
                                        rightenemycheck = false;
                                        //setPos(rayhit.collider.transform.position.x, y);
                                        //네트워크 지정필요
                                    }
                                }
                            }

                        }
                        else if (rayhit.collider.gameObject.layer == 0) //임시작업중 다른곳 클릭스 선택해제
                        {
                            Debug.Log("다른곳 클릭1");
                            GameUIManager.Instance.SelectUnit(this);
                            clickCheck = !clickCheck;
                            check = false;
                            gameObject.layer = 0;
                            GameManager.GetInstance.currentUnit = false;
                            rightenemycheck = false;
                            leftenemycheck = false;
                            rightEnemy = null;
                            leftEnemy = null;
                            tempcol.Clear();
                        }
                    }
                    else
                    {

                        if (firstClick)  //임시작업중 다른곳 클릭스 선택해제
                        {
                            Debug.Log("다른곳 클릭");
                            GameUIManager.Instance.SelectUnit(this);
                            GameManager.GetInstance.currentUnit = false;
                            clickCheck = !clickCheck;
                            check = false;
                            rightenemycheck = false;
                            leftenemycheck = false;
                            rightEnemy = null;
                            leftEnemy = null;
                            gameObject.layer = 0;
                            tempcol.Clear();
                        }

                    }


                }
            }

            if (movecheck)
            {
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

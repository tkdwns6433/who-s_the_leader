using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiledcontrol1 : MonoBehaviour
{

    public bool tclickCheck;
    public int tblockRange;
    public int tattackRange;
    public int building_ID;
    public float posX, posY;
    public bool tEnemycheck;  //적한테 보낼 불자료형
    public bool bCurrentTiledAttack;
    public bool tiledUnitCheck; //타일 위의 유닛체크

    Building buildingState;


    bool charOnoff, charOnoff2;
    Collider2D TempCol;
    Vector3 coltemp;
    Color32 Tempcolor;
    Color32 color1;

    private void Start()
    {
        color1 = GetComponent<SpriteRenderer>().color;
        Tempcolor = this.GetComponent<SpriteRenderer>().color;
        posX = this.transform.position.x;
        posY = this.transform.position.y;

    }

    IEnumerator CheckMovePlayer()                                           //플레이어면 행동범위 실행
    {
        color1 = Color.blue;
        this.GetComponent<SpriteRenderer>().color = color1;


        yield return null;

    }

    IEnumerator CheckAttackPlayer()                                           //플레이어면 공격범위 실행
    {
        color1 = Color.red;
        this.GetComponent<SpriteRenderer>().color = color1;
        yield return null;

    }

    IEnumerator CheckExitPlayer2()                                      //
    {
        this.GetComponent<SpriteRenderer>().color = Tempcolor;   
        yield return null;
    }

    IEnumerator CheckExitPlayer()
    {
        this.GetComponent<SpriteRenderer>().color = Tempcolor;
        yield return null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        
        if (collision.transform.name == "unitrange")
        {



            if (collision.transform.parent.GetComponentInParent<Unit>().control_player == PLAYER.PLAYER1)
            {
                charOnoff = true; //플레이어
                if (GetComponent<SpriteRenderer>().color == Color.red)
                {
                    collision.transform.parent.GetComponentInParent<Unit>().enemyattackCheck = true;

                }
                else
                {
                    collision.transform.parent.GetComponentInParent<Unit>().enemyattackCheck = false;
                }
            }
            else if (collision.transform.parent.GetComponentInParent<Unit>().control_player == PLAYER.PLAYER2)                     //적유닛 or 적플레이어 확인
            {
                charOnoff2 = true; //적
                if (GetComponent<SpriteRenderer>().color == Color.red)
                {
                    collision.transform.parent.GetComponentInParent<Unit>().enemyattackCheck = true;
                }
                else
                {
                    collision.transform.parent.GetComponentInParent<Unit>().enemyattackCheck = false;
                }
            }

            if (collision.transform.parent.GetComponentInParent<Unit>().control_player == PLAYER.PLAYER1 && collision.transform.parent.GetComponentInParent<Unit>().control_player != PLAYER.PLAYER2) //플레이어 기준 타일설정
            {
                bCurrentTiledAttack = collision.transform.parent.GetComponentInParent<Unit>().attackCheck;
                if (charOnoff2 == false)                                                                         //적플레이어 확인후 타일 색상변경
                {
                    tclickCheck = collision.transform.parent.GetComponentInParent<Unit>().clickCheck;
                    if (tclickCheck)                                                                            //클릭 확인 했으면 범위 표시
                    {
                        if (collision.transform.parent.GetComponentInParent<Unit>().attackCheck)
                        {
                            tattackRange = collision.transform.parent.GetComponentInParent<Unit>().attackRange;
                            collision.transform.GetComponent<BoxCollider2D>().size = new Vector2(120 * tattackRange, 139);
                            collision.transform.parent.GetComponentInParent<Unit>().enemyattackCheck = true;
                            StartCoroutine(CheckAttackPlayer());
                        }
                        else if (!collision.transform.parent.GetComponentInParent<Unit>().attackCheck)
                        {
                            tblockRange = collision.transform.parent.GetComponentInParent<Unit>().blockRange;
                            collision.transform.GetComponent<BoxCollider2D>().size = new Vector2(120 * tblockRange, 139);     //tblockRange만큼 타일색 변경
                            collision.transform.parent.GetComponentInParent<Unit>().enemyattackCheck = false;
                            StartCoroutine(CheckMovePlayer());


                        }

                    }
                    if (!tclickCheck)
                    {
                        collision.transform.GetComponent<BoxCollider2D>().size = new Vector2(80, 139);
                        this.GetComponent<SpriteRenderer>().color = Tempcolor;                                 //클릭이 꺼지면 콜리더 사이즈 원래대로
                                                                                                               //버그 생길시 charOnoff2 = false 추가하면 해결가능성있음
                        tiledUnitCheck = true;
                    }
                }
                else if (charOnoff2 == true)
                {
                    if (bCurrentTiledAttack == false)
                    {
                        this.GetComponent<SpriteRenderer>().color = Tempcolor;
                    }
                    else
                    {
                        this.GetComponent<SpriteRenderer>().color = Color.red;

                    }

                    if (collision.transform.parent.transform.position.x == transform.position.x)
                    {
                        this.GetComponent<SpriteRenderer>().color = Tempcolor;
                    }
                }
                
            }
            else if (collision.transform.parent.GetComponentInParent<Unit>().control_player == PLAYER.PLAYER2 && collision.transform.parent.GetComponentInParent<Unit>().control_player != PLAYER.PLAYER1) //적유닛기준 타일설정
            {
                bCurrentTiledAttack = collision.transform.parent.GetComponentInParent<Unit>().attackCheck;
                if (charOnoff == false)                                                                         //적플레이어 확인후 타일 색상변경
                {
                    tclickCheck = collision.transform.parent.GetComponentInParent<Unit>().clickCheck;
                    if (tclickCheck)                                                                            //클릭 확인 했으면 범위 표시
                    {
                        if (collision.transform.parent.GetComponentInParent<Unit>().attackCheck)
                        {
                            tattackRange = collision.transform.parent.GetComponentInParent<Unit>().attackRange;
                            collision.transform.GetComponent<BoxCollider2D>().size = new Vector2(120 * tattackRange, 139);
                            collision.transform.parent.GetComponentInParent<Unit>().enemyattackCheck = true;
                            StartCoroutine(CheckAttackPlayer());

                        }
                        else if (!collision.transform.parent.GetComponentInParent<Unit>().attackCheck)
                        {
                            tblockRange = collision.transform.parent.GetComponentInParent<Unit>().blockRange;
                            collision.transform.GetComponent<BoxCollider2D>().size = new Vector2(120 * tblockRange, 139);     //tblockRange만큼 타일색 변경
                            collision.transform.parent.GetComponentInParent<Unit>().enemyattackCheck = false;
                            StartCoroutine(CheckMovePlayer());

                        }

                    }
                    if (!tclickCheck)
                    {
                        collision.transform.GetComponent<BoxCollider2D>().size = new Vector2(80, 139);
                        this.GetComponent<SpriteRenderer>().color = Tempcolor;                                 //클릭이 꺼지면 콜리더 사이즈 원래대로
                                                                                                               //버그 생길시 charOnoff = false 추가하면 해결가능성있음
                        tiledUnitCheck = true;
                    }
                }
                else if (charOnoff == true)
                {
                    if (bCurrentTiledAttack == false)
                    {
                        this.GetComponent<SpriteRenderer>().color = Tempcolor;
                    }
                    else
                    {
                        this.GetComponent<SpriteRenderer>().color = Color.red;
                    }

                    if (collision.transform.parent.transform.position.x == transform.position.x)
                    {
                        this.GetComponent<SpriteRenderer>().color = Tempcolor;
                    }
                }
            }

        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.name == "unitrange")
        {
            if (collision.transform.parent.GetComponentInParent<Unit>().control_player == PLAYER.PLAYER1)
            {
                StartCoroutine(CheckExitPlayer());
                charOnoff = false;
                tiledUnitCheck = false;

            }
            else if (collision.transform.parent.GetComponentInParent<Unit>().control_player == PLAYER.PLAYER2)
            {
                StartCoroutine(CheckExitPlayer2());
                charOnoff2 = false;
                tiledUnitCheck = false;

            }
        }
    }
}


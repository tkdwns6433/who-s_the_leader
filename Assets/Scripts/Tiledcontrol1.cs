using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiledcontrol1 : MonoBehaviour
{

    public bool tclickCheck;
    public int tblockRange;
    public int building_ID;
    public float posX, posY;
    Building buildingState;


    bool charOnoff, charOnoff2;
    Collider2D TempCol;
    Vector3 coltemp;
    Color32 Tempcolor;
    Color32 color1;

    private void Start()
    {
        color1 = GetComponent<SpriteRenderer>().color;
        posX = this.transform.position.x;
        posY = this.transform.position.x;

    }

    IEnumerator CheckPlayer()                                           //플레이어면 실행
    {
        color1 = Color.red;
        this.GetComponent<SpriteRenderer>().color = color1;


        yield return null;

    }

    IEnumerator CheckPlayer2()                                      //
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
        if (collision.gameObject.tag == "Player")
        {
            Tempcolor = this.GetComponent<SpriteRenderer>().color;
            charOnoff = true;

        }
        else if (collision.gameObject.tag == "Player1")                     //적유닛 or 적플레이어 확인
        {
            Tempcolor = this.GetComponent<SpriteRenderer>().color;
            charOnoff2 = true;

        }
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && collision.gameObject.tag != "Player1")
        {
            if (charOnoff2 != true)                                                                         //적플레이어 확인후 타일 색상변경
            {
                tclickCheck = collision.GetComponent<Tempmove>().clickCheck;
                if (tclickCheck)                                                                            //클릭 확인 했으면 범위 표시
                {
                    tblockRange = collision.GetComponent<Tempmove>().blockRange;
                    collision.GetComponent<BoxCollider2D>().size = new Vector2(119 * tblockRange, 139);     //tblockRange만큼 타일색 변경
                    StartCoroutine(CheckPlayer());
                }
                if (!tclickCheck)
                {
                    collision.GetComponent<BoxCollider2D>().size = new Vector2(80, 139);                     //클릭이 꺼지면 콜리더 사이즈 원래대로
                }
            }
        }

        if (collision.gameObject.tag == "Player1")
        {
            StartCoroutine(CheckPlayer2());
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            StartCoroutine(CheckExitPlayer());
            charOnoff = false;

        }
        else if (collision.gameObject.tag == "Player1")
        {
            charOnoff2 = false ;

        }
    }
}


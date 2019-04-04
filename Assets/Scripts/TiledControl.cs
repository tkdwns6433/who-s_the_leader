using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiledControl : MonoBehaviour
{

    public bool tclickCheck;
    public int tblockRange;
    Vector3 coltemp;
    public Collider2D[] col;
    Color32 Tempcolor;
    Color32 color1;

    private void Start()
    {
        color1 = GetComponent<SpriteRenderer>().color;
    }

    IEnumerator CheckPlayer()
    {

         col = Physics2D.OverlapBoxAll(transform.position, coltemp, 0.0f);
        Tempcolor = this.GetComponent<SpriteRenderer>().color;
        color1 = Color.red;
        this.GetComponent<SpriteRenderer>().color = color1;
        for (int i = 0; i < col.Length; i++)
        {
            if (col[i].gameObject.tag == "Player")
            {
                col[i].GetComponent<SpriteRenderer>().color = color1;
            }
        }

        yield return null;

    }

    IEnumerator CheckExitPlayer()
    {
        for (int i = 0; i < col.Length; i++)
        {
            col[i].GetComponent<SpriteRenderer>().color = Tempcolor;
            Debug.Log(1);
        }
        yield return null;
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            tclickCheck = collision.GetComponent<Tempmove>().clickCheck;
            if (tclickCheck)
            {
                tblockRange = collision.GetComponent<Tempmove>().blockRange;
                coltemp = new Vector3(120 * tblockRange, 23, -1);
                StartCoroutine(CheckPlayer());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        StartCoroutine(CheckExitPlayer());
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Sight : PoolableObject

{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player1Unit")
        {
            //GetComponent<PoolableObject>().Push();

            SpriteRenderer spr = GetComponent<SpriteRenderer>();

            Color color = spr.color;
            color.a = 0f;
            spr.color = color;

        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player1Unit")
        {
            //gameObject.SetActive(true);
            Debug.Log("???");
            SpriteRenderer spr = GetComponent<SpriteRenderer>();

            Color color = spr.color;
            color.a += 87f;
            spr.color = color;
        }
        else
        {
            Debug.Log("error111");
        }
    }
}

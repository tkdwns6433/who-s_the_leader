using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitGenPoint : MonoBehaviour
{
    public bool uNitcheck;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag == "Player1Unit" || collision.transform.tag == "Player2Unit")
        {
            uNitcheck = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        uNitcheck = false;
    }
}

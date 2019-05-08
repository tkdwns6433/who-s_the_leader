using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitGenPoint : MonoBehaviour
{
    public bool uNitcheck;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.transform.tag == "unit")
        {
            uNitcheck = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        uNitcheck = false;
    }
}

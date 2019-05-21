using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Sight : PoolableObject

{
    public Material firstSight;
    public Material secondSight;
    public Material thirdSight;
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "SightRange")
        {
            this.GetComponent<SpriteRenderer>().material = thirdSight;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "SightRange")
        {
            this.GetComponent<SpriteRenderer>().material = secondSight;
            //this.GetComponent<SpriteRenderer>().sortingLayerName = "UsedSight";

        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agit : MonoBehaviour
{
    bool clickcheck;
    SpriteRenderer agitspt;

    private void Start()
    {
        agitspt = GetComponent<SpriteRenderer>();
    }

    private void OnMouseDown()
    {
        Debug.Log("!");
        clickcheck = !clickcheck;
        if (transform.name == "Agit green")
        {
            if (clickcheck)
            {
                agitspt.sprite = Resources.Load<Sprite>("image/Agit green_line") as Sprite;
            }
            else
            {
                agitspt.sprite = Resources.Load<Sprite>("image/Agit green_lineX") as Sprite;
            }
        }
        else if (transform.name == "Agit red")
        {
            if (clickcheck)
            {
                agitspt.sprite = Resources.Load<Sprite>("image/Agit red_line") as Sprite;
            }
            else
            {
                agitspt.sprite = Resources.Load<Sprite>("image/Agit red_lineX") as Sprite;
            }
        }

    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class User : MonoBehaviour
{

    public int actingPower;
    public GameObject acting;
    private int lastFrameClicked = -1;
    GameObject[] killEmAll;
    public void UserBtn()
    {
        //acting.SetActive(true);
        Vector3 AP = transform.position;
        Vector3 width = new Vector3(0.9f,0f,0f);
        killEmAll=GameObject.FindGameObjectsWithTag("act");
        if (lastFrameClicked == -1)
        {
            for (int i = 0; i < actingPower; i++)
            {
                Instantiate(acting).transform.Translate(AP + width);
                AP += width;
            }
            lastFrameClicked++;
        }
        else
        {
            for (int i = 0; i < killEmAll.Length; i++)
                Destroy(killEmAll[i]);
            lastFrameClicked--;
        }


    }
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

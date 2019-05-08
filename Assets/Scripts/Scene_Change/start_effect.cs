using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class start_effect : MonoBehaviour
{
    public GameObject[] btn;
    public Image fade;
    float fades = 1f;
    float time = 0;
    private void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
        if (fades > 0f)
        {
            fade.color = new Color(0, 0, 0, fades);
            fades -= 0.008f;
           
        }
        if(fades<0f)
        {
            btn[0].SetActive(true);
            btn[1].SetActive(true);
            btn[2].SetActive(true);
            btn[3].SetActive(true);
        }
    }
}

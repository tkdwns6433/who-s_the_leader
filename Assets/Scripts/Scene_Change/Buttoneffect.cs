using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Buttoneffect : MonoBehaviour
{
    public GameObject effect;
    // Start is called before the first frame update
    
    // Update is called once per frame
    private void OnMouseOver()
    {
        effect.SetActive(true);
    }
    private void OnMouseExit()
    {
        effect.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tempmove : MonoBehaviour
{
    Vector3 temp;
    public bool clickCheck;
    public int blockRange;


    private void OnMouseDown()
    {
        clickCheck = !clickCheck;
    }

    // Start is called before the first frame update
    void Start()
    {
        blockRange = 3;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            temp.Set(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0);
            transform.position += new Vector3(temp.x * 120, temp.y * 80, temp.z);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class elevator : MonoBehaviour
{
    private float time=0;
    private bool time_start = false;
    private bool player_posChange = false;
    public GameObject Player_Unit;

    private void Update()
    {
        if (time_start)
        {
            time += Time.deltaTime; // 1.5초 시간 후에 켜주기 위해서   
        }
    }
    public float distance;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision)
        {
            if (Player_Unit.GetComponent<Unit>().check == false)
            {
                #region 아래로 내려가는 엘리베이터
                if (collision.tag.Equals("down_elevator") /*|| collision.tag.Equals("mid_down_elevator")*/)
                {
                    #region 중간 엘리베이터 제외 스크립트 
                    Ray ray = new Ray(collision.transform.position + Vector3.down * 100f, Vector2.down); // 레이 발생
                    Debug.DrawRay(ray.origin, ray.direction * distance, Color.red);
                    RaycastHit2D hittile = Physics2D.Raycast(ray.origin, ray.direction, distance, 1 << 10);
                    if (hittile.collider.tag.Equals("up_elevator") /*|| hittile.collider.tag.Equals("mid_up_down_elevator")*/)
                    {// 카메라로 다른 엘리베이터를 클릭해도 부딪힌 콜라이더에서 쏜 ray가 맞는곳이 아니면 이동자체가 불가하게 설정.
                        if (Input.GetMouseButtonDown(0))
                        {
                            Vector2 aPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                            Ray2D aRay = new Ray2D(aPos, Vector2.zero);
                            RaycastHit2D camhit = Physics2D.Raycast(aRay.origin, aRay.direction);

                            if (camhit.collider == hittile.collider) // 마우스클릭한 것이 hittile 일 때
                            {
                                this.GetComponent<SpriteRenderer>().enabled = false;
                                time_start = true;
                                player_posChange = true; // 위치 이동
                            }
                        }
                        if (player_posChange) // 이동 ture
                        {
                            if (time > 1.5f) // 1.5초 이후에
                            {
                                this.transform.position = hittile.transform.position; // 위치 이동
                                this.GetComponent<SpriteRenderer>().enabled = true;
                                time_start = false;
                                time = 0;
                                player_posChange = false;
                            }
                        }
                    }
                    #endregion
                }
                #endregion
                #region 위로가는 엘리베이터
                if (collision.tag.Equals("up_elevator") /*|| collision.tag.Equals("mid_up_elevator")*/)
                {
                    Ray ray = new Ray(collision.transform.position + Vector3.up * 100f, Vector2.up); // 레이 발생
                    Debug.DrawRay(ray.origin, ray.direction * distance, Color.red);
                    RaycastHit2D hittile = Physics2D.Raycast(ray.origin, ray.direction, distance, 1 << 10);
                    if (hittile.collider.tag.Equals("down_elevator") /*|| hittile.collider.tag.Equals("mid_up_down_elevator")*/)
                    {// 카메라로 다른 엘리베이터를 클릭해도 부딪힌 콜라이더에서 쏜 ray가 맞는곳이 아니면 이동자체가 불가하게 설정.
                        if (Input.GetMouseButtonDown(0))
                        {
                            Vector2 aPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                            Ray2D aRay = new Ray2D(aPos, Vector2.zero);
                            RaycastHit2D camhit = Physics2D.Raycast(aRay.origin, aRay.direction);

                            if (camhit.collider == hittile.collider)// 마우스클릭한 것이 hittile 일 때
                            {
                                this.GetComponent<SpriteRenderer>().enabled = false;
                                time_start = true;
                                player_posChange = true; // 위치 이동
                            }
                        }
                        if (player_posChange) // 이동 ture
                        {
                            if (time > 1.5f) // 1.5초 이후에
                            {
                                this.transform.position = hittile.transform.position; // 위치 이동
                                this.GetComponent<SpriteRenderer>().enabled = true;
                                time_start = false;
                                time = 0;
                                player_posChange = false;
                            }
                        }
                    }
                }
                #endregion

                if (collision.tag.Equals("mid_up_down_elevator") || collision.tag.Equals("mid_down_elevator") || collision.tag.Equals("mid_up_elevator"))
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        Vector2 campos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        Ray2D ray = new Ray2D(campos, Vector2.zero);
                        RaycastHit2D middlehit = Physics2D.Raycast(ray.origin, ray.direction, 1 << 10);

                        if (middlehit.collider.tag.Equals("mid_down_elevator") || middlehit.collider.tag.Equals("mid_up_elevator") || middlehit.collider.tag.Equals("mid_up_down_elevator"))
                        {
                            StartCoroutine(ChangePlayer(middlehit.transform));
                            // 클릭 시, 해당 엘리베이터로 이동하기 위해, 클릭 때 위치값을 보내줘야한다.
                        }

                    }
                }
            }
        }
    }
    IEnumerator ChangePlayer(Transform tr)
    {
        this.GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(1.5f); 
        this.transform.position = tr.position; // 위치 이동
        this.GetComponent<SpriteRenderer>().enabled = true;
    }
}


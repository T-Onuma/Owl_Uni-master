using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class xi : MonoBehaviour {
    Vector3 rotatePoint = Vector3.zero;  //回転の中心
    Vector3 rotateAxis = Vector3.zero;   //回転軸
    float cubeAngle = 0f;                //回転角度

    float cubeSizeHalf;                  //キューブの大きさの半分
    bool isRotate = false;               //回転中に立つフラグ。回転中は入力を受け付けない

     float x_hantei = 0;
     float z_hantei = 0;

    float movement = 0;

    public int sai_info = 3;
    public int kari_pos = 6; 
    // bool
    GameObject Pl;
    void Start()
    {
        Pl = GameObject.Find("Player");
        cubeSizeHalf = transform.localScale.x / 2f;
        StartCoroutine(Control());
        
    }

    void Update()
    {
            //return;

       
    }
    IEnumerator Control()
    {
        
        while (true) {
            Controller Cl = Pl.GetComponent<Controller>();
            
            ///回転スクリプト
            //回転中は入力を受け付けない
            if (!isRotate)
            {
                if (Input.GetKey("w"))
                {
                    if (kari_pos >= 6)
                    {
                        rotatePoint = transform.position + new Vector3(0f, -cubeSizeHalf, cubeSizeHalf);
                        rotateAxis = new Vector3(1, 0, 0);
                        kari_pos -= 6;
                    }
                }
                if(Input.GetKey("s"))
                {
                    if (kari_pos <=29)
                    {
                        rotatePoint = transform.position + new Vector3(0f, -cubeSizeHalf, -cubeSizeHalf);
                        rotateAxis = new Vector3(-1, 0, 0);
                        kari_pos += 6;
                    }
                    
                }
                if (Input.GetKey("a"))
                {
                    if (kari_pos  % 6 != 0) { 
                        rotatePoint = transform.position + new Vector3(-cubeSizeHalf, -cubeSizeHalf, 0f);
                        rotateAxis = new Vector3(0, 0, 1);
                        kari_pos -= 1;
                    }
                }
                if (Input.GetKey("d"))
                {
                    if ((kari_pos + 1) % 6 != 0)
                    {
                        rotatePoint = transform.position + new Vector3(cubeSizeHalf, -cubeSizeHalf, 0f);
                        rotateAxis = new Vector3(0, 0, -1);
                        kari_pos += 1;
                    }
                }
                // 入力がない時はコルーチンを呼び出さないようにする
                if (rotatePoint != Vector3.zero) { StartCoroutine(MoveCube()); }
                /*右上2.5　2.5
                左下-2.5　-2.5*/
                x_hantei = transform.position.x - (int)transform.position.x;
                z_hantei = transform.position.z - (int)transform.position.z;
                
            }
            //////////////////////////////////////////////////////////////////////////////////
            //スライドスクリプト
            //////////////////////////////////////////////////////////////////////////////////

            if (Input.GetKey("t"))
            {
                if (kari_pos >= 6)
                {

                    transform.position = new Vector3(1, 0, 0);
                    kari_pos -= 6;
                }
            }
            if (Input.GetKey("g"))
            {
                if (kari_pos <= 29)
                {

                    transform.position = new Vector3(-1, 0, 0);
                    kari_pos += 6;
                }

            }
            if (Input.GetKey("f"))
            {
                if (kari_pos % 6 != 0)
                {

                    transform.position = new Vector3(0, 0, 1);
                    kari_pos -= 1;
                }
            }
            if (Input.GetKey("h"))
            {
                if ((kari_pos + 1) % 6 != 0)
                {

                    transform.position = new Vector3(0, 0, -1);
                    kari_pos += 1;
                }
            }
            yield return null;
        }
        
    }

              //  if (Input.GetKey(KeyCode.DownArrow))
    //Cl.x_vec == 1;
    IEnumerator MoveCube()
    {
        //回転中のフラグを立てる
        isRotate = true;

        //回転処理
        float sumAngle = 0f; //angleの合計を保存
        while (sumAngle < 90f)
        {
            cubeAngle = 10f; //ここを変えると回転速度が変わる
            sumAngle += cubeAngle;

            // 90度以上回転しないように値を制限
            if (sumAngle > 90f)
            {
                cubeAngle -= sumAngle - 90f;
            }
            transform.RotateAround(rotatePoint, rotateAxis, cubeAngle);

            yield return null;
        }

        if (x_hantei != 0 || z_hantei != 0)
        {
            if (kari_pos != 0)
            {
                transform.position = new Vector3(kari_pos % 6, 0, -(kari_pos / 6));
                // Debug.Log("補正");
            }
            else
            {
                transform.position = new Vector3(0, 0, 0);
                //  Debug.Log("補正");
            }

        }
        //回転中のフラグを倒す
        isRotate = false;
        rotatePoint = Vector3.zero;
        rotateAxis = Vector3.zero;

        //四方にray
        // if(Physics.Raycast(transform.position,))
        yield break;
    }
   
}

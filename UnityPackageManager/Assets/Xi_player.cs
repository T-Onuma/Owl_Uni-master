using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Xi_player : MonoBehaviour {

	// Use this for initialization

    public bool moveflag = false;
    public float speed;

    public float testmove = 0;

    public float angle = 0;

    public int pos = 14;
    public int f_pos = 0;//移動先判定用

    //ズレ補正用
    public float x_hantei = 0;
    public float z_hantei = 0;

    public int obj_status = 8;//管理スクリプト用　0なし　1～6サイコロ　7サイコロ（消滅猶予）8プレイヤ

    public int x_vec = 0;
    public int z_vec = 0;
    public bool DDD;


   // Vector3 c_pos;
    public GameObject chara;
    // Use this for initialization
    void Start()
    {
        Vector3 c_pos = transform.position;
        StartCoroutine(MoveControl());
        c_pos = new Vector3(pos % 6, 0, -(pos / 6));
        transform.position = c_pos;
    }

    // Update is called once per frame
    IEnumerator MoveControl()
    {
        while (true)
        {
            if (moveflag == false)
            {
                if (Input.GetKey("up"))
                {
                    if (pos >= 6)
                    {
                        while (true)
                        {


                            transform.position = transform.position + new Vector3(0, 0, 1 / speed);
                            testmove += 1 / speed;
                            if (testmove >= 1)
                            {
                                testmove = 0;
                                break;
                            }
                            z_vec = 1;
                            yield return null;

                        }
                        pos -= 6;
                    }
                }
                if (Input.GetKey("down"))
                {
                    if (pos <= 29)
                    {
                        while (true)
                        {
                            transform.position = transform.position + new Vector3(0, 0, -1 / speed);
                            testmove += 1 / speed;
                            if (testmove >= 1)
                            {
                                testmove = 0;
                                break;
                            }
                            z_vec = -1;
                            yield return null;
                        }
                        pos += 6;
                    }
                }

                if (Input.GetKey("right"))
                {
                    if ((pos + 1) % 6 != 0)
                    {
                        while (true)
                        {
                            transform.position = transform.position + new Vector3(1 / speed, 0, 0);
                            testmove += 1 / speed;
                            if (testmove >= 1)
                            {
                                testmove = 0;
                                break;
                            }
                            x_vec = 1;
                            yield return null;

                        }
                        pos += 1;
                    }
                }
                if (Input.GetKey("left"))
                {
                    if (pos % 6 != 0)
                    {
                        while (true)
                        {
                            transform.position = transform.position + new Vector3(-1 / speed, 0, 0);
                            testmove += 1 / speed;
                            if (testmove >= 1)
                            {
                                testmove = 0;
                                break;
                            }
                            x_vec = -1;
                            yield return null;

                        }
                        pos -= 1;
                    }
                }
                DDD = false;
                x_vec = 0;
                z_vec = 0;
            }
            if (Input.GetKey("space"))
            {
                transform.position = transform.position + new Vector3(0, 1 / speed, 0);
            }

            if (Input.anyKey && Input.GetKey("space") != true)
            {
              
                moveflag = true;
            }
            else
            {
                x_hantei = transform.position.x - (int)transform.position.x;
                z_hantei = transform.position.z - (int)transform.position.z;
                moveflag = false;
            }
            //OnCollisionEnter();
            // Debug.Log("毎フレームの処理");


            if (transform.position.y > 0 && Input.GetKey("space") != true)
            {
                transform.position = transform.position + new Vector3(0, -1 / speed, 0);
                Debug.Log("補正");
            }
            if (x_hantei != 0 || z_hantei != 0)
            {
                if (pos != 0)
                {
                    transform.position = new Vector3(pos % 6, 0, -(pos / 6));
                    // Debug.Log("補正");
                }
                else
                {
                    transform.position = new Vector3(0, 0, 0);
                    //  Debug.Log("補正");
                }

            }
            if (transform.position.y < 0)
            {
                transform.position = new Vector3(transform.position.x, 0, transform.position.z);
            }
            yield return null;
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}

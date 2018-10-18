using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class xiBase : MonoBehaviour {
    
    bool[] pos=new bool[36];
    //
    // 0  1  2  3  4   5
    // 6  7  8  9  10  11
    // 12 13 14 15 16  17
    // 18 19 20 21 22  23
    // 24 25 26 27 28  29
    // 30 31 32 33 34  35
    //
    //zposは
    //xは+1
    //z*6+xで現在の配列の場所
    //
    //
    //0=-2.5,1,2.5
    //5=2.5,1,2.5
    //35=2.5,1,-2.5
    //30=-2.5,1,-2.5
    //上-6　停止条件n<=5
    //右+1　停止条件(n+1)%6==0
    //左-1　停止条件n%6==0
    //下+6　停止条件n>=30
    //0 1 2 3 4  5
    //0 1 2 3 4  5
    //0 1 2 3 4  5
    //0 1 2 3 4  5
    //0 1 2 3 4  5
    //0 1 2 3 4  5
    // Use this for initialization


    //  public Xi_player xi_player;

    public GameObject ref_obj;
    public Xi_player xi_pl;
    public ObjInfo[] objinfo;
    public class ObjInfo
    {
        public ObjInfo()
        {
            base_info = 0;
            base_pos = 0;
        }
        public int base_info;//オブジェクトの状態　0なし　1～6サイコロ　7プレイヤ
        public int base_pos;//配列で表現
                            //public int blast_rady;//消滅猶予
        void cleanup()
        {
            if (base_info == 0)
            {

            }
        }
    }
    void Start () {
        objinfo = new ObjInfo[36];
        for(int i = 0; i < objinfo.Length; i++) {

            objinfo[i] = new ObjInfo();
        }




        ref_obj = GameObject.FindWithTag("Player");
        if (xi_pl == null)
        {
            xi_pl = GameObject.Find("Player").GetComponent<Xi_player>();
        }
        else
        {
            Debug.Log("失敗");
        }
            
            objinfo[1].base_info = xi_pl.obj_status;
        objinfo[1].base_pos = xi_pl.pos;

    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
          //  xi_pl = GameObject.Find("Player").GetComponent<Xi_player>();

            //for(int i = 0; i < 36; i++)
            //{
            //      if (pos[i] == false)
            //      {
            //          Debug.Log("null");
            //      }
            //}
            //Debug.Log((-player.transform.position.z/6)+player.transform.position.x);
           // Debug.Log(/*test.xi_info*/xi_pl.obj_status);
           //Debug.Log(/*test.xi_pos */xi_pl.pos);
            Debug.Log(objinfo[1].base_info);
            Debug.Log(objinfo[1].base_pos);

        }

    }
}

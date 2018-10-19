using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DiceControll : MonoBehaviour {////////////////////stageSettingのフラグ処理
                                           /// </summary>
    //////////////上面判定用(削除予定)
   //public BoxCollider[] surface =new BoxCollider[6];
    

    Vector3 rotatePoint = Vector3.zero;  //回転の中心
    Vector3 rotateAxis = Vector3.zero;   //回転軸
    float cubeAngle = 0f;                //回転角度

    float cubeSizeHalf;                  //キューブの大きさの半分
    bool isRotate = false;               //回転中に立つフラグ。回転中は入力を受け付けない


    float x_hantei = 0;
    float z_hantei = 0;
    public StageSetting stageSetting;

    float rayMaxDistance = 1.0f;
    //現在地
    [SerializeField]
    private int dicePosX = 1;
    [SerializeField]
    private int dicePosZ = 1;
    //移動先地点(存在しない値として初期値-1代入)
    private int bfDicePosX = -1;
    private int bfDicePosZ = -1;
    // bool
    GameObject refObj;
    GameObject Pl;
    [SerializeField]
    int upNumber;//上面の数字

    [SerializeField]
    int chainCount;//chein終端が上面の数以上ならbanish

    GameObject firstChainObj;//コンボの発火点
    public bool banishFlag=false;//banish用フラグ trueでbanish
    public bool chainCheckFlag = false;//コンボ数検査の二度付け禁止

    public DiceControll firstDiceControll;
    public DiceControll chainDiceControll;
    public DiceControll[] stackControll = new DiceControll[5];
   
    
    void Start()
    {
        refObj = GameObject.Find("StageManager");
        stageSetting = refObj.GetComponent<StageSetting>();
        Pl = GameObject.Find("Player");
        cubeSizeHalf = transform.localScale.x / 2f;

        chainCheckFlag = false;
        ////////////////////////////////////////////////////////削除予定
        //iTween.MoveTo(gameObject, iTween.Hash("y", 5f));
        //StartCoroutine(Control());
        /////////////////////////////////////////////////////////
    }

    void Update()
    {
        // 入力がない時はコルーチンを呼び出さないようにする
        if (rotatePoint != Vector3.zero&&!isRotate) { StartCoroutine(MoveCube()); }
       
    }

    void ReceivePoint(GameObject chainFirstObj)
    {
        int wasdCheckFlag;

        wasdCheckFlag = 0;
        //上面数字更新判定 
        NumberCheck();
        chainCheckFlag = true;//二度付け禁止フラグオン

        firstDiceControll = chainFirstObj.GetComponent<DiceControll>();
        firstDiceControll.chainCount += 1;
        if (firstDiceControll.chainCount >= firstDiceControll.upNumber)
        {
            banishFlag = true;
            Banish();

        }
        else if (firstDiceControll.chainCount < firstDiceControll.upNumber)
        {
            bool stackChecker = false;
            for (int i = 0; stackChecker == true;i++)
            {
                if (firstDiceControll.stackControll[i]==null)
                {
                    firstDiceControll.stackControll[i] = this.gameObject.GetComponent<DiceControll>();
                    stackChecker = true;
                }
                if (i >= 5)
                {
                    Debug.Log("stackControllの範囲外、要調査");
                    stackChecker = true;
                }
                    
            }
        }
        //四方上下にray + 数字更新  ここっから
        RaycastHit receiveChain;//連鎖時用
       
        
        //四方
        if (Physics.Raycast(transform.position, Vector3.forward, out receiveChain,rayMaxDistance))
        {
            if (receiveChain.collider.tag == "dice")
            {
                Debug.Log(receiveChain.collider.tag);
                chainDiceControll = receiveChain.collider.gameObject.GetComponent<DiceControll>();
                if (upNumber == chainDiceControll.upNumber&&chainDiceControll.chainCheckFlag==false)
                {
                    chainDiceControll.ReceivePoint(firstChainObj);
                }
                else { wasdCheckFlag += 1; }
            }
            
        }
        if (Physics.Raycast(transform.position, Vector3.back, out receiveChain, rayMaxDistance))
        {
            if (receiveChain.collider.tag == "dice")
            {
                Debug.Log(receiveChain.collider.tag);
                chainDiceControll = receiveChain.collider.gameObject.GetComponent<DiceControll>();
                if (upNumber == chainDiceControll.upNumber&&chainDiceControll.chainCheckFlag==false)
                {
                    chainDiceControll.ReceivePoint(firstChainObj);
                }
                else { wasdCheckFlag += 1; }
            }
        }
        if (Physics.Raycast(transform.position, Vector3.left, out receiveChain, rayMaxDistance))
        {
            if (receiveChain.collider.tag == "dice")
            {
                Debug.Log(receiveChain.collider.tag);
                chainDiceControll = receiveChain.collider.gameObject.GetComponent<DiceControll>();
                if (upNumber == chainDiceControll.upNumber&&chainDiceControll.chainCheckFlag==false)
                {
                    chainDiceControll.ReceivePoint(firstChainObj);
                }
                else { wasdCheckFlag += 1; }
            }
        }
        if (Physics.Raycast(transform.position, Vector3.right, out receiveChain, rayMaxDistance))
        {
            if (receiveChain.collider.tag == "dice")
            {
                Debug.Log(receiveChain.collider.tag);
                chainDiceControll = receiveChain.collider.gameObject.GetComponent<DiceControll>();
                if (upNumber == chainDiceControll.upNumber&&chainDiceControll.chainCheckFlag==false)
                {
                    chainDiceControll.ReceivePoint(firstChainObj);
                }
                else { wasdCheckFlag += 1; }
            }
        }
        if(wasdCheckFlag == 4&& firstDiceControll.chainCount < firstDiceControll.upNumber)//連鎖数不足の場合chainncountを0に
        {
            firstDiceControll.chainCheckFlag = false;
            firstDiceControll.chainCount = 0;
        }
        if (wasdCheckFlag == 4&&firstDiceControll.banishFlag==false && firstDiceControll.chainCount >= firstDiceControll.upNumber)
        {
            StackBanish();
            firstDiceControll.banishFlag=true;
        }
    }
    
    void StackBanish()
    {
        for (int i = 0; i<=4; i++)
        {
            if (firstDiceControll.stackControll[i] != null)
            {
                stackControll[i].banishFlag = true;
                stackControll[i].Banish();
            }
        }
        CleanStack();
    }
    GameObject IgnitionPoint()
    {
        chainCheckFlag = true;
        firstChainObj = this.gameObject;
        return firstChainObj;
    }

    public void Banish()
    {
        iTween.ScaleTo(gameObject, iTween.Hash("x", 1.1f,
                "time", 1.5f,
                "oncomplete", "OncompleteBanish",
                "oncompletetarget", this.gameObject));
    }
    public void OncompleteBanish()
    {
        Destroy(this.gameObject);
        stageSetting.InfoBanith(dicePosX, dicePosZ);
    }

    void CleanStack()
    {
        for (int i = 0; i <= 4; i++)//スタック内洗浄
        {
            stackControll[i] = null;
        }
    }

    public void DiceSpawn(int x,int z)
    {   //spawn時シャッフル機構
        transform.Rotate(Random.Range(0, 4) * 90, Random.Range(0, 4) * 90, Random.Range(0, 4) * 90);

        NumberCheck();
        //
        dicePosX = x;
        dicePosZ = z;
        iTween.MoveTo(this.gameObject, iTween.Hash("y", 0f));
    }

    void WASD_Checker()
    {
        //四方上下にray + 数字更新
        RaycastHit chain;//連鎖時用
        //上面数字更新判定
        NumberCheck();
        //Raycast位置ずらし用vec3
        Vector3 Raypos1 = transform.position;
        Vector3 Raypos2 = transform.position;
        Vector3 Raypos3 = transform.position;
        Vector3 Raypos4 = transform.position;

        Raypos1 += new Vector3(0,0,0.35f);
        Raypos2 += new Vector3(0,0,-0.35f);
        Raypos3 += new Vector3(-0.35f,0,0);
        Raypos4 += new Vector3(0.35f,0,0);
        //四方
        if (Physics.Raycast(Raypos1, Vector3.forward, out chain, rayMaxDistance))
        {

            //debug.log(chain.collider.tag);
            if (chain.collider.tag == "dice")
            {
                chainDiceControll = chain.collider.gameObject.GetComponent<DiceControll>();
                if(upNumber == chainDiceControll.upNumber && chainDiceControll.banishFlag == true)
                {
                    banishFlag = true;
                    Banish();
                }
                else if (upNumber == chainDiceControll.upNumber)
                {
                    chainDiceControll.ReceivePoint(IgnitionPoint());
                }
            }
        }
        if (Physics.Raycast(Raypos2, Vector3.back, out chain, rayMaxDistance))
        {
            //debug.log(chain.collider.tag);
            if (chain.collider.tag == "dice")
            {
                chainDiceControll = chain.collider.gameObject.GetComponent<DiceControll>();
                if (upNumber == chainDiceControll.upNumber && chainDiceControll.banishFlag == true)
                {
                    banishFlag = true;
                    Banish();
                }
                else if(upNumber == chainDiceControll.upNumber)
                {
                    chainDiceControll.ReceivePoint(IgnitionPoint());
                }
            }
        }
        if (Physics.Raycast(Raypos3, Vector3.left, out chain, rayMaxDistance))
        {
            //debug.log(chain.collider.tag);
            if (chain.collider.tag == "dice")
            {
                chainDiceControll = chain.collider.gameObject.GetComponent<DiceControll>();
                if (upNumber == chainDiceControll.upNumber && chainDiceControll.banishFlag == true)
                {
                    banishFlag = true;
                    Banish();
                }
                else if (upNumber == chainDiceControll.upNumber)
                {
                    chainDiceControll.ReceivePoint(IgnitionPoint());
                }
            }
        }
        if (Physics.Raycast(Raypos4, Vector3.right, out chain, rayMaxDistance))
        {
            //debug.log(chain.collider.tag);
            if (chain.collider.tag == "dice")
            {
                chainDiceControll = chain.collider.gameObject.GetComponent<DiceControll>();
                if (upNumber == chainDiceControll.upNumber && chainDiceControll.banishFlag == true)
                {
                    banishFlag = true;
                    Banish();
                }
                else if (upNumber == chainDiceControll.upNumber)
                {
                    chainDiceControll.ReceivePoint(IgnitionPoint());
                }
                
            }
        }
    }

    private void NumberCheck()
    {
        RaycastHit check;//連鎖時用
        //s数字確認
        if (Physics.Raycast(transform.position, Vector3.up, out check))
        {
            if (check.collider.tag == "1")
            {
                upNumber = 1;
            }
            if (check.collider.tag == "2")
            {
                upNumber = 2;
            }
            if (check.collider.tag == "3")
            {
                upNumber = 3;
            }
            if (check.collider.tag == "4")
            {
                upNumber = 4;
            }
            if (check.collider.tag == "5")
            {
                upNumber = 5;
            }
            if (check.collider.tag == "6")
            {
                upNumber = 6;
            }
        }
    }
    ////////////////////////////////////////////////////////
    ///ローテート関数
    ////////////////////////////////////////////////////////
    public void ForwardRotate()
    {
        if (banishFlag)
        {
            return;
        }
        if (dicePosZ >= 1 && stageSetting.MoveChecker(dicePosX, dicePosZ - 1))
        {
            if (isRotate){ return ; }//回転中はリターン
            bfDicePosX = dicePosX;
            bfDicePosZ = dicePosZ;
            dicePosZ -= 1;
            rotatePoint = transform.position + new Vector3(0f, -cubeSizeHalf, cubeSizeHalf);
            rotateAxis = new Vector3(1, 0, 0);
            stageSetting.InfoControll(dicePosX, dicePosZ, bfDicePosX, bfDicePosZ);//StageSetting側のフラグ処理
            return ;
        }
        else
        {
            return ;
        }
    }

    public void BackRotate()
    {
        if (banishFlag)
        {
            return;
        }
        if (dicePosZ < stageSetting.stageZ - 1 && stageSetting.MoveChecker(dicePosX, dicePosZ + 1))
        { 
            if (isRotate) { return ; }//回転中はリターン
            bfDicePosX = dicePosX;
            bfDicePosZ = dicePosZ;
            dicePosZ += 1;
            rotatePoint = transform.position + new Vector3(0f, -cubeSizeHalf, -cubeSizeHalf);
            rotateAxis = new Vector3(-1, 0, 0);
            stageSetting.InfoControll(dicePosX, dicePosZ, bfDicePosX, bfDicePosZ);//StageSetting側のフラグ処理
            return ;
        }
        else
        {
            return ;
        }
    }

    public void LeftRotate()
    {
        if (banishFlag)
        {
            return;
        }
        if (dicePosX >= 1 && stageSetting.MoveChecker(dicePosX - 1, dicePosZ))
        {
            if (isRotate) { return ; }//回転中はリターン
            bfDicePosX = dicePosX;
            bfDicePosZ = dicePosZ;
            dicePosX -= 1;
            rotatePoint = transform.position + new Vector3(-cubeSizeHalf, -cubeSizeHalf, 0f);
            rotateAxis = new Vector3(0, 0, 1);
            stageSetting.InfoControll(dicePosX, dicePosZ, bfDicePosX, bfDicePosZ);//StageSetting側のフラグ処理
            return ;
        }
        else
        {
            return ;
        }
    }

    public void RightRotate()
    {
        if (banishFlag)
        {
            return;
        }

        if (dicePosX < stageSetting.stageX - 1 && stageSetting.MoveChecker(dicePosX + 1, dicePosZ))
        {
            if (isRotate) { return ; }//回転中はリターン
            bfDicePosX = dicePosX;
            bfDicePosZ = dicePosZ;
            dicePosX += 1;
            rotatePoint = transform.position + new Vector3(cubeSizeHalf, -cubeSizeHalf, 0f);
            rotateAxis = new Vector3(0, 0, -1);
            stageSetting.InfoControll(dicePosX, dicePosZ, bfDicePosX, bfDicePosZ);//StageSetting側のフラグ処理
            return ;
        }
        else
        {
            return ;
        }
    }
    ////////////////////////////////////////////////////////
    ///スライド関数
    ////////////////////////////////////////////////////////
    public bool ForwardSlide()
    {
        if (banishFlag)
        {
            return false;
        }
        if (dicePosZ >= 1 && stageSetting.MoveChecker(dicePosX, dicePosZ - 1) )//条件追加　移動先ダイスの場合
        {

            bfDicePosX = dicePosX;
            bfDicePosZ = dicePosZ;
            dicePosZ -= 1;
            iTween.MoveTo(gameObject, iTween.Hash("z", -dicePosZ, "time", 0.1f));
            stageSetting.InfoControll(dicePosX, dicePosZ, bfDicePosX, bfDicePosZ);//StageSetting側のフラグ処理
            WASD_Checker();
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool BackSlide()
    {
        if (banishFlag)
        {
            return false;
        }
        if (dicePosZ < stageSetting.stageZ - 1 && stageSetting.MoveChecker(dicePosX, dicePosZ + 1) )
        {
            bfDicePosX = dicePosX;
            bfDicePosZ = dicePosZ;
            dicePosZ += 1;
            iTween.MoveTo(gameObject, iTween.Hash("z", -dicePosZ, "time", 0.1f));
            stageSetting.InfoControll(dicePosX, dicePosZ, bfDicePosX, bfDicePosZ);//StageSetting側のフラグ処理
            WASD_Checker();
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool LeftSlide()
    {
        if (banishFlag)
        {
            return false;
        }
        if (dicePosX >= 1 && stageSetting.MoveChecker(dicePosX - 1, dicePosZ))
        {
            bfDicePosX = dicePosX;
            bfDicePosZ = dicePosZ;
            dicePosX -= 1;
            iTween.MoveTo(gameObject, iTween.Hash("x", dicePosX, "time", 0.1f));
            stageSetting.InfoControll(dicePosX, dicePosZ, bfDicePosX, bfDicePosZ);//StageSetting側のフラグ処理
            WASD_Checker();
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool RightSlide()
    {
        if (banishFlag)
        {
            return false;
        }
        if (dicePosX < stageSetting.stageX - 1 && stageSetting.MoveChecker(dicePosX + 1, dicePosZ))
        {
            bfDicePosX = dicePosX;
            bfDicePosZ = dicePosZ;
            dicePosX += 1;
            iTween.MoveTo(gameObject, iTween.Hash("x", dicePosX, "time", 0.1f));
            stageSetting.InfoControll(dicePosX, dicePosZ, bfDicePosX, bfDicePosZ);//StageSetting側のフラグ処理
            WASD_Checker();
            return true;
        }
        else
        {
            return false;
        }
    }



    IEnumerator MoveCube()
    {
        //回転中のフラグを立てる
        isRotate = true;

        //回転処理
        float sumAngle = 0f; //angleの合計を保存
        while (sumAngle < 90f)
        {
            cubeAngle = 20f; //ここを変えると回転速度が変わる
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
            if (dicePosX != 0|| dicePosZ != 0)
            {
                transform.position = new Vector3(dicePosX , 0, -dicePosZ );
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

        WASD_Checker();
        

        yield break;
    }
  

}

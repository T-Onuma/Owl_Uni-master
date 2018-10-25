using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*コントローラー改変途中　posを二次配列対応予定　メモ
 */
public class Controller : MonoBehaviour
{
    public StageSetting stageSetting;//stagesetting側
    GameObject refObj;

    /// <summary>
    bool firstEntry = false;//登場時位置補正抑制用
    /// </summary>
    [SerializeField]
    private int posX = 4;//現ｘ座標　初期位置4,4により４で初期化
    [SerializeField]
    private int posY = 4;//現y座標　初期位置4,4より4をで初期化

    public int f_posX = 0;//移動先判定用 X座標
    public int f_posY = 0;//移動先判定用 Y座標

    private int height = 0;//高度情報//0 地上　1 2段目
    [SerializeField]
    private bool transparentFlag = false;//透過
   

    const float maxDistance = 1.0f;//レイの長さ 1で固定

  


    //ダイス壁隣接時強制停止用フラグ
    bool forceStop;

    // 処理が終わったどうかを示すフラグ
    [SerializeField]
    bool iTweenMoving = false;
    // 処理が終わったら呼び出され、フラグをクリアする。

    Vector3 c_pos;

    DiceControll remoteDice;
    // Use this for initialization
    void Start()
    {
        refObj = GameObject.Find("StageManager");
        stageSetting = refObj.GetComponent<StageSetting>();
        iTweenMoving = true;
        iTween.MoveFrom(this.gameObject, iTween.Hash(
            "y", 8f,
            "time", 1f, 
            "oncomplete", "OnCompleteHandler",
            "oncompletetarget", this.gameObject));
        f_posX = posX;
        f_posY = posY;
        firstEntry = true;
    }

    void Update()
    {
            RaycastHit dice;//連鎖時用

        if (!iTweenMoving && Input.GetKey("up")&& posY >= 1)//↑
        {
            iTween.RotateTo(this.gameObject, iTween.Hash("y", 270f, "time", 0.02f));
            if (height == 1&&Physics.Raycast(transform.position, Vector3.down, out dice, maxDistance))
            {
                if (dice.collider.tag == "dice"&&!transparentFlag)
                {
                    remoteDice = dice.collider.gameObject.GetComponent<DiceControll>();
                    remoteDice.ForwardRotate();
                    print("下にある、前方に移動");
                   
                }
            }
            if (height == 0 && Physics.Raycast(transform.position, Vector3.forward, out dice,maxDistance))
            {
                if (dice.collider.tag == "dice")
                {
                    remoteDice=dice.collider.gameObject.GetComponent<DiceControll>();
                    forceStop=remoteDice.ForwardSlide();
                    print("前方に移動");
                }
            }
            if (forceStop == false&&transparentFlag==false)//強制停止 透過フラグがtrueでない場合のみ
            {
                forceStop = true;//強制停止フラグ初期化
                return;
            }

            f_posY -= 1;
            // 処理中のフラグをたてとく。
            iTweenMoving = true;
            iTween.MoveAdd(this.gameObject, iTween.Hash(
                "x", 1,
                "time", 0.1f,
                "oncomplete", "OnCompleteHandler",
                "oncompletetarget", this.gameObject));
            posY -= 1;//座標用数値変更
            forceStop = true;//強制停止フラグ初期化
            transparentFlag = false;//透過フラグ初期化
        }
       
        if (!iTweenMoving && Input.GetKey("down") && posY <= 6-1)//↓
        {
            iTween.RotateTo(this.gameObject, iTween.Hash("y", 90f, "time", 0.02f));
            if (height == 1 && Physics.Raycast(transform.position, Vector3.down, out dice, maxDistance))
            {
                if (dice.collider.tag == "dice" && !transparentFlag)
                {
                    remoteDice = dice.collider.gameObject.GetComponent<DiceControll>();
                   remoteDice.BackRotate();
                    print("下にある、下方に移動");
                }
            }
            if (Physics.Raycast(transform.position, Vector3.back, out dice, maxDistance))
            {
                if (dice.collider.tag == "dice")
                {
                    remoteDice = dice.collider.gameObject.GetComponent<DiceControll>();
                    forceStop = remoteDice.BackSlide();
                    print("下方に移動");
                }
            }
            if (forceStop == false && transparentFlag == false)//強制停止 透過フラグがtrueでない場合のみ
            {
                forceStop = true;//強制停止フラグ初期化
                return;
            }

            // 処理中のフラグをたてとく。
            iTweenMoving = true;
            iTween.MoveAdd(this.gameObject, iTween.Hash(
                "x", 1,
                "time", 0.1f,
                "oncomplete", "OnCompleteHandler",
                "oncompletetarget", this.gameObject));
            posY += 1;//座標用数値変更
            forceStop = true;//強制停止フラグ初期化
            transparentFlag = false;//透過フラグ初期化
        }

        if (!iTweenMoving && Input.GetKey("left") && posX >= 1)//←
        {
            iTween.RotateTo(this.gameObject, iTween.Hash("y", 180f, "time", 0.02f));
            if (height == 1 && Physics.Raycast(transform.position, Vector3.down, out dice, maxDistance))
            {
                if (dice.collider.tag == "dice" && !transparentFlag)
                {
                    remoteDice = dice.collider.gameObject.GetComponent<DiceControll>();
                    remoteDice.LeftRotate();
                    print("下にある、左方に移動");
                }
            }
            if (Physics.Raycast(transform.position, Vector3.left, out dice, maxDistance))
            {
                if (dice.collider.tag == "dice")
                {
                    remoteDice = dice.collider.gameObject.GetComponent<DiceControll>();
                    forceStop = remoteDice.LeftSlide();
                    print("左方に移動");
                }
            }
            if (forceStop == false && transparentFlag == false)//強制停止 透過フラグがtrueでない場合のみ
            {
                forceStop = true;//強制停止フラグ初期化
                return;
            }
            // 処理中のフラグをたてとく。
            iTweenMoving = true;
            iTween.MoveAdd(this.gameObject, iTween.Hash(
                "x", +1,
                "time", 0.1f,
                "oncomplete", "OnCompleteHandler",
                "oncompletetarget", this.gameObject));
            posX -= 1;//座標用数値変更
            forceStop = true;//強制停止フラグ初期化
            transparentFlag = false;//透過フラグ初期化
        }

    
        if (!iTweenMoving && Input.GetKey("right") && posX <= 6-1)//→
        {
            iTween.RotateTo(this.gameObject, iTween.Hash("y", 0f, "time", 0.02f));
            if(height == 1 && Physics.Raycast(transform.position, Vector3.down, out dice, maxDistance))
            {
                if (dice.collider.tag == "dice" && !transparentFlag)
                {
                    remoteDice = dice.collider.gameObject.GetComponent<DiceControll>();
                    remoteDice.RightRotate();
                    print("下にある、右方に移動");
                }
            }
            if (Physics.Raycast(transform.position, Vector3.right, out dice, maxDistance))
            {
                if (dice.collider.tag == "dice")
                {
                    remoteDice = dice.collider.gameObject.GetComponent<DiceControll>();
                    forceStop = remoteDice.RightSlide();
                    print("右方に移動");
                }
            }
            if (forceStop == false && transparentFlag == false)//強制停止 透過フラグがtrueでない場合のみ
            {
                forceStop = true;//強制停止フラグ初期化
                return;
            }
            // 処理中のフラグをたてとく。
            iTweenMoving = true;
            iTween.MoveAdd(this.gameObject, iTween.Hash(
                "x", 1,
                "time", 0.1f,
                "oncomplete", "OnCompleteHandler",
                "oncompletetarget", this.gameObject));
            posX += 1;//座標用数値変更
            forceStop = true;//強制停止フラグ初期化
            transparentFlag = false;//透過フラグ初期化
        }

        if (Input.GetKey(KeyCode.J) )
        {
            transparentFlag = true;
        }
        if (Input.GetKeyUp(KeyCode.J))
        {
            transparentFlag = false;
        }
        if (stageSetting.objinfo[posX, posY])
        {
            transform.position = new Vector3(transform.position.x, 1, transform.position.z);
            height = 1;//高さカウンター変更
        }else if(!stageSetting.objinfo[posX,posY]&&! Mathf.Approximately(transform.position.y, 0))
        {
            if (!firstEntry) { return; }
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
            height = 0;//高さカウンター変更
        }

     
        if (transform.position.y < 0)
        {
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
            transparentFlag = false;
        }


        
    }
    
    void OnCompleteHandler()//itween終了時フラグ復帰
    {
        transparentFlag = false;
        iTweenMoving = false;
        transform.position = new Vector3(posX, transform.position.y, -posY);
    }
   
}

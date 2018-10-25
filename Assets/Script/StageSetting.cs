using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSetting : MonoBehaviour {
    [SerializeField]
    private GameObject StageBlock;
    [SerializeField]
    private GameObject SpawnDice;
    [SerializeField]
    private GameObject Player;
   

    //範囲設定用
    [SerializeField]
    public int stageX;
    [SerializeField]
    public int stageZ;

    private int allBlock;

    public bool[,] objinfo=new bool[7,7];//12*12 false,無し　true,有り

    private int score;
  
    public void scorePlus(int diceScore)
    {
        score += diceScore;
    }
        // Use this for initialization
        void Start ()
    {
        allBlock = stageX * stageZ;
       
        for(int i = 0; i < stageZ-1; i++)
        {
            for (int k=0;k<stageX-1;k++)
            {
                objinfo[k,i] = false;
            }
           
        }

        ///ステージ生成部分
        for(int i = 0; i < stageZ; i++)
        {
            for(int k = 0; k < stageX; k++)
            {
                Instantiate(StageBlock, new Vector3(k,-1f, -i), Quaternion.identity);
            }
        }
       
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey/*Down*/(KeyCode.L))
        {
            int spawnX = UnityEngine.Random.Range(0, 6+1);//X,0~6 Z,0~-6
            int spawnZ = UnityEngine.Random.Range(0, 6+1);//X,0~6 Z,0~-6

            RandomSpawn(spawnX,spawnZ);
            

        }
	}

    public void PlayerSpawn()
    {
        Instantiate(Player, new Vector3(3, 0, -3), Quaternion.identity);
    }
    public void RandomSpawn(int posX,int posZ)
    {
        

        if (objinfo[posX,posZ] == true) { Debug.Log("生成済み位置選択、return"); return; }//座標番号（spawnNumber）がtrueならここ以下を無視する

        objinfo[posX, posZ] = true;
        
        GameObject dice =Instantiate(SpawnDice, new Vector3(posX , -1.1f, -posZ), Quaternion.identity);//7*7だが原点０を考慮し6とする
        DiceControll diceControll = dice.GetComponent<DiceControll>();
        diceControll.DiceSpawn(posX, posZ);
        Debug.Log(posX+","+posZ);//生成時座標確認用
    }
    public void InfoControll(int afX,int afZ ,int bfX,int bfZ )//移動後に呼び出し 
    {

        objinfo[afX, afZ] = true;
        objinfo[bfX, bfZ] = false;

    }
    public void InfoBanith(int x,int z)
    {
        objinfo[x, z] = false;
    }
    public bool MoveChecker(int fPosX, int fPosZ)
    {
        if (!objinfo[fPosX,fPosZ])
        {
            return true;
        }
        else { return false; }

    }

}

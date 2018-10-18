using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartTextControll : MonoBehaviour {
    public Text Qtext;
    bool a_flag;
    float a_color;

    bool once=true;
    //
    [SerializeField]
    private GameObject StManager;
    // Use this for initialization
    void Start()
    {
        a_flag = false;
        a_color = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //左クリックでa_flagをTrueにする
        if (Input.GetKeyDown(KeyCode.Space)&&once==true)
        {
            a_flag = true;
            a_color = 1;
            once = false;
        }
        //a_flagがtrueの間実行する
        if (a_flag)
        {
            //テキストの透明度を変更する
            Qtext.color = new Color(255, 255, 255, a_color);
            a_color -= Time.deltaTime;
            //透明度が0になったら終了する。
            if (a_color < 0)
            {
                a_color = 0;
                a_flag = false;
                StageSetting stageSetting = StManager.GetComponent<StageSetting>();
                stageSetting.PlayerSpawn();
            }
        }
    }
}

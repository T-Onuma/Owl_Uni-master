using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSceneDice : MonoBehaviour {
    private float timeleft;
    private int x = 3;
    private int y = 3;
    private int z = 3;
    bool startFlag = false;
    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (!startFlag) {
            transform.Rotate(new Vector3(x, y, z));
            timeleft -= Time.deltaTime;
            if (timeleft <= 0.0)
            {
                timeleft = 1.0f;

                x = Random.Range(-5, 12);
                y = Random.Range(-5, 12);
                z = Random.Range(-5, 12);
            }
        }
        

    }
    void  StartIE(){
        StartCoroutine(StartControll());
        }
    IEnumerator StartControll()
    {
        transform.Rotate(new Vector3(0, 0, z));
        yield return null;
    }
}

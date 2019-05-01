using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {
    public static int score;//因為每個關卡成績會累計,所以static
    public static int ScoreNum=0;
    private Text scoreText;

    private void Awake()
    {
        scoreText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update () {
        //改變unity中scoreText上的文字
        scoreText.text = "Score："+score;
	}
}

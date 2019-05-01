using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoManager : MonoBehaviour {
    //目前彈藥.彈匣容量.總彈藥量.重新裝填時間
    public static int AmmoCurrent=10;
    public static int AmmoCapNum =10;
    public static int AmmoTotal=900;
    public static int AmmoTotalNum = 900;
    /*public readonly float ReloadTime = 3f;
    public float nextReloadTime;
    //重新裝填文字
    public GameObject AmmoReload;
    */
    // Static Manager
    //public static AmmoManager _Instance;


    private Text AmmoText;

    private void Awake()
    {
        AmmoText = GetComponent<Text>();
        //_Instance = this;
    }
    /*public void AmmoReloading()
    {
        nextReloadTime = Time.time + ReloadTime;
    }*/

    // Update is called once per frame
    void Update () {
        /*if (AmmoCurrent==0 && Time.time >= nextReloadTime)
        {
        
            AmmoCurrent = 30;
            AmmoTotal = AmmoTotal - 30;
            AmmoReload.SetActive(false);
        }*/
        AmmoText.text = AmmoCurrent + "/" + AmmoTotal;

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {
    //初始血量,控制條的血量,目前實際的血量
    public int startingHealth = 100;
    public Slider healthSlider;
    private static int currentHealth;//static,關卡轉換場景的時候如果沒設可能會消失

    //被攻擊
    public AudioClip deathClip;//死亡音效
    public Image damageImage;//被打特效
    public float flashSpeed = 5f;//閃爍速度
    public Color flashColor = new Color(1f, 0f, 0f, 0.1f);//閃爍顏色,Color(R,G,B,A)
    private bool damaged = false;//是否被攻擊
    private AudioSource playerAudio;

    private bool isDeath = false;//角色死亡動畫
    private Animator playerAnimator;//找到player身上的動畫控制器

    //event
    public delegate void PlayerDeathAction();
    public static event PlayerDeathAction PlayerDeathEvent;

    // Use this for initialization
    private void Awake()
    {
        //遊戲開始初始血量=血量最大值
        healthSlider.maxValue = startingHealth;
        //如果血量歸0,把血量變為初始血量(100)
        if (currentHealth <= 0)
        {
            healthSlider.value = startingHealth;
            currentHealth = startingHealth;
        }
        else
        {
            healthSlider.value = startingHealth;
        }
        playerAudio = GetComponent<AudioSource>();
        playerAnimator = GetComponent<Animator>();
    }

    private void Death()
    {
        isDeath = true;
        playerAudio.clip = deathClip;
        playerAudio.Play();
        playerAnimator.SetTrigger("Die");

        //死亡後讓角色不能移動
        GetComponent<PlayMovement>().enabled = false;
        //死亡後讓角色不能開槍(射擊的腳本在player底下的GunbarrelEnd)
        GetComponentInChildren<PlayerShooting>().enabled = false;

        if (PlayerDeathEvent!=null)//沒!=null易出錯
        {
            PlayerDeathEvent();
        }
    }


    //受到攻擊
    public void TakeDamage(int amount)
    {
        if (isDeath)
        {
            return;
        }
        playerAudio.Play();
        damaged = true;
        currentHealth -= amount;
        healthSlider.value = currentHealth;
        if (currentHealth<=0)
        {
           Death();
        }
    }
    private void Update()
    {
        if (damaged)
        {
            damaged = false;//打開後馬上關閉製造閃爍效果
            damageImage.color = flashColor;
        }
        else
        {
            //                        現在的數值(顏色),想要變化到的數值(clear=0000)
            damageImage.color = Color.Lerp(damageImage.color,Color.clear,Time.deltaTime*flashSpeed);
        }
    }
}

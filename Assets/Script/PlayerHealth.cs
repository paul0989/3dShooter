using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {    
    public readonly int startingHealth = 100;
    //初始血量,控制條的血量,目前實際的血量(readonly=唯讀)

    public Slider healthSlider;
    private static int currentHealth;
    //static,關卡轉換場景的時候如果沒設可能會消失

    //被攻擊
    public AudioClip deathClip;
    //死亡音效
    public AudioClip hurtClip;
    //被打音效
    public Image damageImage;
    //被打特效
    public float flashSpeed = 5f;
    //閃爍速度
    public Color flashColor = new Color(1f, 0f, 0f, 0.1f);
    //閃爍顏色,Color(R,G,B,A)
    private bool damaged = false;
    //是否被攻擊    
    private AudioSource playerAudio;

    //被補血
    public Image HealImage;
    //補血特效
    private bool healed = false;
    //是否被補血
    public float HealflashSpeed = 5f;
    //閃爍速度
    public Color HealflashColor = new Color(0f, 1f, 0f, 0.1f);
    //閃爍顏色,Color(R,G,B,A)

    private bool isDeath = false;
    //角色死亡動畫
    private Animator playerAnimator;
    //找到player身上的動畫控制器

    //event
    public delegate void PlayerDeathAction();
    public static event PlayerDeathAction PlayerDeathEvent;

    public delegate void PlayerContinueAction();
    public static event PlayerContinueAction PlayerContinueEvent;

    public delegate void PlayerHealthAction();
    public static event PlayerHealthAction PlayerHealthEvent;

    public delegate void PlayerEatHealthAction();
    public static event PlayerEatHealthAction PlayerEatHealthEvent;




    //private bool isRevival = false;//復活切換
    public GameObject Restart;
    public GameObject Continue;

    //private float delay = 2.0f;

    // Use this for initialization
    private void Awake()
    {        
        healthSlider.maxValue = startingHealth;
        //遊戲開始初始血量=血量最大值     
        
        if (currentHealth <= 0)
        //如果血量歸0,把血量變為初始血量(100)
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
                
        GetComponent<PlayMovement>().enabled = false;
        //死亡後讓角色不能移動        
        GetComponentInChildren<PlayerShooting>().enabled = false;
        //死亡後讓角色不能開槍(射擊的腳本在player底下的GunbarrelEnd)

        Restart.SetActive(true);
        Continue.SetActive(true);

        if (PlayerDeathEvent!=null)
        //沒!=null易出錯
        {
            PlayerDeathEvent();
        }

    }
    
    public void RestartLevel()
    //重新開始
    {
        Application.LoadLevel("Level1");
        AmmoManager.AmmoCurrent = AmmoManager.AmmoCapNum;
        AmmoManager.AmmoTotal = AmmoManager.AmmoTotalNum;
        ScoreManager.score = ScoreManager.ScoreNum;
    }
    
    public void ContinueLevel()
    //接關
    {
        isDeath = false;
        //playerAudio.clip = null;
        //playerAudio.Play();
        playerAnimator.SetTrigger("Live");
        playerAudio.clip = hurtClip;
        currentHealth = startingHealth;
        healthSlider.value = currentHealth;

        //GetComponent<PlayMovement>().enabled = true;
        //復活後讓角色可以移動
        //GetComponentInChildren<PlayerShooting>().enabled = true;
        //復活後讓角色可以開槍(射擊的腳本在player底下的GunbarrelEnd)


        playerAudio.clip = hurtClip;

        Restart.SetActive(false);
        Continue.SetActive(false);

        if (PlayerContinueEvent != null)
        //沒!=null易出錯
        {
            PlayerContinueEvent();
        }

    }

    public void Revival()
    {        
        GetComponent<PlayMovement>().enabled = true;
        //復活後讓角色可以移動        
        GetComponentInChildren<PlayerShooting>().enabled = true;
        //復活後讓角色可以開槍(射擊的腳本在player底下的GunbarrelEnd)
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

    //受到治療
    public void TakeHeal(int Healamount)
    {        
        if (isDeath || currentHealth == startingHealth)
        //如果player死亡or滿血
        {
            return;
        }
        
        healed = true;
        //防止補血超量
        if (startingHealth - currentHealth <= 20)
        {
            currentHealth = startingHealth;
            healthSlider.value = currentHealth;

        }
        else
        {
            currentHealth += Healamount;
            healthSlider.value = currentHealth;
        }

    }

    private void Update()
    {
        if (damaged)
        {
            damaged = false;
            //打開後馬上關閉製造閃爍效果
            damageImage.color = flashColor;
        }
        else
        {            
            damageImage.color = Color.Lerp(damageImage.color,Color.clear,Time.deltaTime*flashSpeed);
            //                        現在的數值(顏色),想要變化到的數值(clear=0000)
        }

        if (healed)
        {
            healed = false;
            HealImage.color = HealflashColor;
        }
        else
        {            
            HealImage.color = Color.Lerp(HealImage.color, Color.clear, Time.deltaTime * HealflashSpeed);
            //                        現在的數值(顏色),想要變化到的數值(clear=0000)
        }

    }
}

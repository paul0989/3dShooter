using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour {    
    public int damagePerShot = 20;
    //槍射擊的傷害(扣血量)    
    public float range = 100f;
    //槍的射程    
    public int AmmoCost = 1;
    //開槍消耗彈藥

    
    private Ray shootRay;
    private RaycastHit shootHit;
    //設定一條射線
    
    private int shootableMask;
    //能夠被射擊到的
    
    private Light gunLight;
    private ParticleSystem gunParticle;
    private AudioSource gunAudio;
    private LineRenderer gunLine;
    //開槍時的燈光,粒子,音效
    
    //public static int AmmoCurrent = 10;
    //public static int AmmoCapNum = 10;
    //public static int AmmoTotal = 900;
    public readonly float ReloadTime = 3f;
    public float nextReloadTime;
    //目前彈藥.總彈藥.重新裝填時間
    
    public bool IsReloading = false;
    //換彈藥狀態    
    public GameObject AmmoReload;
    //重新裝填文字

    public float timeBetweenBullets = 0.15f;
    //開槍頻率(週期)
    private float effectsDisplayTime = 0.1f;
    //特效顯示時間
    float timer;
    //紀錄每個frame的時間

    private void Awake()
    {        
        shootableMask = LayerMask.GetMask("enemy");
        //初始化攻擊到的圖層(enemy)        
        gunParticle = GetComponent<ParticleSystem>();
        //初始化粒子        
        gunLine = GetComponent<LineRenderer>();
        //初始化槍瞄準線        
        gunAudio = GetComponent<AudioSource>();
        //初始化開槍音效        
        gunLight = GetComponent<Light>();
        //初始化開槍光特效
    }

    private void Shoot()
    //射擊
    {
        timer = 0f;
        
        gunAudio.Play();
        gunLight.enabled = true;
        gunParticle.Stop();
        gunParticle.Play();
        //音效打開,燈光亮起,粒子系統播放(先Stop再Play),槍瞄準線

        gunLine.enabled = true;
        
        gunLine.SetPosition(0, transform.position);
        //LineRenderer的線(0=初始,槍口位置開始發射)        
        shootRay.origin = transform.position;
        //原點        
        shootRay.direction = transform.forward;
        //transform是個人座標,依照轉的方向改變,Vector3則是世界座標(不會因旋轉改變方向)        
        AmmoManager.AmmoCurrent-=AmmoCost;
        //消耗彈藥

        
        if (Physics.Raycast(shootRay, out shootHit, range, shootableMask))
        //碰撞              從槍口發出一條射線     範圍 ,能夠打到誰(enemy)
        {
            //
            EnemyHealth enemyHealth=            
            shootHit.collider.GetComponent<EnemyHealth>();
            //打到Enemy,取得EnemyHealth
            enemyHealth.TakeDamage(damagePerShot,shootHit.point);
            //槍射擊到位置的點            
            gunLine.SetPosition(1, shootHit.point);
            //槍雷射線(1=LineRenderer終點,碰撞到的點)
        }
        else
        {            
            gunLine.SetPosition(1, shootRay.origin + shootRay.direction * range);
            //如果沒射中,(1=LineRenderer終點,origin為Vector向量,兩個向量相加會得到新的向量, 雷射起點+雷射方向*雷射長度)
        }

    }
        
    void DisableEffects()
    //關閉特效
    {
        gunLine.enabled = false;
        gunLight.enabled = false;
    }

    public void AmmoReloading()
    {
        nextReloadTime = Time.time + ReloadTime;

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        //呼叫,按下滑鼠左鍵呼叫Shoot  && timer大於或等於timeBetweenBullets的時間

        if (AmmoManager.AmmoCurrent >= 1)
        {
            if (Input.GetButtonDown("Fire1") && timer >= timeBetweenBullets)
            {
                Shoot();
            }

        }

        if (AmmoManager.AmmoCurrent == 0 && IsReloading == false)
        {
            IsReloading = true;
            AmmoReload.SetActive(true);
            nextReloadTime = Time.time + ReloadTime;
            //裝填彈藥
            //AmmoReloading();

        }

        //if (AmmoManager.AmmoCurrent == 0 && Time.time >= nextReloadTime)
        if (AmmoReload.activeSelf == true && Time.time >= nextReloadTime)
        {
            IsReloading = false;
            AmmoManager.AmmoCurrent = AmmoManager.AmmoCapNum;
            AmmoManager.AmmoTotal = AmmoManager.AmmoTotal - AmmoManager.AmmoCapNum;
            AmmoReload.SetActive(false);
        }

        if (timer >= timeBetweenBullets * effectsDisplayTime)
        {
            DisableEffects();
        }

    }

}

       
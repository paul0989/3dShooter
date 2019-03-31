using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour {
    //槍射擊的傷害(扣血量)
    public int damagePerShot = 20;
    //槍的射程
    public float range = 100f;
    //設定一條射線
    private Ray shootRay;
    private RaycastHit shootHit;
    //能夠被射擊到的
    private int shootableMask;
    //開槍時的燈光,粒子,音效
    private Light gunLight;
    private ParticleSystem gunParticle;
    private AudioSource gunAudio;
    private LineRenderer gunLine;

    public float timeBetweenBullets = 0.15f;//開槍頻率(週期)
    private float effectsDisplayTime = 0.1f;//特效顯示時間
    float timer;//紀錄每個frame的時間

    private void Awake()
    {
        //初始化攻擊到的圖層(enemy)
        shootableMask = LayerMask.GetMask("enemy");
        //初始化粒子
        gunParticle = GetComponent<ParticleSystem>();
        //初始化槍瞄準線
        gunLine = GetComponent<LineRenderer>();
        //初始化開槍音效
        gunAudio = GetComponent<AudioSource>();
        //初始化開槍光特效
        gunLight = GetComponent<Light>();
    }
    private void Shoot()
    {
        timer = 0f;
        //音效打開,燈光亮起,粒子系統播放(先Stop再Play),槍瞄準線
        gunAudio.Play();
        gunLight.enabled = true;
        gunParticle.Stop();
        gunParticle.Play();

        gunLine.enabled = true;
        //LineRenderer的線(0=初始,槍口位置開始發射)
        gunLine.SetPosition(0, transform.position);
        //原點
        shootRay.origin = transform.position;
        //transform是個人座標,依照轉的方向改變,Vector3則是世界座標(不會因旋轉改變方向)
        shootRay.direction = transform.forward;

        //碰撞              從槍口發出一條射線     範圍 ,能夠打到誰(enemy)
        if (Physics.Raycast(shootRay, out shootHit, range, shootableMask))
        {
            //
            EnemyHealth enemyHealth=
            //打到Enemy,取得EnemyHealth
            shootHit.collider.GetComponent<EnemyHealth>();
            enemyHealth.TakeDamage(damagePerShot,shootHit.point);//槍射擊到位置的點
            //槍雷射線(1=LineRenderer終點,碰撞到的點)
            gunLine.SetPosition(1, shootHit.point);
        }
        else
        {
            //如果沒射中,(1=LineRenderer終點,origin為Vector向量,兩個向量相加會得到新的向量, 雷射起點+雷射方向*雷射長度)
            gunLine.SetPosition(1, shootRay.origin + shootRay.direction * range);
        }

    }
    //關閉特效
    void DisableEffects()
    {
        gunLine.enabled = false;
        gunLight.enabled = false;
    }

    // Update is called once per frame
    void Update () {
        timer += Time.deltaTime;
        //呼叫,按下滑鼠左鍵呼叫Shoot  && timer大於或等於timeBetweenBullets的時間
        if (Input.GetButtonDown("Fire1") && timer>=timeBetweenBullets)
        {
            Shoot();
        }
        if (timer >= timeBetweenBullets*effectsDisplayTime)
        {
            DisableEffects();
        }
	}
}

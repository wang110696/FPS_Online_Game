using UnityEngine;
using UnityEngine.UI;

namespace new_Scripts.Weapon
{
    public abstract class Firearms : MonoBehaviour, IWeapon
    {
        //枪口位置
        public Transform MuzzlePoint;

        //抛出弹壳位置
        public Transform CasingPoint;
        public AudioSource FirearmShootingAudioSource;
        public AudioSource FirearmReloadAudioSource;
        public FirearmsAudioData FirearmsAudioData;
        public ImpactAudioData ImpactAudioData;

        //枪焰
        public ParticleSystem MuzzleParticle;

        //抛出的弹壳,使用ParticleSystem避免太多的实例化
        public ParticleSystem CastParticle;

        // 弹夹容量
        public int AmmoInMag = 30;

        // 每把枪做多携带的子弹数
        public int MaxAmmoCarried = 120;

        //当前子弹容量和最大容量
        protected int currentAmmoInBag;
        protected int currentMaxAmmoCarried;

        //射速
        public float FireRate;

        // 上次开枪的时间
        protected float lastFireTime;

        // 开枪动画
        protected Animator gunAnimator;

        // 判断当前是否在瞄准
        protected bool isAimming = false;

        // 允不允许装弹？不能够重复装弹
        private bool permitReload = false;

        // 扣动扳机的方法
        protected abstract void Shooting();

        // 换子弹
        protected abstract void Reload();

        // 瞄准
        protected abstract void Aim(bool isAimming);

        //换子弹动画状态
        protected AnimatorStateInfo animatorStateInfo;

        //子弹的prefab
        public GameObject BulletPrefab;

        //显式子弹数目
        public Text AmmoCountTextLable;
        public Text AmmoInBagTextLable;

        protected void updateAmmoInfo()
        {
            // AmmoCountTextLable.text =  "弹夹子弹数:" + currentAmmoInBag.ToString();
            // AmmoInBagTextLable.text =  "背包子弹数:" + currentMaxAmmoCarried.ToString();
        }

        protected virtual void Start()
        {
            currentAmmoInBag = AmmoInMag;
            currentMaxAmmoCarried = MaxAmmoCarried;
            gunAnimator = GetComponent<Animator>();
            // AmmoCountTextLable.text =  "弹夹子弹数:" + currentAmmoInBag.ToString();
            // AmmoInBagTextLable.text =  "背包子弹数:" + currentMaxAmmoCarried.ToString();
            
        }

        public void DoAttack()
        {
            Shooting();
        }

        // 再写个方法控制开枪间隔
        protected bool isAllowShooting()
        {
            // AK射速是每分钟715发  也就是每秒11.7发
            // 也就是说每颗子弹的间隔是1/11.7 下面就判断是否两颗子弹中间间隔时间大于1/11.7，小于则不允许开枪
            return Time.time - lastFireTime > 1 / FireRate;
        }

        //射出子弹
        protected void createBullet()
        {
            //缓存一下子弹
            GameObject tmp_Bullet = Instantiate(BulletPrefab, MuzzlePoint.position, MuzzlePoint.rotation);
            tmp_Bullet.transform.rotation = Quaternion.Euler(tmp_Bullet.transform.localEulerAngles.x + 90f,
                tmp_Bullet.transform.localEulerAngles.y, tmp_Bullet.transform.localEulerAngles.z);
            Destroy(tmp_Bullet, 2);
            // var tmp_BulletRigibody = tmp_Bullet.AddComponent<Rigidbody>();
            var tmp_BulletScript = tmp_Bullet.AddComponent<Bullet>();
            // tmp_BulletScript.ImpactAudioData = ImpactAudioData;
            tmp_BulletScript.BulletSpeed = 270f;
        }
    }
}
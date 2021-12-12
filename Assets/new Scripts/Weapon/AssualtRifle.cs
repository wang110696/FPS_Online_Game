using System.Collections;
using UnityEngine;

namespace new_Scripts.Weapon
{
    public class AssualtRifle : Firearms
    {
        private bool reloading = false;
        public MultiplayerWeapon MultiplayerWeapon;

        public FPMouseRotationControlScript fpMouseRotationControlScript;

        protected override void Start()
        {
            base.Start();
            // fpMouseRotationControlScript = FindObjectOfType<FPMouseRotationControlScript>();
        }

        protected override void Shooting()
        {
            if (currentAmmoInBag <= 0)
            {
                return;
            }

            if (!isAllowShooting())
            {
                return;
            }

            //判断一下是瞄准射击还是腰射
            AudioClip audioClip = FirearmsAudioData.ShootingAudioClip;
            FirearmShootingAudioSource.clip = audioClip;
            FirearmReloadAudioSource.Play();
            if (!isAimming)
            {
                gunAnimator.Play("Fire", 0, 0);
            }
            else
            {
                gunAnimator.Play("Aim Fire", 0, 0);
            }

            // gunAnimator.SetBool("permitReload", true);
            MuzzleParticle.Play();
            CastParticle.Play();
            currentAmmoInBag--;
            if (currentAmmoInBag == 0)
            {
                MuzzleParticle.Stop();
                CastParticle.Stop();
            }
            //用RPC，使第三人称播放动画
            MultiplayerWeapon.playFireEffect();

            //后坐力
            fpMouseRotationControlScript.FiringForTest();
            // fpMouseRotationControlScript.currentRecoilTime = 0;
            createBullet();
            lastFireTime = Time.time;

            updateAmmoInfo();
        }

        protected override void Reload()
        {
            if (currentMaxAmmoCarried != 0)
                //没子弹了就不允许装弹了
            {
                FirearmReloadAudioSource.clip = FirearmsAudioData.ReloadOutOf;
                FirearmReloadAudioSource.Play();
                gunAnimator.SetLayerWeight(1, 1);
                gunAnimator.SetTrigger("ReloadOutOf");
                MultiplayerWeapon.PlayReloadEffect();
                StartCoroutine(CheckReloadAmmoAnimatonEnd());
                
            }
        }

        /**
         * 瞄准
         * <param name="aimming"></param>
         */
        protected override void Aim(bool isAimming)
        {
            if (isAimming)
            {
                FirearmReloadAudioSource.clip = FirearmsAudioData.AimIn;
                FirearmReloadAudioSource.Play();
                //开始瞄准
                gunAnimator.Play("Aim In", 0, 0);
            }
        }

        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                if (!reloading)
                {
                    DoAttack();
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                MuzzleParticle.Stop();
                CastParticle.Stop();
            }

            //右键瞄准
            if (Input.GetMouseButtonDown(1))
            {
                isAimming = !isAimming;
                gunAnimator.SetBool("isAimming", isAimming);
                Aim(isAimming);
            }


            if (Input.GetKeyDown(KeyCode.R))
            {
                if (currentAmmoInBag < AmmoInMag)
                {
                    Reload();
                }
            }
        }

        private IEnumerator CheckReloadAmmoAnimatonEnd()
        {
            while (true)
            {
                yield return null;
                animatorStateInfo = gunAnimator.GetCurrentAnimatorStateInfo(1);
                if (animatorStateInfo.IsTag("ReloadAmmo"))
                {
                    reloading = true;
                    if (animatorStateInfo.normalizedTime > 0.91f)
                    {
                        reloading = false;
                        int need = AmmoInMag - currentAmmoInBag; // 比如剩余10发，只用补充20发
                        if (currentMaxAmmoCarried >= need)
                        {
                            currentAmmoInBag = AmmoInMag;
                            currentMaxAmmoCarried -= need;
                        }
                        else
                        {
                            //不够子弹补充了，只能有多少补多少了
                            currentAmmoInBag += currentMaxAmmoCarried;
                            currentMaxAmmoCarried = 0;
                        }
                        updateAmmoInfo();
                        yield break;
                    }
                }
            }
        }
    }
}
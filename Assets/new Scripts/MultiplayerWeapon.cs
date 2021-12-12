using System;
using System.Collections;
using System.Collections.Generic;
using new_Scripts.Weapon;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Animations.Rigging;

/// <summary>
/// 控制多人游戏下的枪口火焰特效的同步
/// </summary>
[RequireComponent(typeof(PhotonView))]
public class MultiplayerWeapon : Firearms
{
    public Animator gunAnimator_tp;
    private PhotonView photonView;
    private float currentTime = 0;
    public TwoBoneIKConstraint TwoBoneIKConstraint;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    public void playFireEffect()
    {
        photonView.RpcSecure("RPC_PlayFireEffect", RpcTarget.All, false, null);
    }    
    
    public void PlayReloadEffect()
    {
        photonView.RpcSecure("RPC_PlayReloadEffect", RpcTarget.All, false, null);
    }

    [PunRPC]
    public void RPC_PlayFireEffect()
    {
        if (photonView.IsMine)
        {
            return;
        }
        AudioClip audioClip = FirearmsAudioData.ShootingAudioClip;
        Debug.Log("FirearmShootingAudioSource:"+FirearmShootingAudioSource.name);
        FirearmShootingAudioSource.clip = audioClip;
        FirearmReloadAudioSource.Play();
        
        //播放动画
        TwoBoneIKConstraint.weight = 1f;
        gunAnimator_tp.Play("Fire", 0, 0);
        MuzzleParticle.Play();
        CastParticle.Play();
        currentAmmoInBag--;
        if (currentAmmoInBag == 0)
        {
            MuzzleParticle.Stop();
            CastParticle.Stop();
        }
    }
    
    [PunRPC]
    public void RPC_PlayReloadEffect()
    {
        if (photonView.IsMine)
        {
            return;
        }
        gunAnimator_tp.SetLayerWeight(1, 1);
        gunAnimator_tp.SetTrigger("ReloadOutOf");
    }
    


    protected override void Shooting()
    {
    }

    protected override void Reload()
    {
    }

    protected override void Aim(bool isAimming)
    {
    }
}
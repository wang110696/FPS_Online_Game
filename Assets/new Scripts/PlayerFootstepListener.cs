using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 控制脚步声音播放
/// </summary>
public class PlayerFootstepListener : MonoBehaviour
{
    public FootstepAudioData FootstepAudioData;
    public AudioSource FootstepAudioSource;

    private CharacterController characterController;
    private Transform footstepTransform;
    private float nextPlayTime;
    public LayerMask LayerMask;

    //下面来写一下检测角色什么时候才会发出声音的条件
    //1.角色发出声音的必备条件：角色移动或者发生较大幅度动作时发出声音
    //2.如何检测角色是否移动：使用Physic API
    //3.同样用Physic API 检测踩中的材质
    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        footstepTransform = transform;
    }

    private void FixedUpdate()
    {
        if (characterController.isGrounded)
        {
            if (characterController.velocity.magnitude > 0 && characterController.velocity.magnitude < 10.5f)
            {
                nextPlayTime += Time.deltaTime;
            }

            if (characterController.velocity.magnitude > 13f)
            {
                nextPlayTime += Time.deltaTime * 1.5f;
            }

            if (characterController.velocity.normalized.magnitude >= 0.1f)
            {
                // 说明角色在移动，需要播放声音,下面这个检测其实是要考虑characterController的skin的
                bool isHit = Physics.Linecast(footstepTransform.position,
                    footstepTransform.position + (Vector3.down * (characterController.height - 0.32f)),
                    out RaycastHit tmp_HitInfo, LayerMask);
                if (isHit)
                {
                    foreach (var tmp_AudioElement in FootstepAudioData.FootstepAudios)
                    {
                        // 检测一下路面类型
                        if (tmp_HitInfo.collider.CompareTag(tmp_AudioElement.Tag))
                        {
                            var tmp_velocity = Vector3.one;
                            tmp_velocity = characterController.velocity;
                            tmp_velocity.y = 0;
                            //调整声音间隔
                            if (nextPlayTime >= tmp_AudioElement.Delay)
                            {
                                //播放声音
                                AudioClip footstepAudioClip = null;
                                if (tmp_velocity.magnitude > 0 && tmp_velocity.magnitude < 10.5f)
                                {
                                    footstepAudioClip = tmp_AudioElement.AudioClips[0];
                                }

                                if (tmp_velocity.magnitude > 13f)
                                {
                                    footstepAudioClip = tmp_AudioElement.AudioClips[1];
                                }

                                FootstepAudioSource.clip = footstepAudioClip;
                                FootstepAudioSource.volume = 0.3f;
                                FootstepAudioSource.Play();
                                nextPlayTime = 0;
                                break;
                            }

                            if (tmp_velocity.magnitude < 1f)
                            {
                                FootstepAudioSource.Stop();
                            }
                        }
                    }
                }
            }
        }
    }
}
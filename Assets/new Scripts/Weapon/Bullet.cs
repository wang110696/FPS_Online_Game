using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using EventCode = Assets.new_Scripts.EventCode;

/// <summary>
/// 实现子弹特效音效
/// </summary>
namespace new_Scripts.Weapon
{
    public class Bullet : MonoBehaviour
    {
        public float BulletSpeed;

        // private Rigidbody bulletRigidbody;
        private Transform bulletTrasform;
        private Vector3 prePosition;
        // public ImpactAudioData ImpactAudioData;
        // public GameObject ImpactPrefab;

        private void Start()
        {
            // bulletRigidbody = GetComponent<Rigidbody>();
            bulletTrasform = transform;
            prePosition = bulletTrasform.position;
            // ImpactPrefab = Resources.Load<GameObject>("Prefabs/Metal_Impact_Prefab");
        }

        private void Update()
        {
            prePosition = bulletTrasform.position;
            bulletTrasform.Translate(0, BulletSpeed * Time.deltaTime, 0);
            //子弹运动效果
            // bulletRigidbody.velocity = bulletTrasform.up * BulletSpeed;
            if (!Physics.Raycast(prePosition, (bulletTrasform.position - prePosition).normalized,
                out RaycastHit hit, (bulletTrasform.position - prePosition).magnitude))
            {
                //如果没有碰撞到
                return;
            }
            //扣血的碰撞信息
            if (hit.collider.TryGetComponent(out IDamager iDamager))
            {
                Debug.Log("hit player!");
                iDamager.TakeDamage(25);
                
            }

            //这里用Photon的API 
            //byte eventCode,发送事件类型（比如说本地子弹打中地板，触发hit事件，本地发送事件到服务器，服务器转发到别人客户端，别人客户端会解析事件）
            //object eventContent,需要同步到各个客户端的数据,用字典会有不错的效果
            //RaiseEventOptions raiseEventOptions,配置，这里配置的是接受者为全部人
            //SendOptions sendOptions,发送设置，这里设置为可信
            //参数2
            Dictionary<byte, object> tmp_HitData = new Dictionary<byte, object>();
            tmp_HitData.Add(0,hit.point);//记录击中的点
            tmp_HitData.Add(1,hit.normal);//记录击中的点和射击点切线的法线
            tmp_HitData.Add(2,hit.collider.tag);//记录击中的材质类型
            
            //参数3
            RaiseEventOptions tmp_RaiseEventOptions = new RaiseEventOptions() {Receivers = ReceiverGroup.All};
            //参数4
            SendOptions sendOptions = SendOptions.SendReliable;

            PhotonNetwork.RaiseEvent((byte) EventCode.HitGround, tmp_HitData, tmp_RaiseEventOptions, sendOptions);
            // 子弹碰撞后0.001秒销毁
            Destroy(gameObject,0.01f);
            
            // AudioClip audioClip = ImpactAudioData.ImpactAudioClip;
            // AudioSource.PlayClipAtPoint(audioClip, hit.point);
            // var bulletEffect =
            //     Instantiate(ImpactPrefab, hit.point, Quaternion.LookRotation(hit.normal, Vector3.up));
            // Destroy(bulletEffect, 3);
        }
    }
}
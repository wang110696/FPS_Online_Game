using System.Collections.Generic;
using ExitGames.Client.Photon;
using new_Scripts.Weapon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

//这个类用于管理撞到的信息
namespace Assets.new_Scripts
{
    public class HitManager : MonoBehaviour, IOnEventCallback
    {
        public ImpactAudioData ImpactAudioData;
        public GameObject ImpactPrefab;

        //需要先注册，回调才能生效(当脚本生效)
        private void OnEnable()
        {
            PhotonNetwork.AddCallbackTarget(this);
        }

        //当脚本失效（退出房间的时候），取消这个回调注册
        private void OnDisable()
        {
            PhotonNetwork.RemoveCallbackTarget(this);
        }

        //下面进行监听
        public void OnEvent(EventData photonEvent)
        {
            //先遍历EvnetData的code看看是什么样的事件
            switch ((EventCode) photonEvent.Code)
            {
                case EventCode.HitGround:
                    //如果是hitGround这个事件
                    //下面开始从第二个参数，也就是那个diction中取值，击中位置，法线，tag
                    var tmp_HitData = (Dictionary<byte, object>) photonEvent.CustomData;
                    //先处理hit position
                    var tmp_HitPosition = (Vector3) tmp_HitData[0];
                    var tmp_HitNormal = (Vector3) tmp_HitData[1];
                    var tmp_HitTag = (string) tmp_HitData[2];

                    AudioClip audioClip = ImpactAudioData.ImpactAudioClip;
                    AudioSource.PlayClipAtPoint(audioClip, tmp_HitPosition);
                    var bulletEffect =
                        Instantiate(ImpactPrefab, tmp_HitPosition, Quaternion.LookRotation(tmp_HitNormal, Vector3.up));
                    Destroy(bulletEffect, 3);

                    break;
            }
        }
    }
}
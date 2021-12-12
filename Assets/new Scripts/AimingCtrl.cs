using System;
using Photon.Pun;
using UnityEngine;

/// <summary>
/// 第三人称的瞄准控制
/// </summary>
[RequireComponent(typeof(PhotonView))]
public class AimingCtrl : MonoBehaviour, IPunObservable
{
    public Transform Arms;
    private Vector3 localPosition;
    private Quaternion localRotation;
    public Transform AimTarget;
    private PhotonView photonView;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
        localPosition = AimTarget.position;
    }

    private void Update()
    {
        //在别人的客户端中，不需要根据别人的鼠标位置调整自己客户端的瞄准方向
        if (photonView.IsMine)
        {
            //获取第一人称的Arms的朝向
            localRotation = Arms.localRotation;
            //计算出全局坐标下AimTarget应该处于的位置
            localPosition = localRotation * Vector3.forward * 5f;
        }
        //这样接收使得第三人称开上去比较顺滑
        AimTarget.localPosition = Vector3.Lerp(AimTarget.localPosition, localPosition, Time.deltaTime * 20);
    }

    /**
     * 这是为了让别人的客户端也同步自己客户端的第三人称枪口瞄准
     */
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            //发送数据   
            stream.SendNext(localPosition);
        }
        else
        {
            //接受数据
            localPosition = (Vector3) stream.ReceiveNext();
        }
    }
}
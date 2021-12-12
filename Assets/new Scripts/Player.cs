using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

/// <summary>
/// 计算角色血量 ， RPC的前提是这个游戏对象同时挂在了PhotonView组件
/// </summary>
[RequireComponent(typeof(PhotonView))]
public class Player : MonoBehaviour, IDamager
{
    private PhotonView photonView;

    public int Heath = 100;

    private GameObject globalCamera;

    // 角色死亡后的重新部署 float是用来等待多少秒
    public static event Action<float> Respawn;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
        if (photonView.IsMine)
        {
            globalCamera = GameObject.FindWithTag("Global Camera");
            if (globalCamera)
                globalCamera.SetActive(false);
        }
        
    }

    private void Update()
    {
        if (this.transform.position.y <=-100)
        {
            PhotonNetwork.Destroy(this.gameObject);
            if (globalCamera)
                globalCamera.SetActive(true);

            Respawn?.Invoke(3);
        }
    }

    public void TakeDamage(int damage)
    {
        //本地来调用rpc，rpc来具体执行扣血
        //方法名，呼叫的对象,是否加密，方法参数（可以直接写damage）
        photonView.RpcSecure("RPC_TakeDamage", RpcTarget.All, true, damage);
    }

    // 下面用photon自带的RPC的方法(方法名一定要以RPC开头)
    [PunRPC]
    private void RPC_TakeDamage(int damage, PhotonMessageInfo info)
    {
        Heath -= damage;
        if (isDeath() && photonView.IsMine)
        {
            //gameObject.SetActive(false);
            PhotonNetwork.Destroy(this.gameObject);
            if (globalCamera)
                globalCamera.SetActive(true);

            Respawn?.Invoke(3);

            return;
        }
        
        // Heath -= damage;
        // if (isDeath())
        // {
        //     // Destroy(gameObject); 
        //     // globaCamera.enabled = true;
        //     //下面这句只能在本地客户端使用
        //     PhotonNetwork.Destroy(gameObject);
        //     Destroy(gameObject);
        //     //下面这个是委托，在Luanch的地方进行调用
        //     Respawn?.Invoke(3);
        // }
    }

    private bool isDeath()
    {
        return Heath <= 0;
    }
}
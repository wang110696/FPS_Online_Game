using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// 
/// </summary>
public class LocalManager : MonoBehaviour
{
    public List<MonoBehaviour> localScripts;
    private PhotonView PhotonView;
    public Camera mainCamera;
    public GameObject Arm;
    public List<Renderer> TpRenderers;

    private void Start()
    {
        PhotonView = GetComponent<PhotonView>();
        if (PhotonView.IsMine)
        {
            gameObject.AddComponent<AudioListener>();
            //锁定自己的鼠标
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            return;
        }

        //如果是其他人的，那么将他的FP的Arms去掉
        Arm.SetActive(false);
        //把别人的camera给隐藏掉
        mainCamera.enabled = false;
        //禁止本地的脚本对别人进行控制
        foreach (MonoBehaviour behaviour in localScripts)
        {
            behaviour.enabled = false;
        }

        //渲染别人的全身
        foreach (Renderer tpRenderer in TpRenderers)
        {
            tpRenderer.shadowCastingMode = ShadowCastingMode.On;
        }
    }
}
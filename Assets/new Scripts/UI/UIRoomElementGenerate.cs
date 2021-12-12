using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class UIRoomElementGenerate : MonoBehaviour
{
    public GameObject UIRoomElementPrefab;
    public Transform RoomRoot;
    public UIRoomListManager UIRoomListManager;
    private bool isDisplaying;

    //下面这个字典是为了防止重复实例化UIElement
    public Dictionary<string, GameObject> RoomUIElementDictionary = new Dictionary<string, GameObject>();

    //生成ui
    public void generateRoomElementUI(RoomInfo roomInfo)
    {
        //首先生成UI实例
        if (RoomUIElementDictionary.ContainsKey(roomInfo.Name)) return;
        var tmp_ui = Instantiate(UIRoomElementPrefab, RoomRoot);
        RoomUIElementDictionary.Add(roomInfo.Name, tmp_ui);
        var transform = tmp_ui.transform;
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one;
        transform.localRotation = Quaternion.identity;
        //获取UI实例身上的UIRoomElement组件，并将roomInfo传进去，这样UI就有具体房间信息了
        var tmp_uiScript = tmp_ui.GetComponent<UIRoomElement>();
        tmp_uiScript.setElementDetail(roomInfo);
        //之后去到UIManager组件中，因为左侧栏需要显示该房间的具体信息（地图之类的）
        tmp_uiScript.OnSelectedRoom = FindObjectOfType<UIManager>().setSelectedRoomDetail;
    }

    //删除UI
    public void RemoveRoomElementUI(RoomInfo roomInfo)
    {
        if (RoomUIElementDictionary.TryGetValue(roomInfo.Name, out GameObject gameObject))
        {
            //如果有则删除UI和dic中的键值对
            Destroy(gameObject);
            RoomUIElementDictionary.Remove(roomInfo.Name);
        }
    }


    //用于animation的调用
    public void StartGenerateRoomUI()
    {
        isDisplaying = true;
        foreach (KeyValuePair<string, RoomInfo> roomInfo in UIRoomListManager.roomList)
        {
            generateRoomElementUI(roomInfo.Value);
        }
    }

    //同样给Animation调用，用于修改isDisplaying状态值为false
    public void RoomGenerateHiding()
    {
        isDisplaying = false;
        foreach (KeyValuePair<string, GameObject> ui in RoomUIElementDictionary)
        {
            //每次hide起来之后，删除所有的UI和清空字典
            Destroy(ui.Value);
        }

        RoomUIElementDictionary.Clear();
    }

    private void OnEnable()
    {
        UIRoomListManager.OnRoomRemoved += onRoomRemoved;
        UIRoomListManager.OnRoomAdded -= onRoomAdded;
    }


    private void onRoomAdded(RoomInfo info)
    {
        //只有在显式的时候才进行generateUI
        //不然游戏创建大量房间实时更新的话会造成卡顿，如果打开在更新则不会卡顿！
        if (!isDisplaying) return;
        generateRoomElementUI(info);
    }

    private void onRoomRemoved(RoomInfo info)
    {
        if (!isDisplaying) return;
        RemoveRoomElementUI(info);
    }
}
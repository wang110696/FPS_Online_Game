using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

/// <summary>
/// 需要继承MonoBehaviourPunCallbacks才会获得pun的回调
/// </summary>
public class UIRoomListManager : MonoBehaviourPunCallbacks
{
    //接收者一定要加入lobby大厅之后才能接收到这个回调通知
    //下面这个回调能够拿到房间列表！但是不能每次都从下面的for中取，需要整一个字典来存,string 存房间名
    private Dictionary<string, RoomInfo> roomInfosDictionary = new Dictionary<string, RoomInfo>();
    public Dictionary<string, RoomInfo> roomList => roomInfosDictionary;

    //事件委托,委托的执行处在roomgenerate
    public event Action<RoomInfo> OnRoomRemoved;
    public event Action<RoomInfo> OnRoomAdded;

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);
        Debug.Log("加入lobby之后的回调");
        foreach (RoomInfo roomInfo in roomList)
        {
            //如果移除房间，会回调
            if (roomInfo.RemovedFromList)
            {
                if (roomInfosDictionary.ContainsKey(roomInfo.Name))
                {
                    roomInfosDictionary.Remove(roomInfo.Name);
                    //这边要来一个事件委托，去删除已不存在的房间
                    OnRoomRemoved?.Invoke(roomInfo);
                }

                // 移除回调就到这里返回就可以了，不用再执行下面的
                return;
            }

            //这里循环用来将所有的房间存到dic中
            if (!roomInfosDictionary.ContainsKey(roomInfo.Name))
            {
                roomInfosDictionary.Add(roomInfo.Name, roomInfo);
                OnRoomAdded?.Invoke(roomInfo);
            }
        }
    }
}
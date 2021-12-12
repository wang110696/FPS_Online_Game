using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// IPointerClickHandler 点击事件
/// </summary>
public class UIRoomElement : MonoBehaviour, IPointerClickHandler
{
    public Image PwdIcon;
    public Text RoomName;
    public Text PlayerCount;

    public Action<RoomInfo> OnSelectedRoom;
    private RoomInfo roomInfo;

    public void setElementDetail(RoomInfo roomInfo)
    {
        this.roomInfo = roomInfo;
        RoomName.text = roomInfo.Name;
        PlayerCount.text = $"{roomInfo.PlayerCount}/{roomInfo.MaxPlayers}";
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnSelectedRoom?.Invoke(roomInfo);
    }
}
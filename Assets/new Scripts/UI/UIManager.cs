using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

/// <summary>
/// UI管理设置
/// </summary>
public class UIManager : MonoBehaviour
{
    public CanvasGroup MenuItemCanvasGroup;
    public Button PlayButton;
    [Space] public Button CreateGameButton;
    public Button CancelCreateGameButton;
    public Button ConfirmCreateGameButton;

    public InputField RoomNameInputField;
    public InputField RoomPasswordInputField;
    public Dropdown PlayerCountDropdown;
    public string mapName;


    [Space] public Button JoinGameButton;
    public Button CancelJoinGameButton;
    public Button ConfirmJoinGameButton;
    [Space] public Button SettingButton;
    [Space] public Animator CreateGamePanelAnimator;
    public Animator JoinGamePanelAnimator;
    public Image SelectedMapImage;
    public Text SelectedRoomName;
    [Space] public Launcher Launcher;

    //这里有很多个地图
    public MapModelsScriptableObject ModelsScriptableObject;


    private void Start()
    {
        //Create Game 监听
        CreateGameButton.onClick.AddListener(() =>
        {
            CreateGamePanelAnimator.SetTrigger("FadeIn");
            MenuItemCanvasGroup.interactable = false;
        });
        CancelCreateGameButton.onClick.AddListener(() =>
        {
            CreateGamePanelAnimator.SetTrigger("FadeOut");
            MenuItemCanvasGroup.interactable = true;
        });
        ConfirmCreateGameButton.onClick.AddListener(() =>
        {
            //创建房间
            Launcher.createRoom(RoomNameInputField.text,
                (byte) PlayerCountDropdown.value,
                RoomPasswordInputField.text,
                mapName);
        });


        //Join Game 监听
        JoinGameButton.onClick.AddListener(() =>
        {
            JoinGamePanelAnimator.SetTrigger("FadeIn");
            MenuItemCanvasGroup.interactable = false;
        });
        CancelJoinGameButton.onClick.AddListener(() =>
        {
            JoinGamePanelAnimator.SetTrigger("FadeOut");
            MenuItemCanvasGroup.interactable = true;
            //把已经加载在MapDetai上的信息进行清空
            SelectedRoomName.text = null;
            SelectedMapImage.sprite = null;
        });
        
        ConfirmJoinGameButton.onClick.AddListener(() =>
        {
            if (SelectedRoomName.text == null)
            {
                return;
            }
            PhotonNetwork.JoinRoom(SelectedRoomName.text);
        });
    }

    public void setSelectedRoomDetail(RoomInfo roomInfo)
    {
        //下面从ModelsScriptableObject脚本中的MapModels这个地图List中寻找和roomInfo匹配的地图
        // Assert.AreNotEqual(roomInfo.CustomProperties.Count,0);
        //从List中的Model的key（也就是MapName）去匹配roomInfo中的mapName，并返回
        var model = ModelsScriptableObject.MapModels.Find(model1 =>
        {
            return model1.MapName.CompareTo(roomInfo.CustomProperties["mapName"]) == 0;
        });
        //将这个modelset到左侧的mapDetail栏中
        SelectedRoomName.text = roomInfo.Name;
        SelectedMapImage.sprite = model.MapSprite;
    }
}
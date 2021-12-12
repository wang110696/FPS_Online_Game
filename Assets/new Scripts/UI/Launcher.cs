using System;
using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Begin Game UI控制监本
/// </summary>
public class Launcher : MonoBehaviourPunCallbacks
{
    public InputField RoomName;

    //连接成功回调
    private bool isConnectedToMasterServer;

    //上线按钮
    public Button CreateRoomButton;

    public Button JoinInRoomButton;

    //创建房间成功回调
    private bool isCreatedRoom;

    //加入房间成功回调
    private bool isJoinedRoom;

    /**
     * 现在做成游戏点开自动上线
     */
    private void Awake()
    {
        //Awake和Start的区别在于，Awake没有激活脚本也会调用，Start的话只有激活了脚本才会执行
        connectToMasterServer();
    }


    /**
     * 连接到服务器
     */
    public void connectToMasterServer()
    {
        //使用unity上的设置进行连接Master服务器
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.GameVersion = "Alpha";
        Debug.Log("上线中..");
        // CreateRoomButton.interactable = true;
        // JoinInRoomButton.interactable = true;
        // RoomName.interactable = true;
    }


    /**
     * 连接之后才能进行创建房间
     * 当创建房间成功后，会自动加入创建好的房间，
     * 所以就无法点击“创建房间”和”加入房间“的按钮
     */
    public void createRoom(string roomName, byte playerCount, string password, string mapName)
    {
        if (!isConnectedToMasterServer || isCreatedRoom)
        {
            return;
        }

        // 房间信息，地图，密码等,注意这个不是system的hashtable，而是photon的
        ExitGames.Client.Photon.Hashtable tmp_RoomProperties = new ExitGames.Client.Photon.Hashtable();
        tmp_RoomProperties.Add("psw", password);
        tmp_RoomProperties.Add("mapName", mapName);
        // 创建房间
        //下面这个只能是加入房间后才能用的属性
        //下面这个是可以再大厅就可以用的，是一个String的数组，将上面CustomRoomProperties的Key填入，才能在大厅使用上面的K-V
        PhotonNetwork.CreateRoom(roomName,
            new RoomOptions()
            {
                MaxPlayers = playerCount,
                PublishUserId = true,
                CustomRoomProperties = tmp_RoomProperties,
                CustomRoomPropertiesForLobby = new[] {"mapName"}
            },
            TypedLobby.Default);
    }


    /**
     * 连接之后才能进行加入房间
     */
    public void joinRoom()
    {
        if (!isConnectedToMasterServer || isCreatedRoom)
        {
            return;
        }

        PhotonNetwork.JoinRoom(RoomName.text);
    }

    /**
     * 连接回调
     */
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        isConnectedToMasterServer = true;
        PhotonNetwork.JoinLobby(); //加入大厅
    }

    /**
     * 加入大厅
     */
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        Debug.Log("join lobby");
    }

    /**
     * 创建房间回调
     */
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        isCreatedRoom = true;
    }

    /**
     * 加入房间成功回调
     */
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        isJoinedRoom = true;
        // 用下面这个来加载场景,需要放到build setting中~
        PhotonNetwork.LoadLevel(PhotonNetwork.CurrentRoom.CustomProperties["mapName"].ToString());
        // StartSpawn(0);
        // Player.Respawn += StartSpawn;
    }

    //在离开房间的时候将事件移除掉
    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
    }
}
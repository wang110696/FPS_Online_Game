using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// 
/// </summary>
public class BattleRoomManager : MonoBehaviour
{
    public string PlayerPrefabName;
    public Transform RespawnPosition_1;
    public Transform RespawnPosition_2;
    public Transform RespawnPosition_3;
    public Transform RespawnPosition_4;
    public Transform RespawnPosition_5;
    public Transform RespawnPosition_6;
    private List<Transform> locationList = new List<Transform>();

    private void Start()
    {
        StartSpawn(0.5f);
        Player.Respawn += StartSpawn;
        locationList.Add(RespawnPosition_1);
        locationList.Add(RespawnPosition_2);
        locationList.Add(RespawnPosition_3);
        locationList.Add(RespawnPosition_4);
        locationList.Add(RespawnPosition_5);
        locationList.Add(RespawnPosition_6);
    }

    private void OnDisable()
    {
        //离开的时候减掉这个事件委托，避免重叠
        Player.Respawn -= StartSpawn;
    }


    private void StartSpawn(float timeToSpawn)
    {
        StartCoroutine(WaitToInstantiatePlayer(timeToSpawn));
    }


    private IEnumerator WaitToInstantiatePlayer(float timeToSpawn)
    {
        yield return new WaitForSeconds(timeToSpawn);
        //随机生成一个复活点
        int index = Random.Range(0, locationList.Count);
        PhotonNetwork.Instantiate(PlayerPrefabName, locationList[index].position, Quaternion.identity);
    }
}

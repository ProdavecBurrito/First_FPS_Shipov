using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public static NetworkManager instance = null;

    int enemyCounter = 0;
    int maxTotalEnemy = 5;
    GameObject player;

    IEnumerator SpawnEnemy(int count)
    {
        while (enemyCounter < count)
        {
            yield return new WaitForSeconds(Random.Range(5, 15));
            Create("Preffabs/ Enemy");
            enemyCounter++;
        }
    }

    void Create(string prefab)
    {
        float x = Random.Range(-25, 25);
        float z = Random.Range(-25, -25);
        Vector3 vector = new Vector3(x, 6f, z);
        GameObject temp = PhotonNetwork.Instantiate(prefab, vector, Quaternion.identity);
    }

    void Awake()
    {
        instance = this;
        StartGame();
    }

    void StartGame()
    {
        if(!player)
        {
            Create("Preffabs/Player");
        }
        if (PhotonNetwork.IsMasterClient && PhotonNetwork.IsConnected)
        {
            StartCoroutine(SpawnEnemy(maxTotalEnemy));
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        PhotonNetwork.LoadLevel(0);
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        PhotonNetwork.Disconnect();
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        base.OnMasterClientSwitched(newMasterClient);
        if (PhotonNetwork.LocalPlayer.ActorNumber == newMasterClient.ActorNumber)
        {
            StartCoroutine(SpawnEnemy(maxTotalEnemy - enemyCounter));
        }
    }
}

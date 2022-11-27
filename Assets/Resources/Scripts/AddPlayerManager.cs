using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class AddPlayerManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    GameObject playerPrefab;

    [SerializeField]
    GameObject startingSpace;

    [SerializeField]
    GameObject ScoreDisplay;

    [SerializeField]
    GameObject StateManager;

    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            if (playerPrefab != null)
            {
                int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;

                int randomPoint = Random.Range(0, 1);

                PhotonNetwork
                    .Instantiate(playerPrefab.name,
                    new Vector3(playerCount-1 , 0, playerCount-1),
                    Quaternion.identity);

                Debug.Log("Player added");

                // add display for joining player
                // ScoreDisplay.GetComponent<PhotonView>().RPC("addPlayer", RpcTarget.AllBuffered);

                // add players to state manager
                // StateManager.GetComponent<PhotonView>().RPC("addPlayer2", RpcTarget.AllBuffered);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public override void OnJoinedRoom()
    {
        Debug
            .Log(PhotonNetwork.NickName +
            " joined to " +
            PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug
            .Log(PhotonNetwork.NickName +
            " joined to " +
            PhotonNetwork.CurrentRoom.Name +
            " " +
            PhotonNetwork.CurrentRoom.PlayerCount);
    }
}

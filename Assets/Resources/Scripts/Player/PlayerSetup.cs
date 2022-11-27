using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class PlayerSetup : MonoBehaviourPunCallbacks
{
    [SerializeField]
    TextMeshProUGUI playerNameText;

    // Start is called before the first frame update
    void Start()
    {
        if (photonView.IsMine)
        {
        }

        SetPlayerUI();
    }

    void SetPlayerUI()
    {
        if (playerNameText != null)
        {
            playerNameText.text = photonView.Owner.NickName;
        }
    }
}

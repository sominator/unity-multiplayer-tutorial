using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ButtonClick : NetworkBehaviour
{
    public PlayerManager PlayerManager;

    public void OnClick()
    {
        PlayerManager = GameObject.Find("PlayerManager(Clone)").GetComponent<PlayerManager>();
        PlayerManager.DealCards();
    }
}

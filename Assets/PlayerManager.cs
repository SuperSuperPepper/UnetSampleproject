using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Manage player list
/// </summary>
[NetworkSettings(channel = 0, sendInterval = 2f)]
public class PlayerManager : NetworkBehaviour
{

    private static PlayerManager instance;
    public static PlayerManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerManager>();
            }
            return instance;
        }
    }

    public PlayerRemoter GetLocalPlayer()
    {
        foreach (PlayerRemoter player in FindObjectsOfType<PlayerRemoter>())
        {
            if (player.isLocalPlayer)
            {
                //if (player.isServer == false) {
                    return player;
                //}
           
            }
        }

        throw new Exception("Can't find local player");
    }

}
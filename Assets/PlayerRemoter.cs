using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

[NetworkSettings(sendInterval = 1, channel = 0)]
public class PlayerRemoter : NetworkBehaviour
{

    [SyncVar]
    public NetworkInstanceId spawnedCharacterID;
    public GameObject playerPrefab;
    public Vector3 playerSpawnPos;


    private bool isInit = false;
    private float nextUpdate = 0;
    [SyncVar]
    public short ping = 999;
    [SyncVar]
    public string displayName = "unnamed";
    private int hostID;
    private int connID;
    // Use this for initialization
    void Start () {
        Debug.Log("qwe");
        transform.parent = PlayerManager.Instance.transform;
        if (isLocalPlayer)
        {
            //Here we set a name for this player (from PlayerPref for example)
            CmdSetDisplayName("SomeName");
        }
       
    }
	
	// Update is called once per frame
	void Update () {
        //Init some values (first frame only)
        if (isServer && !isInit)
        {
           
            NetworkIdentity identity = GetComponent<NetworkIdentity>();
            if (identity.connectionToClient != null)
            {
                Debug.Log("isinit");
                hostID = identity.connectionToClient.hostId;
                connID = identity.connectionToClient.connectionId;
                isInit = true;
            }
        }
        else
        {
            isInit = true;
        }

        //Update player ping
        if (isServer && !isLocalPlayer && Time.time > nextUpdate)
        {
            nextUpdate = Time.time + GetNetworkSendInterval();

            byte error;
            this.ping = (short)NetworkTransport.GetCurrentRtt(hostID, connID, out error);
        }

        if (Input.GetKeyUp(KeyCode.K))
        {
            CmdSpawnPlayer();
        }
    }
    [Command]
    void CmdSetDisplayName(string name)
    {
        this.displayName = name;
        this.gameObject.name = "Player " + name;
    }
    [Command]
    public void CmdSpawnPlayer()
    {
        Debug.Log("spawnPlayer");
        if (ClientScene.FindLocalObject(spawnedCharacterID) == null)
        {
            var go = (GameObject)Instantiate(playerPrefab, playerSpawnPos, Quaternion.identity);
            NetworkServer.AddPlayerForConnection(GetComponent<NetworkIdentity>().connectionToClient, go, 1);
            spawnedCharacterID = GetComponent<NetworkIdentity>().netId;

        }
        else
        {
            Debug.LogWarning("can not spawn two character");
        }

    }


    public GameObject GetCharacterObject()
    {
        if (spawnedCharacterID == null)
        {
            Debug.Log("characterID 为 null");
            return null;
        }

        return ClientScene.FindLocalObject(spawnedCharacterID);
    }
}

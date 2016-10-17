using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class MyNetworkManager : NetworkBehaviour
{
    public GameObject menu;
    public GameObject choosetrem;
   // public GameObject MainCamera;
    public GameObject canvas;


    public GameObject playerPrefab;
    public Vector3 playerSpawnPos;
    private short _term = 0;
    public bool iscontrol = true;

    public GameObject player;



    // private Vector3 startPositions;
    public override void OnStartServer()//服务器
    {
        Debug.Log("server");
        base.OnStartServer();
    }

    public override void OnStartClient()//客户端
    {

        base.OnStartClient();

        menu.SetActive(false);
        choosetrem.SetActive(true);

   //     MainCamera.SetActive(true);
        Debug.Log("client");
    }

    //public virtual void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    //{
    //    while (true)
    //    {

    //        if (_term == 0)
    //        {
    //            return;
    //        }

    //        return;

    //    }

    //}

        [Command]
    public void CmdOnRedClick()//按下红色队伍按钮
    {
        Debug.Log("red button");
        if (!isClient)
        {
            return;
        }
        canvas.SetActive(false);
        //MainCamera.SetActive(true);
        _term = 1;//1是红
        //RpcChangetrem(1);
        iscontrol = true;
        print(_term);
        PlayerRemoter pc = PlayerManager.Instance.GetLocalPlayer();
        Debug.Log(pc.GetCharacterObject());
        Debug.Log(pc.isServer);
        if (pc.GetCharacterObject() == null && !pc.isServer)
        {
            Debug.Log("start spawn");
            pc.CmdSpawnPlayer();

        }
        else {
            Debug.Log("error");
        }
  


    }

    public void OnBlueClick() //按下蓝色队伍按钮
    {

        if (!isClient)
        {
            return;
        }
        //if (isLocalPlayer == false)
        //{
        //    return;
        //}
        playerPrefab.GetComponent<PlayerControl>().term = 2;

        canvas.SetActive(false);
        //MainCamera.SetActive(true);
        _term = 2;//2是蓝
        //RpcChangetrem(2);
        iscontrol = true;

        //}
        //    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId) {
        //        var player = (GameObject)GameObject.Instantiate(playerPrefab, playerSpawnPos, Quaternion.identity);
        //        Debug.Log("client");

        //        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId); }

        //[ClientRpc]
        //void RpcChangetrem(int i)
        //{
        //    if (i == 1)
        //    {
        //        _term = 1;
        //    }
        //    else
        //    {
        //        _term = 2;
        //    }
        //}
    }
   




    // called when a client connects   
    public virtual void OnServerConnect(NetworkConnection conn) {
        Debug.Log("serverconnect");

    }

    // called when a client disconnects  
    public virtual void OnServerDisconnect(NetworkConnection conn)
    {
        NetworkServer.DestroyPlayersForConnection(conn);
    }

    // called when a client is ready  
    public virtual void OnServerReady(NetworkConnection conn)
    {
        NetworkServer.SetClientReady(conn);
        Debug.Log("server ready");
    }

    // called when a new player is added for a client  
    public virtual void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        var player = (GameObject)GameObject.Instantiate(playerPrefab, playerSpawnPos, Quaternion.identity);
        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
    }

    // called when a player is removed for a client  
    public virtual void OnServerRemovePlayer(NetworkConnection conn, short playerControllerId)
    {
       
    }
}
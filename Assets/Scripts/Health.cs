using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Health : NetworkBehaviour{
    public const int health = 100;//(只读)生命值

    [SyncVar(hook = "OnChangerHealth")]
    public int currenthealth = health;//当前生命值
    public Slider slider; //血量显示条

    private NetworkStartPosition[] startPositions;

    void Start()
    {
        if (isLocalPlayer)
        {
            startPositions = FindObjectsOfType<NetworkStartPosition>();//获得出生点
        }
      
    }



    public void Damage(int damage)             //这里不能加command命令.服务器下进行的角色攻击时会触发两次攻击特效，其本身进行一次，客户端中碰撞后，客户端本应不执行，但是加上command后，会让服务器再执行一次。
    {
        if (!isServer)
        {
            //Debug.Log("1");
            return;
        }
        currenthealth -= damage;
      
        if (currenthealth<0)
        {
            currenthealth = 0;//当生命值不能为负值
            Debug.Log("GG");
            RpcRespawn();

        }

    }

    public void OnChangerHealth(int currenthealth)
    {
        slider.value = currenthealth / (float)health;//在血条中显示
    }

    [ClientRpc]
    void RpcRespawn()
    {
        if (isLocalPlayer)
        {
            Vector3 SpawnPosition =Vector3.zero;//初始化
            //transform.position=Vector3.zero;
            if (startPositions!=null&&startPositions.Length>0)
            {
                SpawnPosition = startPositions[Random.Range(0, startPositions.Length)]
                    .transform.position;//随机选一个位置进行重生。先进行随机重生。
            }
            transform.position = SpawnPosition;
            currenthealth = health;//回复生命值。

        }
    }
}

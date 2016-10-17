using UnityEngine;
using UnityEngine.Networking;//使用networking命名空间

public class PlayerControl : NetworkBehaviour {//继承自networkbehaviour

    public GameObject bulletperfab;//子弹
    public Transform BulletTransform;//子弹生成的位置信息
    public float speed = 6;//子弹的速度
    public float playerspeed = 1;//玩家速度
    public int term=0;//队伍
    //public MyNetworkManager MyManager;
    private bool ishavetrem = false;
   
    public Transform MyCameraTransform;//摄像机位置

    [SerializeField]private Camera MyCamera;//摄像机






    // Use this for initialization
    void Start ()
    {

        MyCamera = Camera.main;
       

    }
    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        // you can use this function to do things like create camera, audio listeners, etc.
        // so things which has to be done only for our player
    }

    // Update is called once per frame
    void FixedUpdate ()
    { 
     if (isLocalPlayer == false)
     {
      // Debug.Log("is not me?");
      // Debug.Log("client" + isClient);
      // Debug.Log("server" + isServer);
      // Debug.Log("islocalplayer"+ isLocalPlayer);
         return;
     }
        //Debug.Log("should is palyer noy register");
  

        //if (ishavetrem == false)
        //{
        //    MyManager = FindObjectOfType<MyNetworkManager>();
        //    term = MyNetworkManager._term;
        //    RpcTrem();
        //}


        // if (MyManager.iscontrol == false)
        // {
        //     Debug.Log("can not control");
        //     return;
        //     
        // }
        //

        //每个客户端都只用自己客户端中的maincamera。而不是一个客户端创建一个maincamera
        MyCamera.transform.position = MyCameraTransform.position;//解决了摄像机同步的问题。
        MyCamera.transform.rotation = MyCameraTransform.rotation;
        float h = Input.GetAxis("Horizontal");//水平 也就是旋转
	    float v = Input.GetAxis("Vertical");//竖直  在这里是前后


        transform.Translate(Vector3.right * h * Time.fixedDeltaTime*playerspeed);
        transform.Translate(Vector3.forward * v * Time.fixedDeltaTime*playerspeed);
        transform.Rotate(Vector3.up*h*120*Time.deltaTime);


        if (Input.GetMouseButtonDown(0))
        {
            CmdFire();//子弹发射。
        }

    }

    //[ClientRpc]
    //void RpcTrem()
    //{
       
    //    if (MyManager == null)//有问题 找不到 
    //    {
    //        Debug.Log("can not find manager");
    //        return;
    //    }
    //    if (term != 0)//这证明term 已经被赋值。赋值之后将不再执行，也就是说值只被赋一次
    //    {
    //        ishavetrem = true;
    //        CmdChangertrem();
    //    }
    //}

    //[Command]
    //void CmdChangertrem()
    //{
    //    MyNetworkManager._term = 0;
    //    print("CmdChangertrem");
    //}


    [Command]       //所有的游戏逻辑要在服务器中写。
     void CmdFire() //注意这里的C 一定要大写
    {
        //生成子弹
        GameObject bullet = Instantiate(bulletperfab,
            BulletTransform.position, 
            BulletTransform.rotation) as GameObject;
         //速度*方向* 力度
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward*speed;
        bullet.GetComponent<bullet>().term = this.term;
        //2秒后销毁
        //Destroy(bullet,2);
        //在服务器中生成
        NetworkServer.Spawn(bullet);
        //Debug.Log("fire");
    }

 
}

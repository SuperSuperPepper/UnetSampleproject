using UnityEngine;
using System.Collections;

public class bullet : MonoBehaviour
{

    public int damage = 10;
    public int term=0;

    void OnCollisionEnter(Collision collision)
    {
        //collider.GetComponent<Health>().Damage(damage);这种方式不可以。不可以在collision上找到组件
        var hit = collision.gameObject;//获得碰撞体的gameobject
        var health = hit.GetComponent<Health>();
        var playercontrol = hit.GetComponent<PlayerControl>();       
        if (health!=null&&playercontrol!=null&&playercontrol.term!=this.term)//这句很重要。如果没有这句碰到的物体很可能没有Heath脚本
        {
            Debug.Log(playercontrol.term);
            health.Damage(damage);//执行其Damage函数
        }
       
        Destroy(gameObject);//销毁自己
    }
}

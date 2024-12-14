using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceAndFire_Controller : ThunderSrike_Controller
{
    // 当2D碰撞触发时调用此方法
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        // 调用基类的OnTriggerEnter2D方法
        base.OnTriggerEnter2D(collision);
    }
}

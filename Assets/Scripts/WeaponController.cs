using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// プレイヤーの攻撃操作に関する処理
public class WeaponController : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Destroy(other.gameObject);
        }
    }
}

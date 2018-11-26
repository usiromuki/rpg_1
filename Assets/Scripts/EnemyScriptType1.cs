// Enemyの行動を定義したスクリプト
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enemy Type1 の行動
public class EnemyScriptType1 : MonoBehaviour
{
    enum MOVEDIR
    {
        LEFT,
        RIGHT
    }
    private MOVEDIR moveDir = MOVEDIR.LEFT; // Enemyの進行方向
    public LayerMask block; // 壁のレイヤーマスク Enemyの方向転換に使用

    // Enemyの寿命 
    private float lifeTimeCounter = 0;
    private float lifeTime = 5;

    void Update ()
    {
        // Enemyと壁の衝突時の方向転換
        if(moveDir == MOVEDIR.LEFT)
        {
            this.transform.position += Vector3.left * Time.deltaTime * 3;
            bool hit = Physics2D.Linecast((Vector2)this.transform.position, (Vector2)this.transform.position + (Vector2.left*0.5f), block);
            if (hit)
            {
                moveDir = MOVEDIR.RIGHT;
            }
        }
        else if(moveDir == MOVEDIR.RIGHT)
        {
            this.transform.position -= Vector3.left * Time.deltaTime * 3;
            bool hit = Physics2D.Linecast((Vector2)this.transform.position, (Vector2)this.transform.position + (Vector2.right * 0.5f), block);
            if (hit)
            {
                moveDir = MOVEDIR.LEFT;
            }
        }
        // Enemyの寿命
        lifeTimeCounter+=Time.deltaTime;
        if (lifeTimeCounter > lifeTime)
        {
            Destroy(this.gameObject);
        }
    }

    private bool isEnemyColision = true;

    // Enemy同士の衝突後、Enemy同士が離れるまで一瞬だけ当たり判定を無効化する
    private IEnumerator IgnoreEnemyColider()
    {
        yield return new WaitForSeconds(0.1f);
        isEnemyColision = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Enemy同士で衝突したときの方向転換
        if (isEnemyColision)
        {
            if (other.gameObject.tag == "EnemySide")
            {
                if (moveDir == MOVEDIR.LEFT)
                {
                    moveDir = MOVEDIR.RIGHT;
                }
                else if (moveDir == MOVEDIR.RIGHT)
                {
                    moveDir = MOVEDIR.LEFT;
                }
                isEnemyColision = false;
                StartCoroutine(IgnoreEnemyColider());
            }
        }
    }
}

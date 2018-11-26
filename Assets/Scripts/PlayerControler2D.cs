using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// プレイヤーの操作に関連する処理
public class PlayerControler2D : MonoBehaviour
{
    // 移動のモード
    enum DASH_MODE
    {
        NORMAL,
        LEFT_DASH,
        RIGHT_DASH,
    }
    private byte prevMoveMode = (byte)DASH_MODE.NORMAL; 
    private byte moveMode = (byte)DASH_MODE.NORMAL; 
    private byte moveSpeed = 1; // 移動速度
    private float jumpXAxisForce = 1; // ジャンプの勢い
    private bool isJumping = false; // ジャンプしているか
    private bool isJumpStart = false; // ジャンプした直後か。
                                      // ジャンプ直後に勢いが無いと地面とあたってるとみなされ
                                      // ジャンプのアニメーションが再生されないため、それを防ぐために
                                      // ジャンプ直後は地面との当たり判定を行わないようにする
    private bool isLeftDashWait = false;
    private bool isRightDashWait = false;
    private bool isLeftDash = false;
    private bool isRightDash = false;

    Rigidbody2D playerRB;
    public StageManager stageManager;
    public GameObject gameOverObj;
    private SpriteRenderer gameOverSprite;

    private GameObject weaponHitBox; // プレイヤーの攻撃に使用する当たり判定用オブジェクト
    private float attackCoolTime = 1.0f; // 攻撃後次に攻撃できるまでの時間
    private bool canAttack = true; // 攻撃が可能か

    // 死亡判定
    private bool isDead = false;
    // ゴールしているか 
    private bool isGoal = false;
    // プレイヤーが死亡時に表示する画像のα値
    float gameOverAlpha = 0.0f;
    // プレイヤーが移動できる左側の範囲
    private int movableLeft = -6;

    // プレイヤーのアニメーションのフラグ制御用のAnimator
    Animator anim;

    // 効果音
    public AudioClip jumpSE;
    public AudioClip attackSE;
    private AudioSource[] sources;

    void Start()
    {
        playerRB = this.gameObject.GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        weaponHitBox = this.gameObject.transform.Find("WeaponHitBox").gameObject;
        sources = gameObject.GetComponents<AudioSource>();
        sources[0].clip = jumpSE;
        sources[1].clip = attackSE;
    }

    void Update()
    {
        MoveInputUpdate();
        JumpInputUpdate();
        UpdateAnimation();

        if(!isDead)
        {
            if (playerDirection == (int)DIRECTION.RIGHT)
            {
                this.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            }
            else
            {
                this.transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
            }
        }

        if(isDead)
        {
            // キャラクターが死んだとき地面に沈んでいく
            this.transform.position += Vector3.down * Time.deltaTime;

            // キャラクターが死んだとき「ゲームオーバー」の画像を表示する
            gameOverAlpha+=0.01f;
            gameOverSprite.color = new Color(0.0f, 0.0f, 0.0f, gameOverAlpha);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Goal")
        {
            if (isDead || isGoal)
            {
                return;
            }
            StageManager.isPlayerGoal = true;
            playerRB.velocity = new Vector2(0, 0);
        }

    }

    void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            if (isDead || isGoal)
            {
                return;
            }
            // キャラクターが死亡したときの動作
            isDead = true;
            anim.SetBool("isDamage", true);
            // キャラクターを回転させて倒れたように表示する
            Quaternion angle = Quaternion.identity;
            angle.eulerAngles = new Vector3(0.0f, 0.0f, 90.0f);
            this.transform.rotation = angle;

            // 敵や地面の影響を受けないようにコンポーネントを削除する
            Destroy(GetComponent<Rigidbody2D>());
            Destroy(GetComponent<Collider2D>());

            // GameOver画像を表示する
            gameOverObj.SetActive(true);
            gameOverSprite = gameOverObj.GetComponent<SpriteRenderer>();
            StartCoroutine(PlayerDead());
        }
    }

    // プレイヤーの行動に関する情報の更新
    enum DIRECTION { RIGHT,LEFT};
    int playerDirection = (int)DIRECTION.RIGHT;
    private void MoveInputUpdate()
    {
        if(isDead || isGoal)
        {
            return;
        }
        // 攻撃アニメーションから通常アニメに戻す
        if(!canAttack)
        {
            if(anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0)
            {
                anim.SetBool("isAttack", false);
            }
        }
        // J キーを押された時攻撃を行い、攻撃のアニメーションを再生する
        // 攻撃は武器としての当たり判定を持ったオブジェクトを有効化して
        // それに敵があたった場合に敵を撃破するものとする。
        if (Input.GetKeyDown(KeyCode.J))
        {
            if(canAttack&&!isJumping)
            {
                sources[1].Play();
                anim.SetBool("isAttack", true); 
                canAttack = false; // 攻撃可能フラグ。 WaitAttackCoolTIme() 内でtrueに戻す
                weaponHitBox.SetActive(true); // 武器の当たり判定の有効化。 EnableAttack() 内で一定時間後に無効化
                StartCoroutine(EnableAttack()); // 武器の当たり判定を一定時間有効化
                StartCoroutine(WaitAttackCoolTIme()); // 攻撃後、一定時間再度攻撃を行えなくする
            }
        }
        // キー入力解除時(ボタンを離したとき)
        if (Input.GetKeyUp(KeyCode.A))
        {
            isLeftDash = false;
            moveSpeed = 1;
            jumpXAxisForce = 0;
            
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            isRightDash = false;
            moveSpeed = 1;
            jumpXAxisForce = 0;
            
        }
        // ジャンプ中ではなく武器が有効化されていない(攻撃中ではない)時のみ移動可能
        if (!isJumping&& !weaponHitBox.activeInHierarchy)
        {
            // * ダッシュについて
            // ダッシュは左右どちらかに移動を入力すると
            // isLeftDashWait か isRightDashWaitが true になり
            // LeftDashWait() か RightDashWait() で上記フラグが falseに戻される前に
            // 再度左右の入力が行われると ダッシュする。

            if (Input.GetKeyDown(KeyCode.A))
            {
                if (isLeftDashWait)
                {
                    isLeftDash = true;
                    moveSpeed = 3;
                    isLeftDashWait = false;
                }
                else
                {
                    isLeftDashWait = true;
                    StartCoroutine(LeftDashWait());
                }
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                if (isRightDashWait)
                {
                    isRightDash = true;
                    moveSpeed = 3;
                    isRightDashWait = false;
                }
                else
                {
                    isRightDashWait = true;
                    StartCoroutine(RightDashWait());
                }
            }

            // 左へ移動
            if (Input.GetKey(KeyCode.A))
            {
                playerDirection = (int)DIRECTION.LEFT;
                if (isLeftDash)
                {

                    playerRB.position += Vector2.left * Time.deltaTime * moveSpeed;
                    jumpXAxisForce = -3;
                }
                else
                {
                    
                    playerRB.position += Vector2.left * Time.deltaTime;
                    jumpXAxisForce = -1;
                }

            }
            if (Input.GetKey(KeyCode.D))
            {
                playerDirection = (int)DIRECTION.RIGHT;
                if (isRightDash)
                {
                    playerRB.position += Vector2.right * Time.deltaTime * moveSpeed;
                    jumpXAxisForce = 3;
                }
                else
                {
                    playerRB.position += Vector2.right * Time.deltaTime;
                    jumpXAxisForce = 1;
                }
            }
            if(playerRB.position.x <= movableLeft)
            {
                playerRB.position = new Vector2(movableLeft, playerRB.position.y);
            }
        }
    }

    // ダッシュのため入力を待機する
    IEnumerator LeftDashWait()
    {
        if (!isLeftDashWait)
        {
            yield break;
        }

        yield return new WaitForSeconds(0.3f);
        isLeftDashWait = false;
    }

    // ダッシュのため入力を待機する
    IEnumerator RightDashWait()
    {
        if (!isRightDashWait)
        {
            yield break;
        }

        yield return new WaitForSeconds(0.3f);
        isRightDashWait = false;
    }

    // 攻撃の当たり判定を有効化する
    private IEnumerator EnableAttack()
    {
        yield return new WaitForSeconds(0.5f);
        weaponHitBox.SetActive(false);
    }

    // 一度攻撃を行った後、attackCoolTimeの秒数だけ攻撃を行えなくする
    private IEnumerator WaitAttackCoolTIme()
    {
        yield return new WaitForSeconds(attackCoolTime);
        canAttack = true;
    }

    // ジャンプした瞬間に指定時間分だけ地面との当たり判定を止める
    private IEnumerator JumpStart()
    {
        yield return new WaitForSeconds(0.2f);
        isJumpStart = false;
    }
    
    // ジャンプ関連の情報の更新
    private void JumpInputUpdate()
    {
        if(isDead || isGoal)
        {
            return;
        }

        float lineDist = 0.2f;
        float linelength = 1.23f;

        if (!isJumpStart)
        {
            RaycastHit2D hitLeft = Physics2D.Linecast(this.transform.position - (transform.right * lineDist), this.transform.position - (transform.right * lineDist) - (transform.up * linelength), (1 << LayerMask.NameToLayer("ground")));
            RaycastHit2D hitRight = Physics2D.Linecast(this.transform.position + (transform.right * lineDist), this.transform.position + (transform.right * lineDist) - (transform.up * linelength), (1 << LayerMask.NameToLayer("ground")));
            Debug.DrawLine(this.transform.position - (transform.right * lineDist), this.transform.position - (transform.right * lineDist) - (transform.up * linelength), Color.red);
            Debug.DrawLine(this.transform.position + (transform.right * lineDist), this.transform.position + (transform.right * lineDist) - (transform.up * linelength), Color.red);

            if (hitLeft.collider || hitRight.collider)
            {
                isJumping = false;
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isJumping&& !weaponHitBox.activeInHierarchy)
            {
                // ダッシュを行わないようにする
                // (ダッシュを行ってからジャンプすると着地時に加速してしまうため)
                isRightDash = false;
                isLeftDash = false;
                moveSpeed = 1;

                isJumping = true;
                isJumpStart = true;
                StartCoroutine(JumpStart());
                playerRB.velocity = new Vector2(jumpXAxisForce, 7);
                sources[0].Play();
            }
        }
        // 画面の左側の端を超えないように制御
        if (playerRB.position.x <= movableLeft)
        {
            playerRB.position = new Vector2(movableLeft, playerRB.position.y);
        }        
    }

    // ジャンプのフラグに応じてジャンプのアニメーションを切り替える
    private void UpdateAnimation()
    {
        if (isJumping)
        {
            anim.SetBool("isJumping", true);
        }
        else
        {
            anim.SetBool("isJumping", false);
        }
    }

    // プレイヤーが死亡時にステージ側に死んだことを伝える
    // ゲームオーバー画面の表示のため少し待つ
    IEnumerator PlayerDead()
    {
        yield return new WaitForSeconds(2.0f);
        StageManager.isPlayerDead = true;
    }
}

using Base;
using GameSystem;
using UnityEngine;

namespace Entity
{
    public class EntityPlayer : EntityBase
    {
        // 私有字段
        private int _nowJumpCount; //可跳高的次数
        private int _nowJumpDouCount; //可二段跳的次数


        private int _jumpMulFor = 1;
        private bool _isOpenDoubleJump = false;
        private float _scalePrefab = 1f;
        private int _speedCutX = 0;
        private int _speedAddX = 0;

        // Unity组件引用
        private Rigidbody2D rb;
        private BoxCollider2D coll;
        private SpriteRenderer sprite;
        private Animator anim;
        private GameObject playerGameObject;

        // 移动相关
        private float dirX = 0f;
        private float lastDirX = 0f;
        private float baseMoveSpeed = 5f;
        private float baseJumpForce = 10f;
        private float groundCheckDistance = 0.1f;
        public bool canDetect = false;


        // 移动状态枚举
        private enum MovementState
        {
            idle,
            running,
            jumping,
            falling
        }

        // 属性包装器，当值改变时触发事件
        public int NowJumpCount
        {
            get => _nowJumpCount;
            set
            {
                if (_nowJumpCount != value)
                {
                    _nowJumpCount = value;
                    GameEventManager.TriggerPlayerUIUpdate(this);
                }
            }
        }

        public int NowJumpDouCount
        {
            get => _nowJumpDouCount;
            set => _nowJumpDouCount = value;
        }


        public int JumPMulFor
        {
            get => _jumpMulFor;
            set
            {
                if (_jumpMulFor != value)
                {
                    _jumpMulFor = value;
                    GameEventManager.TriggerPlayerUIUpdate(this);
                }
            }
        }


        public bool IsOpenDoubleJump
        {
            get => _isOpenDoubleJump;
            set
            {
                if (_isOpenDoubleJump != value)
                {
                    _isOpenDoubleJump = value;
                    GameEventManager.TriggerPlayerUIUpdate(this);
                }
            }
        }

        public float scalePrefab
        {
            get => _scalePrefab;
            set
            {
                if (_scalePrefab != value)
                {
                    _scalePrefab = value;
                    // 立即应用缩放
                    if (playerGameObject != null)
                    {
                        playerGameObject.transform.localScale = new Vector3(_scalePrefab, _scalePrefab, _scalePrefab);
                    }

                    GameEventManager.TriggerPlayerUIUpdate(this);
                }
            }
        }

        public int speedCutX
        {
            get => _speedCutX;
            set
            {
                if (_speedCutX != value)
                {
                    _speedCutX = value;
                    GameEventManager.TriggerPlayerUIUpdate(this);
                }
            }
        }

        public int speedAddX
        {
            get => _speedAddX;
            set
            {
                if (_speedAddX != value)
                {
                    _speedAddX = value;
                    GameEventManager.TriggerPlayerUIUpdate(this);
                }
            }
        }

        // 重写setHp方法添加事件触发
        public override void setHp(int hp)
        {
            int oldHp = getHp();
            base.setHp(hp);
            if (oldHp != hp)
            {
                GameEventManager.TriggerPlayerUIUpdate(this);
            }
        }

        // 重写setSpeed方法添加事件触发
        public override void setSpeed(int speed)
        {
            int oldSpeed = getSpeed();
            base.setSpeed(speed);
            if (oldSpeed != speed)
            {
                baseMoveSpeed = speed; // 同步到移动速度
                GameEventManager.TriggerPlayerUIUpdate(this);
            }
        }

        /// <summary>
        /// 绑定GameObject和初始化组件
        /// </summary>
        /// <param name="gameObject">玩家的GameObject</param>
        public void BindGameObject(GameObject gameObject)
        {
            playerGameObject = gameObject;

            // 获取组件引用
            rb = gameObject.GetComponent<Rigidbody2D>();
            coll = gameObject.GetComponent<BoxCollider2D>();
            sprite = gameObject.GetComponentInChildren<SpriteRenderer>();
            anim = gameObject.GetComponentInChildren<Animator>();

            // 如果没有组件，添加默认组件
            if (rb == null) rb = gameObject.AddComponent<Rigidbody2D>();
            if (coll == null) coll = gameObject.AddComponent<BoxCollider2D>();

            Debug.Log($"EntityPlayer绑定GameObject: {gameObject.name}");
        }

        /// <summary>
        /// 获取绑定的GameObject
        /// </summary>
        /// <returns></returns>
        public override GameObject getEntityObj()
        {
            return playerGameObject;
        }

        /// <summary>
        /// 初始化实体属性
        /// </summary>
        public void initEntityProperty()
        {
            _jumpMulFor = 1;
            _nowJumpCount = 0;
            _isOpenDoubleJump = false;
            _scalePrefab = 1;
            _speedCutX = 0;
            _speedAddX = 0;
            setHp(100);
            setSpeed(5);
            baseMoveSpeed = 5f;
            baseJumpForce = 7f;

            // 初始化完成后触发UI更新
            GameEventManager.TriggerPlayerUIUpdate(this);
        }

        #region 移动和跳跃逻辑（从PlayerMove移植过来）

        /// <summary>
        /// 更新移动逻辑 - 应该在Update中调用
        /// </summary>
        public void UpdateMovement()
        {
            if (rb == null) return;

            lastDirX = dirX;
            dirX = Input.GetAxisRaw("Horizontal");

            // 计算实际移动速度（基础速度 + buff加成 - buff减成）
            float actualMoveSpeed = baseMoveSpeed + speedAddX - speedCutX;
            actualMoveSpeed = Mathf.Max(0, actualMoveSpeed); // 确保速度不为负

            rb.velocity = new Vector2(dirX * actualMoveSpeed, rb.velocity.y);

            // 跳跃逻辑
            if (Input.GetButtonDown("Jump") && CanJump())
            {
                Jump();
            }

            UpdateAnimationState();
        }

        /// <summary>
        /// 检查是否可以跳跃
        /// </summary>
        /// <returns></returns>
        private bool CanJump()
        {
            // 基础地面检测
            if (IsGrounded())
            {
                NowJumpDouCount = 2;

                return true;
            }

            // 多段跳检测
            if (IsOpenDoubleJump)
            {
                NowJumpDouCount--;
                return NowJumpDouCount > 0;
            }

            return false;
        }

        /// <summary>
        /// 执行跳跃
        /// </summary>
        private void Jump()
        {
            if (rb == null) return;
            // 计算实际跳跃力（基础跳跃力 * buff倍数）
            float actualJumpForce = baseJumpForce * JumPMulFor;
            rb.velocity = new Vector2(rb.velocity.x, actualJumpForce);
        }

        /// <summary>
        /// 更新动画状态
        /// </summary>
        private void UpdateAnimationState()
        {
            if (anim == null || sprite == null || rb == null) return;

            MovementState state;

            if (dirX > 0f && IsGrounded())
            {
                state = MovementState.running;
                sprite.flipX = false;
            }
            else if (dirX < 0f && IsGrounded())
            {
                state = MovementState.running;
                sprite.flipX = true;
            }
            else
            {
                state = MovementState.idle;
            }

            if (rb.velocity.y > 0.1f)
            {
                if (lastDirX > 0f) sprite.flipX = false;
                else sprite.flipX = true;
                state = MovementState.jumping;
            }
            else if (rb.velocity.y < -0.1f)
            {
                state = MovementState.falling;
            }

            anim.SetInteger("state", (int)state);
        }

        /// <summary>
        /// 检测是否在地面
        /// </summary>
        /// <returns></returns>
        private bool IsGrounded()
        {
            if (playerGameObject == null) return false;

            // 从玩家碰撞体的底部中心向下发射短射线
            Vector2 origin;
            if (coll != null)
                origin = new Vector2(coll.bounds.center.x, coll.bounds.min.y + 0.01f);
            else
                origin = playerGameObject.transform.position;

            float distance = groundCheckDistance; // 建议 0.05f ~ 0.15f
            int mask = LayerMask.GetMask("Ground") | LayerMask.GetMask("Special");

            RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, distance, mask);

#if UNITY_EDITOR
            Debug.DrawRay(origin, Vector2.down * distance, hit.collider ? Color.green : Color.red);
#endif

            return hit.collider != null;
        }

        /// <summary>
        /// 碰撞检测
        /// </summary>
        /// <param name="collision"></param>
        public void OnCollisionEnter(Collision2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Pop"))
            {
                if (rb != null)
                {
                    rb.velocity = new Vector2(0, baseJumpForce * 3);
                }
            }

            if (collision.gameObject.layer == LayerMask.NameToLayer("Special") && canDetect == true)
            {
                Debug.Log("Special碰撞检测触发！销毁物体，消费检测机会");
                canDetect = false; // 消费这次检测机会
                collision.gameObject.SetActive(false);
            }
        }

        #endregion
    }
}
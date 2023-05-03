using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Sprite leftImg;//0上 1左 2右
    public Sprite rightImg;
    public Sprite upImg;
    
    private enum Dirction
    {
        Up, Right, Left
    }

    private Dirction _dir;
    
    public float jumpDistance;//短跳距离
    public int stepPoint;//单次得分
    
    private float _moveDistance;//每次移动距离
    private Vector3 _targetPos; //最终跳跃的位置
    private int _allPoint;//总得分

    private Rigidbody2D _rb;
    private Animator _anim;
    private SpriteRenderer _renderer;
    private SpriteRenderer _dirRenderer;
    private BoxCollider2D _bc;
    
    private bool _isJumpHeld;//检测是否长按
    private bool _isJumping;//是否正在跳跃
    private bool _canJump;//是否可以跳跃
    private bool _isDead;//是否死亡

    private RaycastHit2D[] _rayResult = new RaycastHit2D[4];
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _renderer = GetComponent<SpriteRenderer>();
        _targetPos = transform.position;
        _bc = GetComponent<BoxCollider2D>();
        _dirRenderer = transform.Find("Dir").GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        _dirRenderer.enabled = false;
    }

    private void Update()
    {
        if(_isDead)
            return;
        if (_canJump)
        {
            TriggerJump();
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    public void Move()
    {
        if (_isJumping)
        {
            _rb.position = Vector3.Lerp(transform.position, _targetPos, 0.134f);//跳跃过程中执行
        }
    }

    public void TriggerJump()
    {
        _canJump = false;
        _anim.SetTrigger("Jump");

        var tempPos = transform.position;
        switch (_dir)
        {
            case Dirction.Left:
                _anim.SetBool("IsSide", true);
                _targetPos = tempPos + new Vector3(-_moveDistance, 0, 0);
                transform.localScale = Vector3.one;
                break;
            case Dirction.Right:
                _anim.SetBool("IsSide", true);
                _targetPos = tempPos + new Vector3(_moveDistance, 0, 0);
                transform.localScale = new Vector3(-1, 1, 1);
                break;
            case Dirction.Up:
                _anim.SetBool("IsSide", false);
                _targetPos = tempPos + new Vector3(0, _moveDistance,  0);
                transform.localScale = Vector3.one;
                break;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!_isJumping && other.CompareTag("Water"))
        {
            Physics2D.RaycastNonAlloc(transform.position + Vector3.up * 0.15f, Vector3.zero, _rayResult);
            
            bool inWater = true;
            
            foreach (var hit in _rayResult)
            {
                if(hit.collider == null) continue;

                
                if (hit.collider.CompareTag("Wood"))
                {
                    //跟随木板移动
                    transform.SetParent(hit.collider.transform);
                    inWater = false;
                }
            }
            
            //:与河流接触逻辑
            if (!_isJumping && inWater)
            {
                _isDead = true;
                Debug.Log("InWater GameOver");
            }

        }
        
        if (other.CompareTag("Border") || other.CompareTag("Car"))
        {
            _isDead = true;
            Debug.Log("GameOver");
        }

        if (!_isJumping && other.CompareTag("Obstacle"))
        {
            _isDead = true;
            Debug.Log("GameOver");
        }

        if (_isDead)
        {
            EventsManager.CallUpOnGameOver();
            GetComponent<PlayerInput>().enabled = false;
            _bc.enabled = false;
        }
    }

    #region AnimationEvents

    public void JumpEnter()
    {
        _isJumping = true;

        _renderer.sortingLayerName = "front";

        transform.parent = null;
        
        AudioManager.instance.PlayTriggerMusic();
    }

    public void JumpExit()
    {
        _isJumping = false;
        
        _renderer.sortingLayerName = "middle";

        if (_dir == Dirction.Up && !_isDead)
        {
            EventsManager.CallUpOnGetPoint(_allPoint);
        }

        _dirRenderer.enabled = false;
    }

    #endregion

    #region Input 输入回调函数

    public void Jump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && !_isJumping)
        {
            _moveDistance = jumpDistance;
            _canJump = true;
            if (_dir == Dirction.Up)
                _allPoint += stepPoint;
            
            AudioManager.instance.SetTriggerMusic(0);
        }
    }

    public void LongJump(InputAction.CallbackContext ctx)
    {
        Debug.Log(ctx.action);
        if (ctx.performed && !_isJumping)
        {
            _isJumpHeld = true;
            _moveDistance = jumpDistance * 2;
            AudioManager.instance.SetTriggerMusic(1);
            _dirRenderer.enabled = true;
        }

        if (ctx.canceled && _isJumpHeld)
        {
            if (_dir == Dirction.Up)
                _allPoint += stepPoint * 2;
            _canJump = true;
            _isJumpHeld = false;
        }
    }

    public void GetTouchPosition(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            var tempPos = Camera.main.ScreenToWorldPoint((Vector3) ctx.ReadValue<Vector2>());
            tempPos.z = 0;
            var moveDir = (tempPos - transform.position).normalized;
            if (Mathf.Abs(moveDir.x) <= 0.7f)
            {
                _dir = Dirction.Up;
                _dirRenderer.sprite = upImg;

            }else if (moveDir.x > 0)
            {
                _dir = Dirction.Right;
                if (Mathf.Approximately(transform.localScale.x, -1))
                {
                    _dirRenderer.sprite = leftImg;
                }
                else
                    _dirRenderer.sprite = rightImg;
                
            }else if (moveDir.x < 0)
            {
                _dir = Dirction.Left;
                if (Mathf.Approximately(transform.localScale.x, -1))
                {
                    _dirRenderer.sprite = rightImg;
                }
                else
                    _dirRenderer.sprite = leftImg;
            }
        }
    }
    
    #endregion
}

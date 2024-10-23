using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Horizontal Movement Setting")]
    [SerializeField] private float walkSpeed = 1;
    // trục
    [Header("Ground CHeck Settings")]
    [SerializeField] private float JumpForce = 45f;
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private float groundCheckY = 0.2f;
    [SerializeField] private float groundCheckX = 0.5f;
    [SerializeField] private LayerMask whatIsGround;
    private float gravity;

    public Rigidbody2D rb;
    private BoxCollider2D coll;
    // tốc độ của nhân vật khi đo bộ
    public float xAxis, y;
    public Animator anim;

    public static PlayerController Instance;
    //[System.NonSerialized]
    //public StateMachine stateMachine;

    // singleton pattern 
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        //stateMachine = new StateMachine(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        gravity = rb.gravityScale;
        //stateMachine.Initialize(stateMachine.idleState);

    }

    // Update is called once per frame
    void Update()
    {
        //stateMachine.Update();

        GetInputs();
        Move();
        Jump();
        Flip();
        UseSkill();
        UseDash();
        //if (Input.GetButtonDown("JumpForce"))
        //{
        //    rb.velocity = new Vector2(rb.velocity.x, JumpForce);
        //}    
    }
    // hàm để đi chuyển theo trục ngang và đưa vào update để cập nhật chúng
    void GetInputs()
    {
        xAxis = Input.GetAxisRaw("Horizontal");
    }

    // doi chieu Player
    void Flip()
    {
        if (xAxis < 0)
        {
            transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);
        }
        else if (xAxis > 0)
        {
            transform.localScale = new Vector2(-Mathf.Abs(transform.localScale.x), transform.localScale.y);
        }
    }
    // hàm di chuyển 
    private void Move()
    {
        if (isDash == true) return;
        rb.velocity = new Vector2(walkSpeed * xAxis, rb.velocity.y);
        if (rb.velocity.x != 0 && IsGrounded() && !anim.GetBool("Walking"))
            anim.SetBool("Walking", true);
        if (rb.velocity.x == 0 && IsGrounded() && anim.GetBool("Walking"))
            anim.SetBool("Walking", false);
    }

    private void UseSkill()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (IsGrounded())
            {
                rb.velocity = Vector2.zero;
                anim.SetBool("Walking", false);
                anim.Play("Player_Skill1");
            }
        }

    }

    bool isDash = false;
    [SerializeField] private float dashTime;
    [SerializeField] GameObject dashEffect;

    IEnumerator DashTime()
    {
        yield return new WaitForSeconds(0.3f);
        isDash = false;
        rb.velocity = Vector2.zero;
        rb.gravityScale = gravity;
    }
    private void UseDash()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (isDash == false)
            {
                isDash = true;
                if (!IsGrounded())
                {
                    rb.gravityScale = 0;
                }
                rb.velocity = Vector2.zero;
                anim.SetBool("Walking", false);
                anim.Play("Player_Dash");
                rb.velocity = new Vector2(dashTime * transform.localScale.x, 0);
                //Debug.Log(dashEffect.name);
                //GameObject DashC = Instantiate(dashEffect);
                Instantiate(dashEffect, transform);
                StartCoroutine(DashTime());
            }
     
        }

    }
    // kiem tra mat dat
    //public bool Grounded()
    //{
    //    if (Physics2D.Raycast(groundCheckPoint.position, Vector2.down, groundCheckY, whatIsGround)
    //        || Physics2D.Raycast(groundCheckPoint.position + new Vector3(groundCheckX, 0, 0), Vector2.down, groundCheckY, whatIsGround)
    //        || Physics2D.Raycast(groundCheckPoint.position + new Vector3(-groundCheckX, 0, 0), Vector2.down, groundCheckY, whatIsGround))
    //    {
    //        return true;
    //    }
    //    else
    //    {
    //        return false;
    //    }
    //}
    void Jump()
    {
        if (Input.GetButtonUp("JumpForce") && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }
        if (Input.GetButtonDown("JumpForce") && IsGrounded())
        {
            rb.velocity = new Vector3(rb.velocity.x, JumpForce);
            anim.SetBool("Jumping", true);
        }
        //anim.SetBool("Jumping", !Grounded());
        if(!IsGrounded() && rb.velocity.y < 0 && anim.GetBool("Jumping") && !anim.GetBool("Falling"))
        {
            anim.SetBool("Falling", true);
        }
        if(IsGrounded() && anim.GetBool("Falling"))
        {
            anim.SetBool("Falling", false);
            anim.SetBool("Jumping", false);
        }
    }

    public bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .05f, whatIsGround);
    }
}
 
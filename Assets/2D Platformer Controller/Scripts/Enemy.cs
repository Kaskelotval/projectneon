using UnityEngine;

[RequireComponent(typeof(EnemyController2D))]
public class Enemy : MonoBehaviour
{
    public float maxJumpHeight = 4f;
    public float minJumpHeight = 1f;
    public float timeToJumpApex = .4f;
    private float accelerationTimeAirborne = .2f;
    private float accelerationTimeGrounded = .1f;
    public float moveSpeed = 0f;
    private Animator m_Anim;
    //GUARDING//
    public bool Guarding = false;
    //PATROLLING//
    public bool Patrolling = false;
    private bool Returning = false;
    public float chillTime = 3f;
    public float PatLeft;
    public float PatRight;
    private Vector3 originalPos;
    private Vector3 PatLeftPos;
    private Vector3 PatRightPos;
    public float Patrollspeed = 0.5f;
    private float chilltimer;
    private bool outside = false;
    //ATTACKING//
    public Collider2D attackArea;
    public float AttackTimerMax = 1f;
    private bool attacking = false;
    private float AttackTimer;
    public Collider2D playerCollider;
    public float huntSpeed = 0.01f;
    
    public Transform me;
    public Transform target;
    public Collider2D colli;
    private Raycaster detray;
    private bool isDetected = false;
    
    private float gravity;
    private Vector3 velocity;
    private float velocityXSmoothing;

    private EnemyController2D controller;
    public float LookForPlayerMax = 4f;
    private float LookForPlayerTimer;

    private Vector2 directionalInput = new Vector2(1, 0);
    public int curHealth;

    private float OriginalMoveSpeed;
    public Player player;
    private bool ScoreGiven = false;
    private void Start()
    {
        AttackTimer = AttackTimerMax;
        attackArea.enabled = true;
        //patrolling
        chilltimer = chillTime;
        originalPos = me.transform.position;
        PatLeftPos = me.transform.position - new Vector3(PatLeft,0);
        PatRightPos = me.transform.position + new Vector3(PatRight, 0);
 

        //health
        //stuff
        Debug.Log("movesp " + moveSpeed);
        OriginalMoveSpeed = moveSpeed;
        m_Anim = GetComponent<Animator>();
        controller = GetComponent<EnemyController2D>();
        detray = GetComponent<Raycaster>();
        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        m_Anim.SetBool("Running", false);
    }

    private void Update()
    {
        if (curHealth <= 0)
        {
            if(!ScoreGiven && !isDetected)
            {
                player.addScore(200);
                ScoreGiven = true;
            }
            else if(!ScoreGiven && isDetected)
            {
                player.addScore(100);
                ScoreGiven = true;
            }
            moveSpeed = 0;
            m_Anim.Play("Henchmandie");
            gravity = 0;
            colli.enabled = false;
            isDetected = false;
            DestroyObject(attackArea);
            DestroyObject(detray);
        }
        else { 

            CalculateVelocity();

            Run();
            controller.Move(velocity * Time.deltaTime, directionalInput);

            if (controller.collisions.above || controller.collisions.below)
            {
                //m_Anim.SetBool("Ground", true);
                velocity.y = 0f;
            }
            else
            {
                //m_Anim.SetBool("Ground", false);
            }
            //m_Anim.SetFloat("vSpeed", velocity.y);

            if(isDetected)
            {
                Patrolling = false;
                Vector3 dir = target.position - me.position;
                dir.z = 0.0f; // Only needed if objects don't share 'z' value
                //Debug.Log("Looking for player");
                moveSpeed = huntSpeed;
                if (dir != Vector3.zero)
                {
                    var tar = new Vector2(0, 0);
                    if(target.position.x - me.position.x < 0)
                    {
                        tar = new Vector2(-5 * moveSpeed * Time.deltaTime, 0);
                    }
                    else
                    {
                        tar = new Vector2(5*moveSpeed * Time.deltaTime, 0);
                    }

                    controller.Move(tar, directionalInput);
                }
                LookForPlayerTimer = LookForPlayerTimer - Time.deltaTime;
                //Debug.Log("Looking Timer: " + LookForPlayerTimer);
                
                if (AttackTimer < 0)
                {

                    attackArea.enabled = true;
                    attacking = false;
                    if (attackArea.IsTouching(playerCollider))
                        attackPlayer();

                }
                else
                {
                    Debug.Log(AttackTimer);
                    attackArea.enabled = false;
                    AttackTimer -= Time.deltaTime;
                }


                if (LookForPlayerTimer < 0)
                {
                    isDetected = false;
                    moveSpeed = OriginalMoveSpeed;
                    Returning = true;
                                    }
            }
            else if(Patrolling)
            {

                if(me.position.x < PatLeftPos.x)
                {
                    moveSpeed = 0f;
                    chilltimer -= Time.deltaTime;
                    if(chilltimer<0)
                    {
                        me.transform.position = PatLeftPos + new Vector3(0.000001f, 0) ;
                        var redirect = new Vector2(-1, 0);
                        directionalInput = Vector2.Scale(directionalInput, redirect);
                        controller.Move(velocity * Time.deltaTime, directionalInput);
                    }
                }
                else if (me.position.x > PatRightPos.x)
                {
                    moveSpeed = 0f;
                    chilltimer -= Time.deltaTime;
                    if (chilltimer < 0)
                    {
                        me.transform.position = PatRightPos - new Vector3(0.000001f, 0);
                        var redirect = new Vector2(-1, 0);
                        directionalInput = Vector2.Scale(directionalInput, redirect);
                        controller.Move(velocity * Time.deltaTime, directionalInput);
                    }
                }
                else
                {
                    chilltimer = chillTime;
                    moveSpeed = Patrollspeed;
                }
            }
            else if(Returning)
            {
                var tar = new Vector2(0,0);
                moveSpeed = 0.6f;
                if(originalPos.x - me.position.x > 0)
                {
                    tar = new Vector2(10*moveSpeed * Time.deltaTime, 0);
                }
                else
                {
                    tar = new Vector2(-10 * moveSpeed * Time.deltaTime, 0);
                }
                
                controller.Move(tar, directionalInput);
                //Debug.Log("Targ " + Mathf.Round(originalPos.x));
                //Debug.Log("Me  " + Mathf.Round(me.position.x));
                if (Mathf.Round(originalPos.x) == Mathf.Round(me.position.x))
                {
                    Returning = false;
                    if (me.position.y != originalPos.y)
                    {
                        PatLeftPos = me.transform.position - new Vector3(PatLeft, 0);
                        PatRightPos = me.transform.position + new Vector3(PatRight, 0);
                        /*Debug.Log("newPos: " + me.transform.position);
                        Debug.Log("newLeft" + PatLeftPos);
                        Debug.Log("newRight" + PatRightPos);*/
                    }
                    Patrolling = true;
                    Debug.Log("Returning " + Returning);
                    Debug.Log("Patrolling " + Patrolling);


                }
                
            }


        }
        //Move Towards Target
        // myTransform.position += myTransform.position * moveSpeed * Time.deltaTime;
    }

    public void SetDirectionalInput(Vector2 input)
    {
        directionalInput = input;
    }

        private void CalculateVelocity()
    {
        float targetVelocityX = directionalInput.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below ? accelerationTimeGrounded : accelerationTimeAirborne));
        velocity.y += gravity * Time.deltaTime;
    }
    private void Run()
    {
        if (velocity.x > 0.01f || velocity.x < -0.01f)
        {
            m_Anim.SetBool("Running", true);
        }
        else
        {
            m_Anim.SetBool("Running", false);
        }
    }
    
    public void Damage(int damage)
    {
        curHealth -= damage;
    }

    public void Detected()
    {
        LookForPlayerTimer = LookForPlayerMax;
        isDetected = true;
        Debug.Log("Detected Player");
    }
    public void attackPlayer()
    {
        attacking = true;
        m_Anim.Play("HenchmanAttack");
        AttackTimer = AttackTimerMax;
    }


}

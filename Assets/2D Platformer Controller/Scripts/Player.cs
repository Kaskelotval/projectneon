using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour
{
    public float maxJumpHeight = 4f;
    public float minJumpHeight = 1f;
    public float timeToJumpApex = .4f;
    public float moveSpeed = 6f;
    public float sprintBoost = 1f;
    private float accelerationTimeAirborne = .2f;
    private float accelerationTimeGrounded = .1f;
    private Animator m_Anim;
    public Camera cam;

    //SCORE//
    public GUIText scoreText;
    public GUIText comboText;
    public GUIText killText;
    public Animator scoreANIM;
    public GameObject death;
    public bool killlimit = false;

    private int kills = 0;
    public int MaxKills;
    public double score;
    public double combo = 1;
    public float comboTimer;
    public float OrgcomboTimer = 5f;
    //for attacks
    private bool attacking = false;
    private float attacktimer = 0;
    private float attackcd = 0.3f;
    public Collider2D attackTrigger;

    //Getting atacked
    private float curHealth = 20;

    public Vector2 wallJumpClimb;
    public Vector2 wallJumpOff;
    public Vector2 wallLeap;

    public bool canDoubleJump;
    private bool isDoubleJumping = false;

    public float wallSlideSpeedMax = 3f;
    public float wallStickTime = .25f;
    private float timeToWallUnstick;

    private float gravity;
    private float maxJumpVelocity;
    private float minJumpVelocity;
    private Vector3 velocity;
    private float velocityXSmoothing;

    private Controller2D controller;

    public float DeathTimer;
    public float DeathTimerMax;

    private Vector2 directionalInput;
    private bool wallSliding;
    private int wallDirX;

    private void Start()
    {
        cam.fieldOfView = 40;
        Time.timeScale = 1.0F;
        death.SetActive(false);
        combo = 1;
        score = 0;
        UpdateScore();
        DeathTimer = DeathTimerMax;
        m_Anim = GetComponent<Animator>();
        attackTrigger.enabled = false;
        controller = GetComponent<Controller2D>();
        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        m_Anim.SetBool("Running", false);
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);

    }

    private void Update()
    {
        if(curHealth <= 0)
        {
            cam.fieldOfView = 20;
            Time.timeScale = 0.2F;
            death.SetActive(true);
            combo = 0;
            UpdateScore();
            if(!controller.collisions.below)
            {
                CalculateVelocity();
                controller.Move(velocity * Time.deltaTime, directionalInput);

            }
            //DestroyObject(attackTrigger);
            m_Anim.Play("MainGuyDeath");
            if (DeathTimer < 0)
            {
                score = 0;
                kills = 0;
            }
            else
                DeathTimer -= Time.deltaTime;
        }
        else
        {
            CalculateVelocity();
            HandleWallSliding();
            
            //m_Anim.SetBool("Attack", false);

            Run();
            controller.Move(velocity * Time.deltaTime, directionalInput);

            if (controller.collisions.above || controller.collisions.below)
            {
                m_Anim.SetBool("Ground", true);
                velocity.y = 0f;
            }
            else
            {
                m_Anim.SetBool("Ground", false);
            }
            m_Anim.SetFloat("vSpeed", velocity.y);

            if (attacking)
            {
                velocity.x = velocity.x * 0.9f;
                if (attacktimer > 0)
                {
                    //Debug.Log(attacktimer);
                    attacktimer -= Time.deltaTime;
                }
                else
                {
                    attacking = false;
                    attackTrigger.enabled = false;
                }
            }
            comboTimer += Time.deltaTime;
            if (comboTimer >= OrgcomboTimer)
            {
                combo = 1;
                UpdateScore();
            }

        }
        
    }

    public void SetDirectionalInput(Vector2 input)
    {
        directionalInput = input;
    }


    public void OnJumpInputDown()
    {
        if (wallSliding)
        {
            if (wallDirX == directionalInput.x)
            {
                velocity.x = -wallDirX * wallJumpClimb.x;
                velocity.y = wallJumpClimb.y;
            }
            else if (directionalInput.x == 0)
            {
                velocity.x = -wallDirX * wallJumpOff.x;
                velocity.y = wallJumpOff.y;
            }
            else
            {
                velocity.x = -wallDirX * wallLeap.x;
                velocity.y = wallLeap.y;
            }
            isDoubleJumping = false;
        }
        if (controller.collisions.below)
        {
            velocity.y = maxJumpVelocity;
            isDoubleJumping = false;
        }
        if (canDoubleJump && !controller.collisions.below && !isDoubleJumping && !wallSliding)
        {
            velocity.y = maxJumpVelocity;
            isDoubleJumping = true;
        }
    }

    public void OnJumpInputUp()
    {
        if (velocity.y > minJumpVelocity)
        {
            velocity.y = minJumpVelocity;
        }
    }

    private void HandleWallSliding()
    {
        wallDirX = (controller.collisions.left) ? -1 : 1;
        wallSliding = false;
        if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && velocity.y < 0)
        {
            wallSliding = true;

            if (velocity.y < -wallSlideSpeedMax)
            {
                velocity.y = -wallSlideSpeedMax;
            }

            if (timeToWallUnstick > 0f)
            {
                velocityXSmoothing = 0f;
                velocity.x = 0f;
                if (directionalInput.x != wallDirX && directionalInput.x != 0f)
                {
                    timeToWallUnstick -= Time.deltaTime;
                }
                else
                {
                    timeToWallUnstick = wallStickTime;
                }
            }
            else
            {
                timeToWallUnstick = wallStickTime;
            }
        }
    }

    private void CalculateVelocity()
    {
        float targetVelocityX = directionalInput.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, sprintBoost*targetVelocityX, ref velocityXSmoothing, (controller.collisions.below ? accelerationTimeGrounded : accelerationTimeAirborne));
        velocity.y += gravity * Time.deltaTime;
    }
    private void Run()
    {
        if (velocity.x > 0.01f || velocity.x < -0.01f)
        {
            //Debug.Log(velocity.x);
            m_Anim.SetBool("Running", true);
        }
        else
        {
            m_Anim.SetBool("Running", false);
            //Debug.Log("Speed" + velocity.x);
        }
    }
    public void OnSprintInputDown()
    {
        m_Anim.SetBool("Sprinting", true);
        sprintBoost = 1.5f;
    }
    public void OnSprintInputUp()
    {
        m_Anim.SetBool("Sprinting", false);
        sprintBoost = 1f;
    }
    public void OnActionInputDown()
    {
        m_Anim.Play("MainGuySmoke");
    }

    public void DamagePlayer(int damage)
    {
        curHealth -= damage;
        Debug.Log("HP: " + curHealth);
    }

    public void OnAttackInputDown()
    {
        if (!attacking)
        {
            attacking = true;
            attacktimer = attackcd;
            attackTrigger.enabled = true;
            if (velocity.y > 0)
                m_Anim.Play("MainGuyAttackJump");
            else
                m_Anim.Play("MainGuyAttack");
        }

    }
    public void addScore(double addV)
    {
        if (comboTimer <= OrgcomboTimer)
            combo += 0.5;
        else
            combo = 1;

        kills++;
        comboTimer = 0;
        score += addV*combo;
        scoreANIM.Play("ScoreBounce");
        UpdateScore();

        if (kills == MaxKills && killlimit)
        {
            int i = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(i + 1);

        }



    }
    void UpdateScore()
    {
        killText.text = "Kills: " + kills;
        scoreText.text = "Score: " + score;
        if (combo > 1)
            comboText.text = "Multiplier: " + combo;
        else
            comboText.text = "";
    }
    public void RestartScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

}

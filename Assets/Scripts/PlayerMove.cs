using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour

{
    public float speed;
    public float jumpHeight;
    public float gravityMultiplier;
    public float afterjumpVelocity;
    public float speakingNumber;
    public Transform spawnPoint;
    public Transform winPoint;

    private float speedValue;


    bool onFloor;
    bool onWall;
    bool onNPC1;
    bool onNPC2;
    bool onNPC3;
    bool onNPC4;


    Rigidbody2D myBody;
    SpriteRenderer myRenderer;

    public Animator anim;
    public Animator animFlag;
    public Sprite jumpSprite;
    public Sprite walkSprite;
    public Sprite wallSprite;
    public Sprite newSprite;
    public Sprite newSign;

    public Dialogue cannotpassDialogue;
    public Dialogue youwinDialogue;

    public static bool faceRight = true;

    // Start is called before the first frame update
    void Start()
    {
        anim.SetBool("Walking", false);
        animFlag.SetBool("Winning", false);
        myBody = gameObject.GetComponent<Rigidbody2D>();
        myRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {


        if (onFloor && myBody.velocity.y != 0)
        {
            onFloor = false;
        }


        if (onFloor)
        {
            onWall = false;
        }

        if (onWall)
        {
            onFloor = false;
        }

        if (onWall && myBody.velocity.y != 0)
        {
            onFloor = false;
            anim.SetBool("On Wall", true);
            anim.SetBool("Jumping", false);
            anim.SetBool("Walking", false);
        }

        if (onFloor && myBody.velocity.x == 0)
        {
            anim.SetBool("Walking", false);
        }

        if (myBody.velocity.y != 0)
        {
            anim.SetBool("Walking", false);
        }


        CheckKeys();
        JumpPhysics();

    }

    void CheckKeys()
    {
        if (Input.GetKey(KeyCode.D))
        {
            if (onFloor)
            {
                anim.SetBool("Jumping", false);
                anim.SetBool("On Wall", false);
                anim.SetBool("Walking", true);
            }
            faceRight = true;
            myRenderer.flipX = false;
            HandleLRMovement(speed);
            speedValue = 1;
            //myBody.velocity += Vector2.right * Physics2D.gravity * (accelerationMultiplier - 1f) * Time.deltaTime;


        } else if (Input.GetKey(KeyCode.A))
        {
            if (onFloor)
            {
                anim.SetBool("Jumping", false);
                anim.SetBool("On Wall", false);
                anim.SetBool("Walking", true);
            }
            faceRight = false;
            myRenderer.flipX = true;
            HandleLRMovement(-speed);
            speedValue = -1;
            //myBody.velocity += Vector2.left * -Physics2D.gravity * (accelerationMultiplier - 1f) * Time.deltaTime;

        }

        if (Input.GetKeyDown(KeyCode.W) && onFloor)
        {


            anim.SetBool("Walking", false);
            anim.SetBool("Jumping", true);
            myBody.velocity = new Vector3(myBody.velocity.x, jumpHeight);

        }

        if (Input.GetKeyDown(KeyCode.W) && onWall)
        {
            anim.SetBool("Walking", false);
            anim.SetBool("Jumping", false);
            anim.SetBool("On Wall", true);
            myBody.velocity = new Vector3(myBody.velocity.x, jumpHeight);

        }

        if (Input.GetKeyDown(KeyCode.Q) && onNPC1)
        {
            GameObject.Find("NPC").GetComponent<NPC>().TriggerDialogue();
            speakingNumber += 1;

        }

        if (Input.GetKeyDown(KeyCode.Q) && onNPC2)
        {
            GameObject.Find("NPC2").GetComponent<NPC>().TriggerDialogue();
            speakingNumber += 1;

        }
        if (Input.GetKeyDown(KeyCode.Q) && onNPC3)
        {
            GameObject.Find("NPC3").GetComponent<NPC>().TriggerDialogue();
            speakingNumber += 1;

        }
        if (Input.GetKeyDown(KeyCode.Q) && onNPC4)
        {
            speakingNumber += 1;
            if (speakingNumber >= 4)
            {
                GameObject.Find("NPC4").GetComponent<NPC>().dialogue = youwinDialogue;
                GameObject.Find("NPC4").GetComponent<NPC>().TriggerDialogue();
            }
            else
            {
                GameObject.Find("NPC4").GetComponent<NPC>().dialogue = cannotpassDialogue;
                GameObject.Find("NPC4").GetComponent<NPC>().TriggerDialogue();
            }
            
            

        }

        if (Input.GetKeyDown(KeyCode.E) && (onNPC1 || onNPC2 || onNPC3 || onNPC4))
        {
            FindObjectOfType<DialogueManager>().DisplayNextSentence();
        }

        else if (!Input.GetKeyDown(KeyCode.W) && !onFloor)
        {
            anim.SetBool("Jumping", true);
            myBody.velocity += Vector2.up * (Physics2D.gravity.y) / 4 * (jumpHeight - 1f) * Time.deltaTime;
        }
    }

    void JumpPhysics()
    {
        if(myBody.velocity.y < 0)
        {
            if (!onWall)
            {
                anim.SetBool("Jumping", true);
            }
            myBody.velocity += Vector2.up * Physics2D.gravity.y * (gravityMultiplier - 1f) * Time.deltaTime;
        } 
    }

    void HandleLRMovement(float dir)
    {
        myBody.velocity = new Vector3(dir, myBody.velocity.y);
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "NPC")
        {
            onNPC1 = true;
            onNPC2 = false;
            onNPC3 = false;
            onNPC4 = false;
        }
        if (collision.gameObject.name == "NPC2")
        {
            onNPC1 = false;
            onNPC2 = true;
            onNPC3 = false;
            onNPC4 = false;
        }
        if (collision.gameObject.name == "NPC3")
        {
            onNPC1 = false;
            onNPC2 = false;
            onNPC3 = true;
            onNPC4 = false;
        }
        if (collision.gameObject.name == "NPC4")
        {
            onNPC1 = false;
            onNPC2 = false;
            onNPC3 = false;
            onNPC4 = true;
        }

        if (collision.gameObject.tag == "enemy")
        {
            gameObject.transform.position = spawnPoint.transform.position;
            Camera.main.transform.position = spawnPoint.transform.position;
            GetComponent<ScreenChange>().newcamPos = -8;
            myBody.velocity = new Vector3(0, 0);
            FindObjectOfType<DialogueManager>().EndDialogue();
        }

        if (collision.gameObject.name == "Ending")
        {
            if (speakingNumber >= 4)
            {
                animFlag.SetBool("Winning", true);
                GameObject.Find("tutorialsign").GetComponent<SpriteRenderer>().sprite = newSign;


            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "floor")
        {
            anim.SetBool("Jumping", false);
            anim.SetBool("On Wall", false);
            onFloor = true;
            myBody.velocity = new Vector2(speedValue / 2 * afterjumpVelocity, myBody.velocity.y);

        }

        if(collision.gameObject.tag == "wall")
        {
            anim.SetBool("On Wall", true);
            anim.SetBool("Walking", false);
            anim.SetBool("Jumping", false);
            onWall = true;
        }

       

    }
}

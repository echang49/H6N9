using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {

    [SerializeField] float      m_speed = 1.0f;
    [SerializeField] float      m_jumpForce = 2.0f;

    private Animator            _animator;
    private Rigidbody2D         _playerRigidBody;

    public static float health = 3; //beginning health of the game
    public static float healthReference = health;
    public float newHealth; //health after points have been accounted for
    public int score = 0;

    public bool key;

    public GameObject shield;
    public GameObject prefabShield;
    // Use this for initialization
    void Start () {
        _animator = GetComponent<Animator>();
        _playerRigidBody = GetComponent<Rigidbody2D>();
        key = false;
    }
	
	// Update is called once per frame
	void Update () {
        if(transform.position.y < -20){
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        // Handle input
        float inputX = Input.GetAxis("Horizontal");

        // flip the sprite if the player inputs another direction
        if (inputX > 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (inputX < 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }


        // Move
        _playerRigidBody.velocity = new Vector2(inputX * m_speed, _playerRigidBody.velocity.y);

        

        //Animations
        _animator.SetInteger("AnimState", 0);
   

        //Jump if player hits space
        if (Input.GetKeyDown("space") && _playerRigidBody.velocity.y==0) {
       
            _playerRigidBody.velocity = new Vector2(_playerRigidBody.velocity.x, m_jumpForce);

        }  //Run
        else if (Mathf.Abs(inputX) > Mathf.Epsilon)
        {
            _animator.SetInteger("AnimState", 2);
        }
        else  //if no other animation  then the player is idling
        {
           _animator.SetInteger("AnimState", 0);
        }


        //player progression
        newHealth = health;

        //player changes in health and damage done when certain points are achieved
       /*if (score == 5) {

               newHealth =+ 1;

        } else if (score == 10) {

                PlayerAttack.damage = +1;

        } else if (score == 20) {

            //tempHealth = -1;

        } else if (score == 30) {

            PlayerAttack.damage = -1;

        }*/
    }

   
    //collision event
    private void OnTriggerEnter2D(Collider2D other)
    {   
        //if hit by enemy lower health and use hurt animation
        if(other.gameObject.tag == "enemy" )
        {
              //if player does not have shield 
            if (shield.transform.position.y >= 20000)
            {
                _animator.SetTrigger("Hurt");
                health += -1;

                if (health <= 0)
                {
                    health = healthReference; //dies so the character restarts with same hp
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }

            }
            else //he has shield so no health is lost but we destroy the shield
            {
                shield.transform.position = new Vector2(shield.transform.position.x,20000);
            }
            

        }
        if (other.gameObject.tag == "Coin"){ //collides with coin
            score++;  //add score
            Destroy(other.gameObject); //destroy the coin

        }
        if (other.gameObject.tag == "Potion"){
            health++; //add to health
            Destroy(other.gameObject); //destory the potion
        }

        //if if hit flag we win
        if (other.gameObject.tag == "Respawn")
        {
//<<<<<<< Updated upstream
            //change next scene based off score of previous scene

            //if current scene is level 1
            if (SceneManager.GetActiveScene().name == "LevelOneScene")
            {
                if (score>=5 && score<10)
                {
                    healthReference += 1;
                }
                if (score>10)
                {
                    PlayerAttack.damage += 1;
                }
            }

            //if current scene is level 2
            if (SceneManager.GetActiveScene().name == "LevelTwoScene")
            {
                if (score>=20 && score<30)
                {
                    if (healthReference > 1)
                    {
                        healthReference += -1;
                    }
                }
                if (score>30)
                {
                    if(PlayerAttack.damage > 0)
                    {
                        PlayerAttack.damage += -1;
                    }
                }
            }
            health = healthReference;
            //create reference variable for starting health
            
//=======
            //CREATE A REFERENCE VARIABLE FOR BEGINNING HP
//>>>>>>> Stashed changes

            //if in level 1 go here
                //if score is this,
            //if in level 2 go here
            SceneManager.LoadScene("LevelTwoScene", LoadSceneMode.Single);

        }


        //if hit by enemy projectile lower health and use hurt animation
        if (other.gameObject.tag == "projectile")
        {   
            //destory the projectile immediately
            Destroy(other.gameObject);
            //if player does not have shield 
            if (shield.transform.position.y >= 20000)
            {
                Debug.Log("hit by enemy");
                _animator.SetTrigger("Hurt");
                health += -1;

            
                 //if less than 0 health restart game
                if (health <= 0)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }

            }
            else //he has shield so no health is lost but we destroy the shield
            {
               shield.transform.position = new Vector2(shield.transform.position.x,20000);
            }

        }
        if(other.gameObject.tag == "shieldpotion"){
            if(shield.transform.position.y >= 20000){
                shield.transform.position = new Vector2(transform.position.x,transform.position.y);
                Destroy(other.gameObject);
            }else{
                Destroy(other.gameObject);
            }

        }

    }


    void OnCollisionEnter2D(Collision2D col)
    {   
        if (col.gameObject.tag == "Coin"){ //collides with coin
            score++;  //add score
            Destroy(col.gameObject); //destroy the coin

        }
        //if its a projectile 
        if (col.gameObject.tag == "projectile")
        {

            //destory the projectile immediately
            Destroy(col.gameObject);
            //if player does not have shield lower health
            if (shield.transform.position.y >= 20000)
            {
                _animator.SetTrigger("Hurt"); //set animation to hurt
                health += -1;                 //lower health


                //if less than 0 health restart game
                if (health <= 0)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }

            }
            else //he has shield so no health is lost but move the shield
            {
                shield.transform.position = new Vector2(shield.transform.position.x,20000);
            }

        }

        //if its a key 
        if(col.gameObject.tag == "key")
        {   
            //player know has key so set true
            key = true;
            Destroy(col.gameObject); //destroy the key once collided
        }
    }
}

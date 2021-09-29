using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
using System.IO;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody rb;
    private CapsuleCollider playerCollider;

    public float jumpScale = 5;
    public float movementScale;     //Note, movementScale is being set in FixedUpdate, depending on if ctrl is held down

    //Default value, the user can jump 2 times before they land
    public int maxJumps = 2;
    public int remainingJumps;

    //Used in some powerups, the player may be invulnerable and this bool is how you check
    public bool isInvulnerable = false;

    //Powerups
    public bool hasShield = false;
    public int numTeleClicks = 0;
    public bool hasLaserClick = false;

    //The # of hits the player can take before death
    public int maxHealth = 3;
    public int health; //Current health

    //List of materials
    public Material defaultMaterial, shieldMaterial, teleClickMaterial, laserClickMaterial, invulnerabilityMaterial, tripleJumpMaterial;

    LineRenderer line;

    private float deathLoadDelay = 5f;

    public bool isDead = false;

    //For playing sounds
    AudioSource audioSource;
    public AudioClip jumpSound, collectablePickupSound, damageSound, deathSong, healthPickupSound, laserFiringSound, powerupLossSound, powerupPickupSound, shieldBreakSound, teleportSound;

    //The last touched platform. Need this to see if the user fell off the map
    private float lastPlatformY;

    private int collectableCounter;

    public int curLevel;

    public int numEnemiesKilled;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerCollider = GetComponent<CapsuleCollider>();

        remainingJumps = maxJumps;
        health = maxHealth;

        line = GetComponent<LineRenderer>();
        line.useWorldSpace = true;
        line.positionCount = 2;
        line.enabled = false;

        jumpScale = 5;

        audioSource = GetComponent<AudioSource>();

        lastPlatformY = GameObject.FindGameObjectWithTag("Platform").transform.position.y;


        //Set the counters to the values in the csv file

        string filePath = "Assets/Resources/counters.csv";

        //Need to read the current counter data
        StreamReader reader = new StreamReader(filePath);
        collectableCounter = int.Parse(reader.ReadLine());

        reader.Close();

        filePath = "Assets/Resources/lastLevel.csv";
        reader = new StreamReader(filePath);
        curLevel = int.Parse(reader.ReadLine());

        reader.Close();

        Debug.Log("Read levelNum from file: " + curLevel);

        numEnemiesKilled = 0;
    }

    private void Update()
    {
        if (!isDead)
        {
            //Check if the user is trying to jump
            if (Input.GetKeyDown(KeyCode.Space) && remainingJumps > 0)
            {
                audioSource.PlayOneShot(jumpSound);
                rb.AddForce(Vector3.up * jumpScale, ForceMode.Impulse);
                remainingJumps--;
            }
            if (Input.GetMouseButtonDown(0))
            {
                //Check if the user has the teleport powerup remaining, aka numTPClicks > 0. They're trying to teleport
                if (numTeleClicks > 0)
                {
                    audioSource.PlayOneShot(teleportSound);
                    Vector3 teleportPosition = Input.mousePosition;
                    teleportPosition.x = Camera.main.ScreenToWorldPoint(teleportPosition).x;
                    teleportPosition.y = Camera.main.ScreenToWorldPoint(teleportPosition).y;

                    //Move them to their click
                    transform.position = teleportPosition;

                    //Replenish their jumps
                    remainingJumps = 1;

                    //Decrement amount of remaining clicks
                    numTeleClicks--;

                    if (numTeleClicks <= 0)
                    {
                        GetComponent<Renderer>().material = defaultMaterial;
                    }

                    //Give 2 seconds invulnerability
                    StartCoroutine(TeleClickInvulnerabilityCoroutine());
                }
                //For the laser click powerup
                if (hasLaserClick)
                {
                    Vector3 mouseClickPosition = Input.mousePosition;
                    mouseClickPosition.x = Camera.main.ScreenToWorldPoint(mouseClickPosition).x;
                    mouseClickPosition.y = Camera.main.ScreenToWorldPoint(mouseClickPosition).y;

                    //The user clicked so fire laser
                    if (Input.GetMouseButton(0))
                    {
                        audioSource.PlayOneShot(laserFiringSound);
                        //The direction that the ray is being sent on (outward from the player toward the mouse click
                        Vector3 direction = new Vector3(mouseClickPosition.x - transform.position.x, mouseClickPosition.y - transform.position.y, 0);
                        //The actual ray that will be used as the damage dealer
                        Ray ray = new Ray(transform.position, direction);

                        //Every collider that is hit by this ray
                        RaycastHit[] hits = Physics.RaycastAll(ray);

                        //Slope, used to calculate an extended version of the laser, ensuring it goes to the edge of the screen
                        float slope = (mouseClickPosition.y - transform.position.y) / (mouseClickPosition.x - transform.position.x);
                        float yInt = transform.position.y - (slope * transform.position.x);
                        Vector3 lineEndPoint;
                        //Shooting to right
                        if (mouseClickPosition.x > transform.position.x)
                        {
                            lineEndPoint = new Vector3(transform.position.x + 20, (slope * (transform.position.x + 20)) + yInt, 0);
                        }
                        //Shooting to left
                        else
                        {
                            lineEndPoint = new Vector3(transform.position.x - 20, (slope * (transform.position.x - 20)) + yInt, 0);
                        }


                        //Need to display the laser on the screen
                        line.enabled = true;
                        line.SetPosition(0, transform.position);
                        line.SetPosition(1, lineEndPoint);

                        for (int i = 0; i < hits.Length; i++)
                        {
                            //If the laser collided with any of the enemy tags
                            if (hits[i].collider.gameObject.tag.Equals("Enemy1") || hits[i].collider.gameObject.tag.Equals("Enemy2") || hits[i].collider.gameObject.tag.Equals("Enemy3") || hits[i].collider.gameObject.tag.Equals("Enemy4") || hits[i].collider.gameObject.tag.Equals("Enemy5"))
                            {
                                //Destroy all enemy objects that touched the ray
                                Destroy(hits[i].collider.gameObject);
                                numEnemiesKilled++;
                            }
                        }

                        //Need to do something to have the line shrink over time

                        StartCoroutine(ShrinkLaserCoroutine());
                    };


                    //Remove the powerup
                    hasLaserClick = false;
                    GetComponent<Renderer>().material = defaultMaterial;
                }
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isDead)
        {
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))    //-x
            {
                rb.AddForce(Vector3.left * movementScale, ForceMode.Force);
            }
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))   //+x
            {
                rb.AddForce(Vector3.right * movementScale, ForceMode.Force);
            }

            //Sprint control
            if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))   //Sprint
            {
                movementScale = 20;
            }
            //Reset speed if they're not holding sprint
            else
            {
                movementScale = 10;
            }
        }

        //Check if the player is more than 50 units away from the last platform on the y. If so, they fell off the map
        if(!isDead && transform.position.y <= lastPlatformY - 20)
        {
            KillPlayer();
        }
    }

    //Different things happen depending on what the collision is with. 
    private void OnCollisionEnter(Collision collisionInfo)
    {
        GameObject collidedObject = collisionInfo.gameObject;

        //If the character collided with an object with the tag "Platforms", all platforms, then give them their total jumps
        if (collidedObject.tag.Equals("Platform") || collidedObject.tag.Equals("PlatformCollect"))
        {
            remainingJumps = maxJumps;
            lastPlatformY = collidedObject.transform.position.y;
        }

        // This is being taken care of in the enemy scripts
        //The PC collided with an object with the tag saved in whatever is stored in enemyTagName. Note: The player does not currently take damage for the duration of them touching the enemy. There should be a greater bounce off of the enemy on touch
        for (int i = 1; i <= 5; i++)
        {
            //The PC collided with an object with the tag saved in whatever is stored in enemyTagName. Note: The player does not currently take damage for the duration of them touching the enemy. There should be a greater bounce off of the enemy on touch
            if (collidedObject.tag.Equals("Enemy" + i))
            {
                //Set the #jumps to 1 on collision with an enemy. Need to make sure they can't get stuck and can jump off
                remainingJumps = maxJumps - 1;
                //Enemy
                Collider enemyCollider = collidedObject.GetComponent<Collider>();

                //Need to determine if the enemy will die or if the player will die

                //Data for damage determination
                float playerCenterX = playerCollider.bounds.center.x;       //The x value for the center of the player
                float enemyMinX = enemyCollider.bounds.min.x;               //The smallest x value for the enemy that is being collided with (ie the left edge)
                float enemyMaxX = enemyCollider.bounds.max.x;               //The highest x value for the enemy that is being collided with (ie the right edge)
                                                                            //Check if the center.x of the player is between the min and max x of the enemy. This means the enemy dies
                //Enemy killed
                if (playerCenterX >= enemyMinX && playerCenterX <= enemyMaxX)
                {
                    numEnemiesKilled++;
                    //Need to destroy the enemy
                    Destroy(collidedObject);
                }



                //Player should take damage
                else
                {
                    //Make sure the user is not invulnerable before applying damage
                    if (!isInvulnerable)
                    {
                        //Check if the user has a shield
                        if (hasShield)
                        {
                            //Don't take damage
                            hasShield = false;
                            GetComponent<Renderer>().material = defaultMaterial;
                        }
                        else
                        {
                            //They took damage from enemy
                            RemoveHealth(1);
                        }
                    }
                }
            }
        }

        //The player collided with a shield powerup. 
        if (collidedObject.tag.Equals("ShieldPowerup"))
        {
            RemovePowerups();
            GiveShield();
            Destroy(collidedObject);
        }
        //The player collided with the TP Powerup
        if (collidedObject.tag.Equals("TeleClickPowerup"))
        {
            RemovePowerups();
            GiveTeleClick();
            Destroy(collidedObject);
        }
        //The player collided with the Laser Click Powerup
        if (collidedObject.tag.Equals("LaserClickPowerup"))
        {
            RemovePowerups();
            GiveLaserClick();
            Destroy(collidedObject);
        }
        //The player collided with the Invulnerability Powerup
        if (collidedObject.tag.Equals("InvulnerabilityPowerup"))
        {
            RemovePowerups();
            GiveInvulnerability();
            Destroy(collidedObject);
        }
        //The player collided with the Triple Jump Powerup
        if (collidedObject.tag.Equals("TripleJumpPowerup"))
        {
            RemovePowerups();
            GiveTripleJump();
            Destroy(collidedObject);
        }
        //The player collided with a health pickup, heal them
        if (collidedObject.tag.Equals("HealthPickup"))
        {
            GiveHeal();
            Destroy(collidedObject);
        }

        //The player collided with a collectable
        if (collidedObject.tag.Equals("Collectable"))
        {
            audioSource.PlayOneShot(collectablePickupSound);
        }
    }

    //Collision exit is handled differently depending on what the collided object is
    private void OnCollisionExit(Collision collision)
    {
        GameObject collidedObject = collision.gameObject;

        //Remove a jump on collision exit with the platform
        if (collidedObject.tag.Equals("Platform") || collidedObject.tag.Equals("PlatformCollect"))
        {
            lastPlatformY = collidedObject.transform.position.y;
            remainingJumps = maxJumps - 1; //This will make the program easy to change regardless of how many max jumps they have
        }
    }

    //This function will contain everything that needs to be done when the player is going to die (ie destroy object, reset level)
    public void KillPlayer()
    {
        //First, stop the level music
        GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");

        AudioSource cameraAudioSource = camera.GetComponent<AudioSource>();

        cameraAudioSource.Stop();

        if (!isDead)
        {
            //Play death song
            audioSource.PlayOneShot(deathSong);

            isDead = true;

            //Shrink the player
            StartCoroutine(ShrinkPlayer());

            //Load the fail scene
            StartCoroutine(LoadLevelAfterDelay(deathLoadDelay));
        }
    }

    IEnumerator ShrinkPlayer()
    {
        while (playerCollider.transform.localScale.y >= 0.1)
        {
            yield return new WaitForSeconds(0.1f);
            playerCollider.transform.localScale = new Vector3(playerCollider.transform.localScale.x, playerCollider.transform.localScale.y - 0.1f, playerCollider.transform.localScale.z);
        }
    }

    IEnumerator LoadLevelAfterDelay(float delay)
    {
        //loads the scene 
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("levelF");

    }

    //This should be called first on collection of a powerup, to ensure the user doesn't have any other powerups that will interfere
    public void RemovePowerups()
    {
        //Remove shields
        hasShield = false;

        //Remove TeleClick
        numTeleClicks = 0;

        //Remove laser
        hasLaserClick = false;

        //Remove p4
        isInvulnerable = false;

        //Remove p5
        maxJumps = 2;
        //If they currently have 3 jumps bring them back down
        if(remainingJumps == 3)
        {
            remainingJumps = maxJumps;

        }
    }


    //Called when the user gets the shield powerup
    public void GiveShield()
    {
        //Play the sound
        audioSource.PlayOneShot(powerupPickupSound);
        //For the computer to know the player shouldn't take damage
        hasShield = true;

        //Change the colour of the player to signify they have a shield
        GetComponent<Renderer>().material = shieldMaterial;
    }

    //Called when the user gets the teleclick powerup
    public void GiveTeleClick()
    {
        //Play the sound
        audioSource.PlayOneShot(powerupPickupSound);
        //Number of remaining teleports
        numTeleClicks = 5;

        //Change the colour of the player to signify they have the teleclicks
        GetComponent<Renderer>().material = teleClickMaterial;
    }

    //Called when user gets the laser click powerup
    public void GiveLaserClick()
    {
        //Play the sound
        audioSource.PlayOneShot(powerupPickupSound);
        
        //Player can shoot a laser
        hasLaserClick = true;

        GetComponent<Renderer>().material = laserClickMaterial;

        //The player can only use this for 30 seconds, then it disappears
        StartCoroutine(LaserClickCoroutine());
    }

    //Called when the user gets the invulnerability powerup
    public void GiveInvulnerability()
    {
        //Play the sound
        audioSource.PlayOneShot(powerupPickupSound);
        //Player is invulnerable for 10 seconds now
        isInvulnerable = true;

        GetComponent<Renderer>().material = invulnerabilityMaterial;

        //The player gets 10 seconds via this coroutine
        StartCoroutine(InvulnerabilityPowerupCoroutine());
    }

    //Called when they pickup the powerup, this only lasts for the time listed in the coroutine
    public void GiveTripleJump()
    {
        //Play the sound
        audioSource.PlayOneShot(powerupPickupSound);
        //Increase maxJumps to 3
        maxJumps = 3;

        //Give material
        GetComponent<Renderer>().material = tripleJumpMaterial;

        //Set the timer
        StartCoroutine(TripleJumpTimerCoroutine());
    }

    //Called when the player picks up the heal pickup
    public void GiveHeal()
    {
        //Play the sound
        audioSource.PlayOneShot(healthPickupSound);

        health = maxHealth;
    }

    //Access methods for player health, checks if they're dead
    public void RemoveHealth(int amount)
    {
        //Play damage sound
        audioSource.PlayOneShot(damageSound);

        health -= amount;

        if(health <= 0)
        {
            KillPlayer();
        }
    }


    //Give the player 2 seconds of invulnerability
    IEnumerator TeleClickInvulnerabilityCoroutine()
    {
        isInvulnerable = true;
        yield return new WaitForSeconds(2);
        isInvulnerable = false;
    }

    //This gives the user 30 seconds before they lose the powerup
    IEnumerator LaserClickCoroutine()
    {
        yield return new WaitForSeconds(30);
        //If the user still had their powerup after the 30 seconds then you're actually removing the power. So play the song
        if (hasLaserClick)
        {
            audioSource.PlayOneShot(powerupLossSound);
            hasLaserClick = false;
            GetComponent<Renderer>().material = defaultMaterial;
        }
    }

    //Slowly decrease the size of the line until it is 0 and then disable it
    IEnumerator ShrinkLaserCoroutine()
    {
        while (line.widthMultiplier >= 0)
        {
            yield return new WaitForSeconds(0.1f);
            line.widthMultiplier -= 0.1f;
        }
        line.enabled = false;
    }

    //Rainbow power that makes them invulnerable for 10 seconds
    IEnumerator InvulnerabilityPowerupCoroutine()
    {
        yield return new WaitForSeconds(10);
        if (isInvulnerable)
        {
            isInvulnerable = false;

            //Play powerup loss sound
            audioSource.PlayOneShot(powerupLossSound);

            GetComponent<Renderer>().material = defaultMaterial;
        }
    }

    //Give them 3 jumps for 10 seconds
    IEnumerator TripleJumpTimerCoroutine()
    {
        yield return new WaitForSeconds(10);

        if(maxJumps == 3)
        {
            //Play powerup loss sound
            audioSource.PlayOneShot(powerupLossSound);

            maxJumps = 2;
            remainingJumps = maxJumps;
            GetComponent<Renderer>().material = defaultMaterial;

        }
    }

    public void PlaySoundOneShot(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    public void IncrementCollectablesCollected()
    {
        collectableCounter++;
    }

    public void WriteCountersToFile()
    {
        string filePath = "Assets/Resources/counters.csv";

        StreamWriter writer = new StreamWriter(filePath);

        //The CSV files will have the data in this order:
        writer.WriteLine(collectableCounter);

        writer.Close();
    }

    public void WriteLevelNumToFile(int levelNum)
    {
        string filePath = "Assets/Resources/lastLevel.csv";

        StreamWriter writer = new StreamWriter(filePath);

        writer.WriteLine(levelNum);
        writer.Close();
    }
    public int GetCollectableCounter()
    {
        return collectableCounter;
    }
}

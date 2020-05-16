using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject thiccTextObject;
    [SerializeField] private GameObject scoreTextObject;
    [SerializeField] private GameObject gameOver;
    [SerializeField] private Text gameOverText;
    [SerializeField] private ParticleSystem deathParticleSystem;
    [SerializeField] private RuntimeAnimatorController sticController;
    [SerializeField] private RuntimeAnimatorController thicController;
    [SerializeField] private RuntimeAnimatorController thiccController;
    [SerializeField] private Sprite sticSprite;
    [SerializeField] private Sprite thicSprite;
    [SerializeField] private Sprite thiccSprite;
    [SerializeField] private AudioSource bgm;
    [SerializeField] private GameObject deathSound;
    [SerializeField] private GameObject villian;
    [SerializeField] private Camera camera;
    [SerializeField] private GameObject boostParticleSystem;

    [SerializeField] private bool debugMode;

    private Text thiccText;
    private Text scoreText;

    private int thiccLevel;
    private Rigidbody2D rigidBody;
    private PlayerMovement playerMovement;

    // Animation
    private SpriteRenderer renderer;
    private Animator animator;
    public const int THIC_THRESHOLD = 1000;
    public const int THICC_THRESHOLD = 3000;

    // Score
    public static int totalScore;
    private int scoreByFood;
    private float maxX;

    private bool isGameOver;

    private bool isVillianSpawned;

    private float coffeeBoostTimer;
    private bool isCoffeeBoosted;
    private const float COFFEE_BOOST_TIME = 5f;

    private const float MIN_MASS = 0.4f;
    private const float MAX_MASS = 1.5f;
    private const float MASS_PER_THICC = 0.00005f;
    private const float SPEED_PER_THICC = 0.0005f;
    private const float JUMP_FORCE_PER_THICC = 0.03f;
    private const float DISTANCE_PER_SCORE = 1f;
    private const int THICC_PER_LETTER = 1000;
    private const float PITCH_PER_THICC = 0.0001f;
    private const float IDLE_VEL_THRESHOLD = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        thiccText = thiccTextObject.GetComponent<Text>();
        scoreText = scoreTextObject.GetComponent<Text>();
        rigidBody = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<PlayerMovement>();
        renderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Only update stuff if not game over
        if (!isGameOver)
        {
            updateScore();
            checkIdle();

            // Update coffee boost timer
            if (isCoffeeBoosted)
            {
                coffeeBoostTimer -= Time.deltaTime;

                if (coffeeBoostTimer < 0)
                {
                    // Toggle flag
                    isCoffeeBoosted = false;

                    // Deactivate boost particle system
                    boostParticleSystem.SetActive(false);
                }
            }
        }
        else if (renderer.bounds.max.y < ScreenUtility.getDownEdge(camera))
        {
            // Destroy player once off screen
            Destroy(gameObject);
        }

        // Debug for spawning villian manually
        if (debugMode && Input.GetKeyDown(KeyCode.V))
        {
            float spawnX = ScreenUtility.getXRightOffscreen(villian.GetComponent<SpriteRenderer>(), camera);
            villian.transform.position = new Vector3(spawnX, villian.transform.position.y);
            villian.SetActive(true);
        }
    }

    public int getThiccLevel()
    {
        return thiccLevel;
    }

    public void addThicc(int thicc)
    {
        // Increase/decrease thicc level depending on food item
        thiccLevel += thicc;

        // Update mass
        rigidBody.mass += thicc * MASS_PER_THICC;
        rigidBody.mass = Mathf.Clamp(rigidBody.mass, MIN_MASS, MAX_MASS);

        // Update max speed and jump force
        playerMovement.updateMaxSpeed(-thicc * SPEED_PER_THICC);

        // More thicc, need more jump force to make jumps possible
        playerMovement.updateJumpForce(thicc * JUMP_FORCE_PER_THICC);

        updateThicc();
        updateAnimation();
        updateBGM();
    }

    private void updateThicc()
    {
        int numExtraLetters = Mathf.Abs(thiccLevel / THICC_PER_LETTER);
        // If thic, reduce by one 'c' in order to start at 'thic' instead of 'thicc'
        if (thiccLevel >= THICC_PER_LETTER)
        {
            numExtraLetters--;
        }
        string newText = thiccLevel < THIC_THRESHOLD ? "stic" : "thic";

        for (int i = 0; i < numExtraLetters; i++)
        {
            newText += "c";
        }

        thiccText.text = newText;
    }

    private void updateAnimation()
    {
        if (thiccLevel < THIC_THRESHOLD) // Stic
        {
            animator.runtimeAnimatorController = sticController;
        }
        else if (thiccLevel < THICC_THRESHOLD) // Thic
        {
            animator.runtimeAnimatorController = thicController;
        }
        else // Thicc+
        {
            animator.runtimeAnimatorController = thiccController;

            // Spawn villian if not already
            if (!isVillianSpawned)
            {
                isVillianSpawned = true;

                // Spawn the villian
                float spawnX = ScreenUtility.getXRightOffscreen(villian.GetComponent<SpriteRenderer>(), camera);
                villian.transform.position = new Vector3(spawnX, villian.transform.position.y);
                villian.SetActive(true);
            }
        }
    }

    private void updateBGM()
    {
        bgm.pitch = Mathf.Max(.25f, 1f - thiccLevel * PITCH_PER_THICC);
    }

    public void addFoodScore(int foodScore)
    {
        scoreByFood += foodScore;
        updateScore();
    }

    private void updateScore()
    {
        // Score is a function of distance and food eaten
        if (transform.position.x > maxX)
        {
            maxX = transform.position.x;
            int scoreByDistance = (int) (maxX * DISTANCE_PER_SCORE);
            totalScore = scoreByDistance + scoreByFood;
            scoreText.text = totalScore.ToString();
        }
    }

    private void checkIdle()
    {
        bool isIdle = Mathf.Abs(rigidBody.velocity.x) < IDLE_VEL_THRESHOLD;

        if (isIdle && animator.enabled) // Set to idle
        {
            animator.enabled = false;

            if (thiccLevel < THIC_THRESHOLD) // Stic
            {
                renderer.sprite = sticSprite;
            }
            else if (thiccLevel < THICC_THRESHOLD) // Thic
            {
                renderer.sprite = thicSprite;
            }
            else // Thicc+
            {
                renderer.sprite = thiccSprite;
            }
        }
        else if (!isIdle && !animator.enabled) // Set to moving
        {
            animator.enabled = true;
            // Animator is already updated by updateAnimation()
        }
    }

    public void setGameOver()
    {
        // Toggle game over flag
        isGameOver = true;

        // Deactivate score and thicc text
        scoreTextObject.SetActive(false);
        thiccTextObject.SetActive(false);

        // Update high score
        int highscore = MyGameManager.updateHighScore(totalScore);

        // Set GameOver object active
        gameOver.SetActive(true);

        // Update GameOver text
        gameOverText.text = "Game Over!\nHighscore: " + highscore + "\nScore: " + totalScore + "\nYou were " + thiccText.text + "!\nPress 'R' to replay!";

        // Create death particle system and sound
        Instantiate(deathParticleSystem, transform.position, Quaternion.identity);
        Instantiate(deathSound, transform.position, Quaternion.identity);

        // Unfreeze rotation
        rigidBody.freezeRotation = false;

        // Reset velocity and add torque
        rigidBody.velocity = Vector2.zero;
        Vector3 force = new Vector2(Random.Range(0, 2) == 0 ? 50f : -50f, 0f);
        Vector2 pos = new Vector2(transform.position.x, renderer.bounds.max.y);
        rigidBody.AddForceAtPosition(force, pos);

        // Remove animation
        animator.enabled = false;

        // Disable movement
        playerMovement.enabled = false;

        // Disable collisions
        GetComponent<BoxCollider2D>().enabled = false;
    }

    public void activateCoffeeBoost()
    {
        // Toggle flag
        isCoffeeBoosted = true;

        // Reset boost timer
        coffeeBoostTimer = COFFEE_BOOST_TIME;

        // Activate boost particle system
        boostParticleSystem.SetActive(true);
    }

    public bool isPlayerCoffeeBoosted()
    {
        return isCoffeeBoosted;
    }

    public bool checkGameOver()
    {
        return isGameOver;
    }
}

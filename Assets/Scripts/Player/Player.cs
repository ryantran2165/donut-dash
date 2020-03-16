using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private Text thiccText;
    [SerializeField] private Text scoreText;
    [SerializeField] private GameObject gameOver;
    [SerializeField] private Text gameOverText;
    [SerializeField] private ParticleSystem deathParticleSystem;
    [SerializeField] private RuntimeAnimatorController sticController;
    [SerializeField] private RuntimeAnimatorController thicController;
    [SerializeField] private RuntimeAnimatorController thiccController;
    [SerializeField] private AudioSource bgm;
    [SerializeField] private GameObject deathSound;

    private int thiccLevel;
    private Rigidbody2D rigidBody;
    private PlayerMovement playerMovement;

    // Animation
    private Animator animator;
    public const int THIC_THRESHOLD = 1000;
    public const int THICC_THRESHOLD = 3000;

    // Score
    private int scoreByFood;
    private int scoreByDistance;
    private float maxX;

    private const float MIN_MASS = 0.4f;
    private const float MAX_MASS = 1.5f;
    private const float MASS_PER_THICC = 0.00005f;
    private const float SPEED_PER_THICC = 0.0005f;
    private const float JUMP_FORCE_PER_THICC = 0.03f;
    private const float DISTANCE_PER_SCORE = 1f;
    private const int THICC_PER_LETTER = 1000;

    private const float PITCH_PER_THICC = 0.0001f;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        updateScore();
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
            scoreByDistance = (int) (maxX * DISTANCE_PER_SCORE);
            int totalScore = scoreByDistance + scoreByFood;
            scoreText.text = totalScore.ToString();
        }
    }

    public void setGameOver()
    {
        gameOver.SetActive(true);
        gameOverText.text = "Game Over!\nScore: " + scoreText.text + "\nYou were " + thiccText.text + "!\nPress 'R' to replay!";
        Instantiate(deathParticleSystem, transform.position, Quaternion.identity);
        Instantiate(deathSound, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

}

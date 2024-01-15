using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
    private CharacterController characterController;
    private Animator animator;

    private Vector2 input;
    private Vector3 direction;

    //  [SerializeField] float speed;
    [SerializeField] private Movement movement;
    [SerializeField] GameObject SavingText;
    private float rotationSpeed = 500f;
    private Camera mainCamera;

    float gravity = -9.81f;
    float velocity;
    float gravityMultiplier = 1.5f;

    float jumpForce = 3.5f;
    int numOfJump;
    int maxNumOfJump = 1;

    [HideInInspector] public List<GameObject> chickenList = new List<GameObject>();
    public GameObject chickenPefabs;
    private GameManager gameManager;
    

    [SerializeField] GameObject hitEffect;
    bool hasPlayedHitEffect = false;

    [HideInInspector] public bool hasChickenInList;

    [SerializeField] GameObject shield;
    [SerializeField] float mana, maxMana, defendingCost;

    private bool canDefense = true;

    [SerializeField] float stamina, maxStamina, runCost;
    public Image staminaBar, manaBar;

    private GameData gameData;
    [SerializeField] private PlayerData playerData;
    [HideInInspector] public bool hasHittedNextLevel;
    [SerializeField] SaveManager saveManager;
    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        mainCamera = Camera.main;
        animator = GetComponent<Animator>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (shield != null && manaBar != null)
        {
            ManaSystem();
        }

        isFallingDown();
        if (staminaBar != null)
        {
            StaminaSystem();
        }
    }

    private void FixedUpdate()
    {
        if (characterController.isGrounded) { animator.ResetTrigger("Jump"); }

        if (gameManager.gameOver == false)
        {
            ApplyRotation();
            ApplyGravity();
            ApplyMovement();
        }
    }

    private void ApplyRotation()
    {
        if (input.sqrMagnitude == 0) return;

        direction = Quaternion.Euler(0f, mainCamera.transform.eulerAngles.y, 0f) * new Vector3(input.x, 0f, input.y);
        var targetRotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

    }
    private void ApplyMovement()
    {
        var targetSpeed = movement.isRunning ? movement.speed * movement.multiplier : movement.speed;
        movement.currentSpeed = Mathf.MoveTowards(movement.currentSpeed, targetSpeed, movement.acceleration * Time.deltaTime);

        characterController.Move(direction * movement.currentSpeed * Time.deltaTime);

    }
    private void ApplyGravity()
    {
        if (characterController.isGrounded && velocity < 0f)
        {
            velocity = -1f;
        }
        else
        {
            velocity += gravity * gravityMultiplier * Time.deltaTime;
        }
        direction.y = velocity;
    }

    public void Move(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();
        direction = new Vector3(input.x, 0, input.y);

        // Play Walking animation
        if (input.x != 0 || input.y != 0) animator.SetBool("Walking", true);
        else animator.SetBool("Walking", false);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        if (!characterController.isGrounded && numOfJump >= maxNumOfJump) return;
        if (numOfJump == 0)
        {
            animator.ResetTrigger("Jump");
            StartCoroutine(WaitForLanding());
        }

        animator.SetTrigger("Jump");
        numOfJump++;
        velocity += jumpForce;
    }

    public void Run(InputAction.CallbackContext context)
    {
        if (context.started && Input.GetKey(KeyCode.LeftShift) && !movement.isRunning && stamina > 0)
        {
            movement.isRunning = true;
        }
        else if (context.canceled || !Input.GetKey(KeyCode.LeftShift) || stamina <= 0)
        {
            movement.isRunning = false;
        }
    }

    void StaminaSystem()
    {
        if (movement.isRunning)
        {
            stamina -= runCost * Time.deltaTime;
            if (stamina < 0) stamina = 0;

            if (!Input.GetKey(KeyCode.LeftShift) || stamina <= 0)
            {
                movement.isRunning = false;
            }
        }
        else
        {
            stamina += (runCost / 2) * Time.deltaTime;
            if (stamina > maxStamina) stamina = maxStamina;
        }
        staminaBar.fillAmount = stamina / maxStamina;
    }

    void ManaSystem()
    {
        if (Input.GetMouseButton(1))
        {
            if (mana > 0)
            {
                mana -= defendingCost * Time.deltaTime;
                if (mana < 0) mana = 0;
                canDefense = (mana > 0);
                shield.SetActive(true);
            }
            else
            {
                canDefense = false;
                shield.SetActive(false);
            }
        }
        else
        {
            mana += (defendingCost / 2) * Time.deltaTime;
            if (mana > maxMana) mana = maxMana;
            shield.SetActive(false);
        }

        manaBar.fillAmount = mana / maxMana;
    }

    void isFallingDown()
    {
        if (transform.position.y > 1f || transform.position.y < -1f)
        {
            animator.SetBool("falling", true);
        }
        else
        {
            animator.SetBool("falling", false);
        }

        if (hitEffect != null)
        {
            if (transform.position.y <= 0.05f && !hasPlayedHitEffect)
            {
                Instantiate(hitEffect, transform.position, Quaternion.identity);
                hasPlayedHitEffect = true;
            }
        }
    }


    IEnumerator WaitForLanding()
    {
        yield return new WaitUntil(() => !IsGrounded());
        while (!IsGrounded())
        {
            yield return null;
        }
        numOfJump = 0;
    }
    private bool IsGrounded() => characterController.isGrounded;



    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Orc")
            || hit.gameObject.CompareTag("Ghost")
            || hit.gameObject.CompareTag("Fireball"))
        {
            gameManager.gameOver = true;
            animator.SetTrigger("die");
            AudioManager.instance.PlaySFX("GameOver");
        }
       
        if (hit.gameObject.CompareTag("Chicken"))
        {
            hasChickenInList = true;
        }

        if (hit.gameObject.CompareTag("NextLevel"))
        {
            StartCoroutine(ShowSavingText(2f));
            int currentLevel = SceneManager.GetActiveScene().buildIndex;
            saveManager.SaveToCloud(currentLevel);
           
            hasHittedNextLevel = true;
        }
    }

    IEnumerator ShowSavingText(float showingTime)
    {
        SavingText.SetActive(true);
        yield return new WaitForSeconds(showingTime);
        SavingText.SetActive(false);
    }

    [Serializable]
    public struct Movement
    {
        public float multiplier;
        public float speed;
        public float acceleration;

        [HideInInspector] public bool isRunning;
        [HideInInspector] public float currentSpeed;
    }

    [Serializable]
    public struct PlayerData
    {
        public int levelUnlocked;
    }

}




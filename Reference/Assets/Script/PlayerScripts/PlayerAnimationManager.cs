using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{
    private Animator animator;
    //public GameObject Hand;
    private PlayerController movement;
    private CharacterController CC;
    private WeaponController WC;
    private PlayerAbilityController PAC;

    private ParticleSystem vfx;

    public Transform lowerPoint;
    public Transform upperPoint;

    public AudioClip swordPull;
    private AudioSource audioSource;

    private bool shiftPressed = false;

    public bool isSwinging = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        movement = GetComponent<PlayerController>();
        CC = GetComponent<CharacterController>();
        WC = GetComponent<WeaponController>();
        PAC = GetComponent<PlayerAbilityController>();

        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

        //animator.SetFloat("CharacterSpeed", movement.GetMoveSpeed());
        //animator.SetFloat("verticalVelocity", movement.GetVerticalVelocity());
        //animator.SetBool("isFalling", !movement.isGrounded);
        if (Input.GetButtonDown("Fire1") && !WC.isRifle && isSwinging == false)
        {
            isSwinging = true;
            animator.SetTrigger("meleeAttack");

            // Play the melee swing sound
            if (swordPull != null)
            {
              //  audioSource.PlayOneShot(swordPull);
            }
        }


            if (Input.GetKeyDown(KeyCode.E))
            {
                if (!WC.isRifle)
                {
                    animator.SetFloat("weaponMode", 1);
                }
                else
                {
                    animator.SetFloat("weaponMode", 0);
                }
            }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            shiftPressed = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            shiftPressed = false;
        }
        if (Input.GetButtonDown("Fire2"))
        {
            
        }
        /*if (Input.GetButtonUp("Jump"))
        {

        }      */
    }

    public void playSkill()
    {
        if (isSwinging == false && !WC.isRifle)
        {
            isSwinging = true;
            animator.SetTrigger("heavyAttack");
        }
    }

    public void EnableRootMotion()
    {
        animator.applyRootMotion = true;
        float newValue = 0.5f;
        CC.center = new Vector3(0.0f, newValue, 0.0f);
        CC.height = 1;

    }
    public void DisableRootMotion()
    {
        animator.applyRootMotion = false;
        CC.center = new Vector3(0f, 0.86f, 0f);
        CC.height = 1.7f;
    }

    public void DoJump()
    {
        Debug.Log("ANIMATOR: " + movement.isGrounded + " | " + Time.deltaTime);
        if (CC.isGrounded == true)
        {
            animator.SetTrigger("doJump");
        }
        else if (!CC.isGrounded && movement.getCurrentJumpCount() >= 1)
        {
            animator.SetTrigger("doDoubleJump");
        }
    }
}


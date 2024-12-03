using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilityController : MonoBehaviour
{
    private CharacterController controller;
    private PlayerAnimationManager PAM;

    public GameObject Shield;

    private bool shiftPressed = false;

    private float skillCooldown = 7f;

    private bool _canSkill = true;


    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        PAM = GetComponent<PlayerAnimationManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Shield.SetActive(true);
        }
        if (Input.GetKeyUp(KeyCode.F))
        {
            Shield.SetActive(false);
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
            if (shiftPressed && _canSkill)
            {
                print("work");
                PAM.playSkill();
                _canSkill = false;         
                StartCoroutine(InitiateSkillCD());
                HUD.Instance.Start_CD(skillCooldown, 1);
                print(skillCooldown);
            }
        }

    }

    public IEnumerator Dash()
    {
        /*float horizontalAxis = Input.GetAxis("Horizontal");
        float verticalAxis = Input.GetAxis("Vertical");

        var forward = cameraTransform.forward;
        var right = cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 move = forward * verticalAxis + right * horizontalAxis;*/

        float startTime = Time.time;
        while (Time.time < startTime + 0.25)
        {
            controller.Move(transform.forward * 50 * Time.deltaTime);
            yield return null;
        }
    }

    private IEnumerator InitiateSkillCD()
    {
        yield return new WaitForSeconds(skillCooldown);
        _canSkill = true; // Reset dash availability
                        // yield return null;
    }

    public bool _skillAvailable() { return _canSkill; }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] CharacterController characterController;
    [SerializeField] GroundChecker groundChecker;
    [SerializeField] Animator animator;
    [SerializeField] GameObject dot;
    [SerializeField] GameObject cam;
    [SerializeField] GameObject gameUI;
    public GameObject Cam { get { return cam; } }

    [SerializeField] Transform lookAtPoint;
    [SerializeField] List<Weapon> weapons;

    [SerializeField] int nowWeapon;
    [SerializeField] float moveSpeed, walkSpeed, runSpeed, jumpPower, jumpHeight, mouseSensivity, maxFallSpeed;

    [SerializeField] Vector3 moveDir, moveVec, rotDir;
    [SerializeField] bool isJump, isAttack, isRun, readyToAttack, isBattle;

    [SerializeField] bool haveControll;

    public bool IsBattle { get { return isBattle; } set { isBattle = value; } }

    public void Initialize()
    {
        haveControll = true;
        readyToAttack = true;
        weapons[1].WeaponOff();
        weapons[2].WeaponOff();
    }

    void Update()
    {
        if(haveControll)
        {
            Calcuate();
            AnimatorWork();
        }
    }

    void Calcuate()
    {
        if (moveDir.sqrMagnitude < 0.1f)
            moveSpeed = 0f;
        else
        {
            if (isRun)
            {
                if(moveDir.z >= 0f)
                    moveSpeed = runSpeed;
                else
                    moveSpeed = walkSpeed;
            }
            else
                moveSpeed = walkSpeed;
        }

        moveVec.x = moveDir.x * moveSpeed * Time.deltaTime;
        moveVec.z = moveDir.z * moveSpeed * Time.deltaTime;
        if (isJump)
        {
            if (moveVec.y < 0f)
                moveVec.y = 0f;
            moveVec.y += jumpPower * Time.deltaTime;
            if(moveVec.y > jumpHeight)
                isJump = false;
        }
        else
        {
            if (!groundChecker.IsGround)
            {
                if(moveVec.y > maxFallSpeed)
                    moveVec.y += Physics.gravity.y * Time.deltaTime;
                else
                    moveVec.y = maxFallSpeed;
            }
            else
                moveVec.y = Mathf.Lerp(moveVec.y, 0f, Time.deltaTime);
        }

        if (isAttack)
        {
            isAttack = false;
            readyToAttack = false;
            StartCoroutine(AttackRoutine(weapons[nowWeapon].Attack()));
        }
    }

    void AnimatorWork()
    {
        if (Mathf.Abs(animator.GetFloat("Speed") - moveSpeed) < 0.1f)
            animator.SetFloat("Speed", moveSpeed);
        else
            animator.SetFloat("Speed", Mathf.Lerp(animator.GetFloat("Speed"), moveSpeed, 0.1f));
        animator.SetFloat("Foward", moveDir.z);
        animator.SetFloat("Side", moveDir.x);

        if (!isJump)
        {
            if (groundChecker.IsGround)
            {
                animator.SetBool("Jump", false);
                animator.SetBool("Ground", true);
            }
            else
            {
                animator.SetBool("Ground", false);
            }
        } 
    }

    void FixedUpdate()
    {
        if (haveControll)
        {
            Action();
        }
    }

    void Action()
    {
        characterController.Move(transform.forward * moveVec.z + transform.right * moveVec.x + transform.up * moveVec.y);
        transform.Rotate(Vector3.up * rotDir.y * mouseSensivity);
        float xRot = lookAtPoint.localEulerAngles.x - rotDir.x * mouseSensivity;
        if (xRot > 200 && xRot < 330)
            xRot = 330;
        else if (xRot < 200 && xRot > 30)
            xRot = 30;
        lookAtPoint.localEulerAngles = Vector3.right * xRot;
    }

    IEnumerator AttackRoutine(float time)
    {
        yield return new WaitForSeconds(time);
        readyToAttack = true;
    }

    void OnMoveInput(InputValue inputValue)
    {
        Vector2 tmpVector = inputValue.Get<Vector2>();
        moveDir.x = tmpVector.x;
        moveDir.z = tmpVector.y;
    }

    void OnAttackInput(InputValue inputValue)
    {
        if (isBattle && nowWeapon > 0 && readyToAttack &&!isAttack && inputValue.isPressed)
        {
            isAttack = true;
            animator.SetTrigger("ShotMove");
        }
    }

    void OnJumpInput(InputValue inputValue)
    {
        if (groundChecker.IsGround && !isJump && inputValue.isPressed)
        {
            animator.SetBool("Jump", true);
            groundChecker.Jump();
            isJump = true;
        }
    }

    void OnRunInput(InputValue inputValue)
    {
        if (!inputValue.isPressed)
        {
            isRun = false;
        }
        else if (!isRun)
        {
            isRun = true;
        }
    }

    void OnInteractInput(InputValue inputValue)
    {
        if(!inputValue.isPressed)
        {
            RaycastHit cameraRayCastHit;
            Vector3 shotDir;
            if (Physics.Raycast(Cam.transform.position, Cam.transform.forward, out cameraRayCastHit, 100f))
                shotDir = (cameraRayCastHit.point - transform.position).normalized;
            else
                shotDir = transform.forward;

            RaycastHit[] hits = Physics.SphereCastAll(transform.position + shotDir, 1f, transform.up);
            foreach (RaycastHit hit in hits)
            {
                IInteractable interactable = hit.collider.gameObject.GetComponent<IInteractable>();
                interactable?.Interact();
            }
        }
    }

    void OnTalkInput(InputValue inputValue)
    {
        if (!inputValue.isPressed)
        {
            RaycastHit cameraRayCastHit;
            Vector3 shotDir;
            if (Physics.Raycast(Cam.transform.position, Cam.transform.forward, out cameraRayCastHit, 100f))
                shotDir = (cameraRayCastHit.point - transform.position).normalized;
            else
                shotDir = transform.forward;

            RaycastHit[] hits = Physics.SphereCastAll(transform.position + shotDir, 1f, transform.up);
            foreach (RaycastHit hit in hits)
            {
                ITalkable talkable = hit.collider.gameObject.GetComponent<ITalkable>();
                talkable?.Talk(transform);
            }
        }
    }

    void OnMouseMoveInput(InputValue inputValue)
    {
        Vector2 tmpDir = inputValue.Get<Vector2>();
        rotDir.y = tmpDir.x;
        rotDir.x = tmpDir.y;
    }

    void OnWeaponAInput(InputValue inputValue)
    {
        if(readyToAttack && inputValue.isPressed)
        {
            if (nowWeapon == 1)
            {
                weapons[nowWeapon].WeaponOff();
                nowWeapon = 0;
                dot.SetActive(false);
            }
            else
            {
                if (nowWeapon == 2)
                    weapons[nowWeapon].WeaponOff();
                nowWeapon = 1;
                weapons[nowWeapon].WeaponOn();
                animator.SetFloat("ReloadSpeed", 1f);
                dot.SetActive(true);
            }
            animator.SetFloat("Weapon", nowWeapon * 0.5f);
        }
    }

    void OnWeaponBInput(InputValue inputValue)
    {
        if (readyToAttack && inputValue.isPressed)
        {
            if (nowWeapon == 2)
            {
                weapons[nowWeapon].WeaponOff();
                nowWeapon = 0;
                dot.SetActive(false);
            }
            else
            {
                if (nowWeapon == 1)
                    weapons[nowWeapon].WeaponOff();
                nowWeapon = 2;
                weapons[nowWeapon].WeaponOn();
                animator.SetFloat("ReloadSpeed", 0.3f);
                dot.SetActive(true);
            }
            animator.SetFloat("Weapon", nowWeapon * 0.5f);
        }
    }

    public void ControllOut()
    {
        haveControll = false;
    }

    public void ControllIn()
    {
        haveControll = true;
    }

    public void HideUI()
    {
        gameUI.SetActive(false);
    }

    public void ShowUI()
    {
        gameUI.SetActive(true);
    }
}

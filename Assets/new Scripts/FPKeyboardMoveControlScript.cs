using UnityEngine;

/// <summary>
/// 这个脚本复习一下通过键盘控制角色移动
/// 首先，这个脚本是赋给外骨骼的
/// 并且！是朝着视角方向移动！所以要做一个局部坐标和全局坐标的转换
/// </summary>
public class FPKeyboardMoveControlScript : MonoBehaviour
{
    // characterController
    private CharacterController characterController;
    
    // 判断一下是否在地面上，如果在空中就不能再使用wasd进行移动
    // 从局部坐标转世界坐标系
    private Vector3 currentTransformDirection;

    // 移动速度
    private float moveSpeed = 10;

    // 判断是否蹲下
    private bool isCrouch = false;

    // 有关动画绑定
    public Animator BodycharactorAnimator;
    public Animator ArmscharactorAnimator;
    
    // 获取当前角色的速度与动画进行绑定
    private float velocity;
    private Vector3 tmp_charactorAnimatorVetor3;
    


    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (characterController.isGrounded)
        {
            var tmpHorizontalAxis = Input.GetAxis("Horizontal");
            var tmpVerticalAxis = Input.GetAxis("Vertical");
            BodycharactorAnimator.SetFloat("Movement_X",tmpHorizontalAxis);
            BodycharactorAnimator.SetFloat("Movement_Y",tmpVerticalAxis);
            currentTransformDirection = transform.TransformDirection(tmpHorizontalAxis, 0, tmpVerticalAxis);
            // 跳跃
            if (Input.GetKeyDown(KeyCode.Space))
            {
                currentTransformDirection.y = 2f;
            }


            // 蹲
            if (Input.GetKey(KeyCode.LeftControl))
            {
                float currentVelocity = 0;
                characterController.height =
                    Mathf.SmoothDamp(characterController.height, 0.3f,
                        ref currentVelocity, Time.deltaTime * 1.5f);
                moveSpeed = 5f;
                isCrouch = true;
            }
            else
            {
                float currentVelocity = 0;
                characterController.height =
                    Mathf.SmoothDamp(characterController.height, 2f,
                        ref currentVelocity, Time.deltaTime * 2);
                moveSpeed = 10f;
                isCrouch = false;
            }
        }


        if (isCrouch)
        {
            moveSpeed = Input.GetKey(KeyCode.LeftShift) ? 10f : 5f;
        }
        else
        {
            moveSpeed = Input.GetKey(KeyCode.LeftShift) ? 20f : 10f;
        }
        // y轴不参与速度计算，防止跳的时候速度过快导致动画抽搐
        tmp_charactorAnimatorVetor3 = characterController.velocity;
        tmp_charactorAnimatorVetor3.y = 0;
        velocity = tmp_charactorAnimatorVetor3.magnitude;
        BodycharactorAnimator.SetFloat("Velocity", velocity, 0.05f, Time.deltaTime);
        
        ArmscharactorAnimator.SetFloat("velocity", velocity, 0.05f, Time.deltaTime);
        currentTransformDirection.y -= 9.8f * Time.deltaTime;
        characterController.Move(currentTransformDirection * moveSpeed * Time.deltaTime);
    }

    private void OnGUI()
    {
        GUILayout.TextArea(Time.deltaTime.ToString());
        GUILayout.TextArea(characterController.velocity.magnitude.ToString());
    }
}
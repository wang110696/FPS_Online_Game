using System;
using System.Runtime.InteropServices;
using UnityEngine;

/// <summary>
/// 这个脚本复习一下玩家控制
/// 1.鼠标对视角控制
/// </summary>
public class FPMouseRotationControlScript : MonoBehaviour
{
    /*****  鼠标控制  *****/
    //骨骼的transform，开启的时候将脚本付给主相机，需要将外骨骼的transform赋值给此处，就可以绑定其一起动了
    [SerializeField] private Transform bodyTransform;

    //主相机的transform
    private Transform cameraTransform;

    //计算旋转角度的向量
    private Vector3 rotationVector3;

    public float mouseSensitive;

    //后坐力
    public AnimationCurve RecoilCurve;

    //后坐力范围
    public Vector2 RecoilRange;
    public float currentRecoilTime;
    private Vector2 currentRecoil;

    //计算转动角度
    private float tmpHorizontalAxis;

    private float tmpVerticalAxis;


    [DllImport("user32.dll")]
    public static extern int SetCursorPos(int x, int y);


    private void Start()
    {
        cameraTransform = transform; //因为脚本赋给相机
    }

    /**
     * 准心
     */
    public Texture2D texture;

    // void OnGUI()
    // {
    //     Rect rect = new Rect(Input.mousePosition.x - (texture.width/2),
    //
    //         Screen.height - Input.mousePosition.y - (texture.height/2),
    //
    //         texture.width, texture.height);
    //
    //     GUI.DrawTexture(rect, texture);
    // }

    private void LateUpdate()
    {
        //首先获取鼠标的x和y坐标
        var horizontalAxis = Input.GetAxis("Mouse X");
        var verticalAxis = Input.GetAxis("Mouse Y");

        //将上一个时刻的旋转角度获取到，在上一个基础上进行修改，不然鼠标不移动的时候回一直返回（0,0），视角被锁定
        tmpHorizontalAxis += horizontalAxis;
        tmpVerticalAxis += verticalAxis;
        //计算后坐力
        calculateRecoilOffset();

        tmpHorizontalAxis += currentRecoil.y;
        tmpVerticalAxis += currentRecoil.x;

        //对上下视角进行限制
        rotationVector3.x = Mathf.Clamp(-tmpVerticalAxis * mouseSensitive, -65f, 65f);
        rotationVector3.y = tmpHorizontalAxis * mouseSensitive;
        // 外骨骼跟着转动,(但是只围着Y轴转动)
        bodyTransform.rotation = Quaternion.Euler(0, rotationVector3.y, 0);
        // 相机转动
        transform.rotation = Quaternion.Euler(rotationVector3.x, rotationVector3.y, 0);

        // 鼠标居中
        // SetCursorPos((int) Screen.width / 2, (int) Screen.height / 2 + 100);
        // Cursor.visible = false;
    }

    /**
     * 计算后坐力偏移
     */
    private void calculateRecoilOffset()
    {
        currentRecoilTime += Time.deltaTime;
        float tmp_RecoilFraction = RecoilCurve.Evaluate(currentRecoilTime);
        currentRecoil = Vector2.Lerp(Vector2.zero, currentRecoil, tmp_RecoilFraction);

    }


    public void FiringForTest()
    {
        currentRecoil += RecoilRange;
        currentRecoilTime = 0;
    }
}
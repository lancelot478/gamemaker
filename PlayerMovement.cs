using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float turnSpeed = 20f;//旋转速度

    Animator m_Animator;    
    Rigidbody m_Rigidbody;  //刚体
    Vector3 m_Movement;     //移动向量
    Quaternion m_Rotation = Quaternion.identity;//旋转四元数
    void Start()
    {
        //获取组件
        m_Animator = GetComponent<Animator>();  //获取Animator组件
        m_Rigidbody = GetComponent<Rigidbody>();//获取Rigidbody组件
    }
    void FixedUpdate()
    {
        //获取按键输入
        float horizontal = Input.GetAxis("Horizontal"); //水平输入AD键
        float vertical = Input.GetAxis("Vertical");     //竖直输入WS键
       
        //根据输入设置移动向量
        m_Movement.Set(horizontal, 0f, vertical);//（X,Y,Z）轴
        m_Movement.Normalize();                  //转化为单位向量，表示移动方向
        
        //根据输入判断是否在行走
        bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f);//是否有水平输入（根据输入是否等于0）
        bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);    //是否有竖直输入（根据输入是否等于0）
        bool isWalking = hasHorizontalInput || hasVerticalInput;       //如果有水平或者竖直输入，则判断为在行走。"||"或。

        //设置动画参数
        m_Animator.SetBool("IsWalking", isWalking);//与上一篇的过渡条件参数对应

        //旋转相关
        Vector3 desiredForward = Vector3.RotateTowards
            (transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);//（现在前方向量，目标方向向量，角速度，向量大小变化）
        m_Rotation = Quaternion.LookRotation(desiredForward);               //设置旋转四元数，暂时理解为旋转方法要用的参数
 
    //当播放动画的时候，调用Rigidbody(刚体)控制角色运动     
    }
    private void OnAnimatorMove()
      {
        //移动
        m_Rigidbody.MovePosition(m_Rigidbody.position + m_Movement * m_Animator.deltaPosition.magnitude);
        //旋转
        m_Rigidbody.MoveRotation(m_Rotation);
      }
}
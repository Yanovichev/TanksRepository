using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveManager : MonoBehaviour
{
    private MoveLeft moveLeft; 
    private MoveRight moveRight;
    private UpPipe upPipe;
    private DownPipe downPipe;

    [SerializeField] private FixedJoystick joystick;
    [SerializeField] private GameObject tower;
    [SerializeField] private float speedMove, speedRotate, upperAngles, minAngles;
    private Transform transformTower;
    private Rigidbody2D rb;

    public float SpeedMove
    {
        get { return speedMove; }
        set
        {
            speedMove = value;
        }
    }
    
    void Start()
    {
        transformTower = tower.GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        moveLeft = FindObjectOfType<MoveLeft>();
        moveRight = FindObjectOfType<MoveRight>();
        upPipe = FindObjectOfType<UpPipe>();
        downPipe = FindObjectOfType<DownPipe>();
        joystick = FindObjectOfType<FixedJoystick>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Init.Instance.mobile)
        {
            if (Init.Instance.playerData.control == Control.Joystick)
            {
                MoveJoystick();
                TowerRotateJoystick();
            }
            else
            {
                MoveMobile();
                TowerRotateMobile();
            } 
        }
        else
        {
            MoveWASD();
            TowerRotate();
        }   
    }

    private void MoveMobile()
    {
        if (moveLeft.isMoveLeft)
        {
            rb.velocity = Vector2.left * speedMove;
        }
        else if (moveRight.isMoveRight)
        {
            rb.velocity = Vector2.right * speedMove;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }
    private void MoveJoystick()
    {
        if (joystick.Horizontal < 0)
        {
            rb.velocity = Vector2.left * speedMove;
        }
        else if (joystick.Horizontal > 0)
        {
            rb.velocity = Vector2.right * speedMove;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }
    private void MoveWASD()
    {
        if (Input.GetKey(KeyCode.A))
        {
            rb.velocity = Vector2.left * speedMove;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            rb.velocity = Vector2.right * speedMove;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }
    private void TowerRotate()
    {
        if (Input.GetKey(KeyCode.W) && transformTower.rotation.eulerAngles.z < upperAngles)
        {
            transformTower.Rotate(0f, 0f, 1f * speedRotate);
        }
        else if (Input.GetKey(KeyCode.S) && transformTower.rotation.eulerAngles.z > minAngles )
        {
            transformTower.Rotate(0f, 0f, -1f * speedRotate);
        }
    }
    private void TowerRotateMobile()
    {
        if (upPipe.isUpTower && transformTower.rotation.eulerAngles.z < upperAngles)
        {
            transformTower.Rotate(0f, 0f, 1f * speedRotate);
        }
        else if (downPipe.isDownTower && transformTower.rotation.eulerAngles.z > minAngles)
        {
            transformTower.Rotate(0f, 0f, -1f * speedRotate);
        }
    }
    private void TowerRotateJoystick()
    {
        if (joystick.Vertical > 0 && transformTower.rotation.eulerAngles.z < upperAngles)
        {
            transformTower.Rotate(0f, 0f, joystick.Vertical * speedRotate);
        }
        else if (joystick.Vertical < 0 && transformTower.rotation.eulerAngles.z > minAngles)
        {
            transformTower.Rotate(0f, 0f, joystick.Vertical * speedRotate);
        }
    }
}

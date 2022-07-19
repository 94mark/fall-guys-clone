using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������� �Է°��� ���� �¿�յڷ� �̵��ϰ� �ʹ�.
// jumpŰ�� ������ �ٰ� �ʹ�.
public class LHS_MainPlayer : MonoBehaviour
{
    // �̵��ӵ�
    public float speed = 10;

    // ���� �Ŀ�
    public float jumpPower = 5;

    float hAxis;
    float vAxis;

    Vector3 moveVec;

    private Camera currentCamera;
    public bool UseCameraRotation = true;

    Animator anim;
    Rigidbody rigid;

    // ����
    bool isJump;
    //bool isGround;
    bool jDown;
    
    bool isDie;

    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        currentCamera = FindObjectOfType<Camera>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //�浹 ���� �� ���� ȸ���� ���ϰ� �ʹ�.
    // ������ ���� ���� ���ֱ�
    void FreezeRotation()
    {
        rigid.angularVelocity = Vector3.zero;
    }

    private void FixedUpdate()
    {
        FreezeRotation();
        GetInput();
        Move();
        Turn();
        Jump();
        Expression();
        Die();
    }

    void GetInput()
    {
        hAxis = Input.GetAxis("Horizontal");
        vAxis = Input.GetAxis("Vertical");
        jDown = Input.GetButton("Jump");
    }

    void Move()
    {
        // ����
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        //ī�޶� �������� �����ش�.
        if (UseCameraRotation)
        {
            //ī�޶��� yȸ���� ���ؿ´�.
            Quaternion v3Rotation = Quaternion.Euler(0f, currentCamera.transform.eulerAngles.y, 0f);
            //�̵��� ���͸� ������.
            moveVec = v3Rotation * moveVec;
            //Debug.Log(currentCamera.transform.eulerAngles.y.ToString());
        }

        transform.position += moveVec * speed * Time.deltaTime;

        // Move �ִϸ��̼� true
        anim.SetBool("isMove", moveVec != Vector3.zero);
    }

    void Turn()
    {
        // �ڿ������� ȸ�� = ���ư��� �������� �ٶ󺻴�
        transform.LookAt(transform.position + moveVec);
    }

    void Jump()
    {
        //anim.SetBool("isGround", true);
        //isGround = true;
        //anim.SetBool("isJump", false);
        //isJump = false;
        //anim.SetBool("isJumpF", false);

        // jump�ϰ� �մ� ��Ȳ���� Jump���� �ʵ��� ����
        // ������ �ϰ� ���� �ʴٸ�
        if (jDown && !isJump)
        {
            rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);

            anim.SetBool("isJump", true);
            anim.SetTrigger("doJump");
            isJump = true;
        }
    }

    void Die()
    {
        if (isDie)
        {
            //rigid.AddForce(Vector3.up * 10, ForceMode.Impulse);
            anim.SetTrigger("doDie");
            isDie = true;
        }
    }

    // �ٴڿ� ����� �� �ٽ� flase�� �ٲ��ش�. 
    private void OnCollisionEnter(Collision collision)
    {
        // �±װ� �ٴ��̶�� 
        if (collision.gameObject.tag == "Floor")
        {
           // anim.SetBool("isGround", false);
           //isGround = false;
            anim.SetBool("isJump", false);
            isJump = false;
        }

        else if (collision.gameObject.tag == "Platform")
        {
            anim.SetBool("isJump", false);
            isJump = false;
        }

        else if (collision.collider.tag == "Wall")
        {
            anim.SetTrigger("doDie");
            isDie = false;
        }
    }

    // ����ǥ��
    void Expression()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            anim.SetTrigger("doDance01");
        }

        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            anim.SetTrigger("doDance02");
        }

        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            anim.SetTrigger("doVictory");
        }
    }

}



  



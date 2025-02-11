using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

[RequireComponent(typeof(PlayerState))]
public class PlayerTransform : MonoBehaviour
{
    Transform model, mainCamPos, mainCamPosB;
    GameObject backCam, mainCam;

    public GameObject target;
    public Rigidbody rd;
    public PlayerState ps;
    public CombatBase cb;
    public float Gravity = 9.8f;
    public float Speed = 10f;

    public bool CamFollow = true;
    public float CamFollowSpeed = -1;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }
	public void Initialize()
	{
		model = transform.Find("kirito");
        mainCam = GameObject.Find("Main Camera");
        backCam = transform.Find("Back Camera").gameObject;
        backCam.SetActive(false);
        mainCamPos = transform.Find("Main Camera Pos");
        mainCamPosB = transform.Find("Main Camera Block Pos");
        rd = GetComponent<Rigidbody>();
        ps ??= GetComponent<PlayerState>();
        cb ??= GetComponent<CombatBase>();
        cb.ControlledEvent += ControlledEffect;
	}

	private void FixedUpdate()
	{
        if (!IsGrounded()) rd.velocity -= new Vector3(0, Gravity * Time.fixedDeltaTime, 0);
	}

	// Update is called once per frame
	void Update()
    {
        if (ps.GetState(StateType.CanRotate)) PlayerLookTarget();

        if(CamFollow)
            mainCamFollow();
    }

    public void MoveByModelForward(float? moveStep = null)
	{
        PlayerLookTarget();

        Vector3 ModelRot = model.localRotation.eulerAngles;

        transform.Rotate(ModelRot);
        rd.velocity = transform.forward * (moveStep ?? Speed) - (!IsGrounded() ? new Vector3(0, Gravity * Time.deltaTime, 0) : Vector3.zero);
        transform.LookAt(target.transform.position);
	}

	public void PlayerLookTarget(bool onlyAxisY = true)
	{
        transform.LookAt(target?.transform.position ?? new Vector3(0, 0, 0));
		if (onlyAxisY)
		{
            Vector3 Rot = transform.rotation.eulerAngles;
            Rot.z = Rot.x = 0;
            transform.rotation = Quaternion.Euler(Rot);
        }
    }

    public void ResetModelForward()
	{
        SetModelForward();
    }

    public void ResetVelocity()
	{
        rd.velocity = Vector3.zero;
	}

    void mainCamFollow()
	{
        mainCamPos.localEulerAngles = new Vector3(-0.1f * Vector3.Distance(target.transform.position, transform.position) + 21.538f, 0, 0);
        mainCamPosB.localEulerAngles = mainCamPos.localEulerAngles + new Vector3(-20, 0, 0);
        RaycastHit rh;
        Transform pos = !Physics.Linecast(mainCamPos.position, transform.position, out rh, LayerMask.GetMask("scene")) ? mainCamPos : mainCamPosB;
        float offset = CamFollowSpeed * Time.deltaTime;
        float dist = Vector3.Distance(mainCam.transform.position, pos.position);
        if (CamFollowSpeed == -1 || dist <= offset)
		{
            mainCam.transform.position = pos.position;
            mainCam.transform.rotation = pos.rotation;
            CamFollowSpeed = -1;
        }
		else
		{
            float ratio = offset / dist;
            mainCam.transform.position += Vector3.Normalize(pos.position - mainCam.transform.position) * offset;
            mainCam.transform.rotation = Quaternion.Lerp(mainCam.transform.rotation, pos.rotation, ratio);
		}
    }

    public void SetModelForward(int dir = 1)
	{
        switch (dir)
        {
            case 1:
                model.localRotation = Quaternion.Euler(0, 0, 0);
                backCam.SetActive(false);
                break;
            case 2://back
                model.localRotation = Quaternion.Euler(0, 180, 0);
                backCam.SetActive(true);
                break;
            case 3://left
                model.localRotation = Quaternion.Euler(0, -90, 0);
                backCam.SetActive(false);
                break;
            case 4://right
                model.localRotation = Quaternion.Euler(0, 90, 0);
                backCam.SetActive(false);
                break;
        }
    }

    public void StopMove()
	{
        rd.velocity = Vector3.zero;
	}

    public bool IsGrounded()
	{
        return Physics.Raycast(transform.position, -Vector3.up, 0.1f, LayerMask.GetMask("scene"));
	}
    public void ControlledEffect(Attack atk)
    {
        if(atk.ct == ControlType.repulse)
		{
            rd.velocity = -transform.forward * atk.force;
		}
    }

}

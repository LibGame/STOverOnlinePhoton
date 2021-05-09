using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerController : MonoBehaviour
{
	
	public CharacterController controller;
	public Player Player;
	public PhotonView PhotonView;
    public float gravity;
	public Renderer Renderer;
	[HideInInspector] public FloatingJoystick FloatingJoystick;
	[HideInInspector] public GameObject MainCanvas;
	[HideInInspector] public GameObject CameraMain;
	Vector3 moveDirection;

    private void Start()
    {

		if (!PhotonView.IsMine)
        {
			if(CameraMain)
				CameraMain.SetActive(false);
			if(FloatingJoystick)
				FloatingJoystick.gameObject.SetActive(false);
			if(MainCanvas)
				MainCanvas.SetActive(false);
		}
	}

	public void CallSetColor(Color32 color)
    {
		PhotonView.RPC(nameof(SetColor), RpcTarget.All, color);

	}

	[PunRPC]
	public void SetColor(Color32 color)
    {
		Renderer.material.color = color;
	}

	public void SetParams(GameObject cameraMain, GameObject mainCanvas, FloatingJoystick floatingJoystick)
    {
		CameraMain = cameraMain;
		MainCanvas = mainCanvas;
		FloatingJoystick = floatingJoystick;
    }

    void Update()
	{
		if (!PhotonView.IsMine) return;
		Vector2 direction = new Vector2(FloatingJoystick.Horizontal, FloatingJoystick.Vertical);
		if (controller.isGrounded){
			moveDirection = new Vector3(direction.x, 0, direction.y);
			
			Quaternion targetRotation = moveDirection != Vector3.zero ? Quaternion.LookRotation(moveDirection) : transform.rotation;
			transform.rotation = targetRotation;

			moveDirection = moveDirection * Player.Speed;
		}

		moveDirection.y = moveDirection.y - (gravity * Time.deltaTime);
		controller.Move(moveDirection * Time.deltaTime);
		BlockMoveBorder();
	}


	private void BlockMoveBorder()
    {
		if (transform.position.x > GlobalSettings.Instance.WorldSettings.HorizontalSize - 1)
		{
			transform.position = new Vector3(GlobalSettings.Instance.WorldSettings.HorizontalSize - 1, transform.position.y, transform.position.z);
		}
		if (transform.position.z > GlobalSettings.Instance.WorldSettings.VerticalSize - 1)
		{
			transform.position = new Vector3(transform.position.x, transform.position.y, GlobalSettings.Instance.WorldSettings.VerticalSize - 1);
		}
		if (transform.position.x < 0)
		{
			transform.position = new Vector3(0, transform.position.y, transform.position.z);
		}
		if (transform.position.z < 0)
		{
			transform.position = new Vector3(transform.position.x, transform.position.y, 0);
		}
	}

}

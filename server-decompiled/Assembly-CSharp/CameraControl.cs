using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200135D RID: 4957
public class CameraControl : MonoBehaviour
{
	// Token: 0x06009C13 RID: 39955 RVA: 0x003AF4BC File Offset: 0x003AD6BC
	[PublicizedFrom(EAccessModifier.Private)]
	public void Start()
	{
		this.originalRotation = base.transform.localRotation;
		this.textObject.GetComponentInChildren<Text>().text = "PAUSED";
		this.cameraLight = base.transform.GetComponent<Light>();
		this.cameraLight.enabled = false;
	}

	// Token: 0x06009C14 RID: 39956 RVA: 0x003AF50C File Offset: 0x003AD70C
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			this.bPaused = !this.bPaused;
		}
		this.textObject.SetActive(this.bPaused);
		if (this.bPaused)
		{
			return;
		}
		if (Input.GetKeyDown(KeyCode.F))
		{
			this.cameraLight.enabled = !this.cameraLight.enabled;
		}
		float axis = Input.GetAxis("Mouse ScrollWheel");
		if (axis > 0f)
		{
			this.cameraLight.spotAngle += 3f;
		}
		else if (axis < 0f)
		{
			this.cameraLight.spotAngle -= 3f;
		}
		Vector3 vector = Vector3.zero;
		if (Input.GetKey(KeyCode.W))
		{
			vector += base.transform.forward;
		}
		if (Input.GetKey(KeyCode.S))
		{
			vector -= base.transform.forward;
		}
		if (Input.GetKey(KeyCode.A))
		{
			vector -= base.transform.right;
		}
		if (Input.GetKey(KeyCode.D))
		{
			vector += base.transform.right;
		}
		if (Input.GetKey(KeyCode.Space))
		{
			vector += base.transform.up;
		}
		if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.C))
		{
			vector -= base.transform.up;
		}
		float num = (Input.GetKey(KeyCode.LeftShift) ? (this.speed * 2f) : this.speed);
		base.transform.position += vector * num;
		this.rotationX += Input.GetAxis("Mouse X") * this.sensitivityX;
		this.rotationY += Input.GetAxis("Mouse Y") * this.sensitivityY;
		Quaternion quaternion = Quaternion.AngleAxis(this.rotationX, Vector3.up);
		Quaternion quaternion2 = Quaternion.AngleAxis(this.rotationY, -Vector3.right);
		base.transform.localRotation = this.originalRotation * quaternion * quaternion2;
	}

	// Token: 0x0400758A RID: 30090
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Quaternion originalRotation;

	// Token: 0x0400758B RID: 30091
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Light cameraLight;

	// Token: 0x0400758C RID: 30092
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float rotationX;

	// Token: 0x0400758D RID: 30093
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float rotationY;

	// Token: 0x0400758E RID: 30094
	public float sensitivityX = 2f;

	// Token: 0x0400758F RID: 30095
	public float sensitivityY = 2f;

	// Token: 0x04007590 RID: 30096
	public float speed = 0.1f;

	// Token: 0x04007591 RID: 30097
	public GameObject textObject;

	// Token: 0x04007592 RID: 30098
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool bPaused;
}

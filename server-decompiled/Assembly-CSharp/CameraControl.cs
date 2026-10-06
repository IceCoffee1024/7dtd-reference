using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200134B RID: 4939
public class CameraControl : MonoBehaviour
{
	// Token: 0x06009BBB RID: 39867 RVA: 0x003AD0B8 File Offset: 0x003AB2B8
	[PublicizedFrom(EAccessModifier.Private)]
	public void Start()
	{
		this.originalRotation = base.transform.localRotation;
		this.textObject.GetComponentInChildren<Text>().text = "PAUSED";
		this.cameraLight = base.transform.GetComponent<Light>();
		this.cameraLight.enabled = false;
	}

	// Token: 0x06009BBC RID: 39868 RVA: 0x003AD108 File Offset: 0x003AB308
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

	// Token: 0x04007549 RID: 30025
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Quaternion originalRotation;

	// Token: 0x0400754A RID: 30026
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Light cameraLight;

	// Token: 0x0400754B RID: 30027
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float rotationX;

	// Token: 0x0400754C RID: 30028
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float rotationY;

	// Token: 0x0400754D RID: 30029
	public float sensitivityX = 2f;

	// Token: 0x0400754E RID: 30030
	public float sensitivityY = 2f;

	// Token: 0x0400754F RID: 30031
	public float speed = 0.1f;

	// Token: 0x04007550 RID: 30032
	public GameObject textObject;

	// Token: 0x04007551 RID: 30033
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool bPaused;
}

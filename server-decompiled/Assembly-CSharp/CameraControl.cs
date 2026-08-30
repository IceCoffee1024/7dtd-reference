using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001311 RID: 4881
public class CameraControl : MonoBehaviour
{
	// Token: 0x06009A14 RID: 39444 RVA: 0x003A32A4 File Offset: 0x003A14A4
	[PublicizedFrom(EAccessModifier.Private)]
	public void Start()
	{
		this.originalRotation = base.transform.localRotation;
		this.textObject.GetComponentInChildren<Text>().text = "PAUSED";
		this.cameraLight = base.transform.GetComponent<Light>();
		this.cameraLight.enabled = false;
	}

	// Token: 0x06009A15 RID: 39445 RVA: 0x003A32F4 File Offset: 0x003A14F4
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

	// Token: 0x040073F6 RID: 29686
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Quaternion originalRotation;

	// Token: 0x040073F7 RID: 29687
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Light cameraLight;

	// Token: 0x040073F8 RID: 29688
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float rotationX;

	// Token: 0x040073F9 RID: 29689
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float rotationY;

	// Token: 0x040073FA RID: 29690
	public float sensitivityX = 2f;

	// Token: 0x040073FB RID: 29691
	public float sensitivityY = 2f;

	// Token: 0x040073FC RID: 29692
	public float speed = 0.1f;

	// Token: 0x040073FD RID: 29693
	public GameObject textObject;

	// Token: 0x040073FE RID: 29694
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool bPaused;
}

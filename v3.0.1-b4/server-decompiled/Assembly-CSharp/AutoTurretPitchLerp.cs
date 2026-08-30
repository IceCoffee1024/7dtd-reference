using System;
using System.Globalization;
using UnityEngine;

// Token: 0x020003C8 RID: 968
public class AutoTurretPitchLerp : MonoBehaviour
{
	// Token: 0x17000387 RID: 903
	// (get) Token: 0x06001D38 RID: 7480 RVA: 0x000B0996 File Offset: 0x000AEB96
	// (set) Token: 0x06001D39 RID: 7481 RVA: 0x000B099E File Offset: 0x000AEB9E
	public float Pitch { get; set; }

	// Token: 0x17000388 RID: 904
	// (get) Token: 0x06001D3A RID: 7482 RVA: 0x000B09A8 File Offset: 0x000AEBA8
	public float CurrentPitch
	{
		get
		{
			return this.myTransform.localRotation.eulerAngles.x - this.BaseRotation.x;
		}
	}

	// Token: 0x17000389 RID: 905
	// (get) Token: 0x06001D3B RID: 7483 RVA: 0x000B09D9 File Offset: 0x000AEBD9
	// (set) Token: 0x06001D3C RID: 7484 RVA: 0x000B09E1 File Offset: 0x000AEBE1
	public bool IsTurning
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x06001D3D RID: 7485 RVA: 0x000B09EA File Offset: 0x000AEBEA
	public void Init(DynamicProperties _properties)
	{
		if (_properties.Values.ContainsKey("TurnSpeed"))
		{
			this.degreesPerSecond = StringParsers.ParseFloat(_properties.Values["TurnSpeed"], 0, -1, NumberStyles.Any);
		}
	}

	// Token: 0x06001D3E RID: 7486 RVA: 0x000B0A20 File Offset: 0x000AEC20
	[PublicizedFrom(EAccessModifier.Private)]
	public void Awake()
	{
		this.myTransform = base.transform;
	}

	// Token: 0x06001D3F RID: 7487 RVA: 0x000B0A2E File Offset: 0x000AEC2E
	public void SetPitch()
	{
		this.myTransform.localRotation = Quaternion.Euler(this.BaseRotation.x + this.Pitch, this.BaseRotation.y, this.BaseRotation.z);
	}

	// Token: 0x06001D40 RID: 7488 RVA: 0x000B0A68 File Offset: 0x000AEC68
	public void UpdatePitch()
	{
		int num = (int)(this.myTransform.localRotation.eulerAngles.x * 1000f);
		this.myTransform.localRotation = Quaternion.Euler(Mathf.LerpAngle(this.myTransform.localRotation.eulerAngles.x, this.BaseRotation.x + this.Pitch, Time.deltaTime * ((this.IdleScan ? 0.25f : 1f) * this.degreesPerSecond)), this.BaseRotation.y, this.BaseRotation.z);
		this.IsTurning = (int)(this.myTransform.localRotation.eulerAngles.x * 1000f) != num;
	}

	// Token: 0x04001335 RID: 4917
	public Vector3 BaseRotation = Vector3.zero;

	// Token: 0x04001337 RID: 4919
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float degreesPerSecond = 11.25f;

	// Token: 0x04001338 RID: 4920
	public bool IdleScan;

	// Token: 0x04001339 RID: 4921
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform myTransform;
}

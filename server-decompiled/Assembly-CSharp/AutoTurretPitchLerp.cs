using System;
using System.Globalization;
using UnityEngine;

// Token: 0x020003D1 RID: 977
public class AutoTurretPitchLerp : MonoBehaviour
{
	// Token: 0x17000383 RID: 899
	// (get) Token: 0x06001D51 RID: 7505 RVA: 0x000B14F2 File Offset: 0x000AF6F2
	// (set) Token: 0x06001D52 RID: 7506 RVA: 0x000B14FA File Offset: 0x000AF6FA
	public float Pitch { get; set; }

	// Token: 0x17000384 RID: 900
	// (get) Token: 0x06001D53 RID: 7507 RVA: 0x000B1504 File Offset: 0x000AF704
	public float CurrentPitch
	{
		get
		{
			return this.myTransform.localRotation.eulerAngles.x - this.BaseRotation.x;
		}
	}

	// Token: 0x17000385 RID: 901
	// (get) Token: 0x06001D54 RID: 7508 RVA: 0x000B1535 File Offset: 0x000AF735
	// (set) Token: 0x06001D55 RID: 7509 RVA: 0x000B153D File Offset: 0x000AF73D
	public bool IsTurning
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x06001D56 RID: 7510 RVA: 0x000B1546 File Offset: 0x000AF746
	public void Init(DynamicProperties _properties)
	{
		if (_properties.Values.ContainsKey("TurnSpeed"))
		{
			this.degreesPerSecond = StringParsers.ParseFloat(_properties.Values["TurnSpeed"], 0, -1, NumberStyles.Any);
		}
	}

	// Token: 0x06001D57 RID: 7511 RVA: 0x000B157C File Offset: 0x000AF77C
	[PublicizedFrom(EAccessModifier.Private)]
	public void Awake()
	{
		this.myTransform = base.transform;
	}

	// Token: 0x06001D58 RID: 7512 RVA: 0x000B158A File Offset: 0x000AF78A
	public void SetPitch()
	{
		this.myTransform.localRotation = Quaternion.Euler(this.BaseRotation.x + this.Pitch, this.BaseRotation.y, this.BaseRotation.z);
	}

	// Token: 0x06001D59 RID: 7513 RVA: 0x000B15C4 File Offset: 0x000AF7C4
	public void UpdatePitch()
	{
		int num = (int)(this.myTransform.localRotation.eulerAngles.x * 1000f);
		this.myTransform.localRotation = Quaternion.Euler(Mathf.LerpAngle(this.myTransform.localRotation.eulerAngles.x, this.BaseRotation.x + this.Pitch, Time.deltaTime * ((this.IdleScan ? 0.25f : 1f) * this.degreesPerSecond)), this.BaseRotation.y, this.BaseRotation.z);
		this.IsTurning = (int)(this.myTransform.localRotation.eulerAngles.x * 1000f) != num;
	}

	// Token: 0x04001353 RID: 4947
	public Vector3 BaseRotation = Vector3.zero;

	// Token: 0x04001355 RID: 4949
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float degreesPerSecond = 11.25f;

	// Token: 0x04001356 RID: 4950
	public bool IdleScan;

	// Token: 0x04001357 RID: 4951
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform myTransform;
}

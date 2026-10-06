using System;
using System.Globalization;
using UnityEngine;

// Token: 0x020003D0 RID: 976
public class AutoTurretYawLerp : MonoBehaviour
{
	// Token: 0x17000386 RID: 902
	// (get) Token: 0x06001D57 RID: 7511 RVA: 0x000B14A0 File Offset: 0x000AF6A0
	// (set) Token: 0x06001D58 RID: 7512 RVA: 0x000B14A8 File Offset: 0x000AF6A8
	public float Yaw { get; set; }

	// Token: 0x17000387 RID: 903
	// (get) Token: 0x06001D59 RID: 7513 RVA: 0x000B14B4 File Offset: 0x000AF6B4
	public float CurrentYaw
	{
		get
		{
			return this.myTransform.localRotation.eulerAngles.y - this.BaseRotation.y;
		}
	}

	// Token: 0x17000388 RID: 904
	// (get) Token: 0x06001D5A RID: 7514 RVA: 0x000B14E5 File Offset: 0x000AF6E5
	// (set) Token: 0x06001D5B RID: 7515 RVA: 0x000B14ED File Offset: 0x000AF6ED
	public bool IsTurning
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x06001D5C RID: 7516 RVA: 0x000B14F8 File Offset: 0x000AF6F8
	public void Init(DynamicProperties _properties)
	{
		if (_properties.Values.ContainsKey("TurnSpeed"))
		{
			this.degreesPerSecond = StringParsers.ParseFloat(_properties.Values["TurnSpeed"], 0, -1, NumberStyles.Any);
		}
		if (_properties.Values.ContainsKey("TurnSpeedIdle"))
		{
			this.idleDegreesPerSecond = StringParsers.ParseFloat(_properties.Values["TurnSpeedIdle"], 0, -1, NumberStyles.Any);
		}
	}

	// Token: 0x06001D5D RID: 7517 RVA: 0x000B156D File Offset: 0x000AF76D
	[PublicizedFrom(EAccessModifier.Private)]
	public void Awake()
	{
		this.myTransform = base.transform;
	}

	// Token: 0x06001D5E RID: 7518 RVA: 0x000B157B File Offset: 0x000AF77B
	public void SetYaw()
	{
		this.myTransform.localRotation = Quaternion.Euler(this.BaseRotation.x, this.BaseRotation.y + this.Yaw, this.BaseRotation.z);
	}

	// Token: 0x06001D5F RID: 7519 RVA: 0x000B15B8 File Offset: 0x000AF7B8
	public void UpdateYaw()
	{
		float num = Mathf.LerpAngle(this.myTransform.localRotation.eulerAngles.y, this.BaseRotation.y + this.Yaw, Time.deltaTime * (this.IdleScan ? this.idleDegreesPerSecond : this.degreesPerSecond));
		this.myTransform.localRotation = Quaternion.Euler(this.BaseRotation.x, num, this.BaseRotation.z);
		this.IsTurning = (int)num != (int)((this.BaseRotation.y + this.Yaw) * 100f);
	}

	// Token: 0x04001349 RID: 4937
	public Vector3 BaseRotation = Vector3.zero;

	// Token: 0x0400134B RID: 4939
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float degreesPerSecond = 11.25f;

	// Token: 0x0400134C RID: 4940
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float idleDegreesPerSecond = 0.5f;

	// Token: 0x0400134D RID: 4941
	public bool IdleScan;

	// Token: 0x0400134E RID: 4942
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform myTransform;
}

using System;
using System.Globalization;
using UnityEngine;

// Token: 0x020003D2 RID: 978
public class AutoTurretYawLerp : MonoBehaviour
{
	// Token: 0x17000386 RID: 902
	// (get) Token: 0x06001D5B RID: 7515 RVA: 0x000B16B0 File Offset: 0x000AF8B0
	// (set) Token: 0x06001D5C RID: 7516 RVA: 0x000B16B8 File Offset: 0x000AF8B8
	public float Yaw { get; set; }

	// Token: 0x17000387 RID: 903
	// (get) Token: 0x06001D5D RID: 7517 RVA: 0x000B16C4 File Offset: 0x000AF8C4
	public float CurrentYaw
	{
		get
		{
			return this.myTransform.localRotation.eulerAngles.y - this.BaseRotation.y;
		}
	}

	// Token: 0x17000388 RID: 904
	// (get) Token: 0x06001D5E RID: 7518 RVA: 0x000B16F5 File Offset: 0x000AF8F5
	// (set) Token: 0x06001D5F RID: 7519 RVA: 0x000B16FD File Offset: 0x000AF8FD
	public bool IsTurning
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x06001D60 RID: 7520 RVA: 0x000B1708 File Offset: 0x000AF908
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

	// Token: 0x06001D61 RID: 7521 RVA: 0x000B177D File Offset: 0x000AF97D
	[PublicizedFrom(EAccessModifier.Private)]
	public void Awake()
	{
		this.myTransform = base.transform;
	}

	// Token: 0x06001D62 RID: 7522 RVA: 0x000B178B File Offset: 0x000AF98B
	public void SetYaw()
	{
		this.myTransform.localRotation = Quaternion.Euler(this.BaseRotation.x, this.BaseRotation.y + this.Yaw, this.BaseRotation.z);
	}

	// Token: 0x06001D63 RID: 7523 RVA: 0x000B17C8 File Offset: 0x000AF9C8
	public void UpdateYaw()
	{
		float num = Mathf.LerpAngle(this.myTransform.localRotation.eulerAngles.y, this.BaseRotation.y + this.Yaw, Time.deltaTime * (this.IdleScan ? this.idleDegreesPerSecond : this.degreesPerSecond));
		this.myTransform.localRotation = Quaternion.Euler(this.BaseRotation.x, num, this.BaseRotation.z);
		this.IsTurning = (int)num != (int)((this.BaseRotation.y + this.Yaw) * 100f);
	}

	// Token: 0x04001359 RID: 4953
	public Vector3 BaseRotation = Vector3.zero;

	// Token: 0x0400135B RID: 4955
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float degreesPerSecond = 11.25f;

	// Token: 0x0400135C RID: 4956
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float idleDegreesPerSecond = 0.5f;

	// Token: 0x0400135D RID: 4957
	public bool IdleScan;

	// Token: 0x0400135E RID: 4958
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform myTransform;
}

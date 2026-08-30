using System;
using System.Globalization;
using UnityEngine;

// Token: 0x020003C9 RID: 969
public class AutoTurretYawLerp : MonoBehaviour
{
	// Token: 0x1700038A RID: 906
	// (get) Token: 0x06001D42 RID: 7490 RVA: 0x000B0B54 File Offset: 0x000AED54
	// (set) Token: 0x06001D43 RID: 7491 RVA: 0x000B0B5C File Offset: 0x000AED5C
	public float Yaw { get; set; }

	// Token: 0x1700038B RID: 907
	// (get) Token: 0x06001D44 RID: 7492 RVA: 0x000B0B68 File Offset: 0x000AED68
	public float CurrentYaw
	{
		get
		{
			return this.myTransform.localRotation.eulerAngles.y - this.BaseRotation.y;
		}
	}

	// Token: 0x1700038C RID: 908
	// (get) Token: 0x06001D45 RID: 7493 RVA: 0x000B0B99 File Offset: 0x000AED99
	// (set) Token: 0x06001D46 RID: 7494 RVA: 0x000B0BA1 File Offset: 0x000AEDA1
	public bool IsTurning
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x06001D47 RID: 7495 RVA: 0x000B0BAC File Offset: 0x000AEDAC
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

	// Token: 0x06001D48 RID: 7496 RVA: 0x000B0C21 File Offset: 0x000AEE21
	[PublicizedFrom(EAccessModifier.Private)]
	public void Awake()
	{
		this.myTransform = base.transform;
	}

	// Token: 0x06001D49 RID: 7497 RVA: 0x000B0C2F File Offset: 0x000AEE2F
	public void SetYaw()
	{
		this.myTransform.localRotation = Quaternion.Euler(this.BaseRotation.x, this.BaseRotation.y + this.Yaw, this.BaseRotation.z);
	}

	// Token: 0x06001D4A RID: 7498 RVA: 0x000B0C6C File Offset: 0x000AEE6C
	public void UpdateYaw()
	{
		float num = Mathf.LerpAngle(this.myTransform.localRotation.eulerAngles.y, this.BaseRotation.y + this.Yaw, Time.deltaTime * (this.IdleScan ? this.idleDegreesPerSecond : this.degreesPerSecond));
		this.myTransform.localRotation = Quaternion.Euler(this.BaseRotation.x, num, this.BaseRotation.z);
		this.IsTurning = (int)num != (int)((this.BaseRotation.y + this.Yaw) * 100f);
	}

	// Token: 0x0400133B RID: 4923
	public Vector3 BaseRotation = Vector3.zero;

	// Token: 0x0400133D RID: 4925
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float degreesPerSecond = 11.25f;

	// Token: 0x0400133E RID: 4926
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float idleDegreesPerSecond = 0.5f;

	// Token: 0x0400133F RID: 4927
	public bool IdleScan;

	// Token: 0x04001340 RID: 4928
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform myTransform;
}

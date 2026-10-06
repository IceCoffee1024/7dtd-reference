using System;
using System.Globalization;
using UnityEngine;

// Token: 0x020003CF RID: 975
public class AutoTurretPitchLerp : MonoBehaviour
{
	// Token: 0x17000383 RID: 899
	// (get) Token: 0x06001D4D RID: 7501 RVA: 0x000B12E2 File Offset: 0x000AF4E2
	// (set) Token: 0x06001D4E RID: 7502 RVA: 0x000B12EA File Offset: 0x000AF4EA
	public float Pitch { get; set; }

	// Token: 0x17000384 RID: 900
	// (get) Token: 0x06001D4F RID: 7503 RVA: 0x000B12F4 File Offset: 0x000AF4F4
	public float CurrentPitch
	{
		get
		{
			return this.myTransform.localRotation.eulerAngles.x - this.BaseRotation.x;
		}
	}

	// Token: 0x17000385 RID: 901
	// (get) Token: 0x06001D50 RID: 7504 RVA: 0x000B1325 File Offset: 0x000AF525
	// (set) Token: 0x06001D51 RID: 7505 RVA: 0x000B132D File Offset: 0x000AF52D
	public bool IsTurning
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x06001D52 RID: 7506 RVA: 0x000B1336 File Offset: 0x000AF536
	public void Init(DynamicProperties _properties)
	{
		if (_properties.Values.ContainsKey("TurnSpeed"))
		{
			this.degreesPerSecond = StringParsers.ParseFloat(_properties.Values["TurnSpeed"], 0, -1, NumberStyles.Any);
		}
	}

	// Token: 0x06001D53 RID: 7507 RVA: 0x000B136C File Offset: 0x000AF56C
	[PublicizedFrom(EAccessModifier.Private)]
	public void Awake()
	{
		this.myTransform = base.transform;
	}

	// Token: 0x06001D54 RID: 7508 RVA: 0x000B137A File Offset: 0x000AF57A
	public void SetPitch()
	{
		this.myTransform.localRotation = Quaternion.Euler(this.BaseRotation.x + this.Pitch, this.BaseRotation.y, this.BaseRotation.z);
	}

	// Token: 0x06001D55 RID: 7509 RVA: 0x000B13B4 File Offset: 0x000AF5B4
	public void UpdatePitch()
	{
		int num = (int)(this.myTransform.localRotation.eulerAngles.x * 1000f);
		this.myTransform.localRotation = Quaternion.Euler(Mathf.LerpAngle(this.myTransform.localRotation.eulerAngles.x, this.BaseRotation.x + this.Pitch, Time.deltaTime * ((this.IdleScan ? 0.25f : 1f) * this.degreesPerSecond)), this.BaseRotation.y, this.BaseRotation.z);
		this.IsTurning = (int)(this.myTransform.localRotation.eulerAngles.x * 1000f) != num;
	}

	// Token: 0x04001343 RID: 4931
	public Vector3 BaseRotation = Vector3.zero;

	// Token: 0x04001345 RID: 4933
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float degreesPerSecond = 11.25f;

	// Token: 0x04001346 RID: 4934
	public bool IdleScan;

	// Token: 0x04001347 RID: 4935
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform myTransform;
}

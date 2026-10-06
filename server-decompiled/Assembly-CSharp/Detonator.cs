using System;
using UnityEngine;

// Token: 0x020013FA RID: 5114
public class Detonator : MonoBehaviour
{
	// Token: 0x0600A044 RID: 41028 RVA: 0x003C629F File Offset: 0x003C449F
	public void StartCountdown()
	{
		if (base.isActiveAndEnabled)
		{
			return;
		}
		base.enabled = true;
		base.gameObject.SetActive(true);
	}

	// Token: 0x0600A045 RID: 41029 RVA: 0x003C62BD File Offset: 0x003C44BD
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnEnable()
	{
		this._animTime = 0f;
		this._animTimeDetonator = 0f;
		base.gameObject.SetActive(true);
	}

	// Token: 0x0600A046 RID: 41030 RVA: 0x003C62E1 File Offset: 0x003C44E1
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDisable()
	{
		base.gameObject.SetActive(false);
	}

	// Token: 0x0600A047 RID: 41031 RVA: 0x003C62F0 File Offset: 0x003C44F0
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update()
	{
		float num = Time.deltaTime;
		num *= this.PulseRateScale;
		this._animTime += num;
		this._animTimeDetonator += num * ((this._timeRate != null) ? this._timeRate.Evaluate(this._animTime) : 1f);
		if (this._light != null && this._lightIntensity != null)
		{
			this._light.intensity = this._lightIntensity.Evaluate(this._animTimeDetonator);
		}
	}

	// Token: 0x04007956 RID: 31062
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public Light _light;

	// Token: 0x04007957 RID: 31063
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public AnimationCurve _timeRate;

	// Token: 0x04007958 RID: 31064
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public AnimationCurve _lightIntensity;

	// Token: 0x04007959 RID: 31065
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float _animTime;

	// Token: 0x0400795A RID: 31066
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float _animTimeDetonator;

	// Token: 0x0400795B RID: 31067
	public float PulseRateScale = 1f;
}

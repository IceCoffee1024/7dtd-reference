using System;
using UnityEngine;

// Token: 0x0200140C RID: 5132
public class Detonator : MonoBehaviour
{
	// Token: 0x0600A09C RID: 41116 RVA: 0x003C86A3 File Offset: 0x003C68A3
	public void StartCountdown()
	{
		if (base.isActiveAndEnabled)
		{
			return;
		}
		base.enabled = true;
		base.gameObject.SetActive(true);
	}

	// Token: 0x0600A09D RID: 41117 RVA: 0x003C86C1 File Offset: 0x003C68C1
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnEnable()
	{
		this._animTime = 0f;
		this._animTimeDetonator = 0f;
		base.gameObject.SetActive(true);
	}

	// Token: 0x0600A09E RID: 41118 RVA: 0x003C86E5 File Offset: 0x003C68E5
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDisable()
	{
		base.gameObject.SetActive(false);
	}

	// Token: 0x0600A09F RID: 41119 RVA: 0x003C86F4 File Offset: 0x003C68F4
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

	// Token: 0x04007997 RID: 31127
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public Light _light;

	// Token: 0x04007998 RID: 31128
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public AnimationCurve _timeRate;

	// Token: 0x04007999 RID: 31129
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public AnimationCurve _lightIntensity;

	// Token: 0x0400799A RID: 31130
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float _animTime;

	// Token: 0x0400799B RID: 31131
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float _animTimeDetonator;

	// Token: 0x0400799C RID: 31132
	public float PulseRateScale = 1f;
}

using System;
using UnityEngine;

// Token: 0x020013DA RID: 5082
public class Detonator : MonoBehaviour
{
	// Token: 0x06009F99 RID: 40857 RVA: 0x003C1703 File Offset: 0x003BF903
	public void StartCountdown()
	{
		if (base.isActiveAndEnabled)
		{
			return;
		}
		base.enabled = true;
		base.gameObject.SetActive(true);
	}

	// Token: 0x06009F9A RID: 40858 RVA: 0x003C1721 File Offset: 0x003BF921
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnEnable()
	{
		this._animTime = 0f;
		this._animTimeDetonator = 0f;
		base.gameObject.SetActive(true);
	}

	// Token: 0x06009F9B RID: 40859 RVA: 0x003C1745 File Offset: 0x003BF945
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDisable()
	{
		base.gameObject.SetActive(false);
	}

	// Token: 0x06009F9C RID: 40860 RVA: 0x003C1754 File Offset: 0x003BF954
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

	// Token: 0x040078DB RID: 30939
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public Light _light;

	// Token: 0x040078DC RID: 30940
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public AnimationCurve _timeRate;

	// Token: 0x040078DD RID: 30941
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public AnimationCurve _lightIntensity;

	// Token: 0x040078DE RID: 30942
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float _animTime;

	// Token: 0x040078DF RID: 30943
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float _animTimeDetonator;

	// Token: 0x040078E0 RID: 30944
	public float PulseRateScale = 1f;
}

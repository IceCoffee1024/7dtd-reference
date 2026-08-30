using System;
using System.Collections;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020006DF RID: 1759
[Preserve]
public class MinEventActionShakeCamera : MinEventActionTargetedBase
{
	// Token: 0x06003616 RID: 13846 RVA: 0x00165C00 File Offset: 0x00163E00
	public override void Execute(MinEventParams _params)
	{
		if (this.targets == null || GameManager.Instance == null)
		{
			return;
		}
		for (int i = 0; i < this.targets.Count; i++)
		{
			if (this.targets[i] as EntityPlayerLocal != null)
			{
				if (!string.IsNullOrEmpty(this.refCvarNameShakeSpeed))
				{
					(this.targets[i] as EntityPlayerLocal).vp_FPCamera.ShakeSpeed = this.targets[i].Buffs.GetCustomVar(this.refCvarNameShakeSpeed);
				}
				else
				{
					(this.targets[i] as EntityPlayerLocal).vp_FPCamera.ShakeSpeed = this.shakeSpeed;
				}
				if (!string.IsNullOrEmpty(this.refCvarNameShakeAmplitude))
				{
					(this.targets[i] as EntityPlayerLocal).vp_FPCamera.ShakeAmplitude = new Vector3(1f, 1f, 0f) * this.targets[i].Buffs.GetCustomVar(this.refCvarNameShakeAmplitude);
				}
				else
				{
					(this.targets[i] as EntityPlayerLocal).vp_FPCamera.ShakeAmplitude = new Vector3(1f, 1f, 0f) * this.shakeAmplitude;
				}
				float customVar = this.shakeTime;
				if (!string.IsNullOrEmpty(this.refCvarNameShakeTime))
				{
					customVar = (this.targets[i] as EntityPlayerLocal).Buffs.GetCustomVar(this.refCvarNameShakeTime);
				}
				if (customVar > 0f)
				{
					GameManager.Instance.StartCoroutine(this.stopShaking(this.targets[i] as EntityPlayerLocal, customVar));
				}
			}
		}
	}

	// Token: 0x06003617 RID: 13847 RVA: 0x00165DBC File Offset: 0x00163FBC
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator stopShaking(EntityPlayerLocal target, float time)
	{
		yield return new WaitForSeconds(time);
		if (!target)
		{
			yield break;
		}
		vp_FPCamera vp_FPCamera = target.vp_FPCamera;
		if (!vp_FPCamera)
		{
			yield break;
		}
		vp_FPCamera.ShakeSpeed = 0f;
		vp_FPCamera.ShakeAmplitude = Vector3.zero;
		yield break;
	}

	// Token: 0x06003618 RID: 13848 RVA: 0x00165DD2 File Offset: 0x00163FD2
	public override bool CanExecute(MinEventTypes _eventType, MinEventParams _params)
	{
		return base.CanExecute(_eventType, _params);
	}

	// Token: 0x06003619 RID: 13849 RVA: 0x00165DDC File Offset: 0x00163FDC
	public override bool ParseXmlAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXmlAttribute(_attribute);
		if (!flag)
		{
			string localName = _attribute.Name.LocalName;
			if (!(localName == "shake_speed"))
			{
				if (!(localName == "shake_amplitude"))
				{
					if (localName == "shake_time")
					{
						if (_attribute.Value.StartsWith("@"))
						{
							this.refCvarNameShakeTime = _attribute.Value.Substring(1);
						}
						else
						{
							this.shakeTime = StringParsers.ParseFloat(_attribute.Value, 0, -1, NumberStyles.Any);
						}
					}
				}
				else if (_attribute.Value.StartsWith("@"))
				{
					this.refCvarNameShakeAmplitude = _attribute.Value.Substring(1);
				}
				else
				{
					this.shakeAmplitude = StringParsers.ParseFloat(_attribute.Value, 0, -1, NumberStyles.Any);
				}
			}
			else if (_attribute.Value.StartsWith("@"))
			{
				this.refCvarNameShakeSpeed = _attribute.Value.Substring(1);
			}
			else
			{
				this.shakeSpeed = StringParsers.ParseFloat(_attribute.Value, 0, -1, NumberStyles.Any);
			}
		}
		return flag;
	}

	// Token: 0x04002B88 RID: 11144
	[PublicizedFrom(EAccessModifier.Private)]
	public float shakeSpeed;

	// Token: 0x04002B89 RID: 11145
	[PublicizedFrom(EAccessModifier.Private)]
	public float shakeAmplitude;

	// Token: 0x04002B8A RID: 11146
	[PublicizedFrom(EAccessModifier.Private)]
	public float shakeTime = 1f;

	// Token: 0x04002B8B RID: 11147
	[PublicizedFrom(EAccessModifier.Private)]
	public string refCvarNameShakeSpeed;

	// Token: 0x04002B8C RID: 11148
	[PublicizedFrom(EAccessModifier.Private)]
	public string refCvarNameShakeAmplitude;

	// Token: 0x04002B8D RID: 11149
	[PublicizedFrom(EAccessModifier.Private)]
	public string refCvarNameShakeTime;
}

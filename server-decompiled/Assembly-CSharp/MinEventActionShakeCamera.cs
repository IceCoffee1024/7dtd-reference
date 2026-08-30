using System;
using System.Collections;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020006E9 RID: 1769
[Preserve]
public class MinEventActionShakeCamera : MinEventActionTargetedBase
{
	// Token: 0x0600366C RID: 13932 RVA: 0x00167DD8 File Offset: 0x00165FD8
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

	// Token: 0x0600366D RID: 13933 RVA: 0x00167F94 File Offset: 0x00166194
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

	// Token: 0x0600366E RID: 13934 RVA: 0x00167FAA File Offset: 0x001661AA
	public override bool CanExecute(MinEventTypes _eventType, MinEventParams _params)
	{
		return base.CanExecute(_eventType, _params);
	}

	// Token: 0x0600366F RID: 13935 RVA: 0x00167FB4 File Offset: 0x001661B4
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

	// Token: 0x04002BFC RID: 11260
	[PublicizedFrom(EAccessModifier.Private)]
	public float shakeSpeed;

	// Token: 0x04002BFD RID: 11261
	[PublicizedFrom(EAccessModifier.Private)]
	public float shakeAmplitude;

	// Token: 0x04002BFE RID: 11262
	[PublicizedFrom(EAccessModifier.Private)]
	public float shakeTime = 1f;

	// Token: 0x04002BFF RID: 11263
	[PublicizedFrom(EAccessModifier.Private)]
	public string refCvarNameShakeSpeed;

	// Token: 0x04002C00 RID: 11264
	[PublicizedFrom(EAccessModifier.Private)]
	public string refCvarNameShakeAmplitude;

	// Token: 0x04002C01 RID: 11265
	[PublicizedFrom(EAccessModifier.Private)]
	public string refCvarNameShakeTime;
}

using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006F2 RID: 1778
[Preserve]
public class MinEventActionSetAudioMixerState : MinEventActionTargetedBase
{
	// Token: 0x1700058F RID: 1423
	// (get) Token: 0x060036D7 RID: 14039 RVA: 0x00169274 File Offset: 0x00167474
	// (set) Token: 0x060036D8 RID: 14040 RVA: 0x0016927C File Offset: 0x0016747C
	public MinEventActionSetAudioMixerState.AudioMixerStates State
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000590 RID: 1424
	// (get) Token: 0x060036D9 RID: 14041 RVA: 0x00169285 File Offset: 0x00167485
	// (set) Token: 0x060036DA RID: 14042 RVA: 0x0016928D File Offset: 0x0016748D
	public bool Value
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x060036DB RID: 14043 RVA: 0x00169298 File Offset: 0x00167498
	public override void Execute(MinEventParams _params)
	{
		if (this.targets == null)
		{
			return;
		}
		for (int i = 0; i < this.targets.Count; i++)
		{
			EntityPlayerLocal entityPlayerLocal = this.targets[i] as EntityPlayerLocal;
			if (entityPlayerLocal != null)
			{
				MinEventActionSetAudioMixerState.AudioMixerStates state = this.State;
				if (state != MinEventActionSetAudioMixerState.AudioMixerStates.Stunned)
				{
					if (state == MinEventActionSetAudioMixerState.AudioMixerStates.Deafened)
					{
						entityPlayerLocal.isDeafened = this.Value;
					}
				}
				else
				{
					entityPlayerLocal.isStunned = this.Value;
				}
			}
		}
	}

	// Token: 0x060036DC RID: 14044 RVA: 0x00169308 File Offset: 0x00167508
	public override bool ParseXmlAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXmlAttribute(_attribute);
		if (!flag)
		{
			string localName = _attribute.Name.LocalName;
			if (!(localName == "state"))
			{
				if (localName == "enabled")
				{
					this.Value = StringParsers.ParseBool(_attribute.Value, 0, -1, true);
				}
			}
			else
			{
				this.State = (MinEventActionSetAudioMixerState.AudioMixerStates)Enum.Parse(typeof(MinEventActionSetAudioMixerState.AudioMixerStates), _attribute.Value);
			}
		}
		return flag;
	}

	// Token: 0x020006F3 RID: 1779
	public enum AudioMixerStates
	{
		// Token: 0x04002C0D RID: 11277
		Stunned,
		// Token: 0x04002C0E RID: 11278
		Deafened
	}
}

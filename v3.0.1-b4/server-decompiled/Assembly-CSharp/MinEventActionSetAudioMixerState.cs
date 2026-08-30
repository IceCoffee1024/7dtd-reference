using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006E6 RID: 1766
[Preserve]
public class MinEventActionSetAudioMixerState : MinEventActionTargetedBase
{
	// Token: 0x17000560 RID: 1376
	// (get) Token: 0x0600363E RID: 13886 RVA: 0x00166738 File Offset: 0x00164938
	// (set) Token: 0x0600363F RID: 13887 RVA: 0x00166740 File Offset: 0x00164940
	public MinEventActionSetAudioMixerState.AudioMixerStates State
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000561 RID: 1377
	// (get) Token: 0x06003640 RID: 13888 RVA: 0x00166749 File Offset: 0x00164949
	// (set) Token: 0x06003641 RID: 13889 RVA: 0x00166751 File Offset: 0x00164951
	public bool Value
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x06003642 RID: 13890 RVA: 0x0016675C File Offset: 0x0016495C
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

	// Token: 0x06003643 RID: 13891 RVA: 0x001667CC File Offset: 0x001649CC
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

	// Token: 0x020006E7 RID: 1767
	public enum AudioMixerStates
	{
		// Token: 0x04002BA0 RID: 11168
		Stunned,
		// Token: 0x04002BA1 RID: 11169
		Deafened
	}
}

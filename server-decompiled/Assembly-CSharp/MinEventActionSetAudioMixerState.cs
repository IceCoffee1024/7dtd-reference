using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006FA RID: 1786
[Preserve]
public class MinEventActionSetAudioMixerState : MinEventActionTargetedBase
{
	// Token: 0x17000593 RID: 1427
	// (get) Token: 0x060036EE RID: 14062 RVA: 0x0016A20C File Offset: 0x0016840C
	// (set) Token: 0x060036EF RID: 14063 RVA: 0x0016A214 File Offset: 0x00168414
	public MinEventActionSetAudioMixerState.AudioMixerStates State
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000594 RID: 1428
	// (get) Token: 0x060036F0 RID: 14064 RVA: 0x0016A21D File Offset: 0x0016841D
	// (set) Token: 0x060036F1 RID: 14065 RVA: 0x0016A225 File Offset: 0x00168425
	public bool Value
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x060036F2 RID: 14066 RVA: 0x0016A230 File Offset: 0x00168430
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

	// Token: 0x060036F3 RID: 14067 RVA: 0x0016A2A0 File Offset: 0x001684A0
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

	// Token: 0x020006FB RID: 1787
	public enum AudioMixerStates
	{
		// Token: 0x04002C29 RID: 11305
		Stunned,
		// Token: 0x04002C2A RID: 11306
		Deafened
	}
}

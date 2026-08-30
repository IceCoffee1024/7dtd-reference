using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006F0 RID: 1776
[Preserve]
public class MinEventActionSetAudioMixerState : MinEventActionTargetedBase
{
	// Token: 0x17000569 RID: 1385
	// (get) Token: 0x06003694 RID: 13972 RVA: 0x00168910 File Offset: 0x00166B10
	// (set) Token: 0x06003695 RID: 13973 RVA: 0x00168918 File Offset: 0x00166B18
	public MinEventActionSetAudioMixerState.AudioMixerStates State
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x1700056A RID: 1386
	// (get) Token: 0x06003696 RID: 13974 RVA: 0x00168921 File Offset: 0x00166B21
	// (set) Token: 0x06003697 RID: 13975 RVA: 0x00168929 File Offset: 0x00166B29
	public bool Value
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x06003698 RID: 13976 RVA: 0x00168934 File Offset: 0x00166B34
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

	// Token: 0x06003699 RID: 13977 RVA: 0x001689A4 File Offset: 0x00166BA4
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

	// Token: 0x020006F1 RID: 1777
	public enum AudioMixerStates
	{
		// Token: 0x04002C14 RID: 11284
		Stunned,
		// Token: 0x04002C15 RID: 11285
		Deafened
	}
}

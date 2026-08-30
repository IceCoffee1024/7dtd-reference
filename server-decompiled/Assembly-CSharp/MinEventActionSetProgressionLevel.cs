using System;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006EB RID: 1771
[Preserve]
public class MinEventActionSetProgressionLevel : MinEventActionTargetedBase
{
	// Token: 0x17000563 RID: 1379
	// (get) Token: 0x06003677 RID: 13943 RVA: 0x00168179 File Offset: 0x00166379
	// (set) Token: 0x06003678 RID: 13944 RVA: 0x00168181 File Offset: 0x00166381
	public string progressionName
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000564 RID: 1380
	// (get) Token: 0x06003679 RID: 13945 RVA: 0x0016818A File Offset: 0x0016638A
	// (set) Token: 0x0600367A RID: 13946 RVA: 0x00168192 File Offset: 0x00166392
	public int level
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x0600367B RID: 13947 RVA: 0x0016819C File Offset: 0x0016639C
	public override void Execute(MinEventParams _params)
	{
		if (this.targets == null)
		{
			return;
		}
		for (int i = 0; i < this.targets.Count; i++)
		{
			if (this.targets[i].Progression != null)
			{
				ProgressionValue progressionValue = this.targets[i].Progression.GetProgressionValue(this.progressionName);
				if (progressionValue != null)
				{
					if (this.level != -1)
					{
						progressionValue.Level = this.level;
						this.targets[i].Progression.bProgressionStatsChanged = !this.targets[i].isEntityRemote;
						this.targets[i].bPlayerStatsChanged |= !this.targets[i].isEntityRemote;
					}
					else
					{
						progressionValue.Level = progressionValue.ProgressionClass.MaxLevel;
						this.targets[i].Progression.bProgressionStatsChanged = !this.targets[i].isEntityRemote;
						this.targets[i].bPlayerStatsChanged |= !this.targets[i].isEntityRemote;
					}
				}
			}
		}
	}

	// Token: 0x0600367C RID: 13948 RVA: 0x001682DB File Offset: 0x001664DB
	public override bool CanExecute(MinEventTypes _eventType, MinEventParams _params)
	{
		return base.CanExecute(_eventType, _params) && this.progressionName != null && this.level >= -1;
	}

	// Token: 0x0600367D RID: 13949 RVA: 0x00168300 File Offset: 0x00166500
	public override bool ParseXmlAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXmlAttribute(_attribute);
		if (!flag)
		{
			string localName = _attribute.Name.LocalName;
			if (!(localName == "progression_name"))
			{
				if (localName == "level")
				{
					this.level = StringParsers.ParseSInt32(_attribute.Value, 0, -1, NumberStyles.Integer);
				}
			}
			else
			{
				this.progressionName = _attribute.Value;
			}
		}
		return flag;
	}
}

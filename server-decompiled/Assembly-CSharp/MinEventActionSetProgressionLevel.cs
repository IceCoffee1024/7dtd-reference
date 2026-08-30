using System;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006E1 RID: 1761
[Preserve]
public class MinEventActionSetProgressionLevel : MinEventActionTargetedBase
{
	// Token: 0x1700055A RID: 1370
	// (get) Token: 0x06003621 RID: 13857 RVA: 0x00165FA1 File Offset: 0x001641A1
	// (set) Token: 0x06003622 RID: 13858 RVA: 0x00165FA9 File Offset: 0x001641A9
	public string progressionName
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x1700055B RID: 1371
	// (get) Token: 0x06003623 RID: 13859 RVA: 0x00165FB2 File Offset: 0x001641B2
	// (set) Token: 0x06003624 RID: 13860 RVA: 0x00165FBA File Offset: 0x001641BA
	public int level
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x06003625 RID: 13861 RVA: 0x00165FC4 File Offset: 0x001641C4
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

	// Token: 0x06003626 RID: 13862 RVA: 0x00166103 File Offset: 0x00164303
	public override bool CanExecute(MinEventTypes _eventType, MinEventParams _params)
	{
		return base.CanExecute(_eventType, _params) && this.progressionName != null && this.level >= -1;
	}

	// Token: 0x06003627 RID: 13863 RVA: 0x00166128 File Offset: 0x00164328
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

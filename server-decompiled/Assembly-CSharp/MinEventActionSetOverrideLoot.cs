using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006FA RID: 1786
[Preserve]
public class MinEventActionSetOverrideLoot : MinEventActionTargetedBase
{
	// Token: 0x060036B3 RID: 14003 RVA: 0x0016908C File Offset: 0x0016728C
	public override void Execute(MinEventParams _params)
	{
		if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			return;
		}
		for (int i = 0; i < this.targets.Count; i++)
		{
			EntityPlayer entityPlayer = this.targets[i] as EntityPlayer;
			if (entityPlayer != null)
			{
				if (this.altLoot == "")
				{
					LootContainer.OverrideItems.Remove(entityPlayer);
				}
				else if (LootContainer.OverrideItems.ContainsKey(entityPlayer))
				{
					LootContainer.OverrideItems[entityPlayer] = this.altLoot.Split(',', StringSplitOptions.None);
				}
				else
				{
					LootContainer.OverrideItems.Add(entityPlayer, this.altLoot.Split(',', StringSplitOptions.None));
				}
			}
		}
	}

	// Token: 0x060036B4 RID: 14004 RVA: 0x00169138 File Offset: 0x00167338
	public override bool ParseXmlAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXmlAttribute(_attribute);
		if (!flag && _attribute.Name.LocalName == "items")
		{
			this.altLoot = _attribute.Value;
			return true;
		}
		return flag;
	}

	// Token: 0x04002C25 RID: 11301
	[PublicizedFrom(EAccessModifier.Private)]
	public string altLoot = "";
}

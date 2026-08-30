using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006F0 RID: 1776
[Preserve]
public class MinEventActionSetOverrideLoot : MinEventActionTargetedBase
{
	// Token: 0x0600365D RID: 13917 RVA: 0x00166EB4 File Offset: 0x001650B4
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

	// Token: 0x0600365E RID: 13918 RVA: 0x00166F60 File Offset: 0x00165160
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

	// Token: 0x04002BB1 RID: 11185
	[PublicizedFrom(EAccessModifier.Private)]
	public string altLoot = "";
}

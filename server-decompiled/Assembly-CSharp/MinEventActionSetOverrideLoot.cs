using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006FC RID: 1788
[Preserve]
public class MinEventActionSetOverrideLoot : MinEventActionTargetedBase
{
	// Token: 0x060036F6 RID: 14070 RVA: 0x001699F0 File Offset: 0x00167BF0
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

	// Token: 0x060036F7 RID: 14071 RVA: 0x00169A9C File Offset: 0x00167C9C
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

	// Token: 0x04002C1E RID: 11294
	[PublicizedFrom(EAccessModifier.Private)]
	public string altLoot = "";
}

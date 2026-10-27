using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x02000704 RID: 1796
[Preserve]
public class MinEventActionSetOverrideLoot : MinEventActionTargetedBase
{
	// Token: 0x0600370D RID: 14093 RVA: 0x0016A988 File Offset: 0x00168B88
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

	// Token: 0x0600370E RID: 14094 RVA: 0x0016AA34 File Offset: 0x00168C34
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

	// Token: 0x04002C3A RID: 11322
	[PublicizedFrom(EAccessModifier.Private)]
	public string altLoot = "";
}

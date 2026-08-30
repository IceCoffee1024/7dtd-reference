using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x02000707 RID: 1799
[Preserve]
public class MinEventActionSetItemInSlot : MinEventActionTargetedBase
{
	// Token: 0x060036DE RID: 14046 RVA: 0x0016A218 File Offset: 0x00168418
	public override void Execute(MinEventParams _params)
	{
		ItemValue item = ItemClass.GetItem(this.itemName, false);
		ItemClassArmor itemClassArmor = item.ItemClass as ItemClassArmor;
		if (itemClassArmor != null && itemClassArmor.EquipSlot == this.slot)
		{
			for (int i = 0; i < this.targets.Count; i++)
			{
				this.targets[i].equipment.SetSlotItem((int)this.slot, item, true);
			}
		}
	}

	// Token: 0x060036DF RID: 14047 RVA: 0x0016A284 File Offset: 0x00168484
	public override bool ParseXmlAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXmlAttribute(_attribute);
		if (!flag)
		{
			string localName = _attribute.Name.LocalName;
			if (localName == "item_name")
			{
				this.itemName = _attribute.Value;
				return true;
			}
			if (localName == "equip_slot")
			{
				Enum.TryParse<EquipmentSlots>(_attribute.Value, out this.slot);
				return true;
			}
		}
		return flag;
	}

	// Token: 0x04002C4A RID: 11338
	[PublicizedFrom(EAccessModifier.Private)]
	public string itemName = "";

	// Token: 0x04002C4B RID: 11339
	[PublicizedFrom(EAccessModifier.Private)]
	public EquipmentSlots slot = EquipmentSlots.BiomeBadge;
}

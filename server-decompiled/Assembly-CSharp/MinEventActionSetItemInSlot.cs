using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x02000709 RID: 1801
[Preserve]
public class MinEventActionSetItemInSlot : MinEventActionTargetedBase
{
	// Token: 0x06003721 RID: 14113 RVA: 0x0016AB7C File Offset: 0x00168D7C
	public override void Execute(MinEventParams _params)
	{
		ItemValue item = ItemClass.GetItem(this.itemName, false);
		ItemClassArmor itemClassArmor = item.ItemClass as ItemClassArmor;
		if (itemClassArmor != null && itemClassArmor.EquipSlot == this.slot)
		{
			for (int i = 0; i < this.targets.Count; i++)
			{
				this.targets[i].equipment.SetSlotItem((int)this.slot, item);
			}
		}
	}

	// Token: 0x06003722 RID: 14114 RVA: 0x0016ABE8 File Offset: 0x00168DE8
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

	// Token: 0x04002C43 RID: 11331
	[PublicizedFrom(EAccessModifier.Private)]
	public string itemName = "";

	// Token: 0x04002C44 RID: 11332
	[PublicizedFrom(EAccessModifier.Private)]
	public EquipmentSlots slot = EquipmentSlots.BiomeBadge;
}

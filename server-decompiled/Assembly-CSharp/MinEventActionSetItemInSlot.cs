using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006FD RID: 1789
[Preserve]
public class MinEventActionSetItemInSlot : MinEventActionTargetedBase
{
	// Token: 0x06003688 RID: 13960 RVA: 0x00168040 File Offset: 0x00166240
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

	// Token: 0x06003689 RID: 13961 RVA: 0x001680AC File Offset: 0x001662AC
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

	// Token: 0x04002BD6 RID: 11222
	[PublicizedFrom(EAccessModifier.Private)]
	public string itemName = "";

	// Token: 0x04002BD7 RID: 11223
	[PublicizedFrom(EAccessModifier.Private)]
	public EquipmentSlots slot = EquipmentSlots.BiomeBadge;
}

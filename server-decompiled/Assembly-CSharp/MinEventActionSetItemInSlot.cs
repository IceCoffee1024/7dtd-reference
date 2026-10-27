using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x02000711 RID: 1809
[Preserve]
public class MinEventActionSetItemInSlot : MinEventActionTargetedBase
{
	// Token: 0x06003738 RID: 14136 RVA: 0x0016BB14 File Offset: 0x00169D14
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

	// Token: 0x06003739 RID: 14137 RVA: 0x0016BB80 File Offset: 0x00169D80
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

	// Token: 0x04002C5F RID: 11359
	[PublicizedFrom(EAccessModifier.Private)]
	public string itemName = "";

	// Token: 0x04002C60 RID: 11360
	[PublicizedFrom(EAccessModifier.Private)]
	public EquipmentSlots slot = EquipmentSlots.BiomeBadge;
}

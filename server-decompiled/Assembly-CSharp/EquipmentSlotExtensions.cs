using System;

// Token: 0x020005FA RID: 1530
public static class EquipmentSlotExtensions
{
	// Token: 0x0600320D RID: 12813 RVA: 0x001446FC File Offset: 0x001428FC
	public static EquipmentSlotType SlotType(this EquipmentSlots slot)
	{
		EquipmentSlotType equipmentSlotType;
		switch (slot)
		{
		case EquipmentSlots.Head:
			equipmentSlotType = EquipmentSlotType.Armor;
			break;
		case EquipmentSlots.Chest:
			equipmentSlotType = EquipmentSlotType.Armor;
			break;
		case EquipmentSlots.Hands:
			equipmentSlotType = EquipmentSlotType.Armor;
			break;
		case EquipmentSlots.Feet:
			equipmentSlotType = EquipmentSlotType.Armor;
			break;
		case EquipmentSlots.BiomeBadge:
			equipmentSlotType = EquipmentSlotType.BiomeGear;
			break;
		case EquipmentSlots.BiomeBadge2:
			equipmentSlotType = EquipmentSlotType.BiomeGear;
			break;
		case EquipmentSlots.BiomeBadge3:
			equipmentSlotType = EquipmentSlotType.BiomeGear;
			break;
		case EquipmentSlots.BiomeBadge4:
			equipmentSlotType = EquipmentSlotType.BiomeGear;
			break;
		case EquipmentSlots.ClothingHead:
			equipmentSlotType = EquipmentSlotType.Clothing;
			break;
		case EquipmentSlots.ClothingChest:
			equipmentSlotType = EquipmentSlotType.Clothing;
			break;
		case EquipmentSlots.ClothingHands:
			equipmentSlotType = EquipmentSlotType.Clothing;
			break;
		case EquipmentSlots.ClothingFeet:
			equipmentSlotType = EquipmentSlotType.Clothing;
			break;
		case EquipmentSlots.Backpack:
			equipmentSlotType = EquipmentSlotType.Armor;
			break;
		case EquipmentSlots.Count:
			equipmentSlotType = EquipmentSlotType.None;
			break;
		default:
			throw new ArgumentOutOfRangeException("slot", slot, null);
		}
		return equipmentSlotType;
	}
}

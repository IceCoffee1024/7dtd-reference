using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004FE RID: 1278
public class PlayerEquippedSlots : MonoBehaviour
{
	// Token: 0x06002998 RID: 10648 RVA: 0x001065FE File Offset: 0x001047FE
	public void Init(Transform outfit)
	{
		this.outfitXF = outfit;
		if (this.outfitXF != null)
		{
			this._DisableAllNASubmeshes();
		}
	}

	// Token: 0x06002999 RID: 10649 RVA: 0x0010661C File Offset: 0x0010481C
	public void ListParts()
	{
		int count = this.parts.Count;
		for (int i = 0; i < count; i++)
		{
			Log.Warning(this.parts[i].name);
		}
	}

	// Token: 0x0600299A RID: 10650 RVA: 0x00106658 File Offset: 0x00104858
	public void ListEquipment()
	{
		int count = this.equippedParts.Count;
		for (int i = 0; i < count; i++)
		{
			Log.Warning(this.equippedParts[i].name);
		}
	}

	// Token: 0x0600299B RID: 10651 RVA: 0x00106694 File Offset: 0x00104894
	public bool IsEquipped(string partName)
	{
		int count = this.equippedParts.Count;
		for (int i = 0; i < count; i++)
		{
			if (this.equippedParts[i].name == partName)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600299C RID: 10652 RVA: 0x001066D8 File Offset: 0x001048D8
	public bool Equip(string partName)
	{
		if (this.IsEquipped(partName))
		{
			return false;
		}
		int count = this.parts.Count;
		for (int i = 0; i < count; i++)
		{
			PlayerEquippedSlots.PartInfo partInfo = this.parts[i];
			if (partInfo.name == partName)
			{
				PlayerEquippedSlots.EquippedPart equippedPart = new PlayerEquippedSlots.EquippedPart();
				equippedPart.name = partName;
				equippedPart.partInfo = partInfo;
				this.equippedParts.Add(equippedPart);
				this._RunRules();
				return true;
			}
		}
		Log.Warning("Part '{0}' not equipped.", new object[] { partName });
		return false;
	}

	// Token: 0x0600299D RID: 10653 RVA: 0x00106760 File Offset: 0x00104960
	public bool UnEquip(string partName)
	{
		int count = this.equippedParts.Count;
		for (int i = 0; i < count; i++)
		{
			if (this.equippedParts[i].name == partName)
			{
				this._EnableNASubmesh(partName, false);
				this.equippedParts.RemoveAt(i);
				this._RunRules();
				return true;
			}
		}
		Log.Warning("Part '{0}' not unequipped.", new object[] { partName });
		return false;
	}

	// Token: 0x0600299E RID: 10654 RVA: 0x001067D0 File Offset: 0x001049D0
	[PublicizedFrom(EAccessModifier.Private)]
	public void _RunRules()
	{
		int count = this.equippedParts.Count;
		for (int i = 0; i < count; i++)
		{
			PlayerEquippedSlots.EquippedPart equippedPart = this.equippedParts[i];
			equippedPart.wasShowing = equippedPart.isShowing;
			equippedPart.isShowing = true;
		}
		for (int j = 0; j < count; j++)
		{
			PlayerEquippedSlots.EquippedPart equippedPart2 = this.equippedParts[j];
			if (equippedPart2.isShowing)
			{
				PlayerEquippedSlots.PartInfo partInfo = equippedPart2.partInfo;
				if (!string.IsNullOrEmpty(partInfo.rule))
				{
					string rule = partInfo.rule;
					for (int k = 0; k < count; k++)
					{
						if (k != j)
						{
							PlayerEquippedSlots.EquippedPart equippedPart3 = this.equippedParts[k];
							if (equippedPart3.isShowing)
							{
								PlayerEquippedSlots.PartInfo partInfo2 = equippedPart3.partInfo;
								if (partInfo2.IsInSlot(rule))
								{
									Log.Warning(" Note: Part {0} hides part {1} with rule {2}.", new object[] { partInfo.name, partInfo2.name, rule });
									equippedPart3.isShowing = false;
								}
							}
						}
					}
				}
			}
		}
		for (int l = 0; l < count; l++)
		{
			PlayerEquippedSlots.EquippedPart equippedPart4 = this.equippedParts[l];
			if (equippedPart4.isShowing && !equippedPart4.wasShowing)
			{
				this._EnableNASubmesh(equippedPart4.partInfo.name, true);
			}
			else if (!equippedPart4.isShowing && equippedPart4.wasShowing)
			{
				this._EnableNASubmesh(equippedPart4.partInfo.name, false);
			}
		}
	}

	// Token: 0x0600299F RID: 10655 RVA: 0x00106939 File Offset: 0x00104B39
	[PublicizedFrom(EAccessModifier.Private)]
	public Transform _GetNAOutfit()
	{
		return this.outfitXF;
	}

	// Token: 0x060029A0 RID: 10656 RVA: 0x00106944 File Offset: 0x00104B44
	public void _EnableNASubmesh(string submeshName, bool enable)
	{
		Transform transform = this._GetNAOutfit();
		if (transform == null)
		{
			return;
		}
		int childCount = transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			Transform child = transform.GetChild(i);
			if (!(child.name == "Origin"))
			{
				GameObject gameObject = child.gameObject;
				if (gameObject.name == submeshName)
				{
					gameObject.SetActive(enable);
				}
			}
		}
	}

	// Token: 0x060029A1 RID: 10657 RVA: 0x001069B0 File Offset: 0x00104BB0
	[PublicizedFrom(EAccessModifier.Private)]
	public void _DisableAllNASubmeshes()
	{
		Transform transform = this._GetNAOutfit();
		if (transform == null)
		{
			return;
		}
		int childCount = transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			Transform child = transform.GetChild(i);
			if (!(child.name == "Origin"))
			{
				child.gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x04001F9F RID: 8095
	public List<PlayerEquippedSlots.PartInfo> parts;

	// Token: 0x04001FA0 RID: 8096
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<PlayerEquippedSlots.EquippedPart> equippedParts = new List<PlayerEquippedSlots.EquippedPart>();

	// Token: 0x04001FA1 RID: 8097
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform outfitXF;

	// Token: 0x020004FF RID: 1279
	[Serializable]
	public class PartInfo
	{
		// Token: 0x060029A3 RID: 10659 RVA: 0x00106A1A File Offset: 0x00104C1A
		public bool IsInSlot(string slotReference)
		{
			return PlayerEquippedSlots.PartInfo.RefMatchesSlot(slotReference, this.slot);
		}

		// Token: 0x060029A4 RID: 10660 RVA: 0x00106A28 File Offset: 0x00104C28
		public static bool RefMatchesSlot(string slotReference, string slotName)
		{
			int num = slotReference.IndexOf("*");
			if (num == -1)
			{
				return slotReference.Equals(slotName);
			}
			return string.Compare(slotReference, 0, slotName, 0, num) == 0;
		}

		// Token: 0x04001FA2 RID: 8098
		public string name;

		// Token: 0x04001FA3 RID: 8099
		public string slot;

		// Token: 0x04001FA4 RID: 8100
		public string rule;
	}

	// Token: 0x02000500 RID: 1280
	[PublicizedFrom(EAccessModifier.Private)]
	public class EquippedPart
	{
		// Token: 0x04001FA5 RID: 8101
		public string name;

		// Token: 0x04001FA6 RID: 8102
		public PlayerEquippedSlots.PartInfo partInfo;

		// Token: 0x04001FA7 RID: 8103
		public bool wasShowing;

		// Token: 0x04001FA8 RID: 8104
		public bool isShowing;
	}
}

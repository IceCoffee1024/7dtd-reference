using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000500 RID: 1280
public class PlayerEquippedSlots : MonoBehaviour
{
	// Token: 0x0600299F RID: 10655 RVA: 0x00106B52 File Offset: 0x00104D52
	public void Init(Transform outfit)
	{
		this.outfitXF = outfit;
		if (this.outfitXF != null)
		{
			this._DisableAllNASubmeshes();
		}
	}

	// Token: 0x060029A0 RID: 10656 RVA: 0x00106B70 File Offset: 0x00104D70
	public void ListParts()
	{
		int count = this.parts.Count;
		for (int i = 0; i < count; i++)
		{
			Log.Warning(this.parts[i].name);
		}
	}

	// Token: 0x060029A1 RID: 10657 RVA: 0x00106BAC File Offset: 0x00104DAC
	public void ListEquipment()
	{
		int count = this.equippedParts.Count;
		for (int i = 0; i < count; i++)
		{
			Log.Warning(this.equippedParts[i].name);
		}
	}

	// Token: 0x060029A2 RID: 10658 RVA: 0x00106BE8 File Offset: 0x00104DE8
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

	// Token: 0x060029A3 RID: 10659 RVA: 0x00106C2C File Offset: 0x00104E2C
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

	// Token: 0x060029A4 RID: 10660 RVA: 0x00106CB4 File Offset: 0x00104EB4
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

	// Token: 0x060029A5 RID: 10661 RVA: 0x00106D24 File Offset: 0x00104F24
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

	// Token: 0x060029A6 RID: 10662 RVA: 0x00106E8D File Offset: 0x0010508D
	[PublicizedFrom(EAccessModifier.Private)]
	public Transform _GetNAOutfit()
	{
		return this.outfitXF;
	}

	// Token: 0x060029A7 RID: 10663 RVA: 0x00106E98 File Offset: 0x00105098
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

	// Token: 0x060029A8 RID: 10664 RVA: 0x00106F04 File Offset: 0x00105104
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

	// Token: 0x04001FAF RID: 8111
	public List<PlayerEquippedSlots.PartInfo> parts;

	// Token: 0x04001FB0 RID: 8112
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<PlayerEquippedSlots.EquippedPart> equippedParts = new List<PlayerEquippedSlots.EquippedPart>();

	// Token: 0x04001FB1 RID: 8113
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform outfitXF;

	// Token: 0x02000501 RID: 1281
	[Serializable]
	public class PartInfo
	{
		// Token: 0x060029AA RID: 10666 RVA: 0x00106F6E File Offset: 0x0010516E
		public bool IsInSlot(string slotReference)
		{
			return PlayerEquippedSlots.PartInfo.RefMatchesSlot(slotReference, this.slot);
		}

		// Token: 0x060029AB RID: 10667 RVA: 0x00106F7C File Offset: 0x0010517C
		public static bool RefMatchesSlot(string slotReference, string slotName)
		{
			int num = slotReference.IndexOf("*");
			if (num == -1)
			{
				return slotReference.Equals(slotName);
			}
			return string.Compare(slotReference, 0, slotName, 0, num) == 0;
		}

		// Token: 0x04001FB2 RID: 8114
		public string name;

		// Token: 0x04001FB3 RID: 8115
		public string slot;

		// Token: 0x04001FB4 RID: 8116
		public string rule;
	}

	// Token: 0x02000502 RID: 1282
	[PublicizedFrom(EAccessModifier.Private)]
	public class EquippedPart
	{
		// Token: 0x04001FB5 RID: 8117
		public string name;

		// Token: 0x04001FB6 RID: 8118
		public PlayerEquippedSlots.PartInfo partInfo;

		// Token: 0x04001FB7 RID: 8119
		public bool wasShowing;

		// Token: 0x04001FB8 RID: 8120
		public bool isShowing;
	}
}

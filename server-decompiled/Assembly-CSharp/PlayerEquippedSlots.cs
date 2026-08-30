using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004F8 RID: 1272
public class PlayerEquippedSlots : MonoBehaviour
{
	// Token: 0x060029A3 RID: 10659 RVA: 0x00105CCE File Offset: 0x00103ECE
	public void Init(Transform outfit)
	{
		this.outfitXF = outfit;
		if (this.outfitXF != null)
		{
			this._DisableAllNASubmeshes();
		}
	}

	// Token: 0x060029A4 RID: 10660 RVA: 0x00105CEC File Offset: 0x00103EEC
	public void ListParts()
	{
		int count = this.parts.Count;
		for (int i = 0; i < count; i++)
		{
			Log.Warning(this.parts[i].name);
		}
	}

	// Token: 0x060029A5 RID: 10661 RVA: 0x00105D28 File Offset: 0x00103F28
	public void ListEquipment()
	{
		int count = this.equippedParts.Count;
		for (int i = 0; i < count; i++)
		{
			Log.Warning(this.equippedParts[i].name);
		}
	}

	// Token: 0x060029A6 RID: 10662 RVA: 0x00105D64 File Offset: 0x00103F64
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

	// Token: 0x060029A7 RID: 10663 RVA: 0x00105DA8 File Offset: 0x00103FA8
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

	// Token: 0x060029A8 RID: 10664 RVA: 0x00105E30 File Offset: 0x00104030
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

	// Token: 0x060029A9 RID: 10665 RVA: 0x00105EA0 File Offset: 0x001040A0
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

	// Token: 0x060029AA RID: 10666 RVA: 0x00106009 File Offset: 0x00104209
	[PublicizedFrom(EAccessModifier.Private)]
	public Transform _GetNAOutfit()
	{
		return this.outfitXF;
	}

	// Token: 0x060029AB RID: 10667 RVA: 0x00106014 File Offset: 0x00104214
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

	// Token: 0x060029AC RID: 10668 RVA: 0x00106080 File Offset: 0x00104280
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

	// Token: 0x04001F97 RID: 8087
	public List<PlayerEquippedSlots.PartInfo> parts;

	// Token: 0x04001F98 RID: 8088
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<PlayerEquippedSlots.EquippedPart> equippedParts = new List<PlayerEquippedSlots.EquippedPart>();

	// Token: 0x04001F99 RID: 8089
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform outfitXF;

	// Token: 0x020004F9 RID: 1273
	[Serializable]
	public class PartInfo
	{
		// Token: 0x060029AE RID: 10670 RVA: 0x001060EA File Offset: 0x001042EA
		public bool IsInSlot(string slotReference)
		{
			return PlayerEquippedSlots.PartInfo.RefMatchesSlot(slotReference, this.slot);
		}

		// Token: 0x060029AF RID: 10671 RVA: 0x001060F8 File Offset: 0x001042F8
		public static bool RefMatchesSlot(string slotReference, string slotName)
		{
			int num = slotReference.IndexOf("*");
			if (num == -1)
			{
				return slotReference.Equals(slotName);
			}
			return string.Compare(slotReference, 0, slotName, 0, num) == 0;
		}

		// Token: 0x04001F9A RID: 8090
		public string name;

		// Token: 0x04001F9B RID: 8091
		public string slot;

		// Token: 0x04001F9C RID: 8092
		public string rule;
	}

	// Token: 0x020004FA RID: 1274
	[PublicizedFrom(EAccessModifier.Private)]
	public class EquippedPart
	{
		// Token: 0x04001F9D RID: 8093
		public string name;

		// Token: 0x04001F9E RID: 8094
		public PlayerEquippedSlots.PartInfo partInfo;

		// Token: 0x04001F9F RID: 8095
		public bool wasShowing;

		// Token: 0x04001FA0 RID: 8096
		public bool isShowing;
	}
}

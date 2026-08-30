using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004F3 RID: 1267
public class PlayerEquippedSlots : MonoBehaviour
{
	// Token: 0x06002979 RID: 10617 RVA: 0x00104CC2 File Offset: 0x00102EC2
	public void Init(Transform outfit)
	{
		this.outfitXF = outfit;
		if (this.outfitXF != null)
		{
			this._DisableAllNASubmeshes();
		}
	}

	// Token: 0x0600297A RID: 10618 RVA: 0x00104CE0 File Offset: 0x00102EE0
	public void ListParts()
	{
		int count = this.parts.Count;
		for (int i = 0; i < count; i++)
		{
			Log.Warning(this.parts[i].name);
		}
	}

	// Token: 0x0600297B RID: 10619 RVA: 0x00104D1C File Offset: 0x00102F1C
	public void ListEquipment()
	{
		int count = this.equippedParts.Count;
		for (int i = 0; i < count; i++)
		{
			Log.Warning(this.equippedParts[i].name);
		}
	}

	// Token: 0x0600297C RID: 10620 RVA: 0x00104D58 File Offset: 0x00102F58
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

	// Token: 0x0600297D RID: 10621 RVA: 0x00104D9C File Offset: 0x00102F9C
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

	// Token: 0x0600297E RID: 10622 RVA: 0x00104E24 File Offset: 0x00103024
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

	// Token: 0x0600297F RID: 10623 RVA: 0x00104E94 File Offset: 0x00103094
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

	// Token: 0x06002980 RID: 10624 RVA: 0x00104FFD File Offset: 0x001031FD
	[PublicizedFrom(EAccessModifier.Private)]
	public Transform _GetNAOutfit()
	{
		return this.outfitXF;
	}

	// Token: 0x06002981 RID: 10625 RVA: 0x00105008 File Offset: 0x00103208
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

	// Token: 0x06002982 RID: 10626 RVA: 0x00105074 File Offset: 0x00103274
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

	// Token: 0x04001F68 RID: 8040
	public List<PlayerEquippedSlots.PartInfo> parts;

	// Token: 0x04001F69 RID: 8041
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<PlayerEquippedSlots.EquippedPart> equippedParts = new List<PlayerEquippedSlots.EquippedPart>();

	// Token: 0x04001F6A RID: 8042
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform outfitXF;

	// Token: 0x020004F4 RID: 1268
	[Serializable]
	public class PartInfo
	{
		// Token: 0x06002984 RID: 10628 RVA: 0x001050DE File Offset: 0x001032DE
		public bool IsInSlot(string slotReference)
		{
			return PlayerEquippedSlots.PartInfo.RefMatchesSlot(slotReference, this.slot);
		}

		// Token: 0x06002985 RID: 10629 RVA: 0x001050EC File Offset: 0x001032EC
		public static bool RefMatchesSlot(string slotReference, string slotName)
		{
			int num = slotReference.IndexOf("*");
			if (num == -1)
			{
				return slotReference.Equals(slotName);
			}
			return string.Compare(slotReference, 0, slotName, 0, num) == 0;
		}

		// Token: 0x04001F6B RID: 8043
		public string name;

		// Token: 0x04001F6C RID: 8044
		public string slot;

		// Token: 0x04001F6D RID: 8045
		public string rule;
	}

	// Token: 0x020004F5 RID: 1269
	[PublicizedFrom(EAccessModifier.Private)]
	public class EquippedPart
	{
		// Token: 0x04001F6E RID: 8046
		public string name;

		// Token: 0x04001F6F RID: 8047
		public PlayerEquippedSlots.PartInfo partInfo;

		// Token: 0x04001F70 RID: 8048
		public bool wasShowing;

		// Token: 0x04001F71 RID: 8049
		public bool isShowing;
	}
}

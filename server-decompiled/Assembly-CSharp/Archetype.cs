using System;
using System.Collections.Generic;
using UniLinq;

// Token: 0x02000A58 RID: 2648
public class Archetype
{
	// Token: 0x170008A4 RID: 2212
	// (get) Token: 0x06004F6A RID: 20330 RVA: 0x001E453B File Offset: 0x001E273B
	// (set) Token: 0x06004F6B RID: 20331 RVA: 0x001E4550 File Offset: 0x001E2750
	public string Sex
	{
		get
		{
			if (!this.IsMale)
			{
				return "Female";
			}
			return "Male";
		}
		set
		{
			this.IsMale = value.ToLower() == "male";
		}
	}

	// Token: 0x170008A5 RID: 2213
	// (get) Token: 0x06004F6C RID: 20332 RVA: 0x001E4568 File Offset: 0x001E2768
	public bool ShowInList
	{
		get
		{
			return this.Name != "BaseMale" && this.Name != "BaseFemale";
		}
	}

	// Token: 0x06004F6D RID: 20333 RVA: 0x001E4590 File Offset: 0x001E2790
	public Archetype(string _name, bool _isMale, bool _canCustomize)
	{
		this.Name = _name;
		this.IsMale = _isMale;
		this.CanCustomize = _canCustomize;
	}

	// Token: 0x06004F6E RID: 20334 RVA: 0x001E45FA File Offset: 0x001E27FA
	public static void InitializeStatics()
	{
		Archetype.s_Archetypes = new CaseInsensitiveStringDictionary<Archetype>();
	}

	// Token: 0x06004F6F RID: 20335 RVA: 0x001E4608 File Offset: 0x001E2808
	public static void SetArchetype(Archetype archetype)
	{
		if (Archetype.s_Archetypes == null)
		{
			return;
		}
		if (Archetype.s_Archetypes.ContainsKey(archetype.Name))
		{
			Archetype.s_Archetypes[archetype.Name] = archetype;
			return;
		}
		Archetype.s_Archetypes[archetype.Name] = archetype;
		if (!archetype.CanCustomize)
		{
			ProfileSDF.SaveArchetype(archetype.Name, archetype.IsMale);
		}
	}

	// Token: 0x06004F70 RID: 20336 RVA: 0x001E466B File Offset: 0x001E286B
	public static Archetype GetArchetype(string name)
	{
		if (Archetype.s_Archetypes == null)
		{
			return null;
		}
		if (!Archetype.s_Archetypes.ContainsKey(name))
		{
			return null;
		}
		return Archetype.s_Archetypes[name];
	}

	// Token: 0x06004F71 RID: 20337 RVA: 0x001E4690 File Offset: 0x001E2890
	public void AddEquipmentSlot(SDCSUtils.SlotData slotData)
	{
		if (this.Equipment == null)
		{
			this.Equipment = new List<SDCSUtils.SlotData>();
		}
		this.Equipment.Add(slotData);
	}

	// Token: 0x06004F72 RID: 20338 RVA: 0x001E46B4 File Offset: 0x001E28B4
	public Archetype Clone()
	{
		return new Archetype(this.Name, this.IsMale, this.CanCustomize)
		{
			CanCustomize = this.CanCustomize,
			IsMale = this.IsMale,
			Race = this.Race,
			Variant = this.Variant,
			Hair = this.Hair,
			HairColor = this.HairColor,
			MustacheName = this.MustacheName,
			ChopsName = this.ChopsName,
			BeardName = this.BeardName,
			EyeColorName = this.EyeColorName
		};
	}

	// Token: 0x06004F73 RID: 20339 RVA: 0x001E4750 File Offset: 0x001E2950
	public static void SaveArchetypesToFile()
	{
		SDCSArchetypesFromXml.Save("archetypes", Archetype.s_Archetypes.Values.ToList<Archetype>());
	}

	// Token: 0x04003D4A RID: 15690
	public static Dictionary<string, Archetype> s_Archetypes;

	// Token: 0x04003D4B RID: 15691
	public string Name;

	// Token: 0x04003D4C RID: 15692
	public string Race;

	// Token: 0x04003D4D RID: 15693
	public int Variant;

	// Token: 0x04003D4E RID: 15694
	public string Hair = "";

	// Token: 0x04003D4F RID: 15695
	public string HairColor = "";

	// Token: 0x04003D50 RID: 15696
	public string MustacheName = "";

	// Token: 0x04003D51 RID: 15697
	public string ChopsName = "";

	// Token: 0x04003D52 RID: 15698
	public string BeardName = "";

	// Token: 0x04003D53 RID: 15699
	public string EyeColorName = "Blue01";

	// Token: 0x04003D54 RID: 15700
	public bool IsMale;

	// Token: 0x04003D55 RID: 15701
	public bool CanCustomize;

	// Token: 0x04003D56 RID: 15702
	public List<SDCSUtils.SlotData> Equipment;
}

using System;
using System.Collections.Generic;
using UniLinq;

// Token: 0x02000A4E RID: 2638
public class Archetype
{
	// Token: 0x1700089E RID: 2206
	// (get) Token: 0x06004F46 RID: 20294 RVA: 0x001E3453 File Offset: 0x001E1653
	// (set) Token: 0x06004F47 RID: 20295 RVA: 0x001E3468 File Offset: 0x001E1668
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

	// Token: 0x1700089F RID: 2207
	// (get) Token: 0x06004F48 RID: 20296 RVA: 0x001E3480 File Offset: 0x001E1680
	public bool ShowInList
	{
		get
		{
			return this.Name != "BaseMale" && this.Name != "BaseFemale";
		}
	}

	// Token: 0x06004F49 RID: 20297 RVA: 0x001E34A8 File Offset: 0x001E16A8
	public Archetype(string _name, bool _isMale, bool _canCustomize)
	{
		this.Name = _name;
		this.IsMale = _isMale;
		this.CanCustomize = _canCustomize;
	}

	// Token: 0x06004F4A RID: 20298 RVA: 0x001E3512 File Offset: 0x001E1712
	public static void InitializeStatics()
	{
		Archetype.s_Archetypes = new CaseInsensitiveStringDictionary<Archetype>();
	}

	// Token: 0x06004F4B RID: 20299 RVA: 0x001E3520 File Offset: 0x001E1720
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

	// Token: 0x06004F4C RID: 20300 RVA: 0x001E3583 File Offset: 0x001E1783
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

	// Token: 0x06004F4D RID: 20301 RVA: 0x001E35A8 File Offset: 0x001E17A8
	public void AddEquipmentSlot(SDCSUtils.SlotData slotData)
	{
		if (this.Equipment == null)
		{
			this.Equipment = new List<SDCSUtils.SlotData>();
		}
		this.Equipment.Add(slotData);
	}

	// Token: 0x06004F4E RID: 20302 RVA: 0x001E35CC File Offset: 0x001E17CC
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

	// Token: 0x06004F4F RID: 20303 RVA: 0x001E3668 File Offset: 0x001E1868
	public static void SaveArchetypesToFile()
	{
		SDCSArchetypesFromXml.Save("archetypes", Archetype.s_Archetypes.Values.ToList<Archetype>());
	}

	// Token: 0x04003D29 RID: 15657
	public static Dictionary<string, Archetype> s_Archetypes;

	// Token: 0x04003D2A RID: 15658
	public string Name;

	// Token: 0x04003D2B RID: 15659
	public string Race;

	// Token: 0x04003D2C RID: 15660
	public int Variant;

	// Token: 0x04003D2D RID: 15661
	public string Hair = "";

	// Token: 0x04003D2E RID: 15662
	public string HairColor = "";

	// Token: 0x04003D2F RID: 15663
	public string MustacheName = "";

	// Token: 0x04003D30 RID: 15664
	public string ChopsName = "";

	// Token: 0x04003D31 RID: 15665
	public string BeardName = "";

	// Token: 0x04003D32 RID: 15666
	public string EyeColorName = "Blue01";

	// Token: 0x04003D33 RID: 15667
	public bool IsMale;

	// Token: 0x04003D34 RID: 15668
	public bool CanCustomize;

	// Token: 0x04003D35 RID: 15669
	public List<SDCSUtils.SlotData> Equipment;
}

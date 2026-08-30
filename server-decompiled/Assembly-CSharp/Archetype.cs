using System;
using System.Collections.Generic;
using UniLinq;

// Token: 0x02000A33 RID: 2611
public class Archetype
{
	// Token: 0x1700086C RID: 2156
	// (get) Token: 0x06004EF1 RID: 20209 RVA: 0x001DF70B File Offset: 0x001DD90B
	// (set) Token: 0x06004EF2 RID: 20210 RVA: 0x001DF720 File Offset: 0x001DD920
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

	// Token: 0x1700086D RID: 2157
	// (get) Token: 0x06004EF3 RID: 20211 RVA: 0x001DF738 File Offset: 0x001DD938
	public bool ShowInList
	{
		get
		{
			return this.Name != "BaseMale" && this.Name != "BaseFemale";
		}
	}

	// Token: 0x06004EF4 RID: 20212 RVA: 0x001DF760 File Offset: 0x001DD960
	public Archetype(string _name, bool _isMale, bool _canCustomize)
	{
		this.Name = _name;
		this.IsMale = _isMale;
		this.CanCustomize = _canCustomize;
	}

	// Token: 0x06004EF5 RID: 20213 RVA: 0x001DF7CA File Offset: 0x001DD9CA
	public static void InitializeStatics()
	{
		Archetype.s_Archetypes = new CaseInsensitiveStringDictionary<Archetype>();
	}

	// Token: 0x06004EF6 RID: 20214 RVA: 0x001DF7D8 File Offset: 0x001DD9D8
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

	// Token: 0x06004EF7 RID: 20215 RVA: 0x001DF83B File Offset: 0x001DDA3B
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

	// Token: 0x06004EF8 RID: 20216 RVA: 0x001DF860 File Offset: 0x001DDA60
	public void AddEquipmentSlot(SDCSUtils.SlotData slotData)
	{
		if (this.Equipment == null)
		{
			this.Equipment = new List<SDCSUtils.SlotData>();
		}
		this.Equipment.Add(slotData);
	}

	// Token: 0x06004EF9 RID: 20217 RVA: 0x001DF884 File Offset: 0x001DDA84
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

	// Token: 0x06004EFA RID: 20218 RVA: 0x001DF920 File Offset: 0x001DDB20
	public static void SaveArchetypesToFile()
	{
		SDCSArchetypesFromXml.Save("archetypes", Archetype.s_Archetypes.Values.ToList<Archetype>());
	}

	// Token: 0x04003C8B RID: 15499
	public static Dictionary<string, Archetype> s_Archetypes;

	// Token: 0x04003C8C RID: 15500
	public string Name;

	// Token: 0x04003C8D RID: 15501
	public string Race;

	// Token: 0x04003C8E RID: 15502
	public int Variant;

	// Token: 0x04003C8F RID: 15503
	public string Hair = "";

	// Token: 0x04003C90 RID: 15504
	public string HairColor = "";

	// Token: 0x04003C91 RID: 15505
	public string MustacheName = "";

	// Token: 0x04003C92 RID: 15506
	public string ChopsName = "";

	// Token: 0x04003C93 RID: 15507
	public string BeardName = "";

	// Token: 0x04003C94 RID: 15508
	public string EyeColorName = "Blue01";

	// Token: 0x04003C95 RID: 15509
	public bool IsMale;

	// Token: 0x04003C96 RID: 15510
	public bool CanCustomize;

	// Token: 0x04003C97 RID: 15511
	public List<SDCSUtils.SlotData> Equipment;
}

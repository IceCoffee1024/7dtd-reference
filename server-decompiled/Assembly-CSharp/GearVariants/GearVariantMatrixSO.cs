using System;
using System.Collections.Generic;
using UnityEngine;

namespace GearVariants
{
	// Token: 0x020016F1 RID: 5873
	public class GearVariantMatrixSO : ScriptableObject
	{
		// Token: 0x17001664 RID: 5732
		// (get) Token: 0x0600B74B RID: 46923 RVA: 0x0043FA24 File Offset: 0x0043DC24
		public static GearVariantMatrixSO Instance
		{
			get
			{
				if (GearVariantMatrixSO._instance == null)
				{
					if (!Application.isPlaying)
					{
						LoadManager.InitSync();
					}
					GearVariantMatrixSO._instance = DataLoader.LoadAsset<GearVariantMatrixSO>("@:Entities/Player/Common/GearVariantMatrix.asset", false);
				}
				return GearVariantMatrixSO._instance;
			}
		}

		// Token: 0x0600B74C RID: 46924 RVA: 0x0043FA54 File Offset: 0x0043DC54
		[PublicizedFrom(EAccessModifier.Private)]
		public void EnsureIndex(GearVariantMatrixSO.Sex sex)
		{
			List<string> list;
			if (sex != GearVariantMatrixSO.Sex.Male)
			{
				SexGearTables sexGearTables = this.female;
				list = ((sexGearTables != null) ? sexGearTables.gearPaths : null);
			}
			else
			{
				SexGearTables sexGearTables2 = this.male;
				list = ((sexGearTables2 != null) ? sexGearTables2.gearPaths : null);
			}
			List<string> list2 = list ?? new List<string>();
			int count = list2.Count;
			if (sex == GearVariantMatrixSO.Sex.Male)
			{
				if (this._maleIndex == null || this._maleIndexVersionRow != count)
				{
					this._maleIndex = new Dictionary<string, int>(StringComparer.Ordinal);
					for (int i = 0; i < list2.Count; i++)
					{
						this._maleIndex[list2[i]] = i;
					}
					this._maleIndexVersionRow = count;
					return;
				}
			}
			else if (this._femaleIndex == null || this._femaleIndexVersionRow != count)
			{
				this._femaleIndex = new Dictionary<string, int>(StringComparer.Ordinal);
				for (int j = 0; j < list2.Count; j++)
				{
					this._femaleIndex[list2[j]] = j;
				}
				this._femaleIndexVersionRow = count;
			}
		}

		// Token: 0x0600B74D RID: 46925 RVA: 0x0043FB3A File Offset: 0x0043DD3A
		[PublicizedFrom(EAccessModifier.Private)]
		public SexGearTables GetSexTables(GearVariantMatrixSO.Sex sex)
		{
			if (sex != GearVariantMatrixSO.Sex.Male)
			{
				return this.female;
			}
			return this.male;
		}

		// Token: 0x0600B74E RID: 46926 RVA: 0x0043FB4C File Offset: 0x0043DD4C
		[PublicizedFrom(EAccessModifier.Private)]
		public static bool TryParseSex(string sexName, out GearVariantMatrixSO.Sex sex)
		{
			string text = sexName.ToLower();
			if (text == "male")
			{
				sex = GearVariantMatrixSO.Sex.Male;
				return true;
			}
			if (!(text == "female"))
			{
				sex = GearVariantMatrixSO.Sex.Male;
				return false;
			}
			sex = GearVariantMatrixSO.Sex.Female;
			return true;
		}

		// Token: 0x0600B74F RID: 46927 RVA: 0x0043FB8C File Offset: 0x0043DD8C
		[PublicizedFrom(EAccessModifier.Private)]
		public static bool TryParsePart(string partName, out GearVariantMatrixSO.GearPart part)
		{
			if (partName == "head")
			{
				part = GearVariantMatrixSO.GearPart.Head;
				return true;
			}
			if (partName == "hands")
			{
				part = GearVariantMatrixSO.GearPart.Hands;
				return true;
			}
			if (partName == "feet")
			{
				part = GearVariantMatrixSO.GearPart.Feet;
				return true;
			}
			string text = ((partName != null) ? partName.Trim().ToLowerInvariant() : null);
			if (text == "head")
			{
				part = GearVariantMatrixSO.GearPart.Head;
				return true;
			}
			if (text == "hands")
			{
				part = GearVariantMatrixSO.GearPart.Hands;
				return true;
			}
			if (!(text == "feet"))
			{
				part = GearVariantMatrixSO.GearPart.Hands;
				return false;
			}
			part = GearVariantMatrixSO.GearPart.Feet;
			return true;
		}

		// Token: 0x0600B750 RID: 46928 RVA: 0x0043FC1F File Offset: 0x0043DE1F
		[PublicizedFrom(EAccessModifier.Private)]
		public StringTable2D GetTable(GearVariantMatrixSO.Sex sex, GearVariantMatrixSO.GearPart part)
		{
			SexGearTables sexTables = this.GetSexTables(sex);
			if (sexTables == null)
			{
				return null;
			}
			return sexTables.GetTable(part);
		}

		// Token: 0x0600B751 RID: 46929 RVA: 0x0043FC34 File Offset: 0x0043DE34
		[PublicizedFrom(EAccessModifier.Private)]
		public bool TryGetIndices(GearVariantMatrixSO.Sex sex, string sourceGearPath, string targetBodyPath, out int row, out int col)
		{
			row = -1;
			col = -1;
			if (this.GetSexTables(sex) == null)
			{
				return false;
			}
			this.EnsureIndex(sex);
			Dictionary<string, int> dictionary = ((sex == GearVariantMatrixSO.Sex.Male) ? this._maleIndex : this._femaleIndex);
			return dictionary != null && dictionary.TryGetValue(sourceGearPath, out row) && dictionary.TryGetValue(targetBodyPath, out col);
		}

		// Token: 0x0600B752 RID: 46930 RVA: 0x0043FC90 File Offset: 0x0043DE90
		[PublicizedFrom(EAccessModifier.Private)]
		public string GetVariantOrEmpty(GearVariantMatrixSO.Sex sex, GearVariantMatrixSO.GearPart part, string sourceGearPath, string targetBodyPath)
		{
			if (string.IsNullOrEmpty(sourceGearPath) || string.IsNullOrEmpty(targetBodyPath))
			{
				return string.Empty;
			}
			StringTable2D table = this.GetTable(sex, part);
			if (table == null)
			{
				return string.Empty;
			}
			this.EnsureShapes();
			int num;
			int num2;
			if (!this.TryGetIndices(sex, sourceGearPath, targetBodyPath, out num, out num2))
			{
				return string.Empty;
			}
			if (num < 0 || num >= table.rows.Count)
			{
				return string.Empty;
			}
			if (num2 < 0 || num2 >= table.columnKeys.Count)
			{
				return string.Empty;
			}
			string text = table.Get(num, num2);
			if (!string.IsNullOrEmpty(text))
			{
				return text;
			}
			return string.Empty;
		}

		// Token: 0x0600B753 RID: 46931 RVA: 0x0043FD2C File Offset: 0x0043DF2C
		[PublicizedFrom(EAccessModifier.Private)]
		public bool TryGetVariant(GearVariantMatrixSO.Sex sex, GearVariantMatrixSO.GearPart part, string sourceGearPath, string targetBodyPath, out string variant)
		{
			variant = string.Empty;
			if (string.IsNullOrEmpty(sourceGearPath) || string.IsNullOrEmpty(targetBodyPath))
			{
				return false;
			}
			StringTable2D table = this.GetTable(sex, part);
			if (table == null)
			{
				return false;
			}
			this.EnsureShapes();
			int num;
			int num2;
			if (!this.TryGetIndices(sex, sourceGearPath, targetBodyPath, out num, out num2))
			{
				return false;
			}
			if (num < 0 || num >= table.rows.Count)
			{
				return false;
			}
			if (num2 < 0 || num2 >= table.columnKeys.Count)
			{
				return false;
			}
			variant = table.Get(num, num2) ?? string.Empty;
			return true;
		}

		// Token: 0x0600B754 RID: 46932 RVA: 0x0043FDB8 File Offset: 0x0043DFB8
		public string GetVariantOrEmpty(string sex, string part, string sourceGearPath, string targetBodyPath)
		{
			GearVariantMatrixSO.GearPart gearPart;
			if (!GearVariantMatrixSO.TryParsePart(part, out gearPart))
			{
				return string.Empty;
			}
			GearVariantMatrixSO.Sex sex2;
			if (!GearVariantMatrixSO.TryParseSex(sex, out sex2))
			{
				return string.Empty;
			}
			return this.GetVariantOrEmpty(sex2, gearPart, sourceGearPath, targetBodyPath);
		}

		// Token: 0x0600B755 RID: 46933 RVA: 0x0043FDF0 File Offset: 0x0043DFF0
		public bool TryGetVariant(string sex, string part, string sourceGearPath, string targetBodyPath, out string variant)
		{
			GearVariantMatrixSO.GearPart gearPart;
			if (!GearVariantMatrixSO.TryParsePart(part, out gearPart))
			{
				variant = string.Empty;
				return false;
			}
			GearVariantMatrixSO.Sex sex2;
			if (!GearVariantMatrixSO.TryParseSex(sex, out sex2))
			{
				variant = string.Empty;
				return false;
			}
			return this.TryGetVariant(sex2, gearPart, sourceGearPath, targetBodyPath, out variant);
		}

		// Token: 0x0600B756 RID: 46934 RVA: 0x0043FE32 File Offset: 0x0043E032
		public void EnsureShapes()
		{
			SexGearTables sexGearTables = this.male;
			if (sexGearTables != null)
			{
				sexGearTables.EnsureShapes();
			}
			SexGearTables sexGearTables2 = this.female;
			if (sexGearTables2 == null)
			{
				return;
			}
			sexGearTables2.EnsureShapes();
		}

		// Token: 0x040088FD RID: 35069
		[Header("Sex: Male")]
		public SexGearTables male = new SexGearTables();

		// Token: 0x040088FE RID: 35070
		[Header("Sex: Female")]
		public SexGearTables female = new SexGearTables();

		// Token: 0x040088FF RID: 35071
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public static GearVariantMatrixSO _instance;

		// Token: 0x04008900 RID: 35072
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Dictionary<string, int> _maleIndex;

		// Token: 0x04008901 RID: 35073
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int _maleIndexVersionRow;

		// Token: 0x04008902 RID: 35074
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Dictionary<string, int> _femaleIndex;

		// Token: 0x04008903 RID: 35075
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int _femaleIndexVersionRow;

		// Token: 0x020016F2 RID: 5874
		public enum GearPart
		{
			// Token: 0x04008905 RID: 35077
			Head,
			// Token: 0x04008906 RID: 35078
			Hands,
			// Token: 0x04008907 RID: 35079
			Feet
		}

		// Token: 0x020016F3 RID: 5875
		public enum Sex
		{
			// Token: 0x04008909 RID: 35081
			Male,
			// Token: 0x0400890A RID: 35082
			Female
		}
	}
}

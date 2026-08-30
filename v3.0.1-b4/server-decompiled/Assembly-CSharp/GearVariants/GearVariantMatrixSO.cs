using System;
using System.Collections.Generic;
using UnityEngine;

namespace GearVariants
{
	// Token: 0x020016CB RID: 5835
	public class GearVariantMatrixSO : ScriptableObject
	{
		// Token: 0x170015F2 RID: 5618
		// (get) Token: 0x0600B5C8 RID: 46536 RVA: 0x00439CAC File Offset: 0x00437EAC
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

		// Token: 0x0600B5C9 RID: 46537 RVA: 0x00439CDC File Offset: 0x00437EDC
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

		// Token: 0x0600B5CA RID: 46538 RVA: 0x00439DC2 File Offset: 0x00437FC2
		[PublicizedFrom(EAccessModifier.Private)]
		public SexGearTables GetSexTables(GearVariantMatrixSO.Sex sex)
		{
			if (sex != GearVariantMatrixSO.Sex.Male)
			{
				return this.female;
			}
			return this.male;
		}

		// Token: 0x0600B5CB RID: 46539 RVA: 0x00439DD4 File Offset: 0x00437FD4
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

		// Token: 0x0600B5CC RID: 46540 RVA: 0x00439E14 File Offset: 0x00438014
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

		// Token: 0x0600B5CD RID: 46541 RVA: 0x00439EA7 File Offset: 0x004380A7
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

		// Token: 0x0600B5CE RID: 46542 RVA: 0x00439EBC File Offset: 0x004380BC
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

		// Token: 0x0600B5CF RID: 46543 RVA: 0x00439F18 File Offset: 0x00438118
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

		// Token: 0x0600B5D0 RID: 46544 RVA: 0x00439FB4 File Offset: 0x004381B4
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

		// Token: 0x0600B5D1 RID: 46545 RVA: 0x0043A040 File Offset: 0x00438240
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

		// Token: 0x0600B5D2 RID: 46546 RVA: 0x0043A078 File Offset: 0x00438278
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

		// Token: 0x0600B5D3 RID: 46547 RVA: 0x0043A0BA File Offset: 0x004382BA
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

		// Token: 0x040087E1 RID: 34785
		[Header("Sex: Male")]
		public SexGearTables male = new SexGearTables();

		// Token: 0x040087E2 RID: 34786
		[Header("Sex: Female")]
		public SexGearTables female = new SexGearTables();

		// Token: 0x040087E3 RID: 34787
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public static GearVariantMatrixSO _instance;

		// Token: 0x040087E4 RID: 34788
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Dictionary<string, int> _maleIndex;

		// Token: 0x040087E5 RID: 34789
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int _maleIndexVersionRow;

		// Token: 0x040087E6 RID: 34790
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Dictionary<string, int> _femaleIndex;

		// Token: 0x040087E7 RID: 34791
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int _femaleIndexVersionRow;

		// Token: 0x020016CC RID: 5836
		public enum GearPart
		{
			// Token: 0x040087E9 RID: 34793
			Head,
			// Token: 0x040087EA RID: 34794
			Hands,
			// Token: 0x040087EB RID: 34795
			Feet
		}

		// Token: 0x020016CD RID: 5837
		public enum Sex
		{
			// Token: 0x040087ED RID: 34797
			Male,
			// Token: 0x040087EE RID: 34798
			Female
		}
	}
}

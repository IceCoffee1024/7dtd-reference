using System;
using System.Collections.Generic;
using UnityEngine;

namespace GearVariants
{
	// Token: 0x0200174E RID: 5966
	public class GearVariantMatrixSO : ScriptableObject
	{
		// Token: 0x1700174E RID: 5966
		// (get) Token: 0x0600B9B6 RID: 47542 RVA: 0x004478F0 File Offset: 0x00445AF0
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

		// Token: 0x0600B9B7 RID: 47543 RVA: 0x00447920 File Offset: 0x00445B20
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

		// Token: 0x0600B9B8 RID: 47544 RVA: 0x00447A06 File Offset: 0x00445C06
		[PublicizedFrom(EAccessModifier.Private)]
		public SexGearTables GetSexTables(GearVariantMatrixSO.Sex sex)
		{
			if (sex != GearVariantMatrixSO.Sex.Male)
			{
				return this.female;
			}
			return this.male;
		}

		// Token: 0x0600B9B9 RID: 47545 RVA: 0x00447A18 File Offset: 0x00445C18
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

		// Token: 0x0600B9BA RID: 47546 RVA: 0x00447A58 File Offset: 0x00445C58
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

		// Token: 0x0600B9BB RID: 47547 RVA: 0x00447AEB File Offset: 0x00445CEB
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

		// Token: 0x0600B9BC RID: 47548 RVA: 0x00447B00 File Offset: 0x00445D00
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

		// Token: 0x0600B9BD RID: 47549 RVA: 0x00447B5C File Offset: 0x00445D5C
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

		// Token: 0x0600B9BE RID: 47550 RVA: 0x00447BF8 File Offset: 0x00445DF8
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

		// Token: 0x0600B9BF RID: 47551 RVA: 0x00447C84 File Offset: 0x00445E84
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

		// Token: 0x0600B9C0 RID: 47552 RVA: 0x00447CBC File Offset: 0x00445EBC
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

		// Token: 0x0600B9C1 RID: 47553 RVA: 0x00447CFE File Offset: 0x00445EFE
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

		// Token: 0x04008AAB RID: 35499
		[Header("Sex: Male")]
		public SexGearTables male = new SexGearTables();

		// Token: 0x04008AAC RID: 35500
		[Header("Sex: Female")]
		public SexGearTables female = new SexGearTables();

		// Token: 0x04008AAD RID: 35501
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public static GearVariantMatrixSO _instance;

		// Token: 0x04008AAE RID: 35502
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Dictionary<string, int> _maleIndex;

		// Token: 0x04008AAF RID: 35503
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int _maleIndexVersionRow;

		// Token: 0x04008AB0 RID: 35504
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Dictionary<string, int> _femaleIndex;

		// Token: 0x04008AB1 RID: 35505
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int _femaleIndexVersionRow;

		// Token: 0x0200174F RID: 5967
		public enum GearPart
		{
			// Token: 0x04008AB3 RID: 35507
			Head,
			// Token: 0x04008AB4 RID: 35508
			Hands,
			// Token: 0x04008AB5 RID: 35509
			Feet
		}

		// Token: 0x02001750 RID: 5968
		public enum Sex
		{
			// Token: 0x04008AB7 RID: 35511
			Male,
			// Token: 0x04008AB8 RID: 35512
			Female
		}
	}
}

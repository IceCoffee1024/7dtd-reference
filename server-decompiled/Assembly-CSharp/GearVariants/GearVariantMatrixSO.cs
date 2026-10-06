using System;
using System.Collections.Generic;
using UnityEngine;

namespace GearVariants
{
	// Token: 0x0200171D RID: 5917
	public class GearVariantMatrixSO : ScriptableObject
	{
		// Token: 0x17001706 RID: 5894
		// (get) Token: 0x0600B8A2 RID: 47266 RVA: 0x00443F44 File Offset: 0x00442144
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

		// Token: 0x0600B8A3 RID: 47267 RVA: 0x00443F74 File Offset: 0x00442174
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

		// Token: 0x0600B8A4 RID: 47268 RVA: 0x0044405A File Offset: 0x0044225A
		[PublicizedFrom(EAccessModifier.Private)]
		public SexGearTables GetSexTables(GearVariantMatrixSO.Sex sex)
		{
			if (sex != GearVariantMatrixSO.Sex.Male)
			{
				return this.female;
			}
			return this.male;
		}

		// Token: 0x0600B8A5 RID: 47269 RVA: 0x0044406C File Offset: 0x0044226C
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

		// Token: 0x0600B8A6 RID: 47270 RVA: 0x004440AC File Offset: 0x004422AC
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

		// Token: 0x0600B8A7 RID: 47271 RVA: 0x0044413F File Offset: 0x0044233F
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

		// Token: 0x0600B8A8 RID: 47272 RVA: 0x00444154 File Offset: 0x00442354
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

		// Token: 0x0600B8A9 RID: 47273 RVA: 0x004441B0 File Offset: 0x004423B0
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

		// Token: 0x0600B8AA RID: 47274 RVA: 0x0044424C File Offset: 0x0044244C
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

		// Token: 0x0600B8AB RID: 47275 RVA: 0x004442D8 File Offset: 0x004424D8
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

		// Token: 0x0600B8AC RID: 47276 RVA: 0x00444310 File Offset: 0x00442510
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

		// Token: 0x0600B8AD RID: 47277 RVA: 0x00444352 File Offset: 0x00442552
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

		// Token: 0x040089C2 RID: 35266
		[Header("Sex: Male")]
		public SexGearTables male = new SexGearTables();

		// Token: 0x040089C3 RID: 35267
		[Header("Sex: Female")]
		public SexGearTables female = new SexGearTables();

		// Token: 0x040089C4 RID: 35268
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public static GearVariantMatrixSO _instance;

		// Token: 0x040089C5 RID: 35269
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Dictionary<string, int> _maleIndex;

		// Token: 0x040089C6 RID: 35270
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int _maleIndexVersionRow;

		// Token: 0x040089C7 RID: 35271
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Dictionary<string, int> _femaleIndex;

		// Token: 0x040089C8 RID: 35272
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int _femaleIndexVersionRow;

		// Token: 0x0200171E RID: 5918
		public enum GearPart
		{
			// Token: 0x040089CA RID: 35274
			Head,
			// Token: 0x040089CB RID: 35275
			Hands,
			// Token: 0x040089CC RID: 35276
			Feet
		}

		// Token: 0x0200171F RID: 5919
		public enum Sex
		{
			// Token: 0x040089CE RID: 35278
			Male,
			// Token: 0x040089CF RID: 35279
			Female
		}
	}
}

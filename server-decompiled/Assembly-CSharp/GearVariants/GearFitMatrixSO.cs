using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace GearVariants
{
	// Token: 0x02001739 RID: 5945
	[NullableContext(1)]
	[Nullable(0)]
	public class GearFitMatrixSO : ScriptableObject
	{
		// Token: 0x1700174D RID: 5965
		// (get) Token: 0x0600B96B RID: 47467 RVA: 0x004462BF File Offset: 0x004444BF
		public static GearFitMatrixSO Instance
		{
			get
			{
				if (GearFitMatrixSO._instance != null)
				{
					return GearFitMatrixSO._instance;
				}
				if (!Application.isPlaying)
				{
					LoadManager.InitSync();
				}
				GearFitMatrixSO._instance = DataLoader.LoadAsset<GearFitMatrixSO>("@:Entities/Player/Common/GearFitMatrix.asset", false);
				return GearFitMatrixSO._instance;
			}
		}

		// Token: 0x0600B96C RID: 47468 RVA: 0x004462F5 File Offset: 0x004444F5
		[PublicizedFrom(EAccessModifier.Private)]
		public GenderFitTables GetGenderTables(GearFitMatrixSO.Gender gender)
		{
			if (gender != GearFitMatrixSO.Gender.Male)
			{
				return this.female;
			}
			return this.male;
		}

		// Token: 0x0600B96D RID: 47469 RVA: 0x00446308 File Offset: 0x00444508
		[PublicizedFrom(EAccessModifier.Private)]
		public static GearFitMatrixSO.Gender? ParseGender(string genderName)
		{
			string text = genderName.Trim().ToLowerInvariant();
			GearFitMatrixSO.Gender? gender;
			if (!(text == "male"))
			{
				if (!(text == "female"))
				{
					gender = null;
				}
				else
				{
					gender = new GearFitMatrixSO.Gender?(GearFitMatrixSO.Gender.Female);
				}
			}
			else
			{
				gender = new GearFitMatrixSO.Gender?(GearFitMatrixSO.Gender.Male);
			}
			return gender;
		}

		// Token: 0x0600B96E RID: 47470 RVA: 0x0044635C File Offset: 0x0044455C
		[PublicizedFrom(EAccessModifier.Private)]
		public static GearFitMatrixSO.GearPart? ParsePart(string partName)
		{
			string text = ((partName != null) ? partName.Trim().ToLowerInvariant() : null);
			GearFitMatrixSO.GearPart? gearPart;
			if (!(text == "head"))
			{
				if (!(text == "hands"))
				{
					if (!(text == "feet"))
					{
						gearPart = null;
					}
					else
					{
						gearPart = new GearFitMatrixSO.GearPart?(GearFitMatrixSO.GearPart.Feet);
					}
				}
				else
				{
					gearPart = new GearFitMatrixSO.GearPart?(GearFitMatrixSO.GearPart.Hands);
				}
			}
			else
			{
				gearPart = new GearFitMatrixSO.GearPart?(GearFitMatrixSO.GearPart.Head);
			}
			return gearPart;
		}

		// Token: 0x0600B96F RID: 47471 RVA: 0x004463CC File Offset: 0x004445CC
		public string GetVariantOrEmpty(string gender, string part, string sourceGearPath, string targetBodyPath)
		{
			if (string.IsNullOrEmpty(sourceGearPath) || string.IsNullOrEmpty(targetBodyPath))
			{
				return string.Empty;
			}
			GearFitMatrixSO.GearPart? gearPart = GearFitMatrixSO.ParsePart(part);
			if (gearPart != null)
			{
				GearFitMatrixSO.GearPart valueOrDefault = gearPart.GetValueOrDefault();
				GearFitMatrixSO.Gender? gender2 = GearFitMatrixSO.ParseGender(gender);
				if (gender2 != null)
				{
					GearFitMatrixSO.Gender valueOrDefault2 = gender2.GetValueOrDefault();
					GenderFitTables genderTables = this.GetGenderTables(valueOrDefault2);
					if (!genderTables.IsKnownGear(sourceGearPath) || !genderTables.IsKnownGear(targetBodyPath))
					{
						return string.Empty;
					}
					return genderTables.GetTable(valueOrDefault).GetVariantOrEmpty(sourceGearPath, targetBodyPath);
				}
			}
			return string.Empty;
		}

		// Token: 0x0600B970 RID: 47472 RVA: 0x0044645C File Offset: 0x0044465C
		public IReadOnlyList<BodyFit> GetFits(GearFitMatrixSO.Gender gender, GearFitMatrixSO.GearPart part, string wornKey)
		{
			GearFitEntry gearFitEntry = this.GetGenderTables(gender).GetTable(part).Find(wornKey);
			IReadOnlyList<BodyFit> readOnlyList = ((gearFitEntry != null) ? gearFitEntry.fits : null);
			return readOnlyList ?? Array.Empty<BodyFit>();
		}

		// Token: 0x0600B971 RID: 47473 RVA: 0x00446493 File Offset: 0x00444693
		public int FitCount(GearFitMatrixSO.Gender gender, GearFitMatrixSO.GearPart part, string wornKey)
		{
			GearFitEntry gearFitEntry = this.GetGenderTables(gender).GetTable(part).Find(wornKey);
			if (gearFitEntry == null)
			{
				return 0;
			}
			return gearFitEntry.fits.Count;
		}

		// Token: 0x0600B972 RID: 47474 RVA: 0x004464B8 File Offset: 0x004446B8
		public void SetFit(GearFitMatrixSO.Gender gender, GearFitMatrixSO.GearPart part, string wornKey, string wornGuid, string bodyKey, string bodyGuid, string variant)
		{
			if (string.IsNullOrEmpty(wornKey) || string.IsNullOrEmpty(bodyKey))
			{
				return;
			}
			PartFitTable table = this.GetGenderTables(gender).GetTable(part);
			GearFitEntry gearFitEntry = table.Find(wornKey);
			if (gearFitEntry == null)
			{
				if (string.IsNullOrEmpty(variant))
				{
					return;
				}
				gearFitEntry = GearFitMatrixSO.AddEntry(table, wornKey, wornGuid);
			}
			GearFitMatrixSO.SetFitInline(gearFitEntry, bodyKey, bodyGuid, variant);
			GearFitMatrixSO.PruneIfEmpty(table, gearFitEntry);
			table.InvalidateIndex();
		}

		// Token: 0x0600B973 RID: 47475 RVA: 0x00446520 File Offset: 0x00444720
		public int SetFitMany(GearFitMatrixSO.Gender gender, GearFitMatrixSO.GearPart part, string wornKey, string wornGuid, [TupleElementNames(new string[] { "bodyKey", "bodyGuid" })] [Nullable(new byte[] { 2, 0, 1, 1 })] IEnumerable<ValueTuple<string, string>> bodies, string variant)
		{
			if (string.IsNullOrEmpty(wornKey) || bodies == null)
			{
				return 0;
			}
			PartFitTable table = this.GetGenderTables(gender).GetTable(part);
			GearFitEntry gearFitEntry = table.Find(wornKey);
			int num = 0;
			foreach (ValueTuple<string, string> valueTuple in bodies)
			{
				if (!string.IsNullOrEmpty(valueTuple.Item1) && !string.Equals((gearFitEntry == null) ? "" : GearFitMatrixSO.VariantIn(gearFitEntry, valueTuple.Item1), variant, StringComparison.Ordinal))
				{
					if (gearFitEntry == null)
					{
						gearFitEntry = GearFitMatrixSO.AddEntry(table, wornKey, wornGuid);
					}
					GearFitMatrixSO.SetFitInline(gearFitEntry, valueTuple.Item1, valueTuple.Item2, variant);
					num++;
				}
			}
			if (gearFitEntry != null)
			{
				GearFitMatrixSO.PruneIfEmpty(table, gearFitEntry);
			}
			table.InvalidateIndex();
			return num;
		}

		// Token: 0x0600B974 RID: 47476 RVA: 0x004465F0 File Offset: 0x004447F0
		public GearFitMatrixSO.CopyResult CopyFits(GearFitMatrixSO.Gender gender, GearFitMatrixSO.GearPart part, string targetKey, string targetGuid, string sourceKey, GearFitMatrixSO.CopyMode mode, ISet<string> targetValidVariants)
		{
			PartFitTable table = this.GetGenderTables(gender).GetTable(part);
			GearFitEntry gearFitEntry = table.Find(sourceKey);
			if (gearFitEntry == null || string.Equals(sourceKey, targetKey, StringComparison.Ordinal))
			{
				return GearFitMatrixSO.CopyResult.Nothing;
			}
			GearFitEntry gearFitEntry2 = table.Find(targetKey) ?? GearFitMatrixSO.AddEntry(table, targetKey, targetGuid);
			if (mode == GearFitMatrixSO.CopyMode.Replace)
			{
				gearFitEntry2.fits.Clear();
			}
			int num = 0;
			int num2 = 0;
			foreach (BodyFit bodyFit in gearFitEntry.fits)
			{
				if (targetValidVariants != null && targetValidVariants.Contains(bodyFit.variant))
				{
					GearFitMatrixSO.SetFitInline(gearFitEntry2, bodyFit.bodyKey, bodyFit.bodyGuid, bodyFit.variant);
					num++;
				}
				else
				{
					num2++;
				}
			}
			GearFitMatrixSO.PruneIfEmpty(table, gearFitEntry2);
			table.InvalidateIndex();
			return new GearFitMatrixSO.CopyResult(num, num2, !GearFitMatrixSO.HasNonBaseVariant(targetValidVariants));
		}

		// Token: 0x0600B975 RID: 47477 RVA: 0x004466EC File Offset: 0x004448EC
		[PublicizedFrom(EAccessModifier.Private)]
		public static bool HasNonBaseVariant([Nullable(new byte[] { 2, 1 })] ISet<string> variants)
		{
			if (variants != null)
			{
				return variants.Any<string>((string v) => !string.IsNullOrEmpty(v));
			}
			return false;
		}

		// Token: 0x0600B976 RID: 47478 RVA: 0x00446718 File Offset: 0x00444918
		public int ClampToValidOptions(GearFitMatrixSO.Gender gender, GearFitMatrixSO.GearPart part, Func<string, HashSet<string>> validForWornKey)
		{
			PartFitTable table = this.GetGenderTables(gender).GetTable(part);
			int num = 0;
			foreach (GearFitEntry gearFitEntry in table.entries)
			{
				HashSet<string> ok = ((validForWornKey != null) ? validForWornKey(gearFitEntry.wornKey) : null);
				if (ok != null)
				{
					num += gearFitEntry.fits.RemoveAll((BodyFit bodyFit) => !ok.Contains(bodyFit.variant));
				}
			}
			table.entries.RemoveAll(new Predicate<GearFitEntry>(GearFitMatrixSO.IsEmpty));
			table.InvalidateIndex();
			return num;
		}

		// Token: 0x0600B977 RID: 47479 RVA: 0x004467D8 File Offset: 0x004449D8
		public int PruneOrphans(GearFitMatrixSO.Gender gender, GearFitMatrixSO.GearPart part, HashSet<string> validKeys)
		{
			PartFitTable table = this.GetGenderTables(gender).GetTable(part);
			int num = 0;
			Predicate<BodyFit> <>9__1;
			foreach (GearFitEntry gearFitEntry3 in table.entries)
			{
				int num2 = num;
				List<BodyFit> fits = gearFitEntry3.fits;
				Predicate<BodyFit> predicate;
				if ((predicate = <>9__1) == null)
				{
					predicate = (<>9__1 = (BodyFit f) => !validKeys.Contains(f.bodyKey));
				}
				num = num2 + fits.RemoveAll(predicate);
			}
			foreach (GearFitEntry gearFitEntry2 in table.entries)
			{
				if (!validKeys.Contains(gearFitEntry2.wornKey))
				{
					num += gearFitEntry2.fits.Count;
				}
			}
			table.entries.RemoveAll((GearFitEntry gearFitEntry) => !validKeys.Contains(gearFitEntry.wornKey) || GearFitMatrixSO.IsEmpty(gearFitEntry));
			table.InvalidateIndex();
			return num;
		}

		// Token: 0x0600B978 RID: 47480 RVA: 0x004468F0 File Offset: 0x00444AF0
		public int PruneKey(GearFitMatrixSO.Gender gender, GearFitMatrixSO.GearPart part, string key)
		{
			if (string.IsNullOrEmpty(key))
			{
				return 0;
			}
			PartFitTable table = this.GetGenderTables(gender).GetTable(part);
			int num = 0;
			Predicate<BodyFit> <>9__0;
			foreach (GearFitEntry gearFitEntry in table.entries)
			{
				if (string.Equals(gearFitEntry.wornKey, key, StringComparison.Ordinal))
				{
					num += gearFitEntry.fits.Count;
					gearFitEntry.fits.Clear();
				}
				else
				{
					int num2 = num;
					List<BodyFit> fits = gearFitEntry.fits;
					Predicate<BodyFit> predicate;
					if ((predicate = <>9__0) == null)
					{
						predicate = (<>9__0 = (BodyFit f) => string.Equals(f.bodyKey, key, StringComparison.Ordinal));
					}
					num = num2 + fits.RemoveAll(predicate);
				}
			}
			table.entries.RemoveAll(new Predicate<GearFitEntry>(GearFitMatrixSO.IsEmpty));
			table.InvalidateIndex();
			return num;
		}

		// Token: 0x0600B979 RID: 47481 RVA: 0x004469EC File Offset: 0x00444BEC
		public int ClearFits(GearFitMatrixSO.Gender gender, GearFitMatrixSO.GearPart part, string wornKey)
		{
			if (string.IsNullOrEmpty(wornKey))
			{
				return 0;
			}
			PartFitTable table = this.GetGenderTables(gender).GetTable(part);
			GearFitEntry gearFitEntry = table.Find(wornKey);
			if (gearFitEntry == null)
			{
				return 0;
			}
			int count = gearFitEntry.fits.Count;
			gearFitEntry.fits.Clear();
			table.entries.Remove(gearFitEntry);
			table.InvalidateIndex();
			return count;
		}

		// Token: 0x0600B97A RID: 47482 RVA: 0x00446A47 File Offset: 0x00444C47
		public void EnsureRegistry()
		{
			GenderFitTables genderFitTables = this.male;
			if (genderFitTables != null)
			{
				genderFitTables.EnsureRegistry();
			}
			GenderFitTables genderFitTables2 = this.female;
			if (genderFitTables2 == null)
			{
				return;
			}
			genderFitTables2.EnsureRegistry();
		}

		// Token: 0x0600B97B RID: 47483 RVA: 0x00446A6A File Offset: 0x00444C6A
		public void InvalidateIndexes()
		{
			GenderFitTables genderFitTables = this.male;
			if (genderFitTables != null)
			{
				genderFitTables.InvalidateIndexes();
			}
			GenderFitTables genderFitTables2 = this.female;
			if (genderFitTables2 == null)
			{
				return;
			}
			genderFitTables2.InvalidateIndexes();
		}

		// Token: 0x0600B97C RID: 47484 RVA: 0x00446A90 File Offset: 0x00444C90
		[PublicizedFrom(EAccessModifier.Private)]
		public static GearFitEntry AddEntry(PartFitTable partFitTable, string key, string guid)
		{
			GearFitEntry gearFitEntry = new GearFitEntry
			{
				wornKey = key,
				wornGuid = (guid ?? "")
			};
			partFitTable.entries.Add(gearFitEntry);
			partFitTable.InvalidateIndex();
			return gearFitEntry;
		}

		// Token: 0x0600B97D RID: 47485 RVA: 0x00446AD0 File Offset: 0x00444CD0
		[PublicizedFrom(EAccessModifier.Private)]
		public static void SetFitInline(GearFitEntry gearFitEntry, string bodyKey, string bodyGuid, string variant)
		{
			BodyFit bodyFit = gearFitEntry.fits.Find((BodyFit x) => string.Equals(x.bodyKey, bodyKey, StringComparison.Ordinal));
			if (string.IsNullOrEmpty(variant))
			{
				if (bodyFit != null)
				{
					gearFitEntry.fits.Remove(bodyFit);
					return;
				}
			}
			else
			{
				if (bodyFit == null)
				{
					gearFitEntry.fits.Add(new BodyFit
					{
						bodyKey = bodyKey,
						bodyGuid = (bodyGuid ?? ""),
						variant = variant
					});
					return;
				}
				bodyFit.variant = variant;
				if (string.IsNullOrEmpty(bodyFit.bodyGuid) && !string.IsNullOrEmpty(bodyGuid))
				{
					bodyFit.bodyGuid = bodyGuid;
				}
			}
		}

		// Token: 0x0600B97E RID: 47486 RVA: 0x00446B75 File Offset: 0x00444D75
		public string GetVariant(GearFitMatrixSO.Gender gender, GearFitMatrixSO.GearPart part, string wornKey, string bodyKey)
		{
			return this.GetGenderTables(gender).GetTable(part).GetVariantOrEmpty(wornKey, bodyKey);
		}

		// Token: 0x0600B97F RID: 47487 RVA: 0x00446B8C File Offset: 0x00444D8C
		[return: TupleElementNames(new string[] { "wornKey", "bodyKey", "variant" })]
		[return: Nullable(new byte[] { 1, 0, 1, 1, 1 })]
		public IEnumerable<ValueTuple<string, string, string>> EnumerateFits(GearFitMatrixSO.Gender gender, GearFitMatrixSO.GearPart part)
		{
			return from gearFitEntry in this.GetGenderTables(gender).GetTable(part).entries
				from f in gearFitEntry.fits
				select new ValueTuple<string, string, string>(gearFitEntry.wornKey, f.bodyKey, f.variant);
		}

		// Token: 0x0600B980 RID: 47488 RVA: 0x00446BEE File Offset: 0x00444DEE
		public int TotalFitCount(GearFitMatrixSO.Gender gender, GearFitMatrixSO.GearPart part)
		{
			return this.GetGenderTables(gender).GetTable(part).entries.Sum<GearFitEntry>((GearFitEntry e) => e.fits.Count);
		}

		// Token: 0x0600B981 RID: 47489 RVA: 0x00446C26 File Offset: 0x00444E26
		[PublicizedFrom(EAccessModifier.Private)]
		public static bool IsEmpty(GearFitEntry gearFitEntry)
		{
			return gearFitEntry.fits.Count == 0;
		}

		// Token: 0x0600B982 RID: 47490 RVA: 0x00446C36 File Offset: 0x00444E36
		[PublicizedFrom(EAccessModifier.Private)]
		public static void PruneIfEmpty(PartFitTable partFitTable, GearFitEntry gearFitEntry)
		{
			if (GearFitMatrixSO.IsEmpty(gearFitEntry))
			{
				partFitTable.entries.Remove(gearFitEntry);
			}
		}

		// Token: 0x0600B983 RID: 47491 RVA: 0x00446C50 File Offset: 0x00444E50
		[PublicizedFrom(EAccessModifier.Private)]
		public static string VariantIn(GearFitEntry gearFitEntry, string bodyKey)
		{
			BodyFit bodyFit = gearFitEntry.fits.Find((BodyFit x) => string.Equals(x.bodyKey, bodyKey, StringComparison.Ordinal));
			return ((bodyFit != null) ? bodyFit.variant : null) ?? "";
		}

		// Token: 0x04008A6A RID: 35434
		[Header("Gender: Male")]
		public GenderFitTables male = new GenderFitTables();

		// Token: 0x04008A6B RID: 35435
		[Header("Gender: Female")]
		public GenderFitTables female = new GenderFitTables();

		// Token: 0x04008A6C RID: 35436
		public const string AssetAddress = "@:Entities/Player/Common/GearFitMatrix.asset";

		// Token: 0x04008A6D RID: 35437
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public static GearFitMatrixSO _instance;

		// Token: 0x0200173A RID: 5946
		[NullableContext(0)]
		public enum GearPart
		{
			// Token: 0x04008A6F RID: 35439
			Head,
			// Token: 0x04008A70 RID: 35440
			Hands,
			// Token: 0x04008A71 RID: 35441
			Feet
		}

		// Token: 0x0200173B RID: 5947
		[NullableContext(0)]
		public enum Gender
		{
			// Token: 0x04008A73 RID: 35443
			Male,
			// Token: 0x04008A74 RID: 35444
			Female
		}

		// Token: 0x0200173C RID: 5948
		[NullableContext(0)]
		public enum CopyMode
		{
			// Token: 0x04008A76 RID: 35446
			Replace,
			// Token: 0x04008A77 RID: 35447
			Merge
		}

		// Token: 0x0200173D RID: 5949
		[NullableContext(0)]
		public readonly struct CopyResult
		{
			// Token: 0x0600B985 RID: 47493 RVA: 0x00446CB4 File Offset: 0x00444EB4
			public CopyResult(int copied, int skipped, bool baseMeshOnly)
			{
				this.Copied = copied;
				this.Skipped = skipped;
				this.BaseMeshOnly = baseMeshOnly;
			}

			// Token: 0x04008A78 RID: 35448
			public readonly int Copied;

			// Token: 0x04008A79 RID: 35449
			public readonly int Skipped;

			// Token: 0x04008A7A RID: 35450
			public readonly bool BaseMeshOnly;

			// Token: 0x04008A7B RID: 35451
			public static readonly GearFitMatrixSO.CopyResult Nothing = new GearFitMatrixSO.CopyResult(0, 0, false);
		}
	}
}

using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

// Token: 0x02000A7A RID: 2682
public static class PlayerSignStore
{
	// Token: 0x060050C5 RID: 20677 RVA: 0x001EFED6 File Offset: 0x001EE0D6
	[PublicizedFrom(EAccessModifier.Private)]
	public static string GetStoreDir()
	{
		return GameIO.GetPlayerDataDir();
	}

	// Token: 0x060050C6 RID: 20678 RVA: 0x001EFEDD File Offset: 0x001EE0DD
	[PublicizedFrom(EAccessModifier.Private)]
	public static string GetStorePath(string libraryId)
	{
		return PlayerSignStore.GetStoreDir() + "/" + libraryId + ".signs";
	}

	// Token: 0x060050C7 RID: 20679 RVA: 0x001EFEF4 File Offset: 0x001EE0F4
	public unsafe static void BuildImage(IList<byte[]> signBlobs, PooledExpandableMemoryStream image)
	{
		SignStoreIO.BeginImage(image);
		Span<byte> span = new Span<byte>(stackalloc byte[(UIntPtr)4], 4);
		for (int i = 0; i < signBlobs.Count; i++)
		{
			byte[] array = signBlobs[i];
			BinaryPrimitives.WriteInt32LittleEndian(span, array.Length);
			image.Write(span);
			image.Write(array, 0, array.Length);
		}
		SignStoreIO.FinalizeImage(image, 1313297232U, 3, signBlobs.Count, 0);
	}

	// Token: 0x060050C8 RID: 20680 RVA: 0x001EFF60 File Offset: 0x001EE160
	public static void Write(string libraryId, PooledExpandableMemoryStream image)
	{
		string storeDir = PlayerSignStore.GetStoreDir();
		if (!SdDirectory.Exists(storeDir))
		{
			SdDirectory.CreateDirectory(storeDir);
		}
		SignStoreIO.WriteWithBackup(PlayerSignStore.GetStorePath(libraryId), image);
	}

	// Token: 0x060050C9 RID: 20681 RVA: 0x001EFF90 File Offset: 0x001EE190
	public static bool TryLoad(string libraryId, List<byte[]> signBlobs, out int version)
	{
		string storePath = PlayerSignStore.GetStorePath(libraryId);
		List<byte[]> list;
		if (PlayerSignStore.TryReadFile(storePath, out list, out version))
		{
			signBlobs.AddRange(list);
			return true;
		}
		string text = storePath + ".bak";
		if (SdFile.Exists(text))
		{
			Log.Warning(string.Concat(new string[] { "[PlayerSignStore] Could not use '", storePath, "'; falling back to backup '", text, "'." }));
			if (PlayerSignStore.TryReadFile(text, out list, out version))
			{
				signBlobs.AddRange(list);
				return true;
			}
		}
		version = 0;
		return false;
	}

	// Token: 0x060050CA RID: 20682 RVA: 0x001F0014 File Offset: 0x001EE214
	public static void PreserveUnreadable(string libraryId)
	{
		string storePath = PlayerSignStore.GetStorePath(libraryId);
		PlayerSignStore.PreserveFile(storePath);
		PlayerSignStore.PreserveFile(storePath + ".bak");
	}

	// Token: 0x060050CB RID: 20683 RVA: 0x001F0034 File Offset: 0x001EE234
	[PublicizedFrom(EAccessModifier.Private)]
	public static void PreserveFile(string path)
	{
		string text = path + ".loadFailed";
		try
		{
			if (SdFile.Exists(path) && !SdFile.Exists(text))
			{
				SdFile.Copy(path, text, false);
				Log.Warning("[PlayerSignStore] Preserved unreadable store as '" + text + "'.");
			}
		}
		catch (Exception ex)
		{
			Log.Error(string.Format("[PlayerSignStore] Failed to preserve '{0}' as '{1}': {2}", path, text, ex));
		}
	}

	// Token: 0x060050CC RID: 20684 RVA: 0x001F00A4 File Offset: 0x001EE2A4
	public static void GetStoredLibraryIds(HashSet<string> results)
	{
		PlayerSignStore.<>c__DisplayClass14_0 CS$<>8__locals1;
		CS$<>8__locals1.results = results;
		CS$<>8__locals1.dir = PlayerSignStore.GetStoreDir();
		if (!SdDirectory.Exists(CS$<>8__locals1.dir))
		{
			return;
		}
		PlayerSignStore.<GetStoredLibraryIds>g__AddMatching|14_0(".signs", ref CS$<>8__locals1);
		PlayerSignStore.<GetStoredLibraryIds>g__AddMatching|14_0(".signs.bak", ref CS$<>8__locals1);
	}

	// Token: 0x060050CD RID: 20685 RVA: 0x001F00EB File Offset: 0x001EE2EB
	public static void DeleteAll(string libraryId)
	{
		string storePath = PlayerSignStore.GetStorePath(libraryId);
		PlayerSignStore.<DeleteAll>g__DeleteIfPresent|15_0(storePath);
		PlayerSignStore.<DeleteAll>g__DeleteIfPresent|15_0(storePath + ".bak");
	}

	// Token: 0x060050CE RID: 20686 RVA: 0x001F0108 File Offset: 0x001EE308
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool TryReadFile(string path, out List<byte[]> signBlobs, out int version)
	{
		signBlobs = null;
		version = 0;
		byte[] array;
		if (!SignStoreIO.TryReadAllBytes("[PlayerSignStore]", path, out array))
		{
			return false;
		}
		if (!PlayerSignStore.TryParseImage(path, array, out signBlobs, out version))
		{
			PlayerSignStore.PreserveFile(path);
			return false;
		}
		return true;
	}

	// Token: 0x060050CF RID: 20687 RVA: 0x001F0140 File Offset: 0x001EE340
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool TryParseImage(string path, byte[] image, out List<byte[]> signBlobs, out int version)
	{
		signBlobs = null;
		int num;
		byte b;
		if (!SignStoreIO.TryValidateHeader("[PlayerSignStore]", path, image, 1313297232U, 3, 3, out version, out num, out b))
		{
			return false;
		}
		List<byte[]> list = new List<byte[]>(num);
		int num2 = 32;
		for (int i = 0; i < num; i++)
		{
			if (num2 + 4 > image.Length)
			{
				Log.Warning(string.Format("{0} Ignoring '{1}': Ran out of payload reading length of sign {2} of {3}.", new object[] { "[PlayerSignStore]", path, i, num }));
				return false;
			}
			int num3 = BinaryPrimitives.ReadInt32LittleEndian(new ReadOnlySpan<byte>(image, num2, 4));
			num2 += 4;
			if (num3 < 0 || num2 + num3 > image.Length)
			{
				Log.Warning(string.Format("{0} Ignoring '{1}': sign {2} of {3} declares {4} bytes with {5} remaining.", new object[]
				{
					"[PlayerSignStore]",
					path,
					i,
					num,
					num3,
					image.Length - num2
				}));
				return false;
			}
			byte[] array = new byte[num3];
			Buffer.BlockCopy(image, num2, array, 0, num3);
			num2 += num3;
			list.Add(array);
		}
		if (num2 != image.Length)
		{
			Log.Warning(string.Format("{0} Ignoring '{1}': {2} trailing byte(s) after {3} sign(s).", new object[]
			{
				"[PlayerSignStore]",
				path,
				image.Length - num2,
				num
			}));
			return false;
		}
		signBlobs = list;
		return true;
	}

	// Token: 0x060050D0 RID: 20688 RVA: 0x001F02A0 File Offset: 0x001EE4A0
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Internal)]
	public static void <GetStoredLibraryIds>g__AddMatching|14_0(string suffix, ref PlayerSignStore.<>c__DisplayClass14_0 A_1)
	{
		string[] files = SdDirectory.GetFiles(A_1.dir, "*" + suffix);
		for (int i = 0; i < files.Length; i++)
		{
			string fileName = Path.GetFileName(files[i]);
			if (fileName.Length > suffix.Length && fileName.EndsWith(suffix, StringComparison.Ordinal))
			{
				A_1.results.Add(fileName.Substring(0, fileName.Length - suffix.Length));
			}
		}
	}

	// Token: 0x060050D1 RID: 20689 RVA: 0x001F0313 File Offset: 0x001EE513
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Internal)]
	public static void <DeleteAll>g__DeleteIfPresent|15_0(string filePath)
	{
		if (SdFile.Exists(filePath))
		{
			SdFile.Delete(filePath);
		}
	}

	// Token: 0x04003E4B RID: 15947
	[PublicizedFrom(EAccessModifier.Private)]
	public const uint kMagic = 1313297232U;

	// Token: 0x04003E4C RID: 15948
	[PublicizedFrom(EAccessModifier.Private)]
	public const string cLogTag = "[PlayerSignStore]";

	// Token: 0x04003E4D RID: 15949
	public const int CurrentVersion = 3;

	// Token: 0x04003E4E RID: 15950
	public const int MinReadableVersion = 3;

	// Token: 0x04003E4F RID: 15951
	[PublicizedFrom(EAccessModifier.Private)]
	public const string cStoreExt = ".signs";

	// Token: 0x04003E50 RID: 15952
	[PublicizedFrom(EAccessModifier.Private)]
	public const string cBackupExt = ".bak";

	// Token: 0x04003E51 RID: 15953
	[PublicizedFrom(EAccessModifier.Private)]
	public const string cLoadFailedExt = ".loadFailed";
}

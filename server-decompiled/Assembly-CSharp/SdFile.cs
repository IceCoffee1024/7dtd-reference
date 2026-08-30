using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

// Token: 0x02000A07 RID: 2567
public static class SdFile
{
	// Token: 0x06004CF0 RID: 19696 RVA: 0x001D9920 File Offset: 0x001D7B20
	public static StreamReader OpenText(string path)
	{
		SaveDataManagedPath saveDataManagedPath;
		if (SaveDataUtils.TryGetManagedPath(path, out saveDataManagedPath))
		{
			return SdFile.ManagedOpenText(saveDataManagedPath);
		}
		return File.OpenText(path);
	}

	// Token: 0x06004CF1 RID: 19697 RVA: 0x001D9944 File Offset: 0x001D7B44
	[PublicizedFrom(EAccessModifier.Internal)]
	public static StreamReader ManagedOpenText(SaveDataManagedPath path)
	{
		return new StreamReader(SdFile.ManagedOpen(path, FileMode.Open, FileAccess.Read, FileShare.Read));
	}

	// Token: 0x06004CF2 RID: 19698 RVA: 0x001D9954 File Offset: 0x001D7B54
	public static StreamWriter CreateText(string path)
	{
		SaveDataManagedPath saveDataManagedPath;
		if (SaveDataUtils.TryGetManagedPath(path, out saveDataManagedPath))
		{
			return SdFile.ManagedCreateText(saveDataManagedPath);
		}
		return File.CreateText(path);
	}

	// Token: 0x06004CF3 RID: 19699 RVA: 0x001D9978 File Offset: 0x001D7B78
	[PublicizedFrom(EAccessModifier.Internal)]
	public static StreamWriter ManagedCreateText(SaveDataManagedPath path)
	{
		return new StreamWriter(SdFile.ManagedOpen(path, FileMode.Create, FileAccess.Write, FileShare.Read));
	}

	// Token: 0x06004CF4 RID: 19700 RVA: 0x001D9988 File Offset: 0x001D7B88
	public static StreamWriter AppendText(string path)
	{
		SaveDataManagedPath saveDataManagedPath;
		if (SaveDataUtils.TryGetManagedPath(path, out saveDataManagedPath))
		{
			return SdFile.ManagedAppendText(saveDataManagedPath);
		}
		return File.AppendText(path);
	}

	// Token: 0x06004CF5 RID: 19701 RVA: 0x001D99AC File Offset: 0x001D7BAC
	[PublicizedFrom(EAccessModifier.Internal)]
	public static StreamWriter ManagedAppendText(SaveDataManagedPath path)
	{
		return new StreamWriter(SdFile.ManagedOpen(path, FileMode.Append, FileAccess.Write, FileShare.Read));
	}

	// Token: 0x06004CF6 RID: 19702 RVA: 0x001D99BC File Offset: 0x001D7BBC
	public static void Copy(string sourceFileName, string destFileName)
	{
		SaveDataManagedPath saveDataManagedPath;
		bool flag = SaveDataUtils.TryGetManagedPath(sourceFileName, out saveDataManagedPath);
		SaveDataManagedPath saveDataManagedPath2;
		bool flag2 = SaveDataUtils.TryGetManagedPath(destFileName, out saveDataManagedPath2);
		if (flag && flag2)
		{
			SdFile.ManagedToManagedCopy(saveDataManagedPath, saveDataManagedPath2, false);
			return;
		}
		if (flag)
		{
			SdFile.ManagedToUnmanagedCopy(saveDataManagedPath, destFileName, false);
			return;
		}
		if (flag2)
		{
			SdFile.UnmanagedToManagedCopy(sourceFileName, saveDataManagedPath2, false);
			return;
		}
		File.Copy(sourceFileName, destFileName);
	}

	// Token: 0x06004CF7 RID: 19703 RVA: 0x001D9A08 File Offset: 0x001D7C08
	public static void Copy(string sourceFileName, string destFileName, bool overwrite)
	{
		SaveDataManagedPath saveDataManagedPath;
		bool flag = SaveDataUtils.TryGetManagedPath(sourceFileName, out saveDataManagedPath);
		SaveDataManagedPath saveDataManagedPath2;
		bool flag2 = SaveDataUtils.TryGetManagedPath(destFileName, out saveDataManagedPath2);
		if (flag && flag2)
		{
			SdFile.ManagedToManagedCopy(saveDataManagedPath, saveDataManagedPath2, overwrite);
			return;
		}
		if (flag)
		{
			SdFile.ManagedToUnmanagedCopy(saveDataManagedPath, destFileName, overwrite);
			return;
		}
		if (flag2)
		{
			SdFile.UnmanagedToManagedCopy(sourceFileName, saveDataManagedPath2, overwrite);
			return;
		}
		File.Copy(sourceFileName, destFileName, overwrite);
	}

	// Token: 0x06004CF8 RID: 19704 RVA: 0x001D9A58 File Offset: 0x001D7C58
	[PublicizedFrom(EAccessModifier.Internal)]
	public static void ManagedToManagedCopy(SaveDataManagedPath sourceFileName, SaveDataManagedPath destFileName, bool overwrite)
	{
		using (Stream stream = SdFile.ManagedOpen(sourceFileName, FileMode.Open, FileAccess.Read, FileShare.Read))
		{
			using (Stream stream2 = SdFile.ManagedOpen(destFileName, overwrite ? FileMode.Create : FileMode.CreateNew, FileAccess.Write, FileShare.Read))
			{
				StreamUtils.StreamCopy(stream, stream2, null, true);
			}
		}
	}

	// Token: 0x06004CF9 RID: 19705 RVA: 0x001D9ABC File Offset: 0x001D7CBC
	[PublicizedFrom(EAccessModifier.Internal)]
	public static void ManagedToUnmanagedCopy(SaveDataManagedPath sourceFileName, string destFileName, bool overwrite)
	{
		using (Stream stream = SdFile.ManagedOpen(sourceFileName, FileMode.Open, FileAccess.Read, FileShare.Read))
		{
			using (FileStream fileStream = File.Open(destFileName, overwrite ? FileMode.Create : FileMode.CreateNew, FileAccess.Write, FileShare.Read))
			{
				StreamUtils.StreamCopy(stream, fileStream, null, true);
			}
		}
	}

	// Token: 0x06004CFA RID: 19706 RVA: 0x001D9B20 File Offset: 0x001D7D20
	[PublicizedFrom(EAccessModifier.Internal)]
	public static void UnmanagedToManagedCopy(string sourceFileName, SaveDataManagedPath destFileName, bool overwrite)
	{
		using (FileStream fileStream = File.Open(sourceFileName, FileMode.Open, FileAccess.Read, FileShare.Read))
		{
			using (Stream stream = SdFile.ManagedOpen(destFileName, overwrite ? FileMode.Create : FileMode.CreateNew, FileAccess.Write, FileShare.Read))
			{
				StreamUtils.StreamCopy(fileStream, stream, null, true);
			}
		}
	}

	// Token: 0x06004CFB RID: 19707 RVA: 0x001D9B84 File Offset: 0x001D7D84
	public static Stream Create(string path)
	{
		SaveDataManagedPath saveDataManagedPath;
		if (SaveDataUtils.TryGetManagedPath(path, out saveDataManagedPath))
		{
			return SdFile.ManagedCreate(saveDataManagedPath);
		}
		return File.Create(path);
	}

	// Token: 0x06004CFC RID: 19708 RVA: 0x001D9BA8 File Offset: 0x001D7DA8
	[PublicizedFrom(EAccessModifier.Internal)]
	public static Stream ManagedCreate(SaveDataManagedPath path)
	{
		return SdFile.ManagedOpen(path, FileMode.Create, FileAccess.ReadWrite, FileShare.None);
	}

	// Token: 0x06004CFD RID: 19709 RVA: 0x001D9BB4 File Offset: 0x001D7DB4
	public static void Delete(string path)
	{
		SaveDataManagedPath saveDataManagedPath;
		if (SaveDataUtils.TryGetManagedPath(path, out saveDataManagedPath))
		{
			SdFile.ManagedDelete(saveDataManagedPath);
			return;
		}
		File.Delete(path);
	}

	// Token: 0x06004CFE RID: 19710 RVA: 0x001D9BD8 File Offset: 0x001D7DD8
	[PublicizedFrom(EAccessModifier.Internal)]
	public static void ManagedDelete(SaveDataManagedPath path)
	{
		SaveDataUtils.SaveDataManager.ManagedFileDelete(path);
	}

	// Token: 0x06004CFF RID: 19711 RVA: 0x001D9BE8 File Offset: 0x001D7DE8
	public static bool Exists(string path)
	{
		SaveDataManagedPath saveDataManagedPath;
		if (SaveDataUtils.TryGetManagedPath(path, out saveDataManagedPath))
		{
			return SdFile.ManagedExists(saveDataManagedPath);
		}
		return File.Exists(path);
	}

	// Token: 0x06004D00 RID: 19712 RVA: 0x001D9C0C File Offset: 0x001D7E0C
	[PublicizedFrom(EAccessModifier.Internal)]
	public static bool ManagedExists(SaveDataManagedPath path)
	{
		return SaveDataUtils.SaveDataManager.ManagedFileExists(path);
	}

	// Token: 0x06004D01 RID: 19713 RVA: 0x001D9C1C File Offset: 0x001D7E1C
	public static Stream Open(string path, FileMode mode)
	{
		SaveDataManagedPath saveDataManagedPath;
		if (SaveDataUtils.TryGetManagedPath(path, out saveDataManagedPath))
		{
			return SdFile.ManagedOpen(saveDataManagedPath, mode, (mode == FileMode.Append) ? FileAccess.Write : FileAccess.ReadWrite, FileShare.None);
		}
		return File.Open(path, mode);
	}

	// Token: 0x06004D02 RID: 19714 RVA: 0x001D9C4C File Offset: 0x001D7E4C
	public static Stream Open(string path, FileMode mode, FileAccess access)
	{
		SaveDataManagedPath saveDataManagedPath;
		if (SaveDataUtils.TryGetManagedPath(path, out saveDataManagedPath))
		{
			return SdFile.ManagedOpen(saveDataManagedPath, mode, access, FileShare.None);
		}
		return File.Open(path, mode, access);
	}

	// Token: 0x06004D03 RID: 19715 RVA: 0x001D9C78 File Offset: 0x001D7E78
	public static Stream Open(string path, FileMode mode, FileAccess access, FileShare share)
	{
		SaveDataManagedPath saveDataManagedPath;
		if (SaveDataUtils.TryGetManagedPath(path, out saveDataManagedPath))
		{
			return SdFile.ManagedOpen(saveDataManagedPath, mode, access, share);
		}
		return File.Open(path, mode, access, share);
	}

	// Token: 0x06004D04 RID: 19716 RVA: 0x001D9CA2 File Offset: 0x001D7EA2
	[PublicizedFrom(EAccessModifier.Internal)]
	public static Stream ManagedOpen(SaveDataManagedPath path, FileMode mode, FileAccess access, FileShare share)
	{
		return SaveDataUtils.SaveDataManager.ManagedFileOpen(path, mode, access, share);
	}

	// Token: 0x06004D05 RID: 19717 RVA: 0x001D9CB4 File Offset: 0x001D7EB4
	public static DateTime GetLastWriteTime(string path)
	{
		SaveDataManagedPath saveDataManagedPath;
		if (SaveDataUtils.TryGetManagedPath(path, out saveDataManagedPath))
		{
			return SdFile.ManagedGetLastWriteTime(saveDataManagedPath);
		}
		return File.GetLastWriteTime(path);
	}

	// Token: 0x06004D06 RID: 19718 RVA: 0x001D9CD8 File Offset: 0x001D7ED8
	[PublicizedFrom(EAccessModifier.Private)]
	public static DateTime ManagedGetLastWriteTime(SaveDataManagedPath path)
	{
		return SdFile.ManagedGetLastWriteTimeUtc(path).ToLocalTime();
	}

	// Token: 0x06004D07 RID: 19719 RVA: 0x001D9CF4 File Offset: 0x001D7EF4
	public static DateTime GetLastWriteTimeUtc(string path)
	{
		SaveDataManagedPath saveDataManagedPath;
		if (SaveDataUtils.TryGetManagedPath(path, out saveDataManagedPath))
		{
			return SdFile.ManagedGetLastWriteTimeUtc(saveDataManagedPath);
		}
		return File.GetLastWriteTimeUtc(path);
	}

	// Token: 0x06004D08 RID: 19720 RVA: 0x001D9D18 File Offset: 0x001D7F18
	[PublicizedFrom(EAccessModifier.Internal)]
	public static DateTime ManagedGetLastWriteTimeUtc(SaveDataManagedPath path)
	{
		return SaveDataUtils.SaveDataManager.ManagedFileGetLastWriteTimeUtc(path);
	}

	// Token: 0x06004D09 RID: 19721 RVA: 0x001D9D28 File Offset: 0x001D7F28
	public static Stream OpenRead(string path)
	{
		SaveDataManagedPath saveDataManagedPath;
		if (SaveDataUtils.TryGetManagedPath(path, out saveDataManagedPath))
		{
			return SdFile.ManagedOpen(saveDataManagedPath, FileMode.Open, FileAccess.Read, FileShare.Read);
		}
		return File.OpenRead(path);
	}

	// Token: 0x06004D0A RID: 19722 RVA: 0x001D9D50 File Offset: 0x001D7F50
	public static Stream OpenWrite(string path)
	{
		SaveDataManagedPath saveDataManagedPath;
		if (SaveDataUtils.TryGetManagedPath(path, out saveDataManagedPath))
		{
			return SdFile.ManagedOpen(saveDataManagedPath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
		}
		return File.OpenWrite(path);
	}

	// Token: 0x06004D0B RID: 19723 RVA: 0x001D9D78 File Offset: 0x001D7F78
	public static string ReadAllText(string path)
	{
		SaveDataManagedPath saveDataManagedPath;
		if (SaveDataUtils.TryGetManagedPath(path, out saveDataManagedPath))
		{
			return SdFile.ManagedReadAllText(saveDataManagedPath, Encoding.UTF8);
		}
		return File.ReadAllText(path);
	}

	// Token: 0x06004D0C RID: 19724 RVA: 0x001D9DA4 File Offset: 0x001D7FA4
	public static string ReadAllText(string path, Encoding encoding)
	{
		SaveDataManagedPath saveDataManagedPath;
		if (SaveDataUtils.TryGetManagedPath(path, out saveDataManagedPath))
		{
			return SdFile.ManagedReadAllText(saveDataManagedPath, encoding);
		}
		return File.ReadAllText(path, encoding);
	}

	// Token: 0x06004D0D RID: 19725 RVA: 0x001D9DCC File Offset: 0x001D7FCC
	[PublicizedFrom(EAccessModifier.Internal)]
	public static string ManagedReadAllText(SaveDataManagedPath path, Encoding encoding)
	{
		string text;
		using (Stream stream = SdFile.ManagedOpen(path, FileMode.Open, FileAccess.Read, FileShare.Read))
		{
			using (StreamReader streamReader = new StreamReader(stream, encoding))
			{
				text = streamReader.ReadToEnd();
			}
		}
		return text;
	}

	// Token: 0x06004D0E RID: 19726 RVA: 0x001D9E28 File Offset: 0x001D8028
	public static void WriteAllText(string path, string contents)
	{
		SaveDataManagedPath saveDataManagedPath;
		if (SaveDataUtils.TryGetManagedPath(path, out saveDataManagedPath))
		{
			SdFile.ManagedWriteAllText(saveDataManagedPath, contents, SdFile.UTF8NoBom);
			return;
		}
		File.WriteAllText(path, contents);
	}

	// Token: 0x06004D0F RID: 19727 RVA: 0x001D9E54 File Offset: 0x001D8054
	public static void WriteAllText(string path, string contents, Encoding encoding)
	{
		SaveDataManagedPath saveDataManagedPath;
		if (SaveDataUtils.TryGetManagedPath(path, out saveDataManagedPath))
		{
			SdFile.ManagedWriteAllText(saveDataManagedPath, contents, encoding);
			return;
		}
		File.WriteAllText(path, contents, encoding);
	}

	// Token: 0x06004D10 RID: 19728 RVA: 0x001D9E7C File Offset: 0x001D807C
	[PublicizedFrom(EAccessModifier.Private)]
	public static void ManagedWriteAllText(SaveDataManagedPath path, string contents, Encoding encoding)
	{
		using (Stream stream = SdFile.ManagedOpen(path, FileMode.Create, FileAccess.Write, FileShare.Read))
		{
			using (StreamWriter streamWriter = new StreamWriter(stream, encoding))
			{
				streamWriter.Write(contents);
			}
		}
	}

	// Token: 0x06004D11 RID: 19729 RVA: 0x001D9ED4 File Offset: 0x001D80D4
	public static byte[] ReadAllBytes(string path)
	{
		SaveDataManagedPath saveDataManagedPath;
		if (SaveDataUtils.TryGetManagedPath(path, out saveDataManagedPath))
		{
			return SdFile.ManagedReadAllBytes(saveDataManagedPath);
		}
		return File.ReadAllBytes(path);
	}

	// Token: 0x06004D12 RID: 19730 RVA: 0x001D9EF8 File Offset: 0x001D80F8
	[PublicizedFrom(EAccessModifier.Private)]
	public static byte[] ManagedReadAllBytes(SaveDataManagedPath path)
	{
		byte[] array;
		using (PooledExpandableMemoryStream pooledExpandableMemoryStream = MemoryPools.poolMemoryStream.AllocSync(true))
		{
			using (Stream stream = SdFile.ManagedOpen(path, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				stream.CopyTo(pooledExpandableMemoryStream);
				array = pooledExpandableMemoryStream.ToArray();
			}
		}
		return array;
	}

	// Token: 0x06004D13 RID: 19731 RVA: 0x001D9F5C File Offset: 0x001D815C
	public static void WriteAllBytes(string path, byte[] bytes)
	{
		SaveDataManagedPath saveDataManagedPath;
		if (SaveDataUtils.TryGetManagedPath(path, out saveDataManagedPath))
		{
			SdFile.ManagedWriteAllBytes(saveDataManagedPath, bytes);
			return;
		}
		File.WriteAllBytes(path, bytes);
	}

	// Token: 0x06004D14 RID: 19732 RVA: 0x001D9F84 File Offset: 0x001D8184
	[PublicizedFrom(EAccessModifier.Private)]
	public static void ManagedWriteAllBytes(SaveDataManagedPath path, byte[] bytes)
	{
		using (Stream stream = SdFile.ManagedOpen(path, FileMode.Create, FileAccess.Write, FileShare.Read))
		{
			stream.Write(bytes, 0, bytes.Length);
		}
	}

	// Token: 0x06004D15 RID: 19733 RVA: 0x001D9FC4 File Offset: 0x001D81C4
	public static string[] ReadAllLines(string path)
	{
		SaveDataManagedPath saveDataManagedPath;
		if (SaveDataUtils.TryGetManagedPath(path, out saveDataManagedPath))
		{
			return SdFile.ManagedReadAllLines(saveDataManagedPath, Encoding.UTF8);
		}
		return File.ReadAllLines(path);
	}

	// Token: 0x06004D16 RID: 19734 RVA: 0x001D9FF0 File Offset: 0x001D81F0
	public static string[] ReadAllLines(string path, Encoding encoding)
	{
		SaveDataManagedPath saveDataManagedPath;
		if (SaveDataUtils.TryGetManagedPath(path, out saveDataManagedPath))
		{
			return SdFile.ManagedReadAllLines(saveDataManagedPath, encoding);
		}
		return File.ReadAllLines(path, encoding);
	}

	// Token: 0x06004D17 RID: 19735 RVA: 0x001DA018 File Offset: 0x001D8218
	[PublicizedFrom(EAccessModifier.Private)]
	public static string[] ManagedReadAllLines(SaveDataManagedPath path, Encoding encoding)
	{
		List<string> list = new List<string>();
		string[] array;
		using (Stream stream = SdFile.ManagedOpen(path, FileMode.Open, FileAccess.Read, FileShare.Read))
		{
			using (StreamReader streamReader = new StreamReader(stream, encoding))
			{
				for (;;)
				{
					string text = streamReader.ReadLine();
					if (text == null)
					{
						break;
					}
					list.Add(text);
				}
				array = list.ToArray();
			}
		}
		return array;
	}

	// Token: 0x06004D18 RID: 19736 RVA: 0x001DA08C File Offset: 0x001D828C
	public static IEnumerable<string> ReadLines(string path)
	{
		SaveDataManagedPath saveDataManagedPath;
		if (SaveDataUtils.TryGetManagedPath(path, out saveDataManagedPath))
		{
			return SdFile.ManagedReadLines(saveDataManagedPath, Encoding.UTF8);
		}
		return File.ReadLines(path);
	}

	// Token: 0x06004D19 RID: 19737 RVA: 0x001DA0B8 File Offset: 0x001D82B8
	public static IEnumerable<string> ReadLines(string path, Encoding encoding)
	{
		SaveDataManagedPath saveDataManagedPath;
		if (SaveDataUtils.TryGetManagedPath(path, out saveDataManagedPath))
		{
			return SdFile.ManagedReadLines(saveDataManagedPath, encoding);
		}
		return File.ReadLines(path, encoding);
	}

	// Token: 0x06004D1A RID: 19738 RVA: 0x001DA0DE File Offset: 0x001D82DE
	[PublicizedFrom(EAccessModifier.Private)]
	public static IEnumerable<string> ManagedReadLines(SaveDataManagedPath path, Encoding encoding)
	{
		SdFile.<ManagedReadLines>d__43 <ManagedReadLines>d__ = new SdFile.<ManagedReadLines>d__43(-2);
		<ManagedReadLines>d__.<>3__path = path;
		<ManagedReadLines>d__.<>3__encoding = encoding;
		return <ManagedReadLines>d__;
	}

	// Token: 0x06004D1B RID: 19739 RVA: 0x001DA0F8 File Offset: 0x001D82F8
	public static void WriteAllLines(string path, string[] contents)
	{
		SaveDataManagedPath saveDataManagedPath;
		if (SaveDataUtils.TryGetManagedPath(path, out saveDataManagedPath))
		{
			SdFile.ManagedWriteAllLines(saveDataManagedPath, contents, SdFile.UTF8NoBom);
			return;
		}
		File.WriteAllLines(path, contents);
	}

	// Token: 0x06004D1C RID: 19740 RVA: 0x001DA124 File Offset: 0x001D8324
	public static void WriteAllLines(string path, string[] contents, Encoding encoding)
	{
		SaveDataManagedPath saveDataManagedPath;
		if (SaveDataUtils.TryGetManagedPath(path, out saveDataManagedPath))
		{
			SdFile.ManagedWriteAllLines(saveDataManagedPath, contents, encoding);
			return;
		}
		File.WriteAllLines(path, contents, encoding);
	}

	// Token: 0x06004D1D RID: 19741 RVA: 0x001DA14C File Offset: 0x001D834C
	public static void WriteAllLines(string path, IEnumerable<string> contents)
	{
		SaveDataManagedPath saveDataManagedPath;
		if (SaveDataUtils.TryGetManagedPath(path, out saveDataManagedPath))
		{
			SdFile.ManagedWriteAllLines(saveDataManagedPath, contents, SdFile.UTF8NoBom);
			return;
		}
		File.WriteAllLines(path, contents);
	}

	// Token: 0x06004D1E RID: 19742 RVA: 0x001DA178 File Offset: 0x001D8378
	public static void WriteAllLines(string path, IEnumerable<string> contents, Encoding encoding)
	{
		SaveDataManagedPath saveDataManagedPath;
		if (SaveDataUtils.TryGetManagedPath(path, out saveDataManagedPath))
		{
			SdFile.ManagedWriteAllLines(saveDataManagedPath, contents, encoding);
			return;
		}
		File.WriteAllLines(path, contents, encoding);
	}

	// Token: 0x06004D1F RID: 19743 RVA: 0x001DA1A0 File Offset: 0x001D83A0
	[PublicizedFrom(EAccessModifier.Private)]
	public static void ManagedWriteAllLines(SaveDataManagedPath path, IEnumerable<string> contents, Encoding encoding)
	{
		using (Stream stream = SdFile.ManagedOpen(path, FileMode.Create, FileAccess.Write, FileShare.Read))
		{
			using (StreamWriter streamWriter = new StreamWriter(stream, encoding))
			{
				SdFile.ManagedWriteAllLines(streamWriter, contents);
			}
		}
	}

	// Token: 0x06004D20 RID: 19744 RVA: 0x001DA1F8 File Offset: 0x001D83F8
	[PublicizedFrom(EAccessModifier.Private)]
	public static void ManagedWriteAllLines(TextWriter writer, IEnumerable<string> contents)
	{
		try
		{
			foreach (string text in contents)
			{
				writer.WriteLine(text);
			}
		}
		finally
		{
			if (writer != null)
			{
				((IDisposable)writer).Dispose();
			}
		}
	}

	// Token: 0x06004D21 RID: 19745 RVA: 0x001DA258 File Offset: 0x001D8458
	public static void AppendAllText(string path, string contents)
	{
		SaveDataManagedPath saveDataManagedPath;
		if (SaveDataUtils.TryGetManagedPath(path, out saveDataManagedPath))
		{
			SdFile.ManagedAppendAllText(saveDataManagedPath, contents, SdFile.UTF8NoBom);
			return;
		}
		File.AppendAllText(path, contents);
	}

	// Token: 0x06004D22 RID: 19746 RVA: 0x001DA284 File Offset: 0x001D8484
	public static void AppendAllText(string path, string contents, Encoding encoding)
	{
		SaveDataManagedPath saveDataManagedPath;
		if (SaveDataUtils.TryGetManagedPath(path, out saveDataManagedPath))
		{
			SdFile.ManagedAppendAllText(saveDataManagedPath, contents, encoding);
			return;
		}
		File.AppendAllText(path, contents, encoding);
	}

	// Token: 0x06004D23 RID: 19747 RVA: 0x001DA2AC File Offset: 0x001D84AC
	[PublicizedFrom(EAccessModifier.Private)]
	public static void ManagedAppendAllText(SaveDataManagedPath path, string contents, Encoding encoding)
	{
		using (Stream stream = SdFile.ManagedOpen(path, FileMode.Append, FileAccess.Write, FileShare.Read))
		{
			using (StreamWriter streamWriter = new StreamWriter(stream, encoding))
			{
				streamWriter.Write(contents);
			}
		}
	}

	// Token: 0x06004D24 RID: 19748 RVA: 0x001DA304 File Offset: 0x001D8504
	public static void AppendAllLines(string path, IEnumerable<string> contents)
	{
		SaveDataManagedPath saveDataManagedPath;
		if (SaveDataUtils.TryGetManagedPath(path, out saveDataManagedPath))
		{
			SdFile.ManagedAppendAllLines(saveDataManagedPath, contents, SdFile.UTF8NoBom);
			return;
		}
		File.AppendAllLines(path, contents);
	}

	// Token: 0x06004D25 RID: 19749 RVA: 0x001DA330 File Offset: 0x001D8530
	public static void AppendAllLines(string path, IEnumerable<string> contents, Encoding encoding)
	{
		SaveDataManagedPath saveDataManagedPath;
		if (SaveDataUtils.TryGetManagedPath(path, out saveDataManagedPath))
		{
			SdFile.ManagedAppendAllLines(saveDataManagedPath, contents, encoding);
			return;
		}
		File.AppendAllLines(path, contents, encoding);
	}

	// Token: 0x06004D26 RID: 19750 RVA: 0x001DA358 File Offset: 0x001D8558
	[PublicizedFrom(EAccessModifier.Private)]
	public static void ManagedAppendAllLines(SaveDataManagedPath path, IEnumerable<string> contents, Encoding encoding)
	{
		using (Stream stream = SdFile.ManagedOpen(path, FileMode.Append, FileAccess.Write, FileShare.Read))
		{
			using (StreamWriter streamWriter = new StreamWriter(stream, encoding))
			{
				SdFile.ManagedWriteAllLines(streamWriter, contents);
			}
		}
	}

	// Token: 0x04003C00 RID: 15360
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Encoding UTF8NoBom = new UTF8Encoding(false);
}

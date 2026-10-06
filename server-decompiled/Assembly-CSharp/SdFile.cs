using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

// Token: 0x02000A11 RID: 2577
public static class SdFile
{
	// Token: 0x06004CBF RID: 19647 RVA: 0x001DABD0 File Offset: 0x001D8DD0
	public static StreamReader OpenText(string path)
	{
		SaveDataManagedPath saveDataManagedPath;
		if (SaveDataUtils.TryGetManagedPath(path, out saveDataManagedPath))
		{
			return SdFile.ManagedOpenText(saveDataManagedPath);
		}
		return File.OpenText(path);
	}

	// Token: 0x06004CC0 RID: 19648 RVA: 0x001DABF4 File Offset: 0x001D8DF4
	[PublicizedFrom(EAccessModifier.Internal)]
	public static StreamReader ManagedOpenText(SaveDataManagedPath path)
	{
		return new StreamReader(SdFile.ManagedOpen(path, FileMode.Open, FileAccess.Read, FileShare.Read));
	}

	// Token: 0x06004CC1 RID: 19649 RVA: 0x001DAC04 File Offset: 0x001D8E04
	public static StreamWriter CreateText(string path)
	{
		SaveDataManagedPath saveDataManagedPath;
		if (SaveDataUtils.TryGetManagedPath(path, out saveDataManagedPath))
		{
			return SdFile.ManagedCreateText(saveDataManagedPath);
		}
		return File.CreateText(path);
	}

	// Token: 0x06004CC2 RID: 19650 RVA: 0x001DAC28 File Offset: 0x001D8E28
	[PublicizedFrom(EAccessModifier.Internal)]
	public static StreamWriter ManagedCreateText(SaveDataManagedPath path)
	{
		return new StreamWriter(SdFile.ManagedOpen(path, FileMode.Create, FileAccess.Write, FileShare.Read));
	}

	// Token: 0x06004CC3 RID: 19651 RVA: 0x001DAC38 File Offset: 0x001D8E38
	public static StreamWriter AppendText(string path)
	{
		SaveDataManagedPath saveDataManagedPath;
		if (SaveDataUtils.TryGetManagedPath(path, out saveDataManagedPath))
		{
			return SdFile.ManagedAppendText(saveDataManagedPath);
		}
		return File.AppendText(path);
	}

	// Token: 0x06004CC4 RID: 19652 RVA: 0x001DAC5C File Offset: 0x001D8E5C
	[PublicizedFrom(EAccessModifier.Internal)]
	public static StreamWriter ManagedAppendText(SaveDataManagedPath path)
	{
		return new StreamWriter(SdFile.ManagedOpen(path, FileMode.Append, FileAccess.Write, FileShare.Read));
	}

	// Token: 0x06004CC5 RID: 19653 RVA: 0x001DAC6C File Offset: 0x001D8E6C
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

	// Token: 0x06004CC6 RID: 19654 RVA: 0x001DACB8 File Offset: 0x001D8EB8
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

	// Token: 0x06004CC7 RID: 19655 RVA: 0x001DAD08 File Offset: 0x001D8F08
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

	// Token: 0x06004CC8 RID: 19656 RVA: 0x001DAD6C File Offset: 0x001D8F6C
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

	// Token: 0x06004CC9 RID: 19657 RVA: 0x001DADD0 File Offset: 0x001D8FD0
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

	// Token: 0x06004CCA RID: 19658 RVA: 0x001DAE34 File Offset: 0x001D9034
	public static Stream Create(string path)
	{
		SaveDataManagedPath saveDataManagedPath;
		if (SaveDataUtils.TryGetManagedPath(path, out saveDataManagedPath))
		{
			return SdFile.ManagedCreate(saveDataManagedPath);
		}
		return File.Create(path);
	}

	// Token: 0x06004CCB RID: 19659 RVA: 0x001DAE58 File Offset: 0x001D9058
	[PublicizedFrom(EAccessModifier.Internal)]
	public static Stream ManagedCreate(SaveDataManagedPath path)
	{
		return SdFile.ManagedOpen(path, FileMode.Create, FileAccess.ReadWrite, FileShare.None);
	}

	// Token: 0x06004CCC RID: 19660 RVA: 0x001DAE64 File Offset: 0x001D9064
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

	// Token: 0x06004CCD RID: 19661 RVA: 0x001DAE88 File Offset: 0x001D9088
	[PublicizedFrom(EAccessModifier.Internal)]
	public static void ManagedDelete(SaveDataManagedPath path)
	{
		SaveDataUtils.SaveDataManager.ManagedFileDelete(path);
	}

	// Token: 0x06004CCE RID: 19662 RVA: 0x001DAE98 File Offset: 0x001D9098
	public static bool Exists(string path)
	{
		SaveDataManagedPath saveDataManagedPath;
		if (SaveDataUtils.TryGetManagedPath(path, out saveDataManagedPath))
		{
			return SdFile.ManagedExists(saveDataManagedPath);
		}
		return File.Exists(path);
	}

	// Token: 0x06004CCF RID: 19663 RVA: 0x001DAEBC File Offset: 0x001D90BC
	[PublicizedFrom(EAccessModifier.Internal)]
	public static bool ManagedExists(SaveDataManagedPath path)
	{
		return SaveDataUtils.SaveDataManager.ManagedFileExists(path);
	}

	// Token: 0x06004CD0 RID: 19664 RVA: 0x001DAECC File Offset: 0x001D90CC
	public static Stream Open(string path, FileMode mode)
	{
		SaveDataManagedPath saveDataManagedPath;
		if (SaveDataUtils.TryGetManagedPath(path, out saveDataManagedPath))
		{
			return SdFile.ManagedOpen(saveDataManagedPath, mode, (mode == FileMode.Append) ? FileAccess.Write : FileAccess.ReadWrite, FileShare.None);
		}
		return File.Open(path, mode);
	}

	// Token: 0x06004CD1 RID: 19665 RVA: 0x001DAEFC File Offset: 0x001D90FC
	public static Stream Open(string path, FileMode mode, FileAccess access)
	{
		SaveDataManagedPath saveDataManagedPath;
		if (SaveDataUtils.TryGetManagedPath(path, out saveDataManagedPath))
		{
			return SdFile.ManagedOpen(saveDataManagedPath, mode, access, FileShare.None);
		}
		return File.Open(path, mode, access);
	}

	// Token: 0x06004CD2 RID: 19666 RVA: 0x001DAF28 File Offset: 0x001D9128
	public static Stream Open(string path, FileMode mode, FileAccess access, FileShare share)
	{
		SaveDataManagedPath saveDataManagedPath;
		if (SaveDataUtils.TryGetManagedPath(path, out saveDataManagedPath))
		{
			return SdFile.ManagedOpen(saveDataManagedPath, mode, access, share);
		}
		return File.Open(path, mode, access, share);
	}

	// Token: 0x06004CD3 RID: 19667 RVA: 0x001DAF52 File Offset: 0x001D9152
	[PublicizedFrom(EAccessModifier.Internal)]
	public static Stream ManagedOpen(SaveDataManagedPath path, FileMode mode, FileAccess access, FileShare share)
	{
		return SaveDataUtils.SaveDataManager.ManagedFileOpen(path, mode, access, share);
	}

	// Token: 0x06004CD4 RID: 19668 RVA: 0x001DAF64 File Offset: 0x001D9164
	public static DateTime GetLastWriteTime(string path)
	{
		SaveDataManagedPath saveDataManagedPath;
		if (SaveDataUtils.TryGetManagedPath(path, out saveDataManagedPath))
		{
			return SdFile.ManagedGetLastWriteTime(saveDataManagedPath);
		}
		return File.GetLastWriteTime(path);
	}

	// Token: 0x06004CD5 RID: 19669 RVA: 0x001DAF88 File Offset: 0x001D9188
	[PublicizedFrom(EAccessModifier.Private)]
	public static DateTime ManagedGetLastWriteTime(SaveDataManagedPath path)
	{
		return SdFile.ManagedGetLastWriteTimeUtc(path).ToLocalTime();
	}

	// Token: 0x06004CD6 RID: 19670 RVA: 0x001DAFA4 File Offset: 0x001D91A4
	public static DateTime GetLastWriteTimeUtc(string path)
	{
		SaveDataManagedPath saveDataManagedPath;
		if (SaveDataUtils.TryGetManagedPath(path, out saveDataManagedPath))
		{
			return SdFile.ManagedGetLastWriteTimeUtc(saveDataManagedPath);
		}
		return File.GetLastWriteTimeUtc(path);
	}

	// Token: 0x06004CD7 RID: 19671 RVA: 0x001DAFC8 File Offset: 0x001D91C8
	[PublicizedFrom(EAccessModifier.Internal)]
	public static DateTime ManagedGetLastWriteTimeUtc(SaveDataManagedPath path)
	{
		return SaveDataUtils.SaveDataManager.ManagedFileGetLastWriteTimeUtc(path);
	}

	// Token: 0x06004CD8 RID: 19672 RVA: 0x001DAFD8 File Offset: 0x001D91D8
	public static Stream OpenRead(string path)
	{
		SaveDataManagedPath saveDataManagedPath;
		if (SaveDataUtils.TryGetManagedPath(path, out saveDataManagedPath))
		{
			return SdFile.ManagedOpen(saveDataManagedPath, FileMode.Open, FileAccess.Read, FileShare.Read);
		}
		return File.OpenRead(path);
	}

	// Token: 0x06004CD9 RID: 19673 RVA: 0x001DB000 File Offset: 0x001D9200
	public static Stream OpenWrite(string path)
	{
		SaveDataManagedPath saveDataManagedPath;
		if (SaveDataUtils.TryGetManagedPath(path, out saveDataManagedPath))
		{
			return SdFile.ManagedOpen(saveDataManagedPath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
		}
		return File.OpenWrite(path);
	}

	// Token: 0x06004CDA RID: 19674 RVA: 0x001DB028 File Offset: 0x001D9228
	public static string ReadAllText(string path)
	{
		SaveDataManagedPath saveDataManagedPath;
		if (SaveDataUtils.TryGetManagedPath(path, out saveDataManagedPath))
		{
			return SdFile.ManagedReadAllText(saveDataManagedPath, Encoding.UTF8);
		}
		return File.ReadAllText(path);
	}

	// Token: 0x06004CDB RID: 19675 RVA: 0x001DB054 File Offset: 0x001D9254
	public static string ReadAllText(string path, Encoding encoding)
	{
		SaveDataManagedPath saveDataManagedPath;
		if (SaveDataUtils.TryGetManagedPath(path, out saveDataManagedPath))
		{
			return SdFile.ManagedReadAllText(saveDataManagedPath, encoding);
		}
		return File.ReadAllText(path, encoding);
	}

	// Token: 0x06004CDC RID: 19676 RVA: 0x001DB07C File Offset: 0x001D927C
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

	// Token: 0x06004CDD RID: 19677 RVA: 0x001DB0D8 File Offset: 0x001D92D8
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

	// Token: 0x06004CDE RID: 19678 RVA: 0x001DB104 File Offset: 0x001D9304
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

	// Token: 0x06004CDF RID: 19679 RVA: 0x001DB12C File Offset: 0x001D932C
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

	// Token: 0x06004CE0 RID: 19680 RVA: 0x001DB184 File Offset: 0x001D9384
	public static byte[] ReadAllBytes(string path)
	{
		SaveDataManagedPath saveDataManagedPath;
		if (SaveDataUtils.TryGetManagedPath(path, out saveDataManagedPath))
		{
			return SdFile.ManagedReadAllBytes(saveDataManagedPath);
		}
		return File.ReadAllBytes(path);
	}

	// Token: 0x06004CE1 RID: 19681 RVA: 0x001DB1A8 File Offset: 0x001D93A8
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

	// Token: 0x06004CE2 RID: 19682 RVA: 0x001DB20C File Offset: 0x001D940C
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

	// Token: 0x06004CE3 RID: 19683 RVA: 0x001DB234 File Offset: 0x001D9434
	[PublicizedFrom(EAccessModifier.Private)]
	public static void ManagedWriteAllBytes(SaveDataManagedPath path, byte[] bytes)
	{
		using (Stream stream = SdFile.ManagedOpen(path, FileMode.Create, FileAccess.Write, FileShare.Read))
		{
			stream.Write(bytes, 0, bytes.Length);
		}
	}

	// Token: 0x06004CE4 RID: 19684 RVA: 0x001DB274 File Offset: 0x001D9474
	public static string[] ReadAllLines(string path)
	{
		SaveDataManagedPath saveDataManagedPath;
		if (SaveDataUtils.TryGetManagedPath(path, out saveDataManagedPath))
		{
			return SdFile.ManagedReadAllLines(saveDataManagedPath, Encoding.UTF8);
		}
		return File.ReadAllLines(path);
	}

	// Token: 0x06004CE5 RID: 19685 RVA: 0x001DB2A0 File Offset: 0x001D94A0
	public static string[] ReadAllLines(string path, Encoding encoding)
	{
		SaveDataManagedPath saveDataManagedPath;
		if (SaveDataUtils.TryGetManagedPath(path, out saveDataManagedPath))
		{
			return SdFile.ManagedReadAllLines(saveDataManagedPath, encoding);
		}
		return File.ReadAllLines(path, encoding);
	}

	// Token: 0x06004CE6 RID: 19686 RVA: 0x001DB2C8 File Offset: 0x001D94C8
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

	// Token: 0x06004CE7 RID: 19687 RVA: 0x001DB33C File Offset: 0x001D953C
	public static IEnumerable<string> ReadLines(string path)
	{
		SaveDataManagedPath saveDataManagedPath;
		if (SaveDataUtils.TryGetManagedPath(path, out saveDataManagedPath))
		{
			return SdFile.ManagedReadLines(saveDataManagedPath, Encoding.UTF8);
		}
		return File.ReadLines(path);
	}

	// Token: 0x06004CE8 RID: 19688 RVA: 0x001DB368 File Offset: 0x001D9568
	public static IEnumerable<string> ReadLines(string path, Encoding encoding)
	{
		SaveDataManagedPath saveDataManagedPath;
		if (SaveDataUtils.TryGetManagedPath(path, out saveDataManagedPath))
		{
			return SdFile.ManagedReadLines(saveDataManagedPath, encoding);
		}
		return File.ReadLines(path, encoding);
	}

	// Token: 0x06004CE9 RID: 19689 RVA: 0x001DB38E File Offset: 0x001D958E
	[PublicizedFrom(EAccessModifier.Private)]
	public static IEnumerable<string> ManagedReadLines(SaveDataManagedPath path, Encoding encoding)
	{
		SdFile.<ManagedReadLines>d__43 <ManagedReadLines>d__ = new SdFile.<ManagedReadLines>d__43(-2);
		<ManagedReadLines>d__.<>3__path = path;
		<ManagedReadLines>d__.<>3__encoding = encoding;
		return <ManagedReadLines>d__;
	}

	// Token: 0x06004CEA RID: 19690 RVA: 0x001DB3A8 File Offset: 0x001D95A8
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

	// Token: 0x06004CEB RID: 19691 RVA: 0x001DB3D4 File Offset: 0x001D95D4
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

	// Token: 0x06004CEC RID: 19692 RVA: 0x001DB3FC File Offset: 0x001D95FC
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

	// Token: 0x06004CED RID: 19693 RVA: 0x001DB428 File Offset: 0x001D9628
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

	// Token: 0x06004CEE RID: 19694 RVA: 0x001DB450 File Offset: 0x001D9650
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

	// Token: 0x06004CEF RID: 19695 RVA: 0x001DB4A8 File Offset: 0x001D96A8
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

	// Token: 0x06004CF0 RID: 19696 RVA: 0x001DB508 File Offset: 0x001D9708
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

	// Token: 0x06004CF1 RID: 19697 RVA: 0x001DB534 File Offset: 0x001D9734
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

	// Token: 0x06004CF2 RID: 19698 RVA: 0x001DB55C File Offset: 0x001D975C
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

	// Token: 0x06004CF3 RID: 19699 RVA: 0x001DB5B4 File Offset: 0x001D97B4
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

	// Token: 0x06004CF4 RID: 19700 RVA: 0x001DB5E0 File Offset: 0x001D97E0
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

	// Token: 0x06004CF5 RID: 19701 RVA: 0x001DB608 File Offset: 0x001D9808
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

	// Token: 0x04003C0D RID: 15373
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Encoding UTF8NoBom = new UTF8Encoding(false);
}

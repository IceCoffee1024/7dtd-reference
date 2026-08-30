using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

// Token: 0x020009F6 RID: 2550
public static class SdFile
{
	// Token: 0x06004C6A RID: 19562 RVA: 0x001D7138 File Offset: 0x001D5338
	public static StreamReader OpenText(string path)
	{
		SaveDataManagedPath saveDataManagedPath;
		if (SaveDataUtils.TryGetManagedPath(path, out saveDataManagedPath))
		{
			return SdFile.ManagedOpenText(saveDataManagedPath);
		}
		return File.OpenText(path);
	}

	// Token: 0x06004C6B RID: 19563 RVA: 0x001D715C File Offset: 0x001D535C
	[PublicizedFrom(EAccessModifier.Internal)]
	public static StreamReader ManagedOpenText(SaveDataManagedPath path)
	{
		return new StreamReader(SdFile.ManagedOpen(path, FileMode.Open, FileAccess.Read, FileShare.Read));
	}

	// Token: 0x06004C6C RID: 19564 RVA: 0x001D716C File Offset: 0x001D536C
	public static StreamWriter CreateText(string path)
	{
		SaveDataManagedPath saveDataManagedPath;
		if (SaveDataUtils.TryGetManagedPath(path, out saveDataManagedPath))
		{
			return SdFile.ManagedCreateText(saveDataManagedPath);
		}
		return File.CreateText(path);
	}

	// Token: 0x06004C6D RID: 19565 RVA: 0x001D7190 File Offset: 0x001D5390
	[PublicizedFrom(EAccessModifier.Internal)]
	public static StreamWriter ManagedCreateText(SaveDataManagedPath path)
	{
		return new StreamWriter(SdFile.ManagedOpen(path, FileMode.Create, FileAccess.Write, FileShare.Read));
	}

	// Token: 0x06004C6E RID: 19566 RVA: 0x001D71A0 File Offset: 0x001D53A0
	public static StreamWriter AppendText(string path)
	{
		SaveDataManagedPath saveDataManagedPath;
		if (SaveDataUtils.TryGetManagedPath(path, out saveDataManagedPath))
		{
			return SdFile.ManagedAppendText(saveDataManagedPath);
		}
		return File.AppendText(path);
	}

	// Token: 0x06004C6F RID: 19567 RVA: 0x001D71C4 File Offset: 0x001D53C4
	[PublicizedFrom(EAccessModifier.Internal)]
	public static StreamWriter ManagedAppendText(SaveDataManagedPath path)
	{
		return new StreamWriter(SdFile.ManagedOpen(path, FileMode.Append, FileAccess.Write, FileShare.Read));
	}

	// Token: 0x06004C70 RID: 19568 RVA: 0x001D71D4 File Offset: 0x001D53D4
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

	// Token: 0x06004C71 RID: 19569 RVA: 0x001D7220 File Offset: 0x001D5420
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

	// Token: 0x06004C72 RID: 19570 RVA: 0x001D7270 File Offset: 0x001D5470
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

	// Token: 0x06004C73 RID: 19571 RVA: 0x001D72D4 File Offset: 0x001D54D4
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

	// Token: 0x06004C74 RID: 19572 RVA: 0x001D7338 File Offset: 0x001D5538
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

	// Token: 0x06004C75 RID: 19573 RVA: 0x001D739C File Offset: 0x001D559C
	public static Stream Create(string path)
	{
		SaveDataManagedPath saveDataManagedPath;
		if (SaveDataUtils.TryGetManagedPath(path, out saveDataManagedPath))
		{
			return SdFile.ManagedCreate(saveDataManagedPath);
		}
		return File.Create(path);
	}

	// Token: 0x06004C76 RID: 19574 RVA: 0x001D73C0 File Offset: 0x001D55C0
	[PublicizedFrom(EAccessModifier.Internal)]
	public static Stream ManagedCreate(SaveDataManagedPath path)
	{
		return SdFile.ManagedOpen(path, FileMode.Create, FileAccess.ReadWrite, FileShare.None);
	}

	// Token: 0x06004C77 RID: 19575 RVA: 0x001D73CC File Offset: 0x001D55CC
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

	// Token: 0x06004C78 RID: 19576 RVA: 0x001D73F0 File Offset: 0x001D55F0
	[PublicizedFrom(EAccessModifier.Internal)]
	public static void ManagedDelete(SaveDataManagedPath path)
	{
		SaveDataUtils.SaveDataManager.ManagedFileDelete(path);
	}

	// Token: 0x06004C79 RID: 19577 RVA: 0x001D7400 File Offset: 0x001D5600
	public static bool Exists(string path)
	{
		SaveDataManagedPath saveDataManagedPath;
		if (SaveDataUtils.TryGetManagedPath(path, out saveDataManagedPath))
		{
			return SdFile.ManagedExists(saveDataManagedPath);
		}
		return File.Exists(path);
	}

	// Token: 0x06004C7A RID: 19578 RVA: 0x001D7424 File Offset: 0x001D5624
	[PublicizedFrom(EAccessModifier.Internal)]
	public static bool ManagedExists(SaveDataManagedPath path)
	{
		return SaveDataUtils.SaveDataManager.ManagedFileExists(path);
	}

	// Token: 0x06004C7B RID: 19579 RVA: 0x001D7434 File Offset: 0x001D5634
	public static Stream Open(string path, FileMode mode)
	{
		SaveDataManagedPath saveDataManagedPath;
		if (SaveDataUtils.TryGetManagedPath(path, out saveDataManagedPath))
		{
			return SdFile.ManagedOpen(saveDataManagedPath, mode, (mode == FileMode.Append) ? FileAccess.Write : FileAccess.ReadWrite, FileShare.None);
		}
		return File.Open(path, mode);
	}

	// Token: 0x06004C7C RID: 19580 RVA: 0x001D7464 File Offset: 0x001D5664
	public static Stream Open(string path, FileMode mode, FileAccess access)
	{
		SaveDataManagedPath saveDataManagedPath;
		if (SaveDataUtils.TryGetManagedPath(path, out saveDataManagedPath))
		{
			return SdFile.ManagedOpen(saveDataManagedPath, mode, access, FileShare.None);
		}
		return File.Open(path, mode, access);
	}

	// Token: 0x06004C7D RID: 19581 RVA: 0x001D7490 File Offset: 0x001D5690
	public static Stream Open(string path, FileMode mode, FileAccess access, FileShare share)
	{
		SaveDataManagedPath saveDataManagedPath;
		if (SaveDataUtils.TryGetManagedPath(path, out saveDataManagedPath))
		{
			return SdFile.ManagedOpen(saveDataManagedPath, mode, access, share);
		}
		return File.Open(path, mode, access, share);
	}

	// Token: 0x06004C7E RID: 19582 RVA: 0x001D74BA File Offset: 0x001D56BA
	[PublicizedFrom(EAccessModifier.Internal)]
	public static Stream ManagedOpen(SaveDataManagedPath path, FileMode mode, FileAccess access, FileShare share)
	{
		return SaveDataUtils.SaveDataManager.ManagedFileOpen(path, mode, access, share);
	}

	// Token: 0x06004C7F RID: 19583 RVA: 0x001D74CC File Offset: 0x001D56CC
	public static DateTime GetLastWriteTime(string path)
	{
		SaveDataManagedPath saveDataManagedPath;
		if (SaveDataUtils.TryGetManagedPath(path, out saveDataManagedPath))
		{
			return SdFile.ManagedGetLastWriteTime(saveDataManagedPath);
		}
		return File.GetLastWriteTime(path);
	}

	// Token: 0x06004C80 RID: 19584 RVA: 0x001D74F0 File Offset: 0x001D56F0
	[PublicizedFrom(EAccessModifier.Private)]
	public static DateTime ManagedGetLastWriteTime(SaveDataManagedPath path)
	{
		return SdFile.ManagedGetLastWriteTimeUtc(path).ToLocalTime();
	}

	// Token: 0x06004C81 RID: 19585 RVA: 0x001D750C File Offset: 0x001D570C
	public static DateTime GetLastWriteTimeUtc(string path)
	{
		SaveDataManagedPath saveDataManagedPath;
		if (SaveDataUtils.TryGetManagedPath(path, out saveDataManagedPath))
		{
			return SdFile.ManagedGetLastWriteTimeUtc(saveDataManagedPath);
		}
		return File.GetLastWriteTimeUtc(path);
	}

	// Token: 0x06004C82 RID: 19586 RVA: 0x001D7530 File Offset: 0x001D5730
	[PublicizedFrom(EAccessModifier.Internal)]
	public static DateTime ManagedGetLastWriteTimeUtc(SaveDataManagedPath path)
	{
		return SaveDataUtils.SaveDataManager.ManagedFileGetLastWriteTimeUtc(path);
	}

	// Token: 0x06004C83 RID: 19587 RVA: 0x001D7540 File Offset: 0x001D5740
	public static Stream OpenRead(string path)
	{
		SaveDataManagedPath saveDataManagedPath;
		if (SaveDataUtils.TryGetManagedPath(path, out saveDataManagedPath))
		{
			return SdFile.ManagedOpen(saveDataManagedPath, FileMode.Open, FileAccess.Read, FileShare.Read);
		}
		return File.OpenRead(path);
	}

	// Token: 0x06004C84 RID: 19588 RVA: 0x001D7568 File Offset: 0x001D5768
	public static Stream OpenWrite(string path)
	{
		SaveDataManagedPath saveDataManagedPath;
		if (SaveDataUtils.TryGetManagedPath(path, out saveDataManagedPath))
		{
			return SdFile.ManagedOpen(saveDataManagedPath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
		}
		return File.OpenWrite(path);
	}

	// Token: 0x06004C85 RID: 19589 RVA: 0x001D7590 File Offset: 0x001D5790
	public static string ReadAllText(string path)
	{
		SaveDataManagedPath saveDataManagedPath;
		if (SaveDataUtils.TryGetManagedPath(path, out saveDataManagedPath))
		{
			return SdFile.ManagedReadAllText(saveDataManagedPath, Encoding.UTF8);
		}
		return File.ReadAllText(path);
	}

	// Token: 0x06004C86 RID: 19590 RVA: 0x001D75BC File Offset: 0x001D57BC
	public static string ReadAllText(string path, Encoding encoding)
	{
		SaveDataManagedPath saveDataManagedPath;
		if (SaveDataUtils.TryGetManagedPath(path, out saveDataManagedPath))
		{
			return SdFile.ManagedReadAllText(saveDataManagedPath, encoding);
		}
		return File.ReadAllText(path, encoding);
	}

	// Token: 0x06004C87 RID: 19591 RVA: 0x001D75E4 File Offset: 0x001D57E4
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

	// Token: 0x06004C88 RID: 19592 RVA: 0x001D7640 File Offset: 0x001D5840
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

	// Token: 0x06004C89 RID: 19593 RVA: 0x001D766C File Offset: 0x001D586C
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

	// Token: 0x06004C8A RID: 19594 RVA: 0x001D7694 File Offset: 0x001D5894
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

	// Token: 0x06004C8B RID: 19595 RVA: 0x001D76EC File Offset: 0x001D58EC
	public static byte[] ReadAllBytes(string path)
	{
		SaveDataManagedPath saveDataManagedPath;
		if (SaveDataUtils.TryGetManagedPath(path, out saveDataManagedPath))
		{
			return SdFile.ManagedReadAllBytes(saveDataManagedPath);
		}
		return File.ReadAllBytes(path);
	}

	// Token: 0x06004C8C RID: 19596 RVA: 0x001D7710 File Offset: 0x001D5910
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

	// Token: 0x06004C8D RID: 19597 RVA: 0x001D7774 File Offset: 0x001D5974
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

	// Token: 0x06004C8E RID: 19598 RVA: 0x001D779C File Offset: 0x001D599C
	[PublicizedFrom(EAccessModifier.Private)]
	public static void ManagedWriteAllBytes(SaveDataManagedPath path, byte[] bytes)
	{
		using (Stream stream = SdFile.ManagedOpen(path, FileMode.Create, FileAccess.Write, FileShare.Read))
		{
			stream.Write(bytes, 0, bytes.Length);
		}
	}

	// Token: 0x06004C8F RID: 19599 RVA: 0x001D77DC File Offset: 0x001D59DC
	public static string[] ReadAllLines(string path)
	{
		SaveDataManagedPath saveDataManagedPath;
		if (SaveDataUtils.TryGetManagedPath(path, out saveDataManagedPath))
		{
			return SdFile.ManagedReadAllLines(saveDataManagedPath, Encoding.UTF8);
		}
		return File.ReadAllLines(path);
	}

	// Token: 0x06004C90 RID: 19600 RVA: 0x001D7808 File Offset: 0x001D5A08
	public static string[] ReadAllLines(string path, Encoding encoding)
	{
		SaveDataManagedPath saveDataManagedPath;
		if (SaveDataUtils.TryGetManagedPath(path, out saveDataManagedPath))
		{
			return SdFile.ManagedReadAllLines(saveDataManagedPath, encoding);
		}
		return File.ReadAllLines(path, encoding);
	}

	// Token: 0x06004C91 RID: 19601 RVA: 0x001D7830 File Offset: 0x001D5A30
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

	// Token: 0x06004C92 RID: 19602 RVA: 0x001D78A4 File Offset: 0x001D5AA4
	public static IEnumerable<string> ReadLines(string path)
	{
		SaveDataManagedPath saveDataManagedPath;
		if (SaveDataUtils.TryGetManagedPath(path, out saveDataManagedPath))
		{
			return SdFile.ManagedReadLines(saveDataManagedPath, Encoding.UTF8);
		}
		return File.ReadLines(path);
	}

	// Token: 0x06004C93 RID: 19603 RVA: 0x001D78D0 File Offset: 0x001D5AD0
	public static IEnumerable<string> ReadLines(string path, Encoding encoding)
	{
		SaveDataManagedPath saveDataManagedPath;
		if (SaveDataUtils.TryGetManagedPath(path, out saveDataManagedPath))
		{
			return SdFile.ManagedReadLines(saveDataManagedPath, encoding);
		}
		return File.ReadLines(path, encoding);
	}

	// Token: 0x06004C94 RID: 19604 RVA: 0x001D78F6 File Offset: 0x001D5AF6
	[PublicizedFrom(EAccessModifier.Private)]
	public static IEnumerable<string> ManagedReadLines(SaveDataManagedPath path, Encoding encoding)
	{
		SdFile.<ManagedReadLines>d__43 <ManagedReadLines>d__ = new SdFile.<ManagedReadLines>d__43(-2);
		<ManagedReadLines>d__.<>3__path = path;
		<ManagedReadLines>d__.<>3__encoding = encoding;
		return <ManagedReadLines>d__;
	}

	// Token: 0x06004C95 RID: 19605 RVA: 0x001D7910 File Offset: 0x001D5B10
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

	// Token: 0x06004C96 RID: 19606 RVA: 0x001D793C File Offset: 0x001D5B3C
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

	// Token: 0x06004C97 RID: 19607 RVA: 0x001D7964 File Offset: 0x001D5B64
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

	// Token: 0x06004C98 RID: 19608 RVA: 0x001D7990 File Offset: 0x001D5B90
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

	// Token: 0x06004C99 RID: 19609 RVA: 0x001D79B8 File Offset: 0x001D5BB8
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

	// Token: 0x06004C9A RID: 19610 RVA: 0x001D7A10 File Offset: 0x001D5C10
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

	// Token: 0x06004C9B RID: 19611 RVA: 0x001D7A70 File Offset: 0x001D5C70
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

	// Token: 0x06004C9C RID: 19612 RVA: 0x001D7A9C File Offset: 0x001D5C9C
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

	// Token: 0x06004C9D RID: 19613 RVA: 0x001D7AC4 File Offset: 0x001D5CC4
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

	// Token: 0x06004C9E RID: 19614 RVA: 0x001D7B1C File Offset: 0x001D5D1C
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

	// Token: 0x06004C9F RID: 19615 RVA: 0x001D7B48 File Offset: 0x001D5D48
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

	// Token: 0x06004CA0 RID: 19616 RVA: 0x001D7B70 File Offset: 0x001D5D70
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

	// Token: 0x04003B6F RID: 15215
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Encoding UTF8NoBom = new UTF8Encoding(false);
}

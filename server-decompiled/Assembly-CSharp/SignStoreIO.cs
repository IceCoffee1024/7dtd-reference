using System;
using System.Buffers.Binary;
using System.IO;
using Force.Crc32;

// Token: 0x02000ACA RID: 2762
public static class SignStoreIO
{
	// Token: 0x0600526D RID: 21101 RVA: 0x001F90B4 File Offset: 0x001F72B4
	public unsafe static void BeginImage(PooledExpandableMemoryStream image)
	{
		image.SetLength(0L);
		image.Position = 0L;
		Span<byte> span = new Span<byte>(stackalloc byte[(UIntPtr)32], 32);
		span.Clear();
		image.Write(span);
	}

	// Token: 0x0600526E RID: 21102 RVA: 0x001F90F4 File Offset: 0x001F72F4
	public unsafe static void FinalizeImage(PooledExpandableMemoryStream image, uint magic, int version, int recordCount, byte flags = 0)
	{
		long num = image.Length - 32L;
		if (num > 2147483647L)
		{
			throw new InvalidDataException(string.Format("Store payload of {0} bytes exceeds the container's int32 limit.", num));
		}
		int num2 = (int)num;
		uint num3 = ((num2 > 0) ? Crc32Algorithm.Append(0U, image.GetBuffer(), 32, num2) : 0U);
		Span<byte> span = new Span<byte>(stackalloc byte[(UIntPtr)32], 32);
		span.Clear();
		BinaryPrimitives.WriteUInt32LittleEndian(span.Slice(0, 4), magic);
		BinaryPrimitives.WriteInt32LittleEndian(span.Slice(4, 4), version);
		BinaryPrimitives.WriteInt32LittleEndian(span.Slice(8, 4), recordCount);
		BinaryPrimitives.WriteUInt32LittleEndian(span.Slice(12, 4), num3);
		BinaryPrimitives.WriteInt32LittleEndian(span.Slice(16, 4), num2);
		*span[20] = flags;
		image.Position = 0L;
		image.Write(span);
		image.Position = 0L;
	}

	// Token: 0x0600526F RID: 21103 RVA: 0x001F91CE File Offset: 0x001F73CE
	public static void WriteWithBackup(string path, PooledExpandableMemoryStream image)
	{
		image.Position = 0L;
		StreamUtils.WriteStreamToFile(image, path);
		if (SdFile.Exists(path) && GameIO.FileSize(path) > 0L)
		{
			SdFile.Copy(path, path + ".bak", true);
		}
	}

	// Token: 0x06005270 RID: 21104 RVA: 0x001F9204 File Offset: 0x001F7404
	public static bool TryReadAllBytes(string logTag, string path, out byte[] image)
	{
		image = null;
		if (!SdFile.Exists(path))
		{
			return false;
		}
		bool flag;
		try
		{
			using (Stream stream = SdFile.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				long length = stream.Length;
				if (length < 32L)
				{
					Log.Warning(string.Format("{0} Ignoring '{1}': file length {2} is less than required header size {3}.", new object[] { logTag, path, length, 32 }));
					flag = false;
				}
				else
				{
					byte[] array = new byte[length];
					int i;
					int num;
					for (i = 0; i < array.Length; i += num)
					{
						num = stream.Read(array, i, array.Length - i);
						if (num <= 0)
						{
							break;
						}
					}
					if (i != array.Length)
					{
						Log.Warning(string.Format("{0} Ignoring '{1}': read {2} of {3} bytes.", new object[] { logTag, path, i, array.Length }));
						flag = false;
					}
					else
					{
						image = array;
						flag = true;
					}
				}
			}
		}
		catch (Exception ex)
		{
			Log.Warning(string.Concat(new string[] { logTag, " Ignoring '", path, "': ", ex.Message }));
			flag = false;
		}
		return flag;
	}

	// Token: 0x06005271 RID: 21105 RVA: 0x001F9340 File Offset: 0x001F7540
	public unsafe static bool TryValidateHeader(string logTag, string path, byte[] image, uint magic, int minReadableVersion, int currentVersion, out int version, out int recordCount, out byte flags)
	{
		version = 0;
		recordCount = 0;
		flags = 0;
		ReadOnlySpan<byte> readOnlySpan = new ReadOnlySpan<byte>(image, 0, 32);
		if (BinaryPrimitives.ReadUInt32LittleEndian(readOnlySpan.Slice(0, 4)) != magic)
		{
			Log.Warning(logTag + " Ignoring '" + path + "': invalid magic bytes.");
			return false;
		}
		version = BinaryPrimitives.ReadInt32LittleEndian(readOnlySpan.Slice(4, 4));
		if (version < minReadableVersion || version > currentVersion)
		{
			Log.Warning(string.Format("{0} Ignoring '{1}': container version {2} is not readable by this build ", logTag, path, version) + string.Format("(writes {0}, reads {1} and above).", currentVersion, minReadableVersion));
			return false;
		}
		recordCount = BinaryPrimitives.ReadInt32LittleEndian(readOnlySpan.Slice(8, 4));
		uint num = BinaryPrimitives.ReadUInt32LittleEndian(readOnlySpan.Slice(12, 4));
		int num2 = BinaryPrimitives.ReadInt32LittleEndian(readOnlySpan.Slice(16, 4));
		flags = *readOnlySpan[20];
		if (recordCount < 0 || num2 < 0 || 32L + (long)num2 != (long)image.Length)
		{
			Log.Warning(string.Format("{0} Ignoring '{1}': Size mismatch. Header records {2} payload bytes for ", logTag, path, num2) + string.Format("{0} record(s), file holds {1}.", recordCount, image.Length - 32));
			return false;
		}
		if (((num2 > 0) ? Crc32Algorithm.Append(0U, image, 32, num2) : 0U) != num)
		{
			Log.Warning(logTag + " Ignoring '" + path + "': Checksum mismatch, likely an interrupted write.");
			return false;
		}
		return true;
	}

	// Token: 0x04004013 RID: 16403
	public const int cHeaderSize = 32;

	// Token: 0x04004014 RID: 16404
	public const string cBackupExt = ".bak";
}

using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

// Token: 0x02001420 RID: 5152
public static class IncrementalHashExtensions
{
	// Token: 0x0600A1EA RID: 41450 RVA: 0x003CB90E File Offset: 0x003C9B0E
	public unsafe static void AppendDataNoAlloc(this IncrementalHash hash, short value)
	{
		*Unsafe.As<byte, short>(ref IncrementalHashExtensions.scratch[0]) = value;
		if (!BitConverter.IsLittleEndian)
		{
			Array.Reverse<byte>(IncrementalHashExtensions.scratch, 0, 2);
		}
		hash.AppendData(IncrementalHashExtensions.scratch, 0, 2);
	}

	// Token: 0x0600A1EB RID: 41451 RVA: 0x003CB942 File Offset: 0x003C9B42
	public unsafe static void AppendDataNoAlloc(this IncrementalHash hash, ushort value)
	{
		*Unsafe.As<byte, ushort>(ref IncrementalHashExtensions.scratch[0]) = value;
		if (!BitConverter.IsLittleEndian)
		{
			Array.Reverse<byte>(IncrementalHashExtensions.scratch, 0, 2);
		}
		hash.AppendData(IncrementalHashExtensions.scratch, 0, 2);
	}

	// Token: 0x0600A1EC RID: 41452 RVA: 0x003CB976 File Offset: 0x003C9B76
	public unsafe static void AppendDataNoAlloc(this IncrementalHash hash, int value)
	{
		*Unsafe.As<byte, int>(ref IncrementalHashExtensions.scratch[0]) = value;
		if (!BitConverter.IsLittleEndian)
		{
			Array.Reverse<byte>(IncrementalHashExtensions.scratch, 0, 4);
		}
		hash.AppendData(IncrementalHashExtensions.scratch, 0, 4);
	}

	// Token: 0x0600A1ED RID: 41453 RVA: 0x003CB9AA File Offset: 0x003C9BAA
	public unsafe static void AppendDataNoAlloc(this IncrementalHash hash, uint value)
	{
		*Unsafe.As<byte, uint>(ref IncrementalHashExtensions.scratch[0]) = value;
		if (!BitConverter.IsLittleEndian)
		{
			Array.Reverse<byte>(IncrementalHashExtensions.scratch, 0, 4);
		}
		hash.AppendData(IncrementalHashExtensions.scratch, 0, 4);
	}

	// Token: 0x0600A1EE RID: 41454 RVA: 0x003CB9DE File Offset: 0x003C9BDE
	public unsafe static void AppendDataNoAlloc(this IncrementalHash hash, long value)
	{
		*Unsafe.As<byte, long>(ref IncrementalHashExtensions.scratch[0]) = value;
		if (!BitConverter.IsLittleEndian)
		{
			Array.Reverse<byte>(IncrementalHashExtensions.scratch, 0, 8);
		}
		hash.AppendData(IncrementalHashExtensions.scratch, 0, 8);
	}

	// Token: 0x0600A1EF RID: 41455 RVA: 0x003CBA12 File Offset: 0x003C9C12
	public unsafe static void AppendDataNoAlloc(this IncrementalHash hash, ulong value)
	{
		*Unsafe.As<byte, ulong>(ref IncrementalHashExtensions.scratch[0]) = value;
		if (!BitConverter.IsLittleEndian)
		{
			Array.Reverse<byte>(IncrementalHashExtensions.scratch, 0, 8);
		}
		hash.AppendData(IncrementalHashExtensions.scratch, 0, 8);
	}

	// Token: 0x0600A1F0 RID: 41456 RVA: 0x003CBA46 File Offset: 0x003C9C46
	public unsafe static void AppendDataNoAlloc(this IncrementalHash hash, float value)
	{
		*Unsafe.As<byte, float>(ref IncrementalHashExtensions.scratch[0]) = value;
		if (!BitConverter.IsLittleEndian)
		{
			Array.Reverse<byte>(IncrementalHashExtensions.scratch, 0, 4);
		}
		hash.AppendData(IncrementalHashExtensions.scratch, 0, 4);
	}

	// Token: 0x0600A1F1 RID: 41457 RVA: 0x003CBA7A File Offset: 0x003C9C7A
	public static void AppendDataNoAlloc(this IncrementalHash hash, bool value)
	{
		IncrementalHashExtensions.scratch[0] = (value ? 1 : 0);
		hash.AppendData(IncrementalHashExtensions.scratch, 0, 1);
	}

	// Token: 0x040079AF RID: 31151
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly byte[] scratch = new byte[8];
}

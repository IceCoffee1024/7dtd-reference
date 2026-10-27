using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

// Token: 0x02001452 RID: 5202
public static class IncrementalHashExtensions
{
	// Token: 0x0600A2ED RID: 41709 RVA: 0x003D28AE File Offset: 0x003D0AAE
	public unsafe static void AppendDataNoAlloc(this IncrementalHash hash, short value)
	{
		*Unsafe.As<byte, short>(ref IncrementalHashExtensions.scratch[0]) = value;
		if (!BitConverter.IsLittleEndian)
		{
			Array.Reverse<byte>(IncrementalHashExtensions.scratch, 0, 2);
		}
		hash.AppendData(IncrementalHashExtensions.scratch, 0, 2);
	}

	// Token: 0x0600A2EE RID: 41710 RVA: 0x003D28E2 File Offset: 0x003D0AE2
	public unsafe static void AppendDataNoAlloc(this IncrementalHash hash, ushort value)
	{
		*Unsafe.As<byte, ushort>(ref IncrementalHashExtensions.scratch[0]) = value;
		if (!BitConverter.IsLittleEndian)
		{
			Array.Reverse<byte>(IncrementalHashExtensions.scratch, 0, 2);
		}
		hash.AppendData(IncrementalHashExtensions.scratch, 0, 2);
	}

	// Token: 0x0600A2EF RID: 41711 RVA: 0x003D2916 File Offset: 0x003D0B16
	public unsafe static void AppendDataNoAlloc(this IncrementalHash hash, int value)
	{
		*Unsafe.As<byte, int>(ref IncrementalHashExtensions.scratch[0]) = value;
		if (!BitConverter.IsLittleEndian)
		{
			Array.Reverse<byte>(IncrementalHashExtensions.scratch, 0, 4);
		}
		hash.AppendData(IncrementalHashExtensions.scratch, 0, 4);
	}

	// Token: 0x0600A2F0 RID: 41712 RVA: 0x003D294A File Offset: 0x003D0B4A
	public unsafe static void AppendDataNoAlloc(this IncrementalHash hash, uint value)
	{
		*Unsafe.As<byte, uint>(ref IncrementalHashExtensions.scratch[0]) = value;
		if (!BitConverter.IsLittleEndian)
		{
			Array.Reverse<byte>(IncrementalHashExtensions.scratch, 0, 4);
		}
		hash.AppendData(IncrementalHashExtensions.scratch, 0, 4);
	}

	// Token: 0x0600A2F1 RID: 41713 RVA: 0x003D297E File Offset: 0x003D0B7E
	public unsafe static void AppendDataNoAlloc(this IncrementalHash hash, long value)
	{
		*Unsafe.As<byte, long>(ref IncrementalHashExtensions.scratch[0]) = value;
		if (!BitConverter.IsLittleEndian)
		{
			Array.Reverse<byte>(IncrementalHashExtensions.scratch, 0, 8);
		}
		hash.AppendData(IncrementalHashExtensions.scratch, 0, 8);
	}

	// Token: 0x0600A2F2 RID: 41714 RVA: 0x003D29B2 File Offset: 0x003D0BB2
	public unsafe static void AppendDataNoAlloc(this IncrementalHash hash, ulong value)
	{
		*Unsafe.As<byte, ulong>(ref IncrementalHashExtensions.scratch[0]) = value;
		if (!BitConverter.IsLittleEndian)
		{
			Array.Reverse<byte>(IncrementalHashExtensions.scratch, 0, 8);
		}
		hash.AppendData(IncrementalHashExtensions.scratch, 0, 8);
	}

	// Token: 0x0600A2F3 RID: 41715 RVA: 0x003D29E6 File Offset: 0x003D0BE6
	public unsafe static void AppendDataNoAlloc(this IncrementalHash hash, float value)
	{
		*Unsafe.As<byte, float>(ref IncrementalHashExtensions.scratch[0]) = value;
		if (!BitConverter.IsLittleEndian)
		{
			Array.Reverse<byte>(IncrementalHashExtensions.scratch, 0, 4);
		}
		hash.AppendData(IncrementalHashExtensions.scratch, 0, 4);
	}

	// Token: 0x0600A2F4 RID: 41716 RVA: 0x003D2A1A File Offset: 0x003D0C1A
	public static void AppendDataNoAlloc(this IncrementalHash hash, bool value)
	{
		IncrementalHashExtensions.scratch[0] = (value ? 1 : 0);
		hash.AppendData(IncrementalHashExtensions.scratch, 0, 1);
	}

	// Token: 0x04007A6B RID: 31339
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly byte[] scratch = new byte[8];
}

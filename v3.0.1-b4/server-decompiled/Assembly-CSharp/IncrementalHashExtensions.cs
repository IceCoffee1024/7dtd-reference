using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

// Token: 0x02001405 RID: 5125
public static class IncrementalHashExtensions
{
	// Token: 0x0600A0ED RID: 41197 RVA: 0x003C65CA File Offset: 0x003C47CA
	public unsafe static void AppendDataNoAlloc(this IncrementalHash hash, short value)
	{
		*Unsafe.As<byte, short>(ref IncrementalHashExtensions.scratch[0]) = value;
		if (!BitConverter.IsLittleEndian)
		{
			Array.Reverse<byte>(IncrementalHashExtensions.scratch, 0, 2);
		}
		hash.AppendData(IncrementalHashExtensions.scratch, 0, 2);
	}

	// Token: 0x0600A0EE RID: 41198 RVA: 0x003C65FE File Offset: 0x003C47FE
	public unsafe static void AppendDataNoAlloc(this IncrementalHash hash, ushort value)
	{
		*Unsafe.As<byte, ushort>(ref IncrementalHashExtensions.scratch[0]) = value;
		if (!BitConverter.IsLittleEndian)
		{
			Array.Reverse<byte>(IncrementalHashExtensions.scratch, 0, 2);
		}
		hash.AppendData(IncrementalHashExtensions.scratch, 0, 2);
	}

	// Token: 0x0600A0EF RID: 41199 RVA: 0x003C6632 File Offset: 0x003C4832
	public unsafe static void AppendDataNoAlloc(this IncrementalHash hash, int value)
	{
		*Unsafe.As<byte, int>(ref IncrementalHashExtensions.scratch[0]) = value;
		if (!BitConverter.IsLittleEndian)
		{
			Array.Reverse<byte>(IncrementalHashExtensions.scratch, 0, 4);
		}
		hash.AppendData(IncrementalHashExtensions.scratch, 0, 4);
	}

	// Token: 0x0600A0F0 RID: 41200 RVA: 0x003C6666 File Offset: 0x003C4866
	public unsafe static void AppendDataNoAlloc(this IncrementalHash hash, uint value)
	{
		*Unsafe.As<byte, uint>(ref IncrementalHashExtensions.scratch[0]) = value;
		if (!BitConverter.IsLittleEndian)
		{
			Array.Reverse<byte>(IncrementalHashExtensions.scratch, 0, 4);
		}
		hash.AppendData(IncrementalHashExtensions.scratch, 0, 4);
	}

	// Token: 0x0600A0F1 RID: 41201 RVA: 0x003C669A File Offset: 0x003C489A
	public unsafe static void AppendDataNoAlloc(this IncrementalHash hash, long value)
	{
		*Unsafe.As<byte, long>(ref IncrementalHashExtensions.scratch[0]) = value;
		if (!BitConverter.IsLittleEndian)
		{
			Array.Reverse<byte>(IncrementalHashExtensions.scratch, 0, 8);
		}
		hash.AppendData(IncrementalHashExtensions.scratch, 0, 8);
	}

	// Token: 0x0600A0F2 RID: 41202 RVA: 0x003C66CE File Offset: 0x003C48CE
	public unsafe static void AppendDataNoAlloc(this IncrementalHash hash, ulong value)
	{
		*Unsafe.As<byte, ulong>(ref IncrementalHashExtensions.scratch[0]) = value;
		if (!BitConverter.IsLittleEndian)
		{
			Array.Reverse<byte>(IncrementalHashExtensions.scratch, 0, 8);
		}
		hash.AppendData(IncrementalHashExtensions.scratch, 0, 8);
	}

	// Token: 0x0600A0F3 RID: 41203 RVA: 0x003C6702 File Offset: 0x003C4902
	public unsafe static void AppendDataNoAlloc(this IncrementalHash hash, float value)
	{
		*Unsafe.As<byte, float>(ref IncrementalHashExtensions.scratch[0]) = value;
		if (!BitConverter.IsLittleEndian)
		{
			Array.Reverse<byte>(IncrementalHashExtensions.scratch, 0, 4);
		}
		hash.AppendData(IncrementalHashExtensions.scratch, 0, 4);
	}

	// Token: 0x0600A0F4 RID: 41204 RVA: 0x003C6736 File Offset: 0x003C4936
	public static void AppendDataNoAlloc(this IncrementalHash hash, bool value)
	{
		IncrementalHashExtensions.scratch[0] = (value ? 1 : 0);
		hash.AppendData(IncrementalHashExtensions.scratch, 0, 1);
	}

	// Token: 0x040078D6 RID: 30934
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly byte[] scratch = new byte[8];
}

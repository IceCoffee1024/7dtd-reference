using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

// Token: 0x02001440 RID: 5184
public static class IncrementalHashExtensions
{
	// Token: 0x0600A295 RID: 41621 RVA: 0x003D04AA File Offset: 0x003CE6AA
	public unsafe static void AppendDataNoAlloc(this IncrementalHash hash, short value)
	{
		*Unsafe.As<byte, short>(ref IncrementalHashExtensions.scratch[0]) = value;
		if (!BitConverter.IsLittleEndian)
		{
			Array.Reverse<byte>(IncrementalHashExtensions.scratch, 0, 2);
		}
		hash.AppendData(IncrementalHashExtensions.scratch, 0, 2);
	}

	// Token: 0x0600A296 RID: 41622 RVA: 0x003D04DE File Offset: 0x003CE6DE
	public unsafe static void AppendDataNoAlloc(this IncrementalHash hash, ushort value)
	{
		*Unsafe.As<byte, ushort>(ref IncrementalHashExtensions.scratch[0]) = value;
		if (!BitConverter.IsLittleEndian)
		{
			Array.Reverse<byte>(IncrementalHashExtensions.scratch, 0, 2);
		}
		hash.AppendData(IncrementalHashExtensions.scratch, 0, 2);
	}

	// Token: 0x0600A297 RID: 41623 RVA: 0x003D0512 File Offset: 0x003CE712
	public unsafe static void AppendDataNoAlloc(this IncrementalHash hash, int value)
	{
		*Unsafe.As<byte, int>(ref IncrementalHashExtensions.scratch[0]) = value;
		if (!BitConverter.IsLittleEndian)
		{
			Array.Reverse<byte>(IncrementalHashExtensions.scratch, 0, 4);
		}
		hash.AppendData(IncrementalHashExtensions.scratch, 0, 4);
	}

	// Token: 0x0600A298 RID: 41624 RVA: 0x003D0546 File Offset: 0x003CE746
	public unsafe static void AppendDataNoAlloc(this IncrementalHash hash, uint value)
	{
		*Unsafe.As<byte, uint>(ref IncrementalHashExtensions.scratch[0]) = value;
		if (!BitConverter.IsLittleEndian)
		{
			Array.Reverse<byte>(IncrementalHashExtensions.scratch, 0, 4);
		}
		hash.AppendData(IncrementalHashExtensions.scratch, 0, 4);
	}

	// Token: 0x0600A299 RID: 41625 RVA: 0x003D057A File Offset: 0x003CE77A
	public unsafe static void AppendDataNoAlloc(this IncrementalHash hash, long value)
	{
		*Unsafe.As<byte, long>(ref IncrementalHashExtensions.scratch[0]) = value;
		if (!BitConverter.IsLittleEndian)
		{
			Array.Reverse<byte>(IncrementalHashExtensions.scratch, 0, 8);
		}
		hash.AppendData(IncrementalHashExtensions.scratch, 0, 8);
	}

	// Token: 0x0600A29A RID: 41626 RVA: 0x003D05AE File Offset: 0x003CE7AE
	public unsafe static void AppendDataNoAlloc(this IncrementalHash hash, ulong value)
	{
		*Unsafe.As<byte, ulong>(ref IncrementalHashExtensions.scratch[0]) = value;
		if (!BitConverter.IsLittleEndian)
		{
			Array.Reverse<byte>(IncrementalHashExtensions.scratch, 0, 8);
		}
		hash.AppendData(IncrementalHashExtensions.scratch, 0, 8);
	}

	// Token: 0x0600A29B RID: 41627 RVA: 0x003D05E2 File Offset: 0x003CE7E2
	public unsafe static void AppendDataNoAlloc(this IncrementalHash hash, float value)
	{
		*Unsafe.As<byte, float>(ref IncrementalHashExtensions.scratch[0]) = value;
		if (!BitConverter.IsLittleEndian)
		{
			Array.Reverse<byte>(IncrementalHashExtensions.scratch, 0, 4);
		}
		hash.AppendData(IncrementalHashExtensions.scratch, 0, 4);
	}

	// Token: 0x0600A29C RID: 41628 RVA: 0x003D0616 File Offset: 0x003CE816
	public static void AppendDataNoAlloc(this IncrementalHash hash, bool value)
	{
		IncrementalHashExtensions.scratch[0] = (value ? 1 : 0);
		hash.AppendData(IncrementalHashExtensions.scratch, 0, 1);
	}

	// Token: 0x04007A2A RID: 31274
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly byte[] scratch = new byte[8];
}

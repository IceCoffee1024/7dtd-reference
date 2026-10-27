using System;

// Token: 0x020013BC RID: 5052
public static class BackedArrayHandleModeExtensions
{
	// Token: 0x06009ECA RID: 40650 RVA: 0x003C0818 File Offset: 0x003BEA18
	public static bool CanRead(this BackedArrayHandleMode mode)
	{
		bool flag;
		if (mode != BackedArrayHandleMode.ReadOnly)
		{
			if (mode != BackedArrayHandleMode.ReadWrite)
			{
				throw new ArgumentOutOfRangeException("mode", mode, string.Format("Unknown mode: {0}", mode));
			}
			flag = true;
		}
		else
		{
			flag = true;
		}
		return flag;
	}

	// Token: 0x06009ECB RID: 40651 RVA: 0x003C0858 File Offset: 0x003BEA58
	public static bool CanWrite(this BackedArrayHandleMode mode)
	{
		bool flag;
		if (mode != BackedArrayHandleMode.ReadOnly)
		{
			if (mode != BackedArrayHandleMode.ReadWrite)
			{
				throw new ArgumentOutOfRangeException("mode", mode, string.Format("Unknown mode: {0}", mode));
			}
			flag = true;
		}
		else
		{
			flag = false;
		}
		return flag;
	}
}

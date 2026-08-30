using System;

// Token: 0x0200138A RID: 5002
public static class BackedArrayHandleModeExtensions
{
	// Token: 0x06009DC7 RID: 40391 RVA: 0x003B9898 File Offset: 0x003B7A98
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

	// Token: 0x06009DC8 RID: 40392 RVA: 0x003B98D8 File Offset: 0x003B7AD8
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

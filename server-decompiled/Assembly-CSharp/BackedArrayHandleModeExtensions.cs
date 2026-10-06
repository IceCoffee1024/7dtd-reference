using System;

// Token: 0x020013AA RID: 5034
public static class BackedArrayHandleModeExtensions
{
	// Token: 0x06009E72 RID: 40562 RVA: 0x003BE414 File Offset: 0x003BC614
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

	// Token: 0x06009E73 RID: 40563 RVA: 0x003BE454 File Offset: 0x003BC654
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

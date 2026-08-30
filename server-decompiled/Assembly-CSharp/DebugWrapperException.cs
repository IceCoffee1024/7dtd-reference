using System;

// Token: 0x020013D3 RID: 5075
public class DebugWrapperException : Exception
{
	// Token: 0x06009F78 RID: 40824 RVA: 0x003C121F File Offset: 0x003BF41F
	public DebugWrapperException()
	{
	}

	// Token: 0x06009F79 RID: 40825 RVA: 0x003C1227 File Offset: 0x003BF427
	public DebugWrapperException(string message)
		: base(message)
	{
	}

	// Token: 0x06009F7A RID: 40826 RVA: 0x003C1230 File Offset: 0x003BF430
	public DebugWrapperException(string message, Exception innerException)
		: base(message, innerException)
	{
	}
}

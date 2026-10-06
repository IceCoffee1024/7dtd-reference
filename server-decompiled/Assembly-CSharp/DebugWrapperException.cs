using System;

// Token: 0x020013F3 RID: 5107
public class DebugWrapperException : Exception
{
	// Token: 0x0600A023 RID: 40995 RVA: 0x003C5DBB File Offset: 0x003C3FBB
	public DebugWrapperException()
	{
	}

	// Token: 0x0600A024 RID: 40996 RVA: 0x003C5DC3 File Offset: 0x003C3FC3
	public DebugWrapperException(string message)
		: base(message)
	{
	}

	// Token: 0x0600A025 RID: 40997 RVA: 0x003C5DCC File Offset: 0x003C3FCC
	public DebugWrapperException(string message, Exception innerException)
		: base(message, innerException)
	{
	}
}

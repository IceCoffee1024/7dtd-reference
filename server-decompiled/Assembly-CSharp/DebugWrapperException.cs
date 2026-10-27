using System;

// Token: 0x02001405 RID: 5125
public class DebugWrapperException : Exception
{
	// Token: 0x0600A07B RID: 41083 RVA: 0x003C81BF File Offset: 0x003C63BF
	public DebugWrapperException()
	{
	}

	// Token: 0x0600A07C RID: 41084 RVA: 0x003C81C7 File Offset: 0x003C63C7
	public DebugWrapperException(string message)
		: base(message)
	{
	}

	// Token: 0x0600A07D RID: 41085 RVA: 0x003C81D0 File Offset: 0x003C63D0
	public DebugWrapperException(string message, Exception innerException)
		: base(message, innerException)
	{
	}
}

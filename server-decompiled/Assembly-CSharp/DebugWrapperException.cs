using System;

// Token: 0x020013B8 RID: 5048
public class DebugWrapperException : Exception
{
	// Token: 0x06009E7B RID: 40571 RVA: 0x003BBEDB File Offset: 0x003BA0DB
	public DebugWrapperException()
	{
	}

	// Token: 0x06009E7C RID: 40572 RVA: 0x003BBEE3 File Offset: 0x003BA0E3
	public DebugWrapperException(string message)
		: base(message)
	{
	}

	// Token: 0x06009E7D RID: 40573 RVA: 0x003BBEEC File Offset: 0x003BA0EC
	public DebugWrapperException(string message, Exception innerException)
		: base(message, innerException)
	{
	}
}

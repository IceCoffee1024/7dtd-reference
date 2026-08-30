using System;

// Token: 0x020001C2 RID: 450
public class BlockRule<O, M>
{
	// Token: 0x06000DD7 RID: 3543 RVA: 0x0000640C File Offset: 0x0000460C
	public BlockRule()
	{
	}

	// Token: 0x06000DD8 RID: 3544 RVA: 0x0005B697 File Offset: 0x00059897
	public BlockRule(O _output, M[] _mask)
	{
		this.Output = _output;
		this.Mask = _mask;
	}

	// Token: 0x04000BE9 RID: 3049
	public O Output;

	// Token: 0x04000BEA RID: 3050
	public M[] Mask;
}

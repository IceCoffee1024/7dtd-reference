using System;

// Token: 0x020001C5 RID: 453
public class BlockRule<O, M>
{
	// Token: 0x06000DD9 RID: 3545 RVA: 0x0000640C File Offset: 0x0000460C
	public BlockRule()
	{
	}

	// Token: 0x06000DDA RID: 3546 RVA: 0x0005BAB7 File Offset: 0x00059CB7
	public BlockRule(O _output, M[] _mask)
	{
		this.Output = _output;
		this.Mask = _mask;
	}

	// Token: 0x04000BEF RID: 3055
	public O Output;

	// Token: 0x04000BF0 RID: 3056
	public M[] Mask;
}

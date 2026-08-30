using System;

// Token: 0x02000713 RID: 1811
public class TagGroup
{
	// Token: 0x02000714 RID: 1812
	public abstract class TagsGroupAbs
	{
		// Token: 0x060036CB RID: 14027 RVA: 0x0000640C File Offset: 0x0000460C
		[PublicizedFrom(EAccessModifier.Protected)]
		public TagsGroupAbs()
		{
		}
	}

	// Token: 0x02000715 RID: 1813
	public class Global : TagGroup.TagsGroupAbs
	{
	}

	// Token: 0x02000716 RID: 1814
	public class Poi : TagGroup.TagsGroupAbs
	{
	}
}

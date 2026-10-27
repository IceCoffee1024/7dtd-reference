using System;

// Token: 0x02000727 RID: 1831
public class TagGroup
{
	// Token: 0x02000728 RID: 1832
	public abstract class TagsGroupAbs
	{
		// Token: 0x0600377B RID: 14203 RVA: 0x0000640C File Offset: 0x0000460C
		[PublicizedFrom(EAccessModifier.Protected)]
		public TagsGroupAbs()
		{
		}
	}

	// Token: 0x02000729 RID: 1833
	public class Global : TagGroup.TagsGroupAbs
	{
	}

	// Token: 0x0200072A RID: 1834
	public class Poi : TagGroup.TagsGroupAbs
	{
	}
}

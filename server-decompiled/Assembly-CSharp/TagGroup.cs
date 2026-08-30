using System;

// Token: 0x0200071D RID: 1821
public class TagGroup
{
	// Token: 0x0200071E RID: 1822
	public abstract class TagsGroupAbs
	{
		// Token: 0x06003721 RID: 14113 RVA: 0x0000640C File Offset: 0x0000460C
		[PublicizedFrom(EAccessModifier.Protected)]
		public TagsGroupAbs()
		{
		}
	}

	// Token: 0x0200071F RID: 1823
	public class Global : TagGroup.TagsGroupAbs
	{
	}

	// Token: 0x02000720 RID: 1824
	public class Poi : TagGroup.TagsGroupAbs
	{
	}
}

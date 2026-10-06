using System;

// Token: 0x0200071F RID: 1823
public class TagGroup
{
	// Token: 0x02000720 RID: 1824
	public abstract class TagsGroupAbs
	{
		// Token: 0x06003764 RID: 14180 RVA: 0x0000640C File Offset: 0x0000460C
		[PublicizedFrom(EAccessModifier.Protected)]
		public TagsGroupAbs()
		{
		}
	}

	// Token: 0x02000721 RID: 1825
	public class Global : TagGroup.TagsGroupAbs
	{
	}

	// Token: 0x02000722 RID: 1826
	public class Poi : TagGroup.TagsGroupAbs
	{
	}
}

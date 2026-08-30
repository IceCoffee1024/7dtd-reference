using System;

namespace UnityEngine.SearchService
{
	// Token: 0x02000313 RID: 787
	[AttributeUsage(AttributeTargets.Field)]
	[Obsolete("ObjectSelectorHandlerWithTagsAttribute has been deprecated. Use SearchContextAttribute instead.", true)]
	public class ObjectSelectorHandlerWithTagsAttribute : Attribute
	{
		// Token: 0x17000628 RID: 1576
		// (get) Token: 0x06002032 RID: 8242 RVA: 0x000358EF File Offset: 0x00033AEF
		public string[] tags { get; }

		// Token: 0x06002033 RID: 8243 RVA: 0x000358F7 File Offset: 0x00033AF7
		public ObjectSelectorHandlerWithTagsAttribute(params string[] tags)
		{
			this.tags = tags;
		}
	}
}

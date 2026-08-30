using System;

namespace JetBrains.Annotations
{
	// Token: 0x020000D8 RID: 216
	[AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property)]
	public sealed class CollectionAccessAttribute : Attribute
	{
		// Token: 0x060003EC RID: 1004 RVA: 0x00006CBE File Offset: 0x00004EBE
		public CollectionAccessAttribute(CollectionAccessType collectionAccessType)
		{
			this.CollectionAccessType = collectionAccessType;
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x060003ED RID: 1005 RVA: 0x00006CCF File Offset: 0x00004ECF
		public CollectionAccessType CollectionAccessType { get; }
	}
}

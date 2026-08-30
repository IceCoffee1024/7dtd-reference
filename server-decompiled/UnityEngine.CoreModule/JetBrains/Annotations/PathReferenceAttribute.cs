using System;

namespace JetBrains.Annotations
{
	// Token: 0x020000D5 RID: 213
	[AttributeUsage(AttributeTargets.Parameter)]
	public sealed class PathReferenceAttribute : Attribute
	{
		// Token: 0x060003E1 RID: 993 RVA: 0x00002059 File Offset: 0x00000259
		public PathReferenceAttribute()
		{
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x00006C72 File Offset: 0x00004E72
		public PathReferenceAttribute([PathReference] [NotNull] string basePath)
		{
			this.BasePath = basePath;
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x060003E3 RID: 995 RVA: 0x00006C83 File Offset: 0x00004E83
		[CanBeNull]
		public string BasePath { get; }
	}
}

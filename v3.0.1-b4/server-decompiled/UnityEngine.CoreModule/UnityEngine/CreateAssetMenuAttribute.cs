using System;

namespace UnityEngine
{
	// Token: 0x0200022C RID: 556
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public sealed class CreateAssetMenuAttribute : Attribute
	{
		// Token: 0x170004BB RID: 1211
		// (get) Token: 0x06001848 RID: 6216 RVA: 0x0002849C File Offset: 0x0002669C
		// (set) Token: 0x06001849 RID: 6217 RVA: 0x000284A4 File Offset: 0x000266A4
		public string menuName { get; set; }

		// Token: 0x170004BC RID: 1212
		// (get) Token: 0x0600184A RID: 6218 RVA: 0x000284AD File Offset: 0x000266AD
		// (set) Token: 0x0600184B RID: 6219 RVA: 0x000284B5 File Offset: 0x000266B5
		public string fileName { get; set; }

		// Token: 0x170004BD RID: 1213
		// (get) Token: 0x0600184C RID: 6220 RVA: 0x000284BE File Offset: 0x000266BE
		// (set) Token: 0x0600184D RID: 6221 RVA: 0x000284C6 File Offset: 0x000266C6
		public int order { get; set; }
	}
}

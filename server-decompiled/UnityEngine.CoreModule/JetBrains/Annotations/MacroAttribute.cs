using System;

namespace JetBrains.Annotations
{
	// Token: 0x020000D7 RID: 215
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Parameter, AllowMultiple = true)]
	public sealed class MacroAttribute : Attribute
	{
		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x060003E5 RID: 997 RVA: 0x00006C8B File Offset: 0x00004E8B
		// (set) Token: 0x060003E6 RID: 998 RVA: 0x00006C93 File Offset: 0x00004E93
		[CanBeNull]
		public string Expression { get; set; }

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x060003E7 RID: 999 RVA: 0x00006C9C File Offset: 0x00004E9C
		// (set) Token: 0x060003E8 RID: 1000 RVA: 0x00006CA4 File Offset: 0x00004EA4
		public int Editable { get; set; }

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x060003E9 RID: 1001 RVA: 0x00006CAD File Offset: 0x00004EAD
		// (set) Token: 0x060003EA RID: 1002 RVA: 0x00006CB5 File Offset: 0x00004EB5
		[CanBeNull]
		public string Target { get; set; }
	}
}

using System;

namespace HarmonyLib
{
	// Token: 0x02000034 RID: 52
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Parameter, AllowMultiple = true)]
	public class HarmonyArgument : Attribute
	{
		// Token: 0x17000007 RID: 7
		// (get) Token: 0x060000AD RID: 173 RVA: 0x0000482E File Offset: 0x00002A2E
		// (set) Token: 0x060000AE RID: 174 RVA: 0x00004836 File Offset: 0x00002A36
		public string OriginalName { get; private set; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x060000AF RID: 175 RVA: 0x0000483F File Offset: 0x00002A3F
		// (set) Token: 0x060000B0 RID: 176 RVA: 0x00004847 File Offset: 0x00002A47
		public int Index { get; private set; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x060000B1 RID: 177 RVA: 0x00004850 File Offset: 0x00002A50
		// (set) Token: 0x060000B2 RID: 178 RVA: 0x00004858 File Offset: 0x00002A58
		public string NewName { get; private set; }

		// Token: 0x060000B3 RID: 179 RVA: 0x00004861 File Offset: 0x00002A61
		public HarmonyArgument(string originalName)
			: this(originalName, null)
		{
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x0000486B File Offset: 0x00002A6B
		public HarmonyArgument(int index)
			: this(index, null)
		{
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x00004875 File Offset: 0x00002A75
		public HarmonyArgument(string originalName, string newName)
		{
			this.OriginalName = originalName;
			this.Index = -1;
			this.NewName = newName;
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00004892 File Offset: 0x00002A92
		public HarmonyArgument(int index, string name)
		{
			this.OriginalName = null;
			this.Index = index;
			this.NewName = name;
		}
	}
}

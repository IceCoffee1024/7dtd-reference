using System;

namespace HarmonyLib
{
	// Token: 0x02000055 RID: 85
	public class Traverse<T>
	{
		// Token: 0x060003A1 RID: 929 RVA: 0x0000EF91 File Offset: 0x0000D191
		private Traverse()
		{
		}

		// Token: 0x060003A2 RID: 930 RVA: 0x0000EF99 File Offset: 0x0000D199
		public Traverse(Traverse traverse)
		{
			this.traverse = traverse;
		}

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x060003A3 RID: 931 RVA: 0x0000EFA8 File Offset: 0x0000D1A8
		// (set) Token: 0x060003A4 RID: 932 RVA: 0x0000EFB5 File Offset: 0x0000D1B5
		public T Value
		{
			get
			{
				return this.traverse.GetValue<T>();
			}
			set
			{
				this.traverse.SetValue(value);
			}
		}

		// Token: 0x040000C9 RID: 201
		private readonly Traverse traverse;
	}
}

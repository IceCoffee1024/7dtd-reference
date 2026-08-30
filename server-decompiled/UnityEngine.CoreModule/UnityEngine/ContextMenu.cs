using System;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x0200022D RID: 557
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
	[RequiredByNativeCode]
	public sealed class ContextMenu : Attribute
	{
		// Token: 0x0600184F RID: 6223 RVA: 0x000284CF File Offset: 0x000266CF
		public ContextMenu(string itemName)
			: this(itemName, false)
		{
		}

		// Token: 0x06001850 RID: 6224 RVA: 0x000284DB File Offset: 0x000266DB
		public ContextMenu(string itemName, bool isValidateFunction)
			: this(itemName, isValidateFunction, 1000000)
		{
		}

		// Token: 0x06001851 RID: 6225 RVA: 0x000284EC File Offset: 0x000266EC
		public ContextMenu(string itemName, bool isValidateFunction, int priority)
		{
			this.menuItem = itemName;
			this.validate = isValidateFunction;
			this.priority = priority;
		}

		// Token: 0x0400089C RID: 2204
		public readonly string menuItem;

		// Token: 0x0400089D RID: 2205
		public readonly bool validate;

		// Token: 0x0400089E RID: 2206
		public readonly int priority;
	}
}

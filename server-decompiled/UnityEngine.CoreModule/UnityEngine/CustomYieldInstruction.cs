using System;
using System.Collections;

namespace UnityEngine
{
	// Token: 0x0200023F RID: 575
	public abstract class CustomYieldInstruction : IEnumerator
	{
		// Token: 0x170004C5 RID: 1221
		// (get) Token: 0x060018AA RID: 6314
		public abstract bool keepWaiting { get; }

		// Token: 0x170004C6 RID: 1222
		// (get) Token: 0x060018AB RID: 6315 RVA: 0x00028F1C File Offset: 0x0002711C
		public object Current
		{
			get
			{
				return null;
			}
		}

		// Token: 0x060018AC RID: 6316 RVA: 0x00028F30 File Offset: 0x00027130
		public bool MoveNext()
		{
			return this.keepWaiting;
		}

		// Token: 0x060018AD RID: 6317 RVA: 0x00002669 File Offset: 0x00000869
		public virtual void Reset()
		{
		}
	}
}

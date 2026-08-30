using System;
using UnityEngine.Scripting;

namespace UAI
{
	// Token: 0x02001764 RID: 5988
	[Preserve]
	public struct ActionData
	{
		// Token: 0x1700162F RID: 5679
		// (get) Token: 0x0600B912 RID: 47378 RVA: 0x00451590 File Offset: 0x0044F790
		public UAITaskBase CurrentTask
		{
			get
			{
				if (this.Action == null || this.Action.GetTasks() == null || this.TaskIndex < 0 || this.TaskIndex >= this.Action.GetTasks().Count)
				{
					return null;
				}
				return this.Action.GetTasks()[this.TaskIndex];
			}
		}

		// Token: 0x0600B913 RID: 47379 RVA: 0x004515EB File Offset: 0x0044F7EB
		public void ClearData()
		{
			this.Data = null;
			this.TaskStartTimeStamp = 0UL;
			this.Initialized = false;
			this.Started = false;
			this.Executing = false;
			this.Failed = false;
			this.Finished = false;
		}

		// Token: 0x04008AB9 RID: 35513
		public UAIAction Action;

		// Token: 0x04008ABA RID: 35514
		public object Target;

		// Token: 0x04008ABB RID: 35515
		public object Data;

		// Token: 0x04008ABC RID: 35516
		public int TaskIndex;

		// Token: 0x04008ABD RID: 35517
		public ulong TaskStartTimeStamp;

		// Token: 0x04008ABE RID: 35518
		public bool Initialized;

		// Token: 0x04008ABF RID: 35519
		public bool Started;

		// Token: 0x04008AC0 RID: 35520
		public bool Executing;

		// Token: 0x04008AC1 RID: 35521
		public bool Failed;

		// Token: 0x04008AC2 RID: 35522
		public bool Finished;
	}
}

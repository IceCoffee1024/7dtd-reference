using System;
using UnityEngine.Scripting;

namespace UAI
{
	// Token: 0x020017B6 RID: 6070
	[Preserve]
	public struct ActionData
	{
		// Token: 0x17001743 RID: 5955
		// (get) Token: 0x0600BBEC RID: 48108 RVA: 0x0045B828 File Offset: 0x00459A28
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

		// Token: 0x0600BBED RID: 48109 RVA: 0x0045B883 File Offset: 0x00459A83
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

		// Token: 0x04008C9A RID: 35994
		public UAIAction Action;

		// Token: 0x04008C9B RID: 35995
		public object Target;

		// Token: 0x04008C9C RID: 35996
		public object Data;

		// Token: 0x04008C9D RID: 35997
		public int TaskIndex;

		// Token: 0x04008C9E RID: 35998
		public ulong TaskStartTimeStamp;

		// Token: 0x04008C9F RID: 35999
		public bool Initialized;

		// Token: 0x04008CA0 RID: 36000
		public bool Started;

		// Token: 0x04008CA1 RID: 36001
		public bool Executing;

		// Token: 0x04008CA2 RID: 36002
		public bool Failed;

		// Token: 0x04008CA3 RID: 36003
		public bool Finished;
	}
}

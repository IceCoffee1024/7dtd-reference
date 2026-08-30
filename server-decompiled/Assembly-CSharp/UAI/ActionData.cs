using System;
using UnityEngine.Scripting;

namespace UAI
{
	// Token: 0x0200178A RID: 6026
	[Preserve]
	public struct ActionData
	{
		// Token: 0x170016A1 RID: 5793
		// (get) Token: 0x0600BA95 RID: 47765 RVA: 0x00457308 File Offset: 0x00455508
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

		// Token: 0x0600BA96 RID: 47766 RVA: 0x00457363 File Offset: 0x00455563
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

		// Token: 0x04008BD5 RID: 35797
		public UAIAction Action;

		// Token: 0x04008BD6 RID: 35798
		public object Target;

		// Token: 0x04008BD7 RID: 35799
		public object Data;

		// Token: 0x04008BD8 RID: 35800
		public int TaskIndex;

		// Token: 0x04008BD9 RID: 35801
		public ulong TaskStartTimeStamp;

		// Token: 0x04008BDA RID: 35802
		public bool Initialized;

		// Token: 0x04008BDB RID: 35803
		public bool Started;

		// Token: 0x04008BDC RID: 35804
		public bool Executing;

		// Token: 0x04008BDD RID: 35805
		public bool Failed;

		// Token: 0x04008BDE RID: 35806
		public bool Finished;
	}
}

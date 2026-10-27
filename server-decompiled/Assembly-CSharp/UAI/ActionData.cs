using System;
using UnityEngine.Scripting;

namespace UAI
{
	// Token: 0x020017E7 RID: 6119
	[Preserve]
	public struct ActionData
	{
		// Token: 0x1700178B RID: 6027
		// (get) Token: 0x0600BD00 RID: 48384 RVA: 0x0045F1D4 File Offset: 0x0045D3D4
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

		// Token: 0x0600BD01 RID: 48385 RVA: 0x0045F22F File Offset: 0x0045D42F
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

		// Token: 0x04008D83 RID: 36227
		public UAIAction Action;

		// Token: 0x04008D84 RID: 36228
		public object Target;

		// Token: 0x04008D85 RID: 36229
		public object Data;

		// Token: 0x04008D86 RID: 36230
		public int TaskIndex;

		// Token: 0x04008D87 RID: 36231
		public ulong TaskStartTimeStamp;

		// Token: 0x04008D88 RID: 36232
		public bool Initialized;

		// Token: 0x04008D89 RID: 36233
		public bool Started;

		// Token: 0x04008D8A RID: 36234
		public bool Executing;

		// Token: 0x04008D8B RID: 36235
		public bool Failed;

		// Token: 0x04008D8C RID: 36236
		public bool Finished;
	}
}

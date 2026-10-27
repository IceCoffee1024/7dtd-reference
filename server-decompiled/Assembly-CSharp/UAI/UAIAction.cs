using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace UAI
{
	// Token: 0x020017E0 RID: 6112
	[Preserve]
	public class UAIAction
	{
		// Token: 0x17001788 RID: 6024
		// (get) Token: 0x0600BCE4 RID: 48356 RVA: 0x0045EB60 File Offset: 0x0045CD60
		// (set) Token: 0x0600BCE5 RID: 48357 RVA: 0x0045EB68 File Offset: 0x0045CD68
		public string Name
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001789 RID: 6025
		// (get) Token: 0x0600BCE6 RID: 48358 RVA: 0x0045EB71 File Offset: 0x0045CD71
		// (set) Token: 0x0600BCE7 RID: 48359 RVA: 0x0045EB79 File Offset: 0x0045CD79
		public float Weight
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x0600BCE8 RID: 48360 RVA: 0x0045EB82 File Offset: 0x0045CD82
		public UAIAction(string _name, float _weight)
		{
			this.Name = _name;
			this.Weight = _weight;
			this.considerations = new List<UAIConsiderationBase>();
			this.tasks = new List<UAITaskBase>();
		}

		// Token: 0x0600BCE9 RID: 48361 RVA: 0x0045EBB0 File Offset: 0x0045CDB0
		public float GetScore(Context _context, object _target, float min = 0f)
		{
			float num = 1f;
			if (this.considerations.Count == 0)
			{
				return num * this.Weight;
			}
			if (this.tasks.Count == 0)
			{
				return 0f;
			}
			for (int i = 0; i < this.considerations.Count; i++)
			{
				if (0f > num || num < min)
				{
					return 0f;
				}
				num *= this.considerations[i].ComputeResponseCurve(this.considerations[i].GetScore(_context, _target));
			}
			return (num + (1f - num) * (float)(1 - 1 / this.considerations.Count) * num) * this.Weight;
		}

		// Token: 0x0600BCEA RID: 48362 RVA: 0x0045EC5E File Offset: 0x0045CE5E
		public void AddConsideration(UAIConsiderationBase _c)
		{
			this.considerations.Add(_c);
		}

		// Token: 0x0600BCEB RID: 48363 RVA: 0x0045EC6C File Offset: 0x0045CE6C
		public void AddTask(UAITaskBase _t)
		{
			this.tasks.Add(_t);
		}

		// Token: 0x0600BCEC RID: 48364 RVA: 0x0045EC7A File Offset: 0x0045CE7A
		public List<UAIConsiderationBase> GetConsiderations()
		{
			return this.considerations;
		}

		// Token: 0x0600BCED RID: 48365 RVA: 0x0045EC82 File Offset: 0x0045CE82
		public List<UAITaskBase> GetTasks()
		{
			return this.tasks;
		}

		// Token: 0x04008D74 RID: 36212
		[PublicizedFrom(EAccessModifier.Private)]
		public List<UAITaskBase> tasks;

		// Token: 0x04008D75 RID: 36213
		[PublicizedFrom(EAccessModifier.Private)]
		public List<UAIConsiderationBase> considerations;
	}
}

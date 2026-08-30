using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace UAI
{
	// Token: 0x02001783 RID: 6019
	[Preserve]
	public class UAIAction
	{
		// Token: 0x1700169E RID: 5790
		// (get) Token: 0x0600BA79 RID: 47737 RVA: 0x00456C94 File Offset: 0x00454E94
		// (set) Token: 0x0600BA7A RID: 47738 RVA: 0x00456C9C File Offset: 0x00454E9C
		public string Name
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x1700169F RID: 5791
		// (get) Token: 0x0600BA7B RID: 47739 RVA: 0x00456CA5 File Offset: 0x00454EA5
		// (set) Token: 0x0600BA7C RID: 47740 RVA: 0x00456CAD File Offset: 0x00454EAD
		public float Weight
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x0600BA7D RID: 47741 RVA: 0x00456CB6 File Offset: 0x00454EB6
		public UAIAction(string _name, float _weight)
		{
			this.Name = _name;
			this.Weight = _weight;
			this.considerations = new List<UAIConsiderationBase>();
			this.tasks = new List<UAITaskBase>();
		}

		// Token: 0x0600BA7E RID: 47742 RVA: 0x00456CE4 File Offset: 0x00454EE4
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

		// Token: 0x0600BA7F RID: 47743 RVA: 0x00456D92 File Offset: 0x00454F92
		public void AddConsideration(UAIConsiderationBase _c)
		{
			this.considerations.Add(_c);
		}

		// Token: 0x0600BA80 RID: 47744 RVA: 0x00456DA0 File Offset: 0x00454FA0
		public void AddTask(UAITaskBase _t)
		{
			this.tasks.Add(_t);
		}

		// Token: 0x0600BA81 RID: 47745 RVA: 0x00456DAE File Offset: 0x00454FAE
		public List<UAIConsiderationBase> GetConsiderations()
		{
			return this.considerations;
		}

		// Token: 0x0600BA82 RID: 47746 RVA: 0x00456DB6 File Offset: 0x00454FB6
		public List<UAITaskBase> GetTasks()
		{
			return this.tasks;
		}

		// Token: 0x04008BC6 RID: 35782
		[PublicizedFrom(EAccessModifier.Private)]
		public List<UAITaskBase> tasks;

		// Token: 0x04008BC7 RID: 35783
		[PublicizedFrom(EAccessModifier.Private)]
		public List<UAIConsiderationBase> considerations;
	}
}

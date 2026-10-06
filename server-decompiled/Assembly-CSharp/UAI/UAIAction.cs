using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace UAI
{
	// Token: 0x020017AF RID: 6063
	[Preserve]
	public class UAIAction
	{
		// Token: 0x17001740 RID: 5952
		// (get) Token: 0x0600BBD0 RID: 48080 RVA: 0x0045B1B4 File Offset: 0x004593B4
		// (set) Token: 0x0600BBD1 RID: 48081 RVA: 0x0045B1BC File Offset: 0x004593BC
		public string Name
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001741 RID: 5953
		// (get) Token: 0x0600BBD2 RID: 48082 RVA: 0x0045B1C5 File Offset: 0x004593C5
		// (set) Token: 0x0600BBD3 RID: 48083 RVA: 0x0045B1CD File Offset: 0x004593CD
		public float Weight
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x0600BBD4 RID: 48084 RVA: 0x0045B1D6 File Offset: 0x004593D6
		public UAIAction(string _name, float _weight)
		{
			this.Name = _name;
			this.Weight = _weight;
			this.considerations = new List<UAIConsiderationBase>();
			this.tasks = new List<UAITaskBase>();
		}

		// Token: 0x0600BBD5 RID: 48085 RVA: 0x0045B204 File Offset: 0x00459404
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

		// Token: 0x0600BBD6 RID: 48086 RVA: 0x0045B2B2 File Offset: 0x004594B2
		public void AddConsideration(UAIConsiderationBase _c)
		{
			this.considerations.Add(_c);
		}

		// Token: 0x0600BBD7 RID: 48087 RVA: 0x0045B2C0 File Offset: 0x004594C0
		public void AddTask(UAITaskBase _t)
		{
			this.tasks.Add(_t);
		}

		// Token: 0x0600BBD8 RID: 48088 RVA: 0x0045B2CE File Offset: 0x004594CE
		public List<UAIConsiderationBase> GetConsiderations()
		{
			return this.considerations;
		}

		// Token: 0x0600BBD9 RID: 48089 RVA: 0x0045B2D6 File Offset: 0x004594D6
		public List<UAITaskBase> GetTasks()
		{
			return this.tasks;
		}

		// Token: 0x04008C8B RID: 35979
		[PublicizedFrom(EAccessModifier.Private)]
		public List<UAITaskBase> tasks;

		// Token: 0x04008C8C RID: 35980
		[PublicizedFrom(EAccessModifier.Private)]
		public List<UAIConsiderationBase> considerations;
	}
}

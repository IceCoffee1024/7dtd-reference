using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace UAI
{
	// Token: 0x0200175D RID: 5981
	[Preserve]
	public class UAIAction
	{
		// Token: 0x1700162C RID: 5676
		// (get) Token: 0x0600B8F6 RID: 47350 RVA: 0x00450F1C File Offset: 0x0044F11C
		// (set) Token: 0x0600B8F7 RID: 47351 RVA: 0x00450F24 File Offset: 0x0044F124
		public string Name
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x1700162D RID: 5677
		// (get) Token: 0x0600B8F8 RID: 47352 RVA: 0x00450F2D File Offset: 0x0044F12D
		// (set) Token: 0x0600B8F9 RID: 47353 RVA: 0x00450F35 File Offset: 0x0044F135
		public float Weight
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x0600B8FA RID: 47354 RVA: 0x00450F3E File Offset: 0x0044F13E
		public UAIAction(string _name, float _weight)
		{
			this.Name = _name;
			this.Weight = _weight;
			this.considerations = new List<UAIConsiderationBase>();
			this.tasks = new List<UAITaskBase>();
		}

		// Token: 0x0600B8FB RID: 47355 RVA: 0x00450F6C File Offset: 0x0044F16C
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

		// Token: 0x0600B8FC RID: 47356 RVA: 0x0045101A File Offset: 0x0044F21A
		public void AddConsideration(UAIConsiderationBase _c)
		{
			this.considerations.Add(_c);
		}

		// Token: 0x0600B8FD RID: 47357 RVA: 0x00451028 File Offset: 0x0044F228
		public void AddTask(UAITaskBase _t)
		{
			this.tasks.Add(_t);
		}

		// Token: 0x0600B8FE RID: 47358 RVA: 0x00451036 File Offset: 0x0044F236
		public List<UAIConsiderationBase> GetConsiderations()
		{
			return this.considerations;
		}

		// Token: 0x0600B8FF RID: 47359 RVA: 0x0045103E File Offset: 0x0044F23E
		public List<UAITaskBase> GetTasks()
		{
			return this.tasks;
		}

		// Token: 0x04008AAA RID: 35498
		[PublicizedFrom(EAccessModifier.Private)]
		public List<UAITaskBase> tasks;

		// Token: 0x04008AAB RID: 35499
		[PublicizedFrom(EAccessModifier.Private)]
		public List<UAIConsiderationBase> considerations;
	}
}

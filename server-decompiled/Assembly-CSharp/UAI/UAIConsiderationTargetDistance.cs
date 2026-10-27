using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Scripting;

namespace UAI
{
	// Token: 0x020017D5 RID: 6101
	[Preserve]
	public class UAIConsiderationTargetDistance : UAIConsiderationBase
	{
		// Token: 0x0600BCBA RID: 48314 RVA: 0x0045E028 File Offset: 0x0045C228
		public override void Init(Dictionary<string, string> _parameters)
		{
			base.Init(_parameters);
			if (_parameters.ContainsKey("min"))
			{
				this.min = StringParsers.ParseFloat(_parameters["min"], 0, -1, NumberStyles.Any);
				this.min *= this.min;
			}
			if (_parameters.ContainsKey("max"))
			{
				this.max = StringParsers.ParseFloat(_parameters["max"], 0, -1, NumberStyles.Any);
				this.max *= this.max;
			}
		}

		// Token: 0x0600BCBB RID: 48315 RVA: 0x0045E0B8 File Offset: 0x0045C2B8
		public override float GetScore(Context _context, object target)
		{
			EntityAlive entityAlive = UAIUtils.ConvertToEntityAlive(target);
			if (entityAlive != null)
			{
				float num = UAIUtils.DistanceSqr(_context.Self.position, entityAlive.position);
				return Mathf.Clamp01(Mathf.Max(0f, num - this.min) / (this.max - this.min));
			}
			if (target.GetType() == typeof(Vector3))
			{
				Vector3 vector = (Vector3)target;
				float num2 = UAIUtils.DistanceSqr(_context.Self.position, vector);
				return Mathf.Clamp01(Mathf.Max(0f, num2 - this.min) / (this.max - this.min));
			}
			return 0f;
		}

		// Token: 0x04008D63 RID: 36195
		[PublicizedFrom(EAccessModifier.Private)]
		public float min;

		// Token: 0x04008D64 RID: 36196
		[PublicizedFrom(EAccessModifier.Private)]
		public float max = 9126f;
	}
}

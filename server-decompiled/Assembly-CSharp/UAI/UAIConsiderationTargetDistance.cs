using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Scripting;

namespace UAI
{
	// Token: 0x02001778 RID: 6008
	[Preserve]
	public class UAIConsiderationTargetDistance : UAIConsiderationBase
	{
		// Token: 0x0600BA4F RID: 47695 RVA: 0x0045615C File Offset: 0x0045435C
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

		// Token: 0x0600BA50 RID: 47696 RVA: 0x004561EC File Offset: 0x004543EC
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

		// Token: 0x04008BB5 RID: 35765
		[PublicizedFrom(EAccessModifier.Private)]
		public float min;

		// Token: 0x04008BB6 RID: 35766
		[PublicizedFrom(EAccessModifier.Private)]
		public float max = 9126f;
	}
}

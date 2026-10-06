using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Scripting;

namespace UAI
{
	// Token: 0x020017A4 RID: 6052
	[Preserve]
	public class UAIConsiderationTargetDistance : UAIConsiderationBase
	{
		// Token: 0x0600BBA6 RID: 48038 RVA: 0x0045A67C File Offset: 0x0045887C
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

		// Token: 0x0600BBA7 RID: 48039 RVA: 0x0045A70C File Offset: 0x0045890C
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

		// Token: 0x04008C7A RID: 35962
		[PublicizedFrom(EAccessModifier.Private)]
		public float min;

		// Token: 0x04008C7B RID: 35963
		[PublicizedFrom(EAccessModifier.Private)]
		public float max = 9126f;
	}
}

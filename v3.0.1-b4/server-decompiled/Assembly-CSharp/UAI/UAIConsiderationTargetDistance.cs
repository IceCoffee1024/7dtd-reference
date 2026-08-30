using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Scripting;

namespace UAI
{
	// Token: 0x02001752 RID: 5970
	[Preserve]
	public class UAIConsiderationTargetDistance : UAIConsiderationBase
	{
		// Token: 0x0600B8CC RID: 47308 RVA: 0x004503E4 File Offset: 0x0044E5E4
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

		// Token: 0x0600B8CD RID: 47309 RVA: 0x00450474 File Offset: 0x0044E674
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

		// Token: 0x04008A99 RID: 35481
		[PublicizedFrom(EAccessModifier.Private)]
		public float min;

		// Token: 0x04008A9A RID: 35482
		[PublicizedFrom(EAccessModifier.Private)]
		public float max = 9126f;
	}
}

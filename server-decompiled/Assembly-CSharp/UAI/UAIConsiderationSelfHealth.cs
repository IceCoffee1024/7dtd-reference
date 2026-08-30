using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine.Scripting;

namespace UAI
{
	// Token: 0x02001750 RID: 5968
	[Preserve]
	public class UAIConsiderationSelfHealth : UAIConsiderationBase
	{
		// Token: 0x0600B8C7 RID: 47303 RVA: 0x004502A8 File Offset: 0x0044E4A8
		public override void Init(Dictionary<string, string> parameters)
		{
			base.Init(parameters);
			if (parameters.ContainsKey("min"))
			{
				this.min = StringParsers.ParseFloat(parameters["min"], 0, -1, NumberStyles.Any);
			}
			else
			{
				this.min = 0f;
			}
			if (parameters.ContainsKey("max"))
			{
				this.max = StringParsers.ParseFloat(parameters["max"], 0, -1, NumberStyles.Any);
				return;
			}
			this.max = float.NaN;
		}

		// Token: 0x0600B8C8 RID: 47304 RVA: 0x0045032C File Offset: 0x0044E52C
		public override float GetScore(Context _context, object _target)
		{
			if (float.IsNaN(this.max))
			{
				this.max = (float)_context.Self.GetMaxHealth();
			}
			return ((float)_context.Self.Health - this.min) / (this.max - this.min);
		}

		// Token: 0x04008A97 RID: 35479
		[PublicizedFrom(EAccessModifier.Private)]
		public float min;

		// Token: 0x04008A98 RID: 35480
		[PublicizedFrom(EAccessModifier.Private)]
		public float max;
	}
}

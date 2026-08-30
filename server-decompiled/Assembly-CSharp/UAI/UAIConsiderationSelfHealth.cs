using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine.Scripting;

namespace UAI
{
	// Token: 0x02001776 RID: 6006
	[Preserve]
	public class UAIConsiderationSelfHealth : UAIConsiderationBase
	{
		// Token: 0x0600BA4A RID: 47690 RVA: 0x00456020 File Offset: 0x00454220
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

		// Token: 0x0600BA4B RID: 47691 RVA: 0x004560A4 File Offset: 0x004542A4
		public override float GetScore(Context _context, object _target)
		{
			if (float.IsNaN(this.max))
			{
				this.max = (float)_context.Self.GetMaxHealth();
			}
			return ((float)_context.Self.Health - this.min) / (this.max - this.min);
		}

		// Token: 0x04008BB3 RID: 35763
		[PublicizedFrom(EAccessModifier.Private)]
		public float min;

		// Token: 0x04008BB4 RID: 35764
		[PublicizedFrom(EAccessModifier.Private)]
		public float max;
	}
}

using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine.Scripting;

namespace UAI
{
	// Token: 0x020017A2 RID: 6050
	[Preserve]
	public class UAIConsiderationSelfHealth : UAIConsiderationBase
	{
		// Token: 0x0600BBA1 RID: 48033 RVA: 0x0045A540 File Offset: 0x00458740
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

		// Token: 0x0600BBA2 RID: 48034 RVA: 0x0045A5C4 File Offset: 0x004587C4
		public override float GetScore(Context _context, object _target)
		{
			if (float.IsNaN(this.max))
			{
				this.max = (float)_context.Self.GetMaxHealth();
			}
			return ((float)_context.Self.Health - this.min) / (this.max - this.min);
		}

		// Token: 0x04008C78 RID: 35960
		[PublicizedFrom(EAccessModifier.Private)]
		public float min;

		// Token: 0x04008C79 RID: 35961
		[PublicizedFrom(EAccessModifier.Private)]
		public float max;
	}
}

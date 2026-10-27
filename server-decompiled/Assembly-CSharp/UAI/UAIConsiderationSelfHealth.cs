using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine.Scripting;

namespace UAI
{
	// Token: 0x020017D3 RID: 6099
	[Preserve]
	public class UAIConsiderationSelfHealth : UAIConsiderationBase
	{
		// Token: 0x0600BCB5 RID: 48309 RVA: 0x0045DEEC File Offset: 0x0045C0EC
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

		// Token: 0x0600BCB6 RID: 48310 RVA: 0x0045DF70 File Offset: 0x0045C170
		public override float GetScore(Context _context, object _target)
		{
			if (float.IsNaN(this.max))
			{
				this.max = (float)_context.Self.GetMaxHealth();
			}
			return ((float)_context.Self.Health - this.min) / (this.max - this.min);
		}

		// Token: 0x04008D61 RID: 36193
		[PublicizedFrom(EAccessModifier.Private)]
		public float min;

		// Token: 0x04008D62 RID: 36194
		[PublicizedFrom(EAccessModifier.Private)]
		public float max;
	}
}

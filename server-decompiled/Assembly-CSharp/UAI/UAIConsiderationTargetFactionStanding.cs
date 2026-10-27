using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine.Scripting;

namespace UAI
{
	// Token: 0x020017D6 RID: 6102
	[Preserve]
	public class UAIConsiderationTargetFactionStanding : UAIConsiderationBase
	{
		// Token: 0x0600BCBD RID: 48317 RVA: 0x0045E180 File Offset: 0x0045C380
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
			this.max = 255f;
		}

		// Token: 0x0600BCBE RID: 48318 RVA: 0x0045E204 File Offset: 0x0045C404
		public override float GetScore(Context _context, object target)
		{
			if (target is EntityAlive)
			{
				EntityAlive entityAlive = UAIUtils.ConvertToEntityAlive(target);
				return (FactionManager.Instance.GetRelationshipValue(_context.Self, entityAlive) - this.min) / (this.max - this.min);
			}
			return 0f;
		}

		// Token: 0x04008D65 RID: 36197
		[PublicizedFrom(EAccessModifier.Private)]
		public float min;

		// Token: 0x04008D66 RID: 36198
		[PublicizedFrom(EAccessModifier.Private)]
		public float max;
	}
}

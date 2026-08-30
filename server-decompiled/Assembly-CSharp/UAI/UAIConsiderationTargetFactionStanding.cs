using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine.Scripting;

namespace UAI
{
	// Token: 0x02001779 RID: 6009
	[Preserve]
	public class UAIConsiderationTargetFactionStanding : UAIConsiderationBase
	{
		// Token: 0x0600BA52 RID: 47698 RVA: 0x004562B4 File Offset: 0x004544B4
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

		// Token: 0x0600BA53 RID: 47699 RVA: 0x00456338 File Offset: 0x00454538
		public override float GetScore(Context _context, object target)
		{
			if (target is EntityAlive)
			{
				EntityAlive entityAlive = UAIUtils.ConvertToEntityAlive(target);
				return (FactionManager.Instance.GetRelationshipValue(_context.Self, entityAlive) - this.min) / (this.max - this.min);
			}
			return 0f;
		}

		// Token: 0x04008BB7 RID: 35767
		[PublicizedFrom(EAccessModifier.Private)]
		public float min;

		// Token: 0x04008BB8 RID: 35768
		[PublicizedFrom(EAccessModifier.Private)]
		public float max;
	}
}

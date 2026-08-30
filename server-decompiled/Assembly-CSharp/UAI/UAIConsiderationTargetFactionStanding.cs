using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine.Scripting;

namespace UAI
{
	// Token: 0x02001753 RID: 5971
	[Preserve]
	public class UAIConsiderationTargetFactionStanding : UAIConsiderationBase
	{
		// Token: 0x0600B8CF RID: 47311 RVA: 0x0045053C File Offset: 0x0044E73C
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

		// Token: 0x0600B8D0 RID: 47312 RVA: 0x004505C0 File Offset: 0x0044E7C0
		public override float GetScore(Context _context, object target)
		{
			if (target is EntityAlive)
			{
				EntityAlive entityAlive = UAIUtils.ConvertToEntityAlive(target);
				return (FactionManager.Instance.GetRelationshipValue(_context.Self, entityAlive) - this.min) / (this.max - this.min);
			}
			return 0f;
		}

		// Token: 0x04008A9B RID: 35483
		[PublicizedFrom(EAccessModifier.Private)]
		public float min;

		// Token: 0x04008A9C RID: 35484
		[PublicizedFrom(EAccessModifier.Private)]
		public float max;
	}
}

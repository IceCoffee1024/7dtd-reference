using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine.Scripting;

namespace UAI
{
	// Token: 0x020017A5 RID: 6053
	[Preserve]
	public class UAIConsiderationTargetFactionStanding : UAIConsiderationBase
	{
		// Token: 0x0600BBA9 RID: 48041 RVA: 0x0045A7D4 File Offset: 0x004589D4
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

		// Token: 0x0600BBAA RID: 48042 RVA: 0x0045A858 File Offset: 0x00458A58
		public override float GetScore(Context _context, object target)
		{
			if (target is EntityAlive)
			{
				EntityAlive entityAlive = UAIUtils.ConvertToEntityAlive(target);
				return (FactionManager.Instance.GetRelationshipValue(_context.Self, entityAlive) - this.min) / (this.max - this.min);
			}
			return 0f;
		}

		// Token: 0x04008C7C RID: 35964
		[PublicizedFrom(EAccessModifier.Private)]
		public float min;

		// Token: 0x04008C7D RID: 35965
		[PublicizedFrom(EAccessModifier.Private)]
		public float max;
	}
}

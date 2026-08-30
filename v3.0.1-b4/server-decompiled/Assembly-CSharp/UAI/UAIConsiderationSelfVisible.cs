using System;
using UnityEngine.Scripting;

namespace UAI
{
	// Token: 0x02001751 RID: 5969
	[Preserve]
	public class UAIConsiderationSelfVisible : UAIConsiderationBase
	{
		// Token: 0x0600B8CA RID: 47306 RVA: 0x0045037C File Offset: 0x0044E57C
		public override float GetScore(Context _context, object target)
		{
			EntityAlive entityAlive = UAIUtils.ConvertToEntityAlive(target);
			if (entityAlive != null)
			{
				float num = _context.Self.GetSeeDistance();
				num *= num;
				float num2 = 1f - UAIUtils.DistanceSqr(_context.Self.getHeadPosition(), entityAlive.getHeadPosition()) / num;
				return (float)(entityAlive.CanEntityBeSeen(_context.Self, true) ? 1 : 0) * num2;
			}
			return 0f;
		}
	}
}

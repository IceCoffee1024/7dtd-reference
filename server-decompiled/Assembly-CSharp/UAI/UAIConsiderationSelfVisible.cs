using System;
using UnityEngine.Scripting;

namespace UAI
{
	// Token: 0x020017D4 RID: 6100
	[Preserve]
	public class UAIConsiderationSelfVisible : UAIConsiderationBase
	{
		// Token: 0x0600BCB8 RID: 48312 RVA: 0x0045DFC0 File Offset: 0x0045C1C0
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

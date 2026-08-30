using System;
using System.Globalization;
using UnityEngine.Scripting;

namespace UAI
{
	// Token: 0x0200175A RID: 5978
	[Preserve]
	public class UAITaskFleeFromTarget : UAITaskBase
	{
		// Token: 0x0600B8EA RID: 47338 RVA: 0x00450BC0 File Offset: 0x0044EDC0
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void initializeParameters()
		{
			base.initializeParameters();
			if (this.Parameters.ContainsKey("max_distance"))
			{
				this.maxFleeDistance = StringParsers.ParseFloat(this.Parameters["max_distance"], 0, -1, NumberStyles.Any);
			}
		}

		// Token: 0x0600B8EB RID: 47339 RVA: 0x00450BFC File Offset: 0x0044EDFC
		public override void Start(Context _context)
		{
			base.Start(_context);
			EntityAlive entityAlive = UAIUtils.ConvertToEntityAlive(_context.ActionData.Target);
			if (entityAlive != null)
			{
				_context.Self.detachHome();
				_context.Self.FindPath(RandomPositionGenerator.CalcAway(_context.Self, 0, (int)this.maxFleeDistance, (int)this.maxFleeDistance, entityAlive.position), _context.Self.GetMoveSpeedPanic(), false, null);
				return;
			}
			_context.ActionData.Failed = true;
		}

		// Token: 0x0600B8EC RID: 47340 RVA: 0x00450C7A File Offset: 0x0044EE7A
		public override void Update(Context _context)
		{
			base.Update(_context);
			if (_context.Self.getNavigator().noPathAndNotPlanningOne())
			{
				_context.Self.setHomeArea(new Vector3i(_context.Self.position), 10);
				this.Stop(_context);
			}
		}

		// Token: 0x04008AA3 RID: 35491
		[PublicizedFrom(EAccessModifier.Private)]
		public float maxFleeDistance;
	}
}

using System;
using System.Globalization;
using UnityEngine.Scripting;

namespace UAI
{
	// Token: 0x020017AC RID: 6060
	[Preserve]
	public class UAITaskFleeFromTarget : UAITaskBase
	{
		// Token: 0x0600BBC4 RID: 48068 RVA: 0x0045AE58 File Offset: 0x00459058
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void initializeParameters()
		{
			base.initializeParameters();
			if (this.Parameters.ContainsKey("max_distance"))
			{
				this.maxFleeDistance = StringParsers.ParseFloat(this.Parameters["max_distance"], 0, -1, NumberStyles.Any);
			}
		}

		// Token: 0x0600BBC5 RID: 48069 RVA: 0x0045AE94 File Offset: 0x00459094
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

		// Token: 0x0600BBC6 RID: 48070 RVA: 0x0045AF12 File Offset: 0x00459112
		public override void Update(Context _context)
		{
			base.Update(_context);
			if (_context.Self.getNavigator().noPathAndNotPlanningOne())
			{
				_context.Self.setHomeArea(new Vector3i(_context.Self.position), 10);
				this.Stop(_context);
			}
		}

		// Token: 0x04008C84 RID: 35972
		[PublicizedFrom(EAccessModifier.Private)]
		public float maxFleeDistance;
	}
}

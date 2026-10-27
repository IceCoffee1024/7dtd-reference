using System;
using System.Globalization;
using UnityEngine.Scripting;

namespace UAI
{
	// Token: 0x020017DD RID: 6109
	[Preserve]
	public class UAITaskFleeFromTarget : UAITaskBase
	{
		// Token: 0x0600BCD8 RID: 48344 RVA: 0x0045E804 File Offset: 0x0045CA04
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void initializeParameters()
		{
			base.initializeParameters();
			if (this.Parameters.ContainsKey("max_distance"))
			{
				this.maxFleeDistance = StringParsers.ParseFloat(this.Parameters["max_distance"], 0, -1, NumberStyles.Any);
			}
		}

		// Token: 0x0600BCD9 RID: 48345 RVA: 0x0045E840 File Offset: 0x0045CA40
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

		// Token: 0x0600BCDA RID: 48346 RVA: 0x0045E8BE File Offset: 0x0045CABE
		public override void Update(Context _context)
		{
			base.Update(_context);
			if (_context.Self.getNavigator().noPathAndNotPlanningOne())
			{
				_context.Self.setHomeArea(new Vector3i(_context.Self.position), 10);
				this.Stop(_context);
			}
		}

		// Token: 0x04008D6D RID: 36205
		[PublicizedFrom(EAccessModifier.Private)]
		public float maxFleeDistance;
	}
}

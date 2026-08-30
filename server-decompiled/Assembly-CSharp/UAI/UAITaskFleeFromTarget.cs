using System;
using System.Globalization;
using UnityEngine.Scripting;

namespace UAI
{
	// Token: 0x02001780 RID: 6016
	[Preserve]
	public class UAITaskFleeFromTarget : UAITaskBase
	{
		// Token: 0x0600BA6D RID: 47725 RVA: 0x00456938 File Offset: 0x00454B38
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void initializeParameters()
		{
			base.initializeParameters();
			if (this.Parameters.ContainsKey("max_distance"))
			{
				this.maxFleeDistance = StringParsers.ParseFloat(this.Parameters["max_distance"], 0, -1, NumberStyles.Any);
			}
		}

		// Token: 0x0600BA6E RID: 47726 RVA: 0x00456974 File Offset: 0x00454B74
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

		// Token: 0x0600BA6F RID: 47727 RVA: 0x004569F2 File Offset: 0x00454BF2
		public override void Update(Context _context)
		{
			base.Update(_context);
			if (_context.Self.getNavigator().noPathAndNotPlanningOne())
			{
				_context.Self.setHomeArea(new Vector3i(_context.Self.position), 10);
				this.Stop(_context);
			}
		}

		// Token: 0x04008BBF RID: 35775
		[PublicizedFrom(EAccessModifier.Private)]
		public float maxFleeDistance;
	}
}

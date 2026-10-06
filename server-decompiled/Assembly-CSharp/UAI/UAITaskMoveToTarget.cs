using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.Scripting;

namespace UAI
{
	// Token: 0x020017AD RID: 6061
	[Preserve]
	public class UAITaskMoveToTarget : UAITaskBase
	{
		// Token: 0x0600BBC8 RID: 48072 RVA: 0x0045AF54 File Offset: 0x00459154
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void initializeParameters()
		{
			base.initializeParameters();
			if (this.Parameters.ContainsKey("distance"))
			{
				this.distance = StringParsers.ParseFloat(this.Parameters["distance"], 0, -1, NumberStyles.Any);
			}
			if (this.Parameters.ContainsKey("run"))
			{
				this.run = StringParsers.ParseBool(this.Parameters["run"], 0, -1, true);
			}
			if (this.Parameters.ContainsKey("break_walls"))
			{
				this.shouldBreakWalls = StringParsers.ParseBool(this.Parameters["break_walls"], 0, -1, true);
			}
		}

		// Token: 0x0600BBC9 RID: 48073 RVA: 0x0045AFFC File Offset: 0x004591FC
		public override void Start(Context _context)
		{
			base.Start(_context);
			EntityAlive entityAlive = UAIUtils.ConvertToEntityAlive(_context.ActionData.Target);
			if (entityAlive != null)
			{
				_context.Self.FindPath(RandomPositionGenerator.CalcNear(_context.Self, entityAlive.position, (int)this.distance, (int)this.distance), this.run ? _context.Self.GetMoveSpeedPanic() : (_context.Self.IsAlert ? _context.Self.GetMoveSpeedAggro() : _context.Self.GetMoveSpeed()), this.shouldBreakWalls, null);
				return;
			}
			if (_context.ActionData.Target.GetType() == typeof(Vector3))
			{
				_context.Self.FindPath(RandomPositionGenerator.CalcNear(_context.Self, (Vector3)_context.ActionData.Target, (int)this.distance, (int)this.distance), this.run ? _context.Self.GetMoveSpeedPanic() : _context.Self.GetMoveSpeed(), this.shouldBreakWalls, null);
				return;
			}
			this.Stop(_context);
		}

		// Token: 0x0600BBCA RID: 48074 RVA: 0x0045B11A File Offset: 0x0045931A
		public override void Update(Context _context)
		{
			base.Update(_context);
			if (_context.Self.getNavigator().noPathAndNotPlanningOne())
			{
				this.Stop(_context);
			}
		}

		// Token: 0x04008C85 RID: 35973
		public float distance;

		// Token: 0x04008C86 RID: 35974
		public bool run;

		// Token: 0x04008C87 RID: 35975
		public bool shouldBreakWalls;
	}
}

using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.Scripting;

namespace UAI
{
	// Token: 0x0200175B RID: 5979
	[Preserve]
	public class UAITaskMoveToTarget : UAITaskBase
	{
		// Token: 0x0600B8EE RID: 47342 RVA: 0x00450CBC File Offset: 0x0044EEBC
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

		// Token: 0x0600B8EF RID: 47343 RVA: 0x00450D64 File Offset: 0x0044EF64
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

		// Token: 0x0600B8F0 RID: 47344 RVA: 0x00450E82 File Offset: 0x0044F082
		public override void Update(Context _context)
		{
			base.Update(_context);
			if (_context.Self.getNavigator().noPathAndNotPlanningOne())
			{
				this.Stop(_context);
			}
		}

		// Token: 0x04008AA4 RID: 35492
		public float distance;

		// Token: 0x04008AA5 RID: 35493
		public bool run;

		// Token: 0x04008AA6 RID: 35494
		public bool shouldBreakWalls;
	}
}

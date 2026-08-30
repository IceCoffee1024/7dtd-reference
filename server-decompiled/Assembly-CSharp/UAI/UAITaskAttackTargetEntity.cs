using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace UAI
{
	// Token: 0x0200177E RID: 6014
	[Preserve]
	public class UAITaskAttackTargetEntity : UAITaskBase
	{
		// Token: 0x0600BA60 RID: 47712 RVA: 0x0045655F File Offset: 0x0045475F
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void initializeParameters()
		{
			base.initializeParameters();
		}

		// Token: 0x0600BA61 RID: 47713 RVA: 0x00456714 File Offset: 0x00454914
		public override void Start(Context _context)
		{
			base.Start(_context);
			EntityAlive entityAlive = UAIUtils.ConvertToEntityAlive(_context.ActionData.Target);
			if (entityAlive != null)
			{
				_context.Self.SetLookPosition(_context.Self.CanSee(entityAlive) ? entityAlive.getHeadPosition() : Vector3.zero);
				if (_context.Self.bodyDamage.HasLimbs)
				{
					_context.Self.RotateTo(entityAlive.position.x, entityAlive.position.y, entityAlive.position.z, 30f, 30f);
				}
				this.attackTimeout = _context.Self.GetAttackTimeoutTicks();
				return;
			}
			this.Stop(_context);
		}

		// Token: 0x0600BA62 RID: 47714 RVA: 0x004567CC File Offset: 0x004549CC
		public override void Update(Context _context)
		{
			base.Update(_context);
			EntityAlive entityAlive = UAIUtils.ConvertToEntityAlive(_context.ActionData.Target);
			if (entityAlive != null)
			{
				_context.Self.SetLookPosition(_context.Self.CanSee(entityAlive) ? entityAlive.getHeadPosition() : Vector3.zero);
				if (_context.Self.bodyDamage.HasLimbs)
				{
					_context.Self.RotateTo(entityAlive, 30f, 30f);
				}
				this.attackTimeout = Utils.FastMax(this.attackTimeout - 1, 0);
				if (this.attackTimeout > 0)
				{
					return;
				}
				if (_context.Self.Attack(false))
				{
					this.attackTimeout = _context.Self.GetAttackTimeoutTicks();
					_context.Self.Attack(true);
					this.Stop(_context);
					return;
				}
			}
			else
			{
				this.Stop(_context);
			}
		}

		// Token: 0x04008BBB RID: 35771
		public int attackTimeout;
	}
}

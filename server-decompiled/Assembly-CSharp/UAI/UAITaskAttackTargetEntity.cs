using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace UAI
{
	// Token: 0x020017DB RID: 6107
	[Preserve]
	public class UAITaskAttackTargetEntity : UAITaskBase
	{
		// Token: 0x0600BCCB RID: 48331 RVA: 0x0045E42B File Offset: 0x0045C62B
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void initializeParameters()
		{
			base.initializeParameters();
		}

		// Token: 0x0600BCCC RID: 48332 RVA: 0x0045E5E0 File Offset: 0x0045C7E0
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

		// Token: 0x0600BCCD RID: 48333 RVA: 0x0045E698 File Offset: 0x0045C898
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

		// Token: 0x04008D69 RID: 36201
		public int attackTimeout;
	}
}

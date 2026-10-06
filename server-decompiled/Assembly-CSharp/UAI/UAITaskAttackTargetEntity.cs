using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace UAI
{
	// Token: 0x020017AA RID: 6058
	[Preserve]
	public class UAITaskAttackTargetEntity : UAITaskBase
	{
		// Token: 0x0600BBB7 RID: 48055 RVA: 0x0045AA7F File Offset: 0x00458C7F
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void initializeParameters()
		{
			base.initializeParameters();
		}

		// Token: 0x0600BBB8 RID: 48056 RVA: 0x0045AC34 File Offset: 0x00458E34
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

		// Token: 0x0600BBB9 RID: 48057 RVA: 0x0045ACEC File Offset: 0x00458EEC
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

		// Token: 0x04008C80 RID: 35968
		public int attackTimeout;
	}
}

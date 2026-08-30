using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace UAI
{
	// Token: 0x02001758 RID: 5976
	[Preserve]
	public class UAITaskAttackTargetEntity : UAITaskBase
	{
		// Token: 0x0600B8DD RID: 47325 RVA: 0x004507E7 File Offset: 0x0044E9E7
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void initializeParameters()
		{
			base.initializeParameters();
		}

		// Token: 0x0600B8DE RID: 47326 RVA: 0x0045099C File Offset: 0x0044EB9C
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

		// Token: 0x0600B8DF RID: 47327 RVA: 0x00450A54 File Offset: 0x0044EC54
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

		// Token: 0x04008A9F RID: 35487
		public int attackTimeout;
	}
}

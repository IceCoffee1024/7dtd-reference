using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace UAI
{
	// Token: 0x020017A9 RID: 6057
	[Preserve]
	public class UAITaskAttackTargetBlock : UAITaskBase
	{
		// Token: 0x0600BBB3 RID: 48051 RVA: 0x0045AA7F File Offset: 0x00458C7F
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void initializeParameters()
		{
			base.initializeParameters();
		}

		// Token: 0x0600BBB4 RID: 48052 RVA: 0x0045AA88 File Offset: 0x00458C88
		public override void Start(Context _context)
		{
			base.Start(_context);
			if (_context.ActionData.Target.GetType() == typeof(Vector3))
			{
				this.attackTimeout = _context.Self.GetAttackTimeoutTicks();
				Vector3 vector = (Vector3)_context.ActionData.Target;
				_context.Self.SetLookPosition(_context.Self.CanSee(vector) ? vector : Vector3.zero);
				if (_context.Self.bodyDamage.HasLimbs)
				{
					_context.Self.RotateTo(vector.x, vector.y, vector.z, 30f, 30f);
					return;
				}
			}
			else
			{
				this.Stop(_context);
			}
		}

		// Token: 0x0600BBB5 RID: 48053 RVA: 0x0045AB44 File Offset: 0x00458D44
		public override void Update(Context _context)
		{
			base.Update(_context);
			if (_context.ActionData.Target.GetType() == typeof(Vector3))
			{
				Vector3 vector = (Vector3)_context.ActionData.Target;
				this.attackTimeout = Utils.FastMax(this.attackTimeout - 1, 0);
				if (this.attackTimeout > 0)
				{
					return;
				}
				_context.Self.SetLookPosition(vector);
				if (_context.Self.bodyDamage.HasLimbs)
				{
					_context.Self.RotateTo(vector.x, vector.y, vector.z, 30f, 30f);
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

		// Token: 0x04008C7F RID: 35967
		public int attackTimeout;
	}
}

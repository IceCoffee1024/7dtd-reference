using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace UAI
{
	// Token: 0x020017DA RID: 6106
	[Preserve]
	public class UAITaskAttackTargetBlock : UAITaskBase
	{
		// Token: 0x0600BCC7 RID: 48327 RVA: 0x0045E42B File Offset: 0x0045C62B
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void initializeParameters()
		{
			base.initializeParameters();
		}

		// Token: 0x0600BCC8 RID: 48328 RVA: 0x0045E434 File Offset: 0x0045C634
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

		// Token: 0x0600BCC9 RID: 48329 RVA: 0x0045E4F0 File Offset: 0x0045C6F0
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

		// Token: 0x04008D68 RID: 36200
		public int attackTimeout;
	}
}

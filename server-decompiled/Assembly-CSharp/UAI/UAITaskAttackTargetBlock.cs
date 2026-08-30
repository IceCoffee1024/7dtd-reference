using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace UAI
{
	// Token: 0x0200177D RID: 6013
	[Preserve]
	public class UAITaskAttackTargetBlock : UAITaskBase
	{
		// Token: 0x0600BA5C RID: 47708 RVA: 0x0045655F File Offset: 0x0045475F
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void initializeParameters()
		{
			base.initializeParameters();
		}

		// Token: 0x0600BA5D RID: 47709 RVA: 0x00456568 File Offset: 0x00454768
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

		// Token: 0x0600BA5E RID: 47710 RVA: 0x00456624 File Offset: 0x00454824
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

		// Token: 0x04008BBA RID: 35770
		public int attackTimeout;
	}
}

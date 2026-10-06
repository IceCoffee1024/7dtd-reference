using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace UAI
{
	// Token: 0x020017A1 RID: 6049
	[Preserve]
	public class UAIConsiderationPathBlocked : UAIConsiderationBase
	{
		// Token: 0x0600BB99 RID: 48025 RVA: 0x0045A138 File Offset: 0x00458338
		public override float GetScore(Context _context, object target)
		{
			Vector3i zero = Vector3i.zero;
			if (this.IsPathUsageBlocked(_context) && UAIConsiderationPathBlocked.CanAttackBlocks(_context.Self, out zero))
			{
				_context.ConsiderationData.WaypointTargets.Add(zero.ToVector3());
				return 1f;
			}
			return 0f;
		}

		// Token: 0x0600BB9A RID: 48026 RVA: 0x0045A188 File Offset: 0x00458388
		public bool IsPathUsageBlocked(Context _context)
		{
			EntityAlive self = _context.Self;
			if (self.getNavigator() == null)
			{
				return false;
			}
			if (self.getNavigator().getPath() == null)
			{
				return false;
			}
			Vector3 targetPos = UAIConsiderationPathBlocked.GetTargetPos(self);
			Vector3i vector3i = World.worldToBlockPos(targetPos);
			float distanceSq = self.getNavigator().getPath().GetEndPoint()
				.GetDistanceSq(vector3i.x, vector3i.y, vector3i.z);
			if (distanceSq < 2.1f)
			{
				return false;
			}
			float distanceSq2 = self.GetDistanceSq(targetPos);
			return self.GetDistanceSq(targetPos) < 256f || distanceSq > distanceSq2;
		}

		// Token: 0x0600BB9B RID: 48027 RVA: 0x0045A214 File Offset: 0x00458414
		public static bool CanAttackBlocks(EntityAlive theEntity, out Vector3i attackPos)
		{
			float num;
			BlockValue blockValue;
			return UAIConsiderationPathBlocked.CanAttackBlocks(theEntity, UAIConsiderationPathBlocked.GetTargetYaw(theEntity), out num, out attackPos, out blockValue);
		}

		// Token: 0x0600BB9C RID: 48028 RVA: 0x0045A234 File Offset: 0x00458434
		public static bool CanAttackBlocks(EntityAlive theEntity, float yawAngle, out float attackAngle, out Vector3i attackAddPos, out BlockValue attackBlockValue)
		{
			int num = Utils.Fastfloor(theEntity.position.x);
			int num2 = Utils.Fastfloor(theEntity.position.y + 0.5f);
			int num3 = Utils.Fastfloor(theEntity.position.z);
			attackAddPos = new Vector3i(0, 1, 0);
			bool flag = UAIConsiderationPathBlocked.isPosBlocked(theEntity, num, num2 + 1, num3, out attackBlockValue);
			attackAngle = 0f;
			if (!flag)
			{
				attackAddPos = new Vector3i(0, 0, 0);
				flag = UAIConsiderationPathBlocked.isPosBlocked(theEntity, num, num2, num3, out attackBlockValue);
				attackAngle = -65f;
			}
			if (!flag)
			{
				int num4 = 0;
				int num5 = 0;
				float num6 = -Mathf.Sin(yawAngle * 0.0175f - 3.1415927f);
				float num7 = -Mathf.Cos(yawAngle * 0.0175f - 3.1415927f);
				if (Mathf.Abs(num6) > 0.1f)
				{
					num4 = (int)Mathf.Sign(num6);
				}
				if (Mathf.Abs(num7) > 0.1f)
				{
					num5 = (int)Mathf.Sign(num7);
				}
				attackAddPos = new Vector3i(num4, 1, num5);
				flag = UAIConsiderationPathBlocked.isPosBlocked(theEntity, num + num4, num2 + 1, num3 + num5, out attackBlockValue);
				attackAngle = 0f;
				if (!flag)
				{
					attackAddPos = new Vector3i(num4, 0, num5);
					flag = UAIConsiderationPathBlocked.isPosBlocked(theEntity, num + num4, num2, num3 + num5, out attackBlockValue);
					attackAngle = -45f;
				}
				if (!flag)
				{
					attackAddPos = new Vector3i(2 * num4, 1, 2 * num5);
					flag = UAIConsiderationPathBlocked.isPosBlocked(theEntity, num + 2 * num4, num2 + 1, num3 + 2 * num5, out attackBlockValue);
					attackAngle = 0f;
				}
				if (!flag)
				{
					attackAddPos = new Vector3i(2 * num4, 0, 2 * num5);
					flag = UAIConsiderationPathBlocked.isPosBlocked(theEntity, num + 2 * num4, num2, num3 + 2 * num5, out attackBlockValue);
					attackAngle = -45f;
				}
				if (!flag)
				{
					attackAddPos = new Vector3i(num4, 0, 0);
					flag = UAIConsiderationPathBlocked.isPosBlocked(theEntity, num + num4, num2, num3, out attackBlockValue);
					attackAngle = -45f;
				}
				if (!flag)
				{
					attackAddPos = new Vector3i(2 * num4, 1, 0);
					flag = UAIConsiderationPathBlocked.isPosBlocked(theEntity, num + 2 * num4, num2 + 1, num3, out attackBlockValue);
					attackAngle = 0f;
				}
				if (!flag)
				{
					attackAddPos = new Vector3i(0, 0, num5);
					flag = UAIConsiderationPathBlocked.isPosBlocked(theEntity, num, num2, num3 + num5, out attackBlockValue);
					attackAngle = -45f;
				}
				if (!flag)
				{
					attackAddPos = new Vector3i(0, 1, 2 * num5);
					flag = UAIConsiderationPathBlocked.isPosBlocked(theEntity, num, num2 + 1, num3 + 2 * num5, out attackBlockValue);
					attackAngle = 0f;
				}
			}
			attackAddPos += new Vector3i(num, num2, num3);
			return flag;
		}

		// Token: 0x0600BB9D RID: 48029 RVA: 0x0045A4AE File Offset: 0x004586AE
		[PublicizedFrom(EAccessModifier.Private)]
		public static bool isPosBlocked(EntityAlive theEntity, int _x, int _y, int _z, out BlockValue attackBlockValue)
		{
			attackBlockValue = theEntity.world.GetBlock(_x, _y, _z);
			return attackBlockValue.Block.IsMovementBlocked(theEntity.world, new Vector3i(_x, _y, _z), attackBlockValue, BlockFace.None);
		}

		// Token: 0x0600BB9E RID: 48030 RVA: 0x0045A4EB File Offset: 0x004586EB
		[PublicizedFrom(EAccessModifier.Private)]
		public static float GetTargetYaw(EntityAlive theEntity)
		{
			if (theEntity.GetAttackTarget() != null)
			{
				return theEntity.YawForTarget(theEntity.GetAttackTarget());
			}
			return theEntity.YawForTarget(UAIConsiderationPathBlocked.GetTargetPos(theEntity));
		}

		// Token: 0x0600BB9F RID: 48031 RVA: 0x0045A514 File Offset: 0x00458714
		[PublicizedFrom(EAccessModifier.Protected)]
		public static Vector3 GetTargetPos(EntityAlive theEntity)
		{
			if (theEntity.GetAttackTarget() != null)
			{
				return theEntity.GetAttackTarget().GetPosition();
			}
			return theEntity.InvestigatePosition;
		}
	}
}

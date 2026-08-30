using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace RaycastPathing
{
	// Token: 0x020018AB RID: 6315
	[Preserve]
	public class RaycastPathUtils
	{
		// Token: 0x0600C278 RID: 49784 RVA: 0x0047E954 File Offset: 0x0047CB54
		public static bool IsPositionBlocked(Vector3 start, Vector3 end, out RaycastHit hit, int layerMask = 0, bool debugDraw = false)
		{
			Vector3 vector = end - start;
			return RaycastPathUtils.IsPositionBlocked(new Ray(start - Origin.position, vector), out hit, layerMask, debugDraw, vector.magnitude + 1f);
		}

		// Token: 0x0600C279 RID: 49785 RVA: 0x0047E990 File Offset: 0x0047CB90
		public static bool IsPositionBlocked(Vector3 start, Vector3 end, int layerMask = 0, bool debugDraw = false)
		{
			RaycastHit raycastHit;
			return RaycastPathUtils.IsPositionBlocked(start, end, out raycastHit, layerMask, debugDraw);
		}

		// Token: 0x0600C27A RID: 49786 RVA: 0x0047E9A8 File Offset: 0x0047CBA8
		public static bool IsPointBlocked(Vector3 start, Vector3 end, int layerMask = 0, bool debugDraw = false, float duration = 0f)
		{
			RaycastHit raycastHit;
			return RaycastPathUtils.CheckPositionBlocked(start, end, out raycastHit, layerMask, debugDraw, duration);
		}

		// Token: 0x0600C27B RID: 49787 RVA: 0x0047E9C4 File Offset: 0x0047CBC4
		public static bool CheckPositionBlocked(Vector3 start, Vector3 end, out RaycastHit hit, int layerMask = 0, bool debugDraw = false, float duration = 0f)
		{
			Vector3 vector = end - start;
			return RaycastPathUtils.CheckPositionBlocked(new Ray(start - Origin.position, vector), out hit, layerMask, debugDraw, vector.magnitude, duration);
		}

		// Token: 0x0600C27C RID: 49788 RVA: 0x0047E9FC File Offset: 0x0047CBFC
		public static bool CheckPositionBlocked(Ray ray, out RaycastHit hit, int layerMask = 0, bool debugDraw = false, float maxDist = 100f, float duration = 0f)
		{
			bool flag = Physics.Raycast(ray, out hit, maxDist, layerMask);
			if (debugDraw)
			{
				if (flag)
				{
					Utils.DrawLine(ray.origin, ray.origin + ray.direction * hit.distance, Color.magenta, Color.red, 1, duration);
				}
				else
				{
					Utils.DrawLine(ray.origin, ray.origin + ray.direction * maxDist, Color.cyan, Color.blue, 1, duration);
				}
			}
			return flag;
		}

		// Token: 0x0600C27D RID: 49789 RVA: 0x0047EA88 File Offset: 0x0047CC88
		public static bool IsPositionBlocked(Ray ray, out RaycastHit hit, int layerMask = 0, bool debugDraw = false, float maxDist = 100f)
		{
			bool flag = Physics.Raycast(ray, out hit, maxDist, layerMask);
			if (debugDraw)
			{
				if (flag)
				{
					Utils.DrawLine(ray.origin, ray.origin + ray.direction * hit.distance, Color.magenta, Color.red, 1, 5f);
				}
				else
				{
					Utils.DrawLine(ray.origin, ray.origin + ray.direction * maxDist, Color.cyan, Color.blue, 1, 5f);
				}
			}
			return flag;
		}

		// Token: 0x0600C27E RID: 49790 RVA: 0x0047EB1C File Offset: 0x0047CD1C
		public static bool IsPositionBlocked(Ray ray, int layerMask = 0, bool debugDraw = false, float maxDist = 100f)
		{
			RaycastHit raycastHit;
			return RaycastPathUtils.IsPositionBlocked(ray, out raycastHit, layerMask, debugDraw, 100f);
		}

		// Token: 0x0600C27F RID: 49791 RVA: 0x0047EB38 File Offset: 0x0047CD38
		public static void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 1f)
		{
			Utils.DrawLine(start - Origin.position, end - Origin.position, color, color, 10, duration);
		}

		// Token: 0x0600C280 RID: 49792 RVA: 0x0047EB5C File Offset: 0x0047CD5C
		public static void DrawBounds(Vector3i pos, Color color, float duration, float scale = 1f)
		{
			Utils.DrawBoxLines(new Vector3((float)pos.x, (float)pos.y, (float)pos.z) - Origin.position, new Vector3((float)pos.x + scale, (float)pos.y + scale, (float)pos.z + scale) - Origin.position, color, duration);
		}

		// Token: 0x0600C281 RID: 49793 RVA: 0x0047EBC0 File Offset: 0x0047CDC0
		public static void DrawBounds(Vector3 pos, Color color, float duration, float scale = 1f)
		{
			Utils.DrawBoxLines(new Vector3i(pos.x, pos.y, pos.z) - Origin.position, new Vector3i(pos.x + scale, pos.y + scale, pos.z + scale) - Origin.position, color, duration);
		}

		// Token: 0x0600C282 RID: 49794 RVA: 0x0047EC26 File Offset: 0x0047CE26
		public static void DrawNode(RaycastNode node, Color color, float duration)
		{
			RaycastPathUtils.DrawNode(node.Min, node.Max, color, duration);
		}

		// Token: 0x0600C283 RID: 49795 RVA: 0x0047EC3B File Offset: 0x0047CE3B
		public static void DrawNode(Vector3 min, Vector3 max, Color color, float duration)
		{
			RaycastPathUtils.DrawVolume(min - Origin.position, max - Origin.position, color, duration);
		}

		// Token: 0x0600C284 RID: 49796 RVA: 0x0047EC5C File Offset: 0x0047CE5C
		[PublicizedFrom(EAccessModifier.Private)]
		public static void DrawVolume(Vector3 min, Vector3 max, Color color, float duration)
		{
			Vector3 vector = new Vector3(max.x, min.y, min.z);
			Vector3 vector2 = new Vector3(min.x, max.y, min.z);
			Vector3 vector3 = new Vector3(min.x, min.y, max.z);
			Vector3 vector4 = new Vector3(min.x, max.y, max.z);
			Vector3 vector5 = new Vector3(max.x, min.y, max.z);
			Vector3 vector6 = new Vector3(max.x, max.y, min.z);
			Debug.DrawLine(min, vector, color, duration);
			Debug.DrawLine(min, vector2, color, duration);
			Debug.DrawLine(min, vector3, color, duration);
			Debug.DrawLine(max, vector4, color, duration);
			Debug.DrawLine(max, vector5, color, duration);
			Debug.DrawLine(max, vector6, color, duration);
			Debug.DrawLine(vector4, vector2, color, duration);
			Debug.DrawLine(vector, vector5, color, duration);
			Debug.DrawLine(vector5, vector3, color, duration);
			Debug.DrawLine(vector2, vector6, color, duration);
			Debug.DrawLine(vector4, vector3, color, duration);
			Debug.DrawLine(vector, vector6, color, duration);
		}

		// Token: 0x04009318 RID: 37656
		[PublicizedFrom(EAccessModifier.Private)]
		public const int cMaxRayDist = 100;
	}
}

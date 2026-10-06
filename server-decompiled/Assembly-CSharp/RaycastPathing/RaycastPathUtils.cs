using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace RaycastPathing
{
	// Token: 0x020018FD RID: 6397
	[Preserve]
	public class RaycastPathUtils
	{
		// Token: 0x0600C557 RID: 50519 RVA: 0x00488FC8 File Offset: 0x004871C8
		public static bool IsPositionBlocked(Vector3 start, Vector3 end, out RaycastHit hit, int layerMask = 0, bool debugDraw = false)
		{
			Vector3 vector = end - start;
			return RaycastPathUtils.IsPositionBlocked(new Ray(start - Origin.position, vector), out hit, layerMask, debugDraw, vector.magnitude + 1f);
		}

		// Token: 0x0600C558 RID: 50520 RVA: 0x00489004 File Offset: 0x00487204
		public static bool IsPositionBlocked(Vector3 start, Vector3 end, int layerMask = 0, bool debugDraw = false)
		{
			RaycastHit raycastHit;
			return RaycastPathUtils.IsPositionBlocked(start, end, out raycastHit, layerMask, debugDraw);
		}

		// Token: 0x0600C559 RID: 50521 RVA: 0x0048901C File Offset: 0x0048721C
		public static bool IsPointBlocked(Vector3 start, Vector3 end, int layerMask = 0, bool debugDraw = false, float duration = 0f)
		{
			RaycastHit raycastHit;
			return RaycastPathUtils.CheckPositionBlocked(start, end, out raycastHit, layerMask, debugDraw, duration);
		}

		// Token: 0x0600C55A RID: 50522 RVA: 0x00489038 File Offset: 0x00487238
		public static bool CheckPositionBlocked(Vector3 start, Vector3 end, out RaycastHit hit, int layerMask = 0, bool debugDraw = false, float duration = 0f)
		{
			Vector3 vector = end - start;
			return RaycastPathUtils.CheckPositionBlocked(new Ray(start - Origin.position, vector), out hit, layerMask, debugDraw, vector.magnitude, duration);
		}

		// Token: 0x0600C55B RID: 50523 RVA: 0x00489070 File Offset: 0x00487270
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

		// Token: 0x0600C55C RID: 50524 RVA: 0x004890FC File Offset: 0x004872FC
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

		// Token: 0x0600C55D RID: 50525 RVA: 0x00489190 File Offset: 0x00487390
		public static bool IsPositionBlocked(Ray ray, int layerMask = 0, bool debugDraw = false, float maxDist = 100f)
		{
			RaycastHit raycastHit;
			return RaycastPathUtils.IsPositionBlocked(ray, out raycastHit, layerMask, debugDraw, 100f);
		}

		// Token: 0x0600C55E RID: 50526 RVA: 0x004891AC File Offset: 0x004873AC
		public static void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 1f)
		{
			Utils.DrawLine(start - Origin.position, end - Origin.position, color, color, 10, duration);
		}

		// Token: 0x0600C55F RID: 50527 RVA: 0x004891D0 File Offset: 0x004873D0
		public static void DrawBounds(Vector3i pos, Color color, float duration, float scale = 1f)
		{
			Utils.DrawBoxLines(new Vector3((float)pos.x, (float)pos.y, (float)pos.z) - Origin.position, new Vector3((float)pos.x + scale, (float)pos.y + scale, (float)pos.z + scale) - Origin.position, color, duration);
		}

		// Token: 0x0600C560 RID: 50528 RVA: 0x00489234 File Offset: 0x00487434
		public static void DrawBounds(Vector3 pos, Color color, float duration, float scale = 1f)
		{
			Utils.DrawBoxLines(new Vector3i(pos.x, pos.y, pos.z) - Origin.position, new Vector3i(pos.x + scale, pos.y + scale, pos.z + scale) - Origin.position, color, duration);
		}

		// Token: 0x0600C561 RID: 50529 RVA: 0x0048929A File Offset: 0x0048749A
		public static void DrawNode(RaycastNode node, Color color, float duration)
		{
			RaycastPathUtils.DrawNode(node.Min, node.Max, color, duration);
		}

		// Token: 0x0600C562 RID: 50530 RVA: 0x004892AF File Offset: 0x004874AF
		public static void DrawNode(Vector3 min, Vector3 max, Color color, float duration)
		{
			RaycastPathUtils.DrawVolume(min - Origin.position, max - Origin.position, color, duration);
		}

		// Token: 0x0600C563 RID: 50531 RVA: 0x004892D0 File Offset: 0x004874D0
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

		// Token: 0x04009507 RID: 38151
		[PublicizedFrom(EAccessModifier.Private)]
		public const int cMaxRayDist = 100;
	}
}

using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace RaycastPathing
{
	// Token: 0x020018D1 RID: 6353
	[Preserve]
	public class RaycastPathUtils
	{
		// Token: 0x0600C3FF RID: 50175 RVA: 0x00484A80 File Offset: 0x00482C80
		public static bool IsPositionBlocked(Vector3 start, Vector3 end, out RaycastHit hit, int layerMask = 0, bool debugDraw = false)
		{
			Vector3 vector = end - start;
			return RaycastPathUtils.IsPositionBlocked(new Ray(start - Origin.position, vector), out hit, layerMask, debugDraw, vector.magnitude + 1f);
		}

		// Token: 0x0600C400 RID: 50176 RVA: 0x00484ABC File Offset: 0x00482CBC
		public static bool IsPositionBlocked(Vector3 start, Vector3 end, int layerMask = 0, bool debugDraw = false)
		{
			RaycastHit raycastHit;
			return RaycastPathUtils.IsPositionBlocked(start, end, out raycastHit, layerMask, debugDraw);
		}

		// Token: 0x0600C401 RID: 50177 RVA: 0x00484AD4 File Offset: 0x00482CD4
		public static bool IsPointBlocked(Vector3 start, Vector3 end, int layerMask = 0, bool debugDraw = false, float duration = 0f)
		{
			RaycastHit raycastHit;
			return RaycastPathUtils.CheckPositionBlocked(start, end, out raycastHit, layerMask, debugDraw, duration);
		}

		// Token: 0x0600C402 RID: 50178 RVA: 0x00484AF0 File Offset: 0x00482CF0
		public static bool CheckPositionBlocked(Vector3 start, Vector3 end, out RaycastHit hit, int layerMask = 0, bool debugDraw = false, float duration = 0f)
		{
			Vector3 vector = end - start;
			return RaycastPathUtils.CheckPositionBlocked(new Ray(start - Origin.position, vector), out hit, layerMask, debugDraw, vector.magnitude, duration);
		}

		// Token: 0x0600C403 RID: 50179 RVA: 0x00484B28 File Offset: 0x00482D28
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

		// Token: 0x0600C404 RID: 50180 RVA: 0x00484BB4 File Offset: 0x00482DB4
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

		// Token: 0x0600C405 RID: 50181 RVA: 0x00484C48 File Offset: 0x00482E48
		public static bool IsPositionBlocked(Ray ray, int layerMask = 0, bool debugDraw = false, float maxDist = 100f)
		{
			RaycastHit raycastHit;
			return RaycastPathUtils.IsPositionBlocked(ray, out raycastHit, layerMask, debugDraw, 100f);
		}

		// Token: 0x0600C406 RID: 50182 RVA: 0x00484C64 File Offset: 0x00482E64
		public static void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 1f)
		{
			Utils.DrawLine(start - Origin.position, end - Origin.position, color, color, 10, duration);
		}

		// Token: 0x0600C407 RID: 50183 RVA: 0x00484C88 File Offset: 0x00482E88
		public static void DrawBounds(Vector3i pos, Color color, float duration, float scale = 1f)
		{
			Utils.DrawBoxLines(new Vector3((float)pos.x, (float)pos.y, (float)pos.z) - Origin.position, new Vector3((float)pos.x + scale, (float)pos.y + scale, (float)pos.z + scale) - Origin.position, color, duration);
		}

		// Token: 0x0600C408 RID: 50184 RVA: 0x00484CEC File Offset: 0x00482EEC
		public static void DrawBounds(Vector3 pos, Color color, float duration, float scale = 1f)
		{
			Utils.DrawBoxLines(new Vector3i(pos.x, pos.y, pos.z) - Origin.position, new Vector3i(pos.x + scale, pos.y + scale, pos.z + scale) - Origin.position, color, duration);
		}

		// Token: 0x0600C409 RID: 50185 RVA: 0x00484D52 File Offset: 0x00482F52
		public static void DrawNode(RaycastNode node, Color color, float duration)
		{
			RaycastPathUtils.DrawNode(node.Min, node.Max, color, duration);
		}

		// Token: 0x0600C40A RID: 50186 RVA: 0x00484D67 File Offset: 0x00482F67
		public static void DrawNode(Vector3 min, Vector3 max, Color color, float duration)
		{
			RaycastPathUtils.DrawVolume(min - Origin.position, max - Origin.position, color, duration);
		}

		// Token: 0x0600C40B RID: 50187 RVA: 0x00484D88 File Offset: 0x00482F88
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

		// Token: 0x04009441 RID: 37953
		[PublicizedFrom(EAccessModifier.Private)]
		public const int cMaxRayDist = 100;
	}
}

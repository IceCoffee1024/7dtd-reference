using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace RaycastPathing
{
	// Token: 0x0200192E RID: 6446
	[Preserve]
	public class RaycastPathUtils
	{
		// Token: 0x0600C66B RID: 50795 RVA: 0x0048C974 File Offset: 0x0048AB74
		public static bool IsPositionBlocked(Vector3 start, Vector3 end, out RaycastHit hit, int layerMask = 0, bool debugDraw = false)
		{
			Vector3 vector = end - start;
			return RaycastPathUtils.IsPositionBlocked(new Ray(start - Origin.position, vector), out hit, layerMask, debugDraw, vector.magnitude + 1f);
		}

		// Token: 0x0600C66C RID: 50796 RVA: 0x0048C9B0 File Offset: 0x0048ABB0
		public static bool IsPositionBlocked(Vector3 start, Vector3 end, int layerMask = 0, bool debugDraw = false)
		{
			RaycastHit raycastHit;
			return RaycastPathUtils.IsPositionBlocked(start, end, out raycastHit, layerMask, debugDraw);
		}

		// Token: 0x0600C66D RID: 50797 RVA: 0x0048C9C8 File Offset: 0x0048ABC8
		public static bool IsPointBlocked(Vector3 start, Vector3 end, int layerMask = 0, bool debugDraw = false, float duration = 0f)
		{
			RaycastHit raycastHit;
			return RaycastPathUtils.CheckPositionBlocked(start, end, out raycastHit, layerMask, debugDraw, duration);
		}

		// Token: 0x0600C66E RID: 50798 RVA: 0x0048C9E4 File Offset: 0x0048ABE4
		public static bool CheckPositionBlocked(Vector3 start, Vector3 end, out RaycastHit hit, int layerMask = 0, bool debugDraw = false, float duration = 0f)
		{
			Vector3 vector = end - start;
			return RaycastPathUtils.CheckPositionBlocked(new Ray(start - Origin.position, vector), out hit, layerMask, debugDraw, vector.magnitude, duration);
		}

		// Token: 0x0600C66F RID: 50799 RVA: 0x0048CA1C File Offset: 0x0048AC1C
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

		// Token: 0x0600C670 RID: 50800 RVA: 0x0048CAA8 File Offset: 0x0048ACA8
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

		// Token: 0x0600C671 RID: 50801 RVA: 0x0048CB3C File Offset: 0x0048AD3C
		public static bool IsPositionBlocked(Ray ray, int layerMask = 0, bool debugDraw = false, float maxDist = 100f)
		{
			RaycastHit raycastHit;
			return RaycastPathUtils.IsPositionBlocked(ray, out raycastHit, layerMask, debugDraw, 100f);
		}

		// Token: 0x0600C672 RID: 50802 RVA: 0x0048CB58 File Offset: 0x0048AD58
		public static void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 1f)
		{
			Utils.DrawLine(start - Origin.position, end - Origin.position, color, color, 10, duration);
		}

		// Token: 0x0600C673 RID: 50803 RVA: 0x0048CB7C File Offset: 0x0048AD7C
		public static void DrawBounds(Vector3i pos, Color color, float duration, float scale = 1f)
		{
			Utils.DrawBoxLines(new Vector3((float)pos.x, (float)pos.y, (float)pos.z) - Origin.position, new Vector3((float)pos.x + scale, (float)pos.y + scale, (float)pos.z + scale) - Origin.position, color, duration);
		}

		// Token: 0x0600C674 RID: 50804 RVA: 0x0048CBE0 File Offset: 0x0048ADE0
		public static void DrawBounds(Vector3 pos, Color color, float duration, float scale = 1f)
		{
			Utils.DrawBoxLines(new Vector3i(pos.x, pos.y, pos.z) - Origin.position, new Vector3i(pos.x + scale, pos.y + scale, pos.z + scale) - Origin.position, color, duration);
		}

		// Token: 0x0600C675 RID: 50805 RVA: 0x0048CC46 File Offset: 0x0048AE46
		public static void DrawNode(RaycastNode node, Color color, float duration)
		{
			RaycastPathUtils.DrawNode(node.Min, node.Max, color, duration);
		}

		// Token: 0x0600C676 RID: 50806 RVA: 0x0048CC5B File Offset: 0x0048AE5B
		public static void DrawNode(Vector3 min, Vector3 max, Color color, float duration)
		{
			RaycastPathUtils.DrawVolume(min - Origin.position, max - Origin.position, color, duration);
		}

		// Token: 0x0600C677 RID: 50807 RVA: 0x0048CC7C File Offset: 0x0048AE7C
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

		// Token: 0x040095F0 RID: 38384
		[PublicizedFrom(EAccessModifier.Private)]
		public const int cMaxRayDist = 100;
	}
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using Platform;
using Unity.Collections;
using UnityEngine;

// Token: 0x020014EB RID: 5355
public static class Utils
{
	// Token: 0x170013AF RID: 5039
	// (get) Token: 0x0600A7F0 RID: 42992 RVA: 0x003E750C File Offset: 0x003E570C
	public static CultureInfo StandardCulture
	{
		get
		{
			return Utils.ci;
		}
	}

	// Token: 0x0600A7F1 RID: 42993 RVA: 0x003E7514 File Offset: 0x003E5714
	public static void InitStatic()
	{
		Utils.ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
		Utils.ci = (CultureInfo)CultureInfo.InvariantCulture.Clone();
		Utils.ci.NumberFormat.CurrencyDecimalSeparator = ".";
		Utils.ci.NumberFormat.NumberDecimalSeparator = ".";
		Utils.ci.NumberFormat.NumberGroupSeparator = ",";
		Utils.ci.DateTimeFormat.ShortDatePattern = "yyyy-MM-dd";
		Utils.ci.DateTimeFormat.LongDatePattern = "yyyy-MM-dd";
		Utils.ci.DateTimeFormat.ShortTimePattern = "HH:mm:ss";
		Utils.ci.DateTimeFormat.LongTimePattern = "HH:mm:ss";
		if (!GlobalCultureInfo.SetDefaultCulture(Utils.ci))
		{
			Log.Warning("Setting global culture failed!");
		}
		DynamicMethod dynamicMethod = new DynamicMethod("Memset", MethodAttributes.FamANDAssem | MethodAttributes.Family | MethodAttributes.Static, CallingConventions.Standard, null, new Type[]
		{
			typeof(IntPtr),
			typeof(byte),
			typeof(int)
		}, typeof(Utils), true);
		ILGenerator ilgenerator = dynamicMethod.GetILGenerator();
		ilgenerator.Emit(OpCodes.Ldarg_0);
		ilgenerator.Emit(OpCodes.Ldarg_1);
		ilgenerator.Emit(OpCodes.Ldarg_2);
		ilgenerator.Emit(OpCodes.Initblk);
		ilgenerator.Emit(OpCodes.Ret);
		Utils.MemsetDelegate = (Action<IntPtr, byte, int>)dynamicMethod.CreateDelegate(typeof(Action<IntPtr, byte, int>));
	}

	// Token: 0x0600A7F2 RID: 42994 RVA: 0x003E7688 File Offset: 0x003E5888
	public static string GetChildPath(this Transform t, Transform child)
	{
		string text = child.name;
		Transform transform = child.parent;
		while (transform != null && transform != t)
		{
			text = transform.name + "/" + text;
			transform = transform.parent;
		}
		if (transform == null)
		{
			throw new Exception(t.name + " does not have a child " + child.name);
		}
		return text;
	}

	// Token: 0x0600A7F3 RID: 42995 RVA: 0x003E76F8 File Offset: 0x003E58F8
	public static void SetTagsRecursively(Transform t, string tag)
	{
		t.gameObject.tag = tag;
		for (int i = t.childCount - 1; i >= 0; i--)
		{
			Utils.SetTagsRecursively(t.GetChild(i), tag);
		}
	}

	// Token: 0x0600A7F4 RID: 42996 RVA: 0x003E7734 File Offset: 0x003E5934
	public static void SetTagsIfNoneRecursively(Transform t, string tag)
	{
		if (t.gameObject.CompareTag("Untagged"))
		{
			t.gameObject.tag = tag;
		}
		for (int i = t.childCount - 1; i >= 0; i--)
		{
			Utils.SetTagsIfNoneRecursively(t.GetChild(i), tag);
		}
	}

	// Token: 0x0600A7F5 RID: 42997 RVA: 0x003E7780 File Offset: 0x003E5980
	public static void SetTagsIfMatchRecursively(Transform t, string matchTag, string tag)
	{
		if (t.gameObject.CompareTag(matchTag))
		{
			t.gameObject.tag = tag;
		}
		for (int i = t.childCount - 1; i >= 0; i--)
		{
			Utils.SetTagsIfMatchRecursively(t.GetChild(i), matchTag, tag);
		}
	}

	// Token: 0x0600A7F6 RID: 42998 RVA: 0x003E77C8 File Offset: 0x003E59C8
	public static void DestroyWithTagRecursively(Transform t, string tag)
	{
		for (int i = t.childCount - 1; i >= 0; i--)
		{
			Utils.DestroyWithTagRecursively(t.GetChild(i), tag);
		}
		if (t.gameObject.CompareTag(tag))
		{
			global::UnityEngine.Object.Destroy(t.gameObject);
		}
	}

	// Token: 0x0600A7F7 RID: 42999 RVA: 0x003E7810 File Offset: 0x003E5A10
	public static void SetLayerRecursively(GameObject go, int newLayer)
	{
		go.GetComponentsInChildren<Transform>(true, Utils.setLayerRecursivelyList);
		List<Transform> list = Utils.setLayerRecursivelyList;
		if (newLayer == 2)
		{
			for (int i = list.Count - 1; i >= 0; i--)
			{
				list[i].gameObject.layer = newLayer;
			}
		}
		else
		{
			for (int j = list.Count - 1; j >= 0; j--)
			{
				Transform transform = list[j];
				if (transform.CompareTag("LargeEntityBlocker"))
				{
					transform.gameObject.layer = 19;
				}
				else
				{
					transform.gameObject.layer = newLayer;
				}
			}
		}
		Utils.setLayerRecursivelyList.Clear();
	}

	// Token: 0x0600A7F8 RID: 43000 RVA: 0x003E78A8 File Offset: 0x003E5AA8
	public static void SetLayerRecursively(GameObject go, int newLayer, string[] excludeTags)
	{
		go.GetComponentsInChildren<Transform>(true, Utils.setLayerRecursivelyList);
		List<Transform> list = Utils.setLayerRecursivelyList;
		if (newLayer == 2)
		{
			for (int i = list.Count - 1; i >= 0; i--)
			{
				Utils.SetLayerWithExclusionList(list[i].gameObject, newLayer, excludeTags);
			}
		}
		else
		{
			for (int j = list.Count - 1; j >= 0; j--)
			{
				Transform transform = list[j];
				if (transform.CompareTag("LargeEntityBlocker"))
				{
					Utils.SetLayerWithExclusionList(transform.gameObject, 19, excludeTags);
				}
				else
				{
					Utils.SetLayerWithExclusionList(transform.gameObject, newLayer, excludeTags);
				}
			}
		}
		Utils.setLayerRecursivelyList.Clear();
	}

	// Token: 0x0600A7F9 RID: 43001 RVA: 0x003E7944 File Offset: 0x003E5B44
	public static void SetLayerWithExclusionList(GameObject go, int layer, string[] excludeTags)
	{
		if (excludeTags != null)
		{
			foreach (string text in excludeTags)
			{
				if (go.CompareTag(text))
				{
					return;
				}
			}
		}
		go.layer = layer;
	}

	// Token: 0x0600A7FA RID: 43002 RVA: 0x003E797C File Offset: 0x003E5B7C
	public static void SetColliderLayerRecursively(GameObject go, int newLayer)
	{
		go.GetComponentsInChildren<Collider>(true, Utils.setColliderLayerRecursivelyList);
		for (int i = Utils.setColliderLayerRecursivelyList.Count - 1; i >= 0; i--)
		{
			Utils.setColliderLayerRecursivelyList[i].gameObject.layer = newLayer;
		}
	}

	// Token: 0x0600A7FB RID: 43003 RVA: 0x003E79C4 File Offset: 0x003E5BC4
	public static void MoveTaggedToLayer(GameObject go, string tag, int newLayer)
	{
		go.GetComponentsInChildren<Transform>(true, Utils.setLayerRecursivelyList);
		List<Transform> list = Utils.setLayerRecursivelyList;
		for (int i = list.Count - 1; i >= 0; i--)
		{
			Transform transform = list[i];
			if (transform.CompareTag(tag))
			{
				transform.gameObject.layer = newLayer;
			}
		}
		Utils.setLayerRecursivelyList.Clear();
	}

	// Token: 0x0600A7FC RID: 43004 RVA: 0x003E7A20 File Offset: 0x003E5C20
	public static void DrawBlockBounds(BoundsInt _bounds, Color _color, float _duration)
	{
		Bounds bounds = default(Bounds);
		bounds.SetMinMax(World.blockToTransformPos(_bounds.min), World.blockToTransformPos(_bounds.max) + new Vector3(0f, 1f, 0f));
		bounds.Expand(new Vector3(0.5f, 0f, 0.5f));
		Utils.DrawBounds(bounds, _color, _duration);
	}

	// Token: 0x0600A7FD RID: 43005 RVA: 0x003E7A9C File Offset: 0x003E5C9C
	public static void DrawBounds(Bounds _bounds, Color _color, float _duration)
	{
		Vector3 center = _bounds.center;
		Vector3 extents = _bounds.extents;
		Vector3 vector = new Vector3(center.x - extents.x, center.y + extents.y, center.z - extents.z);
		Vector3 vector2 = new Vector3(center.x + extents.x, center.y + extents.y, center.z - extents.z);
		Vector3 vector3 = new Vector3(center.x - extents.x, center.y - extents.y, center.z - extents.z);
		Vector3 vector4 = new Vector3(center.x + extents.x, center.y - extents.y, center.z - extents.z);
		Vector3 vector5 = new Vector3(center.x - extents.x, center.y + extents.y, center.z + extents.z);
		Vector3 vector6 = new Vector3(center.x + extents.x, center.y + extents.y, center.z + extents.z);
		Vector3 vector7 = new Vector3(center.x - extents.x, center.y - extents.y, center.z + extents.z);
		Vector3 vector8 = new Vector3(center.x + extents.x, center.y - extents.y, center.z + extents.z);
		global::UnityEngine.Debug.DrawLine(vector, vector2, _color, _duration);
		global::UnityEngine.Debug.DrawLine(vector2, vector4, _color, _duration);
		global::UnityEngine.Debug.DrawLine(vector4, vector3, _color, _duration);
		global::UnityEngine.Debug.DrawLine(vector3, vector, _color, _duration);
		global::UnityEngine.Debug.DrawLine(vector5, vector6, _color, _duration);
		global::UnityEngine.Debug.DrawLine(vector6, vector8, _color, _duration);
		global::UnityEngine.Debug.DrawLine(vector8, vector7, _color, _duration);
		global::UnityEngine.Debug.DrawLine(vector7, vector5, _color, _duration);
		global::UnityEngine.Debug.DrawLine(vector, vector5, _color, _duration);
		global::UnityEngine.Debug.DrawLine(vector2, vector6, _color, _duration);
		global::UnityEngine.Debug.DrawLine(vector4, vector8, _color, _duration);
		global::UnityEngine.Debug.DrawLine(vector3, vector7, _color, _duration);
	}

	// Token: 0x0600A7FE RID: 43006 RVA: 0x003E7CA8 File Offset: 0x003E5EA8
	public static void DrawAxisLines(Vector3 centerPos, float radius, Color color, float duration)
	{
		Vector3 vector = centerPos;
		Vector3 vector2 = centerPos;
		vector.x -= radius;
		vector2.x += radius;
		global::UnityEngine.Debug.DrawLine(vector, vector2, color, duration);
		vector.x = centerPos.x;
		vector2.x = centerPos.x;
		vector.z -= radius;
		vector2.z += radius;
		global::UnityEngine.Debug.DrawLine(vector, vector2, color, duration);
		vector.z = centerPos.z;
		vector2.z = centerPos.z;
		vector.y -= radius;
		vector2.y += radius;
		global::UnityEngine.Debug.DrawLine(vector, vector2, color, duration);
	}

	// Token: 0x0600A7FF RID: 43007 RVA: 0x003E7D50 File Offset: 0x003E5F50
	public static void DrawBoxLines(Vector3 minPos, Vector3 maxPos, Color color, float duration)
	{
		Utils.DrawBoxLinesHorizontal(minPos, maxPos, color, duration);
		Utils.DrawBoxLinesVerticle(minPos, maxPos, color, duration);
	}

	// Token: 0x0600A800 RID: 43008 RVA: 0x003E7D64 File Offset: 0x003E5F64
	public static void DrawBoxLinesHorizontal(Vector3 minPos, Vector3 maxPos, Color color, float duration)
	{
		Vector3 vector = new Vector3(maxPos.x, minPos.y, maxPos.z);
		Vector3 vector2 = new Vector3(minPos.x, maxPos.y, minPos.z);
		Utils.DrawRectLinesHorzontal(minPos, vector, color, duration);
		Utils.DrawRectLinesHorzontal(vector2, maxPos, color, duration);
	}

	// Token: 0x0600A801 RID: 43009 RVA: 0x003E7DB4 File Offset: 0x003E5FB4
	public static void DrawBoxLinesVerticle(Vector3 minPos, Vector3 maxPos, Color color, float duration)
	{
		Vector3 vector = new Vector3(0f, maxPos.y - minPos.y);
		global::UnityEngine.Debug.DrawRay(minPos, vector, color, duration);
		global::UnityEngine.Debug.DrawRay(new Vector3(minPos.x, minPos.y, maxPos.z), vector, color, duration);
		global::UnityEngine.Debug.DrawRay(maxPos, -vector, color, duration);
		global::UnityEngine.Debug.DrawRay(new Vector3(maxPos.x, minPos.y, minPos.z), vector, color, duration);
	}

	// Token: 0x0600A802 RID: 43010 RVA: 0x003E7E30 File Offset: 0x003E6030
	public static void DrawCubeLines(Vector3 minPos, float size, Color color, float duration)
	{
		Vector3 vector = minPos;
		vector.x += size;
		vector.y += size;
		vector.z += size;
		Utils.DrawBoxLines(minPos, vector, color, duration);
	}

	// Token: 0x0600A803 RID: 43011 RVA: 0x003E7E6C File Offset: 0x003E606C
	public static void DrawCubeLinesCenter(Vector3 centerPos, float radius, Color color, float duration)
	{
		Vector3 vector = centerPos;
		vector.x -= radius;
		vector.y -= radius;
		vector.z -= radius;
		Vector3 vector2 = centerPos;
		vector2.x += radius;
		vector2.y += radius;
		vector2.z += radius;
		Utils.DrawBoxLines(vector, vector2, color, duration);
	}

	// Token: 0x0600A804 RID: 43012 RVA: 0x003E7ED0 File Offset: 0x003E60D0
	public static void DrawRectLinesHorzontal(Vector3 minPos, Vector3 maxPos, Color color, float duration)
	{
		Vector3 vector = new Vector3(maxPos.x, minPos.y, minPos.z);
		Vector3 vector2 = new Vector3(minPos.x, minPos.y, maxPos.z);
		global::UnityEngine.Debug.DrawLine(minPos, vector2, color, duration);
		global::UnityEngine.Debug.DrawLine(vector2, maxPos, color, duration);
		global::UnityEngine.Debug.DrawLine(maxPos, vector, color, duration);
		global::UnityEngine.Debug.DrawLine(vector, minPos, color, duration);
	}

	// Token: 0x0600A805 RID: 43013 RVA: 0x003E7F33 File Offset: 0x003E6133
	public static void DrawCylinderLinesHorzontal(Vector3 centerPos, float radius, float height, Color startColor, Color endColor, int segments, float duration)
	{
		Utils.DrawCircleLinesHorzontal(centerPos, radius, startColor, endColor, segments, duration);
		centerPos.y += height;
		Utils.DrawCircleLinesHorzontal(centerPos, radius, startColor, endColor, segments, duration);
	}

	// Token: 0x0600A806 RID: 43014 RVA: 0x003E7F60 File Offset: 0x003E6160
	public static void DrawCircleLinesHorzontal(Vector3 centerPos, float radius, Color startColor, Color endColor, int segments, float duration)
	{
		if (segments < 3)
		{
			segments = 3;
		}
		float num = -6.2831855f / (float)segments;
		float num2 = 1.5707964f;
		float num3 = 0f;
		float num4 = 1f / (float)(segments - 1);
		Vector3 vector;
		vector.y = centerPos.y;
		vector.x = centerPos.x + Mathf.Cos(num2) * radius;
		vector.z = centerPos.z + Mathf.Sin(num2) * radius;
		Vector3 vector2;
		vector2.y = centerPos.y;
		for (int i = 0; i < segments; i++)
		{
			num2 += num;
			vector2.x = centerPos.x + Mathf.Cos(num2) * radius;
			vector2.z = centerPos.z + Mathf.Sin(num2) * radius;
			Color color = Color.Lerp(startColor, endColor, num3);
			global::UnityEngine.Debug.DrawLine(vector, vector2, color, duration);
			vector.x = vector2.x;
			vector.z = vector2.z;
			num3 += num4;
		}
	}

	// Token: 0x0600A807 RID: 43015 RVA: 0x003E8058 File Offset: 0x003E6258
	public static void DrawLine(Vector3 startPos, Vector3 endPos, Color startColor, Color endColor, int segments, float duration = 0f)
	{
		if (segments <= 1)
		{
			global::UnityEngine.Debug.DrawLine(startPos, endPos, startColor, duration);
			return;
		}
		Vector3 vector = (endPos - startPos) * (1f / (float)segments);
		float num = 0f;
		float num2 = 1f / (float)(segments - 1);
		for (int i = 0; i < segments; i++)
		{
			Color color = Color.Lerp(startColor, endColor, num);
			global::UnityEngine.Debug.DrawRay(startPos, vector, color, duration);
			startPos += vector;
			num += num2;
		}
	}

	// Token: 0x0600A808 RID: 43016 RVA: 0x003E80CC File Offset: 0x003E62CC
	public static void DrawRay(Vector3 startPos, Vector3 dir, Color startColor, Color endColor, int segments, float duration = 0f)
	{
		Utils.DrawLine(startPos, startPos + dir, startColor, endColor, segments, duration);
	}

	// Token: 0x0600A809 RID: 43017 RVA: 0x003E80E4 File Offset: 0x003E62E4
	public static void DrawOutline(Rect position, string text, GUIStyle style, Color outColor, Color inColor)
	{
		style.normal.textColor = outColor;
		float num = position.x;
		position.x = num - 1f;
		GUI.Label(position, text, style);
		position.x += 2f;
		GUI.Label(position, text, style);
		num = position.x;
		position.x = num - 1f;
		num = position.y;
		position.y = num - 1f;
		GUI.Label(position, text, style);
		position.y += 2f;
		GUI.Label(position, text, style);
		num = position.y;
		position.y = num - 1f;
		style.normal.textColor = inColor;
		GUI.Label(position, text, style);
	}

	// Token: 0x0600A80A RID: 43018 RVA: 0x003E81AC File Offset: 0x003E63AC
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int FastAbsInt(int _x)
	{
		if (_x >= 0)
		{
			return _x;
		}
		if (_x == -2147483648)
		{
			return int.MaxValue;
		}
		return -_x;
	}

	// Token: 0x0600A80B RID: 43019 RVA: 0x003E81C4 File Offset: 0x003E63C4
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float FastAbs(float _x)
	{
		if (_x < 0f)
		{
			return -_x;
		}
		return _x;
	}

	// Token: 0x0600A80C RID: 43020 RVA: 0x003E81D2 File Offset: 0x003E63D2
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int Fastfloor(float x)
	{
		if (x < 0f)
		{
			return (int)(x - 0.9999999f);
		}
		return (int)x;
	}

	// Token: 0x0600A80D RID: 43021 RVA: 0x003E81E7 File Offset: 0x003E63E7
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int Fastfloor(double x)
	{
		if (x < 0.0)
		{
			return (int)(x - 0.9999998807907104);
		}
		return (int)x;
	}

	// Token: 0x0600A80E RID: 43022 RVA: 0x003E8204 File Offset: 0x003E6404
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int FastMax(int v1, int v2)
	{
		if (v1 < v2)
		{
			return v2;
		}
		return v1;
	}

	// Token: 0x0600A80F RID: 43023 RVA: 0x003E8204 File Offset: 0x003E6404
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float FastMax(float v1, float v2)
	{
		if (v1 < v2)
		{
			return v2;
		}
		return v1;
	}

	// Token: 0x0600A810 RID: 43024 RVA: 0x003E8204 File Offset: 0x003E6404
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static byte FastMax(byte v1, byte v2)
	{
		if (v1 < v2)
		{
			return v2;
		}
		return v1;
	}

	// Token: 0x0600A811 RID: 43025 RVA: 0x003E820D File Offset: 0x003E640D
	public static float FastMax(float v1, float v2, float v3, float v4)
	{
		if (v1 >= v2 && v1 >= v3 && v1 >= v4)
		{
			return v1;
		}
		if (v2 >= v1 && v2 >= v3 && v2 >= v4)
		{
			return v2;
		}
		if (v3 >= v1 && v3 >= v2 && v3 >= v4)
		{
			return v3;
		}
		return v4;
	}

	// Token: 0x0600A812 RID: 43026 RVA: 0x003E823A File Offset: 0x003E643A
	public static int FastMax(int v1, int v2, int v3, int v4)
	{
		if (v1 >= v2 && v1 >= v3 && v1 >= v4)
		{
			return v1;
		}
		if (v2 >= v1 && v2 >= v3 && v2 >= v4)
		{
			return v2;
		}
		if (v3 >= v1 && v3 >= v2 && v3 >= v4)
		{
			return v3;
		}
		return v4;
	}

	// Token: 0x0600A813 RID: 43027 RVA: 0x003E8267 File Offset: 0x003E6467
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float FastMin(float v1, float v2)
	{
		if (v1 >= v2)
		{
			return v2;
		}
		return v1;
	}

	// Token: 0x0600A814 RID: 43028 RVA: 0x003E8267 File Offset: 0x003E6467
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int FastMin(int v1, int v2)
	{
		if (v1 >= v2)
		{
			return v2;
		}
		return v1;
	}

	// Token: 0x0600A815 RID: 43029 RVA: 0x003E8270 File Offset: 0x003E6470
	public static int FastMin(int v1, int v2, int v3, int v4)
	{
		if (v1 <= v2 && v1 <= v3 && v1 <= v4)
		{
			return v1;
		}
		if (v2 <= v1 && v2 <= v3 && v2 <= v4)
		{
			return v2;
		}
		if (v3 <= v1 && v3 <= v2 && v3 <= v4)
		{
			return v3;
		}
		return v4;
	}

	// Token: 0x0600A816 RID: 43030 RVA: 0x003E829D File Offset: 0x003E649D
	public static float FastMin(float v1, float v2, float v3, float v4)
	{
		if (v1 <= v2 && v1 <= v3 && v1 <= v4)
		{
			return v1;
		}
		if (v2 <= v1 && v2 <= v3 && v2 <= v4)
		{
			return v2;
		}
		if (v3 <= v1 && v3 <= v2 && v3 <= v4)
		{
			return v3;
		}
		return v4;
	}

	// Token: 0x0600A817 RID: 43031 RVA: 0x003E82CA File Offset: 0x003E64CA
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float FastClamp(float _v, float _min, float _max)
	{
		if (_v <= _min)
		{
			return _min;
		}
		if (_v >= _max)
		{
			return _max;
		}
		return _v;
	}

	// Token: 0x0600A818 RID: 43032 RVA: 0x003E82D9 File Offset: 0x003E64D9
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int FastClamp(int _v, int _min, int _max)
	{
		if (_v <= _min)
		{
			return _min;
		}
		if (_v >= _max)
		{
			return _max;
		}
		return _v;
	}

	// Token: 0x0600A819 RID: 43033 RVA: 0x003E82E8 File Offset: 0x003E64E8
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float FastClamp01(float _v)
	{
		if (_v <= 0f)
		{
			return 0f;
		}
		if (_v >= 1f)
		{
			return 1f;
		}
		return _v;
	}

	// Token: 0x0600A81A RID: 43034 RVA: 0x003E8307 File Offset: 0x003E6507
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double FastClamp01(double _v)
	{
		if (_v <= 0.0)
		{
			return 0.0;
		}
		if (_v >= 1.0)
		{
			return 1.0;
		}
		return _v;
	}

	// Token: 0x0600A81B RID: 43035 RVA: 0x003E8336 File Offset: 0x003E6536
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float FastLerp(float a, float b, float t)
	{
		return a + (b - a) * Utils.FastClamp01(t);
	}

	// Token: 0x0600A81C RID: 43036 RVA: 0x003CE34C File Offset: 0x003CC54C
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float FastLerpUnclamped(float a, float b, float t)
	{
		return a + (b - a) * t;
	}

	// Token: 0x0600A81D RID: 43037 RVA: 0x003E8344 File Offset: 0x003E6544
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float FastMoveTowards(float current, float target, float maxDelta)
	{
		if (Utils.FastAbs(target - current) <= maxDelta)
		{
			return target;
		}
		return current + Mathf.Sign(target - current) * maxDelta;
	}

	// Token: 0x0600A81E RID: 43038 RVA: 0x003E835F File Offset: 0x003E655F
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int FastRoundToInt(float f)
	{
		return (int)Math.Round((double)f);
	}

	// Token: 0x0600A81F RID: 43039 RVA: 0x003E8369 File Offset: 0x003E6569
	public static int FastRoundToIntAndMod(float _f, int _mod)
	{
		if (_mod == 0)
		{
			return 0;
		}
		if (_f <= 0f)
		{
			return (_mod + (int)(_f - 0.5f) % _mod) % _mod;
		}
		return (int)(_f + 0.5f) % _mod;
	}

	// Token: 0x0600A820 RID: 43040 RVA: 0x003E8394 File Offset: 0x003E6594
	public static Texture2D LoadRawStampFile(string _filepath)
	{
		Texture2D texture2D2;
		using (Stream stream = SdFile.OpenRead(_filepath))
		{
			using (BinaryReader binaryReader = new BinaryReader(stream))
			{
				binaryReader.BaseStream.Seek(0L, SeekOrigin.Begin);
				int num = (int)Mathf.Sqrt((float)(binaryReader.BaseStream.Length / 6L));
				Texture2D texture2D = new Texture2D(num, num, TextureFormat.RGBAFloat, false, false);
				Color[] array = new Color[num * num];
				for (int i = 0; i < array.Length; i++)
				{
					float num2 = (float)binaryReader.ReadUInt16() / 65535f;
					float num3 = (float)binaryReader.ReadUInt16() / 65535f;
					float num4 = (float)binaryReader.ReadUInt16() / 65535f;
					array[i] = new Color(num2, num2, num4, num3);
				}
				texture2D.SetPixels(array);
				texture2D.Apply();
				texture2D2 = texture2D;
			}
		}
		return texture2D2;
	}

	// Token: 0x0600A821 RID: 43041 RVA: 0x003E8480 File Offset: 0x003E6680
	public static Color[] LoadRawStampFileArray(string _filepath)
	{
		Color[] array3;
		using (Stream stream = SdFile.OpenRead(_filepath))
		{
			using (BinaryReader binaryReader = new BinaryReader(stream))
			{
				binaryReader.BaseStream.Seek(0L, SeekOrigin.Begin);
				byte[] array = new byte[binaryReader.BaseStream.Length];
				binaryReader.Read(array, 0, array.Length);
				int num = (int)(binaryReader.BaseStream.Length / 6L);
				Color[] array2 = new Color[num];
				Color black = Color.black;
				int num2 = 0;
				for (int i = 0; i < num; i++)
				{
					black.r = (float)((int)array[num2] | ((int)array[num2 + 1] << 8)) / 65535f;
					black.a = (float)((int)array[num2 + 2] | ((int)array[num2 + 3] << 8)) / 65535f;
					black.b = (float)((int)array[num2 + 4] | ((int)array[num2 + 5] << 8)) / 65535f;
					array2[i] = black;
					num2 += 6;
				}
				array3 = array2;
			}
		}
		return array3;
	}

	// Token: 0x0600A822 RID: 43042 RVA: 0x003E8598 File Offset: 0x003E6798
	public static void SaveRawStampFile(string _filepath, Texture2D _image)
	{
		using (Stream stream = SdFile.Open(_filepath, FileMode.CreateNew))
		{
			using (BinaryWriter binaryWriter = new BinaryWriter(stream))
			{
				binaryWriter.BaseStream.Seek(0L, SeekOrigin.Begin);
				Color[] pixels = _image.GetPixels();
				for (int i = 0; i < pixels.Length; i++)
				{
					binaryWriter.Write((ushort)(pixels[i].r * 65535f));
					binaryWriter.Write((ushort)(pixels[i].g * 65535f));
					binaryWriter.Write((ushort)(pixels[i].b * 65535f));
				}
			}
		}
	}

	// Token: 0x0600A823 RID: 43043 RVA: 0x003E8658 File Offset: 0x003E6858
	public static void SaveRawStampFile(string _filepath, Color[] _image)
	{
		using (Stream stream = SdFile.Open(_filepath, FileMode.CreateNew))
		{
			using (BinaryWriter binaryWriter = new BinaryWriter(stream))
			{
				binaryWriter.BaseStream.Seek(0L, SeekOrigin.Begin);
				for (int i = 0; i < _image.Length; i++)
				{
					binaryWriter.Write((ushort)(_image[i].r * 65535f));
					binaryWriter.Write((ushort)(_image[i].g * 65535f));
					binaryWriter.Write((ushort)(_image[i].b * 65535f));
				}
			}
		}
	}

	// Token: 0x0600A824 RID: 43044 RVA: 0x003E8710 File Offset: 0x003E6910
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float Repeat(float _value, float _length)
	{
		return Utils.FastClamp(_value - (float)Utils.Fastfloor(_value / _length) * _length, 0f, _length);
	}

	// Token: 0x0600A825 RID: 43045 RVA: 0x003E872C File Offset: 0x003E692C
	public static float DeltaAngle(float _angle1, float _angle2)
	{
		float num = Utils.Repeat(_angle2 - _angle1, 360f);
		if (num > 180f)
		{
			num -= 360f;
		}
		return num;
	}

	// Token: 0x0600A826 RID: 43046 RVA: 0x003E8758 File Offset: 0x003E6958
	public static float GetAngleBetween(Vector3 _dir1, Vector3 _dir2)
	{
		float num = Mathf.Atan2(_dir1.z, _dir1.x) * 57.29578f;
		float num2 = Mathf.Atan2(_dir2.z, _dir2.x) * 57.29578f;
		float num3 = num - num2;
		if (num3 > 180f)
		{
			num3 -= 360f;
		}
		if (num3 < -180f)
		{
			num3 += 360f;
		}
		return num3;
	}

	// Token: 0x0600A827 RID: 43047 RVA: 0x003E87B8 File Offset: 0x003E69B8
	public static int Get4HitDirectionAsInt(Vector3 _direction, Vector3 _myLook)
	{
		float angleBetween = Utils.GetAngleBetween(_direction, _myLook);
		if (angleBetween > -45f && angleBetween <= 45f)
		{
			return 1;
		}
		if (angleBetween < -45f && angleBetween >= -135f)
		{
			return 3;
		}
		if (angleBetween > 45f && angleBetween <= 135f)
		{
			return 2;
		}
		return 0;
	}

	// Token: 0x0600A828 RID: 43048 RVA: 0x003E8804 File Offset: 0x003E6A04
	public static int Get6HitDirectionAsInt(Vector3 _direction, Vector3 _myLook)
	{
		float angleBetween = Utils.GetAngleBetween(_direction, _myLook);
		if (angleBetween > -45f && angleBetween <= 45f)
		{
			return 1;
		}
		if (angleBetween < -45f && angleBetween >= -120f)
		{
			return 3;
		}
		if (angleBetween > 45f && angleBetween <= 120f)
		{
			return 2;
		}
		if (angleBetween > 120f && angleBetween < 160f)
		{
			return 4;
		}
		if (angleBetween < -120f && angleBetween > -160f)
		{
			return 5;
		}
		return 0;
	}

	// Token: 0x0600A829 RID: 43049 RVA: 0x003E8874 File Offset: 0x003E6A74
	public static Utils.EnumHitDirection GetHitDirection4Sides(Vector3 fwd, Vector3 targetDir, Vector3 up)
	{
		targetDir = targetDir.normalized;
		float num = Vector3.Dot(Vector3.Cross(fwd, targetDir), up);
		if (num > 0.75f)
		{
			return Utils.EnumHitDirection.Left;
		}
		if (num < -0.75f)
		{
			return Utils.EnumHitDirection.Right;
		}
		if ((double)Mathf.Abs(targetDir.y) > 0.05 && (double)Mathf.Abs(targetDir.x / targetDir.y) < 0.1 && (double)Mathf.Abs(targetDir.z / targetDir.y) < 0.1)
		{
			return Utils.EnumHitDirection.Back;
		}
		if (Mathf.Abs(Mathf.Atan2(targetDir.z, targetDir.x) - Mathf.Atan2(fwd.z, fwd.x)) > 1f)
		{
			return Utils.EnumHitDirection.Front;
		}
		return Utils.EnumHitDirection.Back;
	}

	// Token: 0x0600A82A RID: 43050 RVA: 0x003E8934 File Offset: 0x003E6B34
	public static Quaternion BlockFaceToRotation(BlockFace _blockFace)
	{
		switch (_blockFace)
		{
		case BlockFace.Top:
			return Quaternion.identity;
		case BlockFace.Bottom:
			return Quaternion.AngleAxis(180f, Vector3.forward);
		case BlockFace.North:
			return Quaternion.AngleAxis(90f, Vector3.right);
		case BlockFace.West:
			return Quaternion.AngleAxis(90f, Vector3.forward);
		case BlockFace.South:
			return Quaternion.AngleAxis(-90f, Vector3.right);
		case BlockFace.East:
			return Quaternion.AngleAxis(-90f, Vector3.forward);
		default:
			return Quaternion.identity;
		}
	}

	// Token: 0x0600A82B RID: 43051 RVA: 0x003E89BC File Offset: 0x003E6BBC
	public static Vector3 BlockFaceToVector(BlockFace _face)
	{
		switch (_face)
		{
		case BlockFace.Top:
			return new Vector3(0f, 1f, 0f);
		case BlockFace.Bottom:
			return new Vector3(0f, -1f, 0f);
		case BlockFace.North:
			return new Vector3(0f, 0f, 1f);
		case BlockFace.West:
			return new Vector3(-1f, 0f, 0f);
		case BlockFace.South:
			return new Vector3(0f, 0f, -1f);
		case BlockFace.East:
			return new Vector3(1f, 0f, 0f);
		default:
			return Vector3.zero;
		}
	}

	// Token: 0x0600A82C RID: 43052 RVA: 0x003E8A6C File Offset: 0x003E6C6C
	public static void MoveInBlockFaceDirection(Vector3[] _vertices, BlockFace _face, float d)
	{
		Vector3 zero = Vector3.zero;
		switch (_face)
		{
		case BlockFace.Top:
			zero = new Vector3(0f, d, 0f);
			break;
		case BlockFace.Bottom:
			zero = new Vector3(0f, -d, 0f);
			break;
		case BlockFace.North:
			zero = new Vector3(0f, 0f, d);
			break;
		case BlockFace.West:
			zero = new Vector3(-d, 0f, 0f);
			break;
		case BlockFace.South:
			zero = new Vector3(0f, 0f, -d);
			break;
		case BlockFace.East:
			zero = new Vector3(d, 0f, 0f);
			break;
		}
		for (int i = 0; i < _vertices.Length; i++)
		{
			_vertices[i] += zero;
		}
	}

	// Token: 0x0600A82D RID: 43053 RVA: 0x003E8B40 File Offset: 0x003E6D40
	public static string EncryptOrDecrypt(string text, string key)
	{
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < text.Length; i++)
		{
			stringBuilder.Append(text[i] ^ key[i % key.Length]);
		}
		return stringBuilder.ToString();
	}

	// Token: 0x0600A82E RID: 43054 RVA: 0x003E8B88 File Offset: 0x003E6D88
	public static string Crypt(string text)
	{
		return Utils.EncryptOrDecrypt(text, "X");
	}

	// Token: 0x0600A82F RID: 43055 RVA: 0x003E8B95 File Offset: 0x003E6D95
	public static string UnCryptFromBase64(char[] text)
	{
		return Utils.Crypt(Utils.FromBase64(text));
	}

	// Token: 0x0600A830 RID: 43056 RVA: 0x003E8BA2 File Offset: 0x003E6DA2
	public static string ToBase64(string _str)
	{
		return Convert.ToBase64String(Encoding.UTF8.GetBytes(_str));
	}

	// Token: 0x0600A831 RID: 43057 RVA: 0x003E8BB4 File Offset: 0x003E6DB4
	public static string FromBase64(string _encodedStr)
	{
		string text;
		try
		{
			byte[] array = Convert.FromBase64String(_encodedStr);
			text = Encoding.UTF8.GetString(array);
		}
		catch (FormatException ex)
		{
			Log.Error("FBase64 Exception");
			Log.Exception(ex);
			text = string.Empty;
		}
		return text;
	}

	// Token: 0x0600A832 RID: 43058 RVA: 0x003E8C00 File Offset: 0x003E6E00
	public static string FromBase64(char[] _bytes)
	{
		string text;
		try
		{
			byte[] array = Convert.FromBase64CharArray(_bytes, 0, _bytes.Length);
			text = Encoding.UTF8.GetString(array);
		}
		catch (FormatException ex)
		{
			Log.Error("FBase64A Exception");
			Log.Exception(ex);
			text = string.Empty;
		}
		return text;
	}

	// Token: 0x0600A833 RID: 43059 RVA: 0x003E8C50 File Offset: 0x003E6E50
	public static string HashString(string password)
	{
		if (password.Length == 0)
		{
			return password;
		}
		return password.GetHashCode().ToString();
	}

	// Token: 0x0600A834 RID: 43060 RVA: 0x003E8C75 File Offset: 0x003E6E75
	public static string ColorToHex(Color32 color)
	{
		return color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
	}

	// Token: 0x0600A835 RID: 43061 RVA: 0x003E8CB0 File Offset: 0x003E6EB0
	public static Color Saturate(Color _c)
	{
		if (_c.r < 0f)
		{
			_c.r = 0f;
		}
		if (_c.r > 1f)
		{
			_c.r = 1f;
		}
		if (_c.g < 0f)
		{
			_c.g = 0f;
		}
		if (_c.g > 1f)
		{
			_c.g = 1f;
		}
		if (_c.b < 0f)
		{
			_c.b = 0f;
		}
		if (_c.b > 1f)
		{
			_c.b = 1f;
		}
		return _c;
	}

	// Token: 0x0600A836 RID: 43062 RVA: 0x003E8D54 File Offset: 0x003E6F54
	public static ushort ToColor5(Color col)
	{
		return (ushort)(((int)(col.r * 31f + 0.5f) << 10) | ((int)(col.g * 31f + 0.5f) << 5) | (int)(col.b * 31f + 0.5f));
	}

	// Token: 0x0600A837 RID: 43063 RVA: 0x003E8DA2 File Offset: 0x003E6FA2
	public static Color FromColor5(ushort col)
	{
		return new Color((float)((col >> 10) & 31) / 31f, (float)((col >> 5) & 31) / 31f, (float)(col & 31) / 31f);
	}

	// Token: 0x0600A838 RID: 43064 RVA: 0x003E8DD0 File Offset: 0x003E6FD0
	public static Color32 FromColor5To32(ushort col)
	{
		return new Color32((byte)((float)((col >> 10) & 31) / 31f * 255f), (byte)((float)((col >> 5) & 31) / 31f * 255f), (byte)((float)(col & 31) / 31f * 255f), byte.MaxValue);
	}

	// Token: 0x0600A839 RID: 43065 RVA: 0x003E8E24 File Offset: 0x003E7024
	public static GameRandom RandomFromSeedOnPos(int x, int y, int seed)
	{
		GameRandom gameRandom = GameRandomManager.Instance.CreateGameRandom(seed);
		long num = (long)gameRandom.RandomInt / 2L * 2L + 1L;
		long num2 = (long)gameRandom.RandomInt / 2L * 2L + 1L;
		int num3 = (int)(((long)((long)x << 4) * num + (long)((long)y << 4) * num2) ^ (long)seed);
		gameRandom.SetSeed(num3);
		return gameRandom;
	}

	// Token: 0x0600A83A RID: 43066 RVA: 0x003E8E78 File Offset: 0x003E7078
	public static GameRandom RandomFromSeedOnPos(int _x, int _y, int _z, int _seed)
	{
		int num = _seed + _x + (_z << 14) + (_y << 24);
		return GameRandomManager.Instance.CreateGameRandom(num);
	}

	// Token: 0x0600A83B RID: 43067 RVA: 0x003E8EA0 File Offset: 0x003E70A0
	public static string DescribeTimeSince(DateTime _newer, DateTime _older)
	{
		TimeSpan timeSpan = _newer - _older;
		if (timeSpan.TotalMinutes < 1.0)
		{
			return Localization.Get("xuiLastSeenNow", false, null);
		}
		if (timeSpan.TotalMinutes < 60.0)
		{
			int num = (int)timeSpan.TotalMinutes;
			if (num != 1)
			{
				return string.Format(Localization.Get("xuiLastSeenMinutes", false, null), num);
			}
			return Localization.Get("xuiLastSeen1Minute", false, null);
		}
		else if (timeSpan.TotalHours < 24.0)
		{
			int num2 = (int)timeSpan.TotalHours;
			if (num2 != 1)
			{
				return string.Format(Localization.Get("xuiLastSeenHours", false, null), num2);
			}
			return Localization.Get("xuiLastSeen1Hour", false, null);
		}
		else
		{
			int num3 = (int)timeSpan.TotalDays;
			if (num3 != 1)
			{
				return string.Format(Localization.Get("xuiLastSeenDays", false, null), num3);
			}
			return Localization.Get("xuiLastSeen1Day", false, null);
		}
	}

	// Token: 0x0600A83C RID: 43068 RVA: 0x003E8F8D File Offset: 0x003E718D
	public static float ToCelsius(float fromFahrenheit)
	{
		return (fromFahrenheit - 32f) * 0.5555556f;
	}

	// Token: 0x0600A83D RID: 43069 RVA: 0x003E8F9C File Offset: 0x003E719C
	public static float ToRelativeCelsius(float fromFahrenheit)
	{
		return fromFahrenheit * 0.5555556f;
	}

	// Token: 0x0600A83E RID: 43070 RVA: 0x003E8FA8 File Offset: 0x003E71A8
	public static string EscapeBbCodes(string _input, bool _removeInstead = false, bool _allowUrls = false)
	{
		if (string.IsNullOrEmpty(_input))
		{
			return _input;
		}
		StringBuilder stringBuilder = new StringBuilder();
		int i = 0;
		int num = 0;
		while (i < _input.Length)
		{
			ValueTuple<int, int, bool> valueTuple = Utils.FindNextBbCode(_input, i, _allowUrls);
			int item = valueTuple.Item1;
			int item2 = valueTuple.Item2;
			if (item == -1)
			{
				stringBuilder.Append(_input.Substring(num));
				break;
			}
			stringBuilder.Append(_input.Substring(num, item - num));
			if (!_removeInstead)
			{
				int num2 = 0;
				while (_input[item + num2] == '[')
				{
					num2++;
					stringBuilder.Append("[");
				}
				stringBuilder.Append("[/c]");
				stringBuilder.Append(_input.Substring(item + num2, item2 - num2));
			}
			i = item + item2;
			num = i;
		}
		return stringBuilder.ToString();
	}

	// Token: 0x0600A83F RID: 43071 RVA: 0x003E906C File Offset: 0x003E726C
	public static ValueTuple<int, int, bool> FindNextBbCode(string _input, int _startIndex, bool _allowUrls = false)
	{
		int i = _startIndex;
		int length = _input.Length;
		while (i < length)
		{
			if (_input[i] != '[')
			{
				i++;
			}
			else
			{
				Match match = Utils.nestedEscapePattern.Match(_input.Substring(i, length - i));
				if (match.Success)
				{
					int index = match.Index;
					if (index == 0)
					{
						return new ValueTuple<int, int, bool>(i, match.Length, true);
					}
					if (index == 1)
					{
						int num = _input.IndexOf(']', i + match.Length);
						if (num == i + match.Length)
						{
							return new ValueTuple<int, int, bool>(i, num + 1 - i, true);
						}
						if (num == -1)
						{
							i += 4;
							continue;
						}
						string text = "[" + _input.Substring(i + match.Length, num - (i + match.Length)) + "]";
						ValueTuple<int, int, bool> valueTuple = Utils.FindNextBbCode(text, 0, false);
						int item = valueTuple.Item1;
						int item2 = valueTuple.Item2;
						if (item == 0 && item2 == text.Length)
						{
							return new ValueTuple<int, int, bool>(i, num + 1 - i, true);
						}
					}
				}
				if (i + 3 > length || _input[i] != '[')
				{
					i++;
				}
				else
				{
					if (_input[i + 2] == ']')
					{
						char c = _input[i + 1];
						if (c <= 'I')
						{
							if (c <= 'B')
							{
								if (c != '-' && c != 'B')
								{
									goto IL_01A8;
								}
							}
							else if (c != 'C' && c != 'I')
							{
								goto IL_01A8;
							}
						}
						else if (c <= 'b')
						{
							switch (c)
							{
							case 'S':
							case 'T':
							case 'U':
								break;
							default:
								if (c != 'b')
								{
									goto IL_01A8;
								}
								break;
							}
						}
						else if (c != 'c' && c != 'i')
						{
							switch (c)
							{
							case 's':
							case 't':
							case 'u':
								break;
							default:
								goto IL_01A8;
							}
						}
						return new ValueTuple<int, int, bool>(i, 3, false);
					}
					IL_01A8:
					if (i + 4 <= length && _input[i + 1] == '/' && _input[i + 3] == ']')
					{
						char c = _input[i + 2];
						if (c <= 'I')
						{
							if (c <= 'B')
							{
								if (c != '-' && c != 'B')
								{
									goto IL_0255;
								}
							}
							else if (c != 'C' && c != 'I')
							{
								goto IL_0255;
							}
						}
						else if (c <= 'b')
						{
							switch (c)
							{
							case 'S':
							case 'T':
							case 'U':
								break;
							default:
								if (c != 'b')
								{
									goto IL_0255;
								}
								break;
							}
						}
						else if (c != 'c' && c != 'i')
						{
							switch (c)
							{
							case 's':
							case 't':
							case 'u':
								break;
							default:
								goto IL_0255;
							}
						}
						return new ValueTuple<int, int, bool>(i, 4, false);
					}
					IL_0255:
					if (i + 4 <= length && _input[i + 3] == ']' && Utils.hexAlphaMatcher.IsMatch(_input.Substring(i + 1, 2)))
					{
						return new ValueTuple<int, int, bool>(i, 4, false);
					}
					if (i + 5 <= length && _input[i + 4] == ']' && _input[i + 1] == 's' && _input[i + 2] == 'u' && (_input[i + 3] == 'b' || _input[i + 3] == 'p'))
					{
						return new ValueTuple<int, int, bool>(i, 5, false);
					}
					if (i + 6 <= length && _input[i + 5] == ']')
					{
						string text2 = _input.Substring(i, 6);
						if (text2.EqualsCaseInsensitive("[/sub]") || text2.EqualsCaseInsensitive("[/sup]"))
						{
							return new ValueTuple<int, int, bool>(i, 6, false);
						}
					}
					if (i + 5 < length && _input.Substring(i, 5).EqualsCaseInsensitive("[url="))
					{
						int num2 = _input.IndexOf(']', i + 5);
						if (num2 != -1)
						{
							if (_allowUrls)
							{
								i = num2;
								continue;
							}
							return new ValueTuple<int, int, bool>(i, num2 - i + 1, false);
						}
					}
					if (!_allowUrls && i + 6 <= length && _input.Substring(i, 6).EqualsCaseInsensitive("[/url]"))
					{
						return new ValueTuple<int, int, bool>(i, 6, false);
					}
					if (i + 8 <= length && _input[i + 7] == ']' && Utils.hexRgbMatcher.IsMatch(_input.Substring(i + 1, 6)))
					{
						return new ValueTuple<int, int, bool>(i, 8, false);
					}
					if (i + 10 <= length && _input[i + 9] == ']' && Utils.hexRgbaMatcher.IsMatch(_input.Substring(i + 1, 8)))
					{
						return new ValueTuple<int, int, bool>(i, 10, false);
					}
					i++;
				}
			}
		}
		return new ValueTuple<int, int, bool>(-1, 0, false);
	}

	// Token: 0x0600A840 RID: 43072 RVA: 0x003E9478 File Offset: 0x003E7678
	public static string GetVisibileTextWithBbCodes(string _input)
	{
		int i = 0;
		StringBuilder stringBuilder = new StringBuilder();
		while (i < _input.Length)
		{
			ValueTuple<int, int, bool> valueTuple = Utils.FindNextBbCode(_input, i, false);
			int item = valueTuple.Item1;
			int item2 = valueTuple.Item2;
			bool item3 = valueTuple.Item3;
			if (item == -1)
			{
				stringBuilder.Append(_input, i, _input.Length - i);
				break;
			}
			stringBuilder.Append(_input, i, item - i);
			if (item3)
			{
				string value = Utils.nestedEscapePattern.Match(_input.Substring(i)).Value;
				value.Replace("[/c]", "");
				stringBuilder.Append(value);
				stringBuilder.Append(_input, value.Length, item2 - value.Length);
			}
			i = item + item2;
		}
		return stringBuilder.ToString();
	}

	// Token: 0x0600A841 RID: 43073 RVA: 0x003E9537 File Offset: 0x003E7737
	public static string CreateGameMessage(string _senderName, string _message)
	{
		if (string.IsNullOrEmpty(_senderName))
		{
			return _message;
		}
		return _senderName + ": " + _message;
	}

	// Token: 0x0600A842 RID: 43074 RVA: 0x003E9550 File Offset: 0x003E7750
	public static string GetCancellationMessage()
	{
		if (PlatformManager.NativePlatform.Input.CurrentInputStyle == PlayerInputManager.InputStyle.Keyboard)
		{
			return Localization.Get("msgConnectingToServerCancel", false, null);
		}
		return string.Format(Localization.Get("msgConnectingToServerCancelTemplate", false, null), LocalPlayerUI.primaryUI.playerInput.PermanentActions.Cancel.GetBindingString(true, PlatformManager.NativePlatform.Input.CurrentControllerInputStyle, XUiUtils.EmptyBindingStyle.LocalizedNone, XUiUtils.DisplayStyle.Plain, false, null));
	}

	// Token: 0x0600A843 RID: 43075 RVA: 0x003E95BC File Offset: 0x003E77BC
	public static void Fill<T>(this T[] array, T value)
	{
		int num = array.Length;
		for (int i = 0; i < num; i++)
		{
			array[i] = value;
		}
	}

	// Token: 0x0600A844 RID: 43076 RVA: 0x003E95E4 File Offset: 0x003E77E4
	public static void Memset(byte[] array, byte what, int length)
	{
		GCHandle gchandle = GCHandle.Alloc(array, GCHandleType.Pinned);
		Utils.MemsetDelegate(gchandle.AddrOfPinnedObject(), what, length);
		gchandle.Free();
	}

	// Token: 0x0600A845 RID: 43077 RVA: 0x003E9614 File Offset: 0x003E7814
	public static void CalculateMeshTangents(List<Vector3> _vertices, List<int> _indices, List<Vector3> _normals, List<Vector2> _uvs, List<Vector4> _tangents, Mesh mesh, bool _bIgnoreUvs = false)
	{
		int count = _vertices.Count;
		if (count > 786432)
		{
			return;
		}
		Array.Clear(Utils.tan1Temp, 0, count);
		Array.Clear(Utils.tan2Temp, 0, count);
		int count2 = _indices.Count;
		for (int i = 0; i < count2; i += 3)
		{
			int num = _indices[i];
			int num2 = _indices[i + 1];
			int num3 = _indices[i + 2];
			Vector3 vector = _vertices[num];
			Vector3 vector2 = _vertices[num2];
			Vector3 vector3 = _vertices[num3];
			Vector2 vector4;
			Vector2 vector5;
			Vector2 vector6;
			if (_bIgnoreUvs)
			{
				vector4 = Vector2.zero;
				vector5 = Vector2.zero;
				vector6 = Vector2.zero;
			}
			else
			{
				vector4 = _uvs[num];
				vector5 = _uvs[num2];
				vector6 = _uvs[num3];
			}
			float num4 = vector2.x - vector.x;
			float num5 = vector3.x - vector.x;
			float num6 = vector2.y - vector.y;
			float num7 = vector3.y - vector.y;
			float num8 = vector2.z - vector.z;
			float num9 = vector3.z - vector.z;
			float num10 = vector5.x - vector4.x;
			float num11 = vector6.x - vector4.x;
			float num12 = vector5.y - vector4.y;
			float num13 = vector6.y - vector4.y;
			float num14 = num10 * num13 - num11 * num12;
			float num15 = ((num14 == 0f) ? 0f : (1f / num14));
			float num16 = (num13 * num4 - num12 * num5) * num15;
			float num17 = (num13 * num6 - num12 * num7) * num15;
			float num18 = (num13 * num8 - num12 * num9) * num15;
			float num19 = (num10 * num5 - num11 * num4) * num15;
			float num20 = (num10 * num7 - num11 * num6) * num15;
			float num21 = (num10 * num9 - num11 * num8) * num15;
			Vector3[] array = Utils.tan1Temp;
			int num22 = num;
			array[num22].x = array[num22].x + num16;
			Vector3[] array2 = Utils.tan1Temp;
			int num23 = num;
			array2[num23].y = array2[num23].y + num17;
			Vector3[] array3 = Utils.tan1Temp;
			int num24 = num;
			array3[num24].z = array3[num24].z + num18;
			Vector3[] array4 = Utils.tan1Temp;
			int num25 = num2;
			array4[num25].x = array4[num25].x + num16;
			Vector3[] array5 = Utils.tan1Temp;
			int num26 = num2;
			array5[num26].y = array5[num26].y + num17;
			Vector3[] array6 = Utils.tan1Temp;
			int num27 = num2;
			array6[num27].z = array6[num27].z + num18;
			Vector3[] array7 = Utils.tan1Temp;
			int num28 = num3;
			array7[num28].x = array7[num28].x + num16;
			Vector3[] array8 = Utils.tan1Temp;
			int num29 = num3;
			array8[num29].y = array8[num29].y + num17;
			Vector3[] array9 = Utils.tan1Temp;
			int num30 = num3;
			array9[num30].z = array9[num30].z + num18;
			Vector3[] array10 = Utils.tan2Temp;
			int num31 = num;
			array10[num31].x = array10[num31].x + num19;
			Vector3[] array11 = Utils.tan2Temp;
			int num32 = num;
			array11[num32].y = array11[num32].y + num20;
			Vector3[] array12 = Utils.tan2Temp;
			int num33 = num;
			array12[num33].z = array12[num33].z + num21;
			Vector3[] array13 = Utils.tan2Temp;
			int num34 = num2;
			array13[num34].x = array13[num34].x + num19;
			Vector3[] array14 = Utils.tan2Temp;
			int num35 = num2;
			array14[num35].y = array14[num35].y + num20;
			Vector3[] array15 = Utils.tan2Temp;
			int num36 = num2;
			array15[num36].z = array15[num36].z + num21;
			Vector3[] array16 = Utils.tan2Temp;
			int num37 = num3;
			array16[num37].x = array16[num37].x + num19;
			Vector3[] array17 = Utils.tan2Temp;
			int num38 = num3;
			array17[num38].y = array17[num38].y + num20;
			Vector3[] array18 = Utils.tan2Temp;
			int num39 = num3;
			array18[num39].z = array18[num39].z + num21;
		}
		if (_normals.Count == 0)
		{
			Utils.tempNormals.Clear();
			mesh.GetNormals(Utils.tempNormals);
		}
		for (int j = 0; j < count; j++)
		{
			Vector3 vector7 = ((_normals.Count > 0) ? _normals[j] : Utils.tempNormals[j]);
			Vector3 vector8 = Utils.tan1Temp[j];
			Vector3.OrthoNormalize(ref vector7, ref vector8);
			Vector4 vector9 = new Vector4(vector8.x, vector8.y, vector8.z, (Vector3.Dot(Vector3.Cross(vector7, vector8), Utils.tan2Temp[j]) < 0f) ? (-1f) : 1f);
			_tangents.Add(vector9);
		}
	}

	// Token: 0x0600A846 RID: 43078 RVA: 0x003E9A58 File Offset: 0x003E7C58
	public static void CalculateMeshTangents(ArrayListMP<Vector3> _vertices, ArrayListMP<int> _indices, ArrayListMP<Vector3> _normals, ArrayListMP<Vector2> _uvs, ArrayListMP<Vector4> _tangents, Mesh _mesh, bool _bIgnoreUvs = false)
	{
		int count = _vertices.Count;
		if (count > 786432)
		{
			return;
		}
		Array.Clear(Utils.tan1Temp, 0, count);
		Array.Clear(Utils.tan2Temp, 0, count);
		int count2 = _indices.Count;
		for (int i = 0; i < count2; i += 3)
		{
			int num = _indices[i];
			int num2 = _indices[i + 1];
			int num3 = _indices[i + 2];
			Vector3 vector = _vertices[num];
			Vector3 vector2 = _vertices[num2];
			Vector3 vector3 = _vertices[num3];
			Vector2 vector4;
			Vector2 vector5;
			Vector2 vector6;
			if (_bIgnoreUvs)
			{
				vector4 = Vector2.zero;
				vector5 = Vector2.zero;
				vector6 = Vector2.zero;
			}
			else
			{
				vector4 = _uvs[num];
				vector5 = _uvs[num2];
				vector6 = _uvs[num3];
			}
			float num4 = vector2.x - vector.x;
			float num5 = vector3.x - vector.x;
			float num6 = vector2.y - vector.y;
			float num7 = vector3.y - vector.y;
			float num8 = vector2.z - vector.z;
			float num9 = vector3.z - vector.z;
			float num10 = vector5.x - vector4.x;
			float num11 = vector6.x - vector4.x;
			float num12 = vector5.y - vector4.y;
			float num13 = vector6.y - vector4.y;
			float num14 = num10 * num13 - num11 * num12;
			float num15 = ((num14 == 0f) ? 0f : (1f / num14));
			float num16 = (num13 * num4 - num12 * num5) * num15;
			float num17 = (num13 * num6 - num12 * num7) * num15;
			float num18 = (num13 * num8 - num12 * num9) * num15;
			float num19 = (num10 * num5 - num11 * num4) * num15;
			float num20 = (num10 * num7 - num11 * num6) * num15;
			float num21 = (num10 * num9 - num11 * num8) * num15;
			Vector3[] array = Utils.tan1Temp;
			int num22 = num;
			array[num22].x = array[num22].x + num16;
			Vector3[] array2 = Utils.tan1Temp;
			int num23 = num;
			array2[num23].y = array2[num23].y + num17;
			Vector3[] array3 = Utils.tan1Temp;
			int num24 = num;
			array3[num24].z = array3[num24].z + num18;
			Vector3[] array4 = Utils.tan1Temp;
			int num25 = num2;
			array4[num25].x = array4[num25].x + num16;
			Vector3[] array5 = Utils.tan1Temp;
			int num26 = num2;
			array5[num26].y = array5[num26].y + num17;
			Vector3[] array6 = Utils.tan1Temp;
			int num27 = num2;
			array6[num27].z = array6[num27].z + num18;
			Vector3[] array7 = Utils.tan1Temp;
			int num28 = num3;
			array7[num28].x = array7[num28].x + num16;
			Vector3[] array8 = Utils.tan1Temp;
			int num29 = num3;
			array8[num29].y = array8[num29].y + num17;
			Vector3[] array9 = Utils.tan1Temp;
			int num30 = num3;
			array9[num30].z = array9[num30].z + num18;
			Vector3[] array10 = Utils.tan2Temp;
			int num31 = num;
			array10[num31].x = array10[num31].x + num19;
			Vector3[] array11 = Utils.tan2Temp;
			int num32 = num;
			array11[num32].y = array11[num32].y + num20;
			Vector3[] array12 = Utils.tan2Temp;
			int num33 = num;
			array12[num33].z = array12[num33].z + num21;
			Vector3[] array13 = Utils.tan2Temp;
			int num34 = num2;
			array13[num34].x = array13[num34].x + num19;
			Vector3[] array14 = Utils.tan2Temp;
			int num35 = num2;
			array14[num35].y = array14[num35].y + num20;
			Vector3[] array15 = Utils.tan2Temp;
			int num36 = num2;
			array15[num36].z = array15[num36].z + num21;
			Vector3[] array16 = Utils.tan2Temp;
			int num37 = num3;
			array16[num37].x = array16[num37].x + num19;
			Vector3[] array17 = Utils.tan2Temp;
			int num38 = num3;
			array17[num38].y = array17[num38].y + num20;
			Vector3[] array18 = Utils.tan2Temp;
			int num39 = num3;
			array18[num39].z = array18[num39].z + num21;
		}
		if (_normals.Count == 0)
		{
			Utils.tempNormals.Clear();
			_mesh.GetNormals(Utils.tempNormals);
		}
		for (int j = 0; j < count; j++)
		{
			Vector3 vector7 = ((_normals.Count > 0) ? _normals[j] : Utils.tempNormals[j]);
			Vector3 vector8 = Utils.tan1Temp[j];
			Vector3.OrthoNormalize(ref vector7, ref vector8);
			Vector4 vector9 = new Vector4(vector8.x, vector8.y, vector8.z, (Vector3.Dot(Vector3.Cross(vector7, vector8), Utils.tan2Temp[j]) < 0f) ? (-1f) : 1f);
			_tangents.Add(vector9);
		}
	}

	// Token: 0x0600A847 RID: 43079 RVA: 0x003E9E9C File Offset: 0x003E809C
	public static void CalculateMeshTangents(VoxelMesh _vm, bool _bIgnoreUvs = false)
	{
		int count = _vm.m_Vertices.Count;
		if (count > 786432)
		{
			return;
		}
		Array.Clear(Utils.tan1Temp, 0, count);
		Array.Clear(Utils.tan2Temp, 0, count);
		Vector2 vector = Vector2.zero;
		Vector2 vector2 = Vector2.zero;
		Vector2 vector3 = Vector2.zero;
		int count2 = _vm.m_Indices.Count;
		for (int i = 0; i < count2; i += 3)
		{
			int num = _vm.m_Indices[i];
			int num2 = _vm.m_Indices[i + 1];
			int num3 = _vm.m_Indices[i + 2];
			Vector3 vector4 = _vm.m_Vertices[num];
			Vector3 vector5 = _vm.m_Vertices[num2];
			Vector3 vector6 = _vm.m_Vertices[num3];
			if (!_bIgnoreUvs)
			{
				vector = _vm.m_Uvs[num];
				vector2 = _vm.m_Uvs[num2];
				vector3 = _vm.m_Uvs[num3];
			}
			float num4 = vector5.x - vector4.x;
			float num5 = vector6.x - vector4.x;
			float num6 = vector5.y - vector4.y;
			float num7 = vector6.y - vector4.y;
			float num8 = vector5.z - vector4.z;
			float num9 = vector6.z - vector4.z;
			float num10 = vector2.x - vector.x;
			float num11 = vector3.x - vector.x;
			float num12 = vector2.y - vector.y;
			float num13 = vector3.y - vector.y;
			float num14 = num10 * num13 - num11 * num12;
			float num15 = ((num14 == 0f) ? 0f : (1f / num14));
			float num16 = (num13 * num4 - num12 * num5) * num15;
			float num17 = (num13 * num6 - num12 * num7) * num15;
			float num18 = (num13 * num8 - num12 * num9) * num15;
			float num19 = (num10 * num5 - num11 * num4) * num15;
			float num20 = (num10 * num7 - num11 * num6) * num15;
			float num21 = (num10 * num9 - num11 * num8) * num15;
			Vector3[] array = Utils.tan1Temp;
			int num22 = num;
			array[num22].x = array[num22].x + num16;
			Vector3[] array2 = Utils.tan1Temp;
			int num23 = num;
			array2[num23].y = array2[num23].y + num17;
			Vector3[] array3 = Utils.tan1Temp;
			int num24 = num;
			array3[num24].z = array3[num24].z + num18;
			Vector3[] array4 = Utils.tan1Temp;
			int num25 = num2;
			array4[num25].x = array4[num25].x + num16;
			Vector3[] array5 = Utils.tan1Temp;
			int num26 = num2;
			array5[num26].y = array5[num26].y + num17;
			Vector3[] array6 = Utils.tan1Temp;
			int num27 = num2;
			array6[num27].z = array6[num27].z + num18;
			Vector3[] array7 = Utils.tan1Temp;
			int num28 = num3;
			array7[num28].x = array7[num28].x + num16;
			Vector3[] array8 = Utils.tan1Temp;
			int num29 = num3;
			array8[num29].y = array8[num29].y + num17;
			Vector3[] array9 = Utils.tan1Temp;
			int num30 = num3;
			array9[num30].z = array9[num30].z + num18;
			Vector3[] array10 = Utils.tan2Temp;
			int num31 = num;
			array10[num31].x = array10[num31].x + num19;
			Vector3[] array11 = Utils.tan2Temp;
			int num32 = num;
			array11[num32].y = array11[num32].y + num20;
			Vector3[] array12 = Utils.tan2Temp;
			int num33 = num;
			array12[num33].z = array12[num33].z + num21;
			Vector3[] array13 = Utils.tan2Temp;
			int num34 = num2;
			array13[num34].x = array13[num34].x + num19;
			Vector3[] array14 = Utils.tan2Temp;
			int num35 = num2;
			array14[num35].y = array14[num35].y + num20;
			Vector3[] array15 = Utils.tan2Temp;
			int num36 = num2;
			array15[num36].z = array15[num36].z + num21;
			Vector3[] array16 = Utils.tan2Temp;
			int num37 = num3;
			array16[num37].x = array16[num37].x + num19;
			Vector3[] array17 = Utils.tan2Temp;
			int num38 = num3;
			array17[num38].y = array17[num38].y + num20;
			Vector3[] array18 = Utils.tan2Temp;
			int num39 = num3;
			array18[num39].z = array18[num39].z + num21;
		}
		Vector3[] items = _vm.m_Normals.Items;
		int num40 = _vm.m_Tangents.Alloc(count);
		Vector4[] items2 = _vm.m_Tangents.Items;
		for (int j = 0; j < count; j++)
		{
			Vector3 vector7 = items[j];
			Vector3 vector8 = Utils.tan1Temp[j];
			Vector3.OrthoNormalize(ref vector7, ref vector8);
			Vector4 vector9;
			vector9.x = vector8.x;
			vector9.y = vector8.y;
			vector9.z = vector8.z;
			vector9.w = (float)((Vector3.Dot(Vector3.Cross(vector7, vector8), Utils.tan2Temp[j]) < 0f) ? (-1) : 1);
			items2[num40++] = vector9;
		}
	}

	// Token: 0x0600A848 RID: 43080 RVA: 0x003EA320 File Offset: 0x003E8520
	public unsafe static void CalculateMeshTangents(ReadOnlySpan<Vector3> _vertices, ReadOnlySpan<int> _indices, ReadOnlySpan<Vector3> _normals, ReadOnlySpan<Vector2> _uvs, Span<Vector4> _tangents, bool _bIgnoreUvs = false)
	{
		int length = _vertices.Length;
		if (length > 786432)
		{
			return;
		}
		NativeArray<Vector3> nativeArray = new NativeArray<Vector3>(length, Allocator.TempJob, NativeArrayOptions.ClearMemory);
		try
		{
			NativeArray<Vector3> nativeArray2 = new NativeArray<Vector3>(length, Allocator.TempJob, NativeArrayOptions.ClearMemory);
			try
			{
				Span<Vector3> span = in nativeArray;
				Span<Vector3> span2 = in nativeArray2;
				int length2 = _indices.Length;
				for (int i = 0; i < length2; i += 3)
				{
					int num = *_indices[i];
					int num2 = *_indices[i + 1];
					int num3 = *_indices[i + 2];
					Vector3 vector = *_vertices[num];
					Vector3 vector2 = *_vertices[num2];
					Vector3 vector3 = *_vertices[num3];
					Vector2 vector4;
					Vector2 vector5;
					Vector2 vector6;
					if (_bIgnoreUvs)
					{
						vector4 = Vector2.zero;
						vector5 = Vector2.zero;
						vector6 = Vector2.zero;
					}
					else
					{
						vector4 = *_uvs[num];
						vector5 = *_uvs[num2];
						vector6 = *_uvs[num3];
					}
					float num4 = vector2.x - vector.x;
					float num5 = vector3.x - vector.x;
					float num6 = vector2.y - vector.y;
					float num7 = vector3.y - vector.y;
					float num8 = vector2.z - vector.z;
					float num9 = vector3.z - vector.z;
					float num10 = vector5.x - vector4.x;
					float num11 = vector6.x - vector4.x;
					float num12 = vector5.y - vector4.y;
					float num13 = vector6.y - vector4.y;
					float num14 = num10 * num13 - num11 * num12;
					float num15 = ((num14 == 0f) ? 0f : (1f / num14));
					float num16 = (num13 * num4 - num12 * num5) * num15;
					float num17 = (num13 * num6 - num12 * num7) * num15;
					float num18 = (num13 * num8 - num12 * num9) * num15;
					float num19 = (num10 * num5 - num11 * num4) * num15;
					float num20 = (num10 * num7 - num11 * num6) * num15;
					float num21 = (num10 * num9 - num11 * num8) * num15;
					span[num].x += num16;
					span[num].y += num17;
					span[num].z += num18;
					span[num2].x += num16;
					span[num2].y += num17;
					span[num2].z += num18;
					span[num3].x += num16;
					span[num3].y += num17;
					span[num3].z += num18;
					span2[num].x += num19;
					span2[num].y += num20;
					span2[num].z += num21;
					span2[num2].x += num19;
					span2[num2].y += num20;
					span2[num2].z += num21;
					span2[num3].x += num19;
					span2[num3].y += num20;
					span2[num3].z += num21;
				}
				for (int j = 0; j < length; j++)
				{
					Vector3 vector7 = *_normals[j];
					Vector3 vector8 = *span[j];
					Vector3.OrthoNormalize(ref vector7, ref vector8);
					Vector4 vector9 = new Vector4(vector8.x, vector8.y, vector8.z, (Vector3.Dot(Vector3.Cross(vector7, vector8), *span2[j]) < 0f) ? (-1f) : 1f);
					*_tangents[j] = vector9;
				}
			}
			finally
			{
				((IDisposable)nativeArray2).Dispose();
			}
		}
		finally
		{
			((IDisposable)nativeArray).Dispose();
		}
	}

	// Token: 0x0600A849 RID: 43081 RVA: 0x003EA7A4 File Offset: 0x003E89A4
	public static int WrapInt(int value, int min, int max)
	{
		int num = max - min;
		while (value < min)
		{
			value += num;
		}
		while (value > max)
		{
			value -= num;
		}
		return value;
	}

	// Token: 0x0600A84A RID: 43082 RVA: 0x003EA7CC File Offset: 0x003E89CC
	public static int WrapIndex(int index, int arraySize)
	{
		while (index < 0)
		{
			index += arraySize;
		}
		while (index >= arraySize)
		{
			index -= arraySize;
		}
		return index;
	}

	// Token: 0x0600A84B RID: 43083 RVA: 0x003EA7E8 File Offset: 0x003E89E8
	public static float WrapFloat(float value, float min, float max)
	{
		float num = max - min;
		if (value < min)
		{
			value += num;
		}
		else if (value > max)
		{
			value -= num;
		}
		return value;
	}

	// Token: 0x0600A84C RID: 43084 RVA: 0x003EA810 File Offset: 0x003E8A10
	public static void CleanupMaterialsOfRenderers<T>(T renderers) where T : IList<Renderer>
	{
		for (int i = 0; i < renderers.Count; i++)
		{
			renderers[i].GetSharedMaterials(Utils.tempMats);
			Utils.CleanupMaterials<List<Material>>(Utils.tempMats);
		}
		Utils.tempMats.Clear();
	}

	// Token: 0x0600A84D RID: 43085 RVA: 0x003EA861 File Offset: 0x003E8A61
	public static void CleanupMaterialsOfRenderer(Renderer renderer)
	{
		Utils.CleanupMaterials<Material[]>(renderer.sharedMaterials);
	}

	// Token: 0x0600A84E RID: 43086 RVA: 0x003EA870 File Offset: 0x003E8A70
	public static void CleanupMaterials<T>(T mats) where T : IList<Material>
	{
		for (int i = 0; i < mats.Count; i++)
		{
			Material material = mats[i];
			if (material && material.GetInstanceID() < 0)
			{
				global::UnityEngine.Object.Destroy(material);
			}
		}
	}

	// Token: 0x0600A84F RID: 43087 RVA: 0x003EA8BB File Offset: 0x003E8ABB
	public static void MarkMaterialAsSafeForManualCleanup(Material mat)
	{
		mat.name += " (Instance)";
	}

	// Token: 0x0600A850 RID: 43088 RVA: 0x003EA8D4 File Offset: 0x003E8AD4
	public static void RestartGame(Utils.ERestartAntiCheatMode _antiCheatMode = Utils.ERestartAntiCheatMode.KeepAntiCheatMode)
	{
		Regex regex = new Regex("^(.*)(output_log_client__)(\\d{4}-\\d{2}-\\d{2}__\\d{2}-\\d{2}-\\d{2})(\\.txt)$");
		Regex regex2 = new Regex("^(.*)(output_log__)(\\d{4}-\\d{2}-\\d{2}__\\d{2}-\\d{2}-\\d{2})(\\.txt)$", RegexOptions.IgnoreCase);
		string[] commandLineArgs = GameStartupHelper.GetCommandLineArgs();
		string text = commandLineArgs[0];
		bool flag;
		if (_antiCheatMode != Utils.ERestartAntiCheatMode.ForceOn)
		{
			if (_antiCheatMode == Utils.ERestartAntiCheatMode.KeepAntiCheatMode)
			{
				IAntiCheatClient antiCheatClient = PlatformManager.MultiPlatform.AntiCheatClient;
				flag = antiCheatClient != null && antiCheatClient.ClientAntiCheatEnabled();
			}
			else
			{
				flag = false;
			}
		}
		else
		{
			flag = true;
		}
		bool flag2 = flag;
		if (flag2)
		{
			RuntimePlatform platform = Application.platform;
			if (platform > RuntimePlatform.WindowsPlayer)
			{
				if (platform != RuntimePlatform.LinuxPlayer)
				{
					switch (platform)
					{
					case RuntimePlatform.LinuxServer:
						break;
					case RuntimePlatform.WindowsServer:
						goto IL_008C;
					case RuntimePlatform.OSXServer:
						goto IL_00EA;
					default:
						goto IL_0119;
					}
				}
				text = Path.GetDirectoryName(text);
				text = ((text.Length > 0) ? (text + "/") : "") + "7DaysToDie_EAC";
				goto IL_0133;
			}
			if (platform == RuntimePlatform.OSXPlayer)
			{
				goto IL_00EA;
			}
			if (platform != RuntimePlatform.WindowsPlayer)
			{
				goto IL_0119;
			}
			IL_008C:
			text = Path.GetDirectoryName(text);
			text = ((text.Length > 0) ? (text + "/") : "") + "7DaysToDie_EAC.exe";
			goto IL_0133;
			IL_00EA:
			text = Path.GetDirectoryName(text);
			text = ((text.Length > 0) ? (text + "/") : "") + "7DaysToDie_EAC";
			goto IL_0133;
			IL_0119:
			Log.Error(string.Format("Restarting the game not supported on this platform ({0})", Application.platform));
			return;
		}
		IL_0133:
		StringBuilder stringBuilder = new StringBuilder();
		if (!flag2)
		{
			stringBuilder.Append("-noeac");
		}
		for (int i = 1; i < commandLineArgs.Length; i++)
		{
			if (!flag2 || i > 1)
			{
				stringBuilder.Append(' ');
			}
			string text2 = commandLineArgs[i];
			if (text2.EqualsCaseInsensitive("-logfile"))
			{
				stringBuilder.Append(text2);
				stringBuilder.Append(' ');
				i++;
				text2 = commandLineArgs[i];
				Match match;
				if ((match = regex.Match(text2)).Success)
				{
					string text3 = DateTime.Now.ToString("yyyy-MM-dd__HH-mm-ss");
					text2 = match.Groups[1].Value + match.Groups[2].Value + text3 + match.Groups[4].Value;
				}
				else if ((match = regex2.Match(text2)).Success)
				{
					string text4 = DateTime.Now.ToString("yyyy-MM-dd__HH-mm-ss");
					text2 = match.Groups[1].Value + match.Groups[2].Value + text4 + match.Groups[4].Value;
				}
				else
				{
					text2 += ".restarted.txt";
				}
			}
			if (text2.IndexOf(' ') >= 0)
			{
				stringBuilder.Append('"');
				stringBuilder.Append(text2);
				stringBuilder.Append('"');
			}
			else
			{
				stringBuilder.Append(text2);
			}
		}
		stringBuilder.Append(" -skipintro");
		stringBuilder.Append(" -skipnewsscreen=true");
		Process.Start(new ProcessStartInfo(text)
		{
			UseShellExecute = true,
			WorkingDirectory = SdDirectory.GetCurrentDirectory(),
			Arguments = stringBuilder.ToString()
		});
		Log.Out("Restarting game: " + text + " " + stringBuilder.ToString());
		Application.Quit();
	}

	// Token: 0x0600A851 RID: 43089 RVA: 0x003EAC08 File Offset: 0x003E8E08
	public unsafe static void GetBytes(int _value, byte[] _target, int _targetOffset = 0)
	{
		Utils.GetBytes((byte*)(&_value), 4, _target, _targetOffset);
	}

	// Token: 0x0600A852 RID: 43090 RVA: 0x003EAC08 File Offset: 0x003E8E08
	public unsafe static void GetBytes(uint _value, byte[] _target, int _targetOffset = 0)
	{
		Utils.GetBytes((byte*)(&_value), 4, _target, _targetOffset);
	}

	// Token: 0x0600A853 RID: 43091 RVA: 0x003EAC15 File Offset: 0x003E8E15
	public unsafe static void GetBytes(long _value, byte[] _target, int _targetOffset = 0)
	{
		Utils.GetBytes((byte*)(&_value), 8, _target, _targetOffset);
	}

	// Token: 0x0600A854 RID: 43092 RVA: 0x003EAC24 File Offset: 0x003E8E24
	public unsafe static void GetBytes(byte* _ptr, int _count, byte[] _target, int _targetOffset = 0)
	{
		for (int i = 0; i < _count; i++)
		{
			_target[_targetOffset + i] = _ptr[i];
		}
	}

	// Token: 0x0600A855 RID: 43093 RVA: 0x003EAC48 File Offset: 0x003E8E48
	public static string MaskIp(string _input)
	{
		if (string.IsNullOrEmpty(_input))
		{
			return _input;
		}
		StringBuilder stringBuilder = new StringBuilder(_input);
		int i;
		if ((i = _input.IndexOfAny(Utils.ipSeparatorChars)) < 0)
		{
			return _input;
		}
		while (i >= 0)
		{
			if (i > 0)
			{
				stringBuilder[i - 1] = '*';
			}
			i = _input.IndexOfAny(Utils.ipSeparatorChars, i + 1);
		}
		stringBuilder[_input.Length - 1] = '*';
		return stringBuilder.ToString();
	}

	// Token: 0x0600A856 RID: 43094 RVA: 0x003EACB0 File Offset: 0x003E8EB0
	public static void ForceMaterialsInstance(GameObject go)
	{
		Renderer[] componentsInChildren = go.GetComponentsInChildren<Renderer>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			Material[] materials = componentsInChildren[i].materials;
		}
	}

	// Token: 0x0600A857 RID: 43095 RVA: 0x003EACDC File Offset: 0x003E8EDC
	public static bool IsValidWebUrl(ref string _input)
	{
		if (string.IsNullOrEmpty(_input))
		{
			return false;
		}
		if (_input.StartsWith("http://") || _input.StartsWith("https://"))
		{
			return true;
		}
		if (_input.Contains("://"))
		{
			return false;
		}
		_input = "http://" + _input;
		return true;
	}

	// Token: 0x0600A858 RID: 43096 RVA: 0x003EAD31 File Offset: 0x003E8F31
	public static bool OpenSystemBrowser(string _url)
	{
		if (!Utils.IsValidWebUrl(ref _url))
		{
			return false;
		}
		Application.OpenURL(_url);
		return true;
	}

	// Token: 0x0600A859 RID: 43097 RVA: 0x003EAD48 File Offset: 0x003E8F48
	public static bool ArrayEquals(byte[] _a, byte[] _b)
	{
		if (_a == _b)
		{
			return true;
		}
		if (_a == null || _b == null)
		{
			return false;
		}
		if (_a.Length != _b.Length)
		{
			return false;
		}
		for (int i = 0; i < _a.Length; i++)
		{
			if (_a[i] != _b[i])
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x0600A85A RID: 43098 RVA: 0x003EAD88 File Offset: 0x003E8F88
	public static bool ArrayEquals(int[] _a, int[] _b)
	{
		if (_a == _b)
		{
			return true;
		}
		if (_a == null || _b == null)
		{
			return false;
		}
		if (_a.Length != _b.Length)
		{
			return false;
		}
		for (int i = 0; i < _a.Length; i++)
		{
			if (_a[i] != _b[i])
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x0600A85B RID: 43099 RVA: 0x003EADC8 File Offset: 0x003E8FC8
	public static string GenerateGuid()
	{
		byte[] array = Guid.NewGuid().ToByteArray();
		StringBuilder stringBuilder = new StringBuilder(32);
		for (int i = 0; i < array.Length; i++)
		{
			stringBuilder.Append(array[i].ToString("X2"));
		}
		return stringBuilder.ToString();
	}

	// Token: 0x170013B0 RID: 5040
	// (get) Token: 0x0600A85C RID: 43100 RVA: 0x003EAE18 File Offset: 0x003E9018
	public static uint CurrentUnixTime
	{
		get
		{
			return (uint)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
		}
	}

	// Token: 0x04007CB8 RID: 31928
	public static string[] ExcludeLayerZoom = new string[] { "VisibleOnZoom" };

	// Token: 0x04007CB9 RID: 31929
	[PublicizedFrom(EAccessModifier.Private)]
	public static CultureInfo ci;

	// Token: 0x04007CBA RID: 31930
	public static readonly Regex hexAlphaMatcher = new Regex("[0-9a-fA-F]{2}");

	// Token: 0x04007CBB RID: 31931
	public static readonly Regex hexRgbMatcher = new Regex("[0-9a-fA-F]{6}");

	// Token: 0x04007CBC RID: 31932
	public static readonly Regex hexRgbaMatcher = new Regex("[0-9a-fA-F]{8}");

	// Token: 0x04007CBD RID: 31933
	public static readonly Regex nestedEscapePattern = new Regex("(\\[+\\/c\\](?:\\/c\\])*)");

	// Token: 0x04007CBE RID: 31934
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly List<Transform> setLayerRecursivelyList = new List<Transform>();

	// Token: 0x04007CBF RID: 31935
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly List<Collider> setColliderLayerRecursivelyList = new List<Collider>();

	// Token: 0x04007CC0 RID: 31936
	[PublicizedFrom(EAccessModifier.Private)]
	public static Action<IntPtr, byte, int> MemsetDelegate;

	// Token: 0x04007CC1 RID: 31937
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Vector3[] tan1Temp = new Vector3[786432];

	// Token: 0x04007CC2 RID: 31938
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Vector3[] tan2Temp = new Vector3[786432];

	// Token: 0x04007CC3 RID: 31939
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly List<Vector3> tempNormals = new List<Vector3>();

	// Token: 0x04007CC4 RID: 31940
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly List<Material> tempMats = new List<Material>();

	// Token: 0x04007CC5 RID: 31941
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly char[] ipSeparatorChars = new char[] { '.', ':' };

	// Token: 0x020014EC RID: 5356
	public enum EnumHitDirection : byte
	{
		// Token: 0x04007CC7 RID: 31943
		Front,
		// Token: 0x04007CC8 RID: 31944
		Back,
		// Token: 0x04007CC9 RID: 31945
		Left,
		// Token: 0x04007CCA RID: 31946
		Right,
		// Token: 0x04007CCB RID: 31947
		Explosion,
		// Token: 0x04007CCC RID: 31948
		None
	}

	// Token: 0x020014ED RID: 5357
	public enum ERestartAntiCheatMode
	{
		// Token: 0x04007CCE RID: 31950
		KeepAntiCheatMode,
		// Token: 0x04007CCF RID: 31951
		ForceOff,
		// Token: 0x04007CD0 RID: 31952
		ForceOn
	}
}

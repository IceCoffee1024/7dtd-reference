using System;
using UnityEngine;

// Token: 0x0200157F RID: 5503
public static class vp_3DUtility
{
	// Token: 0x0600AC2F RID: 44079 RVA: 0x00401E74 File Offset: 0x00400074
	public static Vector3 HorizontalVector(Vector3 value)
	{
		value.y = 0f;
		return value;
	}

	// Token: 0x0600AC30 RID: 44080 RVA: 0x00401E84 File Offset: 0x00400084
	public static Vector3 RandomHorizontalDirection()
	{
		return (global::UnityEngine.Random.rotation * Vector3.up).normalized;
	}

	// Token: 0x0600AC31 RID: 44081 RVA: 0x00401EA8 File Offset: 0x004000A8
	public static bool OnScreen(Camera camera, Renderer renderer, Vector3 worldPosition, out Vector3 screenPosition)
	{
		screenPosition = Vector2.zero;
		if (camera == null || renderer == null || !renderer.isVisible)
		{
			return false;
		}
		screenPosition = camera.WorldToScreenPoint(worldPosition);
		return screenPosition.z >= 0f;
	}

	// Token: 0x0600AC32 RID: 44082 RVA: 0x00401F00 File Offset: 0x00400100
	public static bool InLineOfSight(Vector3 from, Transform target, Vector3 targetOffset, int layerMask)
	{
		RaycastHit raycastHit;
		Physics.Linecast(from, target.position + targetOffset, out raycastHit, layerMask);
		return raycastHit.collider == null || raycastHit.collider.transform.root == target;
	}

	// Token: 0x0600AC33 RID: 44083 RVA: 0x00401F4E File Offset: 0x0040014E
	public static bool WithinRange(Vector3 from, Vector3 to, float range, out float distance)
	{
		distance = Vector3.Distance(from, to);
		return distance <= range;
	}

	// Token: 0x0600AC34 RID: 44084 RVA: 0x00401F64 File Offset: 0x00400164
	public static float DistanceToRay(Ray ray, Vector3 point)
	{
		return Vector3.Cross(ray.direction, point - ray.origin).magnitude;
	}

	// Token: 0x0600AC35 RID: 44085 RVA: 0x00401F94 File Offset: 0x00400194
	public static float LookAtAngle(Vector3 fromPosition, Vector3 fromForward, Vector3 toPosition)
	{
		if (Vector3.Cross(fromForward, (toPosition - fromPosition).normalized).y >= 0f)
		{
			return Vector3.Angle(fromForward, (toPosition - fromPosition).normalized);
		}
		return -Vector3.Angle(fromForward, (toPosition - fromPosition).normalized);
	}

	// Token: 0x0600AC36 RID: 44086 RVA: 0x00401FEE File Offset: 0x004001EE
	public static float LookAtAngleHorizontal(Vector3 fromPosition, Vector3 fromForward, Vector3 toPosition)
	{
		return vp_3DUtility.LookAtAngle(vp_3DUtility.HorizontalVector(fromPosition), vp_3DUtility.HorizontalVector(fromForward), vp_3DUtility.HorizontalVector(toPosition));
	}

	// Token: 0x0600AC37 RID: 44087 RVA: 0x00402008 File Offset: 0x00400208
	public static float AngleAroundAxis(Vector3 dirA, Vector3 dirB, Vector3 axis)
	{
		dirA -= Vector3.Project(dirA, axis);
		dirB -= Vector3.Project(dirB, axis);
		return Vector3.Angle(dirA, dirB) * (float)((Vector3.Dot(axis, Vector3.Cross(dirA, dirB)) < 0f) ? (-1) : 1);
	}

	// Token: 0x0600AC38 RID: 44088 RVA: 0x00402054 File Offset: 0x00400254
	public static Quaternion GetBoneLookRotationInWorldSpace(Quaternion originalRotation, Quaternion parentRotation, Vector3 worldLookDir, float amount, Vector3 referenceLookDir, Vector3 referenceUpDir, Quaternion relativeWorldSpaceDifference)
	{
		Vector3 vector = Quaternion.Inverse(parentRotation) * worldLookDir.normalized;
		vector = Quaternion.AngleAxis(vp_3DUtility.AngleAroundAxis(referenceLookDir, vector, referenceUpDir), referenceUpDir) * Quaternion.AngleAxis(vp_3DUtility.AngleAroundAxis(vector - Vector3.Project(vector, referenceUpDir), vector, Vector3.Cross(referenceUpDir, vector)), Vector3.Cross(referenceUpDir, referenceLookDir)) * referenceLookDir;
		Vector3 vector2 = referenceUpDir;
		Vector3.OrthoNormalize(ref vector, ref vector2);
		Vector3 vector3 = vector;
		Vector3 vector4 = vector2;
		Vector3.OrthoNormalize(ref vector3, ref vector4);
		return Quaternion.Lerp(Quaternion.identity, parentRotation * Quaternion.LookRotation(vector3, vector4) * Quaternion.Inverse(parentRotation * Quaternion.LookRotation(referenceLookDir, referenceUpDir)), amount) * originalRotation * relativeWorldSpaceDifference;
	}

	// Token: 0x0600AC39 RID: 44089 RVA: 0x00402114 File Offset: 0x00400314
	public static GameObject DebugPrimitive(PrimitiveType primitiveType, Vector3 scale, Color color, Vector3 pivotOffset, Transform parent = null)
	{
		GameObject gameObject = null;
		Material material = new Material(Shader.Find("Transparent/Diffuse"));
		material.color = color;
		GameObject gameObject2 = GameObject.CreatePrimitive(primitiveType);
		gameObject2.GetComponent<Collider>().enabled = false;
		gameObject2.GetComponent<Renderer>().material = material;
		gameObject2.transform.localScale = scale;
		gameObject2.name = "Debug" + gameObject2.name;
		if (pivotOffset != Vector3.zero)
		{
			gameObject = new GameObject(gameObject2.name);
			gameObject2.name = gameObject2.name.Replace("Debug", "");
			gameObject2.transform.parent = gameObject.transform;
			gameObject2.transform.localPosition = pivotOffset;
		}
		if (parent != null)
		{
			if (gameObject == null)
			{
				gameObject2.transform.parent = parent;
				gameObject2.transform.localPosition = Vector3.zero;
			}
			else
			{
				gameObject.transform.parent = parent;
				gameObject.transform.localPosition = Vector3.zero;
			}
		}
		if (!(gameObject != null))
		{
			return gameObject2;
		}
		return gameObject;
	}

	// Token: 0x0600AC3A RID: 44090 RVA: 0x00402227 File Offset: 0x00400427
	public static GameObject DebugPointer(Transform parent = null)
	{
		return vp_3DUtility.DebugPrimitive(PrimitiveType.Sphere, new Vector3(0.01f, 0.01f, 3f), new Color(1f, 1f, 0f, 0.75f), Vector3.forward, parent);
	}

	// Token: 0x0600AC3B RID: 44091 RVA: 0x00402262 File Offset: 0x00400462
	public static GameObject DebugBall(Transform parent = null)
	{
		return vp_3DUtility.DebugPrimitive(PrimitiveType.Sphere, Vector3.one * 0.25f, new Color(1f, 0f, 0f, 0.5f), Vector3.zero, parent);
	}
}

using System;
using UnityEngine;

// Token: 0x02001564 RID: 5476
public static class vp_3DUtility
{
	// Token: 0x0600AB32 RID: 43826 RVA: 0x003FCB34 File Offset: 0x003FAD34
	public static Vector3 HorizontalVector(Vector3 value)
	{
		value.y = 0f;
		return value;
	}

	// Token: 0x0600AB33 RID: 43827 RVA: 0x003FCB44 File Offset: 0x003FAD44
	public static Vector3 RandomHorizontalDirection()
	{
		return (global::UnityEngine.Random.rotation * Vector3.up).normalized;
	}

	// Token: 0x0600AB34 RID: 43828 RVA: 0x003FCB68 File Offset: 0x003FAD68
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

	// Token: 0x0600AB35 RID: 43829 RVA: 0x003FCBC0 File Offset: 0x003FADC0
	public static bool InLineOfSight(Vector3 from, Transform target, Vector3 targetOffset, int layerMask)
	{
		RaycastHit raycastHit;
		Physics.Linecast(from, target.position + targetOffset, out raycastHit, layerMask);
		return raycastHit.collider == null || raycastHit.collider.transform.root == target;
	}

	// Token: 0x0600AB36 RID: 43830 RVA: 0x003FCC0E File Offset: 0x003FAE0E
	public static bool WithinRange(Vector3 from, Vector3 to, float range, out float distance)
	{
		distance = Vector3.Distance(from, to);
		return distance <= range;
	}

	// Token: 0x0600AB37 RID: 43831 RVA: 0x003FCC24 File Offset: 0x003FAE24
	public static float DistanceToRay(Ray ray, Vector3 point)
	{
		return Vector3.Cross(ray.direction, point - ray.origin).magnitude;
	}

	// Token: 0x0600AB38 RID: 43832 RVA: 0x003FCC54 File Offset: 0x003FAE54
	public static float LookAtAngle(Vector3 fromPosition, Vector3 fromForward, Vector3 toPosition)
	{
		if (Vector3.Cross(fromForward, (toPosition - fromPosition).normalized).y >= 0f)
		{
			return Vector3.Angle(fromForward, (toPosition - fromPosition).normalized);
		}
		return -Vector3.Angle(fromForward, (toPosition - fromPosition).normalized);
	}

	// Token: 0x0600AB39 RID: 43833 RVA: 0x003FCCAE File Offset: 0x003FAEAE
	public static float LookAtAngleHorizontal(Vector3 fromPosition, Vector3 fromForward, Vector3 toPosition)
	{
		return vp_3DUtility.LookAtAngle(vp_3DUtility.HorizontalVector(fromPosition), vp_3DUtility.HorizontalVector(fromForward), vp_3DUtility.HorizontalVector(toPosition));
	}

	// Token: 0x0600AB3A RID: 43834 RVA: 0x003FCCC8 File Offset: 0x003FAEC8
	public static float AngleAroundAxis(Vector3 dirA, Vector3 dirB, Vector3 axis)
	{
		dirA -= Vector3.Project(dirA, axis);
		dirB -= Vector3.Project(dirB, axis);
		return Vector3.Angle(dirA, dirB) * (float)((Vector3.Dot(axis, Vector3.Cross(dirA, dirB)) < 0f) ? (-1) : 1);
	}

	// Token: 0x0600AB3B RID: 43835 RVA: 0x003FCD14 File Offset: 0x003FAF14
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

	// Token: 0x0600AB3C RID: 43836 RVA: 0x003FCDD4 File Offset: 0x003FAFD4
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

	// Token: 0x0600AB3D RID: 43837 RVA: 0x003FCEE7 File Offset: 0x003FB0E7
	public static GameObject DebugPointer(Transform parent = null)
	{
		return vp_3DUtility.DebugPrimitive(PrimitiveType.Sphere, new Vector3(0.01f, 0.01f, 3f), new Color(1f, 1f, 0f, 0.75f), Vector3.forward, parent);
	}

	// Token: 0x0600AB3E RID: 43838 RVA: 0x003FCF22 File Offset: 0x003FB122
	public static GameObject DebugBall(Transform parent = null)
	{
		return vp_3DUtility.DebugPrimitive(PrimitiveType.Sphere, Vector3.one * 0.25f, new Color(1f, 0f, 0f, 0.5f), Vector3.zero, parent);
	}
}

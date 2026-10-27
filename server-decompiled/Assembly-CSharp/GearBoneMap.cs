using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;

// Token: 0x02000A60 RID: 2656
[DisallowMultipleComponent]
public class GearBoneMap : MonoBehaviour
{
	// Token: 0x06004FCE RID: 20430 RVA: 0x001E64C0 File Offset: 0x001E46C0
	public IReadOnlyList<Transform> GetPartBones(string partName)
	{
		if (string.IsNullOrEmpty(partName))
		{
			return Array.Empty<Transform>();
		}
		GearBoneMap.PartBones partBones = this.parts.FirstOrDefault<GearBoneMap.PartBones>((GearBoneMap.PartBones p) => string.Equals(p.partName, partName, StringComparison.Ordinal));
		if (partBones != null && partBones.bones != null)
		{
			return partBones.bones;
		}
		int num = partName.IndexOf("_");
		if (num < 0)
		{
			num = partName.Length;
		}
		partName = partName.Substring(0, num);
		partBones = this.parts.FirstOrDefault<GearBoneMap.PartBones>((GearBoneMap.PartBones p) => string.Equals(p.partName, partName, StringComparison.Ordinal));
		if (partBones != null && partBones.bones != null)
		{
			return partBones.bones;
		}
		return Array.Empty<Transform>();
	}

	// Token: 0x06004FCF RID: 20431 RVA: 0x001E657A File Offset: 0x001E477A
	public IReadOnlyList<string> GetPartNames()
	{
		return this.parts.Select<GearBoneMap.PartBones, string>((GearBoneMap.PartBones p) => p.partName).ToArray<string>();
	}

	// Token: 0x06004FD0 RID: 20432 RVA: 0x001E65AC File Offset: 0x001E47AC
	public void SetBones(string partName, IEnumerable<Transform> newBones)
	{
		if (string.IsNullOrEmpty(partName))
		{
			return;
		}
		GearBoneMap.PartBones partBones = this.parts.FirstOrDefault<GearBoneMap.PartBones>((GearBoneMap.PartBones p) => string.Equals(p.partName, partName, StringComparison.Ordinal));
		if (partBones == null)
		{
			partBones = new GearBoneMap.PartBones
			{
				partName = partName
			};
			this.parts.Add(partBones);
		}
		List<Transform> list = (from t in newBones.Where<Transform>((Transform t) => t != null).Distinct<Transform>()
			orderby GearBoneMap.GetHierarchyPath(t), t.name
			select t).ToList<Transform>();
		partBones.bones.Clear();
		partBones.bones.AddRange(list);
	}

	// Token: 0x06004FD1 RID: 20433 RVA: 0x001E669C File Offset: 0x001E489C
	public void ClearAll()
	{
		this.parts.Clear();
	}

	// Token: 0x06004FD2 RID: 20434 RVA: 0x001E66AC File Offset: 0x001E48AC
	[PublicizedFrom(EAccessModifier.Private)]
	public static string GetHierarchyPath(Transform t)
	{
		if (t == null)
		{
			return string.Empty;
		}
		Stack<string> stack = new Stack<string>();
		Transform transform = t;
		while (transform != null)
		{
			stack.Push(transform.name);
			transform = transform.parent;
		}
		return string.Join("/", stack);
	}

	// Token: 0x06004FD3 RID: 20435 RVA: 0x001E66FC File Offset: 0x001E48FC
	public void Bake()
	{
		List<Transform> list = new List<Transform>();
		foreach (Transform transform in base.transform.GetComponentsInChildren<Transform>(true))
		{
			int num = transform.name.IndexOf("_");
			if (num < 0)
			{
				num = transform.name.Length;
			}
			if (transform.parent == base.transform && GearBoneMap.DefaultParts.Contains(transform.name.Substring(0, num)))
			{
				list.Add(transform);
			}
		}
		foreach (Transform transform2 in list)
		{
			SkinnedMeshRenderer[] componentsInChildren2 = transform2.GetComponentsInChildren<SkinnedMeshRenderer>(true);
			HashSet<Transform> hashSet = new HashSet<Transform>();
			foreach (SkinnedMeshRenderer skinnedMeshRenderer in componentsInChildren2)
			{
				if (!(skinnedMeshRenderer == null))
				{
					Mesh sharedMesh = skinnedMeshRenderer.sharedMesh;
					if (sharedMesh == null)
					{
						Debug.LogWarning("[GearBoneMap] " + skinnedMeshRenderer.name + " has no sharedMesh.");
					}
					else
					{
						try
						{
							NativeArray<byte> bonesPerVertex = sharedMesh.GetBonesPerVertex();
							NativeArray<BoneWeight1> allBoneWeights = sharedMesh.GetAllBoneWeights();
							Transform[] bones = skinnedMeshRenderer.bones;
							int num2 = 0;
							for (int k = 0; k < bonesPerVertex.Length; k++)
							{
								int num3 = (int)bonesPerVertex[k];
								for (int l = 0; l < num3; l++)
								{
									BoneWeight1 boneWeight = allBoneWeights[num2++];
									if (boneWeight.weight > 0f)
									{
										int boneIndex = boneWeight.boneIndex;
										if (boneIndex >= 0 && bones != null && boneIndex < bones.Length)
										{
											Transform transform3 = bones[boneIndex];
											if (transform3 != null)
											{
												hashSet.Add(transform3);
											}
										}
									}
								}
							}
							if (skinnedMeshRenderer.rootBone != null)
							{
								hashSet.Add(skinnedMeshRenderer.rootBone);
							}
						}
						catch (Exception ex)
						{
							Debug.LogError("[GearBoneMap] Failed reading bone weights for " + skinnedMeshRenderer.name + ": " + ex.Message);
						}
					}
				}
			}
			GearBoneMap.AddComponentHostsUnderIncluded<DynamicBone>(hashSet);
			GearBoneMap.AddComponentHostsUnderIncluded<DynamicBoneColliderBase>(hashSet);
			this.SetBones(transform2.name, hashSet);
		}
	}

	// Token: 0x06004FD4 RID: 20436 RVA: 0x001E6964 File Offset: 0x001E4B64
	[PublicizedFrom(EAccessModifier.Private)]
	public static void AddComponentHostsUnderIncluded<DynamicComponent>(HashSet<Transform> collected) where DynamicComponent : Component
	{
		List<Transform> list = new List<Transform>(collected);
		for (int i = 0; i < list.Count; i++)
		{
			Transform transform = list[i];
			if (!(transform == null))
			{
				foreach (DynamicComponent dynamicComponent in transform.GetComponentsInChildren<DynamicComponent>(true))
				{
					if (!(dynamicComponent == null))
					{
						Transform transform2 = dynamicComponent.transform;
						if (!collected.Contains(transform2))
						{
							collected.Add(transform2);
						}
					}
				}
			}
		}
	}

	// Token: 0x04003D9F RID: 15775
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public List<GearBoneMap.PartBones> parts = new List<GearBoneMap.PartBones>();

	// Token: 0x04003DA0 RID: 15776
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly string[] DefaultParts = new string[] { "head", "body", "feet", "hands" };

	// Token: 0x02000A61 RID: 2657
	[Serializable]
	public class PartBones
	{
		// Token: 0x04003DA1 RID: 15777
		[Tooltip("e.g. head, body, feet, hands")]
		public string partName;

		// Token: 0x04003DA2 RID: 15778
		[Tooltip("The unique bones actually referenced by skin weights for this part")]
		public List<Transform> bones = new List<Transform>();
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;

// Token: 0x02000A3B RID: 2619
[DisallowMultipleComponent]
public class GearBoneMap : MonoBehaviour
{
	// Token: 0x06004F55 RID: 20309 RVA: 0x001E1690 File Offset: 0x001DF890
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

	// Token: 0x06004F56 RID: 20310 RVA: 0x001E174A File Offset: 0x001DF94A
	public IReadOnlyList<string> GetPartNames()
	{
		return this.parts.Select<GearBoneMap.PartBones, string>((GearBoneMap.PartBones p) => p.partName).ToArray<string>();
	}

	// Token: 0x06004F57 RID: 20311 RVA: 0x001E177C File Offset: 0x001DF97C
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

	// Token: 0x06004F58 RID: 20312 RVA: 0x001E186C File Offset: 0x001DFA6C
	public void ClearAll()
	{
		this.parts.Clear();
	}

	// Token: 0x06004F59 RID: 20313 RVA: 0x001E187C File Offset: 0x001DFA7C
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

	// Token: 0x06004F5A RID: 20314 RVA: 0x001E18CC File Offset: 0x001DFACC
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
			this.SetBones(transform2.name, hashSet);
		}
	}

	// Token: 0x04003CE0 RID: 15584
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public List<GearBoneMap.PartBones> parts = new List<GearBoneMap.PartBones>();

	// Token: 0x04003CE1 RID: 15585
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly string[] DefaultParts = new string[] { "head", "body", "feet", "hands" };

	// Token: 0x02000A3C RID: 2620
	[Serializable]
	public class PartBones
	{
		// Token: 0x04003CE2 RID: 15586
		[Tooltip("e.g. head, body, feet, hands")]
		public string partName;

		// Token: 0x04003CE3 RID: 15587
		[Tooltip("The unique bones actually referenced by skin weights for this part")]
		public List<Transform> bones = new List<Transform>();
	}
}

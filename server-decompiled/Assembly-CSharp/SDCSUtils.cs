using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using GearVariants;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Animations.Rigging;

// Token: 0x02000A46 RID: 2630
public static class SDCSUtils
{
	// Token: 0x06004F80 RID: 20352 RVA: 0x001E2A20 File Offset: 0x001E0C20
	public static GameObject Stitch(GameObject sourceObj, GameObject parentObj, SDCSUtils.TransformCatalog boneCatalog, EModelSDCS emodel = null, bool isFPV = false, bool isUI = false, Material eyeMat = null, bool isGear = false)
	{
		GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(sourceObj, parentObj.transform);
		gameObject.name = sourceObj.name;
		gameObject.GetComponentsInChildren<SkinnedMeshRenderer>(SDCSUtils.tempSMRs);
		gameObject.GetComponentsInChildren<Cloth>(SDCSUtils.tempCloths);
		bool flag = gameObject.name.StartsWith("body");
		foreach (SkinnedMeshRenderer skinnedMeshRenderer in SDCSUtils.tempSMRs)
		{
			skinnedMeshRenderer.bones = SDCSUtils.TranslateTransforms(skinnedMeshRenderer.bones, boneCatalog);
			skinnedMeshRenderer.rootBone = SDCSUtils.Find<string, Transform>(boneCatalog, skinnedMeshRenderer.rootBone.name);
			skinnedMeshRenderer.updateWhenOffscreen = true;
			Material[] sharedMaterials = skinnedMeshRenderer.sharedMaterials;
			for (int i = 0; i < sharedMaterials.Length; i++)
			{
				if (sharedMaterials[i] && sharedMaterials[i].HasColor("_Tint"))
				{
					string name = sharedMaterials[i].name;
					Material material = null;
					if (name.Contains("_Body"))
					{
						material = DataLoader.LoadAsset<Material>(SDCSUtils.baseBodyMatLoc, false);
					}
					else if (name.Contains("_Head"))
					{
						material = DataLoader.LoadAsset<Material>(SDCSUtils.baseHeadMatLoc, false);
					}
					else if (name.Contains("_Hand"))
					{
						material = DataLoader.LoadAsset<Material>(SDCSUtils.baseHandsMatLoc, false);
					}
					if (material != null && material.HasColor("_Tint"))
					{
						sharedMaterials[i] = new Material(sharedMaterials[i]);
						sharedMaterials[i].SetColor("_Tint", material.GetColor("_Tint"));
					}
				}
			}
			if (skinnedMeshRenderer.gameObject.name == "eyes" && eyeMat)
			{
				sharedMaterials[0] = eyeMat;
			}
			for (int j = 0; j < sharedMaterials.Length; j++)
			{
				sharedMaterials[j] = new Material(sharedMaterials[j]);
			}
			skinnedMeshRenderer.sharedMaterials = sharedMaterials;
			if (flag && isGear && emodel && !isUI && isFPV)
			{
				List<int> list = null;
				for (int k = 0; k < sharedMaterials.Length; k++)
				{
					if (sharedMaterials[k].HasFloat("_ClipFPV"))
					{
						sharedMaterials[k].SetFloat("_ClipFPV", 1f);
						if (list == null)
						{
							list = new List<int>(sharedMaterials.Length);
						}
						list.Add(k);
					}
				}
				if (list != null && list.Count > 0)
				{
					SDCSUtils.RemoveFPViewObstructingGearPolygons(skinnedMeshRenderer, list.ToArray());
				}
			}
		}
		SDCSUtils.tempSMRs.Clear();
		Transform transform = boneCatalog["Hips"];
		foreach (Cloth cloth in SDCSUtils.tempCloths)
		{
			cloth.capsuleColliders = transform.GetComponentsInChildren<CapsuleCollider>();
		}
		SDCSUtils.tempCloths.Clear();
		return gameObject;
	}

	// Token: 0x06004F81 RID: 20353 RVA: 0x001E2D2C File Offset: 0x001E0F2C
	[PublicizedFrom(EAccessModifier.Private)]
	public static void RemoveFPViewObstructingGearPolygons(SkinnedMeshRenderer smr, params int[] submeshIndices)
	{
		if (!smr || !smr.sharedMesh)
		{
			return;
		}
		Mesh sharedMesh = smr.sharedMesh;
		Color[] colors = sharedMesh.colors;
		if (colors == null || colors.Length == 0)
		{
			return;
		}
		int subMeshCount = sharedMesh.subMeshCount;
		if (subMeshCount <= 0)
		{
			return;
		}
		List<int> list = new List<int>(subMeshCount);
		if (submeshIndices == null || submeshIndices.Length == 0)
		{
			for (int i = 0; i < subMeshCount; i++)
			{
				list.Add(i);
			}
		}
		else
		{
			foreach (int num in submeshIndices)
			{
				if (num >= 0 && num < subMeshCount && !list.Contains(num))
				{
					list.Add(num);
				}
			}
		}
		if (list.Count == 0)
		{
			return;
		}
		Mesh mesh = global::UnityEngine.Object.Instantiate<Mesh>(sharedMesh);
		smr.sharedMesh = mesh;
		for (int k = 0; k < list.Count; k++)
		{
			int num2 = list[k];
			int[] triangles = mesh.GetTriangles(num2);
			if (triangles != null && triangles.Length != 0)
			{
				int num3 = triangles.Length / 3;
				int[] array = new int[triangles.Length];
				int num4 = 0;
				for (int l = 0; l < num3; l++)
				{
					int num5 = triangles[l * 3];
					int num6 = triangles[l * 3 + 1];
					int num7 = triangles[l * 3 + 2];
					if (num5 < colors.Length && num6 < colors.Length && num7 < colors.Length && colors[num5].r == 0f && colors[num6].r == 0f && colors[num7].r == 0f)
					{
						array[num4 * 3] = num5;
						array[num4 * 3 + 1] = num6;
						array[num4 * 3 + 2] = num7;
						num4++;
					}
				}
				Array.Resize<int>(ref array, num4 * 3);
				mesh.SetTriangles(array, num2);
			}
		}
	}

	// Token: 0x06004F82 RID: 20354 RVA: 0x001E2F00 File Offset: 0x001E1100
	[PublicizedFrom(EAccessModifier.Private)]
	public static HashSet<string> CollectRequiredNamesForSlot(Transform root, Transform slotSubRoot)
	{
		HashSet<string> hashSet = new HashSet<string>(StringComparer.Ordinal);
		GearBoneMap component = root.GetComponent<GearBoneMap>();
		if (component != null)
		{
			IReadOnlyList<Transform> partBones = component.GetPartBones(slotSubRoot.name);
			if (partBones == null || partBones.Count <= 0)
			{
				goto IL_013B;
			}
			using (IEnumerator<Transform> enumerator = partBones.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Transform transform = enumerator.Current;
					if (transform)
					{
						hashSet.Add(transform.name);
					}
				}
				goto IL_013B;
			}
		}
		Debug.LogWarning(string.Concat(new string[] { "[SDCSUtils] No GearBoneMap found on root ", root.name, ", falling back to collecting all bones from SMRs under ", slotSubRoot.name, "." }));
		SDCSUtils._smrBuf.Clear();
		slotSubRoot.GetComponentsInChildren<SkinnedMeshRenderer>(true, SDCSUtils._smrBuf);
		for (int i = 0; i < SDCSUtils._smrBuf.Count; i++)
		{
			SkinnedMeshRenderer skinnedMeshRenderer = SDCSUtils._smrBuf[i];
			Transform[] array = ((skinnedMeshRenderer != null) ? skinnedMeshRenderer.bones : null);
			if (array != null)
			{
				foreach (Transform transform2 in array)
				{
					if (transform2)
					{
						hashSet.Add(transform2.name);
					}
				}
			}
		}
		IL_013B:
		hashSet = SDCSUtils.BuildAllowedWithAncestors(root, hashSet);
		SDCSUtils._hingeBuf.Clear();
		slotSubRoot.GetComponentsInChildren<HingeJoint>(true, SDCSUtils._hingeBuf);
		for (int k = 0; k < SDCSUtils._hingeBuf.Count; k++)
		{
			HingeJoint hingeJoint = SDCSUtils._hingeBuf[k];
			if (hingeJoint)
			{
				hashSet.Add(hingeJoint.transform.name);
				if (hingeJoint.connectedBody)
				{
					hashSet.Add(hingeJoint.connectedBody.transform.name);
				}
			}
		}
		SDCSUtils._bcBuf.Clear();
		root.GetComponentsInChildren<BlendConstraint>(true, SDCSUtils._bcBuf);
		for (int l = 0; l < SDCSUtils._bcBuf.Count; l++)
		{
			BlendConstraint blendConstraint = SDCSUtils._bcBuf[l];
			if (blendConstraint)
			{
				Transform constrainedObject = blendConstraint.data.constrainedObject;
				Transform sourceObjectA = blendConstraint.data.sourceObjectA;
				Transform sourceObjectB = blendConstraint.data.sourceObjectB;
				if (constrainedObject && hashSet.Contains(constrainedObject.name))
				{
					if (sourceObjectA)
					{
						hashSet.Add(sourceObjectA.name);
					}
					if (sourceObjectB)
					{
						hashSet.Add(sourceObjectB.name);
					}
				}
			}
		}
		return SDCSUtils.BuildAllowedWithAncestors(root, hashSet);
	}

	// Token: 0x06004F83 RID: 20355 RVA: 0x001E31A4 File Offset: 0x001E13A4
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<string, Transform> MapSourceByName(Transform sourceOrigin)
	{
		SDCSUtils.<>c__DisplayClass10_0 CS$<>8__locals1;
		CS$<>8__locals1.map = new Dictionary<string, Transform>(StringComparer.Ordinal);
		SDCSUtils.<MapSourceByName>g__Recurse|10_0(sourceOrigin, ref CS$<>8__locals1);
		return CS$<>8__locals1.map;
	}

	// Token: 0x06004F84 RID: 20356 RVA: 0x001E31D0 File Offset: 0x001E13D0
	[PublicizedFrom(EAccessModifier.Private)]
	public static HashSet<string> BuildAllowedWithAncestors(Transform sourceOrigin, HashSet<string> allowedBones)
	{
		HashSet<string> hashSet = new HashSet<string>(allowedBones, StringComparer.Ordinal);
		Dictionary<string, Transform> dictionary = SDCSUtils.MapSourceByName(sourceOrigin);
		foreach (string text in allowedBones)
		{
			Transform transform;
			if (dictionary.TryGetValue(text, out transform))
			{
				Transform transform2 = transform.parent;
				while (transform2 != null)
				{
					hashSet.Add(transform2.name);
					if (transform2 == sourceOrigin)
					{
						break;
					}
					transform2 = transform2.parent;
				}
			}
		}
		return hashSet;
	}

	// Token: 0x06004F85 RID: 20357 RVA: 0x001E326C File Offset: 0x001E146C
	[PublicizedFrom(EAccessModifier.Private)]
	public static void AddRequiredChildren(Transform sourceT, Transform targetT, SDCSUtils.TransformCatalog catalog, HashSet<string> allowedBones, AuxBoneTracker auxBoneTracker = null)
	{
		if (auxBoneTracker == null && catalog != null && catalog["Origin"] != null)
		{
			auxBoneTracker = catalog["Origin"].GetComponent<AuxBoneTracker>();
		}
		for (int i = 0; i < sourceT.childCount; i++)
		{
			Transform child = sourceT.GetChild(i);
			if (allowedBones.Contains(child.name))
			{
				Transform transform = null;
				for (int j = 0; j < targetT.childCount; j++)
				{
					Transform child2 = targetT.GetChild(j);
					if (child2.name == child.name)
					{
						transform = child2;
						break;
					}
				}
				if (!transform)
				{
					transform = global::UnityEngine.Object.Instantiate<GameObject>(child.gameObject, targetT, true).transform;
					transform.name = child.name;
					transform.SetLocalPositionAndRotation(child.localPosition, child.localRotation);
					transform.localScale = child.localScale;
					if (auxBoneTracker && !auxBoneTracker.AuxBoneLookup.ContainsKey(transform.name))
					{
						auxBoneTracker.AuxBoneLookup.Add(transform.name, transform);
					}
				}
				if (!catalog.ContainsKey(transform.name))
				{
					catalog.Add(transform.name, transform);
				}
				SDCSUtils.TransferCharacterJoint(child, transform.gameObject, catalog);
				SDCSUtils.AddRequiredChildren(child, transform, catalog, allowedBones, auxBoneTracker);
			}
		}
	}

	// Token: 0x06004F86 RID: 20358 RVA: 0x001E33B8 File Offset: 0x001E15B8
	[PublicizedFrom(EAccessModifier.Private)]
	public static void SetupRigConstraints(RigBuilder rigBuilder, Transform sourceRootT, Transform targetRootT, SDCSUtils.TransformCatalog catalog, HashSet<string> allowedBones)
	{
		Transform transform = sourceRootT.parent.Find("RigConstraints");
		if (!transform)
		{
			return;
		}
		string text = "RigConstraints_" + sourceRootT.name;
		Transform transform2 = targetRootT.Find(text);
		if (!transform2)
		{
			transform2 = new GameObject(text).transform;
			transform2.SetParent(targetRootT, false);
		}
		Rig rig = transform2.gameObject.GetOrAddComponent<Rig>();
		if (!rigBuilder.layers.Any<RigLayer>((RigLayer l) => l.rig == rig))
		{
			rigBuilder.layers.Add(new RigLayer(rig, true));
		}
		for (int i = transform2.childCount - 1; i >= 0; i--)
		{
			global::UnityEngine.Object.DestroyImmediate(transform2.GetChild(i).gameObject);
		}
		foreach (BlendConstraint blendConstraint in transform.GetComponentsInChildren<BlendConstraint>(true))
		{
			string text2 = ((blendConstraint.data.constrainedObject != null) ? blendConstraint.data.constrainedObject.name : null);
			if (text2 != null && allowedBones.Contains(text2))
			{
				GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(blendConstraint.gameObject, transform2, false);
				gameObject.name = blendConstraint.name;
				BlendConstraint component = gameObject.GetComponent<BlendConstraint>();
				component.data.constrainedObject = ((text2 != null) ? SDCSUtils.Find<string, Transform>(catalog, text2) : null);
				string text3 = ((blendConstraint.data.sourceObjectA != null) ? blendConstraint.data.sourceObjectA.name : null);
				string text4 = ((blendConstraint.data.sourceObjectB != null) ? blendConstraint.data.sourceObjectB.name : null);
				component.data.sourceObjectA = ((text3 != null) ? SDCSUtils.Find<string, Transform>(catalog, text3) : null);
				component.data.sourceObjectB = ((text4 != null) ? SDCSUtils.Find<string, Transform>(catalog, text4) : null);
			}
		}
	}

	// Token: 0x06004F87 RID: 20359 RVA: 0x001E35B0 File Offset: 0x001E17B0
	public static void MatchRigs(Transform source, Transform target, SDCSUtils.TransformCatalog catalog, HashSet<string> allowedBones)
	{
		Transform transform = source.Find("Origin");
		Transform transform2 = target.Find("Origin");
		if (!transform || !transform2)
		{
			return;
		}
		AuxBoneTracker auxBoneTracker = null;
		if (catalog != null && catalog["Origin"] != null)
		{
			auxBoneTracker = catalog["Origin"].GetComponent<AuxBoneTracker>();
		}
		SDCSUtils.AddRequiredChildren(transform, transform2, catalog, allowedBones, auxBoneTracker);
	}

	// Token: 0x06004F88 RID: 20360 RVA: 0x001E361C File Offset: 0x001E181C
	[PublicizedFrom(EAccessModifier.Private)]
	public static void TransferCharacterJoint(Transform source, GameObject newBone, SDCSUtils.TransformCatalog transformCatalog)
	{
		CharacterJoint component;
		CharacterJoint characterJoint;
		if ((component = source.GetComponent<CharacterJoint>()) != null && (characterJoint = newBone.AddMissingComponent<CharacterJoint>()) != null)
		{
			Transform transform = SDCSUtils.Find<string, Transform>(transformCatalog, component.connectedBody.name);
			characterJoint.connectedBody = ((transform != null) ? transform.GetComponent<Rigidbody>() : null);
		}
	}

	// Token: 0x06004F89 RID: 20361 RVA: 0x001E3674 File Offset: 0x001E1874
	[PublicizedFrom(EAccessModifier.Private)]
	public static Transform[] TranslateTransforms(Transform[] transforms, SDCSUtils.TransformCatalog transformCatalog)
	{
		for (int i = 0; i < transforms.Length; i++)
		{
			Transform transform = transforms[i];
			if (transform)
			{
				transforms[i] = SDCSUtils.Find<string, Transform>(transformCatalog, transform.name);
			}
			else
			{
				Log.Error("Null transform in bone list");
			}
		}
		return transforms;
	}

	// Token: 0x06004F8A RID: 20362 RVA: 0x001E36B8 File Offset: 0x001E18B8
	public static TValue Find<TKey, TValue>(Dictionary<TKey, TValue> source, TKey key)
	{
		TValue tvalue;
		source.TryGetValue(key, out tvalue);
		return tvalue;
	}

	// Token: 0x1700087E RID: 2174
	// (get) Token: 0x06004F8B RID: 20363 RVA: 0x001E36D0 File Offset: 0x001E18D0
	public static string baseBodyLoc
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return string.Concat(new string[]
			{
				"@:Entities/Player/",
				SDCSUtils.tmpArchetype.Sex,
				"/Common/Meshes/player",
				SDCSUtils.tmpArchetype.Sex,
				".fbx"
			});
		}
	}

	// Token: 0x1700087F RID: 2175
	// (get) Token: 0x06004F8C RID: 20364 RVA: 0x001E3710 File Offset: 0x001E1910
	public static string baseHeadLoc
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return string.Concat(new string[]
			{
				"@:Entities/Player/",
				SDCSUtils.tmpArchetype.Sex,
				"/Heads/",
				SDCSUtils.tmpArchetype.Race,
				"/",
				SDCSUtils.tmpArchetype.Variant.ToString("00"),
				"/Meshes/player",
				SDCSUtils.tmpArchetype.Sex,
				SDCSUtils.tmpArchetype.Race,
				SDCSUtils.tmpArchetype.Variant.ToString("00"),
				".fbx"
			});
		}
	}

	// Token: 0x17000880 RID: 2176
	// (get) Token: 0x06004F8D RID: 20365 RVA: 0x001E37B8 File Offset: 0x001E19B8
	public static string baseHairLoc
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return string.Concat(new string[]
			{
				"@:Entities/Player/",
				SDCSUtils.tmpArchetype.Sex,
				"/Hair/",
				SDCSUtils.tmpArchetype.Hair,
				"/HairMorphMatrix/",
				SDCSUtils.tmpArchetype.Race,
				SDCSUtils.tmpArchetype.Variant.ToString("00")
			});
		}
	}

	// Token: 0x17000881 RID: 2177
	// (get) Token: 0x06004F8E RID: 20366 RVA: 0x001E3828 File Offset: 0x001E1A28
	public static string baseMustacheLoc
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return string.Concat(new string[]
			{
				"@:Entities/Player/",
				SDCSUtils.tmpArchetype.Sex,
				"/FacialHair/Mustache/",
				SDCSUtils.tmpArchetype.MustacheName,
				"/HairMorphMatrix/",
				SDCSUtils.tmpArchetype.Race,
				SDCSUtils.tmpArchetype.Variant.ToString("00")
			});
		}
	}

	// Token: 0x17000882 RID: 2178
	// (get) Token: 0x06004F8F RID: 20367 RVA: 0x001E3898 File Offset: 0x001E1A98
	public static string baseChopsLoc
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return string.Concat(new string[]
			{
				"@:Entities/Player/",
				SDCSUtils.tmpArchetype.Sex,
				"/FacialHair/Chops/",
				SDCSUtils.tmpArchetype.ChopsName,
				"/HairMorphMatrix/",
				SDCSUtils.tmpArchetype.Race,
				SDCSUtils.tmpArchetype.Variant.ToString("00")
			});
		}
	}

	// Token: 0x17000883 RID: 2179
	// (get) Token: 0x06004F90 RID: 20368 RVA: 0x001E3908 File Offset: 0x001E1B08
	public static string baseBeardLoc
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return string.Concat(new string[]
			{
				"@:Entities/Player/",
				SDCSUtils.tmpArchetype.Sex,
				"/FacialHair/Beard/",
				SDCSUtils.tmpArchetype.BeardName,
				"/HairMorphMatrix/",
				SDCSUtils.tmpArchetype.Race,
				SDCSUtils.tmpArchetype.Variant.ToString("00")
			});
		}
	}

	// Token: 0x17000884 RID: 2180
	// (get) Token: 0x06004F91 RID: 20369 RVA: 0x0000A1FE File Offset: 0x000083FE
	public static string baseHairColorLoc
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return "@:Entities/Player/Common/HairColorSwatches";
		}
	}

	// Token: 0x17000885 RID: 2181
	// (get) Token: 0x06004F92 RID: 20370 RVA: 0x001E3976 File Offset: 0x001E1B76
	public static string baseEyeColorMatLoc
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return "@:Entities/Player/Common/Eyes/Materials/" + SDCSUtils.tmpArchetype.EyeColorName + ".mat";
		}
	}

	// Token: 0x17000886 RID: 2182
	// (get) Token: 0x06004F93 RID: 20371 RVA: 0x001E3994 File Offset: 0x001E1B94
	public static string baseBodyMatLoc
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return string.Concat(new string[]
			{
				"@:Entities/Player/",
				SDCSUtils.tmpArchetype.Sex,
				"/Heads/",
				SDCSUtils.tmpArchetype.Race,
				"/",
				SDCSUtils.tmpArchetype.Variant.ToString("00"),
				"/Materials/player",
				SDCSUtils.tmpArchetype.Sex,
				SDCSUtils.tmpArchetype.Race,
				SDCSUtils.tmpArchetype.Variant.ToString("00"),
				"_Body.mat"
			});
		}
	}

	// Token: 0x17000887 RID: 2183
	// (get) Token: 0x06004F94 RID: 20372 RVA: 0x001E3A3C File Offset: 0x001E1C3C
	public static string baseHeadMatLoc
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return string.Concat(new string[]
			{
				"@:Entities/Player/",
				SDCSUtils.tmpArchetype.Sex,
				"/Heads/",
				SDCSUtils.tmpArchetype.Race,
				"/",
				SDCSUtils.tmpArchetype.Variant.ToString("00"),
				"/Materials/player",
				SDCSUtils.tmpArchetype.Sex,
				SDCSUtils.tmpArchetype.Race,
				SDCSUtils.tmpArchetype.Variant.ToString("00"),
				"_Head.mat"
			});
		}
	}

	// Token: 0x17000888 RID: 2184
	// (get) Token: 0x06004F95 RID: 20373 RVA: 0x001E3AE4 File Offset: 0x001E1CE4
	public static string baseHandsMatLoc
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return string.Concat(new string[]
			{
				"@:Entities/Player/",
				SDCSUtils.tmpArchetype.Sex,
				"/Heads/",
				SDCSUtils.tmpArchetype.Race,
				"/",
				SDCSUtils.tmpArchetype.Variant.ToString("00"),
				"/Materials/player",
				SDCSUtils.tmpArchetype.Sex,
				SDCSUtils.tmpArchetype.Race,
				SDCSUtils.tmpArchetype.Variant.ToString("00"),
				"_Hand.mat"
			});
		}
	}

	// Token: 0x17000889 RID: 2185
	// (get) Token: 0x06004F96 RID: 20374 RVA: 0x0000A415 File Offset: 0x00008615
	public static string baseRigPrefab
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return "@:Entities/Player/Common/BaseRigs/baseRigPrefab.prefab";
		}
	}

	// Token: 0x1700088A RID: 2186
	// (get) Token: 0x06004F97 RID: 20375 RVA: 0x001E3B89 File Offset: 0x001E1D89
	public static string baseRigFPPrefab
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return "@:Entities/Player/Common/BaseRigs/baseRigFPPrefab.prefab";
		}
	}

	// Token: 0x1700088B RID: 2187
	// (get) Token: 0x06004F98 RID: 20376 RVA: 0x001E3B90 File Offset: 0x001E1D90
	public static RuntimeAnimatorController UIAnimController
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return DataLoader.LoadAsset<RuntimeAnimatorController>("@:Entities/Player/Common/AnimControllers/MenuSDCS" + SDCSUtils.tmpArchetype.Sex + "Controller" + (SDCSUtils.tmpArchetype.IsMale ? ".controller" : ".overrideController"), false);
		}
	}

	// Token: 0x1700088C RID: 2188
	// (get) Token: 0x06004F99 RID: 20377 RVA: 0x001E3BC9 File Offset: 0x001E1DC9
	public static RuntimeAnimatorController TPAnimController
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return DataLoader.LoadAsset<RuntimeAnimatorController>("@:Entities/Player/Common/AnimControllers/3PPlayer" + SDCSUtils.tmpArchetype.Sex + "Controller" + (SDCSUtils.tmpArchetype.IsMale ? ".controller" : ".overrideController"), false);
		}
	}

	// Token: 0x1700088D RID: 2189
	// (get) Token: 0x06004F9A RID: 20378 RVA: 0x001E3C02 File Offset: 0x001E1E02
	public static RuntimeAnimatorController FPAnimController
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return DataLoader.LoadAsset<RuntimeAnimatorController>("@:Entities/Player/Common/AnimControllers/FPPlayerController.controller", false);
		}
	}

	// Token: 0x06004F9B RID: 20379 RVA: 0x001E3C10 File Offset: 0x001E1E10
	[PublicizedFrom(EAccessModifier.Private)]
	public static SDCSUtils.AssetRefs RemoveAssetOwnerForRig(GameObject rig)
	{
		if (rig == null)
		{
			return null;
		}
		int instanceID = rig.GetInstanceID();
		SDCSUtils.AssetRefs assetRefs;
		SDCSUtils.rigOwnedAssets.Remove(instanceID, out assetRefs);
		return assetRefs;
	}

	// Token: 0x06004F9C RID: 20380 RVA: 0x001E3C40 File Offset: 0x001E1E40
	[PublicizedFrom(EAccessModifier.Private)]
	public static SDCSUtils.AssetRefs GetAssetOwnerForRig(GameObject rig)
	{
		int instanceID = rig.GetInstanceID();
		SDCSUtils.AssetRefs assetRefs;
		if (!SDCSUtils.rigOwnedAssets.TryGetValue(instanceID, out assetRefs))
		{
			assetRefs = new SDCSUtils.AssetRefs();
			SDCSUtils.rigOwnedAssets.Add(instanceID, assetRefs);
		}
		return assetRefs;
	}

	// Token: 0x06004F9D RID: 20381 RVA: 0x001E3C78 File Offset: 0x001E1E78
	public static void CreateVizTP(Archetype _archetype, ref GameObject baseRig, ref SDCSUtils.TransformCatalog boneCatalog, EntityAlive entity, bool isFPV)
	{
		SDCSUtils.AssetRefs assetRefs = SDCSUtils.RemoveAssetOwnerForRig(baseRig);
		SDCSUtils.DestroyViz(baseRig, true);
		SDCSUtils.tmpArchetype = _archetype;
		SDCSUtils.setupRig(ref baseRig, ref boneCatalog, SDCSUtils.baseRigPrefab, null, SDCSUtils.TPAnimController);
		SDCSUtils.tmpAssetsOwner = SDCSUtils.GetAssetOwnerForRig(baseRig);
		if (!isFPV)
		{
			SDCSUtils.setupBase(baseRig, boneCatalog, SDCSUtils.baseParts, isFPV);
			SDCSUtils.setupEquipment(baseRig, boneCatalog, SDCSUtils.ignoredParts, entity, false, !(entity is EntityPlayerLocal), false);
			SDCSUtils.setupHairObjects(baseRig, boneCatalog, SDCSUtils.ignoredParts, entity, false);
		}
		if (assetRefs == null)
		{
			return;
		}
		assetRefs.ReleaseAssets();
	}

	// Token: 0x06004F9E RID: 20382 RVA: 0x001E3D04 File Offset: 0x001E1F04
	public static void CreateVizFP(Archetype _archetype, ref GameObject baseRigFP, ref SDCSUtils.TransformCatalog boneCatalogFP, EntityAlive entity, bool isFPV)
	{
		SDCSUtils.AssetRefs assetRefs = SDCSUtils.RemoveAssetOwnerForRig(baseRigFP);
		SDCSUtils.DestroyViz(baseRigFP, true);
		SDCSUtils.tmpArchetype = _archetype;
		Transform transform = entity.transform.FindInChildren("Camera");
		if (transform == null)
		{
			if (!(GameObject.Find("Camera") != null))
			{
				Log.Error("Unable to find first person camera!");
				return;
			}
			transform = GameObject.Find("Camera").transform;
		}
		Transform transform2 = transform.FindInChildren("UFPSRoot");
		SDCSUtils.setupRig(ref baseRigFP, ref boneCatalogFP, SDCSUtils.baseRigFPPrefab, transform2, SDCSUtils.FPAnimController);
		SDCSUtils.tmpAssetsOwner = SDCSUtils.GetAssetOwnerForRig(baseRigFP);
		if (isFPV)
		{
			SDCSUtils.setupBase(baseRigFP, boneCatalogFP, SDCSUtils.basePartsFP, isFPV);
			SDCSUtils.setupEquipment(baseRigFP, boneCatalogFP, SDCSUtils.ignoredPartsFP, entity, false, !(entity is EntityPlayerLocal), false);
			SDCSUtils.setupHairObjects(baseRigFP, boneCatalogFP, SDCSUtils.ignoredPartsFP, entity, false);
		}
		if (assetRefs != null)
		{
			assetRefs.ReleaseAssets();
		}
		baseRigFP.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
		baseRigFP.name = "baseRigFP";
		baseRigFP.AddMissingComponent<AnimationEventBridge>();
		SkinnedMeshRenderer[] componentsInChildren = baseRigFP.GetComponentsInChildren<SkinnedMeshRenderer>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].gameObject.layer = LayerMask.NameToLayer("HoldingItem");
		}
		foreach (HingeJoint hingeJoint in baseRigFP.GetComponentsInChildren<HingeJoint>())
		{
			if (hingeJoint.connectedBody == null)
			{
				hingeJoint.gameObject.SetActive(false);
			}
		}
		baseRigFP.GetComponentsInChildren<Cloth>(SDCSUtils.tempCloths);
		foreach (Cloth cloth in SDCSUtils.tempCloths)
		{
			cloth.enabled = false;
		}
		SDCSUtils.tempCloths.Clear();
	}

	// Token: 0x06004F9F RID: 20383 RVA: 0x001E3EDC File Offset: 0x001E20DC
	public static void CreateVizUI(Archetype _archetype, ref GameObject baseRigUI, ref SDCSUtils.TransformCatalog boneCatalogUI, EntityAlive entity, bool useTempCosmetics)
	{
		SDCSUtils.AssetRefs assetRefs = SDCSUtils.RemoveAssetOwnerForRig(baseRigUI);
		SDCSUtils.DestroyViz(baseRigUI, true);
		SDCSUtils.tmpArchetype = _archetype;
		SDCSUtils.setupRig(ref baseRigUI, ref boneCatalogUI, SDCSUtils.baseRigPrefab, null, SDCSUtils.UIAnimController);
		SDCSUtils.tmpAssetsOwner = SDCSUtils.GetAssetOwnerForRig(baseRigUI);
		SDCSUtils.setupBase(baseRigUI, boneCatalogUI, SDCSUtils.baseParts, false);
		SDCSUtils.setupEquipment(baseRigUI, boneCatalogUI, SDCSUtils.ignoredParts, entity, true, useTempCosmetics, useTempCosmetics);
		SDCSUtils.setupHairObjects(baseRigUI, boneCatalogUI, SDCSUtils.ignoredParts, entity, true);
		if (assetRefs != null)
		{
			assetRefs.ReleaseAssets();
		}
		Transform transform = baseRigUI.transform.Find("IKRig");
		if (transform != null)
		{
			transform.GetComponent<Rig>().weight = 0f;
		}
		foreach (HingeJoint hingeJoint in baseRigUI.GetComponentsInChildren<HingeJoint>())
		{
			if (hingeJoint.connectedBody == null)
			{
				Log.Warning("SDCSUtils::CreateVizUI: No connected body for " + hingeJoint.transform.name + "'s HingeJoint! Disabling for UI until this is solved.");
				hingeJoint.gameObject.SetActive(false);
			}
		}
		SDCSUtils.fixCloths(baseRigUI);
	}

	// Token: 0x06004FA0 RID: 20384 RVA: 0x001E3FE0 File Offset: 0x001E21E0
	public static void CreateVizUI(Archetype _archetype, ref GameObject baseRigUI, ref SDCSUtils.TransformCatalog boneCatalogUI)
	{
		SDCSUtils.AssetRefs assetRefs = SDCSUtils.RemoveAssetOwnerForRig(baseRigUI);
		SDCSUtils.DestroyViz(baseRigUI, false);
		SDCSUtils.tmpArchetype = _archetype;
		SDCSUtils.setupRig(ref baseRigUI, ref boneCatalogUI, SDCSUtils.baseRigPrefab, null, SDCSUtils.UIAnimController);
		SDCSUtils.tmpAssetsOwner = SDCSUtils.GetAssetOwnerForRig(baseRigUI);
		SDCSUtils.setupBase(baseRigUI, boneCatalogUI, SDCSUtils.baseParts, false);
		SDCSUtils.setupHairObjects(baseRigUI, boneCatalogUI, null, false, SDCSUtils.ignoredParts, true, _archetype.Hair, _archetype.MustacheName, _archetype.ChopsName, _archetype.BeardName);
		SDCSUtils.setupEquipment(baseRigUI, boneCatalogUI, SDCSUtils.ignoredParts, true, _archetype.Equipment, false);
		if (assetRefs != null)
		{
			assetRefs.ReleaseAssets();
		}
		Transform transform = baseRigUI.transform.Find("IKRig");
		if (transform != null)
		{
			transform.GetComponent<Rig>().weight = 0f;
		}
		foreach (HingeJoint hingeJoint in baseRigUI.GetComponentsInChildren<HingeJoint>())
		{
			if (hingeJoint.connectedBody == null)
			{
				Log.Warning("SDCSUtils::CreateVizUI: No connected body for " + hingeJoint.transform.name + "'s HingeJoint! Disabling for UI until this is solved.");
				hingeJoint.gameObject.SetActive(false);
			}
		}
		SDCSUtils.fixCloths(baseRigUI);
	}

	// Token: 0x06004FA1 RID: 20385 RVA: 0x001E4100 File Offset: 0x001E2300
	[PublicizedFrom(EAccessModifier.Private)]
	public static void DestroyViz(GameObject _baseRig, bool _keepRig = false)
	{
		if (_baseRig)
		{
			Transform transform = _baseRig.transform;
			for (int i = 0; i < transform.childCount; i++)
			{
				Transform child = transform.GetChild(i);
				if (child.name != "Origin")
				{
					child.GetComponentsInChildren<SkinnedMeshRenderer>(true, SDCSUtils.tempSMRs);
					foreach (SkinnedMeshRenderer skinnedMeshRenderer in SDCSUtils.tempSMRs)
					{
						if (skinnedMeshRenderer != null)
						{
							Mesh sharedMesh = skinnedMeshRenderer.sharedMesh;
							if (MeshMorph.IsInstance(sharedMesh))
							{
								global::UnityEngine.Object.Destroy(sharedMesh);
							}
							skinnedMeshRenderer.GetSharedMaterials(SDCSUtils.tempMats);
							Utils.CleanupMaterials<List<Material>>(SDCSUtils.tempMats);
							SDCSUtils.tempMats.Clear();
						}
					}
				}
			}
			if (!_keepRig)
			{
				global::UnityEngine.Object.DestroyImmediate(_baseRig);
			}
		}
	}

	// Token: 0x06004FA2 RID: 20386 RVA: 0x001E41E8 File Offset: 0x001E23E8
	public static void UnloadViz(GameObject _baseRig)
	{
		if (_baseRig == null)
		{
			return;
		}
		SDCSUtils.AssetRefs assetRefs = SDCSUtils.RemoveAssetOwnerForRig(_baseRig);
		SDCSUtils.DestroyViz(_baseRig, false);
		assetRefs.ReleaseAssets();
	}

	// Token: 0x06004FA3 RID: 20387 RVA: 0x001E4208 File Offset: 0x001E2408
	public static void SetVisible(GameObject _baseRigUI, bool _visible)
	{
		if (_baseRigUI)
		{
			Transform transform = _baseRigUI.transform;
			for (int i = 0; i < transform.childCount; i++)
			{
				Transform child = transform.GetChild(i);
				if (child.name != "Origin")
				{
					child.GetComponentsInChildren<SkinnedMeshRenderer>(true, SDCSUtils.tempSMRs);
					foreach (SkinnedMeshRenderer skinnedMeshRenderer in SDCSUtils.tempSMRs)
					{
						skinnedMeshRenderer.gameObject.SetActive(_visible);
					}
				}
			}
		}
	}

	// Token: 0x06004FA4 RID: 20388 RVA: 0x001E42A4 File Offset: 0x001E24A4
	[PublicizedFrom(EAccessModifier.Private)]
	public static void setupRig(ref GameObject _rigObj, ref SDCSUtils.TransformCatalog _boneCatalog, string prefabLocation, Transform parent, RuntimeAnimatorController animController)
	{
		if (!_rigObj)
		{
			_rigObj = global::UnityEngine.Object.Instantiate<GameObject>(DataLoader.LoadAsset<GameObject>(prefabLocation, false), parent);
			_boneCatalog = new SDCSUtils.TransformCatalog(_rigObj.transform);
			BoneRenderer[] componentsInChildren = _rigObj.GetComponentsInChildren<BoneRenderer>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].enabled = false;
			}
		}
		else
		{
			SDCSUtils.cleanupEquipment(_rigObj, _boneCatalog);
		}
		Animator component = _rigObj.GetComponent<Animator>();
		if (component && component.runtimeAnimatorController != animController)
		{
			component.runtimeAnimatorController = animController;
		}
		if (!SDCSUtils.tmpArchetype.IsMale)
		{
			CapsuleCollider orAddComponent = _boneCatalog["Hips"].gameObject.GetOrAddComponent<CapsuleCollider>();
			orAddComponent.center = new Vector3(0f, 0f, -0.03f);
			orAddComponent.radius = 0.15f;
			orAddComponent.height = 0.375f;
		}
	}

	// Token: 0x06004FA5 RID: 20389 RVA: 0x001E437C File Offset: 0x001E257C
	[PublicizedFrom(EAccessModifier.Private)]
	public static void cleanupEquipment(GameObject _rigObj, SDCSUtils.TransformCatalog _boneCatalog)
	{
		SDCSUtils.SlotAllowedBonesCache.Clear();
		RigBuilder component = _rigObj.GetComponent<RigBuilder>();
		if (component)
		{
			List<RigLayer> layers = component.layers;
			for (int i = layers.Count - 1; i >= 0; i--)
			{
				if (layers[i].name != "IKRig")
				{
					layers.RemoveAt(i);
				}
			}
			component.Clear();
		}
		Animator component2 = _rigObj.GetComponent<Animator>();
		if (component2)
		{
			component2.UnbindAllStreamHandles();
		}
		GameUtils.DestroyAllChildrenImmediatelyBut(_rigObj.transform, new List<string> { "Origin", "IKRig" });
		SDCSUtils.SanitizeRig(_rigObj, _boneCatalog);
	}

	// Token: 0x06004FA6 RID: 20390 RVA: 0x001E4420 File Offset: 0x001E2620
	[PublicizedFrom(EAccessModifier.Private)]
	public static void SanitizeRig(GameObject _rigObj, SDCSUtils.TransformCatalog _boneCatalog)
	{
		AuxBoneTracker auxBoneTracker = null;
		if (_boneCatalog != null && _boneCatalog["Origin"] != null)
		{
			auxBoneTracker = _boneCatalog["Origin"].GetComponent<AuxBoneTracker>();
		}
		if (auxBoneTracker)
		{
			foreach (string text in auxBoneTracker.AuxBoneLookup.Keys)
			{
				if (_boneCatalog.ContainsKey(text) && _boneCatalog[text] != null)
				{
					global::UnityEngine.Object.DestroyImmediate(_boneCatalog[text].gameObject);
					_boneCatalog.Remove(text);
				}
			}
			auxBoneTracker.AuxBoneLookup.Clear();
		}
		List<string> list = new List<string>();
		foreach (KeyValuePair<string, Transform> keyValuePair in _boneCatalog)
		{
			if (keyValuePair.Value == null)
			{
				list.Add(keyValuePair.Key);
			}
		}
		foreach (string text2 in list)
		{
			_boneCatalog.Remove(text2);
		}
	}

	// Token: 0x06004FA7 RID: 20391 RVA: 0x001E457C File Offset: 0x001E277C
	[PublicizedFrom(EAccessModifier.Private)]
	public static void setupBase(GameObject _rig, SDCSUtils.TransformCatalog _boneCatalog, string[] baseParts, bool isFPV)
	{
		foreach (string text in baseParts)
		{
			GameObject gameObject;
			if (text == "head")
			{
				gameObject = DataLoader.LoadAsset<GameObject>(SDCSUtils.baseHeadLoc, false);
			}
			else if (text == "hands")
			{
				gameObject = DataLoader.LoadAsset<GameObject>(SDCSUtils.baseBodyLoc, false);
			}
			else
			{
				gameObject = DataLoader.LoadAsset<GameObject>(SDCSUtils.baseBodyLoc, false);
			}
			if (gameObject == null)
			{
				return;
			}
			GameObject bodyPartContainingName;
			if (!((bodyPartContainingName = SDCSUtils.getBodyPartContainingName(gameObject.transform, text)) == null))
			{
				bodyPartContainingName.name = text;
				GameObject gameObject2 = bodyPartContainingName;
				EModelSDCS emodelSDCS = null;
				Material material = DataLoader.LoadAsset<Material>(SDCSUtils.baseEyeColorMatLoc, false);
				SDCSUtils.Stitch(gameObject2, _rig, _boneCatalog, emodelSDCS, isFPV, false, material, false);
				if (text == "head")
				{
					Transform transform = _rig.transform;
					Transform transform2 = gameObject.transform;
					CharacterGazeController orAddComponent = transform.FindRecursive("Head").gameObject.GetOrAddComponent<CharacterGazeController>();
					orAddComponent.rootTransform = transform.FindRecursive("Origin");
					orAddComponent.neckTransform = _boneCatalog["Neck"];
					orAddComponent.headTransform = _boneCatalog["Head"];
					orAddComponent.leftEyeTransform = _boneCatalog["LeftEye"];
					orAddComponent.rightEyeTransform = _boneCatalog["RightEye"];
					orAddComponent.eyeSkinnedMeshRenderer = transform.FindRecursive("eyes").GetComponent<SkinnedMeshRenderer>();
					orAddComponent.leftEyeLocalPosition = transform2.FindInChildren("LeftEye").localPosition;
					orAddComponent.rightEyeLocalPosition = transform2.FindInChildren("RightEye").localPosition;
					orAddComponent.eyeLookAtTargetAngle = 35f;
					orAddComponent.eyeRotationSpeed = 30f;
					orAddComponent.twitchSpeed = 25f;
					orAddComponent.headLookAtTargetAngle = 75f;
					orAddComponent.headRotationSpeed = 7f;
					orAddComponent.maxLookAtDistance = 5f;
					EyeLidController orAddComponent2 = transform.FindRecursive("Head").gameObject.GetOrAddComponent<EyeLidController>();
					orAddComponent2.leftTopTransform = _boneCatalog["LeftEyelidTop"];
					orAddComponent2.leftBottomTransform = _boneCatalog["LeftEyelidBot"];
					orAddComponent2.rightTopTransform = _boneCatalog["RightEyelidTop"];
					orAddComponent2.rightBottomTransform = _boneCatalog["RightEyelidBot"];
					orAddComponent2.leftTopLocalPosition = transform2.FindInChildren("LeftEyelidTop").localPosition;
					orAddComponent2.leftBottomLocalPosition = transform2.FindInChildren("LeftEyelidBot").localPosition;
					orAddComponent2.leftTopRotation = transform2.FindInChildren("LeftEyelidTop").localRotation;
					orAddComponent2.leftBottomRotation = transform2.FindInChildren("LeftEyelidBot").localRotation;
					orAddComponent2.rightTopLocalPosition = transform2.FindInChildren("RightEyelidTop").localPosition;
					orAddComponent2.rightBottomLocalPosition = transform2.FindInChildren("RightEyelidBot").localPosition;
					orAddComponent2.rightTopRotation = transform2.FindInChildren("RightEyelidTop").localRotation;
					orAddComponent2.rightBottomRotation = transform2.FindInChildren("RightEyelidBot").localRotation;
				}
			}
		}
	}

	// Token: 0x06004FA8 RID: 20392 RVA: 0x001E4854 File Offset: 0x001E2A54
	[PublicizedFrom(EAccessModifier.Private)]
	public static void setupEquipment(GameObject _rig, SDCSUtils.TransformCatalog _boneCatalog, string[] ignoredParts, bool isUI, List<SDCSUtils.SlotData> slotData, bool _ignoreDlcEntitlements)
	{
		if (slotData == null)
		{
			return;
		}
		List<Transform> list = new List<Transform>();
		Transform transform = _rig.transform.Find("Origin");
		if (transform)
		{
			List<Transform> list2 = SDCSUtils.findStartsWith(transform, "RigConstraints");
			int count = list2.Count;
			for (int i = 0; i < count; i++)
			{
				global::UnityEngine.Object.DestroyImmediate(list2[i].gameObject);
			}
		}
		if (SDCSUtils.tmpArchetype.Equipment == null)
		{
			SDCSUtils.tmpArchetype.Equipment = new List<SDCSUtils.SlotData>();
			foreach (SDCSUtils.SlotData slotData2 in slotData)
			{
				SDCSUtils.tmpArchetype.Equipment.Add(slotData2);
			}
		}
		foreach (SDCSUtils.SlotData slotData3 in slotData)
		{
			GameObject gameObject = null;
			if ("head".Equals(slotData3.PartName, StringComparison.OrdinalIgnoreCase) && slotData3.PrefabName != null && slotData3.PrefabName.Contains("HeadGearMorphMatrix", StringComparison.OrdinalIgnoreCase))
			{
				gameObject = SDCSUtils.setupHeadgearMorph(_rig, _boneCatalog, null, false, isUI, slotData3, _ignoreDlcEntitlements);
			}
			else
			{
				Transform transform2 = SDCSUtils.setupEquipmentSlot(_rig, _boneCatalog, ignoredParts, slotData3, list, _ignoreDlcEntitlements);
				if (transform2)
				{
					gameObject = SDCSUtils.Stitch(transform2.gameObject, _rig, _boneCatalog, null, false, isUI, null, false);
					if (gameObject != null)
					{
						Morphable componentInChildren = gameObject.GetComponentInChildren<Morphable>();
						if (componentInChildren)
						{
							componentInChildren.MorphHeadgear(SDCSUtils.tmpArchetype, _ignoreDlcEntitlements);
						}
					}
				}
			}
			if (gameObject != null)
			{
				ColorSwatchApplicator componentInChildren2 = gameObject.GetComponentInChildren<ColorSwatchApplicator>();
				if (componentInChildren2 != null)
				{
					componentInChildren2.ApplyColorSwatch(SDCSUtils.tmpArchetype.HairColor);
				}
			}
		}
		SDCSUtils.setupEquipmentCommon(_rig, _boneCatalog, list);
	}

	// Token: 0x06004FA9 RID: 20393 RVA: 0x001E4A38 File Offset: 0x001E2C38
	[PublicizedFrom(EAccessModifier.Private)]
	public static void setupEquipment(GameObject _rig, SDCSUtils.TransformCatalog _boneCatalog, string[] ignoredParts, EntityAlive entity, bool isUI, bool _ignoreDlcEntitlements, bool useTempCosmetics = false)
	{
		if (!entity)
		{
			return;
		}
		EModelSDCS emodelSDCS = entity.emodel as EModelSDCS;
		if (!emodelSDCS)
		{
			return;
		}
		emodelSDCS.HairMaskType = SDCSUtils.SlotData.HairMaskTypes.Full;
		emodelSDCS.FacialHairMaskType = SDCSUtils.SlotData.HairMaskTypes.Full;
		if (!isUI && emodelSDCS.IsFPV)
		{
			emodelSDCS.ClipMaterialsFP.Clear();
		}
		List<Transform> list = new List<Transform>();
		Transform transform = _rig.transform.Find("Origin");
		if (transform)
		{
			List<Transform> list2 = SDCSUtils.findStartsWith(transform, "RigConstraints");
			int count = list2.Count;
			for (int i = 0; i < count; i++)
			{
				global::UnityEngine.Object.DestroyImmediate(list2[i].gameObject);
			}
		}
		int slotCount = entity.equipment.GetSlotCount();
		if (SDCSUtils.tmpArchetype.Equipment != null)
		{
			SDCSUtils.tmpArchetype.Equipment.Clear();
		}
		else
		{
			SDCSUtils.tmpArchetype.Equipment = new List<SDCSUtils.SlotData>();
		}
		for (int j = 0; j < slotCount; j++)
		{
			ItemValue slotItem = entity.equipment.GetSlotItem(j);
			ItemClass cosmeticSlot = entity.equipment.GetCosmeticSlot(j, useTempCosmetics);
			object obj;
			if (slotItem == null)
			{
				obj = null;
			}
			else
			{
				ItemClass itemClass = slotItem.ItemClass;
				obj = ((itemClass != null) ? itemClass.SDCSData : null);
			}
			bool flag = obj != null;
			bool flag2 = cosmeticSlot != null && (useTempCosmetics || (flag && (_ignoreDlcEntitlements || entity.equipment.HasCosmeticUnlocked(cosmeticSlot).Item1)));
			if ((flag || flag2) && cosmeticSlot != ItemClass.MissingItem)
			{
				ItemClass itemClass2 = (flag2 ? cosmeticSlot : slotItem.ItemClass);
				SDCSUtils.SlotData sdcsdata = itemClass2.SDCSData;
				if (cosmeticSlot != null && !flag2)
				{
					entity.equipment.SetCosmeticSlot(j, 0);
					entity.bPlayerStatsChanged = true;
				}
				ItemClassArmor itemClassArmor = itemClass2 as ItemClassArmor;
				if (itemClassArmor != null && itemClassArmor.EquipSlot == EquipmentSlots.Head)
				{
					emodelSDCS.HairMaskType = sdcsdata.HairMaskType;
					emodelSDCS.FacialHairMaskType = sdcsdata.FacialHairMaskType;
				}
				SDCSUtils.tmpArchetype.Equipment.Add(sdcsdata);
			}
		}
		foreach (SDCSUtils.SlotData slotData in SDCSUtils.tmpArchetype.Equipment)
		{
			GameObject gameObject = null;
			if ("head".Equals(slotData.PartName, StringComparison.OrdinalIgnoreCase) && slotData.PrefabName != null && slotData.PrefabName.Contains("HeadGearMorphMatrix", StringComparison.OrdinalIgnoreCase))
			{
				gameObject = SDCSUtils.setupHeadgearMorph(_rig, _boneCatalog, emodelSDCS, emodelSDCS.IsFPV, isUI, slotData, _ignoreDlcEntitlements);
			}
			else if (!"head".Equals(slotData.PartName, StringComparison.OrdinalIgnoreCase) || !emodelSDCS.IsFPV || isUI)
			{
				Transform transform2 = SDCSUtils.setupEquipmentSlot(_rig, _boneCatalog, ignoredParts, slotData, list, _ignoreDlcEntitlements);
				if (transform2)
				{
					gameObject = SDCSUtils.Stitch(transform2.gameObject, _rig, _boneCatalog, emodelSDCS, emodelSDCS.IsFPV, isUI, null, true);
					if (gameObject != null)
					{
						Morphable componentInChildren = gameObject.GetComponentInChildren<Morphable>();
						if (componentInChildren)
						{
							componentInChildren.MorphHeadgear(SDCSUtils.tmpArchetype, _ignoreDlcEntitlements);
						}
					}
				}
			}
			if (gameObject != null)
			{
				ColorSwatchApplicator componentInChildren2 = gameObject.GetComponentInChildren<ColorSwatchApplicator>();
				if (componentInChildren2 != null)
				{
					componentInChildren2.ApplyColorSwatch(SDCSUtils.tmpArchetype.HairColor);
				}
			}
		}
		SDCSUtils.setupEquipmentCommon(_rig, _boneCatalog, list);
	}

	// Token: 0x06004FAA RID: 20394 RVA: 0x001E4D78 File Offset: 0x001E2F78
	[PublicizedFrom(EAccessModifier.Private)]
	public static Transform setupEquipmentSlot(GameObject _rig, SDCSUtils.TransformCatalog _boneCatalog, string[] ignoredParts, SDCSUtils.SlotData wornItem, List<Transform> allGears, bool _ignoreDlcEntitlements)
	{
		string text = SDCSUtils.GetPathForSlotData(wornItem, true);
		if (string.IsNullOrEmpty(text))
		{
			return null;
		}
		LoadManager.AssetRequestTask<GameObject> assetRequestTask = LoadManager.LoadAsset<GameObject>(text, null, null, false, true, _ignoreDlcEntitlements);
		if (assetRequestTask.Asset == null && wornItem.PartName == "head")
		{
			text = SDCSUtils.GetPathForSlotData(wornItem, false);
			assetRequestTask = LoadManager.LoadAsset<GameObject>(text, null, null, false, true, _ignoreDlcEntitlements);
		}
		if (!assetRequestTask.Asset)
		{
			Log.Warning(string.Concat(new string[] { "SDCSUtils::", text, " not found for item ", wornItem.PrefabName, "!" }));
			return null;
		}
		GameObject asset = assetRequestTask.Asset;
		SDCSUtils.tmpAssetsOwner.AddAssetHandle(assetRequestTask.GetHandle());
		assetRequestTask.Release();
		string text2 = "";
		for (int i = 0; i < SDCSUtils.tmpArchetype.Equipment.Count; i++)
		{
			SDCSUtils.SlotData slotData = SDCSUtils.tmpArchetype.Equipment[i];
			if (slotData.PartName.Equals("body", StringComparison.OrdinalIgnoreCase))
			{
				text2 = SDCSUtils.GetPathForSlotData(slotData, false);
				break;
			}
		}
		Transform clothingPartWithName = SDCSUtils.getClothingPartWithName(asset, SDCSUtils.getPartNameWithVariant(wornItem.PartName, text, text2));
		if (clothingPartWithName)
		{
			HashSet<string> hashSet = SDCSUtils.CollectRequiredNamesForSlot(asset.transform, clothingPartWithName);
			SDCSUtils.SlotAllowedBonesCache.Set(clothingPartWithName, hashSet);
			if (!allGears.Contains(clothingPartWithName))
			{
				allGears.Add(clothingPartWithName);
			}
			string baseToTurnOff = wornItem.BaseToTurnOff;
			if (baseToTurnOff != null && baseToTurnOff.Length > 0)
			{
				foreach (string text3 in wornItem.BaseToTurnOff.Split(',', StringSplitOptions.None))
				{
					Transform transform = _rig.transform.FindInChildren(text3);
					if (transform)
					{
						global::UnityEngine.Object.Destroy(transform.gameObject);
					}
				}
			}
			if (!clothingPartWithName.gameObject.activeSelf)
			{
				clothingPartWithName.gameObject.SetActive(true);
			}
			SDCSUtils.MatchRigs(asset.transform, _rig.transform, _boneCatalog, hashSet);
		}
		return clothingPartWithName;
	}

	// Token: 0x06004FAB RID: 20395 RVA: 0x001E4F74 File Offset: 0x001E3174
	[PublicizedFrom(EAccessModifier.Private)]
	public static string getPartNameWithVariant(string partName, string sourceGearPath, string targetBodyPath)
	{
		if (SDCSUtils.tmpArchetype == null || SDCSUtils.tmpArchetype.Equipment == null)
		{
			return partName;
		}
		string text = string.Empty;
		if (GearVariantMatrixSO.Instance != null)
		{
			text = GearVariantMatrixSO.Instance.GetVariantOrEmpty(SDCSUtils.tmpArchetype.Sex, partName, sourceGearPath, targetBodyPath);
		}
		else
		{
			Log.Warning("SDCSUtils::getPartNameWithVariant: No GearVariantMatrixSO instance found!");
		}
		if (string.IsNullOrEmpty(text))
		{
			return partName;
		}
		return partName + "_" + text;
	}

	// Token: 0x06004FAC RID: 20396 RVA: 0x001E4FE4 File Offset: 0x001E31E4
	[PublicizedFrom(EAccessModifier.Private)]
	public static void setupEquipmentCommon(GameObject _rigObj, SDCSUtils.TransformCatalog _boneCatalog, List<Transform> allGears)
	{
		RigBuilder orAddComponent = _rigObj.GetOrAddComponent<RigBuilder>();
		orAddComponent.enabled = false;
		foreach (Transform transform in allGears)
		{
			HashSet<string> hashSet;
			if (!SDCSUtils.SlotAllowedBonesCache.TryGet(transform, out hashSet) || hashSet == null || hashSet.Count == 0)
			{
				Debug.LogWarning("[SDCS] No required leaves cached for slot '" + transform.name + "'. Skipping constraints.");
			}
			else
			{
				SDCSUtils.SetupRigConstraints(orAddComponent, transform, _rigObj.transform, _boneCatalog, hashSet);
			}
		}
		foreach (HingeJoint hingeJoint in _rigObj.GetComponentsInChildren<HingeJoint>())
		{
			if (hingeJoint.connectedBody != null && _boneCatalog.ContainsKey(hingeJoint.connectedBody.transform.name))
			{
				hingeJoint.connectedBody = _boneCatalog[hingeJoint.connectedBody.transform.name].GetComponent<Rigidbody>();
			}
			hingeJoint.autoConfigureConnectedAnchor = true;
		}
		orAddComponent.enabled = true;
	}

	// Token: 0x06004FAD RID: 20397 RVA: 0x001E50F8 File Offset: 0x001E32F8
	[PublicizedFrom(EAccessModifier.Private)]
	public static void setupHairObjects(GameObject _rig, SDCSUtils.TransformCatalog _boneCatalog, string[] ignoredParts, EntityAlive entity, bool isUI)
	{
		if (!entity)
		{
			return;
		}
		EModelSDCS emodelSDCS = entity.emodel as EModelSDCS;
		if (!emodelSDCS)
		{
			return;
		}
		if (!isUI && emodelSDCS.IsFPV)
		{
			emodelSDCS.ClipMaterialsFP.Clear();
		}
		if (!emodelSDCS.IsFPV || isUI)
		{
			SDCSUtils.setupHairObjects(_rig, _boneCatalog, emodelSDCS, emodelSDCS.IsFPV, ignoredParts, isUI, SDCSUtils.tmpArchetype.Hair, SDCSUtils.tmpArchetype.MustacheName, SDCSUtils.tmpArchetype.ChopsName, SDCSUtils.tmpArchetype.BeardName);
		}
	}

	// Token: 0x06004FAE RID: 20398 RVA: 0x001E5184 File Offset: 0x001E3384
	[PublicizedFrom(EAccessModifier.Private)]
	public static void setupHairObjects(GameObject _rig, SDCSUtils.TransformCatalog _boneCatalog, EModelSDCS _emodel, bool _isFPV, string[] ignoredParts, bool isUI, string hairName, string mustacheName, string chopsName, string beardName)
	{
		HairColorSwatch hairColorSwatch = null;
		if (!string.IsNullOrEmpty(SDCSUtils.tmpArchetype.HairColor))
		{
			string text = SDCSUtils.baseHairColorLoc + "/" + SDCSUtils.tmpArchetype.HairColor + ".asset";
			ScriptableObject scriptableObject = DataLoader.LoadAsset<ScriptableObject>(text, false);
			if (scriptableObject == null)
			{
				Log.Warning(string.Concat(new string[]
				{
					"SDCSUtils::",
					text,
					" not found for hair color ",
					SDCSUtils.tmpArchetype.HairColor,
					"!"
				}));
			}
			else
			{
				hairColorSwatch = scriptableObject as HairColorSwatch;
			}
		}
		if (!string.IsNullOrEmpty(hairName))
		{
			if (_emodel != null)
			{
				if (_emodel.HairMaskType != SDCSUtils.SlotData.HairMaskTypes.None)
				{
					string text2 = "";
					if (_emodel.HairMaskType != SDCSUtils.SlotData.HairMaskTypes.Full)
					{
						text2 = "_" + _emodel.HairMaskType.ToString().ToLower();
					}
					SDCSUtils.setupHair(_rig, _boneCatalog, _emodel, _isFPV, ignoredParts, isUI, string.Concat(new string[]
					{
						SDCSUtils.baseHairLoc,
						"/hair_",
						hairName,
						text2,
						".asset"
					}), hairName);
				}
			}
			else
			{
				SDCSUtils.setupHair(_rig, _boneCatalog, _emodel, _isFPV, ignoredParts, isUI, SDCSUtils.baseHairLoc + "/hair_" + hairName + ".asset", hairName);
			}
		}
		if (!string.IsNullOrEmpty(mustacheName))
		{
			if (_emodel != null)
			{
				if (_emodel.FacialHairMaskType != SDCSUtils.SlotData.HairMaskTypes.None)
				{
					SDCSUtils.setupHair(_rig, _boneCatalog, _emodel, _isFPV, ignoredParts, isUI, SDCSUtils.baseMustacheLoc + "/hair_facial_mustache" + mustacheName + ".asset", mustacheName);
				}
			}
			else
			{
				SDCSUtils.setupHair(_rig, _boneCatalog, _emodel, _isFPV, ignoredParts, isUI, SDCSUtils.baseMustacheLoc + "/hair_facial_mustache" + mustacheName + ".asset", mustacheName);
			}
		}
		if (!string.IsNullOrEmpty(chopsName))
		{
			if (_emodel != null)
			{
				if (_emodel.FacialHairMaskType != SDCSUtils.SlotData.HairMaskTypes.None)
				{
					SDCSUtils.setupHair(_rig, _boneCatalog, _emodel, _isFPV, ignoredParts, isUI, SDCSUtils.baseChopsLoc + "/hair_facial_sideburns" + chopsName + ".asset", chopsName);
				}
			}
			else
			{
				SDCSUtils.setupHair(_rig, _boneCatalog, _emodel, _isFPV, ignoredParts, isUI, SDCSUtils.baseChopsLoc + "/hair_facial_sideburns" + chopsName + ".asset", chopsName);
			}
		}
		if (!string.IsNullOrEmpty(beardName))
		{
			if (_emodel != null)
			{
				if (_emodel.FacialHairMaskType != SDCSUtils.SlotData.HairMaskTypes.None)
				{
					SDCSUtils.setupHair(_rig, _boneCatalog, _emodel, _isFPV, ignoredParts, isUI, SDCSUtils.baseBeardLoc + "/hair_facial_beard" + beardName + ".asset", beardName);
				}
			}
			else
			{
				SDCSUtils.setupHair(_rig, _boneCatalog, _emodel, _isFPV, ignoredParts, isUI, SDCSUtils.baseBeardLoc + "/hair_facial_beard" + beardName + ".asset", beardName);
			}
		}
		if (hairColorSwatch != null)
		{
			SDCSUtils.ApplySwatchToGameObject(_rig, hairColorSwatch);
		}
	}

	// Token: 0x06004FAF RID: 20399 RVA: 0x001E5410 File Offset: 0x001E3610
	[PublicizedFrom(EAccessModifier.Private)]
	public static void ApplySwatchToGameObject(GameObject targetGameObject, HairColorSwatch hairSwatch)
	{
		if (targetGameObject != null)
		{
			foreach (Renderer renderer in targetGameObject.GetComponentsInChildren<Renderer>(true))
			{
				Material[] array;
				if (Application.isPlaying)
				{
					array = renderer.materials;
				}
				else
				{
					array = renderer.sharedMaterials;
				}
				foreach (Material material in array)
				{
					if (material.shader.name == "Game/SDCS/Hair" && !material.name.Contains("lashes"))
					{
						hairSwatch.ApplyToMaterial(material);
					}
				}
			}
			return;
		}
		Debug.LogWarning("No target GameObject selected.");
	}

	// Token: 0x06004FB0 RID: 20400 RVA: 0x001E54B8 File Offset: 0x001E36B8
	[PublicizedFrom(EAccessModifier.Private)]
	public static void setupHair(GameObject _rig, SDCSUtils.TransformCatalog _boneCatalog, EModelSDCS _emodel, bool _isFPV, string[] ignoredParts, bool isUI, string path, string hairName)
	{
		if (string.IsNullOrEmpty(hairName))
		{
			return;
		}
		MeshMorph meshMorph = DataLoader.LoadAsset<MeshMorph>(path, false);
		GameObject gameObject = ((meshMorph != null) ? meshMorph.GetMorphedSkinnedMesh() : null);
		if (gameObject == null)
		{
			Log.Warning(string.Concat(new string[] { "SDCSUtils::", path, " not found for hair ", hairName, "!" }));
			return;
		}
		if (!gameObject.gameObject.activeSelf)
		{
			gameObject.gameObject.SetActive(true);
		}
		SDCSUtils.Stitch(gameObject.gameObject, _rig, _boneCatalog, _emodel, _isFPV, isUI, null, false);
		global::UnityEngine.Object.Destroy(gameObject);
	}

	// Token: 0x06004FB1 RID: 20401 RVA: 0x001E5554 File Offset: 0x001E3754
	[PublicizedFrom(EAccessModifier.Private)]
	public static string GetPathForSlotData(SDCSUtils.SlotData _slotData, bool _headgearShortHairMask = false)
	{
		if (_slotData == null)
		{
			return null;
		}
		if (string.IsNullOrEmpty(_slotData.PartName))
		{
			return null;
		}
		if (_slotData.PartName.Equals("head", StringComparison.OrdinalIgnoreCase))
		{
			if (SDCSUtils.ignoredParts.Contains("head"))
			{
				return null;
			}
			string text = _slotData.PrefabName;
			if (text.Contains("{sex}"))
			{
				text = text.Replace("{sex}", SDCSUtils.tmpArchetype.Sex);
			}
			if (text.Contains("{race}"))
			{
				text = text.Replace("{race}", SDCSUtils.tmpArchetype.Race);
			}
			if (text.Contains("{variant}"))
			{
				text = text.Replace("{variant}", SDCSUtils.tmpArchetype.Variant.ToString("00"));
			}
			if (text.Contains("{hair}"))
			{
				if (_headgearShortHairMask && (string.IsNullOrEmpty(SDCSUtils.tmpArchetype.Hair) || SDCSUtils.shortHairNames.ContainsCaseInsensitive(SDCSUtils.tmpArchetype.Hair)))
				{
					text = text.Replace("{hair}", "Bald");
				}
				else
				{
					text = text.Replace("{hair}", "");
				}
			}
			return text;
		}
		else
		{
			if (string.IsNullOrEmpty(_slotData.PrefabName))
			{
				return null;
			}
			foreach (string text2 in SDCSUtils.ignoredParts)
			{
				if (_slotData.PartName.Contains(text2, StringComparison.OrdinalIgnoreCase))
				{
					return null;
				}
			}
			return SDCSUtils.parseSexedLocation(_slotData.PrefabName, SDCSUtils.tmpArchetype.Sex);
		}
	}

	// Token: 0x06004FB2 RID: 20402 RVA: 0x001E56C4 File Offset: 0x001E38C4
	[PublicizedFrom(EAccessModifier.Private)]
	public static GameObject setupHeadgearMorph(GameObject _rig, SDCSUtils.TransformCatalog _boneCatalog, EModelSDCS _emodel, bool _isFPV, bool isUI, SDCSUtils.SlotData _slotData, bool _ignoreDlcEntitlements)
	{
		string text = SDCSUtils.GetPathForSlotData(_slotData, true);
		if (string.IsNullOrEmpty(text))
		{
			return null;
		}
		if (!isUI && _isFPV)
		{
			return null;
		}
		LoadManager.AssetRequestTask<MeshMorph> assetRequestTask = LoadManager.LoadAsset<MeshMorph>(text, null, null, false, true, _ignoreDlcEntitlements);
		if (assetRequestTask.Asset == null)
		{
			text = SDCSUtils.GetPathForSlotData(_slotData, false);
			assetRequestTask = LoadManager.LoadAsset<MeshMorph>(text, null, null, false, true, _ignoreDlcEntitlements);
		}
		MeshMorph asset = assetRequestTask.Asset;
		if (asset == null)
		{
			Log.Warning(string.Concat(new string[] { "SDCSUtils::", text, " not found for headgear ", _slotData.PrefabName, "!" }));
			return null;
		}
		GameObject morphedSkinnedMesh = asset.GetMorphedSkinnedMesh();
		if (morphedSkinnedMesh == null)
		{
			Log.Warning(string.Concat(new string[] { "SDCSUtils::", text, " no morphed skinned mesh for ", _slotData.PrefabName, "!" }));
			assetRequestTask.Release();
			return null;
		}
		SDCSUtils.tmpAssetsOwner.AddAssetHandle(assetRequestTask.GetHandle());
		assetRequestTask.Release();
		if (!morphedSkinnedMesh.gameObject.activeSelf)
		{
			morphedSkinnedMesh.gameObject.SetActive(true);
		}
		GameObject gameObject = SDCSUtils.Stitch(morphedSkinnedMesh.gameObject, _rig, _boneCatalog, _emodel, _isFPV, isUI, null, false);
		global::UnityEngine.Object.Destroy(morphedSkinnedMesh);
		return gameObject;
	}

	// Token: 0x06004FB3 RID: 20403 RVA: 0x001E5800 File Offset: 0x001E3A00
	public static bool BasePartsExist(Archetype _archetype)
	{
		SDCSUtils.tmpArchetype = _archetype;
		if (!DataLoader.LoadAsset<GameObject>(SDCSUtils.baseBodyLoc, false))
		{
			Log.Error("base body not found at " + SDCSUtils.baseBodyLoc);
			return false;
		}
		if (!DataLoader.LoadAsset<GameObject>(SDCSUtils.baseHeadLoc, false))
		{
			Log.Error("base head not found at " + SDCSUtils.baseHeadLoc);
			return false;
		}
		if (!DataLoader.LoadAsset<Material>(SDCSUtils.baseBodyMatLoc, false))
		{
			Log.Error("body material not found at " + SDCSUtils.baseBodyMatLoc);
			return false;
		}
		if (!DataLoader.LoadAsset<Material>(SDCSUtils.baseHeadMatLoc, false))
		{
			Log.Error("head material not found at " + SDCSUtils.baseHeadMatLoc);
			return false;
		}
		return true;
	}

	// Token: 0x06004FB4 RID: 20404 RVA: 0x001E58B4 File Offset: 0x001E3AB4
	[PublicizedFrom(EAccessModifier.Private)]
	public static List<Transform> findStartsWith(Transform parent, string key)
	{
		List<Transform> list = new List<Transform>();
		foreach (object obj in parent)
		{
			Transform transform = (Transform)obj;
			if (transform.name.StartsWith(key))
			{
				list.Add(transform);
			}
		}
		return list;
	}

	// Token: 0x06004FB5 RID: 20405 RVA: 0x001E5920 File Offset: 0x001E3B20
	[PublicizedFrom(EAccessModifier.Private)]
	public static GameObject getBodyPartContainingName(Transform parent, string name)
	{
		foreach (object obj in parent.transform)
		{
			Transform transform = (Transform)obj;
			if (transform.name.ToLower().Contains(name))
			{
				return transform.gameObject;
			}
		}
		return null;
	}

	// Token: 0x06004FB6 RID: 20406 RVA: 0x001E5994 File Offset: 0x001E3B94
	[PublicizedFrom(EAccessModifier.Private)]
	public static Transform getClothingPartWithName(GameObject clothingPrefab, string partName)
	{
		foreach (object obj in clothingPrefab.transform)
		{
			Transform transform = (Transform)obj;
			if (transform.name.ToLower() == partName.ToLower())
			{
				return transform;
			}
		}
		return null;
	}

	// Token: 0x06004FB7 RID: 20407 RVA: 0x0000B890 File Offset: 0x00009A90
	[PublicizedFrom(EAccessModifier.Private)]
	public static string parseSexedLocation(string sexedLocation, string sex)
	{
		return sexedLocation.Replace("{sex}", sex);
	}

	// Token: 0x06004FB8 RID: 20408 RVA: 0x001E5A08 File Offset: 0x001E3C08
	[PublicizedFrom(EAccessModifier.Private)]
	public static void fixCloths(GameObject root)
	{
		SDCSUtils.<>c__DisplayClass104_0 CS$<>8__locals1 = new SDCSUtils.<>c__DisplayClass104_0();
		CS$<>8__locals1.cloths = root.GetComponentsInChildren<Cloth>();
		if (CS$<>8__locals1.cloths.Length == 0)
		{
			return;
		}
		Cloth[] cloths = CS$<>8__locals1.cloths;
		for (int i = 0; i < cloths.Length; i++)
		{
			cloths[i].gameObject.SetActive(false);
		}
		ThreadManager.StartCoroutine(CS$<>8__locals1.<fixCloths>g__EnableClothLater|0());
	}

	// Token: 0x06004FBA RID: 20410 RVA: 0x001E5B68 File Offset: 0x001E3D68
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Internal)]
	public static void <MapSourceByName>g__Recurse|10_0(Transform t, ref SDCSUtils.<>c__DisplayClass10_0 A_1)
	{
		A_1.map[t.name] = t;
		for (int i = 0; i < t.childCount; i++)
		{
			SDCSUtils.<MapSourceByName>g__Recurse|10_0(t.GetChild(i), ref A_1);
		}
	}

	// Token: 0x04003D00 RID: 15616
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly List<Cloth> tempCloths = new List<Cloth>();

	// Token: 0x04003D01 RID: 15617
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly List<Material> tempMats = new List<Material>();

	// Token: 0x04003D02 RID: 15618
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly List<SkinnedMeshRenderer> tempSMRs = new List<SkinnedMeshRenderer>();

	// Token: 0x04003D03 RID: 15619
	public static readonly string[] shortHairNames = new string[] { "buzzcut", "cornrows", "flattop_fro", "mohawk", "small_fro" };

	// Token: 0x04003D04 RID: 15620
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly List<SkinnedMeshRenderer> _smrBuf = new List<SkinnedMeshRenderer>(64);

	// Token: 0x04003D05 RID: 15621
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly List<HingeJoint> _hingeBuf = new List<HingeJoint>(32);

	// Token: 0x04003D06 RID: 15622
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly List<BlendConstraint> _bcBuf = new List<BlendConstraint>(32);

	// Token: 0x04003D07 RID: 15623
	[PublicizedFrom(EAccessModifier.Private)]
	public const string ORIGIN = "Origin";

	// Token: 0x04003D08 RID: 15624
	[PublicizedFrom(EAccessModifier.Private)]
	public const string RIGCON = "RigConstraints";

	// Token: 0x04003D09 RID: 15625
	[PublicizedFrom(EAccessModifier.Private)]
	public const string IKRIG = "IKRig";

	// Token: 0x04003D0A RID: 15626
	public const string HEAD = "head";

	// Token: 0x04003D0B RID: 15627
	public const string EYES = "eyes";

	// Token: 0x04003D0C RID: 15628
	public const string BEARD = "beard";

	// Token: 0x04003D0D RID: 15629
	public const string HAIR = "hair";

	// Token: 0x04003D0E RID: 15630
	public const string BODY = "body";

	// Token: 0x04003D0F RID: 15631
	public const string HANDS = "hands";

	// Token: 0x04003D10 RID: 15632
	public const string FEET = "feet";

	// Token: 0x04003D11 RID: 15633
	public const string HELMET = "helmet";

	// Token: 0x04003D12 RID: 15634
	public const string TORSO = "torso";

	// Token: 0x04003D13 RID: 15635
	public const string GLOVES = "gloves";

	// Token: 0x04003D14 RID: 15636
	public const string BOOTS = "boots";

	// Token: 0x04003D15 RID: 15637
	public const string SEX_MARKER = "{sex}";

	// Token: 0x04003D16 RID: 15638
	public const string RACE_MARKER = "{race}";

	// Token: 0x04003D17 RID: 15639
	public const string VARIANT_MARKER = "{variant}";

	// Token: 0x04003D18 RID: 15640
	public const string HAIR_MARKER = "{hair}";

	// Token: 0x04003D19 RID: 15641
	[PublicizedFrom(EAccessModifier.Private)]
	public static Archetype tmpArchetype;

	// Token: 0x04003D1A RID: 15642
	[PublicizedFrom(EAccessModifier.Private)]
	public static string[] baseParts = new string[] { "head", "body", "hands", "feet" };

	// Token: 0x04003D1B RID: 15643
	[PublicizedFrom(EAccessModifier.Private)]
	public static string[] ignoredParts = new string[0];

	// Token: 0x04003D1C RID: 15644
	[PublicizedFrom(EAccessModifier.Private)]
	public static string[] basePartsFP = new string[] { "body", "hands" };

	// Token: 0x04003D1D RID: 15645
	[PublicizedFrom(EAccessModifier.Private)]
	public static string[] ignoredPartsFP = new string[] { "head", "helmet", "feet", "boots" };

	// Token: 0x04003D1E RID: 15646
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<int, SDCSUtils.AssetRefs> rigOwnedAssets = new Dictionary<int, SDCSUtils.AssetRefs>();

	// Token: 0x04003D1F RID: 15647
	[PublicizedFrom(EAccessModifier.Private)]
	public static SDCSUtils.AssetRefs tmpAssetsOwner;

	// Token: 0x02000A47 RID: 2631
	public class SlotData
	{
		// Token: 0x04003D20 RID: 15648
		public string PrefabName;

		// Token: 0x04003D21 RID: 15649
		public string PartName;

		// Token: 0x04003D22 RID: 15650
		public string BaseToTurnOff;

		// Token: 0x04003D23 RID: 15651
		public SDCSUtils.SlotData.HairMaskTypes HairMaskType;

		// Token: 0x04003D24 RID: 15652
		public SDCSUtils.SlotData.HairMaskTypes FacialHairMaskType;

		// Token: 0x02000A48 RID: 2632
		public enum HairMaskTypes
		{
			// Token: 0x04003D26 RID: 15654
			Full,
			// Token: 0x04003D27 RID: 15655
			Hat,
			// Token: 0x04003D28 RID: 15656
			None
		}
	}

	// Token: 0x02000A49 RID: 2633
	public class TransformCatalog : Dictionary<string, Transform>
	{
		// Token: 0x06004FBC RID: 20412 RVA: 0x001E5BA5 File Offset: 0x001E3DA5
		public TransformCatalog(Transform _transform)
		{
			this.AddRecursive(_transform);
		}

		// Token: 0x06004FBD RID: 20413 RVA: 0x001E5BB4 File Offset: 0x001E3DB4
		[PublicizedFrom(EAccessModifier.Private)]
		public void AddRecursive(Transform _transform)
		{
			string name = _transform.name;
			if (base.ContainsKey(name))
			{
				base[name] = _transform;
			}
			else
			{
				base.Add(name, _transform);
			}
			foreach (object obj in _transform)
			{
				Transform transform = (Transform)obj;
				this.AddRecursive(transform);
			}
		}
	}

	// Token: 0x02000A4A RID: 2634
	public static class SlotAllowedBonesCache
	{
		// Token: 0x06004FBE RID: 20414 RVA: 0x001E5C2C File Offset: 0x001E3E2C
		public static void Set(Transform slotRoot, HashSet<string> allowedBones)
		{
			if (!slotRoot || allowedBones == null)
			{
				return;
			}
			SDCSUtils.SlotAllowedBonesCache._map[slotRoot.GetInstanceID()] = allowedBones;
		}

		// Token: 0x06004FBF RID: 20415 RVA: 0x001E5C4B File Offset: 0x001E3E4B
		public static bool TryGet(Transform slotRoot, out HashSet<string> allowedBones)
		{
			allowedBones = null;
			return slotRoot && SDCSUtils.SlotAllowedBonesCache._map.TryGetValue(slotRoot.GetInstanceID(), out allowedBones);
		}

		// Token: 0x06004FC0 RID: 20416 RVA: 0x001E5C6B File Offset: 0x001E3E6B
		public static void Remove(Transform slotRoot)
		{
			if (!slotRoot)
			{
				return;
			}
			SDCSUtils.SlotAllowedBonesCache._map.Remove(slotRoot.GetInstanceID());
		}

		// Token: 0x06004FC1 RID: 20417 RVA: 0x001E5C87 File Offset: 0x001E3E87
		public static void Clear()
		{
			SDCSUtils.SlotAllowedBonesCache._map.Clear();
		}

		// Token: 0x04003D29 RID: 15657
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly Dictionary<int, HashSet<string>> _map = new Dictionary<int, HashSet<string>>();
	}

	// Token: 0x02000A4B RID: 2635
	public class AssetRefs
	{
		// Token: 0x06004FC3 RID: 20419 RVA: 0x001E5C9F File Offset: 0x001E3E9F
		public AssetRefs()
		{
		}

		// Token: 0x06004FC4 RID: 20420 RVA: 0x001E5CB4 File Offset: 0x001E3EB4
		public AssetRefs(SDCSUtils.AssetRefs other)
		{
			foreach (LoadManager.IAssetHandle assetHandle in other.trackedAssetHandles)
			{
				this.trackedAssetHandles.Add(assetHandle.Copy());
			}
		}

		// Token: 0x06004FC5 RID: 20421 RVA: 0x001E5D24 File Offset: 0x001E3F24
		public void AddAssetHandle(LoadManager.IAssetHandle handle)
		{
			this.trackedAssetHandles.Add(handle);
		}

		// Token: 0x06004FC6 RID: 20422 RVA: 0x001E5D34 File Offset: 0x001E3F34
		public void ReleaseAssets()
		{
			foreach (LoadManager.IAssetHandle assetHandle in this.trackedAssetHandles)
			{
				assetHandle.Release();
			}
			this.trackedAssetHandles.Clear();
		}

		// Token: 0x06004FC7 RID: 20423 RVA: 0x001E5D90 File Offset: 0x001E3F90
		public void LogAssets()
		{
			foreach (LoadManager.IAssetHandle assetHandle in this.trackedAssetHandles)
			{
				Log.Out(string.Format("LogAssets {0}", assetHandle));
			}
		}

		// Token: 0x04003D2A RID: 15658
		[PublicizedFrom(EAccessModifier.Private)]
		public List<LoadManager.IAssetHandle> trackedAssetHandles = new List<LoadManager.IAssetHandle>();
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using GearVariants;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Animations.Rigging;

// Token: 0x02000A61 RID: 2657
public static class SDCSUtils
{
	// Token: 0x06004FD6 RID: 20438 RVA: 0x001E67FC File Offset: 0x001E49FC
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

	// Token: 0x06004FD7 RID: 20439 RVA: 0x001E6B08 File Offset: 0x001E4D08
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

	// Token: 0x06004FD8 RID: 20440 RVA: 0x001E6CDC File Offset: 0x001E4EDC
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

	// Token: 0x06004FD9 RID: 20441 RVA: 0x001E6F80 File Offset: 0x001E5180
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<string, Transform> MapSourceByName(Transform sourceOrigin)
	{
		SDCSUtils.<>c__DisplayClass11_0 CS$<>8__locals1;
		CS$<>8__locals1.map = new Dictionary<string, Transform>(StringComparer.Ordinal);
		SDCSUtils.<MapSourceByName>g__Recurse|11_0(sourceOrigin, ref CS$<>8__locals1);
		return CS$<>8__locals1.map;
	}

	// Token: 0x06004FDA RID: 20442 RVA: 0x001E6FAC File Offset: 0x001E51AC
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

	// Token: 0x06004FDB RID: 20443 RVA: 0x001E7048 File Offset: 0x001E5248
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

	// Token: 0x06004FDC RID: 20444 RVA: 0x001E7194 File Offset: 0x001E5394
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

	// Token: 0x06004FDD RID: 20445 RVA: 0x001E738C File Offset: 0x001E558C
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

	// Token: 0x06004FDE RID: 20446 RVA: 0x001E73F8 File Offset: 0x001E55F8
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

	// Token: 0x06004FDF RID: 20447 RVA: 0x001E7450 File Offset: 0x001E5650
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

	// Token: 0x06004FE0 RID: 20448 RVA: 0x001E7494 File Offset: 0x001E5694
	public static TValue Find<TKey, TValue>(Dictionary<TKey, TValue> source, TKey key)
	{
		TValue tvalue;
		source.TryGetValue(key, out tvalue);
		return tvalue;
	}

	// Token: 0x170008B0 RID: 2224
	// (get) Token: 0x06004FE1 RID: 20449 RVA: 0x001E74AC File Offset: 0x001E56AC
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

	// Token: 0x170008B1 RID: 2225
	// (get) Token: 0x06004FE2 RID: 20450 RVA: 0x001E74EC File Offset: 0x001E56EC
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

	// Token: 0x170008B2 RID: 2226
	// (get) Token: 0x06004FE3 RID: 20451 RVA: 0x001E7594 File Offset: 0x001E5794
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

	// Token: 0x170008B3 RID: 2227
	// (get) Token: 0x06004FE4 RID: 20452 RVA: 0x001E7604 File Offset: 0x001E5804
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

	// Token: 0x170008B4 RID: 2228
	// (get) Token: 0x06004FE5 RID: 20453 RVA: 0x001E7674 File Offset: 0x001E5874
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

	// Token: 0x170008B5 RID: 2229
	// (get) Token: 0x06004FE6 RID: 20454 RVA: 0x001E76E4 File Offset: 0x001E58E4
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

	// Token: 0x170008B6 RID: 2230
	// (get) Token: 0x06004FE7 RID: 20455 RVA: 0x0000A1FE File Offset: 0x000083FE
	public static string baseHairColorLoc
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return "@:Entities/Player/Common/HairColorSwatches";
		}
	}

	// Token: 0x170008B7 RID: 2231
	// (get) Token: 0x06004FE8 RID: 20456 RVA: 0x001E7752 File Offset: 0x001E5952
	public static string baseEyeColorMatLoc
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return "@:Entities/Player/Common/Eyes/Materials/" + SDCSUtils.tmpArchetype.EyeColorName + ".mat";
		}
	}

	// Token: 0x170008B8 RID: 2232
	// (get) Token: 0x06004FE9 RID: 20457 RVA: 0x001E7770 File Offset: 0x001E5970
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

	// Token: 0x170008B9 RID: 2233
	// (get) Token: 0x06004FEA RID: 20458 RVA: 0x001E7818 File Offset: 0x001E5A18
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

	// Token: 0x170008BA RID: 2234
	// (get) Token: 0x06004FEB RID: 20459 RVA: 0x001E78C0 File Offset: 0x001E5AC0
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

	// Token: 0x170008BB RID: 2235
	// (get) Token: 0x06004FEC RID: 20460 RVA: 0x0000A415 File Offset: 0x00008615
	public static string baseRigPrefab
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return "@:Entities/Player/Common/BaseRigs/baseRigPrefab.prefab";
		}
	}

	// Token: 0x170008BC RID: 2236
	// (get) Token: 0x06004FED RID: 20461 RVA: 0x001E7965 File Offset: 0x001E5B65
	public static string baseRigFPPrefab
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return "@:Entities/Player/Common/BaseRigs/baseRigFPPrefab.prefab";
		}
	}

	// Token: 0x170008BD RID: 2237
	// (get) Token: 0x06004FEE RID: 20462 RVA: 0x001E796C File Offset: 0x001E5B6C
	public static RuntimeAnimatorController UIAnimController
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return DataLoader.LoadAsset<RuntimeAnimatorController>("@:Entities/Player/Common/AnimControllers/MenuSDCS" + SDCSUtils.tmpArchetype.Sex + "Controller" + (SDCSUtils.tmpArchetype.IsMale ? ".controller" : ".overrideController"), false);
		}
	}

	// Token: 0x170008BE RID: 2238
	// (get) Token: 0x06004FEF RID: 20463 RVA: 0x001E79A5 File Offset: 0x001E5BA5
	public static RuntimeAnimatorController TPAnimController
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return DataLoader.LoadAsset<RuntimeAnimatorController>("@:Entities/Player/Common/AnimControllers/3PPlayer" + SDCSUtils.tmpArchetype.Sex + "Controller" + (SDCSUtils.tmpArchetype.IsMale ? ".controller" : ".overrideController"), false);
		}
	}

	// Token: 0x170008BF RID: 2239
	// (get) Token: 0x06004FF0 RID: 20464 RVA: 0x001E79DE File Offset: 0x001E5BDE
	public static RuntimeAnimatorController FPAnimController
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return DataLoader.LoadAsset<RuntimeAnimatorController>("@:Entities/Player/Common/AnimControllers/FPPlayerController.controller", false);
		}
	}

	// Token: 0x06004FF1 RID: 20465 RVA: 0x001E79EC File Offset: 0x001E5BEC
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

	// Token: 0x06004FF2 RID: 20466 RVA: 0x001E7A1C File Offset: 0x001E5C1C
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

	// Token: 0x06004FF3 RID: 20467 RVA: 0x001E7A54 File Offset: 0x001E5C54
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

	// Token: 0x06004FF4 RID: 20468 RVA: 0x001E7AE0 File Offset: 0x001E5CE0
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

	// Token: 0x06004FF5 RID: 20469 RVA: 0x001E7CB8 File Offset: 0x001E5EB8
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

	// Token: 0x06004FF6 RID: 20470 RVA: 0x001E7DBC File Offset: 0x001E5FBC
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

	// Token: 0x06004FF7 RID: 20471 RVA: 0x001E7EDC File Offset: 0x001E60DC
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

	// Token: 0x06004FF8 RID: 20472 RVA: 0x001E7FC4 File Offset: 0x001E61C4
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

	// Token: 0x06004FF9 RID: 20473 RVA: 0x001E7FE4 File Offset: 0x001E61E4
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

	// Token: 0x06004FFA RID: 20474 RVA: 0x001E8080 File Offset: 0x001E6280
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

	// Token: 0x06004FFB RID: 20475 RVA: 0x001E8158 File Offset: 0x001E6358
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

	// Token: 0x06004FFC RID: 20476 RVA: 0x001E81FC File Offset: 0x001E63FC
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

	// Token: 0x06004FFD RID: 20477 RVA: 0x001E8358 File Offset: 0x001E6558
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

	// Token: 0x06004FFE RID: 20478 RVA: 0x001E8630 File Offset: 0x001E6830
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

	// Token: 0x06004FFF RID: 20479 RVA: 0x001E8814 File Offset: 0x001E6A14
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

	// Token: 0x06005000 RID: 20480 RVA: 0x001E8B54 File Offset: 0x001E6D54
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

	// Token: 0x06005001 RID: 20481 RVA: 0x001E8D50 File Offset: 0x001E6F50
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

	// Token: 0x06005002 RID: 20482 RVA: 0x001E8DC0 File Offset: 0x001E6FC0
	[PublicizedFrom(EAccessModifier.Private)]
	public static Transform RemapBone(Transform t, SDCSUtils.TransformCatalog boneCatalog)
	{
		if (t == null)
		{
			return null;
		}
		Transform transform = SDCSUtils.Find<string, Transform>(boneCatalog, t.name);
		if (!(transform != null))
		{
			return t;
		}
		return transform;
	}

	// Token: 0x06005003 RID: 20483 RVA: 0x001E8DF4 File Offset: 0x001E6FF4
	[PublicizedFrom(EAccessModifier.Private)]
	public static int RemapBoneListAndCount(List<Transform> list, SDCSUtils.TransformCatalog boneCatalog)
	{
		if (list == null)
		{
			return 0;
		}
		int num = 0;
		for (int i = 0; i < list.Count; i++)
		{
			Transform transform = list[i];
			if (!(transform == null))
			{
				Transform transform2 = SDCSUtils.Find<string, Transform>(boneCatalog, transform.name);
				if (transform2 != null && transform2 != transform)
				{
					list[i] = transform2;
					num++;
				}
			}
		}
		return num;
	}

	// Token: 0x06005004 RID: 20484 RVA: 0x001E8E58 File Offset: 0x001E7058
	[PublicizedFrom(EAccessModifier.Private)]
	public static void SetupDynamicBones(GameObject rigObj, SDCSUtils.TransformCatalog boneCatalog)
	{
		Transform transform = boneCatalog["Hips"];
		if (transform == null)
		{
			return;
		}
		rigObj.GetComponentsInChildren<DynamicBone>(true, SDCSUtils.tempDynamicBones);
		if (SDCSUtils.tempDynamicBones.Count == 0)
		{
			return;
		}
		foreach (DynamicBone dynamicBone in SDCSUtils.tempDynamicBones)
		{
			SDCSUtils.RemapColliderList(dynamicBone.m_Colliders, boneCatalog);
			dynamicBone.m_Root = SDCSUtils.RemapBone(dynamicBone.m_Root, boneCatalog);
			SDCSUtils.RemapBoneListAndCount(dynamicBone.m_Roots, boneCatalog);
			SDCSUtils.RemapBoneListAndCount(dynamicBone.m_Exclusions, boneCatalog);
			if (dynamicBone.m_ReferenceObject == null)
			{
				dynamicBone.m_ReferenceObject = transform;
			}
		}
		SDCSUtils.tempDynamicBones.Clear();
	}

	// Token: 0x06005005 RID: 20485 RVA: 0x001E8F2C File Offset: 0x001E712C
	[PublicizedFrom(EAccessModifier.Private)]
	public static int RemapColliderList(List<DynamicBoneColliderBase> list, SDCSUtils.TransformCatalog boneCatalog)
	{
		if (list == null)
		{
			return 0;
		}
		int num = 0;
		for (int i = 0; i < list.Count; i++)
		{
			DynamicBoneColliderBase dynamicBoneColliderBase = list[i];
			if (!(dynamicBoneColliderBase == null))
			{
				Transform transform = SDCSUtils.Find<string, Transform>(boneCatalog, dynamicBoneColliderBase.transform.name);
				if (!(transform == null) && !(transform == dynamicBoneColliderBase.transform))
				{
					DynamicBoneColliderBase component = transform.GetComponent<DynamicBoneColliderBase>();
					if (component != null && component != dynamicBoneColliderBase)
					{
						list[i] = component;
						num++;
					}
				}
			}
		}
		return num;
	}

	// Token: 0x06005006 RID: 20486 RVA: 0x001E8FB8 File Offset: 0x001E71B8
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
		SDCSUtils.SetupDynamicBones(_rigObj, _boneCatalog);
	}

	// Token: 0x06005007 RID: 20487 RVA: 0x001E90D0 File Offset: 0x001E72D0
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

	// Token: 0x06005008 RID: 20488 RVA: 0x001E915C File Offset: 0x001E735C
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

	// Token: 0x06005009 RID: 20489 RVA: 0x001E93E8 File Offset: 0x001E75E8
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

	// Token: 0x0600500A RID: 20490 RVA: 0x001E9490 File Offset: 0x001E7690
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

	// Token: 0x0600500B RID: 20491 RVA: 0x001E952C File Offset: 0x001E772C
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

	// Token: 0x0600500C RID: 20492 RVA: 0x001E969C File Offset: 0x001E789C
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

	// Token: 0x0600500D RID: 20493 RVA: 0x001E97D8 File Offset: 0x001E79D8
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

	// Token: 0x0600500E RID: 20494 RVA: 0x001E988C File Offset: 0x001E7A8C
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

	// Token: 0x0600500F RID: 20495 RVA: 0x001E98F8 File Offset: 0x001E7AF8
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

	// Token: 0x06005010 RID: 20496 RVA: 0x001E996C File Offset: 0x001E7B6C
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

	// Token: 0x06005011 RID: 20497 RVA: 0x0000B890 File Offset: 0x00009A90
	[PublicizedFrom(EAccessModifier.Private)]
	public static string parseSexedLocation(string sexedLocation, string sex)
	{
		return sexedLocation.Replace("{sex}", sex);
	}

	// Token: 0x06005012 RID: 20498 RVA: 0x001E99E0 File Offset: 0x001E7BE0
	[PublicizedFrom(EAccessModifier.Private)]
	public static void fixCloths(GameObject root)
	{
		SDCSUtils.<>c__DisplayClass109_0 CS$<>8__locals1 = new SDCSUtils.<>c__DisplayClass109_0();
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

	// Token: 0x06005014 RID: 20500 RVA: 0x001E9B4C File Offset: 0x001E7D4C
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Internal)]
	public static void <MapSourceByName>g__Recurse|11_0(Transform t, ref SDCSUtils.<>c__DisplayClass11_0 A_1)
	{
		A_1.map[t.name] = t;
		for (int i = 0; i < t.childCount; i++)
		{
			SDCSUtils.<MapSourceByName>g__Recurse|11_0(t.GetChild(i), ref A_1);
		}
	}

	// Token: 0x04003D9E RID: 15774
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly List<Cloth> tempCloths = new List<Cloth>();

	// Token: 0x04003D9F RID: 15775
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly List<Material> tempMats = new List<Material>();

	// Token: 0x04003DA0 RID: 15776
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly List<SkinnedMeshRenderer> tempSMRs = new List<SkinnedMeshRenderer>();

	// Token: 0x04003DA1 RID: 15777
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly List<DynamicBone> tempDynamicBones = new List<DynamicBone>();

	// Token: 0x04003DA2 RID: 15778
	public static readonly string[] shortHairNames = new string[] { "buzzcut", "cornrows", "flattop_fro", "mohawk", "small_fro" };

	// Token: 0x04003DA3 RID: 15779
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly List<SkinnedMeshRenderer> _smrBuf = new List<SkinnedMeshRenderer>(64);

	// Token: 0x04003DA4 RID: 15780
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly List<HingeJoint> _hingeBuf = new List<HingeJoint>(32);

	// Token: 0x04003DA5 RID: 15781
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly List<BlendConstraint> _bcBuf = new List<BlendConstraint>(32);

	// Token: 0x04003DA6 RID: 15782
	[PublicizedFrom(EAccessModifier.Private)]
	public const string ORIGIN = "Origin";

	// Token: 0x04003DA7 RID: 15783
	[PublicizedFrom(EAccessModifier.Private)]
	public const string RIGCON = "RigConstraints";

	// Token: 0x04003DA8 RID: 15784
	[PublicizedFrom(EAccessModifier.Private)]
	public const string IKRIG = "IKRig";

	// Token: 0x04003DA9 RID: 15785
	public const string HEAD = "head";

	// Token: 0x04003DAA RID: 15786
	public const string EYES = "eyes";

	// Token: 0x04003DAB RID: 15787
	public const string BEARD = "beard";

	// Token: 0x04003DAC RID: 15788
	public const string HAIR = "hair";

	// Token: 0x04003DAD RID: 15789
	public const string BODY = "body";

	// Token: 0x04003DAE RID: 15790
	public const string HANDS = "hands";

	// Token: 0x04003DAF RID: 15791
	public const string FEET = "feet";

	// Token: 0x04003DB0 RID: 15792
	public const string HELMET = "helmet";

	// Token: 0x04003DB1 RID: 15793
	public const string TORSO = "torso";

	// Token: 0x04003DB2 RID: 15794
	public const string GLOVES = "gloves";

	// Token: 0x04003DB3 RID: 15795
	public const string BOOTS = "boots";

	// Token: 0x04003DB4 RID: 15796
	public const string SEX_MARKER = "{sex}";

	// Token: 0x04003DB5 RID: 15797
	public const string RACE_MARKER = "{race}";

	// Token: 0x04003DB6 RID: 15798
	public const string VARIANT_MARKER = "{variant}";

	// Token: 0x04003DB7 RID: 15799
	public const string HAIR_MARKER = "{hair}";

	// Token: 0x04003DB8 RID: 15800
	[PublicizedFrom(EAccessModifier.Private)]
	public static Archetype tmpArchetype;

	// Token: 0x04003DB9 RID: 15801
	[PublicizedFrom(EAccessModifier.Private)]
	public static string[] baseParts = new string[] { "head", "body", "hands", "feet" };

	// Token: 0x04003DBA RID: 15802
	[PublicizedFrom(EAccessModifier.Private)]
	public static string[] ignoredParts = new string[0];

	// Token: 0x04003DBB RID: 15803
	[PublicizedFrom(EAccessModifier.Private)]
	public static string[] basePartsFP = new string[] { "body", "hands" };

	// Token: 0x04003DBC RID: 15804
	[PublicizedFrom(EAccessModifier.Private)]
	public static string[] ignoredPartsFP = new string[] { "head", "helmet", "feet", "boots" };

	// Token: 0x04003DBD RID: 15805
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<int, SDCSUtils.AssetRefs> rigOwnedAssets = new Dictionary<int, SDCSUtils.AssetRefs>();

	// Token: 0x04003DBE RID: 15806
	[PublicizedFrom(EAccessModifier.Private)]
	public static SDCSUtils.AssetRefs tmpAssetsOwner;

	// Token: 0x02000A62 RID: 2658
	public class SlotData
	{
		// Token: 0x04003DBF RID: 15807
		public string PrefabName;

		// Token: 0x04003DC0 RID: 15808
		public string PartName;

		// Token: 0x04003DC1 RID: 15809
		public string BaseToTurnOff;

		// Token: 0x04003DC2 RID: 15810
		public SDCSUtils.SlotData.HairMaskTypes HairMaskType;

		// Token: 0x04003DC3 RID: 15811
		public SDCSUtils.SlotData.HairMaskTypes FacialHairMaskType;

		// Token: 0x02000A63 RID: 2659
		public enum HairMaskTypes
		{
			// Token: 0x04003DC5 RID: 15813
			Full,
			// Token: 0x04003DC6 RID: 15814
			Hat,
			// Token: 0x04003DC7 RID: 15815
			None
		}
	}

	// Token: 0x02000A64 RID: 2660
	public class TransformCatalog : Dictionary<string, Transform>
	{
		// Token: 0x06005016 RID: 20502 RVA: 0x001E9B89 File Offset: 0x001E7D89
		public TransformCatalog(Transform _transform)
		{
			this.AddRecursive(_transform);
		}

		// Token: 0x06005017 RID: 20503 RVA: 0x001E9B98 File Offset: 0x001E7D98
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

	// Token: 0x02000A65 RID: 2661
	public static class SlotAllowedBonesCache
	{
		// Token: 0x06005018 RID: 20504 RVA: 0x001E9C10 File Offset: 0x001E7E10
		public static void Set(Transform slotRoot, HashSet<string> allowedBones)
		{
			if (!slotRoot || allowedBones == null)
			{
				return;
			}
			SDCSUtils.SlotAllowedBonesCache._map[slotRoot.GetInstanceID()] = allowedBones;
		}

		// Token: 0x06005019 RID: 20505 RVA: 0x001E9C2F File Offset: 0x001E7E2F
		public static bool TryGet(Transform slotRoot, out HashSet<string> allowedBones)
		{
			allowedBones = null;
			return slotRoot && SDCSUtils.SlotAllowedBonesCache._map.TryGetValue(slotRoot.GetInstanceID(), out allowedBones);
		}

		// Token: 0x0600501A RID: 20506 RVA: 0x001E9C4F File Offset: 0x001E7E4F
		public static void Remove(Transform slotRoot)
		{
			if (!slotRoot)
			{
				return;
			}
			SDCSUtils.SlotAllowedBonesCache._map.Remove(slotRoot.GetInstanceID());
		}

		// Token: 0x0600501B RID: 20507 RVA: 0x001E9C6B File Offset: 0x001E7E6B
		public static void Clear()
		{
			SDCSUtils.SlotAllowedBonesCache._map.Clear();
		}

		// Token: 0x04003DC8 RID: 15816
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly Dictionary<int, HashSet<string>> _map = new Dictionary<int, HashSet<string>>();
	}

	// Token: 0x02000A66 RID: 2662
	public class AssetRefs
	{
		// Token: 0x0600501D RID: 20509 RVA: 0x001E9C83 File Offset: 0x001E7E83
		public AssetRefs()
		{
		}

		// Token: 0x0600501E RID: 20510 RVA: 0x001E9C98 File Offset: 0x001E7E98
		public AssetRefs(SDCSUtils.AssetRefs other)
		{
			foreach (LoadManager.IAssetHandle assetHandle in other.trackedAssetHandles)
			{
				this.trackedAssetHandles.Add(assetHandle.Copy());
			}
		}

		// Token: 0x0600501F RID: 20511 RVA: 0x001E9D08 File Offset: 0x001E7F08
		public void AddAssetHandle(LoadManager.IAssetHandle handle)
		{
			this.trackedAssetHandles.Add(handle);
		}

		// Token: 0x06005020 RID: 20512 RVA: 0x001E9D18 File Offset: 0x001E7F18
		public void ReleaseAssets()
		{
			foreach (LoadManager.IAssetHandle assetHandle in this.trackedAssetHandles)
			{
				assetHandle.Release();
			}
			this.trackedAssetHandles.Clear();
		}

		// Token: 0x06005021 RID: 20513 RVA: 0x001E9D74 File Offset: 0x001E7F74
		public void LogAssets()
		{
			foreach (LoadManager.IAssetHandle assetHandle in this.trackedAssetHandles)
			{
				Log.Out(string.Format("LogAssets {0}", assetHandle));
			}
		}

		// Token: 0x04003DC9 RID: 15817
		[PublicizedFrom(EAccessModifier.Private)]
		public List<LoadManager.IAssetHandle> trackedAssetHandles = new List<LoadManager.IAssetHandle>();
	}
}

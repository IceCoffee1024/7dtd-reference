using System;
using UnityEngine;

// Token: 0x02001350 RID: 4944
[CreateAssetMenu(fileName = "MeshMorphMatrix", menuName = "Mesh Morphing/MeshMorphMatrix", order = 1)]
public class MeshMorphMatrix : ScriptableObject
{
	// Token: 0x17001282 RID: 4738
	// (get) Token: 0x06009C17 RID: 39959 RVA: 0x003AEFD2 File Offset: 0x003AD1D2
	public SkinnedMeshRenderer MorphTargetsSource
	{
		get
		{
			return this.morphTargetsSource;
		}
	}

	// Token: 0x17001283 RID: 4739
	// (get) Token: 0x06009C18 RID: 39960 RVA: 0x003AEFDA File Offset: 0x003AD1DA
	public MeshMorphMatrix.MorphTarget[] MorphTargets
	{
		get
		{
			return this.morphTargets;
		}
	}

	// Token: 0x17001284 RID: 4740
	// (get) Token: 0x06009C19 RID: 39961 RVA: 0x003AEFE2 File Offset: 0x003AD1E2
	public MeshMorphMatrix.MeshData[] Meshes
	{
		get
		{
			return this.meshes;
		}
	}

	// Token: 0x17001285 RID: 4741
	// (get) Token: 0x06009C1A RID: 39962 RVA: 0x003AEFEA File Offset: 0x003AD1EA
	public MeshMorph[] MorphedMeshes
	{
		get
		{
			return this.morphedMeshes;
		}
	}

	// Token: 0x17001286 RID: 4742
	// (get) Token: 0x06009C1B RID: 39963 RVA: 0x003AEFF2 File Offset: 0x003AD1F2
	public float MaxDistance
	{
		get
		{
			return this.maxDistance;
		}
	}

	// Token: 0x17001287 RID: 4743
	// (get) Token: 0x06009C1C RID: 39964 RVA: 0x003AEFFA File Offset: 0x003AD1FA
	public float NormalBias
	{
		get
		{
			return this.normalBias;
		}
	}

	// Token: 0x040075D1 RID: 30161
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public MeshMorphMatrix.MeshMorphMatrixType matrixType;

	// Token: 0x040075D2 RID: 30162
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public SkinnedMeshRenderer morphTargetsSource;

	// Token: 0x040075D3 RID: 30163
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public float maxDistance = 0.1f;

	// Token: 0x040075D4 RID: 30164
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public float normalBias;

	// Token: 0x040075D5 RID: 30165
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public MeshMorphMatrix.MorphTarget[] morphTargets;

	// Token: 0x040075D6 RID: 30166
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public MeshMorphMatrix.MeshData[] meshes;

	// Token: 0x040075D7 RID: 30167
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public MeshMorph[] morphedMeshes;

	// Token: 0x040075D8 RID: 30168
	[HideInInspector]
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public bool initialized;

	// Token: 0x02001351 RID: 4945
	public enum MeshMorphMatrixType
	{
		// Token: 0x040075DA RID: 30170
		Hair,
		// Token: 0x040075DB RID: 30171
		Headgear
	}

	// Token: 0x02001352 RID: 4946
	[Serializable]
	public struct MorphTarget
	{
		// Token: 0x040075DC RID: 30172
		public int blendshapeIndex;

		// Token: 0x040075DD RID: 30173
		public string name;
	}

	// Token: 0x02001353 RID: 4947
	[Serializable]
	public struct MeshData
	{
		// Token: 0x17001288 RID: 4744
		// (get) Token: 0x06009C1E RID: 39966 RVA: 0x003AF015 File Offset: 0x003AD215
		public string MeshName
		{
			get
			{
				if (!(this.gameObject != null))
				{
					return "null";
				}
				return this.gameObject.name;
			}
		}

		// Token: 0x040075DE RID: 30174
		public int blendshapeIndex;

		// Token: 0x040075DF RID: 30175
		public string typeName;

		// Token: 0x040075E0 RID: 30176
		public GameObject gameObject;
	}
}

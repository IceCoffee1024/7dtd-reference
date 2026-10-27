using System;
using UnityEngine;

// Token: 0x02001382 RID: 4994
[CreateAssetMenu(fileName = "MeshMorphMatrix", menuName = "Mesh Morphing/MeshMorphMatrix", order = 1)]
public class MeshMorphMatrix : ScriptableObject
{
	// Token: 0x170012D3 RID: 4819
	// (get) Token: 0x06009D1B RID: 40219 RVA: 0x003B5F52 File Offset: 0x003B4152
	public SkinnedMeshRenderer MorphTargetsSource
	{
		get
		{
			return this.morphTargetsSource;
		}
	}

	// Token: 0x170012D4 RID: 4820
	// (get) Token: 0x06009D1C RID: 40220 RVA: 0x003B5F5A File Offset: 0x003B415A
	public MeshMorphMatrix.MorphTarget[] MorphTargets
	{
		get
		{
			return this.morphTargets;
		}
	}

	// Token: 0x170012D5 RID: 4821
	// (get) Token: 0x06009D1D RID: 40221 RVA: 0x003B5F62 File Offset: 0x003B4162
	public MeshMorphMatrix.MeshData[] Meshes
	{
		get
		{
			return this.meshes;
		}
	}

	// Token: 0x170012D6 RID: 4822
	// (get) Token: 0x06009D1E RID: 40222 RVA: 0x003B5F6A File Offset: 0x003B416A
	public MeshMorph[] MorphedMeshes
	{
		get
		{
			return this.morphedMeshes;
		}
	}

	// Token: 0x170012D7 RID: 4823
	// (get) Token: 0x06009D1F RID: 40223 RVA: 0x003B5F72 File Offset: 0x003B4172
	public float MaxDistance
	{
		get
		{
			return this.maxDistance;
		}
	}

	// Token: 0x170012D8 RID: 4824
	// (get) Token: 0x06009D20 RID: 40224 RVA: 0x003B5F7A File Offset: 0x003B417A
	public float NormalBias
	{
		get
		{
			return this.normalBias;
		}
	}

	// Token: 0x0400768D RID: 30349
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public MeshMorphMatrix.MeshMorphMatrixType matrixType;

	// Token: 0x0400768E RID: 30350
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public SkinnedMeshRenderer morphTargetsSource;

	// Token: 0x0400768F RID: 30351
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public float maxDistance = 0.1f;

	// Token: 0x04007690 RID: 30352
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public float normalBias;

	// Token: 0x04007691 RID: 30353
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public MeshMorphMatrix.MorphTarget[] morphTargets;

	// Token: 0x04007692 RID: 30354
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public MeshMorphMatrix.MeshData[] meshes;

	// Token: 0x04007693 RID: 30355
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public MeshMorph[] morphedMeshes;

	// Token: 0x04007694 RID: 30356
	[HideInInspector]
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public bool initialized;

	// Token: 0x02001383 RID: 4995
	public enum MeshMorphMatrixType
	{
		// Token: 0x04007696 RID: 30358
		Hair,
		// Token: 0x04007697 RID: 30359
		Headgear
	}

	// Token: 0x02001384 RID: 4996
	[Serializable]
	public struct MorphTarget
	{
		// Token: 0x04007698 RID: 30360
		public int blendshapeIndex;

		// Token: 0x04007699 RID: 30361
		public string name;
	}

	// Token: 0x02001385 RID: 4997
	[Serializable]
	public struct MeshData
	{
		// Token: 0x170012D9 RID: 4825
		// (get) Token: 0x06009D22 RID: 40226 RVA: 0x003B5F95 File Offset: 0x003B4195
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

		// Token: 0x0400769A RID: 30362
		public int blendshapeIndex;

		// Token: 0x0400769B RID: 30363
		public string typeName;

		// Token: 0x0400769C RID: 30364
		public GameObject gameObject;
	}
}

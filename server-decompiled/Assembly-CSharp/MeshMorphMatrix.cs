using System;
using UnityEngine;

// Token: 0x02001370 RID: 4976
[CreateAssetMenu(fileName = "MeshMorphMatrix", menuName = "Mesh Morphing/MeshMorphMatrix", order = 1)]
public class MeshMorphMatrix : ScriptableObject
{
	// Token: 0x170012C7 RID: 4807
	// (get) Token: 0x06009CC3 RID: 40131 RVA: 0x003B3B4E File Offset: 0x003B1D4E
	public SkinnedMeshRenderer MorphTargetsSource
	{
		get
		{
			return this.morphTargetsSource;
		}
	}

	// Token: 0x170012C8 RID: 4808
	// (get) Token: 0x06009CC4 RID: 40132 RVA: 0x003B3B56 File Offset: 0x003B1D56
	public MeshMorphMatrix.MorphTarget[] MorphTargets
	{
		get
		{
			return this.morphTargets;
		}
	}

	// Token: 0x170012C9 RID: 4809
	// (get) Token: 0x06009CC5 RID: 40133 RVA: 0x003B3B5E File Offset: 0x003B1D5E
	public MeshMorphMatrix.MeshData[] Meshes
	{
		get
		{
			return this.meshes;
		}
	}

	// Token: 0x170012CA RID: 4810
	// (get) Token: 0x06009CC6 RID: 40134 RVA: 0x003B3B66 File Offset: 0x003B1D66
	public MeshMorph[] MorphedMeshes
	{
		get
		{
			return this.morphedMeshes;
		}
	}

	// Token: 0x170012CB RID: 4811
	// (get) Token: 0x06009CC7 RID: 40135 RVA: 0x003B3B6E File Offset: 0x003B1D6E
	public float MaxDistance
	{
		get
		{
			return this.maxDistance;
		}
	}

	// Token: 0x170012CC RID: 4812
	// (get) Token: 0x06009CC8 RID: 40136 RVA: 0x003B3B76 File Offset: 0x003B1D76
	public float NormalBias
	{
		get
		{
			return this.normalBias;
		}
	}

	// Token: 0x0400764C RID: 30284
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public MeshMorphMatrix.MeshMorphMatrixType matrixType;

	// Token: 0x0400764D RID: 30285
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public SkinnedMeshRenderer morphTargetsSource;

	// Token: 0x0400764E RID: 30286
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public float maxDistance = 0.1f;

	// Token: 0x0400764F RID: 30287
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public float normalBias;

	// Token: 0x04007650 RID: 30288
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public MeshMorphMatrix.MorphTarget[] morphTargets;

	// Token: 0x04007651 RID: 30289
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public MeshMorphMatrix.MeshData[] meshes;

	// Token: 0x04007652 RID: 30290
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public MeshMorph[] morphedMeshes;

	// Token: 0x04007653 RID: 30291
	[HideInInspector]
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public bool initialized;

	// Token: 0x02001371 RID: 4977
	public enum MeshMorphMatrixType
	{
		// Token: 0x04007655 RID: 30293
		Hair,
		// Token: 0x04007656 RID: 30294
		Headgear
	}

	// Token: 0x02001372 RID: 4978
	[Serializable]
	public struct MorphTarget
	{
		// Token: 0x04007657 RID: 30295
		public int blendshapeIndex;

		// Token: 0x04007658 RID: 30296
		public string name;
	}

	// Token: 0x02001373 RID: 4979
	[Serializable]
	public struct MeshData
	{
		// Token: 0x170012CD RID: 4813
		// (get) Token: 0x06009CCA RID: 40138 RVA: 0x003B3B91 File Offset: 0x003B1D91
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

		// Token: 0x04007659 RID: 30297
		public int blendshapeIndex;

		// Token: 0x0400765A RID: 30298
		public string typeName;

		// Token: 0x0400765B RID: 30299
		public GameObject gameObject;
	}
}

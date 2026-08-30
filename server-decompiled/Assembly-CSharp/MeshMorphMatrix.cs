using System;
using UnityEngine;

// Token: 0x02001336 RID: 4918
[CreateAssetMenu(fileName = "MeshMorphMatrix", menuName = "Mesh Morphing/MeshMorphMatrix", order = 1)]
public class MeshMorphMatrix : ScriptableObject
{
	// Token: 0x17001249 RID: 4681
	// (get) Token: 0x06009B1C RID: 39708 RVA: 0x003A9D2A File Offset: 0x003A7F2A
	public SkinnedMeshRenderer MorphTargetsSource
	{
		get
		{
			return this.morphTargetsSource;
		}
	}

	// Token: 0x1700124A RID: 4682
	// (get) Token: 0x06009B1D RID: 39709 RVA: 0x003A9D32 File Offset: 0x003A7F32
	public MeshMorphMatrix.MorphTarget[] MorphTargets
	{
		get
		{
			return this.morphTargets;
		}
	}

	// Token: 0x1700124B RID: 4683
	// (get) Token: 0x06009B1E RID: 39710 RVA: 0x003A9D3A File Offset: 0x003A7F3A
	public MeshMorphMatrix.MeshData[] Meshes
	{
		get
		{
			return this.meshes;
		}
	}

	// Token: 0x1700124C RID: 4684
	// (get) Token: 0x06009B1F RID: 39711 RVA: 0x003A9D42 File Offset: 0x003A7F42
	public MeshMorph[] MorphedMeshes
	{
		get
		{
			return this.morphedMeshes;
		}
	}

	// Token: 0x1700124D RID: 4685
	// (get) Token: 0x06009B20 RID: 39712 RVA: 0x003A9D4A File Offset: 0x003A7F4A
	public float MaxDistance
	{
		get
		{
			return this.maxDistance;
		}
	}

	// Token: 0x1700124E RID: 4686
	// (get) Token: 0x06009B21 RID: 39713 RVA: 0x003A9D52 File Offset: 0x003A7F52
	public float NormalBias
	{
		get
		{
			return this.normalBias;
		}
	}

	// Token: 0x040074F9 RID: 29945
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public MeshMorphMatrix.MeshMorphMatrixType matrixType;

	// Token: 0x040074FA RID: 29946
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public SkinnedMeshRenderer morphTargetsSource;

	// Token: 0x040074FB RID: 29947
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public float maxDistance = 0.1f;

	// Token: 0x040074FC RID: 29948
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public float normalBias;

	// Token: 0x040074FD RID: 29949
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public MeshMorphMatrix.MorphTarget[] morphTargets;

	// Token: 0x040074FE RID: 29950
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public MeshMorphMatrix.MeshData[] meshes;

	// Token: 0x040074FF RID: 29951
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public MeshMorph[] morphedMeshes;

	// Token: 0x04007500 RID: 29952
	[HideInInspector]
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public bool initialized;

	// Token: 0x02001337 RID: 4919
	public enum MeshMorphMatrixType
	{
		// Token: 0x04007502 RID: 29954
		Hair,
		// Token: 0x04007503 RID: 29955
		Headgear
	}

	// Token: 0x02001338 RID: 4920
	[Serializable]
	public struct MorphTarget
	{
		// Token: 0x04007504 RID: 29956
		public int blendshapeIndex;

		// Token: 0x04007505 RID: 29957
		public string name;
	}

	// Token: 0x02001339 RID: 4921
	[Serializable]
	public struct MeshData
	{
		// Token: 0x1700124F RID: 4687
		// (get) Token: 0x06009B23 RID: 39715 RVA: 0x003A9D6D File Offset: 0x003A7F6D
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

		// Token: 0x04007506 RID: 29958
		public int blendshapeIndex;

		// Token: 0x04007507 RID: 29959
		public string typeName;

		// Token: 0x04007508 RID: 29960
		public GameObject gameObject;
	}
}

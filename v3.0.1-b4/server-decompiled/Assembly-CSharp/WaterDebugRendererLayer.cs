using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x02000CCD RID: 3277
public class WaterDebugRendererLayer : IMemoryPoolableObject
{
	// Token: 0x17000A7B RID: 2683
	// (get) Token: 0x0600646A RID: 25706 RVA: 0x0027483A File Offset: 0x00272A3A
	// (set) Token: 0x0600646B RID: 25707 RVA: 0x00274842 File Offset: 0x00272A42
	public bool IsInitialized
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x0600646C RID: 25708 RVA: 0x0027484C File Offset: 0x00272A4C
	[PublicizedFrom(EAccessModifier.Private)]
	public void InitializeData()
	{
		this.transforms = new Matrix4x4[4096];
		this.colors = new float4[4096];
		this.normalizedMass = new float[4096];
		this.RegenerateTransforms();
		Origin.OriginChanged = (Action<Vector3>)Delegate.Combine(Origin.OriginChanged, new Action<Vector3>(this.OnOriginChanged));
		this.IsInitialized = true;
	}

	// Token: 0x0600646D RID: 25709 RVA: 0x002748B8 File Offset: 0x00272AB8
	[PublicizedFrom(EAccessModifier.Private)]
	public void RegenerateTransforms()
	{
		Vector3 vector = Vector3.one * 0.9f;
		Vector3 vector2 = (Vector3.one - vector) * 0.5f;
		for (int i = 0; i < 16; i++)
		{
			for (int j = 0; j < 16; j++)
			{
				for (int k = 0; k < 16; k++)
				{
					int num = this.CoordToIndex(i, j, k);
					this.transforms[num] = Matrix4x4.TRS(this.layerOrigin + new Vector3((float)i, (float)j, (float)k) - Origin.position + vector2 + Vector3.one * 0.5f, Quaternion.identity, vector);
				}
			}
		}
		Vector3 vector3 = this.layerOrigin - Origin.position;
		Vector3 vector4 = this.layerOrigin - Origin.position + new Vector3(16f, 16f, 16f);
		this.bounds = new Bounds((vector4 + vector3) / 2f, vector4 - vector3);
		this.transformsHaveChanged = true;
	}

	// Token: 0x0600646E RID: 25710 RVA: 0x002749EC File Offset: 0x00272BEC
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnOriginChanged(Vector3 _origin)
	{
		if (this.IsInitialized)
		{
			this.RegenerateTransforms();
		}
	}

	// Token: 0x0600646F RID: 25711 RVA: 0x002749FC File Offset: 0x00272BFC
	public void SetLayerOrigin(Vector3 _origin)
	{
		this.layerOrigin = _origin;
		if (this.IsInitialized)
		{
			this.RegenerateTransforms();
		}
	}

	// Token: 0x06006470 RID: 25712 RVA: 0x00274A13 File Offset: 0x00272C13
	[PublicizedFrom(EAccessModifier.Private)]
	public int CoordToIndex(int _x, int _y, int _z)
	{
		return _x + 16 * _y + 256 * _z;
	}

	// Token: 0x06006471 RID: 25713 RVA: 0x00274A24 File Offset: 0x00272C24
	public void SetWater(int _x, int _y, int _z, float mass)
	{
		float num = mass / 19500f;
		if (!this.IsInitialized)
		{
			if (num < 0.01f)
			{
				return;
			}
			this.InitializeData();
		}
		int num2 = this.CoordToIndex(_x, _y, _z);
		if (this.normalizedMass[num2] < 0.01f && num > 0.01f)
		{
			this.totalWater++;
		}
		else if (this.normalizedMass[num2] > 0.01f && num < 0.01f)
		{
			this.totalWater--;
		}
		this.normalizedMass[num2] = num;
		float num3 = math.max((mass - 19500f) / 65535f, 0f);
		this.colors[num2] = math.lerp(WaterDebugRendererLayer.waterColor, WaterDebugRendererLayer.overfullColor, num3);
		this.massesHaveChanged = true;
	}

	// Token: 0x06006472 RID: 25714 RVA: 0x00274AEC File Offset: 0x00272CEC
	public void Draw()
	{
		if (this.totalWater == 0)
		{
			return;
		}
		if (this.materialProperties == null)
		{
			this.materialProperties = new MaterialPropertyBlock();
			this.materialProperties.SetFloat("_ScaleCutoff", 0.01f);
		}
		if (this.transformsBuffer == null)
		{
			this.transformsBuffer = new ComputeBuffer(4096, 64);
			this.materialProperties.SetBuffer("_Transforms", this.transformsBuffer);
		}
		if (this.colorBuffer == null)
		{
			this.colorBuffer = new ComputeBuffer(4096, 16);
			this.materialProperties.SetBuffer("_Colors", this.colorBuffer);
		}
		if (this.transformsHaveChanged)
		{
			this.transformsBuffer.SetData(this.transforms);
			this.transformsHaveChanged = false;
		}
		if (this.massBuffer == null)
		{
			this.massBuffer = new ComputeBuffer(4096, 4);
			this.materialProperties.SetBuffer("_Scales", this.massBuffer);
		}
		if (this.massesHaveChanged)
		{
			this.massBuffer.SetData(this.normalizedMass);
			this.colorBuffer.SetData(this.colors);
			this.massesHaveChanged = false;
		}
		Graphics.DrawMeshInstancedProcedural(WaterDebugAssets.CubeMesh, 0, WaterDebugAssets.DebugMaterial, this.bounds, 4096, this.materialProperties, ShadowCastingMode.On, true, 0, null, LightProbeUsage.BlendProbes, null);
	}

	// Token: 0x06006473 RID: 25715 RVA: 0x00274C34 File Offset: 0x00272E34
	public void Reset()
	{
		if (this.IsInitialized)
		{
			Origin.OriginChanged = (Action<Vector3>)Delegate.Remove(Origin.OriginChanged, new Action<Vector3>(this.OnOriginChanged));
			this.transforms = null;
			this.normalizedMass = null;
			this.IsInitialized = false;
		}
		this.totalWater = 0;
		this.transformsHaveChanged = false;
		this.massesHaveChanged = false;
	}

	// Token: 0x06006474 RID: 25716 RVA: 0x00274C94 File Offset: 0x00272E94
	public void Cleanup()
	{
		if (this.transformsBuffer != null)
		{
			this.transformsBuffer.Dispose();
			this.transformsBuffer = null;
		}
		if (this.massBuffer != null)
		{
			this.massBuffer.Dispose();
			this.massBuffer = null;
		}
		if (this.colorBuffer != null)
		{
			this.colorBuffer.Dispose();
			this.colorBuffer = null;
		}
	}

	// Token: 0x04004DE1 RID: 19937
	public const int dimX = 16;

	// Token: 0x04004DE2 RID: 19938
	public const int dimY = 16;

	// Token: 0x04004DE3 RID: 19939
	public const int dimZ = 16;

	// Token: 0x04004DE4 RID: 19940
	public const int elementsPerLayer = 4096;

	// Token: 0x04004DE5 RID: 19941
	[PublicizedFrom(EAccessModifier.Private)]
	public const float SCALE_CUTOFF = 0.01f;

	// Token: 0x04004DE6 RID: 19942
	[PublicizedFrom(EAccessModifier.Private)]
	public const float RENDER_SCALE = 0.9f;

	// Token: 0x04004DE7 RID: 19943
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly float4 waterColor = new float4(0f, 0f, 1f, 1f);

	// Token: 0x04004DE8 RID: 19944
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly float4 overfullColor = new float4(1f, 0f, 1f, 1f);

	// Token: 0x04004DE9 RID: 19945
	[PublicizedFrom(EAccessModifier.Private)]
	public MaterialPropertyBlock materialProperties;

	// Token: 0x04004DEA RID: 19946
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 layerOrigin;

	// Token: 0x04004DEB RID: 19947
	[PublicizedFrom(EAccessModifier.Private)]
	public Bounds bounds;

	// Token: 0x04004DEC RID: 19948
	[PublicizedFrom(EAccessModifier.Private)]
	public Matrix4x4[] transforms;

	// Token: 0x04004DED RID: 19949
	[PublicizedFrom(EAccessModifier.Private)]
	public float4[] colors;

	// Token: 0x04004DEE RID: 19950
	[PublicizedFrom(EAccessModifier.Private)]
	public bool transformsHaveChanged;

	// Token: 0x04004DEF RID: 19951
	[PublicizedFrom(EAccessModifier.Private)]
	public float[] normalizedMass;

	// Token: 0x04004DF0 RID: 19952
	[PublicizedFrom(EAccessModifier.Private)]
	public bool massesHaveChanged;

	// Token: 0x04004DF1 RID: 19953
	[PublicizedFrom(EAccessModifier.Private)]
	public ComputeBuffer transformsBuffer;

	// Token: 0x04004DF2 RID: 19954
	[PublicizedFrom(EAccessModifier.Private)]
	public ComputeBuffer massBuffer;

	// Token: 0x04004DF3 RID: 19955
	[PublicizedFrom(EAccessModifier.Private)]
	public ComputeBuffer colorBuffer;

	// Token: 0x04004DF5 RID: 19957
	[PublicizedFrom(EAccessModifier.Private)]
	public int totalWater;
}

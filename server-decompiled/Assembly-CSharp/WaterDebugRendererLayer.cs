using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x02000CED RID: 3309
public class WaterDebugRendererLayer : IMemoryPoolableObject
{
	// Token: 0x17000AA4 RID: 2724
	// (get) Token: 0x060064E8 RID: 25832 RVA: 0x0027AADA File Offset: 0x00278CDA
	// (set) Token: 0x060064E9 RID: 25833 RVA: 0x0027AAE2 File Offset: 0x00278CE2
	public bool IsInitialized
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x060064EA RID: 25834 RVA: 0x0027AAEC File Offset: 0x00278CEC
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

	// Token: 0x060064EB RID: 25835 RVA: 0x0027AB58 File Offset: 0x00278D58
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

	// Token: 0x060064EC RID: 25836 RVA: 0x0027AC8C File Offset: 0x00278E8C
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnOriginChanged(Vector3 _origin)
	{
		if (this.IsInitialized)
		{
			this.RegenerateTransforms();
		}
	}

	// Token: 0x060064ED RID: 25837 RVA: 0x0027AC9C File Offset: 0x00278E9C
	public void SetLayerOrigin(Vector3 _origin)
	{
		this.layerOrigin = _origin;
		if (this.IsInitialized)
		{
			this.RegenerateTransforms();
		}
	}

	// Token: 0x060064EE RID: 25838 RVA: 0x0027ACB3 File Offset: 0x00278EB3
	[PublicizedFrom(EAccessModifier.Private)]
	public int CoordToIndex(int _x, int _y, int _z)
	{
		return _x + 16 * _y + 256 * _z;
	}

	// Token: 0x060064EF RID: 25839 RVA: 0x0027ACC4 File Offset: 0x00278EC4
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

	// Token: 0x060064F0 RID: 25840 RVA: 0x0027AD8C File Offset: 0x00278F8C
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

	// Token: 0x060064F1 RID: 25841 RVA: 0x0027AED4 File Offset: 0x002790D4
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

	// Token: 0x060064F2 RID: 25842 RVA: 0x0027AF34 File Offset: 0x00279134
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

	// Token: 0x04004EB5 RID: 20149
	public const int dimX = 16;

	// Token: 0x04004EB6 RID: 20150
	public const int dimY = 16;

	// Token: 0x04004EB7 RID: 20151
	public const int dimZ = 16;

	// Token: 0x04004EB8 RID: 20152
	public const int elementsPerLayer = 4096;

	// Token: 0x04004EB9 RID: 20153
	[PublicizedFrom(EAccessModifier.Private)]
	public const float SCALE_CUTOFF = 0.01f;

	// Token: 0x04004EBA RID: 20154
	[PublicizedFrom(EAccessModifier.Private)]
	public const float RENDER_SCALE = 0.9f;

	// Token: 0x04004EBB RID: 20155
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly float4 waterColor = new float4(0f, 0f, 1f, 1f);

	// Token: 0x04004EBC RID: 20156
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly float4 overfullColor = new float4(1f, 0f, 1f, 1f);

	// Token: 0x04004EBD RID: 20157
	[PublicizedFrom(EAccessModifier.Private)]
	public MaterialPropertyBlock materialProperties;

	// Token: 0x04004EBE RID: 20158
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 layerOrigin;

	// Token: 0x04004EBF RID: 20159
	[PublicizedFrom(EAccessModifier.Private)]
	public Bounds bounds;

	// Token: 0x04004EC0 RID: 20160
	[PublicizedFrom(EAccessModifier.Private)]
	public Matrix4x4[] transforms;

	// Token: 0x04004EC1 RID: 20161
	[PublicizedFrom(EAccessModifier.Private)]
	public float4[] colors;

	// Token: 0x04004EC2 RID: 20162
	[PublicizedFrom(EAccessModifier.Private)]
	public bool transformsHaveChanged;

	// Token: 0x04004EC3 RID: 20163
	[PublicizedFrom(EAccessModifier.Private)]
	public float[] normalizedMass;

	// Token: 0x04004EC4 RID: 20164
	[PublicizedFrom(EAccessModifier.Private)]
	public bool massesHaveChanged;

	// Token: 0x04004EC5 RID: 20165
	[PublicizedFrom(EAccessModifier.Private)]
	public ComputeBuffer transformsBuffer;

	// Token: 0x04004EC6 RID: 20166
	[PublicizedFrom(EAccessModifier.Private)]
	public ComputeBuffer massBuffer;

	// Token: 0x04004EC7 RID: 20167
	[PublicizedFrom(EAccessModifier.Private)]
	public ComputeBuffer colorBuffer;

	// Token: 0x04004EC9 RID: 20169
	[PublicizedFrom(EAccessModifier.Private)]
	public int totalWater;
}

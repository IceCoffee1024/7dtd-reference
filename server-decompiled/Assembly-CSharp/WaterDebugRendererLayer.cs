using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x02000CF8 RID: 3320
public class WaterDebugRendererLayer : IMemoryPoolableObject
{
	// Token: 0x17000AAA RID: 2730
	// (get) Token: 0x0600650D RID: 25869 RVA: 0x0027C002 File Offset: 0x0027A202
	// (set) Token: 0x0600650E RID: 25870 RVA: 0x0027C00A File Offset: 0x0027A20A
	public bool IsInitialized
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x0600650F RID: 25871 RVA: 0x0027C014 File Offset: 0x0027A214
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

	// Token: 0x06006510 RID: 25872 RVA: 0x0027C080 File Offset: 0x0027A280
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

	// Token: 0x06006511 RID: 25873 RVA: 0x0027C1B4 File Offset: 0x0027A3B4
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnOriginChanged(Vector3 _origin)
	{
		if (this.IsInitialized)
		{
			this.RegenerateTransforms();
		}
	}

	// Token: 0x06006512 RID: 25874 RVA: 0x0027C1C4 File Offset: 0x0027A3C4
	public void SetLayerOrigin(Vector3 _origin)
	{
		this.layerOrigin = _origin;
		if (this.IsInitialized)
		{
			this.RegenerateTransforms();
		}
	}

	// Token: 0x06006513 RID: 25875 RVA: 0x0027C1DB File Offset: 0x0027A3DB
	[PublicizedFrom(EAccessModifier.Private)]
	public int CoordToIndex(int _x, int _y, int _z)
	{
		return _x + 16 * _y + 256 * _z;
	}

	// Token: 0x06006514 RID: 25876 RVA: 0x0027C1EC File Offset: 0x0027A3EC
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

	// Token: 0x06006515 RID: 25877 RVA: 0x0027C2B4 File Offset: 0x0027A4B4
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

	// Token: 0x06006516 RID: 25878 RVA: 0x0027C3FC File Offset: 0x0027A5FC
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

	// Token: 0x06006517 RID: 25879 RVA: 0x0027C45C File Offset: 0x0027A65C
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

	// Token: 0x04004EDA RID: 20186
	public const int dimX = 16;

	// Token: 0x04004EDB RID: 20187
	public const int dimY = 16;

	// Token: 0x04004EDC RID: 20188
	public const int dimZ = 16;

	// Token: 0x04004EDD RID: 20189
	public const int elementsPerLayer = 4096;

	// Token: 0x04004EDE RID: 20190
	[PublicizedFrom(EAccessModifier.Private)]
	public const float SCALE_CUTOFF = 0.01f;

	// Token: 0x04004EDF RID: 20191
	[PublicizedFrom(EAccessModifier.Private)]
	public const float RENDER_SCALE = 0.9f;

	// Token: 0x04004EE0 RID: 20192
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly float4 waterColor = new float4(0f, 0f, 1f, 1f);

	// Token: 0x04004EE1 RID: 20193
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly float4 overfullColor = new float4(1f, 0f, 1f, 1f);

	// Token: 0x04004EE2 RID: 20194
	[PublicizedFrom(EAccessModifier.Private)]
	public MaterialPropertyBlock materialProperties;

	// Token: 0x04004EE3 RID: 20195
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 layerOrigin;

	// Token: 0x04004EE4 RID: 20196
	[PublicizedFrom(EAccessModifier.Private)]
	public Bounds bounds;

	// Token: 0x04004EE5 RID: 20197
	[PublicizedFrom(EAccessModifier.Private)]
	public Matrix4x4[] transforms;

	// Token: 0x04004EE6 RID: 20198
	[PublicizedFrom(EAccessModifier.Private)]
	public float4[] colors;

	// Token: 0x04004EE7 RID: 20199
	[PublicizedFrom(EAccessModifier.Private)]
	public bool transformsHaveChanged;

	// Token: 0x04004EE8 RID: 20200
	[PublicizedFrom(EAccessModifier.Private)]
	public float[] normalizedMass;

	// Token: 0x04004EE9 RID: 20201
	[PublicizedFrom(EAccessModifier.Private)]
	public bool massesHaveChanged;

	// Token: 0x04004EEA RID: 20202
	[PublicizedFrom(EAccessModifier.Private)]
	public ComputeBuffer transformsBuffer;

	// Token: 0x04004EEB RID: 20203
	[PublicizedFrom(EAccessModifier.Private)]
	public ComputeBuffer massBuffer;

	// Token: 0x04004EEC RID: 20204
	[PublicizedFrom(EAccessModifier.Private)]
	public ComputeBuffer colorBuffer;

	// Token: 0x04004EEE RID: 20206
	[PublicizedFrom(EAccessModifier.Private)]
	public int totalWater;
}

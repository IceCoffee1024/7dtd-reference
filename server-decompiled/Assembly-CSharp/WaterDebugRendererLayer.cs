using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x02000CDF RID: 3295
public class WaterDebugRendererLayer : IMemoryPoolableObject
{
	// Token: 0x17000A8E RID: 2702
	// (get) Token: 0x060064F6 RID: 25846 RVA: 0x00277736 File Offset: 0x00275936
	// (set) Token: 0x060064F7 RID: 25847 RVA: 0x0027773E File Offset: 0x0027593E
	public bool IsInitialized
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x060064F8 RID: 25848 RVA: 0x00277748 File Offset: 0x00275948
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

	// Token: 0x060064F9 RID: 25849 RVA: 0x002777B4 File Offset: 0x002759B4
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

	// Token: 0x060064FA RID: 25850 RVA: 0x002778E8 File Offset: 0x00275AE8
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnOriginChanged(Vector3 _origin)
	{
		if (this.IsInitialized)
		{
			this.RegenerateTransforms();
		}
	}

	// Token: 0x060064FB RID: 25851 RVA: 0x002778F8 File Offset: 0x00275AF8
	public void SetLayerOrigin(Vector3 _origin)
	{
		this.layerOrigin = _origin;
		if (this.IsInitialized)
		{
			this.RegenerateTransforms();
		}
	}

	// Token: 0x060064FC RID: 25852 RVA: 0x0027790F File Offset: 0x00275B0F
	[PublicizedFrom(EAccessModifier.Private)]
	public int CoordToIndex(int _x, int _y, int _z)
	{
		return _x + 16 * _y + 256 * _z;
	}

	// Token: 0x060064FD RID: 25853 RVA: 0x00277920 File Offset: 0x00275B20
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

	// Token: 0x060064FE RID: 25854 RVA: 0x002779E8 File Offset: 0x00275BE8
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

	// Token: 0x060064FF RID: 25855 RVA: 0x00277B30 File Offset: 0x00275D30
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

	// Token: 0x06006500 RID: 25856 RVA: 0x00277B90 File Offset: 0x00275D90
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

	// Token: 0x04004E7F RID: 20095
	public const int dimX = 16;

	// Token: 0x04004E80 RID: 20096
	public const int dimY = 16;

	// Token: 0x04004E81 RID: 20097
	public const int dimZ = 16;

	// Token: 0x04004E82 RID: 20098
	public const int elementsPerLayer = 4096;

	// Token: 0x04004E83 RID: 20099
	[PublicizedFrom(EAccessModifier.Private)]
	public const float SCALE_CUTOFF = 0.01f;

	// Token: 0x04004E84 RID: 20100
	[PublicizedFrom(EAccessModifier.Private)]
	public const float RENDER_SCALE = 0.9f;

	// Token: 0x04004E85 RID: 20101
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly float4 waterColor = new float4(0f, 0f, 1f, 1f);

	// Token: 0x04004E86 RID: 20102
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly float4 overfullColor = new float4(1f, 0f, 1f, 1f);

	// Token: 0x04004E87 RID: 20103
	[PublicizedFrom(EAccessModifier.Private)]
	public MaterialPropertyBlock materialProperties;

	// Token: 0x04004E88 RID: 20104
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 layerOrigin;

	// Token: 0x04004E89 RID: 20105
	[PublicizedFrom(EAccessModifier.Private)]
	public Bounds bounds;

	// Token: 0x04004E8A RID: 20106
	[PublicizedFrom(EAccessModifier.Private)]
	public Matrix4x4[] transforms;

	// Token: 0x04004E8B RID: 20107
	[PublicizedFrom(EAccessModifier.Private)]
	public float4[] colors;

	// Token: 0x04004E8C RID: 20108
	[PublicizedFrom(EAccessModifier.Private)]
	public bool transformsHaveChanged;

	// Token: 0x04004E8D RID: 20109
	[PublicizedFrom(EAccessModifier.Private)]
	public float[] normalizedMass;

	// Token: 0x04004E8E RID: 20110
	[PublicizedFrom(EAccessModifier.Private)]
	public bool massesHaveChanged;

	// Token: 0x04004E8F RID: 20111
	[PublicizedFrom(EAccessModifier.Private)]
	public ComputeBuffer transformsBuffer;

	// Token: 0x04004E90 RID: 20112
	[PublicizedFrom(EAccessModifier.Private)]
	public ComputeBuffer massBuffer;

	// Token: 0x04004E91 RID: 20113
	[PublicizedFrom(EAccessModifier.Private)]
	public ComputeBuffer colorBuffer;

	// Token: 0x04004E93 RID: 20115
	[PublicizedFrom(EAccessModifier.Private)]
	public int totalWater;
}

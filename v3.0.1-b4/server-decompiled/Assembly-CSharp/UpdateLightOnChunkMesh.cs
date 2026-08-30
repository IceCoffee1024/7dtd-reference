using System;
using UnityEngine;

// Token: 0x020012DE RID: 4830
public class UpdateLightOnChunkMesh : MonoBehaviour
{
	// Token: 0x060098CD RID: 39117 RVA: 0x0039E609 File Offset: 0x0039C809
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Awake()
	{
		if (GameManager.IsDedicatedServer)
		{
			base.enabled = false;
			return;
		}
		this.Reset();
	}

	// Token: 0x060098CE RID: 39118 RVA: 0x0039E620 File Offset: 0x0039C820
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Start()
	{
		if (GameManager.IsDedicatedServer)
		{
			return;
		}
		this.checkLight();
	}

	// Token: 0x060098CF RID: 39119 RVA: 0x0039E630 File Offset: 0x0039C830
	public void SetChunkMesh(VoxelMesh _chunkMesh)
	{
		this.chunkMesh = _chunkMesh;
	}

	// Token: 0x060098D0 RID: 39120 RVA: 0x0039E63C File Offset: 0x0039C83C
	public void Reset()
	{
		this.chunkMesh = null;
		this.lastTimeLightBrightnessChecked = 0f;
		this.lastSunLight = (this.lastBlockLight = byte.MaxValue);
	}

	// Token: 0x060098D1 RID: 39121 RVA: 0x0039E66F File Offset: 0x0039C86F
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update()
	{
		if (GameManager.IsDedicatedServer)
		{
			return;
		}
		if (Time.time - this.lastTimeLightBrightnessChecked < this.cTimeBrightnessUpdate)
		{
			return;
		}
		this.lastTimeLightBrightnessChecked = Time.time;
		this.checkLight();
	}

	// Token: 0x060098D2 RID: 39122 RVA: 0x0039E6A0 File Offset: 0x0039C8A0
	[PublicizedFrom(EAccessModifier.Private)]
	public void checkLight()
	{
		if (this.chunkMesh == null)
		{
			return;
		}
		GameManager instance = GameManager.Instance;
		if (!instance || !instance.gameStateManager.IsGameStarted())
		{
			return;
		}
		World world = instance.World;
		if (world == null)
		{
			return;
		}
		if (this.meshFilter == null)
		{
			this.meshFilter = base.transform.GetComponent<MeshFilter>();
		}
		if (this.meshRenderer == null)
		{
			this.meshRenderer = base.transform.GetComponent<MeshRenderer>();
		}
		if (this.meshFilter == null || this.meshRenderer == null)
		{
			return;
		}
		byte b;
		byte b2;
		world.GetSunAndBlockColors(World.worldToBlockPos(base.transform.position + Origin.position), out b, out b2);
		byte b3;
		byte b4;
		world.GetSunAndBlockColors(World.worldToBlockPos(base.transform.position + Vector3.up + Origin.position), out b3, out b4);
		byte b5 = Utils.FastMax(b, b3);
		byte b6 = Utils.FastMax(b2, b4);
		Color color = (world.IsDaytime() ? world.m_WorldEnvironment.GetSunLightColor() : world.m_WorldEnvironment.GetMoonLightColor());
		color.a = 1f;
		if (b5 != this.lastSunLight || b6 != this.lastBlockLight || !color.Equals(this.lastSunMoonLight))
		{
			this.lastSunLight = b5;
			this.lastBlockLight = b6;
			this.lastSunMoonLight = color;
			this.meshFilter.mesh.colors = this.chunkMesh.UpdateColors(b5, b6);
			this.meshRenderer.material.SetColor("_SunMoonlight", color);
		}
	}

	// Token: 0x04007309 RID: 29449
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float lastTimeLightBrightnessChecked;

	// Token: 0x0400730A RID: 29450
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float cTimeBrightnessUpdate = 0.5f;

	// Token: 0x0400730B RID: 29451
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public byte lastSunLight;

	// Token: 0x0400730C RID: 29452
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public byte lastBlockLight;

	// Token: 0x0400730D RID: 29453
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Color lastSunMoonLight;

	// Token: 0x0400730E RID: 29454
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public MeshFilter meshFilter;

	// Token: 0x0400730F RID: 29455
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public MeshRenderer meshRenderer;

	// Token: 0x04007310 RID: 29456
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public VoxelMesh chunkMesh;
}

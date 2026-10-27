using System;
using UnityEngine;

// Token: 0x0200132A RID: 4906
public class UpdateLightOnChunkMesh : MonoBehaviour
{
	// Token: 0x06009ACC RID: 39628 RVA: 0x003AA855 File Offset: 0x003A8A55
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

	// Token: 0x06009ACD RID: 39629 RVA: 0x003AA86C File Offset: 0x003A8A6C
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Start()
	{
		if (GameManager.IsDedicatedServer)
		{
			return;
		}
		this.checkLight();
	}

	// Token: 0x06009ACE RID: 39630 RVA: 0x003AA87C File Offset: 0x003A8A7C
	public void SetChunkMesh(VoxelMesh _chunkMesh)
	{
		this.chunkMesh = _chunkMesh;
	}

	// Token: 0x06009ACF RID: 39631 RVA: 0x003AA888 File Offset: 0x003A8A88
	public void Reset()
	{
		this.chunkMesh = null;
		this.lastTimeLightBrightnessChecked = 0f;
		this.lastSunLight = (this.lastBlockLight = byte.MaxValue);
	}

	// Token: 0x06009AD0 RID: 39632 RVA: 0x003AA8BB File Offset: 0x003A8ABB
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

	// Token: 0x06009AD1 RID: 39633 RVA: 0x003AA8EC File Offset: 0x003A8AEC
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

	// Token: 0x0400749D RID: 29853
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float lastTimeLightBrightnessChecked;

	// Token: 0x0400749E RID: 29854
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float cTimeBrightnessUpdate = 0.5f;

	// Token: 0x0400749F RID: 29855
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public byte lastSunLight;

	// Token: 0x040074A0 RID: 29856
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public byte lastBlockLight;

	// Token: 0x040074A1 RID: 29857
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Color lastSunMoonLight;

	// Token: 0x040074A2 RID: 29858
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public MeshFilter meshFilter;

	// Token: 0x040074A3 RID: 29859
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public MeshRenderer meshRenderer;

	// Token: 0x040074A4 RID: 29860
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public VoxelMesh chunkMesh;
}

using System;
using UnityEngine;

// Token: 0x02001318 RID: 4888
public class UpdateLightOnChunkMesh : MonoBehaviour
{
	// Token: 0x06009A74 RID: 39540 RVA: 0x003A8451 File Offset: 0x003A6651
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

	// Token: 0x06009A75 RID: 39541 RVA: 0x003A8468 File Offset: 0x003A6668
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Start()
	{
		if (GameManager.IsDedicatedServer)
		{
			return;
		}
		this.checkLight();
	}

	// Token: 0x06009A76 RID: 39542 RVA: 0x003A8478 File Offset: 0x003A6678
	public void SetChunkMesh(VoxelMesh _chunkMesh)
	{
		this.chunkMesh = _chunkMesh;
	}

	// Token: 0x06009A77 RID: 39543 RVA: 0x003A8484 File Offset: 0x003A6684
	public void Reset()
	{
		this.chunkMesh = null;
		this.lastTimeLightBrightnessChecked = 0f;
		this.lastSunLight = (this.lastBlockLight = byte.MaxValue);
	}

	// Token: 0x06009A78 RID: 39544 RVA: 0x003A84B7 File Offset: 0x003A66B7
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

	// Token: 0x06009A79 RID: 39545 RVA: 0x003A84E8 File Offset: 0x003A66E8
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

	// Token: 0x0400745C RID: 29788
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float lastTimeLightBrightnessChecked;

	// Token: 0x0400745D RID: 29789
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float cTimeBrightnessUpdate = 0.5f;

	// Token: 0x0400745E RID: 29790
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public byte lastSunLight;

	// Token: 0x0400745F RID: 29791
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public byte lastBlockLight;

	// Token: 0x04007460 RID: 29792
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Color lastSunMoonLight;

	// Token: 0x04007461 RID: 29793
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public MeshFilter meshFilter;

	// Token: 0x04007462 RID: 29794
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public MeshRenderer meshRenderer;

	// Token: 0x04007463 RID: 29795
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public VoxelMesh chunkMesh;
}

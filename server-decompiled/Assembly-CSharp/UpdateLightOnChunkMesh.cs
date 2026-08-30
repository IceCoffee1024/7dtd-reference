using System;
using UnityEngine;

// Token: 0x020012F8 RID: 4856
public class UpdateLightOnChunkMesh : MonoBehaviour
{
	// Token: 0x060099C8 RID: 39368 RVA: 0x003A38A1 File Offset: 0x003A1AA1
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

	// Token: 0x060099C9 RID: 39369 RVA: 0x003A38B8 File Offset: 0x003A1AB8
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Start()
	{
		if (GameManager.IsDedicatedServer)
		{
			return;
		}
		this.checkLight();
	}

	// Token: 0x060099CA RID: 39370 RVA: 0x003A38C8 File Offset: 0x003A1AC8
	public void SetChunkMesh(VoxelMesh _chunkMesh)
	{
		this.chunkMesh = _chunkMesh;
	}

	// Token: 0x060099CB RID: 39371 RVA: 0x003A38D4 File Offset: 0x003A1AD4
	public void Reset()
	{
		this.chunkMesh = null;
		this.lastTimeLightBrightnessChecked = 0f;
		this.lastSunLight = (this.lastBlockLight = byte.MaxValue);
	}

	// Token: 0x060099CC RID: 39372 RVA: 0x003A3907 File Offset: 0x003A1B07
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

	// Token: 0x060099CD RID: 39373 RVA: 0x003A3938 File Offset: 0x003A1B38
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

	// Token: 0x040073E1 RID: 29665
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float lastTimeLightBrightnessChecked;

	// Token: 0x040073E2 RID: 29666
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float cTimeBrightnessUpdate = 0.5f;

	// Token: 0x040073E3 RID: 29667
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public byte lastSunLight;

	// Token: 0x040073E4 RID: 29668
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public byte lastBlockLight;

	// Token: 0x040073E5 RID: 29669
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Color lastSunMoonLight;

	// Token: 0x040073E6 RID: 29670
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public MeshFilter meshFilter;

	// Token: 0x040073E7 RID: 29671
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public MeshRenderer meshRenderer;

	// Token: 0x040073E8 RID: 29672
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public VoxelMesh chunkMesh;
}

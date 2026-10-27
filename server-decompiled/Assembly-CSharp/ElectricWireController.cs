using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

// Token: 0x020003D3 RID: 979
public class ElectricWireController : MonoBehaviour
{
	// Token: 0x17000389 RID: 905
	// (get) Token: 0x06001D65 RID: 7525 RVA: 0x000B1897 File Offset: 0x000AFA97
	// (set) Token: 0x06001D66 RID: 7526 RVA: 0x000B18A0 File Offset: 0x000AFAA0
	public float HealthRatio
	{
		get
		{
			return this.healthRatio;
		}
		set
		{
			this.lastHealthRatio = this.healthRatio;
			this.healthRatio = value;
			if (this.lastHealthRatio != -1f)
			{
				if (this.lastHealthRatio > this.brokenPercentage && this.healthRatio <= this.brokenPercentage)
				{
					this.setWireDip(true);
					return;
				}
				if (this.lastHealthRatio <= this.brokenPercentage && this.healthRatio > this.brokenPercentage)
				{
					this.setWireDip(false);
				}
			}
		}
	}

	// Token: 0x06001D67 RID: 7527 RVA: 0x000B1914 File Offset: 0x000AFB14
	[PublicizedFrom(EAccessModifier.Private)]
	public void setWireDip(bool dip)
	{
		float wireDip = this.WireNode.GetWireDip();
		if (dip)
		{
			if (Mathf.Approximately(wireDip, 0f))
			{
				this.WireNode.SetWireDip(0.25f);
				this.WireNode.BuildMesh();
				return;
			}
		}
		else if (!Mathf.Approximately(wireDip, 0f))
		{
			this.WireNode.SetWireDip(0f);
			this.WireNode.BuildMesh();
		}
	}

	// Token: 0x06001D68 RID: 7528 RVA: 0x000B1984 File Offset: 0x000AFB84
	public void Init(DynamicProperties _properties)
	{
		if (_properties.Values.ContainsKey("Buff"))
		{
			if (this.buffActions == null)
			{
				this.buffActions = new List<string>();
			}
			string[] array = _properties.Values["Buff"].Split(',', StringSplitOptions.None);
			for (int i = 0; i < array.Length; i++)
			{
				this.buffActions.Add(array[i]);
			}
		}
		if (_properties.Values.ContainsKey("BreakingPercentage"))
		{
			this.breakingPercentage = Mathf.Clamp01(StringParsers.ParseFloat(_properties.Values["BreakingPercentage"], 0, -1, NumberStyles.Any));
		}
		else
		{
			this.breakingPercentage = 0.5f;
		}
		if (_properties.Values.ContainsKey("BrokenPercentage"))
		{
			this.brokenPercentage = Mathf.Clamp01(StringParsers.ParseFloat(_properties.Values["BrokenPercentage"], 0, -1, NumberStyles.Any));
		}
		else
		{
			this.brokenPercentage = 0.25f;
		}
		if (_properties.Values.ContainsKey("DamageReceived"))
		{
			StringParsers.TryParseFloat(_properties.Values["DamageReceived"], out this.damageReceived);
		}
		else
		{
			this.damageReceived = 0.1f;
		}
		this.healthRatio = -1f;
		this.BlockPosition = this.TileEntityChild.ToWorldPos();
		this.startPoint = this.WireNode.GetStartPosition() + this.WireNode.GetStartPositionOffset();
		this.endPoint = this.WireNode.GetEndPosition() + this.WireNode.GetEndPositionOffset();
		BlockValue block = GameManager.Instance.World.GetBlock(this.BlockPosition);
		float num = 1f - (float)block.damage / (float)block.Block.MaxDamage;
		if (num <= this.brokenPercentage)
		{
			this.setWireDip(true);
			return;
		}
		if (num > this.brokenPercentage)
		{
			this.setWireDip(false);
		}
	}

	// Token: 0x06001D69 RID: 7529 RVA: 0x000B1B64 File Offset: 0x000AFD64
	public void DamageSelf(float damage)
	{
		this.totalDamage += damage;
		if (this.totalDamage < 1f)
		{
			return;
		}
		damage = (float)((int)this.totalDamage);
		this.totalDamage = 0f;
		if (this.chunk == null)
		{
			this.chunk = (Chunk)GameManager.Instance.World.GetChunkFromWorldPos(this.BlockPosition);
		}
		BlockValue block = GameManager.Instance.World.GetBlock(this.BlockPosition);
		this.HealthRatio = 1f - (float)block.damage / (float)block.Block.MaxDamage;
		float num = this.HealthRatio;
		float num2 = ((float)block.damage + damage) / (float)block.Block.MaxDamage;
		block.damage = Mathf.Clamp(block.damage + (int)damage, 0, block.Block.MaxDamage);
		GameManager.Instance.World.SetBlockRPC(this.BlockPosition, block);
	}

	// Token: 0x06001D6A RID: 7530 RVA: 0x000B1C60 File Offset: 0x000AFE60
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update()
	{
		BlockValue block = GameManager.Instance.World.GetBlock(this.BlockPosition);
		this.HealthRatio = 1f - (float)block.damage / (float)block.Block.MaxDamage;
		bool flag = this.HealthRatio < this.brokenPercentage;
		this.HandleParticlesForBroken(flag);
		this.setWireDip(flag);
		if (this.TileEntityParent == null || !this.TileEntityParent.IsPowered)
		{
			if (this.CollidersThisFrame != null && this.CollidersThisFrame.Count > 0)
			{
				this.CollidersThisFrame.Clear();
			}
			return;
		}
		if (this.CollidersThisFrame == null || this.CollidersThisFrame.Count == 0)
		{
			return;
		}
		for (int i = 0; i < this.CollidersThisFrame.Count; i++)
		{
			this.touched(this.CollidersThisFrame[i]);
		}
		this.CollidersThisFrame.Clear();
	}

	// Token: 0x06001D6B RID: 7531 RVA: 0x000B1D44 File Offset: 0x000AFF44
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnTriggerEnter(Collider other)
	{
		if (this.TileEntityParent == null || !this.TileEntityParent.IsPowered)
		{
			return;
		}
		if (this.CollidersThisFrame == null)
		{
			this.CollidersThisFrame = new List<Collider>();
		}
		if (!this.CollidersThisFrame.Contains(other))
		{
			this.CollidersThisFrame.Add(other);
		}
	}

	// Token: 0x06001D6C RID: 7532 RVA: 0x000B1D94 File Offset: 0x000AFF94
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnTriggerStay(Collider other)
	{
		if (this.TileEntityParent == null || !this.TileEntityParent.IsPowered)
		{
			return;
		}
		if (this.CollidersThisFrame == null)
		{
			this.CollidersThisFrame = new List<Collider>();
		}
		if (!this.CollidersThisFrame.Contains(other))
		{
			this.CollidersThisFrame.Add(other);
		}
	}

	// Token: 0x06001D6D RID: 7533 RVA: 0x000B1DE4 File Offset: 0x000AFFE4
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnTriggerExit(Collider other)
	{
		if (this.TileEntityParent == null || !this.TileEntityParent.IsPowered)
		{
			return;
		}
		if (this.CollidersThisFrame == null)
		{
			this.CollidersThisFrame = new List<Collider>();
		}
		if (!this.CollidersThisFrame.Contains(other))
		{
			this.CollidersThisFrame.Add(other);
		}
	}

	// Token: 0x06001D6E RID: 7534 RVA: 0x000B1E34 File Offset: 0x000B0034
	[PublicizedFrom(EAccessModifier.Private)]
	public void touched(Collider collider)
	{
		if (this.TileEntityParent == null || this.TileEntityChild == null || this.WireNode == null || collider == null)
		{
			return;
		}
		if (this.TileEntityParent.IsPowered && this.TileEntityChild.IsPowered && collider.transform != null)
		{
			EntityAlive entityAlive = collider.transform.GetComponent<EntityAlive>();
			if (entityAlive == null)
			{
				entityAlive = collider.transform.GetComponentInParent<EntityAlive>();
			}
			if (entityAlive == null && collider.transform.parent != null)
			{
				entityAlive = collider.transform.parent.GetComponentInChildren<EntityAlive>();
			}
			if (entityAlive == null)
			{
				entityAlive = collider.transform.GetComponentInChildren<EntityAlive>();
			}
			if (entityAlive != null && entityAlive.IsAlive())
			{
				bool flag = false;
				if (this.HealthRatio < this.brokenPercentage)
				{
					this.HandleParticlesForBroken(true);
					return;
				}
				if (!entityAlive.Electrocuted && entityAlive.Buffs.GetCustomVar("ShockImmunity") == 0f && this.buffActions != null)
				{
					for (int i = 0; i < this.buffActions.Count; i++)
					{
						if (entityAlive.emodel != null && entityAlive.emodel.transform != null)
						{
							Transform transform = entityAlive.emodel.transform;
							if (entityAlive.emodel.GetHitTransform(BodyPrimaryHit.Torso) != null)
							{
								entityAlive.Buffs.SetCustomVar("ETrapHit", 1f, true, CVarOperation.set, false);
								entityAlive.Buffs.AddBuff(this.buffActions[i], this.TileEntityParent.OwnerEntityID, true, true, -1f);
								entityAlive.Electrocuted = true;
								flag = true;
							}
						}
					}
				}
				if (flag)
				{
					this.DamageSelf(this.damageReceived);
				}
			}
		}
	}

	// Token: 0x06001D6F RID: 7535 RVA: 0x000B2010 File Offset: 0x000B0210
	[PublicizedFrom(EAccessModifier.Private)]
	public static string GetGameObjectPath(Entity e, Transform transform)
	{
		string text = transform.name;
		while (transform.parent != null && transform.parent.name != e.transform.name)
		{
			transform = transform.parent;
			text = transform.name + "/" + text;
		}
		return text;
	}

	// Token: 0x06001D70 RID: 7536 RVA: 0x000B206C File Offset: 0x000B026C
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleParticlesForBroken(bool isBroken)
	{
		if (isBroken && this.TileEntityParent.IsPowered)
		{
			if (this.particleDelay > 0f)
			{
				this.particleDelay -= Time.deltaTime;
			}
			if (isBroken && this.particleDelay <= 0f)
			{
				Vector3 vector = this.WireNode.GetEndPosition() + this.WireNode.GetEndPositionOffset();
				float num = GameManager.Instance.World.GetLightBrightness(World.worldToBlockPos(this.BlockPosition.ToVector3())) / 2f;
				ParticleEffect particleEffect = new ParticleEffect("electric_fence_sparks", vector, num, new Color(1f, 1f, 1f, 0.3f), "electric_fence_impact", null, false, 1f, "");
				GameManager.Instance.SpawnParticleEffectServer(particleEffect, -1, true, true);
				this.particleDelay = 1f + global::UnityEngine.Random.value * 4f;
			}
		}
	}

	// Token: 0x0400135F RID: 4959
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cDipValue = 0.25f;

	// Token: 0x04001360 RID: 4960
	public TileEntityPoweredMeleeTrap TileEntityParent;

	// Token: 0x04001361 RID: 4961
	public TileEntityPoweredMeleeTrap TileEntityChild;

	// Token: 0x04001362 RID: 4962
	public IWireNode WireNode;

	// Token: 0x04001363 RID: 4963
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float healthRatio = -1f;

	// Token: 0x04001364 RID: 4964
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<string> buffActions;

	// Token: 0x04001365 RID: 4965
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public static string PropDamageReceived = "Damage_received";

	// Token: 0x04001366 RID: 4966
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float damageReceived;

	// Token: 0x04001367 RID: 4967
	public Vector3i BlockPosition;

	// Token: 0x04001368 RID: 4968
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Chunk chunk;

	// Token: 0x04001369 RID: 4969
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float totalDamage;

	// Token: 0x0400136A RID: 4970
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float particleDelay;

	// Token: 0x0400136B RID: 4971
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float brokenPercentage;

	// Token: 0x0400136C RID: 4972
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float breakingPercentage;

	// Token: 0x0400136D RID: 4973
	public int OwnerEntityID = -1;

	// Token: 0x0400136E RID: 4974
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 startPoint;

	// Token: 0x0400136F RID: 4975
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 endPoint;

	// Token: 0x04001370 RID: 4976
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<Collider> CollidersThisFrame;

	// Token: 0x04001371 RID: 4977
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float lastHealthRatio = 1f;
}

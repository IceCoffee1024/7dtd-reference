using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

// Token: 0x020003CA RID: 970
public class ElectricWireController : MonoBehaviour
{
	// Token: 0x1700038D RID: 909
	// (get) Token: 0x06001D4C RID: 7500 RVA: 0x000B0D3B File Offset: 0x000AEF3B
	// (set) Token: 0x06001D4D RID: 7501 RVA: 0x000B0D44 File Offset: 0x000AEF44
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

	// Token: 0x06001D4E RID: 7502 RVA: 0x000B0DB8 File Offset: 0x000AEFB8
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

	// Token: 0x06001D4F RID: 7503 RVA: 0x000B0E28 File Offset: 0x000AF028
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

	// Token: 0x06001D50 RID: 7504 RVA: 0x000B1008 File Offset: 0x000AF208
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

	// Token: 0x06001D51 RID: 7505 RVA: 0x000B1104 File Offset: 0x000AF304
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

	// Token: 0x06001D52 RID: 7506 RVA: 0x000B11E8 File Offset: 0x000AF3E8
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

	// Token: 0x06001D53 RID: 7507 RVA: 0x000B1238 File Offset: 0x000AF438
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

	// Token: 0x06001D54 RID: 7508 RVA: 0x000B1288 File Offset: 0x000AF488
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

	// Token: 0x06001D55 RID: 7509 RVA: 0x000B12D8 File Offset: 0x000AF4D8
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
								entityAlive.Buffs.SetCustomVar("ETrapHit", 1f, true, CVarOperation.set);
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

	// Token: 0x06001D56 RID: 7510 RVA: 0x000B14B0 File Offset: 0x000AF6B0
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

	// Token: 0x06001D57 RID: 7511 RVA: 0x000B150C File Offset: 0x000AF70C
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

	// Token: 0x04001341 RID: 4929
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cDipValue = 0.25f;

	// Token: 0x04001342 RID: 4930
	public TileEntityPoweredMeleeTrap TileEntityParent;

	// Token: 0x04001343 RID: 4931
	public TileEntityPoweredMeleeTrap TileEntityChild;

	// Token: 0x04001344 RID: 4932
	public IWireNode WireNode;

	// Token: 0x04001345 RID: 4933
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float healthRatio = -1f;

	// Token: 0x04001346 RID: 4934
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<string> buffActions;

	// Token: 0x04001347 RID: 4935
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public static string PropDamageReceived = "Damage_received";

	// Token: 0x04001348 RID: 4936
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float damageReceived;

	// Token: 0x04001349 RID: 4937
	public Vector3i BlockPosition;

	// Token: 0x0400134A RID: 4938
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Chunk chunk;

	// Token: 0x0400134B RID: 4939
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float totalDamage;

	// Token: 0x0400134C RID: 4940
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float particleDelay;

	// Token: 0x0400134D RID: 4941
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float brokenPercentage;

	// Token: 0x0400134E RID: 4942
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float breakingPercentage;

	// Token: 0x0400134F RID: 4943
	public int OwnerEntityID = -1;

	// Token: 0x04001350 RID: 4944
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 startPoint;

	// Token: 0x04001351 RID: 4945
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 endPoint;

	// Token: 0x04001352 RID: 4946
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<Collider> CollidersThisFrame;

	// Token: 0x04001353 RID: 4947
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float lastHealthRatio = 1f;
}

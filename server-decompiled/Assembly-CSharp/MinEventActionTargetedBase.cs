using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine;

// Token: 0x0200070C RID: 1804
public class MinEventActionTargetedBase : MinEventActionBase
{
	// Token: 0x060036F1 RID: 14065 RVA: 0x0016A5CC File Offset: 0x001687CC
	public override bool CanExecute(MinEventTypes _eventType, MinEventParams _params)
	{
		this.targets.Clear();
		if (this.targetType == MinEventActionTargetedBase.TargetTypes.self)
		{
			if (_params.Self != null && this.isValidTarget(_params.Self, null))
			{
				this.targets.Add(_params.Self);
				return this.singleTargetCheck(_params);
			}
		}
		else if (this.targetType == MinEventActionTargetedBase.TargetTypes.other)
		{
			if (_params.Others != null)
			{
				for (int i = 0; i < _params.Others.Length; i++)
				{
					if (this.isValidTarget(_params.Self, _params.Others[i]))
					{
						MinEventParams minEventParams = new MinEventParams();
						MinEventParams.CopyTo(_params, minEventParams);
						minEventParams.Other = _params.Others[i];
						if (this.singleTargetCheck(minEventParams))
						{
							this.targets.Add(_params.Others[i]);
						}
					}
				}
			}
			else if (_params.Other != null && this.isValidTarget(_params.Self, _params.Other) && this.singleTargetCheck(_params))
			{
				this.targets.Add(_params.Other);
			}
		}
		else if (this.targetType == MinEventActionTargetedBase.TargetTypes.selfAOE)
		{
			if (_params.Self != null)
			{
				this.entsInRange = GameManager.Instance.World.GetLivingEntitiesInBounds(_params.Self, new Bounds(_params.Self.position, Vector3.one * (this.maxRange * 2f)));
				for (int j = 0; j < this.entsInRange.Count; j++)
				{
					if (this.entsInRange[j] != null && this.isValidTarget(_params.Self, this.entsInRange[j]))
					{
						MinEventParams minEventParams2 = new MinEventParams();
						MinEventParams.CopyTo(_params, minEventParams2);
						minEventParams2.Other = this.entsInRange[j];
						if (this.singleTargetCheck(minEventParams2))
						{
							this.targets.Add(this.entsInRange[j]);
						}
					}
				}
				this.entsInRange.Clear();
			}
		}
		else if (this.targetType == MinEventActionTargetedBase.TargetTypes.selfOtherPlayers)
		{
			if (_params.Self != null)
			{
				List<EntityPlayer> players = GameManager.Instance.World.GetPlayers();
				for (int k = 0; k < players.Count; k++)
				{
					EntityPlayer entityPlayer = players[k];
					if (!(entityPlayer == _params.Self) && this.isValidTarget(_params.Self, entityPlayer))
					{
						MinEventParams minEventParams3 = new MinEventParams();
						MinEventParams.CopyTo(_params, minEventParams3);
						minEventParams3.Other = entityPlayer;
						if (this.singleTargetCheck(minEventParams3))
						{
							this.targets.Add(entityPlayer);
						}
					}
				}
			}
		}
		else if (this.targetType == MinEventActionTargetedBase.TargetTypes.otherAOE)
		{
			if (_params.Other != null)
			{
				this.entsInRange = GameManager.Instance.World.GetLivingEntitiesInBounds(_params.Self, new Bounds(_params.Other.position, Vector3.one * (this.maxRange * 2f)));
				for (int l = 0; l < this.entsInRange.Count; l++)
				{
					if (this.entsInRange[l] != null && this.isValidTarget(_params.Self, this.entsInRange[l]))
					{
						MinEventParams minEventParams4 = new MinEventParams();
						MinEventParams.CopyTo(_params, minEventParams4);
						minEventParams4.Other = this.entsInRange[l];
						if (this.singleTargetCheck(minEventParams4))
						{
							this.targets.Add(this.entsInRange[l]);
						}
					}
				}
				this.entsInRange.Clear();
			}
		}
		else if (this.targetType == MinEventActionTargetedBase.TargetTypes.positionAOE)
		{
			this.entsInRange = GameManager.Instance.World.GetLivingEntitiesInBounds(null, new Bounds(_params.Position, Vector3.one * (this.maxRange * 2f)));
			for (int m = 0; m < this.entsInRange.Count; m++)
			{
				EntityAlive entityAlive = this.entsInRange[m];
				if (entityAlive != null && this.isValidTarget(_params.Self, entityAlive) && (!(entityAlive is EntityPlayer) || (entityAlive.getHipPosition() - _params.Position).sqrMagnitude <= this.maxRange * this.maxRange))
				{
					MinEventParams minEventParams5 = new MinEventParams();
					MinEventParams.CopyTo(_params, minEventParams5);
					minEventParams5.Other = entityAlive;
					if (this.singleTargetCheck(minEventParams5))
					{
						this.targets.Add(entityAlive);
					}
				}
			}
			this.entsInRange.Clear();
		}
		return this.targets.Count > 0;
	}

	// Token: 0x060036F2 RID: 14066 RVA: 0x0016AA88 File Offset: 0x00168C88
	public override bool ParseXmlAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXmlAttribute(_attribute);
		if (!flag)
		{
			string localName = _attribute.Name.LocalName;
			if (localName == "target")
			{
				this.targetType = EnumUtils.Parse<MinEventActionTargetedBase.TargetTypes>(_attribute.Value, true);
				return true;
			}
			if (localName == "range")
			{
				this.maxRange = StringParsers.ParseFloat(_attribute.Value, 0, -1, NumberStyles.Any);
				return true;
			}
			if (localName == "target_tags")
			{
				this.targetTags = FastTags<TagGroup.Global>.Parse(_attribute.Value);
				return true;
			}
		}
		return flag;
	}

	// Token: 0x060036F3 RID: 14067 RVA: 0x0016AB17 File Offset: 0x00168D17
	[PublicizedFrom(EAccessModifier.Private)]
	public bool singleTargetCheck(MinEventParams _tempParams)
	{
		return this.Requirements == null || this.Requirements.IsValid(_tempParams);
	}

	// Token: 0x060036F4 RID: 14068 RVA: 0x0016AB30 File Offset: 0x00168D30
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isValidTarget(EntityAlive _self, EntityAlive _other)
	{
		if (this.targetTags.IsEmpty)
		{
			return true;
		}
		if (_self == null && _other != null && _other.HasAnyTags(this.targetTags))
		{
			return true;
		}
		if (_other == null && _self.HasAnyTags(this.targetTags))
		{
			return true;
		}
		if (_other.HasAnyTags(this.targetTags))
		{
			return true;
		}
		if (this.targetTags.Test_AnySet(MinEventActionTargetedBase.party) && _self as EntityPlayer != null && _other as EntityPlayer != null && (_self as EntityPlayer).Party != null && (_self as EntityPlayer).Party.ContainsMember(_other as EntityPlayer))
		{
			return true;
		}
		if (this.targetTags.Test_AnySet(MinEventActionTargetedBase.ally))
		{
			if (_self as EntityPlayer != null && _other as EntityPlayer != null && (_self as EntityPlayer).IsFriendsWith(_other as EntityPlayer))
			{
				return true;
			}
			if (_self as EntityEnemy != null && _other as EntityEnemy != null)
			{
				return true;
			}
			if (FactionManager.Instance != null)
			{
				if (_self as EntityPlayer != null && _other as EntityNPC != null)
				{
					if (FactionManager.Instance.GetRelationshipTier(_self, _other) == FactionManager.Relationship.Like)
					{
						return true;
					}
					if (FactionManager.Instance.GetRelationshipTier(_self, _other) == FactionManager.Relationship.Love)
					{
						return true;
					}
				}
				if (_self as EntityNPC != null && _other as EntityPlayer != null)
				{
					if (FactionManager.Instance.GetRelationshipTier(_self, _other) == FactionManager.Relationship.Like)
					{
						return true;
					}
					if (FactionManager.Instance.GetRelationshipTier(_self, _other) == FactionManager.Relationship.Love)
					{
						return true;
					}
				}
				if (_self as EntityNPC != null && _other as EntityNPC != null)
				{
					if (FactionManager.Instance.GetRelationshipTier(_self, _other) == FactionManager.Relationship.Like)
					{
						return true;
					}
					if (FactionManager.Instance.GetRelationshipTier(_self, _other) == FactionManager.Relationship.Love)
					{
						return true;
					}
				}
			}
		}
		if (this.targetTags.Test_AnySet(MinEventActionTargetedBase.enemy))
		{
			if (_self as EntityEnemy != null && _other as EntityPlayer != null)
			{
				return true;
			}
			if (_self as EntityPlayer != null && _other as EntityEnemy != null)
			{
				return true;
			}
			if (_self as EntityEnemy != null && _other as EntityNPC != null)
			{
				return true;
			}
			if (_self as EntityNPC != null && _other as EntityEnemy != null)
			{
				return true;
			}
			if (FactionManager.Instance != null)
			{
				if (_self as EntityPlayer != null && _other as EntityPlayer != null)
				{
					if (FactionManager.Instance.GetRelationshipTier(_self, _other) == FactionManager.Relationship.Hate)
					{
						return true;
					}
					if (FactionManager.Instance.GetRelationshipTier(_self, _other) == FactionManager.Relationship.Dislike)
					{
						return true;
					}
				}
				if (_self as EntityPlayer != null && _other as EntityNPC != null)
				{
					if (FactionManager.Instance.GetRelationshipTier(_self, _other) == FactionManager.Relationship.Hate)
					{
						return true;
					}
					if (FactionManager.Instance.GetRelationshipTier(_self, _other) == FactionManager.Relationship.Dislike)
					{
						return true;
					}
				}
				if (_self as EntityNPC != null && _other as EntityPlayer != null)
				{
					if (FactionManager.Instance.GetRelationshipTier(_self, _other) == FactionManager.Relationship.Hate)
					{
						return true;
					}
					if (FactionManager.Instance.GetRelationshipTier(_self, _other) == FactionManager.Relationship.Dislike)
					{
						return true;
					}
				}
				if (_self as EntityNPC != null && _other as EntityNPC != null)
				{
					if (FactionManager.Instance.GetRelationshipTier(_self, _other) == FactionManager.Relationship.Hate)
					{
						return true;
					}
					if (FactionManager.Instance.GetRelationshipTier(_self, _other) == FactionManager.Relationship.Dislike)
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	// Token: 0x04002C69 RID: 11369
	[PublicizedFrom(EAccessModifier.Protected)]
	public static FastTags<TagGroup.Global> ally = FastTags<TagGroup.Global>.Parse("ally");

	// Token: 0x04002C6A RID: 11370
	[PublicizedFrom(EAccessModifier.Protected)]
	public static FastTags<TagGroup.Global> party = FastTags<TagGroup.Global>.Parse("party");

	// Token: 0x04002C6B RID: 11371
	[PublicizedFrom(EAccessModifier.Protected)]
	public static FastTags<TagGroup.Global> enemy = FastTags<TagGroup.Global>.Parse("enemy");

	// Token: 0x04002C6C RID: 11372
	public MinEventActionTargetedBase.TargetTypes targetType;

	// Token: 0x04002C6D RID: 11373
	[PublicizedFrom(EAccessModifier.Protected)]
	public FastTags<TagGroup.Global> targetTags = FastTags<TagGroup.Global>.none;

	// Token: 0x04002C6E RID: 11374
	[PublicizedFrom(EAccessModifier.Protected)]
	public List<EntityAlive> targets = new List<EntityAlive>();

	// Token: 0x04002C6F RID: 11375
	[PublicizedFrom(EAccessModifier.Protected)]
	public float maxRange;

	// Token: 0x04002C70 RID: 11376
	[PublicizedFrom(EAccessModifier.Private)]
	public List<EntityAlive> entsInRange;

	// Token: 0x0200070D RID: 1805
	public enum TargetTypes
	{
		// Token: 0x04002C72 RID: 11378
		self,
		// Token: 0x04002C73 RID: 11379
		other,
		// Token: 0x04002C74 RID: 11380
		selfAOE,
		// Token: 0x04002C75 RID: 11381
		otherAOE,
		// Token: 0x04002C76 RID: 11382
		positionAOE,
		// Token: 0x04002C77 RID: 11383
		selfOtherPlayers
	}
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine;

// Token: 0x02000716 RID: 1814
public class MinEventActionTargetedBase : MinEventActionBase
{
	// Token: 0x0600374B RID: 14155 RVA: 0x0016BEC8 File Offset: 0x0016A0C8
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

	// Token: 0x0600374C RID: 14156 RVA: 0x0016C384 File Offset: 0x0016A584
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

	// Token: 0x0600374D RID: 14157 RVA: 0x0016C413 File Offset: 0x0016A613
	[PublicizedFrom(EAccessModifier.Private)]
	public bool singleTargetCheck(MinEventParams _tempParams)
	{
		return this.Requirements == null || this.Requirements.IsValid(_tempParams);
	}

	// Token: 0x0600374E RID: 14158 RVA: 0x0016C42C File Offset: 0x0016A62C
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

	// Token: 0x04002C7E RID: 11390
	[PublicizedFrom(EAccessModifier.Protected)]
	public static FastTags<TagGroup.Global> ally = FastTags<TagGroup.Global>.Parse("ally");

	// Token: 0x04002C7F RID: 11391
	[PublicizedFrom(EAccessModifier.Protected)]
	public static FastTags<TagGroup.Global> party = FastTags<TagGroup.Global>.Parse("party");

	// Token: 0x04002C80 RID: 11392
	[PublicizedFrom(EAccessModifier.Protected)]
	public static FastTags<TagGroup.Global> enemy = FastTags<TagGroup.Global>.Parse("enemy");

	// Token: 0x04002C81 RID: 11393
	public MinEventActionTargetedBase.TargetTypes targetType;

	// Token: 0x04002C82 RID: 11394
	[PublicizedFrom(EAccessModifier.Protected)]
	public FastTags<TagGroup.Global> targetTags = FastTags<TagGroup.Global>.none;

	// Token: 0x04002C83 RID: 11395
	[PublicizedFrom(EAccessModifier.Protected)]
	public List<EntityAlive> targets = new List<EntityAlive>();

	// Token: 0x04002C84 RID: 11396
	[PublicizedFrom(EAccessModifier.Protected)]
	public float maxRange;

	// Token: 0x04002C85 RID: 11397
	[PublicizedFrom(EAccessModifier.Private)]
	public List<EntityAlive> entsInRange;

	// Token: 0x02000717 RID: 1815
	public enum TargetTypes
	{
		// Token: 0x04002C87 RID: 11399
		self,
		// Token: 0x04002C88 RID: 11400
		other,
		// Token: 0x04002C89 RID: 11401
		selfAOE,
		// Token: 0x04002C8A RID: 11402
		otherAOE,
		// Token: 0x04002C8B RID: 11403
		positionAOE,
		// Token: 0x04002C8C RID: 11404
		selfOtherPlayers
	}
}

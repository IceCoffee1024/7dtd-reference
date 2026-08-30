using System;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020006F3 RID: 1779
[Preserve]
public class MinEventActionExplode : MinEventActionTargetedBase
{
	// Token: 0x06003666 RID: 13926 RVA: 0x00167198 File Offset: 0x00165398
	public override void Execute(MinEventParams _params)
	{
		if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			return;
		}
		new Vector3(0f, 0.5f, 0f);
		for (int i = 0; i < this.targets.Count; i++)
		{
			EntityAlive entityAlive = this.targets[i];
			if (entityAlive != null)
			{
				ExplosionData explosionData = new ExplosionData
				{
					BlastPower = this.blastPower,
					BlockDamage = (float)this.blockDamage,
					BlockRadius = (float)this.blockRadius,
					BlockTags = this.blockTags,
					EntityDamage = (float)this.entityDamage,
					EntityRadius = this.entityRadius,
					DamageType = this.damageType,
					ParticleIndex = 13
				};
				GameManager.Instance.ExplosionServer(entityAlive.getHeadPosition(), entityAlive.GetBlockPosition(), entityAlive.qrotation, explosionData, entityAlive.entityId, 0.1f, false, null);
			}
		}
	}

	// Token: 0x06003667 RID: 13927 RVA: 0x00167298 File Offset: 0x00165498
	public override bool ParseXmlAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXmlAttribute(_attribute);
		if (!flag)
		{
			string localName = _attribute.Name.LocalName;
			uint num = <PrivateImplementationDetails>.ComputeStringHash(localName);
			if (num <= 430148698U)
			{
				if (num != 69632989U)
				{
					if (num != 203327429U)
					{
						if (num == 430148698U)
						{
							if (localName == "block_tags")
							{
								this.blockTags = _attribute.Value;
							}
						}
					}
					else if (localName == "damage_type")
					{
						this.damageType = (EnumDamageTypes)Enum.Parse(typeof(EnumDamageTypes), _attribute.Value);
					}
				}
				else if (localName == "entity_radius")
				{
					this.entityRadius = StringParsers.ParseSInt32(_attribute.Value, 0, -1, NumberStyles.Integer);
				}
			}
			else if (num <= 1332589382U)
			{
				if (num != 796463477U)
				{
					if (num == 1332589382U)
					{
						if (localName == "block_damage")
						{
							this.blockDamage = StringParsers.ParseSInt32(_attribute.Value, 0, -1, NumberStyles.Integer);
							return true;
						}
					}
				}
				else if (localName == "block_radius")
				{
					this.blockRadius = StringParsers.ParseSInt32(_attribute.Value, 0, -1, NumberStyles.Integer);
				}
			}
			else if (num != 2865571118U)
			{
				if (num == 3191003235U)
				{
					if (localName == "blast_power")
					{
						this.blastPower = StringParsers.ParseSInt32(_attribute.Value, 0, -1, NumberStyles.Integer);
						return true;
					}
				}
			}
			else if (localName == "entity_damage")
			{
				this.entityDamage = StringParsers.ParseSInt32(_attribute.Value, 0, -1, NumberStyles.Integer);
			}
		}
		return flag;
	}

	// Token: 0x04002BB6 RID: 11190
	[PublicizedFrom(EAccessModifier.Protected)]
	public int blastPower = 75;

	// Token: 0x04002BB7 RID: 11191
	[PublicizedFrom(EAccessModifier.Protected)]
	public int blockDamage = 1;

	// Token: 0x04002BB8 RID: 11192
	[PublicizedFrom(EAccessModifier.Protected)]
	public int blockRadius = 4;

	// Token: 0x04002BB9 RID: 11193
	[PublicizedFrom(EAccessModifier.Protected)]
	public int entityDamage = 5000;

	// Token: 0x04002BBA RID: 11194
	[PublicizedFrom(EAccessModifier.Protected)]
	public int entityRadius = 3;

	// Token: 0x04002BBB RID: 11195
	[PublicizedFrom(EAccessModifier.Protected)]
	public string blockTags = "";

	// Token: 0x04002BBC RID: 11196
	[PublicizedFrom(EAccessModifier.Protected)]
	public EnumDamageTypes damageType = EnumDamageTypes.Heat;
}

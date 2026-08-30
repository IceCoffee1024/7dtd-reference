using System;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020006D2 RID: 1746
[Preserve]
public class MinEventActionRagdoll : MinEventActionTargetedBase
{
	// Token: 0x060035E7 RID: 13799 RVA: 0x00164340 File Offset: 0x00162540
	public override void Execute(MinEventParams _params)
	{
		DamageResponse damageResponse = DamageResponse.New(false);
		damageResponse.StunDuration = this.duration;
		damageResponse.Strength = (int)this.force;
		if (this.cvarRef && this.targets.Count > 0)
		{
			damageResponse.StunDuration = this.targets[0].Buffs.GetCustomVar(this.refCvarName);
		}
		Vector3 vector = _params.StartPosition;
		if (vector.y == 0f)
		{
			vector = _params.Self.position;
		}
		for (int i = 0; i < this.targets.Count; i++)
		{
			EntityAlive entityAlive = this.targets[i];
			if (entityAlive.AttachedToEntity != null)
			{
				entityAlive.Detach();
			}
			EModelBase.RagdollMode ragdollMode = EModelBase.RagdollMode.Default;
			Vector3 vector2 = entityAlive.position - vector;
			if (this.scaleY == 0f)
			{
				vector2.y = 0f;
				damageResponse.Source = new DamageSource(EnumDamageSource.External, EnumDamageTypes.Bashing, vector2.normalized);
			}
			else
			{
				ragdollMode = EModelBase.RagdollMode.FullForce;
				vector2.y = _params.Self.GetLookVector().y * this.scaleY;
				float num = this.force;
				if (this.massScale > 0f)
				{
					num *= EntityClass.list[entityAlive.entityClass].MassKg * this.massScale;
				}
				damageResponse.Source = new DamageSource(EnumDamageSource.External, EnumDamageTypes.Falling, vector2.normalized * num);
			}
			entityAlive.DoRagdoll(in damageResponse, ragdollMode);
		}
	}

	// Token: 0x060035E8 RID: 13800 RVA: 0x001644C4 File Offset: 0x001626C4
	public override bool ParseXmlAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXmlAttribute(_attribute);
		if (!flag)
		{
			string localName = _attribute.Name.LocalName;
			if (localName == "duration")
			{
				if (_attribute.Value.StartsWith("@"))
				{
					this.cvarRef = true;
					this.refCvarName = _attribute.Value.Substring(1);
				}
				else
				{
					this.duration = StringParsers.ParseFloat(_attribute.Value, 0, -1, NumberStyles.Any);
				}
				return true;
			}
			if (localName == "force")
			{
				this.force = StringParsers.ParseFloat(_attribute.Value, 0, -1, NumberStyles.Any);
				return true;
			}
			if (localName == "massScale")
			{
				this.massScale = StringParsers.ParseFloat(_attribute.Value, 0, -1, NumberStyles.Any);
				return true;
			}
			if (localName == "scaleY")
			{
				this.scaleY = StringParsers.ParseFloat(_attribute.Value, 0, -1, NumberStyles.Any);
				return true;
			}
		}
		return flag;
	}

	// Token: 0x04002B5A RID: 11098
	[PublicizedFrom(EAccessModifier.Private)]
	public float duration = 2.5f;

	// Token: 0x04002B5B RID: 11099
	[PublicizedFrom(EAccessModifier.Private)]
	public bool cvarRef;

	// Token: 0x04002B5C RID: 11100
	[PublicizedFrom(EAccessModifier.Private)]
	public string refCvarName = string.Empty;

	// Token: 0x04002B5D RID: 11101
	[PublicizedFrom(EAccessModifier.Private)]
	public float force;

	// Token: 0x04002B5E RID: 11102
	[PublicizedFrom(EAccessModifier.Private)]
	public float scaleY;

	// Token: 0x04002B5F RID: 11103
	[PublicizedFrom(EAccessModifier.Private)]
	public float massScale;
}

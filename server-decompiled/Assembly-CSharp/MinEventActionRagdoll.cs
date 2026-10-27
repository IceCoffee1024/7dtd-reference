using System;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020006E6 RID: 1766
[Preserve]
public class MinEventActionRagdoll : MinEventActionTargetedBase
{
	// Token: 0x06003697 RID: 13975 RVA: 0x00167C70 File Offset: 0x00165E70
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

	// Token: 0x06003698 RID: 13976 RVA: 0x00167DF4 File Offset: 0x00165FF4
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

	// Token: 0x04002BE3 RID: 11235
	[PublicizedFrom(EAccessModifier.Private)]
	public float duration = 2.5f;

	// Token: 0x04002BE4 RID: 11236
	[PublicizedFrom(EAccessModifier.Private)]
	public bool cvarRef;

	// Token: 0x04002BE5 RID: 11237
	[PublicizedFrom(EAccessModifier.Private)]
	public string refCvarName = string.Empty;

	// Token: 0x04002BE6 RID: 11238
	[PublicizedFrom(EAccessModifier.Private)]
	public float force;

	// Token: 0x04002BE7 RID: 11239
	[PublicizedFrom(EAccessModifier.Private)]
	public float scaleY;

	// Token: 0x04002BE8 RID: 11240
	[PublicizedFrom(EAccessModifier.Private)]
	public float massScale;
}

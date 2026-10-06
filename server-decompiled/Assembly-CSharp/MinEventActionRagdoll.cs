using System;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020006DE RID: 1758
[Preserve]
public class MinEventActionRagdoll : MinEventActionTargetedBase
{
	// Token: 0x06003680 RID: 13952 RVA: 0x00166E88 File Offset: 0x00165088
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

	// Token: 0x06003681 RID: 13953 RVA: 0x0016700C File Offset: 0x0016520C
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

	// Token: 0x04002BC7 RID: 11207
	[PublicizedFrom(EAccessModifier.Private)]
	public float duration = 2.5f;

	// Token: 0x04002BC8 RID: 11208
	[PublicizedFrom(EAccessModifier.Private)]
	public bool cvarRef;

	// Token: 0x04002BC9 RID: 11209
	[PublicizedFrom(EAccessModifier.Private)]
	public string refCvarName = string.Empty;

	// Token: 0x04002BCA RID: 11210
	[PublicizedFrom(EAccessModifier.Private)]
	public float force;

	// Token: 0x04002BCB RID: 11211
	[PublicizedFrom(EAccessModifier.Private)]
	public float scaleY;

	// Token: 0x04002BCC RID: 11212
	[PublicizedFrom(EAccessModifier.Private)]
	public float massScale;
}

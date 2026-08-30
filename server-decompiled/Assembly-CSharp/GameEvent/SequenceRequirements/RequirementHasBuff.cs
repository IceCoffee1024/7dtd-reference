using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x0200192C RID: 6444
	[Preserve]
	public class RequirementHasBuff : BaseRequirement
	{
		// Token: 0x0600C6EF RID: 50927 RVA: 0x00490E71 File Offset: 0x0048F071
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
			this.BuffList = this.BuffName.Split(',', StringSplitOptions.None);
		}

		// Token: 0x0600C6F0 RID: 50928 RVA: 0x00490E88 File Offset: 0x0048F088
		public override bool CanPerform(Entity target)
		{
			EntityAlive entityAlive = target as EntityAlive;
			if (entityAlive != null)
			{
				for (int i = 0; i < this.BuffList.Length; i++)
				{
					if (!this.CheckBuff(entityAlive, this.BuffList[i]))
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x0600C6F1 RID: 50929 RVA: 0x00490EC8 File Offset: 0x0048F0C8
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool CheckBuff(EntityAlive player, string buffName)
		{
			if (player.Buffs.HasBuff(buffName))
			{
				return !this.Invert;
			}
			return this.Invert;
		}

		// Token: 0x0600C6F2 RID: 50930 RVA: 0x00490EE8 File Offset: 0x0048F0E8
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			if (properties.Values.ContainsKey(RequirementHasBuff.PropBuffName))
			{
				this.BuffName = properties.Values[RequirementHasBuff.PropBuffName];
			}
		}

		// Token: 0x0600C6F3 RID: 50931 RVA: 0x00490F19 File Offset: 0x0048F119
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseRequirement CloneChildSettings()
		{
			return new RequirementHasBuff
			{
				BuffName = this.BuffName,
				BuffList = this.BuffList,
				Invert = this.Invert
			};
		}

		// Token: 0x040095E3 RID: 38371
		[PublicizedFrom(EAccessModifier.Protected)]
		public string BuffName = "";

		// Token: 0x040095E4 RID: 38372
		[PublicizedFrom(EAccessModifier.Protected)]
		public string[] BuffList;

		// Token: 0x040095E5 RID: 38373
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropBuffName = "buff_name";
	}
}

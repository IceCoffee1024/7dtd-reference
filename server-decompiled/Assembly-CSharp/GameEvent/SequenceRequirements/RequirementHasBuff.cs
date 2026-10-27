using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x02001989 RID: 6537
	[Preserve]
	public class RequirementHasBuff : BaseRequirement
	{
		// Token: 0x0600C95B RID: 51547 RVA: 0x00498D51 File Offset: 0x00496F51
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
			this.BuffList = this.BuffName.Split(',', StringSplitOptions.None);
		}

		// Token: 0x0600C95C RID: 51548 RVA: 0x00498D68 File Offset: 0x00496F68
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

		// Token: 0x0600C95D RID: 51549 RVA: 0x00498DA8 File Offset: 0x00496FA8
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool CheckBuff(EntityAlive player, string buffName)
		{
			if (player.Buffs.HasBuff(buffName))
			{
				return !this.Invert;
			}
			return this.Invert;
		}

		// Token: 0x0600C95E RID: 51550 RVA: 0x00498DC8 File Offset: 0x00496FC8
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			if (properties.Values.ContainsKey(RequirementHasBuff.PropBuffName))
			{
				this.BuffName = properties.Values[RequirementHasBuff.PropBuffName];
			}
		}

		// Token: 0x0600C95F RID: 51551 RVA: 0x00498DF9 File Offset: 0x00496FF9
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

		// Token: 0x04009792 RID: 38802
		[PublicizedFrom(EAccessModifier.Protected)]
		public string BuffName = "";

		// Token: 0x04009793 RID: 38803
		[PublicizedFrom(EAccessModifier.Protected)]
		public string[] BuffList;

		// Token: 0x04009794 RID: 38804
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropBuffName = "buff_name";
	}
}

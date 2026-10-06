using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x02001958 RID: 6488
	[Preserve]
	public class RequirementHasBuff : BaseRequirement
	{
		// Token: 0x0600C847 RID: 51271 RVA: 0x0049535D File Offset: 0x0049355D
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
			this.BuffList = this.BuffName.Split(',', StringSplitOptions.None);
		}

		// Token: 0x0600C848 RID: 51272 RVA: 0x00495374 File Offset: 0x00493574
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

		// Token: 0x0600C849 RID: 51273 RVA: 0x004953B4 File Offset: 0x004935B4
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool CheckBuff(EntityAlive player, string buffName)
		{
			if (player.Buffs.HasBuff(buffName))
			{
				return !this.Invert;
			}
			return this.Invert;
		}

		// Token: 0x0600C84A RID: 51274 RVA: 0x004953D4 File Offset: 0x004935D4
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			if (properties.Values.ContainsKey(RequirementHasBuff.PropBuffName))
			{
				this.BuffName = properties.Values[RequirementHasBuff.PropBuffName];
			}
		}

		// Token: 0x0600C84B RID: 51275 RVA: 0x00495405 File Offset: 0x00493605
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

		// Token: 0x040096A9 RID: 38569
		[PublicizedFrom(EAccessModifier.Protected)]
		public string BuffName = "";

		// Token: 0x040096AA RID: 38570
		[PublicizedFrom(EAccessModifier.Protected)]
		public string[] BuffList;

		// Token: 0x040096AB RID: 38571
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropBuffName = "buff_name";
	}
}

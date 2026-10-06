using System;
using UnityEngine.Scripting;

namespace Twitch
{
	// Token: 0x02001886 RID: 6278
	[Preserve]
	public class TwitchRequirementHasBuff : BaseTwitchRequirement
	{
		// Token: 0x0600C1ED RID: 49645 RVA: 0x00478CA8 File Offset: 0x00476EA8
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
			this.BuffList = this.BuffName.Split(',', StringSplitOptions.None);
		}

		// Token: 0x0600C1EE RID: 49646 RVA: 0x00478CC0 File Offset: 0x00476EC0
		public override bool CanPerform(Entity entity)
		{
			EntityPlayer entityPlayer = entity as EntityPlayer;
			if (entityPlayer != null)
			{
				for (int i = 0; i < this.BuffList.Length; i++)
				{
					if (!this.CheckBuff(entityPlayer, this.BuffList[i]))
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x0600C1EF RID: 49647 RVA: 0x00478D00 File Offset: 0x00476F00
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool CheckBuff(EntityPlayer player, string buffName)
		{
			if (player.Buffs.HasBuff(buffName))
			{
				return !this.Invert;
			}
			return this.Invert;
		}

		// Token: 0x0600C1F0 RID: 49648 RVA: 0x00478D20 File Offset: 0x00476F20
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(TwitchRequirementHasBuff.PropBuffName, ref this.BuffName);
		}

		// Token: 0x04009203 RID: 37379
		[PublicizedFrom(EAccessModifier.Protected)]
		public string BuffName = "";

		// Token: 0x04009204 RID: 37380
		[PublicizedFrom(EAccessModifier.Protected)]
		public string[] BuffList;

		// Token: 0x04009205 RID: 37381
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropBuffName = "buff_name";
	}
}

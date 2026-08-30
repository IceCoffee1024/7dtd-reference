using System;
using UnityEngine.Scripting;

namespace Twitch
{
	// Token: 0x02001834 RID: 6196
	[Preserve]
	public class TwitchRequirementHasBuff : BaseTwitchRequirement
	{
		// Token: 0x0600BF10 RID: 48912 RVA: 0x0046E9D0 File Offset: 0x0046CBD0
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
			this.BuffList = this.BuffName.Split(',', StringSplitOptions.None);
		}

		// Token: 0x0600BF11 RID: 48913 RVA: 0x0046E9E8 File Offset: 0x0046CBE8
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

		// Token: 0x0600BF12 RID: 48914 RVA: 0x0046EA28 File Offset: 0x0046CC28
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool CheckBuff(EntityPlayer player, string buffName)
		{
			if (player.Buffs.HasBuff(buffName))
			{
				return !this.Invert;
			}
			return this.Invert;
		}

		// Token: 0x0600BF13 RID: 48915 RVA: 0x0046EA48 File Offset: 0x0046CC48
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(TwitchRequirementHasBuff.PropBuffName, ref this.BuffName);
		}

		// Token: 0x04009021 RID: 36897
		[PublicizedFrom(EAccessModifier.Protected)]
		public string BuffName = "";

		// Token: 0x04009022 RID: 36898
		[PublicizedFrom(EAccessModifier.Protected)]
		public string[] BuffList;

		// Token: 0x04009023 RID: 36899
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropBuffName = "buff_name";
	}
}

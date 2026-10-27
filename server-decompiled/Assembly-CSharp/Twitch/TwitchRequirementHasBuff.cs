using System;
using UnityEngine.Scripting;

namespace Twitch
{
	// Token: 0x020018B7 RID: 6327
	[Preserve]
	public class TwitchRequirementHasBuff : BaseTwitchRequirement
	{
		// Token: 0x0600C301 RID: 49921 RVA: 0x0047C654 File Offset: 0x0047A854
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
			this.BuffList = this.BuffName.Split(',', StringSplitOptions.None);
		}

		// Token: 0x0600C302 RID: 49922 RVA: 0x0047C66C File Offset: 0x0047A86C
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

		// Token: 0x0600C303 RID: 49923 RVA: 0x0047C6AC File Offset: 0x0047A8AC
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool CheckBuff(EntityPlayer player, string buffName)
		{
			if (player.Buffs.HasBuff(buffName))
			{
				return !this.Invert;
			}
			return this.Invert;
		}

		// Token: 0x0600C304 RID: 49924 RVA: 0x0047C6CC File Offset: 0x0047A8CC
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(TwitchRequirementHasBuff.PropBuffName, ref this.BuffName);
		}

		// Token: 0x040092EC RID: 37612
		[PublicizedFrom(EAccessModifier.Protected)]
		public string BuffName = "";

		// Token: 0x040092ED RID: 37613
		[PublicizedFrom(EAccessModifier.Protected)]
		public string[] BuffList;

		// Token: 0x040092EE RID: 37614
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropBuffName = "buff_name";
	}
}

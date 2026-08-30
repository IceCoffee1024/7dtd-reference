using System;
using UnityEngine.Scripting;

namespace Twitch
{
	// Token: 0x0200185A RID: 6234
	[Preserve]
	public class TwitchRequirementHasBuff : BaseTwitchRequirement
	{
		// Token: 0x0600C095 RID: 49301 RVA: 0x0047476C File Offset: 0x0047296C
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
			this.BuffList = this.BuffName.Split(',', StringSplitOptions.None);
		}

		// Token: 0x0600C096 RID: 49302 RVA: 0x00474784 File Offset: 0x00472984
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

		// Token: 0x0600C097 RID: 49303 RVA: 0x004747C4 File Offset: 0x004729C4
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool CheckBuff(EntityPlayer player, string buffName)
		{
			if (player.Buffs.HasBuff(buffName))
			{
				return !this.Invert;
			}
			return this.Invert;
		}

		// Token: 0x0600C098 RID: 49304 RVA: 0x004747E4 File Offset: 0x004729E4
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(TwitchRequirementHasBuff.PropBuffName, ref this.BuffName);
		}

		// Token: 0x0400913D RID: 37181
		[PublicizedFrom(EAccessModifier.Protected)]
		public string BuffName = "";

		// Token: 0x0400913E RID: 37182
		[PublicizedFrom(EAccessModifier.Protected)]
		public string[] BuffList;

		// Token: 0x0400913F RID: 37183
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropBuffName = "buff_name";
	}
}

using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001991 RID: 6545
	[Preserve]
	public class ActionModifyProgression : ActionBaseClientAction
	{
		// Token: 0x0600C8AD RID: 51373 RVA: 0x0049B330 File Offset: 0x00499530
		public override void OnClientPerform(Entity target)
		{
			EntityPlayerLocal entityPlayerLocal = target as EntityPlayerLocal;
			if (entityPlayerLocal != null)
			{
				for (int i = 0; i < this.ProgressionNames.Length; i++)
				{
					string text = ((this.Values != null && this.Values.Length > i) ? this.Values[i] : "1");
					ProgressionValue progressionValue = entityPlayerLocal.Progression.GetProgressionValue(this.ProgressionNames[i]);
					int intValue = GameEventManager.GetIntValue(entityPlayerLocal, text, 1);
					ActionModifyProgression.ModifyTypes modifyType = this.ModifyType;
					if (modifyType != ActionModifyProgression.ModifyTypes.Set)
					{
						if (modifyType == ActionModifyProgression.ModifyTypes.Add)
						{
							progressionValue.Level += intValue;
						}
					}
					else
					{
						progressionValue.Level = intValue;
					}
				}
			}
		}

		// Token: 0x0600C8AE RID: 51374 RVA: 0x0049B3CC File Offset: 0x004995CC
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			if (properties.Values.ContainsKey(ActionModifyProgression.PropProgressionNames))
			{
				this.ProgressionNames = properties.Values[ActionModifyProgression.PropProgressionNames].Replace(" ", "").Split(',', StringSplitOptions.None);
				if (properties.Values.ContainsKey(ActionModifyProgression.PropValues))
				{
					this.Values = properties.Values[ActionModifyProgression.PropValues].Replace(" ", "").Split(',', StringSplitOptions.None);
				}
				else
				{
					this.Values = null;
				}
			}
			else
			{
				this.ProgressionNames = null;
				this.Values = null;
			}
			properties.ParseEnum<ActionModifyProgression.ModifyTypes>(ActionModifyProgression.PropModifyType, ref this.ModifyType);
		}

		// Token: 0x0600C8AF RID: 51375 RVA: 0x0049B488 File Offset: 0x00499688
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionModifyProgression
			{
				ModifyType = this.ModifyType,
				ProgressionNames = this.ProgressionNames,
				Values = this.Values,
				targetGroup = this.targetGroup
			};
		}

		// Token: 0x040097FC RID: 38908
		[PublicizedFrom(EAccessModifier.Protected)]
		public ActionModifyProgression.ModifyTypes ModifyType;

		// Token: 0x040097FD RID: 38909
		public string[] ProgressionNames;

		// Token: 0x040097FE RID: 38910
		public string[] Values;

		// Token: 0x040097FF RID: 38911
		public static string PropModifyType = "modify_type";

		// Token: 0x04009800 RID: 38912
		public static string PropProgressionNames = "progression_names";

		// Token: 0x04009801 RID: 38913
		public static string PropValues = "values";

		// Token: 0x02001992 RID: 6546
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum ModifyTypes
		{
			// Token: 0x04009803 RID: 38915
			Set,
			// Token: 0x04009804 RID: 38916
			Add,
			// Token: 0x04009805 RID: 38917
			Remove
		}
	}
}

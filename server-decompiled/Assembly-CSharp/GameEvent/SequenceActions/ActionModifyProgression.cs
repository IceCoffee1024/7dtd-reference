using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019BD RID: 6589
	[Preserve]
	public class ActionModifyProgression : ActionBaseClientAction
	{
		// Token: 0x0600CA05 RID: 51717 RVA: 0x0049F7F4 File Offset: 0x0049D9F4
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

		// Token: 0x0600CA06 RID: 51718 RVA: 0x0049F890 File Offset: 0x0049DA90
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

		// Token: 0x0600CA07 RID: 51719 RVA: 0x0049F94C File Offset: 0x0049DB4C
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

		// Token: 0x040098C2 RID: 39106
		[PublicizedFrom(EAccessModifier.Protected)]
		public ActionModifyProgression.ModifyTypes ModifyType;

		// Token: 0x040098C3 RID: 39107
		public string[] ProgressionNames;

		// Token: 0x040098C4 RID: 39108
		public string[] Values;

		// Token: 0x040098C5 RID: 39109
		public static string PropModifyType = "modify_type";

		// Token: 0x040098C6 RID: 39110
		public static string PropProgressionNames = "progression_names";

		// Token: 0x040098C7 RID: 39111
		public static string PropValues = "values";

		// Token: 0x020019BE RID: 6590
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum ModifyTypes
		{
			// Token: 0x040098C9 RID: 39113
			Set,
			// Token: 0x040098CA RID: 39114
			Add,
			// Token: 0x040098CB RID: 39115
			Remove
		}
	}
}

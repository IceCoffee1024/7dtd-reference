using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x0200196B RID: 6507
	[Preserve]
	public class ActionModifyProgression : ActionBaseClientAction
	{
		// Token: 0x0600C726 RID: 50982 RVA: 0x00494FEC File Offset: 0x004931EC
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

		// Token: 0x0600C727 RID: 50983 RVA: 0x00495088 File Offset: 0x00493288
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

		// Token: 0x0600C728 RID: 50984 RVA: 0x00495144 File Offset: 0x00493344
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

		// Token: 0x040096D3 RID: 38611
		[PublicizedFrom(EAccessModifier.Protected)]
		public ActionModifyProgression.ModifyTypes ModifyType;

		// Token: 0x040096D4 RID: 38612
		public string[] ProgressionNames;

		// Token: 0x040096D5 RID: 38613
		public string[] Values;

		// Token: 0x040096D6 RID: 38614
		public static string PropModifyType = "modify_type";

		// Token: 0x040096D7 RID: 38615
		public static string PropProgressionNames = "progression_names";

		// Token: 0x040096D8 RID: 38616
		public static string PropValues = "values";

		// Token: 0x0200196C RID: 6508
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum ModifyTypes
		{
			// Token: 0x040096DA RID: 38618
			Set,
			// Token: 0x040096DB RID: 38619
			Add,
			// Token: 0x040096DC RID: 38620
			Remove
		}
	}
}

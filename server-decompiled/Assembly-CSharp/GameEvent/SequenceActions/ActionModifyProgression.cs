using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019EE RID: 6638
	[Preserve]
	public class ActionModifyProgression : ActionBaseClientAction
	{
		// Token: 0x0600CB19 RID: 51993 RVA: 0x004A32F4 File Offset: 0x004A14F4
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

		// Token: 0x0600CB1A RID: 51994 RVA: 0x004A3390 File Offset: 0x004A1590
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

		// Token: 0x0600CB1B RID: 51995 RVA: 0x004A344C File Offset: 0x004A164C
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

		// Token: 0x040099AB RID: 39339
		[PublicizedFrom(EAccessModifier.Protected)]
		public ActionModifyProgression.ModifyTypes ModifyType;

		// Token: 0x040099AC RID: 39340
		public string[] ProgressionNames;

		// Token: 0x040099AD RID: 39341
		public string[] Values;

		// Token: 0x040099AE RID: 39342
		public static string PropModifyType = "modify_type";

		// Token: 0x040099AF RID: 39343
		public static string PropProgressionNames = "progression_names";

		// Token: 0x040099B0 RID: 39344
		public static string PropValues = "values";

		// Token: 0x020019EF RID: 6639
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum ModifyTypes
		{
			// Token: 0x040099B2 RID: 39346
			Set,
			// Token: 0x040099B3 RID: 39347
			Add,
			// Token: 0x040099B4 RID: 39348
			Remove
		}
	}
}

using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019C0 RID: 6592
	[Preserve]
	public class ActionSetWeather : BaseAction
	{
		// Token: 0x0600C993 RID: 51603 RVA: 0x0049F884 File Offset: 0x0049DA84
		public override BaseAction.ActionCompleteStates OnPerformAction()
		{
			float floatValue = GameEventManager.GetFloatValue(base.Owner.Target as EntityAlive, this.timeText, 60f);
			WeatherManager.Instance.ForceWeather(this.weatherGroup, floatValue);
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600C994 RID: 51604 RVA: 0x0049F8C4 File Offset: 0x0049DAC4
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionSetWeather.PropTime, ref this.timeText);
			properties.ParseString(ActionSetWeather.PropWeatherGroup, ref this.weatherGroup);
		}

		// Token: 0x0600C995 RID: 51605 RVA: 0x0049F8EF File Offset: 0x0049DAEF
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionSetWeather
			{
				weatherGroup = this.weatherGroup,
				timeText = this.timeText
			};
		}

		// Token: 0x040098E2 RID: 39138
		[PublicizedFrom(EAccessModifier.Protected)]
		public string timeText;

		// Token: 0x040098E3 RID: 39139
		[PublicizedFrom(EAccessModifier.Protected)]
		public string weatherGroup = "default";

		// Token: 0x040098E4 RID: 39140
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTime = "time";

		// Token: 0x040098E5 RID: 39141
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropWeatherGroup = "weather_group";
	}
}

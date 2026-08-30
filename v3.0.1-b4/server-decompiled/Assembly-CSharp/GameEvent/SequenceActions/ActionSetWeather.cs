using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x0200199A RID: 6554
	[Preserve]
	public class ActionSetWeather : BaseAction
	{
		// Token: 0x0600C80C RID: 51212 RVA: 0x0049952C File Offset: 0x0049772C
		public override BaseAction.ActionCompleteStates OnPerformAction()
		{
			float floatValue = GameEventManager.GetFloatValue(base.Owner.Target as EntityAlive, this.timeText, 60f);
			WeatherManager.Instance.ForceWeather(this.weatherGroup, floatValue);
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600C80D RID: 51213 RVA: 0x0049956C File Offset: 0x0049776C
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionSetWeather.PropTime, ref this.timeText);
			properties.ParseString(ActionSetWeather.PropWeatherGroup, ref this.weatherGroup);
		}

		// Token: 0x0600C80E RID: 51214 RVA: 0x00499597 File Offset: 0x00497797
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionSetWeather
			{
				weatherGroup = this.weatherGroup,
				timeText = this.timeText
			};
		}

		// Token: 0x040097B9 RID: 38841
		[PublicizedFrom(EAccessModifier.Protected)]
		public string timeText;

		// Token: 0x040097BA RID: 38842
		[PublicizedFrom(EAccessModifier.Protected)]
		public string weatherGroup = "default";

		// Token: 0x040097BB RID: 38843
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTime = "time";

		// Token: 0x040097BC RID: 38844
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropWeatherGroup = "weather_group";
	}
}

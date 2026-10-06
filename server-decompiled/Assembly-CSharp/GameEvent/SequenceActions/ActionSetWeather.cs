using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019EC RID: 6636
	[Preserve]
	public class ActionSetWeather : BaseAction
	{
		// Token: 0x0600CAEB RID: 51947 RVA: 0x004A3D70 File Offset: 0x004A1F70
		public override BaseAction.ActionCompleteStates OnPerformAction()
		{
			float floatValue = GameEventManager.GetFloatValue(base.Owner.Target as EntityAlive, this.timeText, 60f);
			WeatherManager.Instance.ForceWeather(this.weatherGroup, floatValue);
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600CAEC RID: 51948 RVA: 0x004A3DB0 File Offset: 0x004A1FB0
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionSetWeather.PropTime, ref this.timeText);
			properties.ParseString(ActionSetWeather.PropWeatherGroup, ref this.weatherGroup);
		}

		// Token: 0x0600CAED RID: 51949 RVA: 0x004A3DDB File Offset: 0x004A1FDB
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionSetWeather
			{
				weatherGroup = this.weatherGroup,
				timeText = this.timeText
			};
		}

		// Token: 0x040099A8 RID: 39336
		[PublicizedFrom(EAccessModifier.Protected)]
		public string timeText;

		// Token: 0x040099A9 RID: 39337
		[PublicizedFrom(EAccessModifier.Protected)]
		public string weatherGroup = "default";

		// Token: 0x040099AA RID: 39338
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTime = "time";

		// Token: 0x040099AB RID: 39339
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropWeatherGroup = "weather_group";
	}
}

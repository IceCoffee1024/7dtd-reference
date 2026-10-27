using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A1D RID: 6685
	[Preserve]
	public class ActionSetWeather : BaseAction
	{
		// Token: 0x0600CBFF RID: 52223 RVA: 0x004A7870 File Offset: 0x004A5A70
		public override BaseAction.ActionCompleteStates OnPerformAction()
		{
			float floatValue = GameEventManager.GetFloatValue(base.Owner.Target as EntityAlive, this.timeText, 60f);
			WeatherManager.Instance.ForceWeather(this.weatherGroup, floatValue);
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600CC00 RID: 52224 RVA: 0x004A78B0 File Offset: 0x004A5AB0
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionSetWeather.PropTime, ref this.timeText);
			properties.ParseString(ActionSetWeather.PropWeatherGroup, ref this.weatherGroup);
		}

		// Token: 0x0600CC01 RID: 52225 RVA: 0x004A78DB File Offset: 0x004A5ADB
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionSetWeather
			{
				weatherGroup = this.weatherGroup,
				timeText = this.timeText
			};
		}

		// Token: 0x04009A91 RID: 39569
		[PublicizedFrom(EAccessModifier.Protected)]
		public string timeText;

		// Token: 0x04009A92 RID: 39570
		[PublicizedFrom(EAccessModifier.Protected)]
		public string weatherGroup = "default";

		// Token: 0x04009A93 RID: 39571
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTime = "time";

		// Token: 0x04009A94 RID: 39572
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropWeatherGroup = "weather_group";
	}
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using BhvrAnalyticsServices.Interfaces;
using Services;
using Services.Analytics;
using Services.Analytics.Events;
using UnityEngine.Scripting;

// Token: 0x020006DA RID: 1754
[Preserve]
public class MinEventActionAddBuff : MinEventActionBuffModifierBase
{
	// Token: 0x06003671 RID: 13937 RVA: 0x00167018 File Offset: 0x00165218
	public override void Execute(MinEventParams _params)
	{
		bool flag = !_params.Self.isEntityRemote | _params.IsLocal;
		int num = -1;
		if (_params.Buff != null)
		{
			num = _params.Buff.InstigatorId;
		}
		if (num == -1)
		{
			num = _params.Self.entityId;
		}
		for (int i = 0; i < this.targets.Count; i++)
		{
			string[] array = this.buffNames;
			if (this.buffOneOnly && this.buffWeights != null)
			{
				float randomFloat = this.targets[i].rand.RandomFloat;
				float num2 = 0f;
				for (int j = 0; j < this.buffWeights.Length; j++)
				{
					num2 += this.buffWeights[j];
					if (num2 >= randomFloat)
					{
						array = new string[] { this.buffNames[j] };
						break;
					}
				}
			}
			else if (this.buffWeights != null)
			{
				List<string> list = new List<string>();
				for (int k = 0; k < this.buffWeights.Length; k++)
				{
					float randomFloat2 = this.targets[i].rand.RandomFloat;
					if (this.buffWeights[k] >= randomFloat2)
					{
						list.Add(this.buffNames[k]);
					}
				}
				array = list.ToArray();
			}
			foreach (string text in array)
			{
				BuffClass buff3 = BuffManager.GetBuff(text);
				if (buff3 != null)
				{
					if (this.durationAltered && this.cvarRef)
					{
						if (this.targets[i].Buffs.HasCustomVar(this.refCvarName))
						{
							this.duration = this.targets[i].Buffs.GetCustomVar(this.refCvarName);
						}
						else
						{
							this.duration = buff3.InitialDurationMax;
						}
					}
					if (this.durationAltered)
					{
						this.targets[i].Buffs.AddBuff(text, num, flag, false, this.duration);
					}
					else
					{
						this.targets[i].Buffs.AddBuff(text, num, flag, false, -1f);
						EntityPlayerLocal entityPlayerLocal = _params.Self as EntityPlayerLocal;
						BuffValue buff2 = _params.Buff;
						if (entityPlayerLocal != null && buff2 != null && !Helper.IsEditingOrPlaytesting())
						{
							if (!Helper.IsBuffEventValidOrNew(buff2))
							{
								return;
							}
							BuffClass buffClass = buff2.BuffClass;
							IEnumerable<PlayerStateEventData.ActiveConditionData> enumerable = entityPlayerLocal.Buffs.ActiveBuffs.Select<BuffValue, PlayerStateEventData.ActiveConditionData>((BuffValue buff) => new PlayerStateEventData.ActiveConditionData
							{
								ConditionName = buff.BuffName,
								MaxDuration = buffClass.DurationMax,
								CurrentDuration = buff.DurationInSeconds,
								DamageType = buffClass.DamageType.ToString(),
								StackType = buffClass.StackType.ToString(),
								UpdateRate = buffClass.UpdateRateTicks
							});
							PlayerStateEventData playerStateEventData = new PlayerStateEventData
							{
								ServerId = Helper.GetServerId(),
								SaveId = Helper.GetSaveID(),
								StateTimestamp = DateTime.UtcNow.ToString("O"),
								ElapsedTime = (int)(entityPlayerLocal.totalTimePlayed * 60f),
								TriggerReason = PlayerStateEventData.triggerReasons.ConditionReset.ToString(),
								TriggerSource = buffClass.DamageType.ToString(),
								ConditionsAdded = null,
								ConditionsRemoved = null,
								ActiveConditions = enumerable
							};
							Helper.SetUseDateTimeForBuff(buff2, DateTime.UtcNow);
							ServiceProvider.Instance.Get<IAnalyticsService>().LogEvent(playerStateEventData);
						}
					}
				}
			}
		}
	}

	// Token: 0x06003672 RID: 13938 RVA: 0x00167358 File Offset: 0x00165558
	public override bool ParseXmlAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXmlAttribute(_attribute);
		if (!flag && _attribute.Name.LocalName == "duration")
		{
			if (_attribute.Value.StartsWith("@"))
			{
				this.cvarRef = true;
				this.refCvarName = _attribute.Value.Substring(1);
			}
			else
			{
				this.duration = StringParsers.ParseFloat(_attribute.Value, 0, -1, NumberStyles.Any);
			}
			this.durationAltered = true;
			return true;
		}
		return flag;
	}

	// Token: 0x04002BC5 RID: 11205
	[PublicizedFrom(EAccessModifier.Private)]
	public float duration;

	// Token: 0x04002BC6 RID: 11206
	[PublicizedFrom(EAccessModifier.Private)]
	public bool durationAltered;

	// Token: 0x04002BC7 RID: 11207
	[PublicizedFrom(EAccessModifier.Private)]
	public bool cvarRef;

	// Token: 0x04002BC8 RID: 11208
	[PublicizedFrom(EAccessModifier.Private)]
	public string refCvarName = string.Empty;
}

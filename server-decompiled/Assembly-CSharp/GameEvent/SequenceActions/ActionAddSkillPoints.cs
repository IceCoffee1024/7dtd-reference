using System;
using BhvrAnalyticsServices.Interfaces;
using Services;
using Services.Analytics;
using Services.Analytics.Events;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019BF RID: 6591
	[Preserve]
	public class ActionAddSkillPoints : ActionBaseClientAction
	{
		// Token: 0x0600CA5E RID: 51806 RVA: 0x0049D1F4 File Offset: 0x0049B3F4
		public override void OnClientPerform(Entity target)
		{
			EntityPlayer entityPlayer = target as EntityPlayer;
			if (entityPlayer != null)
			{
				int intValue = GameEventManager.GetIntValue(entityPlayer, this.skillPointsText, 1);
				if (intValue <= 0)
				{
					return;
				}
				int skillPoints = entityPlayer.Progression.SkillPoints;
				entityPlayer.Progression.SkillPoints += intValue;
				if (!Helper.IsEditingOrPlaytesting())
				{
					PlayerSkillPointEventData playerSkillPointEventData = new PlayerSkillPointEventData
					{
						ServerId = Helper.GetServerId(),
						SaveId = Helper.GetSaveId(),
						TransactionTimestamp = DateTime.UtcNow.ToString("O"),
						ElapsedTime = (int)(entityPlayer.totalTimePlayed * 60f),
						PlayerLevel = entityPlayer.Progression.Level,
						InGameDays = GameUtils.WorldTimeToDays(GameManager.Instance.World.worldTime),
						TransactionType = "Earn",
						Amount = entityPlayer.Progression.SkillPoints - skillPoints,
						BalanceBefore = skillPoints,
						BalanceAfter = entityPlayer.Progression.SkillPoints,
						TotalSpent = entityPlayer.Progression.GetAllTimeSkillPoint(),
						SPSource = "challenge_completed",
						SPTarget = null,
						SPTargetDetail = null,
						SPTargerLevel = null
					};
					ServiceProvider.Instance.Get<IAnalyticsService>().LogEvent(playerSkillPointEventData);
				}
			}
		}

		// Token: 0x0600CA5F RID: 51807 RVA: 0x0049D33D File Offset: 0x0049B53D
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionAddSkillPoints.PropSkillPoints, ref this.skillPointsText);
		}

		// Token: 0x0600CA60 RID: 51808 RVA: 0x0049D357 File Offset: 0x0049B557
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionAddSkillPoints
			{
				skillPointsText = this.skillPointsText
			};
		}

		// Token: 0x0400986B RID: 39019
		[PublicizedFrom(EAccessModifier.Protected)]
		public string skillPointsText = "";

		// Token: 0x0400986C RID: 39020
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSkillPoints = "skill_points";
	}
}

using System;
using System.Collections.Generic;
using GameEvent.SequenceActions;
using GameEvent.SequenceRequirements;
using UnityEngine;

// Token: 0x02000531 RID: 1329
public class GameEventActionSequence
{
	// Token: 0x170004AC RID: 1196
	// (get) Token: 0x06002BD0 RID: 11216 RVA: 0x001144DB File Offset: 0x001126DB
	public GameEventVariables EventVariables
	{
		get
		{
			if (this.eventVariables == null)
			{
				this.eventVariables = new GameEventVariables();
			}
			return this.eventVariables;
		}
	}

	// Token: 0x06002BD1 RID: 11217 RVA: 0x001144F8 File Offset: 0x001126F8
	public bool HasTarget()
	{
		if (this.TargetType == GameEventActionSequence.TargetTypes.Entity)
		{
			return this.Target != null && !this.DeadCheck;
		}
		if (this.TargetType == GameEventActionSequence.TargetTypes.POI)
		{
			return this.POIPosition != Vector3i.zero;
		}
		return this.blockValue.type != GameManager.Instance.World.GetBlock(this.POIPosition).type || this.AllowWhileDead;
	}

	// Token: 0x170004AD RID: 1197
	// (get) Token: 0x06002BD2 RID: 11218 RVA: 0x00114573 File Offset: 0x00112773
	public bool DeadCheck
	{
		get
		{
			return !this.Target.IsAlive() && !this.AllowWhileDead;
		}
	}

	// Token: 0x06002BD3 RID: 11219 RVA: 0x00114590 File Offset: 0x00112790
	public void SetupTarget()
	{
		if (this.TargetType == GameEventActionSequence.TargetTypes.POI)
		{
			if (this.POIPosition != Vector3i.zero)
			{
				this.POIInstance = GameManager.Instance.GetDynamicPrefabDecorator().GetPrefabFromWorldPos(this.POIPosition.x, this.POIPosition.z);
				return;
			}
			EntityPlayer entityPlayer = this.Target as EntityPlayer;
			if (entityPlayer != null)
			{
				this.POIInstance = entityPlayer.prefab;
				if (this.POIInstance != null)
				{
					this.POIPosition = this.POIInstance.boundingBoxPosition;
					return;
				}
			}
		}
		else if (this.TargetType == GameEventActionSequence.TargetTypes.Entity)
		{
			EntityPlayer entityPlayer2 = this.Target as EntityPlayer;
			if (entityPlayer2 != null)
			{
				this.POIInstance = entityPlayer2.prefab;
				if (this.POIInstance != null)
				{
					this.POIPosition = this.POIInstance.boundingBoxPosition;
					return;
				}
			}
		}
		else if (this.TargetType == GameEventActionSequence.TargetTypes.Block)
		{
			if (this.POIPosition != Vector3i.zero)
			{
				this.POIInstance = GameManager.Instance.GetDynamicPrefabDecorator().GetPrefabFromWorldPos(this.POIPosition.x, this.POIPosition.z);
				return;
			}
			this.POIInstance = GameManager.Instance.GetDynamicPrefabDecorator().GetPrefabFromWorldPos((int)this.TargetPosition.x, (int)this.TargetPosition.z);
		}
	}

	// Token: 0x06002BD4 RID: 11220 RVA: 0x001146D6 File Offset: 0x001128D6
	public void StartSequence(GameEventManager manager)
	{
		this.StartTime = Time.time;
	}

	// Token: 0x06002BD5 RID: 11221 RVA: 0x001146E4 File Offset: 0x001128E4
	public void Init()
	{
		List<int> list = new List<int>();
		for (int i = 0; i < this.Actions.Count; i++)
		{
			if (!list.Contains(this.Actions[i].Phase))
			{
				list.Add(this.Actions[i].Phase);
			}
			this.Actions[i].SetActionKeyData(i, null, this.Name);
		}
		list.Sort();
		if (list.Count > 0)
		{
			this.PhaseMax = list[list.Count - 1] + 1;
		}
		else
		{
			this.PhaseMax = 0;
		}
		this.IsComplete = false;
	}

	// Token: 0x06002BD6 RID: 11222 RVA: 0x0011478C File Offset: 0x0011298C
	public bool CanPerform(Entity player)
	{
		for (int i = 0; i < this.Requirements.Count; i++)
		{
			if (!this.Requirements[i].CanPerform(player))
			{
				return false;
			}
		}
		for (int j = 0; j < this.Actions.Count; j++)
		{
			if (!this.Actions[j].CanPerform(player))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06002BD7 RID: 11223 RVA: 0x001147F4 File Offset: 0x001129F4
	public void HandleVariablesForProperties(DynamicProperties properties)
	{
		if (properties == null)
		{
			return;
		}
		foreach (KeyValuePair<string, string> keyValuePair in properties.Params1)
		{
			if (this.Variables.ContainsKey(keyValuePair.Value))
			{
				properties.Values[keyValuePair.Key] = this.Variables[keyValuePair.Value];
			}
		}
	}

	// Token: 0x06002BD8 RID: 11224 RVA: 0x0011487C File Offset: 0x00112A7C
	public void ParseProperties(DynamicProperties properties)
	{
		this.Properties = properties;
		if (properties.Values.ContainsKey(GameEventActionSequence.PropAllowUserTrigger))
		{
			this.AllowUserTrigger = StringParsers.ParseBool(properties.Values[GameEventActionSequence.PropAllowUserTrigger], 0, -1, true);
		}
		properties.ParseEnum<GameEventActionSequence.ActionTypes>(GameEventActionSequence.PropActionType, ref this.ActionType);
		if (properties.Values.ContainsKey(GameEventActionSequence.PropAllowWhileDead))
		{
			this.AllowWhileDead = StringParsers.ParseBool(properties.Values[GameEventActionSequence.PropAllowWhileDead], 0, -1, true);
		}
		properties.ParseEnum<GameEventActionSequence.TargetTypes>(GameEventActionSequence.PropTargetType, ref this.TargetType);
		properties.ParseBool(GameEventActionSequence.PropRefundInactivity, ref this.RefundInactivity);
		properties.ParseBool(GameEventActionSequence.PropSingleInstance, ref this.SingleInstance);
		string text = "";
		properties.ParseString(GameEventActionSequence.PropCategory, ref text);
		if (text != "")
		{
			this.CategoryNames = text.Split(',', StringSplitOptions.None);
		}
	}

	// Token: 0x06002BD9 RID: 11225 RVA: 0x00114964 File Offset: 0x00112B64
	public void Update()
	{
		bool flag = false;
		int num = this.CurrentPhase;
		for (int i = 0; i < this.Actions.Count; i++)
		{
			if (this.Actions[i].Phase == this.CurrentPhase && !this.Actions[i].IsComplete)
			{
				flag = true;
				BaseAction.ActionCompleteStates actionCompleteStates;
				if (this.AllowRefunds && this.RefundInactivity && Time.time - this.StartTime > 60f)
				{
					actionCompleteStates = BaseAction.ActionCompleteStates.InCompleteRefund;
				}
				else
				{
					actionCompleteStates = this.Actions[i].PerformAction();
				}
				if (actionCompleteStates == BaseAction.ActionCompleteStates.Complete || (actionCompleteStates == BaseAction.ActionCompleteStates.InCompleteRefund && this.Actions[i].IgnoreRefund))
				{
					this.Actions[i].IsComplete = true;
					if (this.Actions[i].PhaseOnComplete != -1)
					{
						num = this.Actions[i].PhaseOnComplete;
					}
				}
				else if (actionCompleteStates == BaseAction.ActionCompleteStates.RequirementsNotMet)
				{
					this.Actions[i].IsComplete = true;
					if (this.Actions[i].PhaseOnDenied != -1)
					{
						num = this.Actions[i].PhaseOnDenied;
					}
				}
				else if (this.AllowRefunds && actionCompleteStates == BaseAction.ActionCompleteStates.InCompleteRefund)
				{
					if (this.ActionType == GameEventActionSequence.ActionTypes.TwitchAction)
					{
						if (this.Requester is EntityPlayerLocal)
						{
							GameEventManager.Current.HandleTwitchRefundNeeded(this.Name, this.Target.entityId, this.ExtraData, this.Tag);
						}
						else
						{
							SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageGameEventResponse>().Setup(this.Name, this.Target.entityId, this.ExtraData, this.Tag, NetPackageGameEventResponse.ResponseTypes.TwitchRefundNeeded, -1, -1, false, ""), false, this.Requester.entityId, -1, -1, null, 192, false);
						}
						this.IsComplete = true;
					}
					else
					{
						this.Actions[i].IsComplete = true;
					}
				}
			}
		}
		if (!flag)
		{
			this.CurrentPhase++;
		}
		else if (this.CurrentPhase != num)
		{
			this.CurrentPhase = num;
			for (int j = 0; j < this.Actions.Count; j++)
			{
				if (this.Actions[j].Phase >= this.CurrentPhase)
				{
					this.Actions[j].Reset();
				}
			}
		}
		if (this.CurrentPhase >= this.PhaseMax)
		{
			this.IsComplete = true;
			if (this.Requester != null)
			{
				if (this.Requester is EntityPlayerLocal)
				{
					GameEventManager.Current.HandleGameEventCompleted(this.Name, this.Target ? this.Target.entityId : (-1), this.ExtraData, this.Tag);
					return;
				}
				SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageGameEventResponse>().Setup(this.Name, this.Target ? this.Target.entityId : (-1), this.ExtraData, this.Tag, NetPackageGameEventResponse.ResponseTypes.Completed, -1, -1, false, ""), false, this.Requester.entityId, -1, -1, null, 192, false);
			}
		}
	}

	// Token: 0x06002BDA RID: 11226 RVA: 0x00114CAC File Offset: 0x00112EAC
	public void HandleClientPerform(EntityPlayer player, string key)
	{
		BaseAction baseAction = BaseAction.FindKey(key);
		if (baseAction != null)
		{
			baseAction.OnClientPerform(player);
		}
	}

	// Token: 0x06002BDB RID: 11227 RVA: 0x00114CCC File Offset: 0x00112ECC
	public void AddEntitiesToGroup(string groupName, List<Entity> entityList, bool twitchNegative)
	{
		for (int i = entityList.Count - 1; i >= 0; i--)
		{
			EntityPlayer entityPlayer = entityList[i] as EntityPlayer;
			if (entityPlayer != null)
			{
				EntityPlayer.TwitchActionsStates twitchActionsEnabled = entityPlayer.TwitchActionsEnabled;
				if (twitchActionsEnabled != EntityPlayer.TwitchActionsStates.Enabled && (twitchActionsEnabled == EntityPlayer.TwitchActionsStates.Disabled || twitchNegative))
				{
					entityList.RemoveAt(i);
				}
			}
		}
		if (entityList.Count == 0)
		{
			return;
		}
		if (this.EntityGroups == null)
		{
			this.EntityGroups = new Dictionary<string, List<Entity>>();
		}
		if (this.EntityGroups.ContainsKey(groupName))
		{
			this.EntityGroups[groupName] = entityList;
			return;
		}
		this.EntityGroups.Add(groupName, entityList);
	}

	// Token: 0x06002BDC RID: 11228 RVA: 0x00114D5C File Offset: 0x00112F5C
	public void AddEntityToGroup(string groupName, Entity entity)
	{
		if (this.ActionType == GameEventActionSequence.ActionTypes.TwitchAction && entity is EntityPlayer && (entity as EntityPlayer).TwitchActionsEnabled != EntityPlayer.TwitchActionsStates.Enabled)
		{
			return;
		}
		if (this.EntityGroups == null)
		{
			this.EntityGroups = new Dictionary<string, List<Entity>>();
		}
		if (!this.EntityGroups.ContainsKey(groupName))
		{
			this.EntityGroups.Add(groupName, new List<Entity>());
		}
		this.EntityGroups[groupName].Add(entity);
	}

	// Token: 0x06002BDD RID: 11229 RVA: 0x00114DCC File Offset: 0x00112FCC
	public List<Entity> GetEntityGroup(string groupName)
	{
		if (this.EntityGroups == null || !this.EntityGroups.ContainsKey(groupName))
		{
			return null;
		}
		return this.EntityGroups[groupName];
	}

	// Token: 0x06002BDE RID: 11230 RVA: 0x00114DF4 File Offset: 0x00112FF4
	public int GetEntityGroupLiveCount(string groupName)
	{
		if (this.EntityGroups == null || !this.EntityGroups.ContainsKey(groupName))
		{
			return 0;
		}
		int num = 0;
		List<Entity> list = this.EntityGroups[groupName];
		for (int i = 0; i < list.Count; i++)
		{
			EntityAlive entityAlive = list[i] as EntityAlive;
			if (entityAlive != null && entityAlive.IsAlive())
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x06002BDF RID: 11231 RVA: 0x00114E56 File Offset: 0x00113056
	public void ClearEntityGroup(string groupName)
	{
		if (this.EntityGroups == null || !this.EntityGroups.ContainsKey(groupName))
		{
			return;
		}
		this.EntityGroups[groupName].Clear();
	}

	// Token: 0x06002BE0 RID: 11232 RVA: 0x00114E80 File Offset: 0x00113080
	public GameEventActionSequence Clone()
	{
		GameEventActionSequence gameEventActionSequence = new GameEventActionSequence();
		gameEventActionSequence.Name = this.Name;
		gameEventActionSequence.PhaseMax = this.PhaseMax;
		gameEventActionSequence.CurrentPhase = this.CurrentPhase;
		gameEventActionSequence.AllowUserTrigger = this.AllowUserTrigger;
		gameEventActionSequence.AllowWhileDead = this.AllowWhileDead;
		gameEventActionSequence.ActionType = this.ActionType;
		gameEventActionSequence.CrateShare = this.CrateShare;
		gameEventActionSequence.TargetType = this.TargetType;
		gameEventActionSequence.SingleInstance = this.SingleInstance;
		gameEventActionSequence.RefundInactivity = this.RefundInactivity;
		for (int i = 0; i < this.Actions.Count; i++)
		{
			BaseAction baseAction = this.Actions[i].Clone();
			baseAction.Owner = gameEventActionSequence;
			gameEventActionSequence.Actions.Add(baseAction);
		}
		return gameEventActionSequence;
	}

	// Token: 0x06002BE1 RID: 11233 RVA: 0x00114F48 File Offset: 0x00113148
	[PublicizedFrom(EAccessModifier.Internal)]
	public DynamicProperties AssignValuesFrom(GameEventActionSequence oldSeq)
	{
		DynamicProperties dynamicProperties = new DynamicProperties();
		HashSet<string> hashSet = new HashSet<string> { GameEventActionSequence.PropAllowUserTrigger };
		if (oldSeq.Properties != null)
		{
			dynamicProperties.CopyFrom(oldSeq.Properties, hashSet);
		}
		for (int i = 0; i < oldSeq.Requirements.Count; i++)
		{
			BaseRequirement baseRequirement = oldSeq.Requirements[i].Clone();
			baseRequirement.Properties = new DynamicProperties();
			if (oldSeq.Requirements[i].Properties != null)
			{
				baseRequirement.Properties.CopyFrom(oldSeq.Requirements[i].Properties, null);
			}
			baseRequirement.Owner = this;
			baseRequirement.Init();
			this.Requirements.Add(baseRequirement);
		}
		for (int j = 0; j < oldSeq.Actions.Count; j++)
		{
			BaseAction baseAction = oldSeq.Actions[j].HandleAssignFrom(this, oldSeq);
			this.Actions.Add(baseAction);
		}
		return dynamicProperties;
	}

	// Token: 0x06002BE2 RID: 11234 RVA: 0x0011503C File Offset: 0x0011323C
	public void HandleTemplateInit()
	{
		for (int i = 0; i < this.Actions.Count; i++)
		{
			this.Actions[i].HandleTemplateInit(this);
		}
		for (int j = 0; j < this.Requirements.Count; j++)
		{
			this.HandleVariablesForProperties(this.Requirements[j].Properties);
			this.Requirements[j].ParseProperties(this.Requirements[j].Properties);
			this.Requirements[j].Init();
		}
	}

	// Token: 0x06002BE3 RID: 11235 RVA: 0x001150D4 File Offset: 0x001132D4
	public void SetRefundNeeded()
	{
		if (this.ActionType == GameEventActionSequence.ActionTypes.TwitchAction)
		{
			if (this.Requester is EntityPlayerLocal)
			{
				GameEventManager.Current.HandleTwitchRefundNeeded(this.Name, this.Target.entityId, this.ExtraData, this.Tag);
			}
			else
			{
				SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageGameEventResponse>().Setup(this.Name, this.Target.entityId, this.ExtraData, this.Tag, NetPackageGameEventResponse.ResponseTypes.TwitchRefundNeeded, -1, -1, false, ""), false, this.Requester.entityId, -1, -1, null, 192, false);
			}
			this.IsComplete = true;
		}
	}

	// Token: 0x04002158 RID: 8536
	public string Name;

	// Token: 0x04002159 RID: 8537
	public int PhaseMax = 1;

	// Token: 0x0400215A RID: 8538
	public int CurrentPhase;

	// Token: 0x0400215B RID: 8539
	public string ExtraData = "";

	// Token: 0x0400215C RID: 8540
	public string Tag = "";

	// Token: 0x0400215D RID: 8541
	public int ReservedSpawnCount;

	// Token: 0x0400215E RID: 8542
	public GameEventActionSequence.ActionTypes ActionType;

	// Token: 0x0400215F RID: 8543
	public bool AllowUserTrigger = true;

	// Token: 0x04002160 RID: 8544
	public bool AllowWhileDead;

	// Token: 0x04002161 RID: 8545
	public bool RefundInactivity = true;

	// Token: 0x04002162 RID: 8546
	public bool CrateShare;

	// Token: 0x04002163 RID: 8547
	public bool SingleInstance;

	// Token: 0x04002164 RID: 8548
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropAllowUserTrigger = "allow_user_trigger";

	// Token: 0x04002165 RID: 8549
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropActionType = "action_type";

	// Token: 0x04002166 RID: 8550
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropAllowWhileDead = "allow_while_dead";

	// Token: 0x04002167 RID: 8551
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropTargetType = "target_type";

	// Token: 0x04002168 RID: 8552
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropRefundInactivity = "refund_inactivity";

	// Token: 0x04002169 RID: 8553
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropCategory = "category";

	// Token: 0x0400216A RID: 8554
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropSingleInstance = "single_instance";

	// Token: 0x0400216B RID: 8555
	public Dictionary<string, List<Entity>> EntityGroups;

	// Token: 0x0400216C RID: 8556
	public string[] CategoryNames;

	// Token: 0x0400216D RID: 8557
	public List<BaseRequirement> Requirements = new List<BaseRequirement>();

	// Token: 0x0400216E RID: 8558
	public List<BaseAction> Actions = new List<BaseAction>();

	// Token: 0x0400216F RID: 8559
	public EntityPlayer Requester;

	// Token: 0x04002170 RID: 8560
	public Entity Target;

	// Token: 0x04002171 RID: 8561
	public Vector3 TargetPosition;

	// Token: 0x04002172 RID: 8562
	public Vector3i POIPosition;

	// Token: 0x04002173 RID: 8563
	public GameEventActionSequence.TargetTypes TargetType;

	// Token: 0x04002174 RID: 8564
	public PrefabInstance POIInstance;

	// Token: 0x04002175 RID: 8565
	public BlockValue blockValue;

	// Token: 0x04002176 RID: 8566
	public int CurrentBossGroupID = -1;

	// Token: 0x04002177 RID: 8567
	public bool IsComplete;

	// Token: 0x04002178 RID: 8568
	public bool AllowRefunds = true;

	// Token: 0x04002179 RID: 8569
	public bool TwitchActivated;

	// Token: 0x0400217A RID: 8570
	public Dictionary<string, string> Variables = new Dictionary<string, string>();

	// Token: 0x0400217B RID: 8571
	public GameEventVariables eventVariables;

	// Token: 0x0400217C RID: 8572
	public DynamicProperties Properties;

	// Token: 0x0400217D RID: 8573
	public float StartTime = -1f;

	// Token: 0x0400217E RID: 8574
	public bool HasDespawn;

	// Token: 0x0400217F RID: 8575
	public GameEventActionSequence OwnerSequence;

	// Token: 0x02000532 RID: 1330
	public enum ActionTypes
	{
		// Token: 0x04002181 RID: 8577
		TwitchAction,
		// Token: 0x04002182 RID: 8578
		TwitchVote,
		// Token: 0x04002183 RID: 8579
		Game
	}

	// Token: 0x02000533 RID: 1331
	public enum TargetTypes
	{
		// Token: 0x04002185 RID: 8581
		Entity,
		// Token: 0x04002186 RID: 8582
		POI,
		// Token: 0x04002187 RID: 8583
		Block
	}
}

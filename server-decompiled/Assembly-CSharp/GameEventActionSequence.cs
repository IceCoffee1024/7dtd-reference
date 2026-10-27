using System;
using System.Collections.Generic;
using GameEvent.SequenceActions;
using GameEvent.SequenceRequirements;
using UnityEngine;

// Token: 0x02000540 RID: 1344
public class GameEventActionSequence
{
	// Token: 0x170004A6 RID: 1190
	// (get) Token: 0x06002BF8 RID: 11256 RVA: 0x001168AF File Offset: 0x00114AAF
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

	// Token: 0x06002BF9 RID: 11257 RVA: 0x001168CC File Offset: 0x00114ACC
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

	// Token: 0x170004A7 RID: 1191
	// (get) Token: 0x06002BFA RID: 11258 RVA: 0x00116947 File Offset: 0x00114B47
	public bool DeadCheck
	{
		get
		{
			return !this.Target.IsAlive() && !this.AllowWhileDead;
		}
	}

	// Token: 0x06002BFB RID: 11259 RVA: 0x00116964 File Offset: 0x00114B64
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

	// Token: 0x06002BFC RID: 11260 RVA: 0x00116AAA File Offset: 0x00114CAA
	public void StartSequence(GameEventManager manager)
	{
		this.StartTime = Time.time;
	}

	// Token: 0x06002BFD RID: 11261 RVA: 0x00116AB8 File Offset: 0x00114CB8
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

	// Token: 0x06002BFE RID: 11262 RVA: 0x00116B60 File Offset: 0x00114D60
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

	// Token: 0x06002BFF RID: 11263 RVA: 0x00116BC8 File Offset: 0x00114DC8
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

	// Token: 0x06002C00 RID: 11264 RVA: 0x00116C50 File Offset: 0x00114E50
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

	// Token: 0x06002C01 RID: 11265 RVA: 0x00116D38 File Offset: 0x00114F38
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

	// Token: 0x06002C02 RID: 11266 RVA: 0x00117080 File Offset: 0x00115280
	public void HandleClientPerform(EntityPlayer player, string key)
	{
		BaseAction baseAction = BaseAction.FindKey(key);
		if (baseAction != null)
		{
			baseAction.OnClientPerform(player);
		}
	}

	// Token: 0x06002C03 RID: 11267 RVA: 0x001170A0 File Offset: 0x001152A0
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

	// Token: 0x06002C04 RID: 11268 RVA: 0x00117130 File Offset: 0x00115330
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

	// Token: 0x06002C05 RID: 11269 RVA: 0x001171A0 File Offset: 0x001153A0
	public List<Entity> GetEntityGroup(string groupName)
	{
		if (this.EntityGroups == null || !this.EntityGroups.ContainsKey(groupName))
		{
			return null;
		}
		return this.EntityGroups[groupName];
	}

	// Token: 0x06002C06 RID: 11270 RVA: 0x001171C8 File Offset: 0x001153C8
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

	// Token: 0x06002C07 RID: 11271 RVA: 0x0011722A File Offset: 0x0011542A
	public void ClearEntityGroup(string groupName)
	{
		if (this.EntityGroups == null || !this.EntityGroups.ContainsKey(groupName))
		{
			return;
		}
		this.EntityGroups[groupName].Clear();
	}

	// Token: 0x06002C08 RID: 11272 RVA: 0x00117254 File Offset: 0x00115454
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

	// Token: 0x06002C09 RID: 11273 RVA: 0x0011731C File Offset: 0x0011551C
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

	// Token: 0x06002C0A RID: 11274 RVA: 0x00117410 File Offset: 0x00115610
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

	// Token: 0x06002C0B RID: 11275 RVA: 0x001174A8 File Offset: 0x001156A8
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

	// Token: 0x040021A6 RID: 8614
	public string Name;

	// Token: 0x040021A7 RID: 8615
	public int PhaseMax = 1;

	// Token: 0x040021A8 RID: 8616
	public int CurrentPhase;

	// Token: 0x040021A9 RID: 8617
	public string ExtraData = "";

	// Token: 0x040021AA RID: 8618
	public string Tag = "";

	// Token: 0x040021AB RID: 8619
	public int ReservedSpawnCount;

	// Token: 0x040021AC RID: 8620
	public GameEventActionSequence.ActionTypes ActionType;

	// Token: 0x040021AD RID: 8621
	public bool AllowUserTrigger = true;

	// Token: 0x040021AE RID: 8622
	public bool AllowWhileDead;

	// Token: 0x040021AF RID: 8623
	public bool RefundInactivity = true;

	// Token: 0x040021B0 RID: 8624
	public bool CrateShare;

	// Token: 0x040021B1 RID: 8625
	public bool SingleInstance;

	// Token: 0x040021B2 RID: 8626
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropAllowUserTrigger = "allow_user_trigger";

	// Token: 0x040021B3 RID: 8627
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropActionType = "action_type";

	// Token: 0x040021B4 RID: 8628
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropAllowWhileDead = "allow_while_dead";

	// Token: 0x040021B5 RID: 8629
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropTargetType = "target_type";

	// Token: 0x040021B6 RID: 8630
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropRefundInactivity = "refund_inactivity";

	// Token: 0x040021B7 RID: 8631
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropCategory = "category";

	// Token: 0x040021B8 RID: 8632
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropSingleInstance = "single_instance";

	// Token: 0x040021B9 RID: 8633
	public Dictionary<string, List<Entity>> EntityGroups;

	// Token: 0x040021BA RID: 8634
	public string[] CategoryNames;

	// Token: 0x040021BB RID: 8635
	public List<BaseRequirement> Requirements = new List<BaseRequirement>();

	// Token: 0x040021BC RID: 8636
	public List<BaseAction> Actions = new List<BaseAction>();

	// Token: 0x040021BD RID: 8637
	public EntityPlayer Requester;

	// Token: 0x040021BE RID: 8638
	public Entity Target;

	// Token: 0x040021BF RID: 8639
	public Vector3 TargetPosition;

	// Token: 0x040021C0 RID: 8640
	public Vector3i POIPosition;

	// Token: 0x040021C1 RID: 8641
	public GameEventActionSequence.TargetTypes TargetType;

	// Token: 0x040021C2 RID: 8642
	public PrefabInstance POIInstance;

	// Token: 0x040021C3 RID: 8643
	public BlockValue blockValue;

	// Token: 0x040021C4 RID: 8644
	public int CurrentBossGroupID = -1;

	// Token: 0x040021C5 RID: 8645
	public bool IsComplete;

	// Token: 0x040021C6 RID: 8646
	public bool AllowRefunds = true;

	// Token: 0x040021C7 RID: 8647
	public bool TwitchActivated;

	// Token: 0x040021C8 RID: 8648
	public Dictionary<string, string> Variables = new Dictionary<string, string>();

	// Token: 0x040021C9 RID: 8649
	public GameEventVariables eventVariables;

	// Token: 0x040021CA RID: 8650
	public DynamicProperties Properties;

	// Token: 0x040021CB RID: 8651
	public float StartTime = -1f;

	// Token: 0x040021CC RID: 8652
	public bool HasDespawn;

	// Token: 0x040021CD RID: 8653
	public GameEventActionSequence OwnerSequence;

	// Token: 0x02000541 RID: 1345
	public enum ActionTypes
	{
		// Token: 0x040021CF RID: 8655
		TwitchAction,
		// Token: 0x040021D0 RID: 8656
		TwitchVote,
		// Token: 0x040021D1 RID: 8657
		Game
	}

	// Token: 0x02000542 RID: 1346
	public enum TargetTypes
	{
		// Token: 0x040021D3 RID: 8659
		Entity,
		// Token: 0x040021D4 RID: 8660
		POI,
		// Token: 0x040021D5 RID: 8661
		Block
	}
}

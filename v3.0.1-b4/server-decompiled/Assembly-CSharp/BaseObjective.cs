using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Token: 0x0200096F RID: 2415
public abstract class BaseObjective
{
	// Token: 0x14000051 RID: 81
	// (add) Token: 0x06004694 RID: 18068 RVA: 0x001BD464 File Offset: 0x001BB664
	// (remove) Token: 0x06004695 RID: 18069 RVA: 0x001BD49C File Offset: 0x001BB69C
	public event ObjectiveValueChanged ValueChanged;

	// Token: 0x17000769 RID: 1897
	// (get) Token: 0x06004696 RID: 18070 RVA: 0x001BD4D1 File Offset: 0x001BB6D1
	// (set) Token: 0x06004697 RID: 18071 RVA: 0x001BD4D9 File Offset: 0x001BB6D9
	public byte CurrentVersion { get; set; }

	// Token: 0x1700076A RID: 1898
	// (get) Token: 0x06004698 RID: 18072 RVA: 0x001BD4E2 File Offset: 0x001BB6E2
	// (set) Token: 0x06004699 RID: 18073 RVA: 0x001BD4EA File Offset: 0x001BB6EA
	public BaseObjective.ObjectiveStates ObjectiveState { get; set; }

	// Token: 0x1700076B RID: 1899
	// (get) Token: 0x0600469A RID: 18074 RVA: 0x001BD4F3 File Offset: 0x001BB6F3
	// (set) Token: 0x0600469B RID: 18075 RVA: 0x001BD509 File Offset: 0x001BB709
	public bool Complete
	{
		get
		{
			return this.ObjectiveState == BaseObjective.ObjectiveStates.Complete || this.ObjectiveState == BaseObjective.ObjectiveStates.Warning;
		}
		set
		{
			if (value)
			{
				this.ObjectiveState = BaseObjective.ObjectiveStates.Complete;
				this.DisableModifiers();
			}
		}
	}

	// Token: 0x1700076C RID: 1900
	// (get) Token: 0x0600469C RID: 18076 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool useUpdateLoop
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return false;
		}
	}

	// Token: 0x1700076D RID: 1901
	// (get) Token: 0x0600469D RID: 18077 RVA: 0x001BD51B File Offset: 0x001BB71B
	// (set) Token: 0x0600469E RID: 18078 RVA: 0x001BD523 File Offset: 0x001BB723
	public QuestClass OwnerQuestClass { get; set; }

	// Token: 0x1700076E RID: 1902
	// (get) Token: 0x0600469F RID: 18079 RVA: 0x001BD52C File Offset: 0x001BB72C
	// (set) Token: 0x060046A0 RID: 18080 RVA: 0x001BD534 File Offset: 0x001BB734
	public Quest OwnerQuest { get; set; }

	// Token: 0x1700076F RID: 1903
	// (get) Token: 0x060046A1 RID: 18081 RVA: 0x001BD53D File Offset: 0x001BB73D
	// (set) Token: 0x060046A2 RID: 18082 RVA: 0x001BD545 File Offset: 0x001BB745
	public byte Phase { get; set; }

	// Token: 0x060046A3 RID: 18083 RVA: 0x001BD550 File Offset: 0x001BB750
	public BaseObjective()
	{
		this.ObjectiveState = BaseObjective.ObjectiveStates.NotStarted;
		this.Phase = 1;
	}

	// Token: 0x17000770 RID: 1904
	// (get) Token: 0x060046A4 RID: 18084 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual BaseObjective.ObjectiveValueTypes ObjectiveValueType
	{
		get
		{
			return BaseObjective.ObjectiveValueTypes.Boolean;
		}
	}

	// Token: 0x17000771 RID: 1905
	// (get) Token: 0x060046A5 RID: 18085 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public virtual bool PlayObjectiveComplete
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000772 RID: 1906
	// (get) Token: 0x060046A6 RID: 18086 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool RequiresZombies
	{
		get
		{
			return false;
		}
	}

	// Token: 0x060046A7 RID: 18087 RVA: 0x001BD5A0 File Offset: 0x001BB7A0
	[PublicizedFrom(EAccessModifier.Internal)]
	public void ChangeStatus(bool isSuccess)
	{
		this.ObjectiveState = (isSuccess ? BaseObjective.ObjectiveStates.Complete : BaseObjective.ObjectiveStates.Failed);
		if (isSuccess)
		{
			this.OwnerQuest.RallyMarkerActivated = true;
			this.OwnerQuest.RemoveMapObject();
			this.OwnerQuest.Tracked = true;
			this.OwnerQuest.OwnerJournal.TrackedQuest = this.OwnerQuest;
			this.OwnerQuest.OwnerJournal.RefreshTracked();
			this.OwnerQuest.OwnerJournal.ActiveQuest = this.OwnerQuest;
			this.OwnerQuest.RefreshQuestCompletion(QuestClass.CompletionTypes.AutoComplete, null, true, null);
			return;
		}
		this.OwnerQuest.CloseQuest(Quest.QuestState.Failed, null);
	}

	// Token: 0x17000773 RID: 1907
	// (get) Token: 0x060046A8 RID: 18088 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool UpdateUI
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000774 RID: 1908
	// (get) Token: 0x060046A9 RID: 18089 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool NeedsNPCSetPosition
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000775 RID: 1909
	// (get) Token: 0x060046AA RID: 18090 RVA: 0x001BD639 File Offset: 0x001BB839
	// (set) Token: 0x060046AB RID: 18091 RVA: 0x001BD656 File Offset: 0x001BB856
	public string Description
	{
		get
		{
			if (!this.displaySetup)
			{
				this.SetupDisplay();
				this.displaySetup = true;
			}
			return this.description;
		}
		[PublicizedFrom(EAccessModifier.Protected)]
		set
		{
			this.description = value;
		}
	}

	// Token: 0x17000776 RID: 1910
	// (get) Token: 0x060046AC RID: 18092 RVA: 0x001BD65F File Offset: 0x001BB85F
	// (set) Token: 0x060046AD RID: 18093 RVA: 0x001BD67C File Offset: 0x001BB87C
	public virtual string StatusText
	{
		get
		{
			if (!this.displaySetup)
			{
				this.SetupDisplay();
				this.displaySetup = true;
			}
			return this.statusText;
		}
		[PublicizedFrom(EAccessModifier.Protected)]
		set
		{
			this.statusText = value;
		}
	}

	// Token: 0x17000777 RID: 1911
	// (get) Token: 0x060046AE RID: 18094 RVA: 0x001BD685 File Offset: 0x001BB885
	// (set) Token: 0x060046AF RID: 18095 RVA: 0x001BD68D File Offset: 0x001BB88D
	public byte CurrentValue
	{
		get
		{
			return this.currentValue;
		}
		set
		{
			this.currentValue = value;
			this.SetupDisplay();
			if (this.ValueChanged != null)
			{
				this.ValueChanged();
			}
		}
	}

	// Token: 0x17000778 RID: 1912
	// (get) Token: 0x060046B0 RID: 18096 RVA: 0x001BD6AF File Offset: 0x001BB8AF
	// (set) Token: 0x060046B1 RID: 18097 RVA: 0x001BD6B7 File Offset: 0x001BB8B7
	public bool Optional { get; set; }

	// Token: 0x17000779 RID: 1913
	// (get) Token: 0x060046B2 RID: 18098 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool AlwaysComplete
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700077A RID: 1914
	// (get) Token: 0x060046B3 RID: 18099 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public virtual bool ShowInQuestLog
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060046B4 RID: 18100 RVA: 0x001BD6C0 File Offset: 0x001BB8C0
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void CopyValues(BaseObjective objective)
	{
		objective.ID = this.ID;
		objective.Value = this.Value;
		objective.Optional = this.Optional;
		objective.currentValue = this.currentValue;
		objective.Phase = this.Phase;
		objective.NavObjectName = this.NavObjectName;
		objective.HiddenObjective = this.HiddenObjective;
		objective.ForcePhaseFinish = this.ForcePhaseFinish;
		if (this.Modifiers != null)
		{
			for (int i = 0; i < this.Modifiers.Count; i++)
			{
				objective.AddModifier(this.Modifiers[i].Clone());
			}
		}
	}

	// Token: 0x060046B5 RID: 18101 RVA: 0x001BD762 File Offset: 0x001BB962
	public virtual void HandleVariables()
	{
		this.ID = this.OwnerQuest.ParseVariable(this.ID);
		this.Value = this.OwnerQuest.ParseVariable(this.Value);
	}

	// Token: 0x060046B6 RID: 18102 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void SetupQuestTag()
	{
	}

	// Token: 0x060046B7 RID: 18103 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void SetupObjective()
	{
	}

	// Token: 0x060046B8 RID: 18104 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void SetupDisplay()
	{
	}

	// Token: 0x060046B9 RID: 18105 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool SetupPosition(EntityNPC ownerNPC = null, EntityPlayer player = null, List<Vector2> usedPOILocations = null, int entityIDforQuests = -1)
	{
		return false;
	}

	// Token: 0x060046BA RID: 18106 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool SetupActivationList(Vector3 prefabPos, List<Vector3i> activateList)
	{
		return false;
	}

	// Token: 0x060046BB RID: 18107 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void SetPosition(Vector3 position, Vector3 size)
	{
	}

	// Token: 0x060046BC RID: 18108 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void SetPosition(Quest.PositionDataTypes dataType, Vector3i position)
	{
	}

	// Token: 0x060046BD RID: 18109 RVA: 0x001BD794 File Offset: 0x001BB994
	public void HandleAddHooks()
	{
		this.AddHooks();
		if (!this.Complete && this.Modifiers != null)
		{
			for (int i = 0; i < this.Modifiers.Count; i++)
			{
				this.Modifiers[i].OwnerObjective = this;
				this.Modifiers[i].HandleAddHooks();
			}
		}
		if (this.useUpdateLoop)
		{
			QuestEventManager.Current.AddObjectiveToBeUpdated(this);
		}
	}

	// Token: 0x060046BE RID: 18110 RVA: 0x001BD804 File Offset: 0x001BBA04
	public void HandleRemoveHooks()
	{
		this.RemoveHooks();
		this.RemoveNavObject();
		if (this.Modifiers != null)
		{
			for (int i = 0; i < this.Modifiers.Count; i++)
			{
				this.Modifiers[i].HandleRemoveHooks();
			}
		}
		if (this.useUpdateLoop)
		{
			QuestEventManager.Current.RemoveObjectiveToBeUpdated(this);
		}
	}

	// Token: 0x060046BF RID: 18111 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void AddHooks()
	{
	}

	// Token: 0x060046C0 RID: 18112 RVA: 0x001BD85F File Offset: 0x001BBA5F
	public virtual void AddNavObject(Vector3 position)
	{
		if (this.NavObjectName != "")
		{
			this.NavObject = NavObjectManager.Instance.RegisterNavObject(this.NavObjectName, position, "", false, -1, null);
		}
	}

	// Token: 0x060046C1 RID: 18113 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void RemoveHooks()
	{
	}

	// Token: 0x060046C2 RID: 18114 RVA: 0x001BD892 File Offset: 0x001BBA92
	public virtual void RemoveNavObject()
	{
		if (this.NavObject != null)
		{
			NavObjectManager.Instance.UnRegisterNavObject(this.NavObject);
			this.NavObject = null;
		}
	}

	// Token: 0x060046C3 RID: 18115 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void Refresh()
	{
	}

	// Token: 0x060046C4 RID: 18116 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void RemoveObjectives()
	{
	}

	// Token: 0x060046C5 RID: 18117 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void HandleCompleted()
	{
	}

	// Token: 0x060046C6 RID: 18118 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void HandlePhaseCompleted()
	{
	}

	// Token: 0x060046C7 RID: 18119 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void HandleFailed()
	{
	}

	// Token: 0x060046C8 RID: 18120 RVA: 0x001BD8B3 File Offset: 0x001BBAB3
	public virtual void ResetObjective()
	{
		this.CurrentValue = 0;
	}

	// Token: 0x060046C9 RID: 18121 RVA: 0x001BD8BC File Offset: 0x001BBABC
	public virtual void Read(BinaryReader _br)
	{
		this.CurrentVersion = _br.ReadByte();
		this.currentValue = _br.ReadByte();
	}

	// Token: 0x060046CA RID: 18122 RVA: 0x001BD8D6 File Offset: 0x001BBAD6
	public virtual void Write(BinaryWriter _bw)
	{
		_bw.Write(BaseObjective.FileVersion);
		_bw.Write(this.CurrentValue);
	}

	// Token: 0x060046CB RID: 18123 RVA: 0x0001FF1E File Offset: 0x0001E11E
	public virtual BaseObjective Clone()
	{
		return null;
	}

	// Token: 0x060046CC RID: 18124 RVA: 0x001BD8EF File Offset: 0x001BBAEF
	public void HandleUpdate(float deltaTime)
	{
		if (this.Phase == this.OwnerQuest.CurrentPhase)
		{
			this.Update(deltaTime);
		}
	}

	// Token: 0x060046CD RID: 18125 RVA: 0x001BD90C File Offset: 0x001BBB0C
	public virtual void Update(float deltaTime)
	{
		if (Time.time > this.updateTime)
		{
			this.updateTime = Time.time + 1f;
			switch (this.CurrentValue)
			{
			case 0:
				this.UpdateState_NeedSetup();
				return;
			case 1:
				this.UpdateState_WaitingForServer();
				return;
			case 2:
				this.UpdateState_Update();
				return;
			case 3:
				this.UpdateState_Completed();
				QuestEventManager.Current.RemoveObjectiveToBeUpdated(this);
				break;
			default:
				return;
			}
		}
	}

	// Token: 0x060046CE RID: 18126 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void UpdateState_NeedSetup()
	{
	}

	// Token: 0x060046CF RID: 18127 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void UpdateState_WaitingForServer()
	{
	}

	// Token: 0x060046D0 RID: 18128 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void UpdateState_Update()
	{
	}

	// Token: 0x060046D1 RID: 18129 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void UpdateState_Completed()
	{
	}

	// Token: 0x060046D2 RID: 18130 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool SetLocation(Vector3 pos, Vector3 size)
	{
		return false;
	}

	// Token: 0x060046D3 RID: 18131 RVA: 0x00031E8B File Offset: 0x0003008B
	public virtual string ParseBinding(string bindingName)
	{
		return "";
	}

	// Token: 0x060046D4 RID: 18132 RVA: 0x001BD97C File Offset: 0x001BBB7C
	public virtual void ParseProperties(DynamicProperties properties)
	{
		this.Properties = properties;
		this.OwnerQuestClass.HandleVariablesForProperties(properties);
		if (properties.Values.ContainsKey(BaseObjective.PropID))
		{
			this.ID = properties.Values[BaseObjective.PropID];
		}
		if (properties.Values.ContainsKey(BaseObjective.PropValue))
		{
			this.Value = properties.Values[BaseObjective.PropValue];
		}
		if (properties.Values.ContainsKey(BaseObjective.PropPhase))
		{
			this.Phase = Convert.ToByte(properties.Values[BaseObjective.PropPhase]);
			if (this.Phase > this.OwnerQuestClass.HighestPhase)
			{
				this.OwnerQuestClass.HighestPhase = this.Phase;
			}
		}
		if (properties.Values.ContainsKey(BaseObjective.PropOptional))
		{
			bool flag;
			StringParsers.TryParseBool(properties.Values[BaseObjective.PropOptional], out flag);
			this.Optional = flag;
		}
		if (properties.Values.ContainsKey(BaseObjective.PropNavObject))
		{
			this.NavObjectName = properties.Values[BaseObjective.PropNavObject];
		}
		if (properties.Values.ContainsKey(BaseObjective.PropHidden))
		{
			this.HiddenObjective = StringParsers.ParseBool(properties.Values[BaseObjective.PropHidden], 0, -1, true);
		}
		properties.ParseBool(BaseObjective.PropForcePhaseFinish, ref this.ForcePhaseFinish);
	}

	// Token: 0x060046D5 RID: 18133 RVA: 0x001BDAD7 File Offset: 0x001BBCD7
	public void AddModifier(BaseObjectiveModifier modifier)
	{
		if (this.Modifiers == null)
		{
			this.Modifiers = new List<BaseObjectiveModifier>();
		}
		this.Modifiers.Add(modifier);
		modifier.OwnerObjective = this;
	}

	// Token: 0x060046D6 RID: 18134 RVA: 0x001BDB00 File Offset: 0x001BBD00
	[PublicizedFrom(EAccessModifier.Private)]
	public void DisableModifiers()
	{
		if (this.Modifiers != null)
		{
			for (int i = 0; i < this.Modifiers.Count; i++)
			{
				this.Modifiers[i].HandleRemoveHooks();
			}
		}
	}

	// Token: 0x04003885 RID: 14469
	public static byte FileVersion = 0;

	// Token: 0x04003886 RID: 14470
	public static string PropID = "id";

	// Token: 0x04003887 RID: 14471
	public static string PropValue = "value";

	// Token: 0x04003888 RID: 14472
	public static string PropPhase = "phase";

	// Token: 0x04003889 RID: 14473
	public static string PropOptional = "optional";

	// Token: 0x0400388A RID: 14474
	public static string PropNavObject = "nav_object";

	// Token: 0x0400388B RID: 14475
	public static string PropHidden = "hidden";

	// Token: 0x0400388C RID: 14476
	public static string PropForcePhaseFinish = "force_phase_finish";

	// Token: 0x0400388E RID: 14478
	public string ID;

	// Token: 0x04003890 RID: 14480
	public string Value;

	// Token: 0x04003894 RID: 14484
	[PublicizedFrom(EAccessModifier.Private)]
	public bool displaySetup;

	// Token: 0x04003896 RID: 14486
	[PublicizedFrom(EAccessModifier.Protected)]
	public string keyword = "";

	// Token: 0x04003897 RID: 14487
	[PublicizedFrom(EAccessModifier.Private)]
	public string description = "";

	// Token: 0x04003898 RID: 14488
	[PublicizedFrom(EAccessModifier.Private)]
	public string statusText = "";

	// Token: 0x04003899 RID: 14489
	public bool HiddenObjective;

	// Token: 0x0400389A RID: 14490
	public bool ForcePhaseFinish;

	// Token: 0x0400389B RID: 14491
	[PublicizedFrom(EAccessModifier.Protected)]
	public NavObject NavObject;

	// Token: 0x0400389C RID: 14492
	[PublicizedFrom(EAccessModifier.Protected)]
	public string NavObjectName = "";

	// Token: 0x0400389D RID: 14493
	public List<BaseObjectiveModifier> Modifiers;

	// Token: 0x0400389E RID: 14494
	public DynamicProperties Properties;

	// Token: 0x0400389F RID: 14495
	[PublicizedFrom(EAccessModifier.Protected)]
	public byte currentValue;

	// Token: 0x040038A1 RID: 14497
	[PublicizedFrom(EAccessModifier.Private)]
	public float updateTime;

	// Token: 0x02000970 RID: 2416
	public enum ObjectiveStates
	{
		// Token: 0x040038A3 RID: 14499
		NotStarted,
		// Token: 0x040038A4 RID: 14500
		InProgress,
		// Token: 0x040038A5 RID: 14501
		Warning,
		// Token: 0x040038A6 RID: 14502
		Complete,
		// Token: 0x040038A7 RID: 14503
		Failed
	}

	// Token: 0x02000971 RID: 2417
	public enum ObjectiveTypes
	{
		// Token: 0x040038A9 RID: 14505
		AnimalKill,
		// Token: 0x040038AA RID: 14506
		Assemble,
		// Token: 0x040038AB RID: 14507
		BlockPickup,
		// Token: 0x040038AC RID: 14508
		BlockPlace,
		// Token: 0x040038AD RID: 14509
		BlockUpgrade,
		// Token: 0x040038AE RID: 14510
		Buff,
		// Token: 0x040038AF RID: 14511
		ExchangeItemFrom,
		// Token: 0x040038B0 RID: 14512
		Fetch,
		// Token: 0x040038B1 RID: 14513
		FetchKeep,
		// Token: 0x040038B2 RID: 14514
		CraftItem,
		// Token: 0x040038B3 RID: 14515
		Repair,
		// Token: 0x040038B4 RID: 14516
		Scrap,
		// Token: 0x040038B5 RID: 14517
		SkillsPurchased,
		// Token: 0x040038B6 RID: 14518
		Time,
		// Token: 0x040038B7 RID: 14519
		Wear,
		// Token: 0x040038B8 RID: 14520
		WindowOpen,
		// Token: 0x040038B9 RID: 14521
		ZombieKill
	}

	// Token: 0x02000972 RID: 2418
	public enum ObjectiveValueTypes
	{
		// Token: 0x040038BB RID: 14523
		Boolean,
		// Token: 0x040038BC RID: 14524
		Number,
		// Token: 0x040038BD RID: 14525
		Time,
		// Token: 0x040038BE RID: 14526
		Distance
	}

	// Token: 0x02000973 RID: 2419
	[PublicizedFrom(EAccessModifier.Protected)]
	public enum UpdateStates
	{
		// Token: 0x040038C0 RID: 14528
		NeedSetup,
		// Token: 0x040038C1 RID: 14529
		WaitingForServer,
		// Token: 0x040038C2 RID: 14530
		Update,
		// Token: 0x040038C3 RID: 14531
		Completed
	}
}

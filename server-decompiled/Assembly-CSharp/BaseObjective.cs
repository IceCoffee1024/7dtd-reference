using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000986 RID: 2438
public abstract class BaseObjective
{
	// Token: 0x14000052 RID: 82
	// (add) Token: 0x060046BB RID: 18107 RVA: 0x001C0160 File Offset: 0x001BE360
	// (remove) Token: 0x060046BC RID: 18108 RVA: 0x001C0198 File Offset: 0x001BE398
	public event ObjectiveValueChanged ValueChanged;

	// Token: 0x1700079B RID: 1947
	// (get) Token: 0x060046BD RID: 18109 RVA: 0x001C01CD File Offset: 0x001BE3CD
	// (set) Token: 0x060046BE RID: 18110 RVA: 0x001C01D5 File Offset: 0x001BE3D5
	public byte CurrentVersion { get; set; }

	// Token: 0x1700079C RID: 1948
	// (get) Token: 0x060046BF RID: 18111 RVA: 0x001C01DE File Offset: 0x001BE3DE
	// (set) Token: 0x060046C0 RID: 18112 RVA: 0x001C01E6 File Offset: 0x001BE3E6
	public BaseObjective.ObjectiveStates ObjectiveState { get; set; }

	// Token: 0x1700079D RID: 1949
	// (get) Token: 0x060046C1 RID: 18113 RVA: 0x001C01EF File Offset: 0x001BE3EF
	// (set) Token: 0x060046C2 RID: 18114 RVA: 0x001C0205 File Offset: 0x001BE405
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

	// Token: 0x1700079E RID: 1950
	// (get) Token: 0x060046C3 RID: 18115 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool useUpdateLoop
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return false;
		}
	}

	// Token: 0x1700079F RID: 1951
	// (get) Token: 0x060046C4 RID: 18116 RVA: 0x001C0217 File Offset: 0x001BE417
	// (set) Token: 0x060046C5 RID: 18117 RVA: 0x001C021F File Offset: 0x001BE41F
	public QuestClass OwnerQuestClass { get; set; }

	// Token: 0x170007A0 RID: 1952
	// (get) Token: 0x060046C6 RID: 18118 RVA: 0x001C0228 File Offset: 0x001BE428
	// (set) Token: 0x060046C7 RID: 18119 RVA: 0x001C0230 File Offset: 0x001BE430
	public Quest OwnerQuest { get; set; }

	// Token: 0x170007A1 RID: 1953
	// (get) Token: 0x060046C8 RID: 18120 RVA: 0x001C0239 File Offset: 0x001BE439
	// (set) Token: 0x060046C9 RID: 18121 RVA: 0x001C0241 File Offset: 0x001BE441
	public byte Phase { get; set; }

	// Token: 0x060046CA RID: 18122 RVA: 0x001C024C File Offset: 0x001BE44C
	public BaseObjective()
	{
		this.ObjectiveState = BaseObjective.ObjectiveStates.NotStarted;
		this.Phase = 1;
	}

	// Token: 0x170007A2 RID: 1954
	// (get) Token: 0x060046CB RID: 18123 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual BaseObjective.ObjectiveValueTypes ObjectiveValueType
	{
		get
		{
			return BaseObjective.ObjectiveValueTypes.Boolean;
		}
	}

	// Token: 0x170007A3 RID: 1955
	// (get) Token: 0x060046CC RID: 18124 RVA: 0x00020119 File Offset: 0x0001E319
	public virtual bool PlayObjectiveComplete
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170007A4 RID: 1956
	// (get) Token: 0x060046CD RID: 18125 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool RequiresZombies
	{
		get
		{
			return false;
		}
	}

	// Token: 0x060046CE RID: 18126 RVA: 0x001C029C File Offset: 0x001BE49C
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

	// Token: 0x170007A5 RID: 1957
	// (get) Token: 0x060046CF RID: 18127 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool UpdateUI
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170007A6 RID: 1958
	// (get) Token: 0x060046D0 RID: 18128 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool NeedsNPCSetPosition
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170007A7 RID: 1959
	// (get) Token: 0x060046D1 RID: 18129 RVA: 0x001C0335 File Offset: 0x001BE535
	// (set) Token: 0x060046D2 RID: 18130 RVA: 0x001C0352 File Offset: 0x001BE552
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

	// Token: 0x170007A8 RID: 1960
	// (get) Token: 0x060046D3 RID: 18131 RVA: 0x001C035B File Offset: 0x001BE55B
	// (set) Token: 0x060046D4 RID: 18132 RVA: 0x001C0378 File Offset: 0x001BE578
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

	// Token: 0x170007A9 RID: 1961
	// (get) Token: 0x060046D5 RID: 18133 RVA: 0x001C0381 File Offset: 0x001BE581
	// (set) Token: 0x060046D6 RID: 18134 RVA: 0x001C0389 File Offset: 0x001BE589
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

	// Token: 0x170007AA RID: 1962
	// (get) Token: 0x060046D7 RID: 18135 RVA: 0x001C03AB File Offset: 0x001BE5AB
	// (set) Token: 0x060046D8 RID: 18136 RVA: 0x001C03B3 File Offset: 0x001BE5B3
	public bool Optional { get; set; }

	// Token: 0x170007AB RID: 1963
	// (get) Token: 0x060046D9 RID: 18137 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool AlwaysComplete
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170007AC RID: 1964
	// (get) Token: 0x060046DA RID: 18138 RVA: 0x00020119 File Offset: 0x0001E319
	public virtual bool ShowInQuestLog
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060046DB RID: 18139 RVA: 0x001C03BC File Offset: 0x001BE5BC
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

	// Token: 0x060046DC RID: 18140 RVA: 0x001C045E File Offset: 0x001BE65E
	public virtual void HandleVariables()
	{
		this.ID = this.OwnerQuest.ParseVariable(this.ID);
		this.Value = this.OwnerQuest.ParseVariable(this.Value);
	}

	// Token: 0x060046DD RID: 18141 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void SetupQuestTag()
	{
	}

	// Token: 0x060046DE RID: 18142 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void SetupObjective()
	{
	}

	// Token: 0x060046DF RID: 18143 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void SetupDisplay()
	{
	}

	// Token: 0x060046E0 RID: 18144 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool SetupPosition(EntityNPC ownerNPC = null, EntityPlayer player = null, List<Vector2> usedPOILocations = null, int entityIDforQuests = -1)
	{
		return false;
	}

	// Token: 0x060046E1 RID: 18145 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool SetupActivationList(Vector3 prefabPos, List<Vector3i> activateList)
	{
		return false;
	}

	// Token: 0x060046E2 RID: 18146 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void SetPosition(Vector3 position, Vector3 size)
	{
	}

	// Token: 0x060046E3 RID: 18147 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void SetPosition(Quest.PositionDataTypes dataType, Vector3i position)
	{
	}

	// Token: 0x060046E4 RID: 18148 RVA: 0x001C0490 File Offset: 0x001BE690
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

	// Token: 0x060046E5 RID: 18149 RVA: 0x001C0500 File Offset: 0x001BE700
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

	// Token: 0x060046E6 RID: 18150 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void AddHooks()
	{
	}

	// Token: 0x060046E7 RID: 18151 RVA: 0x001C055B File Offset: 0x001BE75B
	public virtual void AddNavObject(Vector3 position)
	{
		if (this.NavObjectName != "")
		{
			this.NavObject = NavObjectManager.Instance.RegisterNavObject(this.NavObjectName, position, "", false, -1, null);
		}
	}

	// Token: 0x060046E8 RID: 18152 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void RemoveHooks()
	{
	}

	// Token: 0x060046E9 RID: 18153 RVA: 0x001C058E File Offset: 0x001BE78E
	public virtual void RemoveNavObject()
	{
		if (this.NavObject != null)
		{
			NavObjectManager.Instance.UnRegisterNavObject(this.NavObject);
			this.NavObject = null;
		}
	}

	// Token: 0x060046EA RID: 18154 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void Refresh()
	{
	}

	// Token: 0x060046EB RID: 18155 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void RemoveObjectives()
	{
	}

	// Token: 0x060046EC RID: 18156 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void HandleCompleted()
	{
	}

	// Token: 0x060046ED RID: 18157 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void HandlePhaseCompleted()
	{
	}

	// Token: 0x060046EE RID: 18158 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void HandleFailed()
	{
	}

	// Token: 0x060046EF RID: 18159 RVA: 0x001C05AF File Offset: 0x001BE7AF
	public virtual void ResetObjective()
	{
		this.CurrentValue = 0;
	}

	// Token: 0x060046F0 RID: 18160 RVA: 0x001C05B8 File Offset: 0x001BE7B8
	public virtual void Read(PooledBinaryReader _br)
	{
		this.CurrentVersion = _br.ReadByte();
		this.currentValue = _br.ReadByte();
	}

	// Token: 0x060046F1 RID: 18161 RVA: 0x001C05D2 File Offset: 0x001BE7D2
	public virtual void Write(PooledBinaryWriter _bw)
	{
		_bw.Write(BaseObjective.FileVersion);
		_bw.Write(this.CurrentValue);
	}

	// Token: 0x060046F2 RID: 18162 RVA: 0x000200DA File Offset: 0x0001E2DA
	public virtual BaseObjective Clone()
	{
		return null;
	}

	// Token: 0x060046F3 RID: 18163 RVA: 0x001C05EB File Offset: 0x001BE7EB
	public void HandleUpdate(float deltaTime)
	{
		if (this.Phase == this.OwnerQuest.CurrentPhase)
		{
			this.Update(deltaTime);
		}
	}

	// Token: 0x060046F4 RID: 18164 RVA: 0x001C0608 File Offset: 0x001BE808
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

	// Token: 0x060046F5 RID: 18165 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void UpdateState_NeedSetup()
	{
	}

	// Token: 0x060046F6 RID: 18166 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void UpdateState_WaitingForServer()
	{
	}

	// Token: 0x060046F7 RID: 18167 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void UpdateState_Update()
	{
	}

	// Token: 0x060046F8 RID: 18168 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void UpdateState_Completed()
	{
	}

	// Token: 0x060046F9 RID: 18169 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool SetLocation(Vector3 pos, Vector3 size)
	{
		return false;
	}

	// Token: 0x060046FA RID: 18170 RVA: 0x0003223F File Offset: 0x0003043F
	public virtual string ParseBinding(string bindingName)
	{
		return "";
	}

	// Token: 0x060046FB RID: 18171 RVA: 0x001C0678 File Offset: 0x001BE878
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

	// Token: 0x060046FC RID: 18172 RVA: 0x001C07D3 File Offset: 0x001BE9D3
	public void AddModifier(BaseObjectiveModifier modifier)
	{
		if (this.Modifiers == null)
		{
			this.Modifiers = new List<BaseObjectiveModifier>();
		}
		this.Modifiers.Add(modifier);
		modifier.OwnerObjective = this;
	}

	// Token: 0x060046FD RID: 18173 RVA: 0x001C07FC File Offset: 0x001BE9FC
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

	// Token: 0x060046FE RID: 18174 RVA: 0x001C0838 File Offset: 0x001BEA38
	public IReadOnlyDictionary<string, object> ToParametersDictionary()
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary.Add("objective_state", this.ObjectiveState.ToString());
		dictionary.Add("objective_optional", this.Optional);
		dictionary.Add("objective_hidden", this.HiddenObjective);
		dictionary.Add("objective_phase", this.Phase);
		this.InternalToParametersDictionary(ref dictionary);
		return dictionary;
	}

	// Token: 0x060046FF RID: 18175
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract void InternalToParametersDictionary(ref Dictionary<string, object> parameters);

	// Token: 0x04003912 RID: 14610
	public static byte FileVersion = 0;

	// Token: 0x04003913 RID: 14611
	public static string PropID = "id";

	// Token: 0x04003914 RID: 14612
	public static string PropValue = "value";

	// Token: 0x04003915 RID: 14613
	public static string PropPhase = "phase";

	// Token: 0x04003916 RID: 14614
	public static string PropOptional = "optional";

	// Token: 0x04003917 RID: 14615
	public static string PropNavObject = "nav_object";

	// Token: 0x04003918 RID: 14616
	public static string PropHidden = "hidden";

	// Token: 0x04003919 RID: 14617
	public static string PropForcePhaseFinish = "force_phase_finish";

	// Token: 0x0400391B RID: 14619
	public string ID;

	// Token: 0x0400391D RID: 14621
	public string Value;

	// Token: 0x04003921 RID: 14625
	[PublicizedFrom(EAccessModifier.Private)]
	public bool displaySetup;

	// Token: 0x04003923 RID: 14627
	[PublicizedFrom(EAccessModifier.Protected)]
	public string keyword = "";

	// Token: 0x04003924 RID: 14628
	[PublicizedFrom(EAccessModifier.Private)]
	public string description = "";

	// Token: 0x04003925 RID: 14629
	[PublicizedFrom(EAccessModifier.Private)]
	public string statusText = "";

	// Token: 0x04003926 RID: 14630
	public bool HiddenObjective;

	// Token: 0x04003927 RID: 14631
	public bool ForcePhaseFinish;

	// Token: 0x04003928 RID: 14632
	[PublicizedFrom(EAccessModifier.Protected)]
	public NavObject NavObject;

	// Token: 0x04003929 RID: 14633
	[PublicizedFrom(EAccessModifier.Protected)]
	public string NavObjectName = "";

	// Token: 0x0400392A RID: 14634
	public List<BaseObjectiveModifier> Modifiers;

	// Token: 0x0400392B RID: 14635
	public DynamicProperties Properties;

	// Token: 0x0400392C RID: 14636
	[PublicizedFrom(EAccessModifier.Protected)]
	public byte currentValue;

	// Token: 0x0400392E RID: 14638
	[PublicizedFrom(EAccessModifier.Private)]
	public float updateTime;

	// Token: 0x02000987 RID: 2439
	public enum ObjectiveStates
	{
		// Token: 0x04003930 RID: 14640
		NotStarted,
		// Token: 0x04003931 RID: 14641
		InProgress,
		// Token: 0x04003932 RID: 14642
		Warning,
		// Token: 0x04003933 RID: 14643
		Complete,
		// Token: 0x04003934 RID: 14644
		Failed
	}

	// Token: 0x02000988 RID: 2440
	public enum ObjectiveTypes
	{
		// Token: 0x04003936 RID: 14646
		AnimalKill,
		// Token: 0x04003937 RID: 14647
		Assemble,
		// Token: 0x04003938 RID: 14648
		BlockPickup,
		// Token: 0x04003939 RID: 14649
		BlockPlace,
		// Token: 0x0400393A RID: 14650
		BlockUpgrade,
		// Token: 0x0400393B RID: 14651
		Buff,
		// Token: 0x0400393C RID: 14652
		ExchangeItemFrom,
		// Token: 0x0400393D RID: 14653
		Fetch,
		// Token: 0x0400393E RID: 14654
		FetchKeep,
		// Token: 0x0400393F RID: 14655
		CraftItem,
		// Token: 0x04003940 RID: 14656
		Repair,
		// Token: 0x04003941 RID: 14657
		Scrap,
		// Token: 0x04003942 RID: 14658
		SkillsPurchased,
		// Token: 0x04003943 RID: 14659
		Time,
		// Token: 0x04003944 RID: 14660
		Wear,
		// Token: 0x04003945 RID: 14661
		WindowOpen,
		// Token: 0x04003946 RID: 14662
		ZombieKill
	}

	// Token: 0x02000989 RID: 2441
	public enum ObjectiveValueTypes
	{
		// Token: 0x04003948 RID: 14664
		Boolean,
		// Token: 0x04003949 RID: 14665
		Number,
		// Token: 0x0400394A RID: 14666
		Time,
		// Token: 0x0400394B RID: 14667
		Distance
	}

	// Token: 0x0200098A RID: 2442
	[PublicizedFrom(EAccessModifier.Protected)]
	public enum UpdateStates
	{
		// Token: 0x0400394D RID: 14669
		NeedSetup,
		// Token: 0x0400394E RID: 14670
		WaitingForServer,
		// Token: 0x0400394F RID: 14671
		Update,
		// Token: 0x04003950 RID: 14672
		Completed
	}
}

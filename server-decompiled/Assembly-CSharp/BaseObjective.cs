using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000990 RID: 2448
public abstract class BaseObjective
{
	// Token: 0x14000052 RID: 82
	// (add) Token: 0x060046DE RID: 18142 RVA: 0x001C11E4 File Offset: 0x001BF3E4
	// (remove) Token: 0x060046DF RID: 18143 RVA: 0x001C121C File Offset: 0x001BF41C
	public event ObjectiveValueChanged ValueChanged;

	// Token: 0x170007A1 RID: 1953
	// (get) Token: 0x060046E0 RID: 18144 RVA: 0x001C1251 File Offset: 0x001BF451
	// (set) Token: 0x060046E1 RID: 18145 RVA: 0x001C1259 File Offset: 0x001BF459
	public byte CurrentVersion { get; set; }

	// Token: 0x170007A2 RID: 1954
	// (get) Token: 0x060046E2 RID: 18146 RVA: 0x001C1262 File Offset: 0x001BF462
	// (set) Token: 0x060046E3 RID: 18147 RVA: 0x001C126A File Offset: 0x001BF46A
	public BaseObjective.ObjectiveStates ObjectiveState { get; set; }

	// Token: 0x170007A3 RID: 1955
	// (get) Token: 0x060046E4 RID: 18148 RVA: 0x001C1273 File Offset: 0x001BF473
	// (set) Token: 0x060046E5 RID: 18149 RVA: 0x001C1289 File Offset: 0x001BF489
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

	// Token: 0x170007A4 RID: 1956
	// (get) Token: 0x060046E6 RID: 18150 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool useUpdateLoop
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return false;
		}
	}

	// Token: 0x170007A5 RID: 1957
	// (get) Token: 0x060046E7 RID: 18151 RVA: 0x001C129B File Offset: 0x001BF49B
	// (set) Token: 0x060046E8 RID: 18152 RVA: 0x001C12A3 File Offset: 0x001BF4A3
	public QuestClass OwnerQuestClass { get; set; }

	// Token: 0x170007A6 RID: 1958
	// (get) Token: 0x060046E9 RID: 18153 RVA: 0x001C12AC File Offset: 0x001BF4AC
	// (set) Token: 0x060046EA RID: 18154 RVA: 0x001C12B4 File Offset: 0x001BF4B4
	public Quest OwnerQuest { get; set; }

	// Token: 0x170007A7 RID: 1959
	// (get) Token: 0x060046EB RID: 18155 RVA: 0x001C12BD File Offset: 0x001BF4BD
	// (set) Token: 0x060046EC RID: 18156 RVA: 0x001C12C5 File Offset: 0x001BF4C5
	public byte Phase { get; set; }

	// Token: 0x060046ED RID: 18157 RVA: 0x001C12D0 File Offset: 0x001BF4D0
	public BaseObjective()
	{
		this.ObjectiveState = BaseObjective.ObjectiveStates.NotStarted;
		this.Phase = 1;
	}

	// Token: 0x170007A8 RID: 1960
	// (get) Token: 0x060046EE RID: 18158 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual BaseObjective.ObjectiveValueTypes ObjectiveValueType
	{
		get
		{
			return BaseObjective.ObjectiveValueTypes.Boolean;
		}
	}

	// Token: 0x170007A9 RID: 1961
	// (get) Token: 0x060046EF RID: 18159 RVA: 0x00020119 File Offset: 0x0001E319
	public virtual bool PlayObjectiveComplete
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170007AA RID: 1962
	// (get) Token: 0x060046F0 RID: 18160 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool RequiresZombies
	{
		get
		{
			return false;
		}
	}

	// Token: 0x060046F1 RID: 18161 RVA: 0x001C1320 File Offset: 0x001BF520
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

	// Token: 0x170007AB RID: 1963
	// (get) Token: 0x060046F2 RID: 18162 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool UpdateUI
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170007AC RID: 1964
	// (get) Token: 0x060046F3 RID: 18163 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool NeedsNPCSetPosition
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170007AD RID: 1965
	// (get) Token: 0x060046F4 RID: 18164 RVA: 0x001C13B9 File Offset: 0x001BF5B9
	// (set) Token: 0x060046F5 RID: 18165 RVA: 0x001C13D6 File Offset: 0x001BF5D6
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

	// Token: 0x170007AE RID: 1966
	// (get) Token: 0x060046F6 RID: 18166 RVA: 0x001C13DF File Offset: 0x001BF5DF
	// (set) Token: 0x060046F7 RID: 18167 RVA: 0x001C13FC File Offset: 0x001BF5FC
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

	// Token: 0x170007AF RID: 1967
	// (get) Token: 0x060046F8 RID: 18168 RVA: 0x001C1405 File Offset: 0x001BF605
	// (set) Token: 0x060046F9 RID: 18169 RVA: 0x001C140D File Offset: 0x001BF60D
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

	// Token: 0x170007B0 RID: 1968
	// (get) Token: 0x060046FA RID: 18170 RVA: 0x001C142F File Offset: 0x001BF62F
	// (set) Token: 0x060046FB RID: 18171 RVA: 0x001C1437 File Offset: 0x001BF637
	public bool Optional { get; set; }

	// Token: 0x170007B1 RID: 1969
	// (get) Token: 0x060046FC RID: 18172 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool AlwaysComplete
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170007B2 RID: 1970
	// (get) Token: 0x060046FD RID: 18173 RVA: 0x00020119 File Offset: 0x0001E319
	public virtual bool ShowInQuestLog
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060046FE RID: 18174 RVA: 0x001C1440 File Offset: 0x001BF640
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

	// Token: 0x060046FF RID: 18175 RVA: 0x001C14E2 File Offset: 0x001BF6E2
	public virtual void HandleVariables()
	{
		this.ID = this.OwnerQuest.ParseVariable(this.ID);
		this.Value = this.OwnerQuest.ParseVariable(this.Value);
	}

	// Token: 0x06004700 RID: 18176 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void SetupQuestTag()
	{
	}

	// Token: 0x06004701 RID: 18177 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void SetupObjective()
	{
	}

	// Token: 0x06004702 RID: 18178 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void SetupDisplay()
	{
	}

	// Token: 0x06004703 RID: 18179 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool SetupPosition(EntityNPC ownerNPC = null, EntityPlayer player = null, List<Vector2> usedPOILocations = null, int entityIDforQuests = -1)
	{
		return false;
	}

	// Token: 0x06004704 RID: 18180 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool SetupActivationList(Vector3 prefabPos, List<Vector3i> activateList)
	{
		return false;
	}

	// Token: 0x06004705 RID: 18181 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void SetPosition(Vector3 position, Vector3 size)
	{
	}

	// Token: 0x06004706 RID: 18182 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void SetPosition(Quest.PositionDataTypes dataType, Vector3i position)
	{
	}

	// Token: 0x06004707 RID: 18183 RVA: 0x001C1514 File Offset: 0x001BF714
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

	// Token: 0x06004708 RID: 18184 RVA: 0x001C1584 File Offset: 0x001BF784
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

	// Token: 0x06004709 RID: 18185 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void AddHooks()
	{
	}

	// Token: 0x0600470A RID: 18186 RVA: 0x001C15DF File Offset: 0x001BF7DF
	public virtual void AddNavObject(Vector3 position)
	{
		if (this.NavObjectName != "")
		{
			this.NavObject = NavObjectManager.Instance.RegisterNavObject(this.NavObjectName, position, "", false, -1, null);
		}
	}

	// Token: 0x0600470B RID: 18187 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void RemoveHooks()
	{
	}

	// Token: 0x0600470C RID: 18188 RVA: 0x001C1612 File Offset: 0x001BF812
	public virtual void RemoveNavObject()
	{
		if (this.NavObject != null)
		{
			NavObjectManager.Instance.UnRegisterNavObject(this.NavObject);
			this.NavObject = null;
		}
	}

	// Token: 0x0600470D RID: 18189 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void Refresh()
	{
	}

	// Token: 0x0600470E RID: 18190 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void RemoveObjectives()
	{
	}

	// Token: 0x0600470F RID: 18191 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void HandleCompleted()
	{
	}

	// Token: 0x06004710 RID: 18192 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void HandlePhaseCompleted()
	{
	}

	// Token: 0x06004711 RID: 18193 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void HandleFailed()
	{
	}

	// Token: 0x06004712 RID: 18194 RVA: 0x001C1633 File Offset: 0x001BF833
	public virtual void ResetObjective()
	{
		this.CurrentValue = 0;
	}

	// Token: 0x06004713 RID: 18195 RVA: 0x001C163C File Offset: 0x001BF83C
	public virtual void Read(PooledBinaryReader _br)
	{
		this.CurrentVersion = _br.ReadByte();
		this.currentValue = _br.ReadByte();
	}

	// Token: 0x06004714 RID: 18196 RVA: 0x001C1656 File Offset: 0x001BF856
	public virtual void Write(PooledBinaryWriter _bw)
	{
		_bw.Write(BaseObjective.FileVersion);
		_bw.Write(this.CurrentValue);
	}

	// Token: 0x06004715 RID: 18197 RVA: 0x000200DA File Offset: 0x0001E2DA
	public virtual BaseObjective Clone()
	{
		return null;
	}

	// Token: 0x06004716 RID: 18198 RVA: 0x001C166F File Offset: 0x001BF86F
	public void HandleUpdate(float deltaTime)
	{
		if (this.Phase == this.OwnerQuest.CurrentPhase)
		{
			this.Update(deltaTime);
		}
	}

	// Token: 0x06004717 RID: 18199 RVA: 0x001C168C File Offset: 0x001BF88C
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

	// Token: 0x06004718 RID: 18200 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void UpdateState_NeedSetup()
	{
	}

	// Token: 0x06004719 RID: 18201 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void UpdateState_WaitingForServer()
	{
	}

	// Token: 0x0600471A RID: 18202 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void UpdateState_Update()
	{
	}

	// Token: 0x0600471B RID: 18203 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void UpdateState_Completed()
	{
	}

	// Token: 0x0600471C RID: 18204 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool SetLocation(Vector3 pos, Vector3 size)
	{
		return false;
	}

	// Token: 0x0600471D RID: 18205 RVA: 0x000321FF File Offset: 0x000303FF
	public virtual string ParseBinding(string bindingName)
	{
		return "";
	}

	// Token: 0x0600471E RID: 18206 RVA: 0x001C16FC File Offset: 0x001BF8FC
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

	// Token: 0x0600471F RID: 18207 RVA: 0x001C1857 File Offset: 0x001BFA57
	public void AddModifier(BaseObjectiveModifier modifier)
	{
		if (this.Modifiers == null)
		{
			this.Modifiers = new List<BaseObjectiveModifier>();
		}
		this.Modifiers.Add(modifier);
		modifier.OwnerObjective = this;
	}

	// Token: 0x06004720 RID: 18208 RVA: 0x001C1880 File Offset: 0x001BFA80
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

	// Token: 0x06004721 RID: 18209 RVA: 0x001C18BC File Offset: 0x001BFABC
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

	// Token: 0x06004722 RID: 18210
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract void InternalToParametersDictionary(ref Dictionary<string, object> parameters);

	// Token: 0x04003932 RID: 14642
	public static byte FileVersion = 0;

	// Token: 0x04003933 RID: 14643
	public static string PropID = "id";

	// Token: 0x04003934 RID: 14644
	public static string PropValue = "value";

	// Token: 0x04003935 RID: 14645
	public static string PropPhase = "phase";

	// Token: 0x04003936 RID: 14646
	public static string PropOptional = "optional";

	// Token: 0x04003937 RID: 14647
	public static string PropNavObject = "nav_object";

	// Token: 0x04003938 RID: 14648
	public static string PropHidden = "hidden";

	// Token: 0x04003939 RID: 14649
	public static string PropForcePhaseFinish = "force_phase_finish";

	// Token: 0x0400393B RID: 14651
	public string ID;

	// Token: 0x0400393D RID: 14653
	public string Value;

	// Token: 0x04003941 RID: 14657
	[PublicizedFrom(EAccessModifier.Private)]
	public bool displaySetup;

	// Token: 0x04003943 RID: 14659
	[PublicizedFrom(EAccessModifier.Protected)]
	public string keyword = "";

	// Token: 0x04003944 RID: 14660
	[PublicizedFrom(EAccessModifier.Private)]
	public string description = "";

	// Token: 0x04003945 RID: 14661
	[PublicizedFrom(EAccessModifier.Private)]
	public string statusText = "";

	// Token: 0x04003946 RID: 14662
	public bool HiddenObjective;

	// Token: 0x04003947 RID: 14663
	public bool ForcePhaseFinish;

	// Token: 0x04003948 RID: 14664
	[PublicizedFrom(EAccessModifier.Protected)]
	public NavObject NavObject;

	// Token: 0x04003949 RID: 14665
	[PublicizedFrom(EAccessModifier.Protected)]
	public string NavObjectName = "";

	// Token: 0x0400394A RID: 14666
	public List<BaseObjectiveModifier> Modifiers;

	// Token: 0x0400394B RID: 14667
	public DynamicProperties Properties;

	// Token: 0x0400394C RID: 14668
	[PublicizedFrom(EAccessModifier.Protected)]
	public byte currentValue;

	// Token: 0x0400394E RID: 14670
	[PublicizedFrom(EAccessModifier.Private)]
	public float updateTime;

	// Token: 0x02000991 RID: 2449
	public enum ObjectiveStates
	{
		// Token: 0x04003950 RID: 14672
		NotStarted,
		// Token: 0x04003951 RID: 14673
		InProgress,
		// Token: 0x04003952 RID: 14674
		Warning,
		// Token: 0x04003953 RID: 14675
		Complete,
		// Token: 0x04003954 RID: 14676
		Failed
	}

	// Token: 0x02000992 RID: 2450
	public enum ObjectiveTypes
	{
		// Token: 0x04003956 RID: 14678
		AnimalKill,
		// Token: 0x04003957 RID: 14679
		Assemble,
		// Token: 0x04003958 RID: 14680
		BlockPickup,
		// Token: 0x04003959 RID: 14681
		BlockPlace,
		// Token: 0x0400395A RID: 14682
		BlockUpgrade,
		// Token: 0x0400395B RID: 14683
		Buff,
		// Token: 0x0400395C RID: 14684
		ExchangeItemFrom,
		// Token: 0x0400395D RID: 14685
		Fetch,
		// Token: 0x0400395E RID: 14686
		FetchKeep,
		// Token: 0x0400395F RID: 14687
		CraftItem,
		// Token: 0x04003960 RID: 14688
		Repair,
		// Token: 0x04003961 RID: 14689
		Scrap,
		// Token: 0x04003962 RID: 14690
		SkillsPurchased,
		// Token: 0x04003963 RID: 14691
		Time,
		// Token: 0x04003964 RID: 14692
		Wear,
		// Token: 0x04003965 RID: 14693
		WindowOpen,
		// Token: 0x04003966 RID: 14694
		ZombieKill
	}

	// Token: 0x02000993 RID: 2451
	public enum ObjectiveValueTypes
	{
		// Token: 0x04003968 RID: 14696
		Boolean,
		// Token: 0x04003969 RID: 14697
		Number,
		// Token: 0x0400396A RID: 14698
		Time,
		// Token: 0x0400396B RID: 14699
		Distance
	}

	// Token: 0x02000994 RID: 2452
	[PublicizedFrom(EAccessModifier.Protected)]
	public enum UpdateStates
	{
		// Token: 0x0400396D RID: 14701
		NeedSetup,
		// Token: 0x0400396E RID: 14702
		WaitingForServer,
		// Token: 0x0400396F RID: 14703
		Update,
		// Token: 0x04003970 RID: 14704
		Completed
	}
}

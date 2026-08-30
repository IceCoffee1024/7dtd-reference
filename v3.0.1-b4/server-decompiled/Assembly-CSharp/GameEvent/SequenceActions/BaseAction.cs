using System;
using System.Collections;
using System.Collections.Generic;
using GameEvent.SequenceRequirements;
using UnityEngine;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019B1 RID: 6577
	public class BaseAction
	{
		// Token: 0x1700183F RID: 6207
		// (get) Token: 0x0600C86E RID: 51310 RVA: 0x0049AF57 File Offset: 0x00499157
		// (set) Token: 0x0600C86F RID: 51311 RVA: 0x0049AF60 File Offset: 0x00499160
		public GameEventActionSequence Owner
		{
			get
			{
				return this.owner;
			}
			set
			{
				this.owner = value;
				if (this.Requirements != null)
				{
					for (int i = 0; i < this.Requirements.Count; i++)
					{
						this.Requirements[i].Owner = value;
					}
				}
			}
		}

		// Token: 0x0600C870 RID: 51312 RVA: 0x0049AFA4 File Offset: 0x004991A4
		public static BaseAction FindKey(string key)
		{
			BaseAction baseAction = null;
			if (!BaseAction.sLookupByKey.TryGetValue(key, out baseAction))
			{
				baseAction = null;
			}
			return baseAction;
		}

		// Token: 0x0600C871 RID: 51313 RVA: 0x0049AFC8 File Offset: 0x004991C8
		public virtual void SetActionKeyData(int _actionIndex, BaseAction _parent, string prefix = "")
		{
			if (_parent != null)
			{
				this.actionKey = _parent.GetActionKey() + ":" + _actionIndex.ToString();
			}
			else
			{
				this.actionKey = prefix + _actionIndex.ToString();
			}
			BaseAction.sLookupByKey[this.actionKey] = this;
		}

		// Token: 0x0600C872 RID: 51314 RVA: 0x0049B01B File Offset: 0x0049921B
		public string GetActionKey()
		{
			return this.actionKey;
		}

		// Token: 0x17001840 RID: 6208
		// (get) Token: 0x0600C873 RID: 51315 RVA: 0x0001FF5D File Offset: 0x0001E15D
		public virtual bool UseRequirements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600C874 RID: 51316 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual void OnInit()
		{
		}

		// Token: 0x0600C875 RID: 51317 RVA: 0x0049B023 File Offset: 0x00499223
		public void Init()
		{
			this.OnInit();
			this.IsComplete = false;
		}

		// Token: 0x0600C876 RID: 51318 RVA: 0x0001FF5D File Offset: 0x0001E15D
		public virtual bool CanPerform(Entity target)
		{
			return true;
		}

		// Token: 0x0600C877 RID: 51319 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void OnClientPerform(Entity target)
		{
		}

		// Token: 0x0600C878 RID: 51320 RVA: 0x0001FF5D File Offset: 0x0001E15D
		public virtual BaseAction.ActionCompleteStates OnPerformAction()
		{
			return BaseAction.ActionCompleteStates.InCompleteRefund;
		}

		// Token: 0x0600C879 RID: 51321 RVA: 0x0049B034 File Offset: 0x00499234
		public BaseAction.ActionCompleteStates PerformAction()
		{
			if (this.UseRequirements && this.Requirements != null)
			{
				for (int i = 0; i < this.Requirements.Count; i++)
				{
					this.Requirements[i].Owner = this.Owner;
					if (!this.Requirements[i].CanPerform(this.Owner.Target))
					{
						return BaseAction.ActionCompleteStates.RequirementsNotMet;
					}
				}
			}
			return this.OnPerformAction();
		}

		// Token: 0x0600C87A RID: 51322 RVA: 0x0049B0A4 File Offset: 0x004992A4
		public void Reset()
		{
			this.IsComplete = false;
			this.OnReset();
		}

		// Token: 0x0600C87B RID: 51323 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual void OnReset()
		{
		}

		// Token: 0x0600C87C RID: 51324 RVA: 0x0049B0B4 File Offset: 0x004992B4
		public virtual void ParseProperties(DynamicProperties properties)
		{
			this.Properties = properties;
			this.Owner.HandleVariablesForProperties(properties);
			properties.ParseInt(BaseAction.PropPhase, ref this.Phase);
			properties.ParseInt(BaseAction.PropPhaseOnComplete, ref this.PhaseOnComplete);
			properties.ParseInt(BaseAction.PropPhaseOnDenied, ref this.PhaseOnDenied);
			properties.ParseBool(BaseAction.PropIgnoreRefund, ref this.IgnoreRefund);
		}

		// Token: 0x0600C87D RID: 51325 RVA: 0x0049B118 File Offset: 0x00499318
		public virtual void HandleTemplateInit(GameEventActionSequence seq)
		{
			seq.HandleVariablesForProperties(this.Properties);
			this.Owner = seq;
			if (this.Properties != null)
			{
				this.ParseProperties(this.Properties);
			}
			this.Init();
			if (this.Requirements != null)
			{
				for (int i = 0; i < this.Requirements.Count; i++)
				{
					seq.HandleVariablesForProperties(this.Requirements[i].Properties);
					if (this.Requirements[i].Properties != null)
					{
						this.Requirements[i].ParseProperties(this.Requirements[i].Properties);
					}
					this.Requirements[i].Init();
				}
			}
		}

		// Token: 0x0600C87E RID: 51326 RVA: 0x0049B1CD File Offset: 0x004993CD
		public void AddRequirement(BaseRequirement req)
		{
			if (this.Requirements == null)
			{
				this.Requirements = new List<BaseRequirement>();
			}
			req.Owner = this.Owner;
			this.Requirements.Add(req);
		}

		// Token: 0x0600C87F RID: 51327 RVA: 0x0049B1FC File Offset: 0x004993FC
		public virtual BaseAction Clone()
		{
			BaseAction baseAction = this.CloneChildSettings();
			if (this.Properties != null)
			{
				baseAction.Properties = new DynamicProperties();
				baseAction.Properties.CopyFrom(this.Properties, null);
			}
			baseAction.Phase = this.Phase;
			baseAction.PhaseOnComplete = this.PhaseOnComplete;
			baseAction.PhaseOnDenied = this.PhaseOnDenied;
			baseAction.IsComplete = false;
			baseAction.actionKey = this.actionKey;
			baseAction.IgnoreRefund = this.IgnoreRefund;
			if (this.Requirements != null)
			{
				for (int i = 0; i < this.Requirements.Count; i++)
				{
					baseAction.AddRequirement(this.Requirements[i].Clone());
				}
			}
			return baseAction;
		}

		// Token: 0x0600C880 RID: 51328 RVA: 0x0001FF1E File Offset: 0x0001E11E
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual BaseAction CloneChildSettings()
		{
			return null;
		}

		// Token: 0x0600C881 RID: 51329 RVA: 0x0049B2AE File Offset: 0x004994AE
		public IEnumerator TeleportEntity(Entity entity, Vector3 position, float teleportDelay)
		{
			yield return new WaitForSeconds(teleportDelay);
			EntityPlayer entityPlayer = entity as EntityPlayer;
			if (entityPlayer != null)
			{
				if (entityPlayer.isEntityRemote)
				{
					SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageCloseAllWindows>().Setup(entityPlayer.entityId), false, entityPlayer.entityId, -1, -1, null, 192, false);
					SingletonMonoBehaviour<ConnectionManager>.Instance.Clients.ForEntityId(entityPlayer.entityId).SendPackage(NetPackageManager.GetPackage<NetPackageTeleportPlayer>().Setup(position, null, false));
				}
				else
				{
					((EntityPlayerLocal)entityPlayer).PlayerUI.windowManager.CloseAllOpenModalWindows(null, false);
					((EntityPlayerLocal)entityPlayer).TeleportToPosition(position, false, null);
				}
			}
			else if (entity.AttachedToEntity != null)
			{
				entity.AttachedToEntity.SetPosition(position, true);
			}
			else
			{
				entity.SetPosition(position, true);
			}
			yield break;
		}

		// Token: 0x0600C882 RID: 51330 RVA: 0x000149AE File Offset: 0x00012BAE
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual string ParseTextElement(string element)
		{
			return element;
		}

		// Token: 0x0600C883 RID: 51331 RVA: 0x0049B2CC File Offset: 0x004994CC
		[PublicizedFrom(EAccessModifier.Protected)]
		public string GetTextWithElements(string text)
		{
			int num = text.IndexOf("{", StringComparison.Ordinal);
			Dictionary<string, string> dictionary = null;
			while (num != -1)
			{
				int num2 = text.IndexOf("}", num, StringComparison.Ordinal);
				if (num2 == -1)
				{
					break;
				}
				string text2 = text.Substring(num + 1, num2 - num - 1);
				string text3 = this.ParseTextElement(text2);
				if (text3 != text2)
				{
					if (dictionary == null)
					{
						dictionary = new Dictionary<string, string>();
					}
					dictionary.Add(text.Substring(num, num2 - num + 1), text3);
				}
				num = text.IndexOf("{", num2, StringComparison.Ordinal);
			}
			if (dictionary != null)
			{
				foreach (string text4 in dictionary.Keys)
				{
					text = text.Replace(text4, dictionary[text4]);
				}
			}
			return text;
		}

		// Token: 0x0600C884 RID: 51332 RVA: 0x0049B3A4 File Offset: 0x004995A4
		public virtual BaseAction HandleAssignFrom(GameEventActionSequence newSeq, GameEventActionSequence oldSeq)
		{
			BaseAction baseAction = this.Clone();
			baseAction.Properties = new DynamicProperties();
			if (this.Properties != null)
			{
				baseAction.Properties.CopyFrom(this.Properties, null);
			}
			baseAction.Owner = newSeq;
			if (baseAction.Requirements != null)
			{
				for (int i = 0; i < baseAction.Requirements.Count; i++)
				{
					baseAction.Requirements[i].Properties = new DynamicProperties();
					if (this.Requirements[i].Properties != null)
					{
						baseAction.Requirements[i].Properties.CopyFrom(this.Requirements[i].Properties, null);
					}
					baseAction.Requirements[i].Owner = newSeq;
					baseAction.Requirements[i].Init();
				}
			}
			return baseAction;
		}

		// Token: 0x04009819 RID: 38937
		[PublicizedFrom(EAccessModifier.Protected)]
		public static Dictionary<string, BaseAction> sLookupByKey = new Dictionary<string, BaseAction>();

		// Token: 0x0400981A RID: 38938
		[PublicizedFrom(EAccessModifier.Protected)]
		public GameEventActionSequence owner;

		// Token: 0x0400981B RID: 38939
		public int Phase;

		// Token: 0x0400981C RID: 38940
		public int PhaseOnComplete = -1;

		// Token: 0x0400981D RID: 38941
		public int PhaseOnDenied = -1;

		// Token: 0x0400981E RID: 38942
		[PublicizedFrom(EAccessModifier.Protected)]
		public string actionKey = "";

		// Token: 0x0400981F RID: 38943
		public bool IgnoreRefund;

		// Token: 0x04009820 RID: 38944
		public bool IsComplete;

		// Token: 0x04009821 RID: 38945
		public DynamicProperties Properties;

		// Token: 0x04009822 RID: 38946
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropPhase = "phase";

		// Token: 0x04009823 RID: 38947
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropPhaseOnComplete = "phase_on_complete";

		// Token: 0x04009824 RID: 38948
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropPhaseOnDenied = "phase_on_denied";

		// Token: 0x04009825 RID: 38949
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropIgnoreRefund = "ignore_refund";

		// Token: 0x04009826 RID: 38950
		public List<BaseRequirement> Requirements;

		// Token: 0x020019B2 RID: 6578
		public enum ActionCompleteStates
		{
			// Token: 0x04009828 RID: 38952
			InComplete,
			// Token: 0x04009829 RID: 38953
			InCompleteRefund,
			// Token: 0x0400982A RID: 38954
			RequirementsNotMet,
			// Token: 0x0400982B RID: 38955
			Complete
		}
	}
}

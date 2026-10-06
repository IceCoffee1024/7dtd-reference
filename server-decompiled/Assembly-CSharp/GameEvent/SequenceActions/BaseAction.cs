using System;
using System.Collections;
using System.Collections.Generic;
using GameEvent.SequenceRequirements;
using UnityEngine;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A03 RID: 6659
	public class BaseAction
	{
		// Token: 0x17001953 RID: 6483
		// (get) Token: 0x0600CB4D RID: 52045 RVA: 0x004A5753 File Offset: 0x004A3953
		// (set) Token: 0x0600CB4E RID: 52046 RVA: 0x004A575C File Offset: 0x004A395C
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

		// Token: 0x0600CB4F RID: 52047 RVA: 0x004A57A0 File Offset: 0x004A39A0
		public static BaseAction FindKey(string key)
		{
			BaseAction baseAction = null;
			if (!BaseAction.sLookupByKey.TryGetValue(key, out baseAction))
			{
				baseAction = null;
			}
			return baseAction;
		}

		// Token: 0x0600CB50 RID: 52048 RVA: 0x004A57C4 File Offset: 0x004A39C4
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

		// Token: 0x0600CB51 RID: 52049 RVA: 0x004A5817 File Offset: 0x004A3A17
		public string GetActionKey()
		{
			return this.actionKey;
		}

		// Token: 0x17001954 RID: 6484
		// (get) Token: 0x0600CB52 RID: 52050 RVA: 0x00020119 File Offset: 0x0001E319
		public virtual bool UseRequirements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600CB53 RID: 52051 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual void OnInit()
		{
		}

		// Token: 0x0600CB54 RID: 52052 RVA: 0x004A581F File Offset: 0x004A3A1F
		public void Init()
		{
			this.OnInit();
			this.IsComplete = false;
		}

		// Token: 0x0600CB55 RID: 52053 RVA: 0x00020119 File Offset: 0x0001E319
		public virtual bool CanPerform(Entity target)
		{
			return true;
		}

		// Token: 0x0600CB56 RID: 52054 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void OnClientPerform(Entity target)
		{
		}

		// Token: 0x0600CB57 RID: 52055 RVA: 0x00020119 File Offset: 0x0001E319
		public virtual BaseAction.ActionCompleteStates OnPerformAction()
		{
			return BaseAction.ActionCompleteStates.InCompleteRefund;
		}

		// Token: 0x0600CB58 RID: 52056 RVA: 0x004A5830 File Offset: 0x004A3A30
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

		// Token: 0x0600CB59 RID: 52057 RVA: 0x004A58A0 File Offset: 0x004A3AA0
		public void Reset()
		{
			this.IsComplete = false;
			this.OnReset();
		}

		// Token: 0x0600CB5A RID: 52058 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual void OnReset()
		{
		}

		// Token: 0x0600CB5B RID: 52059 RVA: 0x004A58B0 File Offset: 0x004A3AB0
		public virtual void ParseProperties(DynamicProperties properties)
		{
			this.Properties = properties;
			this.Owner.HandleVariablesForProperties(properties);
			properties.ParseInt(BaseAction.PropPhase, ref this.Phase);
			properties.ParseInt(BaseAction.PropPhaseOnComplete, ref this.PhaseOnComplete);
			properties.ParseInt(BaseAction.PropPhaseOnDenied, ref this.PhaseOnDenied);
			properties.ParseBool(BaseAction.PropIgnoreRefund, ref this.IgnoreRefund);
		}

		// Token: 0x0600CB5C RID: 52060 RVA: 0x004A5914 File Offset: 0x004A3B14
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

		// Token: 0x0600CB5D RID: 52061 RVA: 0x004A59C9 File Offset: 0x004A3BC9
		public void AddRequirement(BaseRequirement req)
		{
			if (this.Requirements == null)
			{
				this.Requirements = new List<BaseRequirement>();
			}
			req.Owner = this.Owner;
			this.Requirements.Add(req);
		}

		// Token: 0x0600CB5E RID: 52062 RVA: 0x004A59F8 File Offset: 0x004A3BF8
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

		// Token: 0x0600CB5F RID: 52063 RVA: 0x000200DA File Offset: 0x0001E2DA
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual BaseAction CloneChildSettings()
		{
			return null;
		}

		// Token: 0x0600CB60 RID: 52064 RVA: 0x004A5AAA File Offset: 0x004A3CAA
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

		// Token: 0x0600CB61 RID: 52065 RVA: 0x000149AE File Offset: 0x00012BAE
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual string ParseTextElement(string element)
		{
			return element;
		}

		// Token: 0x0600CB62 RID: 52066 RVA: 0x004A5AC8 File Offset: 0x004A3CC8
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

		// Token: 0x0600CB63 RID: 52067 RVA: 0x004A5BA0 File Offset: 0x004A3DA0
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

		// Token: 0x04009A08 RID: 39432
		[PublicizedFrom(EAccessModifier.Protected)]
		public static Dictionary<string, BaseAction> sLookupByKey = new Dictionary<string, BaseAction>();

		// Token: 0x04009A09 RID: 39433
		[PublicizedFrom(EAccessModifier.Protected)]
		public GameEventActionSequence owner;

		// Token: 0x04009A0A RID: 39434
		public int Phase;

		// Token: 0x04009A0B RID: 39435
		public int PhaseOnComplete = -1;

		// Token: 0x04009A0C RID: 39436
		public int PhaseOnDenied = -1;

		// Token: 0x04009A0D RID: 39437
		[PublicizedFrom(EAccessModifier.Protected)]
		public string actionKey = "";

		// Token: 0x04009A0E RID: 39438
		public bool IgnoreRefund;

		// Token: 0x04009A0F RID: 39439
		public bool IsComplete;

		// Token: 0x04009A10 RID: 39440
		public DynamicProperties Properties;

		// Token: 0x04009A11 RID: 39441
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropPhase = "phase";

		// Token: 0x04009A12 RID: 39442
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropPhaseOnComplete = "phase_on_complete";

		// Token: 0x04009A13 RID: 39443
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropPhaseOnDenied = "phase_on_denied";

		// Token: 0x04009A14 RID: 39444
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropIgnoreRefund = "ignore_refund";

		// Token: 0x04009A15 RID: 39445
		public List<BaseRequirement> Requirements;

		// Token: 0x02001A04 RID: 6660
		public enum ActionCompleteStates
		{
			// Token: 0x04009A17 RID: 39447
			InComplete,
			// Token: 0x04009A18 RID: 39448
			InCompleteRefund,
			// Token: 0x04009A19 RID: 39449
			RequirementsNotMet,
			// Token: 0x04009A1A RID: 39450
			Complete
		}
	}
}

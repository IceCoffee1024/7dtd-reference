using System;
using System.Collections;
using System.Collections.Generic;
using GameEvent.SequenceRequirements;
using UnityEngine;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019D7 RID: 6615
	public class BaseAction
	{
		// Token: 0x170018B1 RID: 6321
		// (get) Token: 0x0600C9F5 RID: 51701 RVA: 0x004A12AF File Offset: 0x0049F4AF
		// (set) Token: 0x0600C9F6 RID: 51702 RVA: 0x004A12B8 File Offset: 0x0049F4B8
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

		// Token: 0x0600C9F7 RID: 51703 RVA: 0x004A12FC File Offset: 0x0049F4FC
		public static BaseAction FindKey(string key)
		{
			BaseAction baseAction = null;
			if (!BaseAction.sLookupByKey.TryGetValue(key, out baseAction))
			{
				baseAction = null;
			}
			return baseAction;
		}

		// Token: 0x0600C9F8 RID: 51704 RVA: 0x004A1320 File Offset: 0x0049F520
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

		// Token: 0x0600C9F9 RID: 51705 RVA: 0x004A1373 File Offset: 0x0049F573
		public string GetActionKey()
		{
			return this.actionKey;
		}

		// Token: 0x170018B2 RID: 6322
		// (get) Token: 0x0600C9FA RID: 51706 RVA: 0x00020021 File Offset: 0x0001E221
		public virtual bool UseRequirements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600C9FB RID: 51707 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual void OnInit()
		{
		}

		// Token: 0x0600C9FC RID: 51708 RVA: 0x004A137B File Offset: 0x0049F57B
		public void Init()
		{
			this.OnInit();
			this.IsComplete = false;
		}

		// Token: 0x0600C9FD RID: 51709 RVA: 0x00020021 File Offset: 0x0001E221
		public virtual bool CanPerform(Entity target)
		{
			return true;
		}

		// Token: 0x0600C9FE RID: 51710 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void OnClientPerform(Entity target)
		{
		}

		// Token: 0x0600C9FF RID: 51711 RVA: 0x00020021 File Offset: 0x0001E221
		public virtual BaseAction.ActionCompleteStates OnPerformAction()
		{
			return BaseAction.ActionCompleteStates.InCompleteRefund;
		}

		// Token: 0x0600CA00 RID: 51712 RVA: 0x004A138C File Offset: 0x0049F58C
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

		// Token: 0x0600CA01 RID: 51713 RVA: 0x004A13FC File Offset: 0x0049F5FC
		public void Reset()
		{
			this.IsComplete = false;
			this.OnReset();
		}

		// Token: 0x0600CA02 RID: 51714 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual void OnReset()
		{
		}

		// Token: 0x0600CA03 RID: 51715 RVA: 0x004A140C File Offset: 0x0049F60C
		public virtual void ParseProperties(DynamicProperties properties)
		{
			this.Properties = properties;
			this.Owner.HandleVariablesForProperties(properties);
			properties.ParseInt(BaseAction.PropPhase, ref this.Phase);
			properties.ParseInt(BaseAction.PropPhaseOnComplete, ref this.PhaseOnComplete);
			properties.ParseInt(BaseAction.PropPhaseOnDenied, ref this.PhaseOnDenied);
			properties.ParseBool(BaseAction.PropIgnoreRefund, ref this.IgnoreRefund);
		}

		// Token: 0x0600CA04 RID: 51716 RVA: 0x004A1470 File Offset: 0x0049F670
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

		// Token: 0x0600CA05 RID: 51717 RVA: 0x004A1525 File Offset: 0x0049F725
		public void AddRequirement(BaseRequirement req)
		{
			if (this.Requirements == null)
			{
				this.Requirements = new List<BaseRequirement>();
			}
			req.Owner = this.Owner;
			this.Requirements.Add(req);
		}

		// Token: 0x0600CA06 RID: 51718 RVA: 0x004A1554 File Offset: 0x0049F754
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

		// Token: 0x0600CA07 RID: 51719 RVA: 0x0001FFE2 File Offset: 0x0001E1E2
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual BaseAction CloneChildSettings()
		{
			return null;
		}

		// Token: 0x0600CA08 RID: 51720 RVA: 0x004A1606 File Offset: 0x0049F806
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

		// Token: 0x0600CA09 RID: 51721 RVA: 0x000149AE File Offset: 0x00012BAE
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual string ParseTextElement(string element)
		{
			return element;
		}

		// Token: 0x0600CA0A RID: 51722 RVA: 0x004A1624 File Offset: 0x0049F824
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

		// Token: 0x0600CA0B RID: 51723 RVA: 0x004A16FC File Offset: 0x0049F8FC
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

		// Token: 0x04009942 RID: 39234
		[PublicizedFrom(EAccessModifier.Protected)]
		public static Dictionary<string, BaseAction> sLookupByKey = new Dictionary<string, BaseAction>();

		// Token: 0x04009943 RID: 39235
		[PublicizedFrom(EAccessModifier.Protected)]
		public GameEventActionSequence owner;

		// Token: 0x04009944 RID: 39236
		public int Phase;

		// Token: 0x04009945 RID: 39237
		public int PhaseOnComplete = -1;

		// Token: 0x04009946 RID: 39238
		public int PhaseOnDenied = -1;

		// Token: 0x04009947 RID: 39239
		[PublicizedFrom(EAccessModifier.Protected)]
		public string actionKey = "";

		// Token: 0x04009948 RID: 39240
		public bool IgnoreRefund;

		// Token: 0x04009949 RID: 39241
		public bool IsComplete;

		// Token: 0x0400994A RID: 39242
		public DynamicProperties Properties;

		// Token: 0x0400994B RID: 39243
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropPhase = "phase";

		// Token: 0x0400994C RID: 39244
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropPhaseOnComplete = "phase_on_complete";

		// Token: 0x0400994D RID: 39245
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropPhaseOnDenied = "phase_on_denied";

		// Token: 0x0400994E RID: 39246
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropIgnoreRefund = "ignore_refund";

		// Token: 0x0400994F RID: 39247
		public List<BaseRequirement> Requirements;

		// Token: 0x020019D8 RID: 6616
		public enum ActionCompleteStates
		{
			// Token: 0x04009951 RID: 39249
			InComplete,
			// Token: 0x04009952 RID: 39250
			InCompleteRefund,
			// Token: 0x04009953 RID: 39251
			RequirementsNotMet,
			// Token: 0x04009954 RID: 39252
			Complete
		}
	}
}

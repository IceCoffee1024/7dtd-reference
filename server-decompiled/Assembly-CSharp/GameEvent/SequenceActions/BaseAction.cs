using System;
using System.Collections;
using System.Collections.Generic;
using GameEvent.SequenceRequirements;
using UnityEngine;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A34 RID: 6708
	public class BaseAction
	{
		// Token: 0x1700199B RID: 6555
		// (get) Token: 0x0600CC61 RID: 52321 RVA: 0x004A924F File Offset: 0x004A744F
		// (set) Token: 0x0600CC62 RID: 52322 RVA: 0x004A9258 File Offset: 0x004A7458
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

		// Token: 0x0600CC63 RID: 52323 RVA: 0x004A929C File Offset: 0x004A749C
		public static BaseAction FindKey(string key)
		{
			BaseAction baseAction = null;
			if (!BaseAction.sLookupByKey.TryGetValue(key, out baseAction))
			{
				baseAction = null;
			}
			return baseAction;
		}

		// Token: 0x0600CC64 RID: 52324 RVA: 0x004A92C0 File Offset: 0x004A74C0
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

		// Token: 0x0600CC65 RID: 52325 RVA: 0x004A9313 File Offset: 0x004A7513
		public string GetActionKey()
		{
			return this.actionKey;
		}

		// Token: 0x1700199C RID: 6556
		// (get) Token: 0x0600CC66 RID: 52326 RVA: 0x00020119 File Offset: 0x0001E319
		public virtual bool UseRequirements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600CC67 RID: 52327 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual void OnInit()
		{
		}

		// Token: 0x0600CC68 RID: 52328 RVA: 0x004A931B File Offset: 0x004A751B
		public void Init()
		{
			this.OnInit();
			this.IsComplete = false;
		}

		// Token: 0x0600CC69 RID: 52329 RVA: 0x00020119 File Offset: 0x0001E319
		public virtual bool CanPerform(Entity target)
		{
			return true;
		}

		// Token: 0x0600CC6A RID: 52330 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void OnClientPerform(Entity target)
		{
		}

		// Token: 0x0600CC6B RID: 52331 RVA: 0x00020119 File Offset: 0x0001E319
		public virtual BaseAction.ActionCompleteStates OnPerformAction()
		{
			return BaseAction.ActionCompleteStates.InCompleteRefund;
		}

		// Token: 0x0600CC6C RID: 52332 RVA: 0x004A932C File Offset: 0x004A752C
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

		// Token: 0x0600CC6D RID: 52333 RVA: 0x004A939C File Offset: 0x004A759C
		public void Reset()
		{
			this.IsComplete = false;
			this.OnReset();
		}

		// Token: 0x0600CC6E RID: 52334 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual void OnReset()
		{
		}

		// Token: 0x0600CC6F RID: 52335 RVA: 0x004A93AC File Offset: 0x004A75AC
		public virtual void ParseProperties(DynamicProperties properties)
		{
			this.Properties = properties;
			this.Owner.HandleVariablesForProperties(properties);
			properties.ParseInt(BaseAction.PropPhase, ref this.Phase);
			properties.ParseInt(BaseAction.PropPhaseOnComplete, ref this.PhaseOnComplete);
			properties.ParseInt(BaseAction.PropPhaseOnDenied, ref this.PhaseOnDenied);
			properties.ParseBool(BaseAction.PropIgnoreRefund, ref this.IgnoreRefund);
		}

		// Token: 0x0600CC70 RID: 52336 RVA: 0x004A9410 File Offset: 0x004A7610
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

		// Token: 0x0600CC71 RID: 52337 RVA: 0x004A94C5 File Offset: 0x004A76C5
		public void AddRequirement(BaseRequirement req)
		{
			if (this.Requirements == null)
			{
				this.Requirements = new List<BaseRequirement>();
			}
			req.Owner = this.Owner;
			this.Requirements.Add(req);
		}

		// Token: 0x0600CC72 RID: 52338 RVA: 0x004A94F4 File Offset: 0x004A76F4
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

		// Token: 0x0600CC73 RID: 52339 RVA: 0x000200DA File Offset: 0x0001E2DA
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual BaseAction CloneChildSettings()
		{
			return null;
		}

		// Token: 0x0600CC74 RID: 52340 RVA: 0x004A95A6 File Offset: 0x004A77A6
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

		// Token: 0x0600CC75 RID: 52341 RVA: 0x000149AE File Offset: 0x00012BAE
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual string ParseTextElement(string element)
		{
			return element;
		}

		// Token: 0x0600CC76 RID: 52342 RVA: 0x004A95C4 File Offset: 0x004A77C4
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

		// Token: 0x0600CC77 RID: 52343 RVA: 0x004A969C File Offset: 0x004A789C
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

		// Token: 0x04009AF1 RID: 39665
		[PublicizedFrom(EAccessModifier.Protected)]
		public static Dictionary<string, BaseAction> sLookupByKey = new Dictionary<string, BaseAction>();

		// Token: 0x04009AF2 RID: 39666
		[PublicizedFrom(EAccessModifier.Protected)]
		public GameEventActionSequence owner;

		// Token: 0x04009AF3 RID: 39667
		public int Phase;

		// Token: 0x04009AF4 RID: 39668
		public int PhaseOnComplete = -1;

		// Token: 0x04009AF5 RID: 39669
		public int PhaseOnDenied = -1;

		// Token: 0x04009AF6 RID: 39670
		[PublicizedFrom(EAccessModifier.Protected)]
		public string actionKey = "";

		// Token: 0x04009AF7 RID: 39671
		public bool IgnoreRefund;

		// Token: 0x04009AF8 RID: 39672
		public bool IsComplete;

		// Token: 0x04009AF9 RID: 39673
		public DynamicProperties Properties;

		// Token: 0x04009AFA RID: 39674
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropPhase = "phase";

		// Token: 0x04009AFB RID: 39675
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropPhaseOnComplete = "phase_on_complete";

		// Token: 0x04009AFC RID: 39676
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropPhaseOnDenied = "phase_on_denied";

		// Token: 0x04009AFD RID: 39677
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropIgnoreRefund = "ignore_refund";

		// Token: 0x04009AFE RID: 39678
		public List<BaseRequirement> Requirements;

		// Token: 0x02001A35 RID: 6709
		public enum ActionCompleteStates
		{
			// Token: 0x04009B00 RID: 39680
			InComplete,
			// Token: 0x04009B01 RID: 39681
			InCompleteRefund,
			// Token: 0x04009B02 RID: 39682
			RequirementsNotMet,
			// Token: 0x04009B03 RID: 39683
			Complete
		}
	}
}

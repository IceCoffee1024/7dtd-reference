using System;
using Platform;
using UnityEngine;

// Token: 0x0200120F RID: 4623
public class GUIHUDEntityName : MonoBehaviour
{
	// Token: 0x0600937F RID: 37759 RVA: 0x0037CCBC File Offset: 0x0037AEBC
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Start()
	{
		if (GameManager.IsDedicatedServer)
		{
			global::UnityEngine.Object.Destroy(this);
			return;
		}
		this.entity = base.GetComponent<EntityAlive>();
		this.entityPlayer = this.entity as EntityPlayer;
		if (GUIHUDEntityName.gameManager == null)
		{
			GUIHUDEntityName.gameManager = global::UnityEngine.Object.FindAnyObjectByType<GameManager>();
		}
		if (NGuiHUDRoot.go == null)
		{
			global::UnityEngine.Object.Destroy(this);
			return;
		}
		GameObject gameObject = Resources.Load("Prefabs/prefabPlayerHUDText", typeof(GameObject)) as GameObject;
		if (gameObject != null)
		{
			GameObject gameObject2 = NGuiHUDRoot.go.AddChild(gameObject);
			gameObject2.name = ((this.entityPlayer != null) ? this.entityPlayer.PlayerDisplayName : this.entity.EntityName);
			this.hudText = gameObject2.GetComponentInChildren<NGuiHUDText>();
			this.hudTextObj = this.hudText.gameObject;
			if (this.hudText.ambigiousFont == null)
			{
				Log.Error("GUIHUDEntityName font null");
			}
			this.followTarget = gameObject2.AddComponent<NGuiUIFollowTarget>();
			this.followTarget.offset = new Vector3(0f, 0.6f, 0f);
			this.followTarget.target = null;
			this.hudText.Add(string.Empty, Color.white, float.MaxValue);
			this.hudText.Add(string.Empty, Color.white, float.MaxValue);
			this.hudText.Add(string.Empty, Color.white, float.MaxValue);
			this.hudTextObj.SetActive(false);
			this.updatePhysicsVisibilityCounter = 9999;
		}
	}

	// Token: 0x06009380 RID: 37760 RVA: 0x0037CE4D File Offset: 0x0037B04D
	[PublicizedFrom(EAccessModifier.Protected)]
	public void OnDestroy()
	{
		if (this.hudText != null)
		{
			global::UnityEngine.Object.Destroy(this.hudTextObj);
			this.hudText = null;
			this.hudTextObj = null;
		}
	}

	// Token: 0x06009381 RID: 37761 RVA: 0x0037CE78 File Offset: 0x0037B078
	[PublicizedFrom(EAccessModifier.Private)]
	public void findRenderers()
	{
		if (this.entity.emodel)
		{
			Transform modelTransform = this.entity.emodel.GetModelTransform();
			if (modelTransform)
			{
				this.renderers = modelTransform.GetComponentsInChildren<Renderer>();
			}
		}
	}

	// Token: 0x06009382 RID: 37762 RVA: 0x0037CEBC File Offset: 0x0037B0BC
	[PublicizedFrom(EAccessModifier.Protected)]
	public void OnGUI()
	{
		if (GameManager.IsDedicatedServer)
		{
			return;
		}
		if (GUIHUDEntityName.mainCamera == null)
		{
			GUIHUDEntityName.mainCamera = Camera.main;
			if (GUIHUDEntityName.mainCamera == null)
			{
				return;
			}
		}
		Vector3 vector = this.entity.getHeadPosition() - Origin.position - GUIHUDEntityName.mainCamera.transform.position;
		float magnitude = vector.magnitude;
		bool flag = this.entityPlayer != null;
		if (magnitude > 8f && flag)
		{
			this.setActiveIfDifferent(false);
			return;
		}
		int num;
		if (this.renderers == null || this.renderers.Length == 0)
		{
			num = this.updatePhysicsVisibilityCounter + 1;
			this.updatePhysicsVisibilityCounter = num;
			if (num > 100)
			{
				this.updatePhysicsVisibilityCounter = 0;
				this.findRenderers();
			}
			return;
		}
		bool flag2 = false;
		for (int i = 0; i < this.renderers.Length; i++)
		{
			Renderer renderer = this.renderers[i];
			if (!renderer)
			{
				this.renderers = null;
				return;
			}
			if (renderer.isVisible)
			{
				flag2 = true;
				break;
			}
		}
		if (!flag2)
		{
			this.setActiveIfDifferent(false);
			return;
		}
		if (this.followTarget.target == null)
		{
			this.followTarget.target = this.entity.ModelTransform;
			this.followTarget.offset = new Vector3(0f, this.entity.GetEyeHeight() + 0.6f, 0f);
		}
		num = this.updatePhysicsVisibilityCounter + 1;
		this.updatePhysicsVisibilityCounter = num;
		if (num > 5)
		{
			this.updatePhysicsVisibilityCounter = 0;
			EntityPlayerLocal primaryPlayer = GUIHUDEntityName.gameManager.World.GetPrimaryPlayer();
			if (primaryPlayer == null || !primaryPlayer.Spawned)
			{
				this.bShowHUDText = false;
				this.setActiveIfDifferent(false);
				return;
			}
			if (!primaryPlayer.PlayerUI.windowManager.IsHUDEnabled())
			{
				this.bShowHUDText = false;
				this.setActiveIfDifferent(false);
				return;
			}
			RaycastHit raycastHit;
			this.bShowHUDText = Physics.Raycast(new Ray(GUIHUDEntityName.mainCamera.transform.position + vector.normalized * 0.15f, vector), out raycastHit, 9.6f, -538480653);
			this.bShowHUDText = this.bShowHUDText && raycastHit.distance < 8f;
			Transform transform = raycastHit.transform;
			if (this.bShowHUDText && transform.tag.StartsWith("E_BP_"))
			{
				transform = GameUtils.GetHitRootTransform(transform.tag, transform);
			}
			this.bShowHUDText &= transform == this.entity.transform;
			if (!flag)
			{
				this.bShowHUDText = true;
			}
			if (!this.bShowHUDText && this.bLastShowHUDText && this.hideCountdownTime <= 0f)
			{
				this.hideCountdownTime = 0.4f;
			}
			int num2 = 45;
			string text = ((this.entityPlayer != null) ? this.entityPlayer.PlayerDisplayName : this.entity.EntityName);
			if (this.entity.DebugNameInfo.Length > 0 && !this.entity.IsDead())
			{
				text += this.entity.DebugNameInfo;
				num2 = (int)(150f / Utils.FastClamp(magnitude, 3.333f, 8f));
			}
			string text2 = string.Empty;
			PersistentPlayerData persistentPlayerData;
			if (GameManager.Instance.persistentPlayers.EntityToPlayerMap.TryGetValue(this.entity.entityId, out persistentPlayerData))
			{
				GameServerInfo gameServerInfo = (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer ? SingletonMonoBehaviour<ConnectionManager>.Instance.LocalServerInfo : SingletonMonoBehaviour<ConnectionManager>.Instance.LastGameServerInfo);
				if (gameServerInfo != null && gameServerInfo.AllowsCrossplay)
				{
					EPlayGroup playGroup = GameManager.Instance.persistentPlayers.Players[persistentPlayerData.PrimaryId].PlayGroup;
					text2 = PlatformManager.NativePlatform.Utils.GetCrossplayPlayerIcon(playGroup, false, persistentPlayerData.PlatformData.NativeId.PlatformIdentifier);
				}
			}
			INGUIAtlas atlasByName = primaryPlayer.PlayerUI.xui.GetAtlasByName("SymbolAtlas", text2);
			this.hudText.SetEntry(0, text2, true, atlasByName);
			this.hudText.SetEntrySize(0, num2 - 5);
			this.hudText.SetEntryOffset(0, new Vector3(-0.1f, 0f, 0f));
			this.hudText.SetEntry(1, text, false, null);
			this.hudText.SetEntrySize(1, num2);
			IPartyVoice.EVoiceMemberState playerVoiceState = VoiceHelpers.GetPlayerVoiceState(this.entityPlayer, false);
			if (playerVoiceState != IPartyVoice.EVoiceMemberState.Disabled)
			{
				INGUIAtlas atlasByName2 = primaryPlayer.PlayerUI.xui.GetAtlasByName("UIAtlas", "ui_game_symbol_talk");
				this.hudText.SetEntry(2, "ui_game_symbol_talk", true, atlasByName2);
				this.hudText.SetEntrySize(2, num2 - 5);
				this.hudText.SetEntryOffset(2, new Vector3(0.1f, 0f, 0f));
				this.hudText.SetEntryColor(2, (playerVoiceState == IPartyVoice.EVoiceMemberState.Muted) ? Color.red : ((playerVoiceState == IPartyVoice.EVoiceMemberState.VoiceActive) ? Color.white : Color.grey));
			}
			else
			{
				this.hudText.SetEntry(2, string.Empty, true, null);
				this.hudText.SetEntrySize(2, num2 - 5);
				this.hudText.SetEntryOffset(2, new Vector3(0.1f, 0f, 0f));
			}
		}
		if (this.hideCountdownTime > 0f)
		{
			this.hideCountdownTime -= Time.deltaTime;
		}
		if (this.hideCountdownTime <= 0f)
		{
			this.setActiveIfDifferent(this.bShowHUDText);
			this.bLastShowHUDText = this.bShowHUDText;
			return;
		}
		if (this.bShowHUDText)
		{
			this.setActiveIfDifferent(this.bShowHUDText);
		}
	}

	// Token: 0x06009383 RID: 37763 RVA: 0x0037D44D File Offset: 0x0037B64D
	[PublicizedFrom(EAccessModifier.Private)]
	public void setActiveIfDifferent(bool _active)
	{
		if (this.hudTextObj.activeSelf != _active)
		{
			this.hudTextObj.SetActive(_active);
		}
	}

	// Token: 0x04006E69 RID: 28265
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const int cRaycastFrameDelay = 5;

	// Token: 0x04006E6A RID: 28266
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const int cVisibleDistance = 8;

	// Token: 0x04006E6B RID: 28267
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cHeadOffset = 0.6f;

	// Token: 0x04006E6C RID: 28268
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public EntityAlive entity;

	// Token: 0x04006E6D RID: 28269
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public EntityPlayer entityPlayer;

	// Token: 0x04006E6E RID: 28270
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Renderer[] renderers;

	// Token: 0x04006E6F RID: 28271
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int updatePhysicsVisibilityCounter;

	// Token: 0x04006E70 RID: 28272
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool bShowHUDText;

	// Token: 0x04006E71 RID: 28273
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool bLastShowHUDText;

	// Token: 0x04006E72 RID: 28274
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float hideCountdownTime;

	// Token: 0x04006E73 RID: 28275
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static GameManager gameManager;

	// Token: 0x04006E74 RID: 28276
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Camera mainCamera;

	// Token: 0x04006E75 RID: 28277
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public NGuiHUDText hudText;

	// Token: 0x04006E76 RID: 28278
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public GameObject hudTextObj;

	// Token: 0x04006E77 RID: 28279
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public NGuiUIFollowTarget followTarget;
}

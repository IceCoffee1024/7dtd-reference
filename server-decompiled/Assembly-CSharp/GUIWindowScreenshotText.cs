using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using InControl;
using UnityEngine;

// Token: 0x0200124F RID: 4687
public class GUIWindowScreenshotText : GUIWindow
{
	// Token: 0x06009546 RID: 38214 RVA: 0x00387C38 File Offset: 0x00385E38
	public GUIWindowScreenshotText()
		: base(GUIWindowScreenshotText.ID)
	{
		this.alwaysUsesMouseCursor = true;
	}

	// Token: 0x06009547 RID: 38215 RVA: 0x00387C4C File Offset: 0x00385E4C
	public override void OnGUI()
	{
		base.OnGUI();
		Vector2i vector2i = new Vector2i(Screen.width, Screen.height);
		if (this.lastResolution != vector2i)
		{
			this.lastResolution = vector2i;
			this.labelStyle = new GUIStyle(GUI.skin.label)
			{
				wordWrap = true,
				fontStyle = FontStyle.Bold
			};
			this.lineHeight = 18;
			this.inputAreaHeight = this.lineHeight + 7;
		}
		float num;
		float num2;
		Matrix4x4 matrix4x = GUIWindow.UiScaleMatrix(out num, out num2, 0f, 0f, 0.4f, 2f);
		if (Event.current.type == EventType.KeyDown)
		{
			if (Event.current.keyCode == KeyCode.Escape)
			{
				this.CloseWindow();
			}
			else if (Event.current.keyCode == KeyCode.Return || Event.current.keyCode == KeyCode.KeypadEnter)
			{
				this.DoScreenshot();
			}
		}
		float num3 = 20f;
		float num4 = 700f;
		float num5 = 0f;
		for (int i = 0; i < 2; i++)
		{
			float num6 = 70f;
			if (i == 1)
			{
				GUI.Box(new Rect(num3, num6, num4, num5 - 70f + 5f), "");
			}
			if (GameManager.Instance.World != null)
			{
				ChunkCluster chunkCache = GameManager.Instance.World.ChunkCache;
				GameUtils.WorldInfo worldInfo;
				if (chunkCache == null)
				{
					worldInfo = null;
				}
				else
				{
					IChunkProvider chunkProvider = chunkCache.ChunkProvider;
					worldInfo = ((chunkProvider != null) ? chunkProvider.WorldInfo : null);
				}
				GameUtils.WorldInfo worldInfo2 = worldInfo;
				if (i == 1)
				{
					string text = "World: " + GamePrefs.GetString(EnumGamePrefs.GameWorld);
					Utils.DrawOutline(new Rect(num3 + 5f, num6, num4 - 10f, (float)this.inputAreaHeight), text, this.labelStyle, Color.black, Color.white);
				}
				num6 += (float)this.lineHeight;
				if (!PrefabEditModeManager.Instance.IsActive() && worldInfo2 != null && worldInfo2.RandomGeneratedWorld && worldInfo2.DynamicProperties.Contains("Generation", "Seed"))
				{
					if (i == 1)
					{
						Utils.DrawOutline(new Rect(num3 + 5f, num6, num4 - 10f, (float)this.inputAreaHeight), "World gen seed: " + worldInfo2.DynamicProperties.GetString("Generation", "Seed"), this.labelStyle, Color.black, Color.white);
					}
					num6 += (float)this.lineHeight;
				}
				if (i == 1)
				{
					Utils.DrawOutline(new Rect(num3 + 5f, num6, num4 - 10f, (float)this.inputAreaHeight), "Save name / deco seed: " + (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer ? GamePrefs.GetString(EnumGamePrefs.GameName) : GamePrefs.GetString(EnumGamePrefs.GameNameClient)) + " / " + GameManager.Instance.World.Seed.ToString(), this.labelStyle, Color.black, Color.white);
				}
				num6 += (float)this.lineHeight;
				if (LocalPlayerUI.GetUIForPrimaryPlayer() != null)
				{
					EntityPlayer entityPlayer = LocalPlayerUI.GetUIForPrimaryPlayer().entityPlayer;
					if (entityPlayer != null)
					{
						PrefabInstance prefabInstance = entityPlayer.world.GetPOIAtPosition(entityPlayer.position, null, null);
						if (i == 1)
						{
							string text2 = string.Format("Coordinates: {0:F0} {1:F0} {2:F0}", entityPlayer.position.x, entityPlayer.position.y, entityPlayer.position.z);
							if (prefabInstance != null)
							{
								text2 += string.Format(" / relative to POI: {0}", prefabInstance.GetPositionRelativeToPoi(Vector3i.Floor(entityPlayer.position)));
							}
							Utils.DrawOutline(new Rect(num3 + 5f, num6, num4 - 10f, (float)this.inputAreaHeight), text2, this.labelStyle, Color.black, Color.white);
						}
						num6 += (float)this.lineHeight;
						if (prefabInstance != null)
						{
							Prefab prefab = prefabInstance.prefab;
							string text3 = ((prefab != null) ? prefab.PrefabName : null) ?? prefabInstance.name;
							Prefab prefab2 = prefabInstance.prefab;
							string text4 = ((prefab2 != null) ? prefab2.LocalizedEnglishName : null) ?? "";
							if (i == 1)
							{
								Utils.DrawOutline(new Rect(num3 + 5f, num6, num4 - 10f, (float)this.inputAreaHeight), string.Concat(new string[] { "POI: ", text3, " (", text4, ")" }), this.labelStyle, Color.black, Color.white);
							}
							num6 += (float)this.lineHeight;
						}
						prefabInstance = entityPlayer.world.GetPOIAtPosition(entityPlayer.position, new FastTags<TagGroup.Poi>?(FastTags<TagGroup.Poi>.none), new FastTags<TagGroup.Poi>?(DynamicPrefabDecorator.streetTileTag));
						if (prefabInstance != null)
						{
							Prefab prefab3 = prefabInstance.prefab;
							string text5 = ((prefab3 != null) ? prefab3.PrefabName : null) ?? prefabInstance.name;
							Prefab prefab4 = prefabInstance.prefab;
							string text6 = ((prefab4 != null) ? prefab4.LocalizedEnglishName : null) ?? "";
							if (i == 1)
							{
								Utils.DrawOutline(new Rect(num3 + 5f, num6, num4 - 10f, (float)this.inputAreaHeight), string.Concat(new string[] { "Tile: ", text5, " (", text6, ")" }), this.labelStyle, Color.black, Color.white);
							}
							num6 += (float)this.lineHeight;
						}
						if (!this.confirmed)
						{
							if (i == 1)
							{
								this.savePerks = GUI.Toggle(new Rect(num3 + 5f, num6, num4 - 10f, (float)this.lineHeight), this.savePerks, "Save Perks, Buffs and CVars");
							}
							num6 += (float)this.lineHeight;
						}
					}
				}
				num6 += (float)this.lineHeight;
			}
			if (!this.confirmed)
			{
				if (i == 1)
				{
					GUI.SetNextControlName("InputField");
					this.noteInput = GUI.TextField(new Rect(num3 + 5f, num6, num4 - 60f, (float)this.inputAreaHeight), this.noteInput, 300);
					if (this.bFirstTime)
					{
						this.bFirstTime = false;
						GUI.FocusControl("InputField");
					}
					if (GUI.Button(new Rect(num3 + num4 - 50f, num6, 50f, (float)this.inputAreaHeight), "Ok"))
					{
						this.DoScreenshot();
						return;
					}
				}
				num6 += (float)this.inputAreaHeight;
			}
			else
			{
				float num7 = this.labelStyle.CalcHeight(new GUIContent("Note: " + this.noteInput), num4 - 10f);
				if (i == 1)
				{
					Utils.DrawOutline(new Rect(num3 + 5f, num6, num4 - 10f, num7 + 4f), "Note: " + this.noteInput, this.labelStyle, Color.black, Color.white);
				}
				num6 += num7;
			}
			num5 = num6;
		}
		if (this.nGuiWdwDebugPanels == null)
		{
			this.nGuiWdwDebugPanels = global::UnityEngine.Object.FindAnyObjectByType<NGuiWdwDebugPanels>();
		}
		if (this.nGuiWdwDebugPanels != null)
		{
			this.nGuiWdwDebugPanels.showDebugPanel_FocusedBlock((int)num3, (int)num5 + 10, true);
		}
		GUI.matrix = matrix4x;
	}

	// Token: 0x06009548 RID: 38216 RVA: 0x00388374 File Offset: 0x00386574
	[PublicizedFrom(EAccessModifier.Private)]
	public void CloseWindow()
	{
		this.windowManager.Close(this, false);
	}

	// Token: 0x06009549 RID: 38217 RVA: 0x00388383 File Offset: 0x00386583
	[PublicizedFrom(EAccessModifier.Private)]
	public void DoScreenshot()
	{
		this.confirmed = true;
		ThreadManager.StartCoroutine(this.screenshotCo(null));
	}

	// Token: 0x0600954A RID: 38218 RVA: 0x00388399 File Offset: 0x00386599
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator screenshotCo(string _filename)
	{
		yield return null;
		bool saved = true;
		yield return ThreadManager.CoroutineWrapperWithExceptionCallback(GameUtils.TakeScreenshotEnum(GameUtils.EScreenshotMode.Both, _filename, 0f, false, 0, 0, false), delegate(Exception _exception)
		{
			saved = false;
			Log.Exception(_exception);
		});
		if (saved && this.savePerks)
		{
			this.StoreAdditionalStats();
		}
		yield return null;
		this.CloseWindow();
		yield break;
	}

	// Token: 0x0600954B RID: 38219 RVA: 0x003883B0 File Offset: 0x003865B0
	[PublicizedFrom(EAccessModifier.Private)]
	public void StoreAdditionalStats()
	{
		string text = GameUtils.lastSavedScreenshotFilename;
		text = text.Substring(0, text.LastIndexOf('.'));
		if (GameManager.Instance.World != null && GameManager.Instance.World.GetPrimaryPlayer() != null)
		{
			this.StorePlayerStats(text);
		}
	}

	// Token: 0x0600954C RID: 38220 RVA: 0x00388400 File Offset: 0x00386600
	[PublicizedFrom(EAccessModifier.Private)]
	public void StorePlayerStats(string _filenameBase)
	{
		EntityPlayerLocal primaryPlayer = GameManager.Instance.World.GetPrimaryPlayer();
		_filenameBase += "_playerstats.csv";
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine(string.Format("Level,{0}", primaryPlayer.Progression.GetLevel()));
		stringBuilder.AppendLine();
		this.writePlayerSkills(stringBuilder, primaryPlayer);
		this.writePlayerBuffs(stringBuilder, primaryPlayer);
		this.writePlayerCVars(stringBuilder, primaryPlayer);
		SdFile.WriteAllText(_filenameBase, stringBuilder.ToString());
	}

	// Token: 0x0600954D RID: 38221 RVA: 0x0038847C File Offset: 0x0038667C
	[PublicizedFrom(EAccessModifier.Private)]
	public void writePlayerSkills(StringBuilder _sb, EntityPlayerLocal _epl)
	{
		List<ProgressionValue> list = new List<ProgressionValue>();
		foreach (KeyValuePair<int, ProgressionValue> keyValuePair in _epl.Progression.GetDict())
		{
			ProgressionValue value = keyValuePair.Value;
			bool flag;
			if (value == null)
			{
				flag = null != null;
			}
			else
			{
				ProgressionClass progressionClass = value.ProgressionClass;
				flag = ((progressionClass != null) ? progressionClass.Name : null) != null;
			}
			if (flag)
			{
				list.Add(keyValuePair.Value);
			}
		}
		list.Sort(ProgressionClass.ListSortOrderComparer.Instance);
		_sb.AppendLine("Skills");
		_sb.AppendLine("Name,Level,CalcLevel");
		foreach (ProgressionValue progressionValue in list)
		{
			ProgressionClass progressionClass2 = progressionValue.ProgressionClass;
			if (progressionClass2.IsAttribute && progressionClass2.MaxLevel != 0)
			{
				_sb.AppendLine();
				_sb.AppendLine(string.Format("{0},{1},{2}", progressionClass2.Name, progressionValue.Level, progressionValue.CalculatedLevel(_epl)));
			}
			else if (progressionClass2.IsPerk)
			{
				_sb.AppendLine(string.Format(" - {0},{1},{2}", progressionClass2.Name, progressionValue.Level, progressionValue.CalculatedLevel(_epl)));
			}
		}
		_sb.AppendLine();
		_sb.AppendLine("Books");
		_sb.AppendLine("Name,Level,CalcLevel");
		foreach (ProgressionValue progressionValue2 in list)
		{
			ProgressionClass progressionClass3 = progressionValue2.ProgressionClass;
			if (progressionClass3.IsBook)
			{
				_sb.AppendLine(string.Format("{0},{1},{2}", progressionClass3.Name, progressionValue2.Level, progressionValue2.CalculatedLevel(_epl)));
			}
		}
		_sb.AppendLine();
		_sb.AppendLine("Crafting Skills");
		_sb.AppendLine("Name,Level,CalcLevel");
		foreach (ProgressionValue progressionValue3 in list)
		{
			ProgressionClass progressionClass4 = progressionValue3.ProgressionClass;
			if (progressionClass4.IsCrafting)
			{
				_sb.AppendLine(string.Format("{0},{1},{2}", progressionClass4.Name, progressionValue3.Level, progressionValue3.CalculatedLevel(_epl)));
			}
		}
		_sb.AppendLine();
	}

	// Token: 0x0600954E RID: 38222 RVA: 0x0038872C File Offset: 0x0038692C
	[PublicizedFrom(EAccessModifier.Private)]
	public void writePlayerBuffs(StringBuilder _sb, EntityPlayerLocal _epl)
	{
		_sb.AppendLine("Buffs");
		_sb.AppendLine("Buff,FromName,FromId,Missing?");
		foreach (BuffValue buffValue in _epl.Buffs.ActiveBuffs)
		{
			BuffClass buffClass = buffValue.BuffClass;
			Entity entity = GameManager.Instance.World.GetEntity(buffValue.InstigatorId);
			string text = string.Format("none (id {0})", buffValue.InstigatorId);
			_sb.AppendLine(string.Concat(new string[]
			{
				buffValue.BuffName,
				",",
				entity ? entity.GetDebugName() : text,
				",",
				entity ? entity.entityId.ToString() : "",
				",",
				(buffClass == null) ? "BuffClass missing" : ""
			}));
		}
		_sb.AppendLine();
	}

	// Token: 0x0600954F RID: 38223 RVA: 0x00388850 File Offset: 0x00386A50
	[PublicizedFrom(EAccessModifier.Private)]
	public void writePlayerCVars(StringBuilder _sb, EntityPlayerLocal _epl)
	{
		_sb.AppendLine("Buffs");
		_sb.AppendLine("Name,Value");
		foreach (KeyValuePair<string, float> keyValuePair in _epl.Buffs.EnumerateCustomVars(null, false))
		{
			string text;
			float num;
			keyValuePair.Deconstruct(out text, out num);
			string text2 = text;
			float num2 = num;
			if (num2 != 0f)
			{
				_sb.AppendLine(string.Format("{0},{1}", text2, num2));
			}
		}
		_sb.AppendLine();
	}

	// Token: 0x06009550 RID: 38224 RVA: 0x003888F0 File Offset: 0x00386AF0
	public override void OnOpen()
	{
		this.confirmed = false;
		this.bFirstTime = true;
		this.noteInput = "";
		this.isInputActive = true;
		if (UIInput.selection != null)
		{
			UIInput.selection.isSelected = false;
		}
		InputManager.Enabled = false;
	}

	// Token: 0x06009551 RID: 38225 RVA: 0x00388930 File Offset: 0x00386B30
	public override void OnClose()
	{
		base.OnClose();
		this.isInputActive = false;
		InputManager.Enabled = true;
	}

	// Token: 0x06009552 RID: 38226 RVA: 0x00388948 File Offset: 0x00386B48
	public static void Open(LocalPlayerUI _playerUi, bool _savePerks)
	{
		GUIWindowScreenshotText window = _playerUi.windowManager.GetWindow<GUIWindowScreenshotText>(GUIWindowScreenshotText.ID);
		if (window == null)
		{
			return;
		}
		window.savePerks = _savePerks;
		_playerUi.windowManager.Open(window, false);
	}

	// Token: 0x04006FD3 RID: 28627
	public static readonly string ID = "GUIWindowScreenshotText";

	// Token: 0x04006FD4 RID: 28628
	[PublicizedFrom(EAccessModifier.Private)]
	public const int PosX = 20;

	// Token: 0x04006FD5 RID: 28629
	[PublicizedFrom(EAccessModifier.Private)]
	public const int PosY = 70;

	// Token: 0x04006FD6 RID: 28630
	[PublicizedFrom(EAccessModifier.Private)]
	public const int Width = 700;

	// Token: 0x04006FD7 RID: 28631
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bFirstTime;

	// Token: 0x04006FD8 RID: 28632
	[PublicizedFrom(EAccessModifier.Private)]
	public string noteInput;

	// Token: 0x04006FD9 RID: 28633
	[PublicizedFrom(EAccessModifier.Private)]
	public bool savePerks;

	// Token: 0x04006FDA RID: 28634
	[PublicizedFrom(EAccessModifier.Private)]
	public bool confirmed;

	// Token: 0x04006FDB RID: 28635
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2i lastResolution;

	// Token: 0x04006FDC RID: 28636
	[PublicizedFrom(EAccessModifier.Private)]
	public GUIStyle labelStyle;

	// Token: 0x04006FDD RID: 28637
	[PublicizedFrom(EAccessModifier.Private)]
	public int lineHeight;

	// Token: 0x04006FDE RID: 28638
	[PublicizedFrom(EAccessModifier.Private)]
	public int inputAreaHeight;

	// Token: 0x04006FDF RID: 28639
	[PublicizedFrom(EAccessModifier.Private)]
	public NGuiWdwDebugPanels nGuiWdwDebugPanels;
}

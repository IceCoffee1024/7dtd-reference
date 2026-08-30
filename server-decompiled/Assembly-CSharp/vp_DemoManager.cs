using System;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x0200157A RID: 5498
public class vp_DemoManager
{
	// Token: 0x0600ABE5 RID: 44005 RVA: 0x003FF5E0 File Offset: 0x003FD7E0
	public vp_DemoManager()
	{
		this.DesktopResolution = Screen.currentResolution;
		this.LastInputTime = Time.time;
		this.m_FullScreenFadeTexture = new Texture2D(1, 1, TextureFormat.RGB24, false);
	}

	// Token: 0x0600ABE6 RID: 44006 RVA: 0x003FF694 File Offset: 0x003FD894
	public virtual void Update()
	{
		if (double.IsNaN((double)Camera.main.fieldOfView))
		{
			Camera.main.fieldOfView = 60f;
		}
		if (Input.GetKeyDown(KeyCode.U))
		{
			Screen.fullScreen = !Screen.fullScreen;
		}
		if (Screen.fullScreen && Screen.currentResolution.width != this.DesktopResolution.width)
		{
			Screen.SetResolution(this.DesktopResolution.width, this.DesktopResolution.height, true);
		}
		if (Input.GetKeyDown(KeyCode.L))
		{
			this.m_SimulateLowFPS = !this.m_SimulateLowFPS;
		}
		if (this.m_SimulateLowFPS)
		{
			for (int i = 0; i < 20000000; i++)
			{
			}
		}
	}

	// Token: 0x0600ABE7 RID: 44007 RVA: 0x003FF748 File Offset: 0x003FD948
	public bool ButtonToggle(Rect rect, string label, bool state, bool arrow, Texture imageUpPointer)
	{
		if (!this.ShowGUI)
		{
			return false;
		}
		GUIStyle guistyle = this.UpStyle;
		GUIStyle guistyle2 = this.DownStyle;
		float num = 0f;
		if (state)
		{
			guistyle = this.DownStyle;
			guistyle2 = this.UpStyle;
			num = rect.width * 0.5f + 2f;
		}
		GUI.Label(new Rect(rect.x, rect.y - 30f, rect.width, rect.height), label, this.CenterStyle);
		if (GUI.Button(new Rect(rect.x, rect.y, rect.width * 0.5f - 2f, rect.height), "OFF", guistyle2))
		{
			state = false;
		}
		if (GUI.Button(new Rect(rect.x + rect.width * 0.5f + 2f, rect.y, rect.width * 0.5f, rect.height), "ON", guistyle))
		{
			state = true;
		}
		if (arrow)
		{
			GUI.Label(new Rect(rect.x + rect.width * 0.5f * 0.5f - 14f + num, rect.y + rect.height, 32f, 32f), imageUpPointer);
		}
		return state;
	}

	// Token: 0x0600ABE8 RID: 44008 RVA: 0x003FF8A0 File Offset: 0x003FDAA0
	public void DrawBoxes(string caption, string description, Texture imageLeftArrow, Texture imageRightArrow, vp_DemoManager.LoadLevelCallback nextLevelCallback = null, vp_DemoManager.LoadLevelCallback prevLevelCallback = null, bool drawBox = true)
	{
		if (!this.ShowGUI)
		{
			return;
		}
		GUI.color = new Color(1f, 1f, 1f, 1f * this.GlobalAlpha);
		GUILayout.BeginArea(new Rect((float)(Screen.width / 2) - 400f, 30f, 800f, 100f));
		if (imageLeftArrow != null)
		{
			GUI.Box(new Rect(30f, 10f, 80f, 80f), "");
		}
		if (drawBox)
		{
			GUI.Box(new Rect(120f, 0f, 560f, 100f), "");
		}
		GUI.color = new Color(1f, 1f, 1f, this.m_TextAlpha * this.GlobalAlpha);
		for (int i = 0; i < 3; i++)
		{
			GUILayout.BeginArea(new Rect(130f, 10f, 540f, 80f));
			GUILayout.Label("--- " + caption.ToUpper() + " ---\n" + description, this.LabelStyle, Array.Empty<GUILayoutOption>());
			GUILayout.EndArea();
		}
		GUI.color = new Color(1f, 1f, 1f, 1f * this.GlobalAlpha);
		if (imageRightArrow != null)
		{
			GUI.Box(new Rect(690f, 10f, 80f, 80f), "");
		}
		if (imageLeftArrow != null && GUI.Button(new Rect(35f, 15f, 80f, 80f), imageLeftArrow, "Label"))
		{
			if (prevLevelCallback == null)
			{
				this.m_FadeToScreen = Mathf.Max(this.CurrentScreen - 1, 1);
				this.m_FadeState = vp_DemoManager.FadeState.FadeOut;
			}
			else
			{
				prevLevelCallback();
			}
		}
		if (Time.time < this.LastInputTime + 30f)
		{
			this.m_BigArrowFadeAlpha = 1f;
		}
		else
		{
			this.m_BigArrowFadeAlpha = 0.5f - Mathf.Sin((Time.time - 0.5f) * 6f) * 1f;
		}
		GUI.color = new Color(1f, 1f, 1f, this.m_BigArrowFadeAlpha * this.GlobalAlpha);
		if (imageRightArrow != null && GUI.Button(new Rect(700f, 15f, 80f, 80f), imageRightArrow, "Label"))
		{
			if (nextLevelCallback == null)
			{
				this.m_FadeToScreen = this.CurrentScreen + 1;
				this.m_FadeState = vp_DemoManager.FadeState.FadeOut;
			}
			else
			{
				nextLevelCallback();
			}
		}
		GUI.color = new Color(1f, 1f, 1f, 1f * this.GlobalAlpha);
		GUILayout.EndArea();
		GUI.color = new Color(1f, 1f, 1f, this.m_TextAlpha * this.GlobalAlpha);
	}

	// Token: 0x0600ABE9 RID: 44009 RVA: 0x003FFB94 File Offset: 0x003FDD94
	public int ToggleColumn(int width, int y, int sel, string[] strings, bool center, bool arrow, Texture imageRightPointer, Texture imageLeftPointer)
	{
		if (!this.ShowGUI)
		{
			return 0;
		}
		float num = (float)(strings.Length * 30);
		Vector2 vector = new Vector2((float)(Screen.width / 2), (float)(Screen.height / 2));
		Rect rect;
		if (center)
		{
			rect = new Rect(vector.x - (float)width, (float)y, (float)width, 30f);
		}
		else
		{
			rect = new Rect((float)(Screen.width - width - 10), vector.y - num / 2f, (float)width, 30f);
		}
		int num2 = 0;
		foreach (string text in strings)
		{
			if (center)
			{
				rect.x = vector.x - (float)(width / 2);
			}
			else
			{
				rect.x = 10f;
			}
			rect.width = (float)width;
			GUIStyle guistyle = this.UpStyle;
			if (num2 == sel)
			{
				Color color = GUI.color;
				GUI.color = new Color(1f, 1f, 1f, 1f * this.GlobalAlpha);
				guistyle = this.DownStyle;
				if (center)
				{
					rect.x = vector.x - (float)(width / 2) + 10f;
				}
				else
				{
					rect.x = 20f;
				}
				rect.width = (float)(width - 20);
				if (arrow && !this.ClosingDown)
				{
					if (center)
					{
						GUI.Label(new Rect(rect.x - 27f, rect.y, 32f, 32f), imageRightPointer);
					}
					else
					{
						GUI.Label(new Rect(rect.x + rect.width + 5f, rect.y, 32f, 32f), imageLeftPointer);
					}
				}
				GUI.color = color;
			}
			if (GUI.Button(rect, text, guistyle))
			{
				sel = num2;
			}
			rect.y += 35f;
			num2++;
		}
		return sel;
	}

	// Token: 0x0600ABEA RID: 44010 RVA: 0x003FFD78 File Offset: 0x003FDF78
	public int ButtonColumn(int y, int sel, string[] strings, Texture imagePointer)
	{
		if (!this.ShowGUI)
		{
			return 0;
		}
		float num = (float)(Screen.width / 2);
		Rect rect = new Rect(num - 100f, (float)y, 200f, 30f);
		int num2 = 0;
		foreach (string text in strings)
		{
			rect.x = num - 100f;
			rect.width = 200f;
			if (GUI.Button(rect, text))
			{
				sel = num2;
				this.ButtonColumnClickTime = Time.time;
				this.ButtonColumnArrowY = rect.y;
			}
			rect.y += 35f;
			num2++;
		}
		if (Time.time < this.ButtonColumnArrowFadeoutTime)
		{
			this.ButtonColumnClickTime = Time.time;
		}
		GUI.color = new Color(1f, 1f, 1f, Mathf.Max(0f, 1f - (Time.time - this.ButtonColumnClickTime) * 1f * this.GlobalAlpha));
		GUI.Label(new Rect(rect.x - 27f, this.ButtonColumnArrowY, 32f, 32f), imagePointer);
		GUI.color = new Color(1f, 1f, 1f, 1f * this.GlobalAlpha);
		return sel;
	}

	// Token: 0x0600ABEB RID: 44011 RVA: 0x003FFECD File Offset: 0x003FE0CD
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Reset()
	{
		this.ButtonSelection = 0;
		this.FirstFrame = true;
		this.LastInputTime = Time.time;
	}

	// Token: 0x0600ABEC RID: 44012 RVA: 0x003FFEE8 File Offset: 0x003FE0E8
	[PublicizedFrom(EAccessModifier.Private)]
	public void InitGUIStyles()
	{
		this.LabelStyle = new GUIStyle("Label");
		this.LabelStyle.alignment = TextAnchor.LowerCenter;
		this.UpStyle = new GUIStyle("Button");
		this.DownStyle = new GUIStyle("Button");
		this.DownStyle.normal = this.DownStyle.active;
		this.CenterStyle = new GUIStyle("Label");
		this.CenterStyle.alignment = TextAnchor.MiddleCenter;
		this.m_StylesInitialized = true;
	}

	// Token: 0x0600ABED RID: 44013 RVA: 0x003FFF80 File Offset: 0x003FE180
	public void DrawImage(Texture image, float xOffset, float yOffset)
	{
		if (!this.ShowGUI)
		{
			return;
		}
		if (image == null)
		{
			return;
		}
		float num = (float)(Screen.width / 2);
		float num2 = (float)Mathf.Min(image.width, Screen.width);
		float num3 = (float)image.height / (float)image.width;
		GUI.DrawTexture(new Rect(num - num2 / 2f + xOffset, 140f + yOffset, num2, num2 * num3), image);
	}

	// Token: 0x0600ABEE RID: 44014 RVA: 0x003FFFEA File Offset: 0x003FE1EA
	public void DrawImage(Texture image)
	{
		this.DrawImage(image, 0f, 0f);
	}

	// Token: 0x0600ABEF RID: 44015 RVA: 0x00400000 File Offset: 0x003FE200
	public void DrawEditorPreview(Texture section, Texture imageEditorPreview, Texture imageEditorScreenshot)
	{
		if (!this.ShowGUI)
		{
			return;
		}
		Color color = GUI.color;
		Vector2 vector = new Vector2(Input.mousePosition.x, (float)Screen.height - Input.mousePosition.y);
		float num = 0f;
		if (this.EditorPreviewSectionExpanded)
		{
			float num2 = (float)(Screen.height - section.height - imageEditorPreview.height);
			float num3 = (float)(Screen.height - section.height);
			GUI.DrawTexture(new Rect(num, num2, (float)imageEditorPreview.width, (float)imageEditorPreview.height), imageEditorPreview);
			GUI.DrawTexture(new Rect(num, num3, (float)section.width, (float)section.height), section);
			if (vector.x > num && vector.x < num + (float)section.width && vector.y > num2 && vector.y < (float)(Screen.height - imageEditorPreview.height))
			{
				this.m_EditorPreviewScreenshotTextAlpha = Mathf.Min(1f, this.m_EditorPreviewScreenshotTextAlpha + 0.01f);
				if (Input.GetMouseButtonDown(0))
				{
					this.EditorPreviewSectionExpanded = false;
				}
			}
			else
			{
				this.m_EditorPreviewScreenshotTextAlpha = Mathf.Max(0f, this.m_EditorPreviewScreenshotTextAlpha - 0.03f);
			}
			GUI.color = new Color(1f, 1f, 1f, color.a * 0.5f * this.m_EditorPreviewScreenshotTextAlpha * this.GlobalAlpha);
			GUI.DrawTexture(new Rect(num + 48f, num3 + (float)(section.height / 2) - (float)(imageEditorScreenshot.height / 2), (float)imageEditorScreenshot.width, (float)imageEditorScreenshot.height), imageEditorScreenshot);
		}
		else
		{
			float num4 = (float)(Screen.height - imageEditorPreview.height);
			GUI.DrawTexture(new Rect(num, num4, (float)imageEditorPreview.width, (float)imageEditorPreview.height), imageEditorPreview);
			if (vector.x > num && vector.x < num + (float)section.width && vector.y > num4 && Input.GetMouseButtonUp(0))
			{
				this.EditorPreviewSectionExpanded = true;
			}
		}
		GUI.color = color;
	}

	// Token: 0x0600ABF0 RID: 44016 RVA: 0x00400204 File Offset: 0x003FE404
	public void DrawFullScreenText(Texture imageFullScreen)
	{
		if (!this.ShowGUI)
		{
			return;
		}
		if (Time.realtimeSinceStartup > 5f)
		{
			return;
		}
		if (Time.realtimeSinceStartup > 3f)
		{
			this.m_FullScreenTextAlpha -= this.m_FadeSpeed * Time.deltaTime * 15f;
		}
		GUI.color = new Color(1f, 1f, 1f, this.m_FullScreenTextAlpha * this.GlobalAlpha);
		GUI.DrawTexture(new Rect((float)(Screen.width / 2 - 120), (float)(Screen.height / 2 - 16), 240f, 32f), imageFullScreen);
		GUI.color = new Color(1f, 1f, 1f, 1f * this.GlobalAlpha);
	}

	// Token: 0x0600ABF1 RID: 44017 RVA: 0x004002C8 File Offset: 0x003FE4C8
	public void DoScreenTransition()
	{
		if (!this.ShowGUI)
		{
			return;
		}
		if (this.m_FadeState == vp_DemoManager.FadeState.FadeOut)
		{
			this.m_TextAlpha -= this.m_FadeSpeed;
			if (this.m_TextAlpha <= 0f)
			{
				this.m_TextAlpha = 0f;
				this.Reset();
				this.CurrentScreen = this.m_FadeToScreen;
				this.m_FadeState = vp_DemoManager.FadeState.FadeIn;
				return;
			}
		}
		else if (this.m_FadeState == vp_DemoManager.FadeState.FadeIn && !this.ClosingDown)
		{
			this.m_TextAlpha += this.m_FadeSpeed;
			if (this.m_TextAlpha >= 1f)
			{
				this.m_TextAlpha = 1f;
				this.m_FadeState = vp_DemoManager.FadeState.None;
			}
		}
	}

	// Token: 0x0600ABF2 RID: 44018 RVA: 0x0040036F File Offset: 0x003FE56F
	public void SetScreen(int screen)
	{
		this.m_FadeToScreen = screen;
		this.m_FadeState = vp_DemoManager.FadeState.FadeOut;
	}

	// Token: 0x0600ABF3 RID: 44019 RVA: 0x00400380 File Offset: 0x003FE580
	public void OnGUI()
	{
		if (!this.m_StylesInitialized)
		{
			this.InitGUIStyles();
		}
		this.DoScreenTransition();
		if (vp_Utility.LockCursor && this.FadeGUIOnCursorLock)
		{
			this.GlobalAlpha = 0.35f;
		}
		else if (!this.ClosingDown)
		{
			this.GlobalAlpha = 1f;
		}
		if (Time.time - this.CurrentFullScreenFadeTime < this.m_FullScreenFadeInDuration)
		{
			this.GlobalAlpha = Time.time - this.CurrentFullScreenFadeTime;
			GUI.color = new Color(0f, 0f, 0f, (this.m_FullScreenFadeInDuration - this.GlobalAlpha) / this.m_FullScreenFadeInDuration);
			GUI.DrawTexture(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height), this.m_FullScreenFadeTexture);
			GUI.color = Color.white;
		}
		if (this.CurrentFullScreenFadeTime > Time.time)
		{
			this.GlobalAlpha = this.CurrentFullScreenFadeTime - Time.time;
			GUI.color = new Color(0f, 0f, 0f, (this.m_FullScreenFadeOutDuration - this.GlobalAlpha) / this.m_FullScreenFadeOutDuration);
			GUI.DrawTexture(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height), this.m_FullScreenFadeTexture);
			GUI.color = Color.white;
		}
	}

	// Token: 0x0600ABF4 RID: 44020 RVA: 0x004004D0 File Offset: 0x003FE6D0
	public void LoadLevel(int level)
	{
		this.ClosingDown = true;
		vp_Timer.CancelAll();
		vp_TimeUtility.TimeScale = 1f;
		this.m_FadeState = vp_DemoManager.FadeState.FadeOut;
		this.CurrentFullScreenFadeTime = Time.time + this.m_FullScreenFadeOutDuration;
		vp_Timer.In(this.m_FullScreenFadeOutDuration, delegate
		{
			SceneManager.LoadScene(level);
		}, null);
	}

	// Token: 0x04007EED RID: 32493
	public GUIStyle UpStyle;

	// Token: 0x04007EEE RID: 32494
	public GUIStyle LabelStyle;

	// Token: 0x04007EEF RID: 32495
	public GUIStyle DownStyle;

	// Token: 0x04007EF0 RID: 32496
	public GUIStyle CenterStyle;

	// Token: 0x04007EF1 RID: 32497
	public int CurrentScreen = 1;

	// Token: 0x04007EF2 RID: 32498
	public Resolution DesktopResolution;

	// Token: 0x04007EF3 RID: 32499
	public bool FirstFrame = true;

	// Token: 0x04007EF4 RID: 32500
	public bool EditorPreviewSectionExpanded = true;

	// Token: 0x04007EF5 RID: 32501
	public bool ShowGUI = true;

	// Token: 0x04007EF6 RID: 32502
	public float ButtonColumnClickTime;

	// Token: 0x04007EF7 RID: 32503
	public float ButtonColumnArrowY = -100f;

	// Token: 0x04007EF8 RID: 32504
	public float ButtonColumnArrowFadeoutTime;

	// Token: 0x04007EF9 RID: 32505
	public int ButtonSelection;

	// Token: 0x04007EFA RID: 32506
	public float LastInputTime;

	// Token: 0x04007EFB RID: 32507
	public bool FadeGUIOnCursorLock = true;

	// Token: 0x04007EFC RID: 32508
	[PublicizedFrom(EAccessModifier.Private)]
	public float m_FadeSpeed = 0.03f;

	// Token: 0x04007EFD RID: 32509
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_FadeToScreen;

	// Token: 0x04007EFE RID: 32510
	[PublicizedFrom(EAccessModifier.Private)]
	public bool m_StylesInitialized;

	// Token: 0x04007EFF RID: 32511
	[PublicizedFrom(EAccessModifier.Private)]
	public vp_DemoManager.FadeState m_FadeState;

	// Token: 0x04007F00 RID: 32512
	[PublicizedFrom(EAccessModifier.Private)]
	public Texture2D m_FullScreenFadeTexture;

	// Token: 0x04007F01 RID: 32513
	[PublicizedFrom(EAccessModifier.Private)]
	public float m_FullScreenFadeOutDuration = 0.5f;

	// Token: 0x04007F02 RID: 32514
	[PublicizedFrom(EAccessModifier.Private)]
	public float m_FullScreenFadeInDuration = 0.75f;

	// Token: 0x04007F03 RID: 32515
	public float CurrentFullScreenFadeTime;

	// Token: 0x04007F04 RID: 32516
	public bool ClosingDown;

	// Token: 0x04007F05 RID: 32517
	public float GlobalAlpha = 0.35f;

	// Token: 0x04007F06 RID: 32518
	[PublicizedFrom(EAccessModifier.Private)]
	public float m_TextAlpha = 1f;

	// Token: 0x04007F07 RID: 32519
	[PublicizedFrom(EAccessModifier.Private)]
	public float m_EditorPreviewScreenshotTextAlpha;

	// Token: 0x04007F08 RID: 32520
	[PublicizedFrom(EAccessModifier.Private)]
	public float m_FullScreenTextAlpha = 1f;

	// Token: 0x04007F09 RID: 32521
	[PublicizedFrom(EAccessModifier.Private)]
	public float m_BigArrowFadeAlpha = 1f;

	// Token: 0x04007F0A RID: 32522
	[PublicizedFrom(EAccessModifier.Private)]
	public bool m_SimulateLowFPS;

	// Token: 0x0200157B RID: 5499
	[PublicizedFrom(EAccessModifier.Private)]
	public enum FadeState
	{
		// Token: 0x04007F0C RID: 32524
		None,
		// Token: 0x04007F0D RID: 32525
		FadeOut,
		// Token: 0x04007F0E RID: 32526
		FadeIn
	}

	// Token: 0x0200157C RID: 5500
	// (Invoke) Token: 0x0600ABF6 RID: 44022
	public delegate void LoadLevelCallback();
}

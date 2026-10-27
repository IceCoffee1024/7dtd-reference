using System;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x020015C9 RID: 5577
public class vp_DemoManager
{
	// Token: 0x0600ADED RID: 44525 RVA: 0x0040BB78 File Offset: 0x00409D78
	public vp_DemoManager()
	{
		this.DesktopResolution = Screen.currentResolution;
		this.LastInputTime = Time.time;
		this.m_FullScreenFadeTexture = new Texture2D(1, 1, TextureFormat.RGB24, false);
	}

	// Token: 0x0600ADEE RID: 44526 RVA: 0x0040BC2C File Offset: 0x00409E2C
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

	// Token: 0x0600ADEF RID: 44527 RVA: 0x0040BCE0 File Offset: 0x00409EE0
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

	// Token: 0x0600ADF0 RID: 44528 RVA: 0x0040BE38 File Offset: 0x0040A038
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

	// Token: 0x0600ADF1 RID: 44529 RVA: 0x0040C12C File Offset: 0x0040A32C
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

	// Token: 0x0600ADF2 RID: 44530 RVA: 0x0040C310 File Offset: 0x0040A510
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

	// Token: 0x0600ADF3 RID: 44531 RVA: 0x0040C465 File Offset: 0x0040A665
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Reset()
	{
		this.ButtonSelection = 0;
		this.FirstFrame = true;
		this.LastInputTime = Time.time;
	}

	// Token: 0x0600ADF4 RID: 44532 RVA: 0x0040C480 File Offset: 0x0040A680
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

	// Token: 0x0600ADF5 RID: 44533 RVA: 0x0040C518 File Offset: 0x0040A718
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

	// Token: 0x0600ADF6 RID: 44534 RVA: 0x0040C582 File Offset: 0x0040A782
	public void DrawImage(Texture image)
	{
		this.DrawImage(image, 0f, 0f);
	}

	// Token: 0x0600ADF7 RID: 44535 RVA: 0x0040C598 File Offset: 0x0040A798
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

	// Token: 0x0600ADF8 RID: 44536 RVA: 0x0040C79C File Offset: 0x0040A99C
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

	// Token: 0x0600ADF9 RID: 44537 RVA: 0x0040C860 File Offset: 0x0040AA60
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

	// Token: 0x0600ADFA RID: 44538 RVA: 0x0040C907 File Offset: 0x0040AB07
	public void SetScreen(int screen)
	{
		this.m_FadeToScreen = screen;
		this.m_FadeState = vp_DemoManager.FadeState.FadeOut;
	}

	// Token: 0x0600ADFB RID: 44539 RVA: 0x0040C918 File Offset: 0x0040AB18
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

	// Token: 0x0600ADFC RID: 44540 RVA: 0x0040CA68 File Offset: 0x0040AC68
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

	// Token: 0x0400808A RID: 32906
	public GUIStyle UpStyle;

	// Token: 0x0400808B RID: 32907
	public GUIStyle LabelStyle;

	// Token: 0x0400808C RID: 32908
	public GUIStyle DownStyle;

	// Token: 0x0400808D RID: 32909
	public GUIStyle CenterStyle;

	// Token: 0x0400808E RID: 32910
	public int CurrentScreen = 1;

	// Token: 0x0400808F RID: 32911
	public Resolution DesktopResolution;

	// Token: 0x04008090 RID: 32912
	public bool FirstFrame = true;

	// Token: 0x04008091 RID: 32913
	public bool EditorPreviewSectionExpanded = true;

	// Token: 0x04008092 RID: 32914
	public bool ShowGUI = true;

	// Token: 0x04008093 RID: 32915
	public float ButtonColumnClickTime;

	// Token: 0x04008094 RID: 32916
	public float ButtonColumnArrowY = -100f;

	// Token: 0x04008095 RID: 32917
	public float ButtonColumnArrowFadeoutTime;

	// Token: 0x04008096 RID: 32918
	public int ButtonSelection;

	// Token: 0x04008097 RID: 32919
	public float LastInputTime;

	// Token: 0x04008098 RID: 32920
	public bool FadeGUIOnCursorLock = true;

	// Token: 0x04008099 RID: 32921
	[PublicizedFrom(EAccessModifier.Private)]
	public float m_FadeSpeed = 0.03f;

	// Token: 0x0400809A RID: 32922
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_FadeToScreen;

	// Token: 0x0400809B RID: 32923
	[PublicizedFrom(EAccessModifier.Private)]
	public bool m_StylesInitialized;

	// Token: 0x0400809C RID: 32924
	[PublicizedFrom(EAccessModifier.Private)]
	public vp_DemoManager.FadeState m_FadeState;

	// Token: 0x0400809D RID: 32925
	[PublicizedFrom(EAccessModifier.Private)]
	public Texture2D m_FullScreenFadeTexture;

	// Token: 0x0400809E RID: 32926
	[PublicizedFrom(EAccessModifier.Private)]
	public float m_FullScreenFadeOutDuration = 0.5f;

	// Token: 0x0400809F RID: 32927
	[PublicizedFrom(EAccessModifier.Private)]
	public float m_FullScreenFadeInDuration = 0.75f;

	// Token: 0x040080A0 RID: 32928
	public float CurrentFullScreenFadeTime;

	// Token: 0x040080A1 RID: 32929
	public bool ClosingDown;

	// Token: 0x040080A2 RID: 32930
	public float GlobalAlpha = 0.35f;

	// Token: 0x040080A3 RID: 32931
	[PublicizedFrom(EAccessModifier.Private)]
	public float m_TextAlpha = 1f;

	// Token: 0x040080A4 RID: 32932
	[PublicizedFrom(EAccessModifier.Private)]
	public float m_EditorPreviewScreenshotTextAlpha;

	// Token: 0x040080A5 RID: 32933
	[PublicizedFrom(EAccessModifier.Private)]
	public float m_FullScreenTextAlpha = 1f;

	// Token: 0x040080A6 RID: 32934
	[PublicizedFrom(EAccessModifier.Private)]
	public float m_BigArrowFadeAlpha = 1f;

	// Token: 0x040080A7 RID: 32935
	[PublicizedFrom(EAccessModifier.Private)]
	public bool m_SimulateLowFPS;

	// Token: 0x020015CA RID: 5578
	[PublicizedFrom(EAccessModifier.Private)]
	public enum FadeState
	{
		// Token: 0x040080A9 RID: 32937
		None,
		// Token: 0x040080AA RID: 32938
		FadeOut,
		// Token: 0x040080AB RID: 32939
		FadeIn
	}

	// Token: 0x020015CB RID: 5579
	// (Invoke) Token: 0x0600ADFE RID: 44542
	public delegate void LoadLevelCallback();
}

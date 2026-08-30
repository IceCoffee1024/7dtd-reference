using System;
using UnityEngine;

// Token: 0x0200159F RID: 5535
public class vp_FPSDemo3 : MonoBehaviour
{
	// Token: 0x0600AD35 RID: 44341 RVA: 0x004092C0 File Offset: 0x004074C0
	[PublicizedFrom(EAccessModifier.Private)]
	public void Start()
	{
		this.m_FPSCamera = global::UnityEngine.Object.FindAnyObjectByType<vp_FPCamera>();
		this.m_Demo = new vp_FPSDemoManager(this.PlayerGameObject);
		this.m_Demo.CurrentFullScreenFadeTime = Time.time;
		this.m_Demo.DrawCrosshair = false;
		this.m_Demo.FadeGUIOnCursorLock = false;
		this.m_Demo.Input.MouseCursorZones = new Rect[2];
		this.m_Demo.Input.MouseCursorZones[0] = new Rect((float)Screen.width * 0.5f - 370f, 40f, 80f, 80f);
		this.m_Demo.Input.MouseCursorZones[1] = new Rect((float)Screen.width * 0.5f + 290f, 40f, 80f, 80f);
		vp_Utility.LockCursor = false;
	}

	// Token: 0x0600AD36 RID: 44342 RVA: 0x004093A8 File Offset: 0x004075A8
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update()
	{
		this.m_Demo.Update();
		if (Vector3.Distance(this.PlayerGameObject.transform.position, this.m_StartPos) > 100f)
		{
			this.m_Demo.Teleport(this.m_StartPos, this.m_StartAngle);
		}
	}

	// Token: 0x0600AD37 RID: 44343 RVA: 0x004093FC File Offset: 0x004075FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void DemoIntro()
	{
		if (this.m_Demo.FirstFrame)
		{
			this.m_Demo.FirstFrame = false;
			this.m_Demo.DrawCrosshair = false;
			this.m_Demo.FreezePlayer(this.m_OverviewPos, this.m_OverviewAngle, true);
			this.m_Demo.Input.MouseCursorForced = true;
			this.m_BodyAnimator = global::UnityEngine.Object.FindAnyObjectByType<vp_BodyAnimator>();
			if (this.m_BodyAnimator != null)
			{
				this.m_BodyAnimator.gameObject.SetActive(false);
			}
		}
		this.m_Demo.DrawBoxes("welcome", "Featuring the SMOOTHEST CONTROLS and the most POWERFUL FPS CAMERA\navailable for Unity, Ultimate FPS is an awesome script pack for achieving that special\n 'AAA FPS' feeling. This demo will walk you through some of its core features ...\n", null, this.ImageRightArrow, null, null, true);
		this.m_Demo.ForceCameraShake();
	}

	// Token: 0x0600AD38 RID: 44344 RVA: 0x004094AC File Offset: 0x004076AC
	[PublicizedFrom(EAccessModifier.Private)]
	public void DemoGameplay()
	{
		if (this.m_Demo.FirstFrame)
		{
			this.m_Demo.FirstFrame = false;
			this.m_Demo.DrawCrosshair = true;
			this.m_Demo.UnFreezePlayer();
			this.m_Demo.Teleport(this.m_StartPos, this.m_StartAngle);
			vp_Utility.LockCursor = true;
			this.m_Demo.Input.MouseCursorForced = false;
			if (this.m_BodyAnimator != null)
			{
				this.m_BodyAnimator.gameObject.SetActive(true);
			}
		}
		this.m_Demo.DrawBoxes("part i: some examples", "This level has some basic gameplay features.\n• Press SHIFT to SPRINT, C to CROUCH, and the RIGHT MOUSE BUTTON to AIM.\n• To SWITCH WEAPONS, press Q, E or 1-3.\n• Press R to RELOAD, F to INTERACT and V for 3RD PERSON.", this.ImageLeftArrow, this.ImageRightArrow, delegate
		{
			this.m_Demo.LoadLevel(1);
		}, null, true);
		if (this.m_Demo.ShowGUI && vp_Utility.LockCursor && !this.m_LoadingNextLevel && !this.m_Demo.ClosingDown)
		{
			GUI.color = new Color(1f, 1f, 1f, this.m_Demo.ClosingDown ? this.m_Demo.GlobalAlpha : 1f);
			GUI.Label(new Rect((float)(Screen.width / 2 - 200), 140f, 400f, 20f), "(Press ENTER to reenable mouse cursor.)", this.m_Demo.CenterStyle);
			GUI.color = new Color(1f, 1f, 1f, 1f * this.m_Demo.GlobalAlpha);
		}
	}

	// Token: 0x0600AD39 RID: 44345 RVA: 0x00409634 File Offset: 0x00407834
	[PublicizedFrom(EAccessModifier.Private)]
	public void DemoOutro()
	{
		if (this.m_Demo.FirstFrame)
		{
			this.m_Demo.FirstFrame = false;
			this.m_Demo.DrawCrosshair = false;
			this.m_Demo.FreezePlayer(this.m_OutroPos, this.m_OutroAngle, true);
			this.m_Demo.Input.MouseCursorForced = true;
			this.m_OutroStartTime = Time.time;
		}
		this.m_FPSCamera.Angle = new Vector2(this.m_OutroAngle.x, this.m_OutroAngle.y + Mathf.Cos((Time.time - this.m_OutroStartTime + 50f) * 0.03f) * 20f);
		this.m_Demo.DrawBoxes("putting it all together", "Included in the package is full, well commented C# source code, an in-depth 70-page MANUAL in PDF format, a game-ready FPS PLAYER prefab along with all the scripts and content used in this demo. A FANTASTIC starting point (or upgrade) for any FPS project.\nBest part? It can be yours in a minute. GET IT NOW on visionpunk.com!", this.ImageLeftArrow, this.ImageCheckmark, delegate
		{
			this.m_Demo.LoadLevel(0);
		}, null, true);
		this.m_Demo.DrawImage(this.ImageAllParams);
	}

	// Token: 0x0600AD3A RID: 44346 RVA: 0x00409724 File Offset: 0x00407924
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnGUI()
	{
		this.m_Demo.OnGUI();
		switch (this.m_Demo.CurrentScreen)
		{
		case 1:
			this.DemoIntro();
			return;
		case 2:
			this.DemoGameplay();
			return;
		case 3:
			this.DemoOutro();
			return;
		default:
			return;
		}
	}

	// Token: 0x040080B9 RID: 32953
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_FPCamera m_FPSCamera;

	// Token: 0x040080BA RID: 32954
	public GameObject PlayerGameObject;

	// Token: 0x040080BB RID: 32955
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_FPSDemoManager m_Demo;

	// Token: 0x040080BC RID: 32956
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_BodyAnimator m_BodyAnimator;

	// Token: 0x040080BD RID: 32957
	public Texture ImageLeftArrow;

	// Token: 0x040080BE RID: 32958
	public Texture ImageRightArrow;

	// Token: 0x040080BF RID: 32959
	public Texture ImageCheckmark;

	// Token: 0x040080C0 RID: 32960
	public Texture ImagePresetDialogs;

	// Token: 0x040080C1 RID: 32961
	public Texture ImageShooter;

	// Token: 0x040080C2 RID: 32962
	public Texture ImageAllParams;

	// Token: 0x040080C3 RID: 32963
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 m_StartPos = new Vector3(113f, 106f, -87f);

	// Token: 0x040080C4 RID: 32964
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 m_OverviewPos = new Vector3(113f, 106f, -87f);

	// Token: 0x040080C5 RID: 32965
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 m_OutroPos = new Vector3(135f, 105.8f, -70.7f);

	// Token: 0x040080C6 RID: 32966
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector2 m_StartAngle = new Vector2(13f, 153.5f);

	// Token: 0x040080C7 RID: 32967
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector2 m_OverviewAngle = new Vector2(13f, 153.5f);

	// Token: 0x040080C8 RID: 32968
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector2 m_OutroAngle = new Vector2(-19.3f, 241.7f);

	// Token: 0x040080C9 RID: 32969
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float m_OutroStartTime;

	// Token: 0x040080CA RID: 32970
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool m_LoadingNextLevel;
}

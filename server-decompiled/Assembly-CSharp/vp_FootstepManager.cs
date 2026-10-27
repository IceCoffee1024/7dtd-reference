using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200160C RID: 5644
public class vp_FootstepManager : MonoBehaviour
{
	// Token: 0x1700147B RID: 5243
	// (get) Token: 0x0600AFAD RID: 44973 RVA: 0x00418247 File Offset: 0x00416447
	public static vp_FootstepManager[] FootstepManagers
	{
		get
		{
			if (vp_FootstepManager.mIsDirty)
			{
				vp_FootstepManager.mIsDirty = false;
				vp_FootstepManager.m_FootstepManagers = global::UnityEngine.Object.FindObjectsByType<vp_FootstepManager>(FindObjectsSortMode.None);
				if (vp_FootstepManager.m_FootstepManagers == null)
				{
					vp_FootstepManager.m_FootstepManagers = Resources.FindObjectsOfTypeAll(typeof(vp_FootstepManager)) as vp_FootstepManager[];
				}
			}
			return vp_FootstepManager.m_FootstepManagers;
		}
	}

	// Token: 0x1700147C RID: 5244
	// (get) Token: 0x0600AFAE RID: 44974 RVA: 0x00418286 File Offset: 0x00416486
	public bool IsDirty
	{
		get
		{
			return vp_FootstepManager.mIsDirty;
		}
	}

	// Token: 0x0600AFAF RID: 44975 RVA: 0x00418290 File Offset: 0x00416490
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Awake()
	{
		this.m_Player = base.transform.root.GetComponentInChildren<vp_FPPlayerEventHandler>();
		this.m_Camera = base.transform.root.GetComponentInChildren<vp_FPCamera>();
		this.m_Controller = base.transform.root.GetComponentInChildren<vp_FPController>();
		this.m_Audio = base.gameObject.AddComponent<AudioSource>();
	}

	// Token: 0x0600AFB0 RID: 44976 RVA: 0x004182F0 File Offset: 0x004164F0
	public virtual void SetDirty(bool dirty)
	{
		vp_FootstepManager.mIsDirty = dirty;
	}

	// Token: 0x0600AFB1 RID: 44977 RVA: 0x004182F8 File Offset: 0x004164F8
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update()
	{
		if (this.m_Camera.BobStepCallback == null)
		{
			vp_FPCamera camera = this.m_Camera;
			camera.BobStepCallback = (vp_FPCamera.BobStepDelegate)Delegate.Combine(camera.BobStepCallback, new vp_FPCamera.BobStepDelegate(this.Footstep));
		}
	}

	// Token: 0x0600AFB2 RID: 44978 RVA: 0x0041832F File Offset: 0x0041652F
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		vp_FPCamera camera = this.m_Camera;
		camera.BobStepCallback = (vp_FPCamera.BobStepDelegate)Delegate.Combine(camera.BobStepCallback, new vp_FPCamera.BobStepDelegate(this.Footstep));
	}

	// Token: 0x0600AFB3 RID: 44979 RVA: 0x00418359 File Offset: 0x00416559
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDisable()
	{
		vp_FPCamera camera = this.m_Camera;
		camera.BobStepCallback = (vp_FPCamera.BobStepDelegate)Delegate.Remove(camera.BobStepCallback, new vp_FPCamera.BobStepDelegate(this.Footstep));
	}

	// Token: 0x0600AFB4 RID: 44980 RVA: 0x00418384 File Offset: 0x00416584
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Footstep()
	{
		if (!this.m_Controller.Grounded)
		{
			return;
		}
		if (this.m_Player.GroundTexture.Get() == null && this.m_Player.SurfaceType.Get() == null)
		{
			return;
		}
		if (this.m_Player.SurfaceType.Get() != null)
		{
			this.PlaySound(this.SurfaceTypes[this.m_Player.SurfaceType.Get().SurfaceID]);
			return;
		}
		foreach (vp_FootstepManager.vp_SurfaceTypes vp_SurfaceTypes in this.SurfaceTypes)
		{
			using (List<Texture>.Enumerator enumerator2 = vp_SurfaceTypes.Textures.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					if (enumerator2.Current == this.m_Player.GroundTexture.Get())
					{
						this.PlaySound(vp_SurfaceTypes);
						break;
					}
				}
			}
		}
	}

	// Token: 0x0600AFB5 RID: 44981 RVA: 0x004184C4 File Offset: 0x004166C4
	public virtual void PlaySound(vp_FootstepManager.vp_SurfaceTypes st)
	{
		if (st.Sounds == null || st.Sounds.Count == 0)
		{
			return;
		}
		for (;;)
		{
			this.m_SoundToPlay = st.Sounds[global::UnityEngine.Random.Range(0, st.Sounds.Count)];
			if (this.m_SoundToPlay == null)
			{
				break;
			}
			if (!(this.m_SoundToPlay == this.m_LastPlayedSound) || st.Sounds.Count <= 1)
			{
				goto IL_0068;
			}
		}
		return;
		IL_0068:
		this.m_Audio.pitch = global::UnityEngine.Random.Range(st.RandomPitch.x, st.RandomPitch.y) * Time.timeScale;
		this.m_Audio.clip = this.m_SoundToPlay;
		this.m_Audio.Play();
		this.m_LastPlayedSound = this.m_SoundToPlay;
	}

	// Token: 0x0600AFB6 RID: 44982 RVA: 0x00418590 File Offset: 0x00416790
	public static int GetMainTerrainTexture(Vector3 worldPos, Terrain terrain)
	{
		TerrainData terrainData = terrain.terrainData;
		Vector3 position = terrain.transform.position;
		int num = (int)((worldPos.x - position.x) / terrainData.size.x * (float)terrainData.alphamapWidth);
		int num2 = (int)((worldPos.z - position.z) / terrainData.size.z * (float)terrainData.alphamapHeight);
		float[,,] alphamaps = terrainData.GetAlphamaps(num, num2, 1, 1);
		float[] array = new float[alphamaps.GetUpperBound(2) + 1];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = alphamaps[0, 0, i];
		}
		float num3 = 0f;
		int num4 = 0;
		for (int j = 0; j < array.Length; j++)
		{
			if (array[j] > num3)
			{
				num4 = j;
				num3 = array[j];
			}
		}
		return num4;
	}

	// Token: 0x04008301 RID: 33537
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static vp_FootstepManager[] m_FootstepManagers;

	// Token: 0x04008302 RID: 33538
	public static bool mIsDirty = true;

	// Token: 0x04008303 RID: 33539
	public List<vp_FootstepManager.vp_SurfaceTypes> SurfaceTypes = new List<vp_FootstepManager.vp_SurfaceTypes>();

	// Token: 0x04008304 RID: 33540
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_FPPlayerEventHandler m_Player;

	// Token: 0x04008305 RID: 33541
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_FPCamera m_Camera;

	// Token: 0x04008306 RID: 33542
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_FPController m_Controller;

	// Token: 0x04008307 RID: 33543
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public AudioSource m_Audio;

	// Token: 0x04008308 RID: 33544
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public AudioClip m_SoundToPlay;

	// Token: 0x04008309 RID: 33545
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public AudioClip m_LastPlayedSound;

	// Token: 0x0200160D RID: 5645
	[Serializable]
	public class vp_SurfaceTypes
	{
		// Token: 0x0400830A RID: 33546
		public Vector2 RandomPitch = new Vector2(1f, 1.5f);

		// Token: 0x0400830B RID: 33547
		public bool Foldout = true;

		// Token: 0x0400830C RID: 33548
		public bool SoundsFoldout = true;

		// Token: 0x0400830D RID: 33549
		public bool TexturesFoldout = true;

		// Token: 0x0400830E RID: 33550
		public string SurfaceName = "";

		// Token: 0x0400830F RID: 33551
		public List<AudioClip> Sounds = new List<AudioClip>();

		// Token: 0x04008310 RID: 33552
		public List<Texture> Textures = new List<Texture>();
	}
}

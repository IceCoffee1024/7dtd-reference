using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020015FA RID: 5626
public class vp_FootstepManager : MonoBehaviour
{
	// Token: 0x1700146F RID: 5231
	// (get) Token: 0x0600AF55 RID: 44885 RVA: 0x00415E43 File Offset: 0x00414043
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

	// Token: 0x17001470 RID: 5232
	// (get) Token: 0x0600AF56 RID: 44886 RVA: 0x00415E82 File Offset: 0x00414082
	public bool IsDirty
	{
		get
		{
			return vp_FootstepManager.mIsDirty;
		}
	}

	// Token: 0x0600AF57 RID: 44887 RVA: 0x00415E8C File Offset: 0x0041408C
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Awake()
	{
		this.m_Player = base.transform.root.GetComponentInChildren<vp_FPPlayerEventHandler>();
		this.m_Camera = base.transform.root.GetComponentInChildren<vp_FPCamera>();
		this.m_Controller = base.transform.root.GetComponentInChildren<vp_FPController>();
		this.m_Audio = base.gameObject.AddComponent<AudioSource>();
	}

	// Token: 0x0600AF58 RID: 44888 RVA: 0x00415EEC File Offset: 0x004140EC
	public virtual void SetDirty(bool dirty)
	{
		vp_FootstepManager.mIsDirty = dirty;
	}

	// Token: 0x0600AF59 RID: 44889 RVA: 0x00415EF4 File Offset: 0x004140F4
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update()
	{
		if (this.m_Camera.BobStepCallback == null)
		{
			vp_FPCamera camera = this.m_Camera;
			camera.BobStepCallback = (vp_FPCamera.BobStepDelegate)Delegate.Combine(camera.BobStepCallback, new vp_FPCamera.BobStepDelegate(this.Footstep));
		}
	}

	// Token: 0x0600AF5A RID: 44890 RVA: 0x00415F2B File Offset: 0x0041412B
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		vp_FPCamera camera = this.m_Camera;
		camera.BobStepCallback = (vp_FPCamera.BobStepDelegate)Delegate.Combine(camera.BobStepCallback, new vp_FPCamera.BobStepDelegate(this.Footstep));
	}

	// Token: 0x0600AF5B RID: 44891 RVA: 0x00415F55 File Offset: 0x00414155
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDisable()
	{
		vp_FPCamera camera = this.m_Camera;
		camera.BobStepCallback = (vp_FPCamera.BobStepDelegate)Delegate.Remove(camera.BobStepCallback, new vp_FPCamera.BobStepDelegate(this.Footstep));
	}

	// Token: 0x0600AF5C RID: 44892 RVA: 0x00415F80 File Offset: 0x00414180
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

	// Token: 0x0600AF5D RID: 44893 RVA: 0x004160C0 File Offset: 0x004142C0
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

	// Token: 0x0600AF5E RID: 44894 RVA: 0x0041618C File Offset: 0x0041438C
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

	// Token: 0x040082C0 RID: 33472
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static vp_FootstepManager[] m_FootstepManagers;

	// Token: 0x040082C1 RID: 33473
	public static bool mIsDirty = true;

	// Token: 0x040082C2 RID: 33474
	public List<vp_FootstepManager.vp_SurfaceTypes> SurfaceTypes = new List<vp_FootstepManager.vp_SurfaceTypes>();

	// Token: 0x040082C3 RID: 33475
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_FPPlayerEventHandler m_Player;

	// Token: 0x040082C4 RID: 33476
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_FPCamera m_Camera;

	// Token: 0x040082C5 RID: 33477
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_FPController m_Controller;

	// Token: 0x040082C6 RID: 33478
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public AudioSource m_Audio;

	// Token: 0x040082C7 RID: 33479
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public AudioClip m_SoundToPlay;

	// Token: 0x040082C8 RID: 33480
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public AudioClip m_LastPlayedSound;

	// Token: 0x020015FB RID: 5627
	[Serializable]
	public class vp_SurfaceTypes
	{
		// Token: 0x040082C9 RID: 33481
		public Vector2 RandomPitch = new Vector2(1f, 1.5f);

		// Token: 0x040082CA RID: 33482
		public bool Foldout = true;

		// Token: 0x040082CB RID: 33483
		public bool SoundsFoldout = true;

		// Token: 0x040082CC RID: 33484
		public bool TexturesFoldout = true;

		// Token: 0x040082CD RID: 33485
		public string SurfaceName = "";

		// Token: 0x040082CE RID: 33486
		public List<AudioClip> Sounds = new List<AudioClip>();

		// Token: 0x040082CF RID: 33487
		public List<Texture> Textures = new List<Texture>();
	}
}

using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020015D8 RID: 5592
public class vp_FootstepManager : MonoBehaviour
{
	// Token: 0x1700142A RID: 5162
	// (get) Token: 0x0600AEA2 RID: 44706 RVA: 0x00410FEF File Offset: 0x0040F1EF
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

	// Token: 0x1700142B RID: 5163
	// (get) Token: 0x0600AEA3 RID: 44707 RVA: 0x0041102E File Offset: 0x0040F22E
	public bool IsDirty
	{
		get
		{
			return vp_FootstepManager.mIsDirty;
		}
	}

	// Token: 0x0600AEA4 RID: 44708 RVA: 0x00411038 File Offset: 0x0040F238
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Awake()
	{
		this.m_Player = base.transform.root.GetComponentInChildren<vp_FPPlayerEventHandler>();
		this.m_Camera = base.transform.root.GetComponentInChildren<vp_FPCamera>();
		this.m_Controller = base.transform.root.GetComponentInChildren<vp_FPController>();
		this.m_Audio = base.gameObject.AddComponent<AudioSource>();
	}

	// Token: 0x0600AEA5 RID: 44709 RVA: 0x00411098 File Offset: 0x0040F298
	public virtual void SetDirty(bool dirty)
	{
		vp_FootstepManager.mIsDirty = dirty;
	}

	// Token: 0x0600AEA6 RID: 44710 RVA: 0x004110A0 File Offset: 0x0040F2A0
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update()
	{
		if (this.m_Camera.BobStepCallback == null)
		{
			vp_FPCamera camera = this.m_Camera;
			camera.BobStepCallback = (vp_FPCamera.BobStepDelegate)Delegate.Combine(camera.BobStepCallback, new vp_FPCamera.BobStepDelegate(this.Footstep));
		}
	}

	// Token: 0x0600AEA7 RID: 44711 RVA: 0x004110D7 File Offset: 0x0040F2D7
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		vp_FPCamera camera = this.m_Camera;
		camera.BobStepCallback = (vp_FPCamera.BobStepDelegate)Delegate.Combine(camera.BobStepCallback, new vp_FPCamera.BobStepDelegate(this.Footstep));
	}

	// Token: 0x0600AEA8 RID: 44712 RVA: 0x00411101 File Offset: 0x0040F301
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDisable()
	{
		vp_FPCamera camera = this.m_Camera;
		camera.BobStepCallback = (vp_FPCamera.BobStepDelegate)Delegate.Remove(camera.BobStepCallback, new vp_FPCamera.BobStepDelegate(this.Footstep));
	}

	// Token: 0x0600AEA9 RID: 44713 RVA: 0x0041112C File Offset: 0x0040F32C
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

	// Token: 0x0600AEAA RID: 44714 RVA: 0x0041126C File Offset: 0x0040F46C
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

	// Token: 0x0600AEAB RID: 44715 RVA: 0x00411338 File Offset: 0x0040F538
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

	// Token: 0x0400823D RID: 33341
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static vp_FootstepManager[] m_FootstepManagers;

	// Token: 0x0400823E RID: 33342
	public static bool mIsDirty = true;

	// Token: 0x0400823F RID: 33343
	public List<vp_FootstepManager.vp_SurfaceTypes> SurfaceTypes = new List<vp_FootstepManager.vp_SurfaceTypes>();

	// Token: 0x04008240 RID: 33344
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_FPPlayerEventHandler m_Player;

	// Token: 0x04008241 RID: 33345
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_FPCamera m_Camera;

	// Token: 0x04008242 RID: 33346
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_FPController m_Controller;

	// Token: 0x04008243 RID: 33347
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public AudioSource m_Audio;

	// Token: 0x04008244 RID: 33348
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public AudioClip m_SoundToPlay;

	// Token: 0x04008245 RID: 33349
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public AudioClip m_LastPlayedSound;

	// Token: 0x020015D9 RID: 5593
	[Serializable]
	public class vp_SurfaceTypes
	{
		// Token: 0x04008246 RID: 33350
		public Vector2 RandomPitch = new Vector2(1f, 1.5f);

		// Token: 0x04008247 RID: 33351
		public bool Foldout = true;

		// Token: 0x04008248 RID: 33352
		public bool SoundsFoldout = true;

		// Token: 0x04008249 RID: 33353
		public bool TexturesFoldout = true;

		// Token: 0x0400824A RID: 33354
		public string SurfaceName = "";

		// Token: 0x0400824B RID: 33355
		public List<AudioClip> Sounds = new List<AudioClip>();

		// Token: 0x0400824C RID: 33356
		public List<Texture> Textures = new List<Texture>();
	}
}

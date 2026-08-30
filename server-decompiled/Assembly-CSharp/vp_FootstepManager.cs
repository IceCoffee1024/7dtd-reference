using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020015BD RID: 5565
public class vp_FootstepManager : MonoBehaviour
{
	// Token: 0x170013F1 RID: 5105
	// (get) Token: 0x0600ADA5 RID: 44453 RVA: 0x0040BCAF File Offset: 0x00409EAF
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

	// Token: 0x170013F2 RID: 5106
	// (get) Token: 0x0600ADA6 RID: 44454 RVA: 0x0040BCEE File Offset: 0x00409EEE
	public bool IsDirty
	{
		get
		{
			return vp_FootstepManager.mIsDirty;
		}
	}

	// Token: 0x0600ADA7 RID: 44455 RVA: 0x0040BCF8 File Offset: 0x00409EF8
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Awake()
	{
		this.m_Player = base.transform.root.GetComponentInChildren<vp_FPPlayerEventHandler>();
		this.m_Camera = base.transform.root.GetComponentInChildren<vp_FPCamera>();
		this.m_Controller = base.transform.root.GetComponentInChildren<vp_FPController>();
		this.m_Audio = base.gameObject.AddComponent<AudioSource>();
	}

	// Token: 0x0600ADA8 RID: 44456 RVA: 0x0040BD58 File Offset: 0x00409F58
	public virtual void SetDirty(bool dirty)
	{
		vp_FootstepManager.mIsDirty = dirty;
	}

	// Token: 0x0600ADA9 RID: 44457 RVA: 0x0040BD60 File Offset: 0x00409F60
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update()
	{
		if (this.m_Camera.BobStepCallback == null)
		{
			vp_FPCamera camera = this.m_Camera;
			camera.BobStepCallback = (vp_FPCamera.BobStepDelegate)Delegate.Combine(camera.BobStepCallback, new vp_FPCamera.BobStepDelegate(this.Footstep));
		}
	}

	// Token: 0x0600ADAA RID: 44458 RVA: 0x0040BD97 File Offset: 0x00409F97
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		vp_FPCamera camera = this.m_Camera;
		camera.BobStepCallback = (vp_FPCamera.BobStepDelegate)Delegate.Combine(camera.BobStepCallback, new vp_FPCamera.BobStepDelegate(this.Footstep));
	}

	// Token: 0x0600ADAB RID: 44459 RVA: 0x0040BDC1 File Offset: 0x00409FC1
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDisable()
	{
		vp_FPCamera camera = this.m_Camera;
		camera.BobStepCallback = (vp_FPCamera.BobStepDelegate)Delegate.Remove(camera.BobStepCallback, new vp_FPCamera.BobStepDelegate(this.Footstep));
	}

	// Token: 0x0600ADAC RID: 44460 RVA: 0x0040BDEC File Offset: 0x00409FEC
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

	// Token: 0x0600ADAD RID: 44461 RVA: 0x0040BF2C File Offset: 0x0040A12C
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

	// Token: 0x0600ADAE RID: 44462 RVA: 0x0040BFF8 File Offset: 0x0040A1F8
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

	// Token: 0x04008164 RID: 33124
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static vp_FootstepManager[] m_FootstepManagers;

	// Token: 0x04008165 RID: 33125
	public static bool mIsDirty = true;

	// Token: 0x04008166 RID: 33126
	public List<vp_FootstepManager.vp_SurfaceTypes> SurfaceTypes = new List<vp_FootstepManager.vp_SurfaceTypes>();

	// Token: 0x04008167 RID: 33127
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_FPPlayerEventHandler m_Player;

	// Token: 0x04008168 RID: 33128
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_FPCamera m_Camera;

	// Token: 0x04008169 RID: 33129
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_FPController m_Controller;

	// Token: 0x0400816A RID: 33130
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public AudioSource m_Audio;

	// Token: 0x0400816B RID: 33131
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public AudioClip m_SoundToPlay;

	// Token: 0x0400816C RID: 33132
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public AudioClip m_LastPlayedSound;

	// Token: 0x020015BE RID: 5566
	[Serializable]
	public class vp_SurfaceTypes
	{
		// Token: 0x0400816D RID: 33133
		public Vector2 RandomPitch = new Vector2(1f, 1.5f);

		// Token: 0x0400816E RID: 33134
		public bool Foldout = true;

		// Token: 0x0400816F RID: 33135
		public bool SoundsFoldout = true;

		// Token: 0x04008170 RID: 33136
		public bool TexturesFoldout = true;

		// Token: 0x04008171 RID: 33137
		public string SurfaceName = "";

		// Token: 0x04008172 RID: 33138
		public List<AudioClip> Sounds = new List<AudioClip>();

		// Token: 0x04008173 RID: 33139
		public List<Texture> Textures = new List<Texture>();
	}
}

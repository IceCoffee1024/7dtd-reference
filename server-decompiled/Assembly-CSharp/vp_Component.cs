using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200151C RID: 5404
[Preserve]
public class vp_Component : MonoBehaviour
{
	// Token: 0x17001391 RID: 5009
	// (get) Token: 0x0600A976 RID: 43382 RVA: 0x003F4310 File Offset: 0x003F2510
	public vp_EventHandler EventHandler
	{
		get
		{
			if (this.m_EventHandler == null)
			{
				this.m_EventHandler = (vp_EventHandler)this.Transform.GetComponentInChildren(typeof(vp_EventHandler));
			}
			if (this.m_EventHandler == null && this.Transform.parent != null)
			{
				this.m_EventHandler = (vp_EventHandler)this.Transform.parent.GetComponentInChildren(typeof(vp_EventHandler));
			}
			if (this.m_EventHandler == null && this.Transform.parent != null && this.Transform.parent.parent != null)
			{
				this.m_EventHandler = (vp_EventHandler)this.Transform.parent.parent.GetComponentInChildren(typeof(vp_EventHandler));
			}
			if (this.m_EventHandler == null)
			{
				this.m_EventHandler = (vp_EventHandler)this.Transform.GetComponentInParent(typeof(vp_EventHandler));
			}
			return this.m_EventHandler;
		}
	}

	// Token: 0x17001392 RID: 5010
	// (get) Token: 0x0600A977 RID: 43383 RVA: 0x003F4428 File Offset: 0x003F2628
	public Type Type
	{
		get
		{
			if (this.m_Type == null)
			{
				this.m_Type = base.GetType();
			}
			return this.m_Type;
		}
	}

	// Token: 0x17001393 RID: 5011
	// (get) Token: 0x0600A978 RID: 43384 RVA: 0x003F444A File Offset: 0x003F264A
	public FieldInfo[] Fields
	{
		get
		{
			if (this.m_Fields == null)
			{
				this.m_Fields = this.Type.GetFields(BindingFlags.Instance | BindingFlags.Public);
			}
			return this.m_Fields;
		}
	}

	// Token: 0x17001394 RID: 5012
	// (get) Token: 0x0600A979 RID: 43385 RVA: 0x003F446D File Offset: 0x003F266D
	public vp_StateManager StateManager
	{
		get
		{
			if (this.m_StateManager == null)
			{
				this.m_StateManager = new vp_StateManager(this, this.States);
			}
			return this.m_StateManager;
		}
	}

	// Token: 0x17001395 RID: 5013
	// (get) Token: 0x0600A97A RID: 43386 RVA: 0x003F448F File Offset: 0x003F268F
	public vp_State DefaultState
	{
		get
		{
			return this.m_DefaultState;
		}
	}

	// Token: 0x17001396 RID: 5014
	// (get) Token: 0x0600A97B RID: 43387 RVA: 0x003F4497 File Offset: 0x003F2697
	public float Delta
	{
		get
		{
			return Time.deltaTime * 60f;
		}
	}

	// Token: 0x17001397 RID: 5015
	// (get) Token: 0x0600A97C RID: 43388 RVA: 0x003F44A4 File Offset: 0x003F26A4
	public float SDelta
	{
		get
		{
			return Time.smoothDeltaTime * 60f;
		}
	}

	// Token: 0x17001398 RID: 5016
	// (get) Token: 0x0600A97D RID: 43389 RVA: 0x003F44B1 File Offset: 0x003F26B1
	public Transform Transform
	{
		get
		{
			if (this.m_Transform == null)
			{
				this.m_Transform = base.transform;
			}
			return this.m_Transform;
		}
	}

	// Token: 0x17001399 RID: 5017
	// (get) Token: 0x0600A97E RID: 43390 RVA: 0x003F44D3 File Offset: 0x003F26D3
	public Transform Parent
	{
		get
		{
			if (this.m_Parent == null)
			{
				this.m_Parent = base.transform.parent;
			}
			return this.m_Parent;
		}
	}

	// Token: 0x1700139A RID: 5018
	// (get) Token: 0x0600A97F RID: 43391 RVA: 0x003F44FA File Offset: 0x003F26FA
	public Transform Root
	{
		get
		{
			if (this.m_Root == null)
			{
				this.m_Root = base.transform.root;
			}
			return this.m_Root;
		}
	}

	// Token: 0x1700139B RID: 5019
	// (get) Token: 0x0600A980 RID: 43392 RVA: 0x003F4521 File Offset: 0x003F2721
	public AudioSource Audio
	{
		get
		{
			if (this.m_Audio == null)
			{
				this.m_Audio = base.GetComponent<AudioSource>();
			}
			return this.m_Audio;
		}
	}

	// Token: 0x1700139C RID: 5020
	// (get) Token: 0x0600A981 RID: 43393 RVA: 0x003F4543 File Offset: 0x003F2743
	public Collider Collider
	{
		get
		{
			if (this.m_Collider == null)
			{
				this.m_Collider = base.GetComponent<Collider>();
			}
			return this.m_Collider;
		}
	}

	// Token: 0x1700139D RID: 5021
	// (get) Token: 0x0600A982 RID: 43394 RVA: 0x003F4568 File Offset: 0x003F2768
	// (set) Token: 0x0600A983 RID: 43395 RVA: 0x003F45D4 File Offset: 0x003F27D4
	public bool Rendering
	{
		get
		{
			if (this.Renderers != null)
			{
				foreach (Renderer renderer in this.Renderers)
				{
					if (renderer != null && renderer.enabled)
					{
						return true;
					}
				}
				return false;
			}
			return false;
		}
		set
		{
			foreach (Renderer renderer in this.Renderers)
			{
				if (!(renderer == null))
				{
					renderer.enabled = value;
				}
			}
		}
	}

	// Token: 0x0600A984 RID: 43396 RVA: 0x003F4630 File Offset: 0x003F2830
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Awake()
	{
		this.CacheChildren();
		this.CacheSiblings();
		this.CacheFamily();
		this.CacheRenderers();
		this.CacheAudioSources();
		this.StateManager.SetState("Default", base.enabled);
	}

	// Token: 0x0600A985 RID: 43397 RVA: 0x003F4666 File Offset: 0x003F2866
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Start()
	{
		this.ResetState();
	}

	// Token: 0x0600A986 RID: 43398 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Init()
	{
	}

	// Token: 0x0600A987 RID: 43399 RVA: 0x003F466E File Offset: 0x003F286E
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		if (this.EventHandler != null)
		{
			this.EventHandler.Register(this);
		}
	}

	// Token: 0x0600A988 RID: 43400 RVA: 0x003F468A File Offset: 0x003F288A
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDisable()
	{
		if (this.EventHandler != null)
		{
			this.EventHandler.Unregister(this);
		}
	}

	// Token: 0x0600A989 RID: 43401 RVA: 0x003F46A6 File Offset: 0x003F28A6
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Update()
	{
		if (!this.m_Initialized)
		{
			this.Init();
			this.m_Initialized = true;
		}
	}

	// Token: 0x0600A98A RID: 43402 RVA: 0x003F46C0 File Offset: 0x003F28C0
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void FixedUpdate()
	{
		if (GameManager.Instance == null)
		{
			return;
		}
		if (GameManager.Instance.World == null)
		{
			return;
		}
		if (GameManager.Instance.World.GetPrimaryPlayer() == null)
		{
			return;
		}
		Color color = Color.white;
		color.a = 1f;
		if (color == Color.black)
		{
			color.a = 1f;
			if (color == Color.black)
			{
				color = Color.gray;
			}
		}
		if (this.prevSkinColor == color)
		{
			return;
		}
		this.prevSkinColor = color;
		foreach (Renderer renderer in this.Renderers)
		{
			if (!(renderer == null) && renderer.enabled)
			{
				renderer.material.SetColor("Tint", color);
			}
		}
	}

	// Token: 0x0600A98B RID: 43403 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void LateUpdate()
	{
	}

	// Token: 0x0600A98C RID: 43404 RVA: 0x003F47B8 File Offset: 0x003F29B8
	public void SetState(string state, bool enabled = true, bool recursive = false, bool includeDisabled = false)
	{
		this.StateManager.SetState(state, enabled);
		if (recursive)
		{
			foreach (vp_Component vp_Component in this.Children)
			{
				if (includeDisabled || (vp_Utility.IsActive(vp_Component.gameObject) && vp_Component.enabled))
				{
					vp_Component.SetState(state, enabled, true, includeDisabled);
				}
			}
		}
	}

	// Token: 0x0600A98D RID: 43405 RVA: 0x003F4838 File Offset: 0x003F2A38
	public void ActivateGameObject(bool setActive = true)
	{
		if (setActive)
		{
			this.Activate();
			foreach (vp_Component vp_Component in this.Siblings)
			{
				vp_Component.Activate();
			}
			this.VerifyRenderers();
			return;
		}
		this.DeactivateWhenSilent();
		foreach (vp_Component vp_Component2 in this.Siblings)
		{
			vp_Component2.DeactivateWhenSilent();
		}
	}

	// Token: 0x0600A98E RID: 43406 RVA: 0x003F48E0 File Offset: 0x003F2AE0
	public void ResetState()
	{
		this.StateManager.Reset();
		this.Refresh();
	}

	// Token: 0x0600A98F RID: 43407 RVA: 0x003F48F3 File Offset: 0x003F2AF3
	public bool StateEnabled(string stateName)
	{
		return this.StateManager.IsEnabled(stateName);
	}

	// Token: 0x0600A990 RID: 43408 RVA: 0x003F4904 File Offset: 0x003F2B04
	public void RefreshDefaultState()
	{
		vp_State vp_State = null;
		if (this.States.Count == 0)
		{
			vp_State = new vp_State(this.Type.Name, "Default", null, null);
			this.States.Add(vp_State);
		}
		else
		{
			for (int i = this.States.Count - 1; i > -1; i--)
			{
				if (this.States[i].Name == "Default")
				{
					vp_State = this.States[i];
					this.States.Remove(vp_State);
					this.States.Add(vp_State);
				}
			}
			if (vp_State == null)
			{
				vp_State = new vp_State(this.Type.Name, "Default", null, null);
				this.States.Add(vp_State);
			}
		}
		if (vp_State.Preset == null || vp_State.Preset.ComponentType == null)
		{
			vp_State.Preset = new vp_ComponentPreset();
		}
		if (vp_State.TextAsset == null)
		{
			vp_State.Preset.InitFromComponent(this);
		}
		vp_State.Enabled = true;
		this.m_DefaultState = vp_State;
	}

	// Token: 0x0600A991 RID: 43409 RVA: 0x003F4A19 File Offset: 0x003F2C19
	public void ApplyPreset(vp_ComponentPreset preset)
	{
		vp_ComponentPreset.Apply(this, preset);
		this.RefreshDefaultState();
		this.Refresh();
	}

	// Token: 0x0600A992 RID: 43410 RVA: 0x003F4A2F File Offset: 0x003F2C2F
	public vp_ComponentPreset Load(string path)
	{
		vp_ComponentPreset vp_ComponentPreset = vp_ComponentPreset.LoadFromResources(this, path);
		this.RefreshDefaultState();
		this.Refresh();
		return vp_ComponentPreset;
	}

	// Token: 0x0600A993 RID: 43411 RVA: 0x003F4A44 File Offset: 0x003F2C44
	public vp_ComponentPreset Load(TextAsset asset)
	{
		vp_ComponentPreset vp_ComponentPreset = vp_ComponentPreset.LoadFromTextAsset(this, asset);
		this.RefreshDefaultState();
		this.Refresh();
		return vp_ComponentPreset;
	}

	// Token: 0x0600A994 RID: 43412 RVA: 0x003F4A5C File Offset: 0x003F2C5C
	public void CacheChildren()
	{
		this.Children.Clear();
		foreach (vp_Component vp_Component in base.GetComponentsInChildren<vp_Component>(true))
		{
			if (vp_Component.transform.parent == base.transform)
			{
				this.Children.Add(vp_Component);
			}
		}
	}

	// Token: 0x0600A995 RID: 43413 RVA: 0x003F4AB4 File Offset: 0x003F2CB4
	public void CacheSiblings()
	{
		this.Siblings.Clear();
		foreach (vp_Component vp_Component in base.GetComponents<vp_Component>())
		{
			if (vp_Component != this)
			{
				this.Siblings.Add(vp_Component);
			}
		}
	}

	// Token: 0x0600A996 RID: 43414 RVA: 0x003F4AFC File Offset: 0x003F2CFC
	public void CacheFamily()
	{
		this.Family.Clear();
		foreach (vp_Component vp_Component in base.transform.root.GetComponentsInChildren<vp_Component>(true))
		{
			if (vp_Component != this)
			{
				this.Family.Add(vp_Component);
			}
		}
	}

	// Token: 0x0600A997 RID: 43415 RVA: 0x003F4B50 File Offset: 0x003F2D50
	public void CacheRenderers()
	{
		this.Renderers.Clear();
		foreach (Renderer renderer in base.GetComponentsInChildren<Renderer>(true))
		{
			this.Renderers.Add(renderer);
		}
	}

	// Token: 0x0600A998 RID: 43416 RVA: 0x003F4B90 File Offset: 0x003F2D90
	[PublicizedFrom(EAccessModifier.Protected)]
	public void VerifyRenderers()
	{
		if (this.Renderers.Count == 0)
		{
			return;
		}
		if (this.Renderers[0] == null || !vp_Utility.IsDescendant(this.Renderers[0].transform, this.Transform))
		{
			this.Renderers.Clear();
			this.CacheRenderers();
		}
	}

	// Token: 0x0600A999 RID: 43417 RVA: 0x003F4BF0 File Offset: 0x003F2DF0
	public void CacheAudioSources()
	{
		this.AudioSources.Clear();
		foreach (AudioSource audioSource in base.GetComponentsInChildren<AudioSource>(true))
		{
			this.AudioSources.Add(audioSource);
		}
	}

	// Token: 0x0600A99A RID: 43418 RVA: 0x003F4C2E File Offset: 0x003F2E2E
	public virtual void Activate()
	{
		this.m_DeactivationTimer.Cancel();
		vp_Utility.Activate(base.gameObject, true);
	}

	// Token: 0x0600A99B RID: 43419 RVA: 0x003F4C47 File Offset: 0x003F2E47
	public virtual void Deactivate()
	{
		vp_Utility.Activate(base.gameObject, false);
	}

	// Token: 0x0600A99C RID: 43420 RVA: 0x003F4C58 File Offset: 0x003F2E58
	public void DeactivateWhenSilent()
	{
		if (this == null)
		{
			return;
		}
		if (vp_Utility.IsActive(base.gameObject))
		{
			foreach (AudioSource audioSource in this.AudioSources)
			{
				if (audioSource.isPlaying && !audioSource.loop)
				{
					this.Rendering = false;
					vp_Timer.In(0.1f, delegate
					{
						this.DeactivateWhenSilent();
					}, this.m_DeactivationTimer);
					return;
				}
			}
		}
		this.Deactivate();
	}

	// Token: 0x0600A99D RID: 43421 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void Refresh()
	{
	}

	// Token: 0x04007DE4 RID: 32228
	public bool Persist;

	// Token: 0x04007DE5 RID: 32229
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_StateManager m_StateManager;

	// Token: 0x04007DE6 RID: 32230
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_EventHandler m_EventHandler;

	// Token: 0x04007DE7 RID: 32231
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_State m_DefaultState;

	// Token: 0x04007DE8 RID: 32232
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_Initialized;

	// Token: 0x04007DE9 RID: 32233
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Transform;

	// Token: 0x04007DEA RID: 32234
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Parent;

	// Token: 0x04007DEB RID: 32235
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Root;

	// Token: 0x04007DEC RID: 32236
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public AudioSource m_Audio;

	// Token: 0x04007DED RID: 32237
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Collider m_Collider;

	// Token: 0x04007DEE RID: 32238
	public List<vp_State> States = new List<vp_State>();

	// Token: 0x04007DEF RID: 32239
	public List<vp_Component> Children = new List<vp_Component>();

	// Token: 0x04007DF0 RID: 32240
	public List<vp_Component> Siblings = new List<vp_Component>();

	// Token: 0x04007DF1 RID: 32241
	public List<vp_Component> Family = new List<vp_Component>();

	// Token: 0x04007DF2 RID: 32242
	public List<Renderer> Renderers = new List<Renderer>();

	// Token: 0x04007DF3 RID: 32243
	public List<AudioSource> AudioSources = new List<AudioSource>();

	// Token: 0x04007DF4 RID: 32244
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Type m_Type;

	// Token: 0x04007DF5 RID: 32245
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public FieldInfo[] m_Fields;

	// Token: 0x04007DF6 RID: 32246
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Timer.Handle m_DeactivationTimer = new vp_Timer.Handle();

	// Token: 0x04007DF7 RID: 32247
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Color prevSkinColor = Color.white;
}

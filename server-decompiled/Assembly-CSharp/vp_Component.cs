using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001537 RID: 5431
[Preserve]
public class vp_Component : MonoBehaviour
{
	// Token: 0x170013CA RID: 5066
	// (get) Token: 0x0600AA73 RID: 43635 RVA: 0x003F9650 File Offset: 0x003F7850
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

	// Token: 0x170013CB RID: 5067
	// (get) Token: 0x0600AA74 RID: 43636 RVA: 0x003F9768 File Offset: 0x003F7968
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

	// Token: 0x170013CC RID: 5068
	// (get) Token: 0x0600AA75 RID: 43637 RVA: 0x003F978A File Offset: 0x003F798A
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

	// Token: 0x170013CD RID: 5069
	// (get) Token: 0x0600AA76 RID: 43638 RVA: 0x003F97AD File Offset: 0x003F79AD
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

	// Token: 0x170013CE RID: 5070
	// (get) Token: 0x0600AA77 RID: 43639 RVA: 0x003F97CF File Offset: 0x003F79CF
	public vp_State DefaultState
	{
		get
		{
			return this.m_DefaultState;
		}
	}

	// Token: 0x170013CF RID: 5071
	// (get) Token: 0x0600AA78 RID: 43640 RVA: 0x003F97D7 File Offset: 0x003F79D7
	public float Delta
	{
		get
		{
			return Time.deltaTime * 60f;
		}
	}

	// Token: 0x170013D0 RID: 5072
	// (get) Token: 0x0600AA79 RID: 43641 RVA: 0x003F97E4 File Offset: 0x003F79E4
	public float SDelta
	{
		get
		{
			return Time.smoothDeltaTime * 60f;
		}
	}

	// Token: 0x170013D1 RID: 5073
	// (get) Token: 0x0600AA7A RID: 43642 RVA: 0x003F97F1 File Offset: 0x003F79F1
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

	// Token: 0x170013D2 RID: 5074
	// (get) Token: 0x0600AA7B RID: 43643 RVA: 0x003F9813 File Offset: 0x003F7A13
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

	// Token: 0x170013D3 RID: 5075
	// (get) Token: 0x0600AA7C RID: 43644 RVA: 0x003F983A File Offset: 0x003F7A3A
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

	// Token: 0x170013D4 RID: 5076
	// (get) Token: 0x0600AA7D RID: 43645 RVA: 0x003F9861 File Offset: 0x003F7A61
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

	// Token: 0x170013D5 RID: 5077
	// (get) Token: 0x0600AA7E RID: 43646 RVA: 0x003F9883 File Offset: 0x003F7A83
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

	// Token: 0x170013D6 RID: 5078
	// (get) Token: 0x0600AA7F RID: 43647 RVA: 0x003F98A8 File Offset: 0x003F7AA8
	// (set) Token: 0x0600AA80 RID: 43648 RVA: 0x003F9914 File Offset: 0x003F7B14
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

	// Token: 0x0600AA81 RID: 43649 RVA: 0x003F9970 File Offset: 0x003F7B70
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

	// Token: 0x0600AA82 RID: 43650 RVA: 0x003F99A6 File Offset: 0x003F7BA6
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Start()
	{
		this.ResetState();
	}

	// Token: 0x0600AA83 RID: 43651 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Init()
	{
	}

	// Token: 0x0600AA84 RID: 43652 RVA: 0x003F99AE File Offset: 0x003F7BAE
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		if (this.EventHandler != null)
		{
			this.EventHandler.Register(this);
		}
	}

	// Token: 0x0600AA85 RID: 43653 RVA: 0x003F99CA File Offset: 0x003F7BCA
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDisable()
	{
		if (this.EventHandler != null)
		{
			this.EventHandler.Unregister(this);
		}
	}

	// Token: 0x0600AA86 RID: 43654 RVA: 0x003F99E6 File Offset: 0x003F7BE6
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Update()
	{
		if (!this.m_Initialized)
		{
			this.Init();
			this.m_Initialized = true;
		}
	}

	// Token: 0x0600AA87 RID: 43655 RVA: 0x003F9A00 File Offset: 0x003F7C00
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

	// Token: 0x0600AA88 RID: 43656 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void LateUpdate()
	{
	}

	// Token: 0x0600AA89 RID: 43657 RVA: 0x003F9AF8 File Offset: 0x003F7CF8
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

	// Token: 0x0600AA8A RID: 43658 RVA: 0x003F9B78 File Offset: 0x003F7D78
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

	// Token: 0x0600AA8B RID: 43659 RVA: 0x003F9C20 File Offset: 0x003F7E20
	public void ResetState()
	{
		this.StateManager.Reset();
		this.Refresh();
	}

	// Token: 0x0600AA8C RID: 43660 RVA: 0x003F9C33 File Offset: 0x003F7E33
	public bool StateEnabled(string stateName)
	{
		return this.StateManager.IsEnabled(stateName);
	}

	// Token: 0x0600AA8D RID: 43661 RVA: 0x003F9C44 File Offset: 0x003F7E44
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

	// Token: 0x0600AA8E RID: 43662 RVA: 0x003F9D59 File Offset: 0x003F7F59
	public void ApplyPreset(vp_ComponentPreset preset)
	{
		vp_ComponentPreset.Apply(this, preset);
		this.RefreshDefaultState();
		this.Refresh();
	}

	// Token: 0x0600AA8F RID: 43663 RVA: 0x003F9D6F File Offset: 0x003F7F6F
	public vp_ComponentPreset Load(string path)
	{
		vp_ComponentPreset vp_ComponentPreset = vp_ComponentPreset.LoadFromResources(this, path);
		this.RefreshDefaultState();
		this.Refresh();
		return vp_ComponentPreset;
	}

	// Token: 0x0600AA90 RID: 43664 RVA: 0x003F9D84 File Offset: 0x003F7F84
	public vp_ComponentPreset Load(TextAsset asset)
	{
		vp_ComponentPreset vp_ComponentPreset = vp_ComponentPreset.LoadFromTextAsset(this, asset);
		this.RefreshDefaultState();
		this.Refresh();
		return vp_ComponentPreset;
	}

	// Token: 0x0600AA91 RID: 43665 RVA: 0x003F9D9C File Offset: 0x003F7F9C
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

	// Token: 0x0600AA92 RID: 43666 RVA: 0x003F9DF4 File Offset: 0x003F7FF4
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

	// Token: 0x0600AA93 RID: 43667 RVA: 0x003F9E3C File Offset: 0x003F803C
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

	// Token: 0x0600AA94 RID: 43668 RVA: 0x003F9E90 File Offset: 0x003F8090
	public void CacheRenderers()
	{
		this.Renderers.Clear();
		foreach (Renderer renderer in base.GetComponentsInChildren<Renderer>(true))
		{
			this.Renderers.Add(renderer);
		}
	}

	// Token: 0x0600AA95 RID: 43669 RVA: 0x003F9ED0 File Offset: 0x003F80D0
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

	// Token: 0x0600AA96 RID: 43670 RVA: 0x003F9F30 File Offset: 0x003F8130
	public void CacheAudioSources()
	{
		this.AudioSources.Clear();
		foreach (AudioSource audioSource in base.GetComponentsInChildren<AudioSource>(true))
		{
			this.AudioSources.Add(audioSource);
		}
	}

	// Token: 0x0600AA97 RID: 43671 RVA: 0x003F9F6E File Offset: 0x003F816E
	public virtual void Activate()
	{
		this.m_DeactivationTimer.Cancel();
		vp_Utility.Activate(base.gameObject, true);
	}

	// Token: 0x0600AA98 RID: 43672 RVA: 0x003F9F87 File Offset: 0x003F8187
	public virtual void Deactivate()
	{
		vp_Utility.Activate(base.gameObject, false);
	}

	// Token: 0x0600AA99 RID: 43673 RVA: 0x003F9F98 File Offset: 0x003F8198
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

	// Token: 0x0600AA9A RID: 43674 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void Refresh()
	{
	}

	// Token: 0x04007EBD RID: 32445
	public bool Persist;

	// Token: 0x04007EBE RID: 32446
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_StateManager m_StateManager;

	// Token: 0x04007EBF RID: 32447
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_EventHandler m_EventHandler;

	// Token: 0x04007EC0 RID: 32448
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_State m_DefaultState;

	// Token: 0x04007EC1 RID: 32449
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_Initialized;

	// Token: 0x04007EC2 RID: 32450
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Transform;

	// Token: 0x04007EC3 RID: 32451
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Parent;

	// Token: 0x04007EC4 RID: 32452
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Root;

	// Token: 0x04007EC5 RID: 32453
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public AudioSource m_Audio;

	// Token: 0x04007EC6 RID: 32454
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Collider m_Collider;

	// Token: 0x04007EC7 RID: 32455
	public List<vp_State> States = new List<vp_State>();

	// Token: 0x04007EC8 RID: 32456
	public List<vp_Component> Children = new List<vp_Component>();

	// Token: 0x04007EC9 RID: 32457
	public List<vp_Component> Siblings = new List<vp_Component>();

	// Token: 0x04007ECA RID: 32458
	public List<vp_Component> Family = new List<vp_Component>();

	// Token: 0x04007ECB RID: 32459
	public List<Renderer> Renderers = new List<Renderer>();

	// Token: 0x04007ECC RID: 32460
	public List<AudioSource> AudioSources = new List<AudioSource>();

	// Token: 0x04007ECD RID: 32461
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Type m_Type;

	// Token: 0x04007ECE RID: 32462
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public FieldInfo[] m_Fields;

	// Token: 0x04007ECF RID: 32463
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Timer.Handle m_DeactivationTimer = new vp_Timer.Handle();

	// Token: 0x04007ED0 RID: 32464
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Color prevSkinColor = Color.white;
}

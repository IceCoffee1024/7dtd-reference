using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001559 RID: 5465
[Preserve]
public class vp_Component : MonoBehaviour
{
	// Token: 0x1700140F RID: 5135
	// (get) Token: 0x0600AB26 RID: 43814 RVA: 0x003FE4A4 File Offset: 0x003FC6A4
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

	// Token: 0x17001410 RID: 5136
	// (get) Token: 0x0600AB27 RID: 43815 RVA: 0x003FE5BC File Offset: 0x003FC7BC
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

	// Token: 0x17001411 RID: 5137
	// (get) Token: 0x0600AB28 RID: 43816 RVA: 0x003FE5DE File Offset: 0x003FC7DE
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

	// Token: 0x17001412 RID: 5138
	// (get) Token: 0x0600AB29 RID: 43817 RVA: 0x003FE601 File Offset: 0x003FC801
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

	// Token: 0x17001413 RID: 5139
	// (get) Token: 0x0600AB2A RID: 43818 RVA: 0x003FE623 File Offset: 0x003FC823
	public vp_State DefaultState
	{
		get
		{
			return this.m_DefaultState;
		}
	}

	// Token: 0x17001414 RID: 5140
	// (get) Token: 0x0600AB2B RID: 43819 RVA: 0x003FE62B File Offset: 0x003FC82B
	public float Delta
	{
		get
		{
			return Time.deltaTime * 60f;
		}
	}

	// Token: 0x17001415 RID: 5141
	// (get) Token: 0x0600AB2C RID: 43820 RVA: 0x003FE638 File Offset: 0x003FC838
	public float SDelta
	{
		get
		{
			return Time.smoothDeltaTime * 60f;
		}
	}

	// Token: 0x17001416 RID: 5142
	// (get) Token: 0x0600AB2D RID: 43821 RVA: 0x003FE645 File Offset: 0x003FC845
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

	// Token: 0x17001417 RID: 5143
	// (get) Token: 0x0600AB2E RID: 43822 RVA: 0x003FE667 File Offset: 0x003FC867
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

	// Token: 0x17001418 RID: 5144
	// (get) Token: 0x0600AB2F RID: 43823 RVA: 0x003FE68E File Offset: 0x003FC88E
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

	// Token: 0x17001419 RID: 5145
	// (get) Token: 0x0600AB30 RID: 43824 RVA: 0x003FE6B5 File Offset: 0x003FC8B5
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

	// Token: 0x1700141A RID: 5146
	// (get) Token: 0x0600AB31 RID: 43825 RVA: 0x003FE6D7 File Offset: 0x003FC8D7
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

	// Token: 0x1700141B RID: 5147
	// (get) Token: 0x0600AB32 RID: 43826 RVA: 0x003FE6FC File Offset: 0x003FC8FC
	// (set) Token: 0x0600AB33 RID: 43827 RVA: 0x003FE768 File Offset: 0x003FC968
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

	// Token: 0x0600AB34 RID: 43828 RVA: 0x003FE7C4 File Offset: 0x003FC9C4
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

	// Token: 0x0600AB35 RID: 43829 RVA: 0x003FE7FA File Offset: 0x003FC9FA
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Start()
	{
		this.ResetState();
	}

	// Token: 0x0600AB36 RID: 43830 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Init()
	{
	}

	// Token: 0x0600AB37 RID: 43831 RVA: 0x003FE802 File Offset: 0x003FCA02
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		if (this.EventHandler != null)
		{
			this.EventHandler.Register(this);
		}
	}

	// Token: 0x0600AB38 RID: 43832 RVA: 0x003FE81E File Offset: 0x003FCA1E
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDisable()
	{
		if (this.EventHandler != null)
		{
			this.EventHandler.Unregister(this);
		}
	}

	// Token: 0x0600AB39 RID: 43833 RVA: 0x003FE83A File Offset: 0x003FCA3A
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Update()
	{
		if (!this.m_Initialized)
		{
			this.Init();
			this.m_Initialized = true;
		}
	}

	// Token: 0x0600AB3A RID: 43834 RVA: 0x003FE854 File Offset: 0x003FCA54
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

	// Token: 0x0600AB3B RID: 43835 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void LateUpdate()
	{
	}

	// Token: 0x0600AB3C RID: 43836 RVA: 0x003FE94C File Offset: 0x003FCB4C
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

	// Token: 0x0600AB3D RID: 43837 RVA: 0x003FE9CC File Offset: 0x003FCBCC
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

	// Token: 0x0600AB3E RID: 43838 RVA: 0x003FEA74 File Offset: 0x003FCC74
	public void ResetState()
	{
		this.StateManager.Reset();
		this.Refresh();
	}

	// Token: 0x0600AB3F RID: 43839 RVA: 0x003FEA87 File Offset: 0x003FCC87
	public bool StateEnabled(string stateName)
	{
		return this.StateManager.IsEnabled(stateName);
	}

	// Token: 0x0600AB40 RID: 43840 RVA: 0x003FEA98 File Offset: 0x003FCC98
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

	// Token: 0x0600AB41 RID: 43841 RVA: 0x003FEBAD File Offset: 0x003FCDAD
	public void ApplyPreset(vp_ComponentPreset preset)
	{
		vp_ComponentPreset.Apply(this, preset);
		this.RefreshDefaultState();
		this.Refresh();
	}

	// Token: 0x0600AB42 RID: 43842 RVA: 0x003FEBC3 File Offset: 0x003FCDC3
	public vp_ComponentPreset Load(string path)
	{
		vp_ComponentPreset vp_ComponentPreset = vp_ComponentPreset.LoadFromResources(this, path);
		this.RefreshDefaultState();
		this.Refresh();
		return vp_ComponentPreset;
	}

	// Token: 0x0600AB43 RID: 43843 RVA: 0x003FEBD8 File Offset: 0x003FCDD8
	public vp_ComponentPreset Load(TextAsset asset)
	{
		vp_ComponentPreset vp_ComponentPreset = vp_ComponentPreset.LoadFromTextAsset(this, asset);
		this.RefreshDefaultState();
		this.Refresh();
		return vp_ComponentPreset;
	}

	// Token: 0x0600AB44 RID: 43844 RVA: 0x003FEBF0 File Offset: 0x003FCDF0
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

	// Token: 0x0600AB45 RID: 43845 RVA: 0x003FEC48 File Offset: 0x003FCE48
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

	// Token: 0x0600AB46 RID: 43846 RVA: 0x003FEC90 File Offset: 0x003FCE90
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

	// Token: 0x0600AB47 RID: 43847 RVA: 0x003FECE4 File Offset: 0x003FCEE4
	public void CacheRenderers()
	{
		this.Renderers.Clear();
		foreach (Renderer renderer in base.GetComponentsInChildren<Renderer>(true))
		{
			this.Renderers.Add(renderer);
		}
	}

	// Token: 0x0600AB48 RID: 43848 RVA: 0x003FED24 File Offset: 0x003FCF24
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

	// Token: 0x0600AB49 RID: 43849 RVA: 0x003FED84 File Offset: 0x003FCF84
	public void CacheAudioSources()
	{
		this.AudioSources.Clear();
		foreach (AudioSource audioSource in base.GetComponentsInChildren<AudioSource>(true))
		{
			this.AudioSources.Add(audioSource);
		}
	}

	// Token: 0x0600AB4A RID: 43850 RVA: 0x003FEDC2 File Offset: 0x003FCFC2
	public virtual void Activate()
	{
		this.m_DeactivationTimer.Cancel();
		vp_Utility.Activate(base.gameObject, true);
	}

	// Token: 0x0600AB4B RID: 43851 RVA: 0x003FEDDB File Offset: 0x003FCFDB
	public virtual void Deactivate()
	{
		vp_Utility.Activate(base.gameObject, false);
	}

	// Token: 0x0600AB4C RID: 43852 RVA: 0x003FEDEC File Offset: 0x003FCFEC
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

	// Token: 0x0600AB4D RID: 43853 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void Refresh()
	{
	}

	// Token: 0x04007F40 RID: 32576
	public bool Persist;

	// Token: 0x04007F41 RID: 32577
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_StateManager m_StateManager;

	// Token: 0x04007F42 RID: 32578
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_EventHandler m_EventHandler;

	// Token: 0x04007F43 RID: 32579
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_State m_DefaultState;

	// Token: 0x04007F44 RID: 32580
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_Initialized;

	// Token: 0x04007F45 RID: 32581
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Transform;

	// Token: 0x04007F46 RID: 32582
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Parent;

	// Token: 0x04007F47 RID: 32583
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Root;

	// Token: 0x04007F48 RID: 32584
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public AudioSource m_Audio;

	// Token: 0x04007F49 RID: 32585
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Collider m_Collider;

	// Token: 0x04007F4A RID: 32586
	public List<vp_State> States = new List<vp_State>();

	// Token: 0x04007F4B RID: 32587
	public List<vp_Component> Children = new List<vp_Component>();

	// Token: 0x04007F4C RID: 32588
	public List<vp_Component> Siblings = new List<vp_Component>();

	// Token: 0x04007F4D RID: 32589
	public List<vp_Component> Family = new List<vp_Component>();

	// Token: 0x04007F4E RID: 32590
	public List<Renderer> Renderers = new List<Renderer>();

	// Token: 0x04007F4F RID: 32591
	public List<AudioSource> AudioSources = new List<AudioSource>();

	// Token: 0x04007F50 RID: 32592
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Type m_Type;

	// Token: 0x04007F51 RID: 32593
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public FieldInfo[] m_Fields;

	// Token: 0x04007F52 RID: 32594
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Timer.Handle m_DeactivationTimer = new vp_Timer.Handle();

	// Token: 0x04007F53 RID: 32595
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Color prevSkinColor = Color.white;
}

using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200156B RID: 5483
[Preserve]
public class vp_Component : MonoBehaviour
{
	// Token: 0x1700141B RID: 5147
	// (get) Token: 0x0600AB7E RID: 43902 RVA: 0x004008A8 File Offset: 0x003FEAA8
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

	// Token: 0x1700141C RID: 5148
	// (get) Token: 0x0600AB7F RID: 43903 RVA: 0x004009C0 File Offset: 0x003FEBC0
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

	// Token: 0x1700141D RID: 5149
	// (get) Token: 0x0600AB80 RID: 43904 RVA: 0x004009E2 File Offset: 0x003FEBE2
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

	// Token: 0x1700141E RID: 5150
	// (get) Token: 0x0600AB81 RID: 43905 RVA: 0x00400A05 File Offset: 0x003FEC05
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

	// Token: 0x1700141F RID: 5151
	// (get) Token: 0x0600AB82 RID: 43906 RVA: 0x00400A27 File Offset: 0x003FEC27
	public vp_State DefaultState
	{
		get
		{
			return this.m_DefaultState;
		}
	}

	// Token: 0x17001420 RID: 5152
	// (get) Token: 0x0600AB83 RID: 43907 RVA: 0x00400A2F File Offset: 0x003FEC2F
	public float Delta
	{
		get
		{
			return Time.deltaTime * 60f;
		}
	}

	// Token: 0x17001421 RID: 5153
	// (get) Token: 0x0600AB84 RID: 43908 RVA: 0x00400A3C File Offset: 0x003FEC3C
	public float SDelta
	{
		get
		{
			return Time.smoothDeltaTime * 60f;
		}
	}

	// Token: 0x17001422 RID: 5154
	// (get) Token: 0x0600AB85 RID: 43909 RVA: 0x00400A49 File Offset: 0x003FEC49
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

	// Token: 0x17001423 RID: 5155
	// (get) Token: 0x0600AB86 RID: 43910 RVA: 0x00400A6B File Offset: 0x003FEC6B
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

	// Token: 0x17001424 RID: 5156
	// (get) Token: 0x0600AB87 RID: 43911 RVA: 0x00400A92 File Offset: 0x003FEC92
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

	// Token: 0x17001425 RID: 5157
	// (get) Token: 0x0600AB88 RID: 43912 RVA: 0x00400AB9 File Offset: 0x003FECB9
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

	// Token: 0x17001426 RID: 5158
	// (get) Token: 0x0600AB89 RID: 43913 RVA: 0x00400ADB File Offset: 0x003FECDB
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

	// Token: 0x17001427 RID: 5159
	// (get) Token: 0x0600AB8A RID: 43914 RVA: 0x00400B00 File Offset: 0x003FED00
	// (set) Token: 0x0600AB8B RID: 43915 RVA: 0x00400B6C File Offset: 0x003FED6C
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

	// Token: 0x0600AB8C RID: 43916 RVA: 0x00400BC8 File Offset: 0x003FEDC8
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

	// Token: 0x0600AB8D RID: 43917 RVA: 0x00400BFE File Offset: 0x003FEDFE
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Start()
	{
		this.ResetState();
	}

	// Token: 0x0600AB8E RID: 43918 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Init()
	{
	}

	// Token: 0x0600AB8F RID: 43919 RVA: 0x00400C06 File Offset: 0x003FEE06
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		if (this.EventHandler != null)
		{
			this.EventHandler.Register(this);
		}
	}

	// Token: 0x0600AB90 RID: 43920 RVA: 0x00400C22 File Offset: 0x003FEE22
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDisable()
	{
		if (this.EventHandler != null)
		{
			this.EventHandler.Unregister(this);
		}
	}

	// Token: 0x0600AB91 RID: 43921 RVA: 0x00400C3E File Offset: 0x003FEE3E
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Update()
	{
		if (!this.m_Initialized)
		{
			this.Init();
			this.m_Initialized = true;
		}
	}

	// Token: 0x0600AB92 RID: 43922 RVA: 0x00400C58 File Offset: 0x003FEE58
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

	// Token: 0x0600AB93 RID: 43923 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void LateUpdate()
	{
	}

	// Token: 0x0600AB94 RID: 43924 RVA: 0x00400D50 File Offset: 0x003FEF50
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

	// Token: 0x0600AB95 RID: 43925 RVA: 0x00400DD0 File Offset: 0x003FEFD0
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

	// Token: 0x0600AB96 RID: 43926 RVA: 0x00400E78 File Offset: 0x003FF078
	public void ResetState()
	{
		this.StateManager.Reset();
		this.Refresh();
	}

	// Token: 0x0600AB97 RID: 43927 RVA: 0x00400E8B File Offset: 0x003FF08B
	public bool StateEnabled(string stateName)
	{
		return this.StateManager.IsEnabled(stateName);
	}

	// Token: 0x0600AB98 RID: 43928 RVA: 0x00400E9C File Offset: 0x003FF09C
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

	// Token: 0x0600AB99 RID: 43929 RVA: 0x00400FB1 File Offset: 0x003FF1B1
	public void ApplyPreset(vp_ComponentPreset preset)
	{
		vp_ComponentPreset.Apply(this, preset);
		this.RefreshDefaultState();
		this.Refresh();
	}

	// Token: 0x0600AB9A RID: 43930 RVA: 0x00400FC7 File Offset: 0x003FF1C7
	public vp_ComponentPreset Load(string path)
	{
		vp_ComponentPreset vp_ComponentPreset = vp_ComponentPreset.LoadFromResources(this, path);
		this.RefreshDefaultState();
		this.Refresh();
		return vp_ComponentPreset;
	}

	// Token: 0x0600AB9B RID: 43931 RVA: 0x00400FDC File Offset: 0x003FF1DC
	public vp_ComponentPreset Load(TextAsset asset)
	{
		vp_ComponentPreset vp_ComponentPreset = vp_ComponentPreset.LoadFromTextAsset(this, asset);
		this.RefreshDefaultState();
		this.Refresh();
		return vp_ComponentPreset;
	}

	// Token: 0x0600AB9C RID: 43932 RVA: 0x00400FF4 File Offset: 0x003FF1F4
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

	// Token: 0x0600AB9D RID: 43933 RVA: 0x0040104C File Offset: 0x003FF24C
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

	// Token: 0x0600AB9E RID: 43934 RVA: 0x00401094 File Offset: 0x003FF294
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

	// Token: 0x0600AB9F RID: 43935 RVA: 0x004010E8 File Offset: 0x003FF2E8
	public void CacheRenderers()
	{
		this.Renderers.Clear();
		foreach (Renderer renderer in base.GetComponentsInChildren<Renderer>(true))
		{
			this.Renderers.Add(renderer);
		}
	}

	// Token: 0x0600ABA0 RID: 43936 RVA: 0x00401128 File Offset: 0x003FF328
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

	// Token: 0x0600ABA1 RID: 43937 RVA: 0x00401188 File Offset: 0x003FF388
	public void CacheAudioSources()
	{
		this.AudioSources.Clear();
		foreach (AudioSource audioSource in base.GetComponentsInChildren<AudioSource>(true))
		{
			this.AudioSources.Add(audioSource);
		}
	}

	// Token: 0x0600ABA2 RID: 43938 RVA: 0x004011C6 File Offset: 0x003FF3C6
	public virtual void Activate()
	{
		this.m_DeactivationTimer.Cancel();
		vp_Utility.Activate(base.gameObject, true);
	}

	// Token: 0x0600ABA3 RID: 43939 RVA: 0x004011DF File Offset: 0x003FF3DF
	public virtual void Deactivate()
	{
		vp_Utility.Activate(base.gameObject, false);
	}

	// Token: 0x0600ABA4 RID: 43940 RVA: 0x004011F0 File Offset: 0x003FF3F0
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

	// Token: 0x0600ABA5 RID: 43941 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void Refresh()
	{
	}

	// Token: 0x04007F81 RID: 32641
	public bool Persist;

	// Token: 0x04007F82 RID: 32642
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_StateManager m_StateManager;

	// Token: 0x04007F83 RID: 32643
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_EventHandler m_EventHandler;

	// Token: 0x04007F84 RID: 32644
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_State m_DefaultState;

	// Token: 0x04007F85 RID: 32645
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_Initialized;

	// Token: 0x04007F86 RID: 32646
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Transform;

	// Token: 0x04007F87 RID: 32647
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Parent;

	// Token: 0x04007F88 RID: 32648
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Root;

	// Token: 0x04007F89 RID: 32649
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public AudioSource m_Audio;

	// Token: 0x04007F8A RID: 32650
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Collider m_Collider;

	// Token: 0x04007F8B RID: 32651
	public List<vp_State> States = new List<vp_State>();

	// Token: 0x04007F8C RID: 32652
	public List<vp_Component> Children = new List<vp_Component>();

	// Token: 0x04007F8D RID: 32653
	public List<vp_Component> Siblings = new List<vp_Component>();

	// Token: 0x04007F8E RID: 32654
	public List<vp_Component> Family = new List<vp_Component>();

	// Token: 0x04007F8F RID: 32655
	public List<Renderer> Renderers = new List<Renderer>();

	// Token: 0x04007F90 RID: 32656
	public List<AudioSource> AudioSources = new List<AudioSource>();

	// Token: 0x04007F91 RID: 32657
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Type m_Type;

	// Token: 0x04007F92 RID: 32658
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public FieldInfo[] m_Fields;

	// Token: 0x04007F93 RID: 32659
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Timer.Handle m_DeactivationTimer = new vp_Timer.Handle();

	// Token: 0x04007F94 RID: 32660
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Color prevSkinColor = Color.white;
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using Audio;
using InControl;
using Unity.Profiling;
using UnityEngine;

// Token: 0x02001166 RID: 4454
public abstract class XUiView : IXUiElement
{
	// Token: 0x1700106C RID: 4204
	// (get) Token: 0x06008C8A RID: 35978 RVA: 0x00357FE3 File Offset: 0x003561E3
	// (set) Token: 0x06008C8B RID: 35979 RVA: 0x00357FEB File Offset: 0x003561EB
	public XUiController Controller
	{
		get
		{
			return this.controller;
		}
		set
		{
			this.controller = value;
			if (this.controller.ViewComponent != this)
			{
				this.controller.ViewComponent = this;
			}
		}
	}

	// Token: 0x1700106D RID: 4205
	// (get) Token: 0x06008C8C RID: 35980
	public abstract UIRect UiRect
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get;
	}

	// Token: 0x1700106E RID: 4206
	// (get) Token: 0x06008C8D RID: 35981 RVA: 0x0035800E File Offset: 0x0035620E
	public Transform UiTransform
	{
		get
		{
			return this.uiTransform;
		}
	}

	// Token: 0x1700106F RID: 4207
	// (get) Token: 0x06008C8E RID: 35982 RVA: 0x00358016 File Offset: 0x00356216
	public bool ColliderEnabled
	{
		get
		{
			return !this.destroyed && this.collider.enabled;
		}
	}

	// Token: 0x17001070 RID: 4208
	// (get) Token: 0x06008C8F RID: 35983 RVA: 0x00358030 File Offset: 0x00356230
	public Vector3 ColliderCenter
	{
		get
		{
			if (!this.destroyed)
			{
				return this.collider.bounds.center;
			}
			return default(Vector3);
		}
	}

	// Token: 0x17001071 RID: 4209
	// (get) Token: 0x06008C90 RID: 35984 RVA: 0x00358064 File Offset: 0x00356264
	public float ColliderHeightExtent
	{
		get
		{
			if (!this.destroyed)
			{
				return this.collider.bounds.size.y / 2f;
			}
			return 0f;
		}
	}

	// Token: 0x17001072 RID: 4210
	// (get) Token: 0x06008C91 RID: 35985 RVA: 0x003580A0 File Offset: 0x003562A0
	public float ColliderWidthExtent
	{
		get
		{
			if (!this.destroyed)
			{
				return this.collider.bounds.size.x / 2f;
			}
			return 0f;
		}
	}

	// Token: 0x17001073 RID: 4211
	// (get) Token: 0x06008C92 RID: 35986 RVA: 0x003580DC File Offset: 0x003562DC
	public Bounds ColliderBounds
	{
		get
		{
			if (!this.destroyed)
			{
				return this.collider.bounds;
			}
			return default(Bounds);
		}
	}

	// Token: 0x17001074 RID: 4212
	// (get) Token: 0x06008C93 RID: 35987 RVA: 0x00358108 File Offset: 0x00356308
	public virtual Vector3 LocalCenter
	{
		get
		{
			return (this.position + this.size / 2).AsVector2();
		}
	}

	// Token: 0x17001075 RID: 4213
	// (get) Token: 0x06008C94 RID: 35988
	public abstract Vector3[] WorldCorners { get; }

	// Token: 0x17001076 RID: 4214
	// (get) Token: 0x06008C95 RID: 35989 RVA: 0x00358139 File Offset: 0x00356339
	// (set) Token: 0x06008C96 RID: 35990 RVA: 0x00358141 File Offset: 0x00356341
	[XuiXmlAttribute("repeat_content", false)]
	public bool RepeatContent { get; set; }

	// Token: 0x17001077 RID: 4215
	// (get) Token: 0x06008C97 RID: 35991 RVA: 0x0035814A File Offset: 0x0035634A
	// (set) Token: 0x06008C98 RID: 35992 RVA: 0x00358152 File Offset: 0x00356352
	[XuiXmlAttribute("repeat_count", false)]
	public virtual int RepeatCount { get; set; }

	// Token: 0x17001078 RID: 4216
	// (get) Token: 0x06008C99 RID: 35993 RVA: 0x0035815B File Offset: 0x0035635B
	public bool UiTransformIsHovered
	{
		get
		{
			return UICamera.hoveredObject == this.uiTransform.gameObject;
		}
	}

	// Token: 0x17001079 RID: 4217
	// (get) Token: 0x06008C9A RID: 35994 RVA: 0x00358172 File Offset: 0x00356372
	public bool IsActiveInHierarchy
	{
		get
		{
			if (!(this.uiTransform != null))
			{
				return this.isVisible;
			}
			return this.uiTransform.gameObject.activeInHierarchy;
		}
	}

	// Token: 0x1700107A RID: 4218
	// (get) Token: 0x06008C9B RID: 35995 RVA: 0x00358199 File Offset: 0x00356399
	public string ID
	{
		get
		{
			return this.id;
		}
	}

	// Token: 0x06008C9C RID: 35996 RVA: 0x003581A1 File Offset: 0x003563A1
	[XuiXmlAttribute("name", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeName(string _value)
	{
		this.id = _value;
	}

	// Token: 0x1700107B RID: 4219
	// (get) Token: 0x06008C9D RID: 35997 RVA: 0x003581AA File Offset: 0x003563AA
	// (set) Token: 0x06008C9E RID: 35998 RVA: 0x003581B4 File Offset: 0x003563B4
	[XuiXmlAttribute("visible", false)]
	public bool IsVisible
	{
		get
		{
			return this.isVisible;
		}
		set
		{
			if (this.isVisible == value)
			{
				return;
			}
			this.isVisible = value;
			if (this.uiTransform != null && this.isVisible != this.uiTransform.gameObject.activeSelf)
			{
				this.uiTransform.gameObject.SetActive(this.isVisible);
			}
			this.Controller.OnVisibilityChanged(this.IsActiveInHierarchy);
			if (this.xui.playerUI.CursorController.navigationTarget == this)
			{
				this.xui.playerUI.RefreshNavigationTarget();
			}
			this.SetDirty();
		}
	}

	// Token: 0x1700107C RID: 4220
	// (get) Token: 0x06008C9F RID: 35999 RVA: 0x0035824D File Offset: 0x0035644D
	// (set) Token: 0x06008CA0 RID: 36000 RVA: 0x00358258 File Offset: 0x00356458
	[XuiXmlAttribute("enabled", false)]
	public virtual bool Enabled
	{
		get
		{
			return this.enabled;
		}
		set
		{
			if (value == this.enabled)
			{
				return;
			}
			this.enabled = value;
			if (!this.enabled && this.xui.playerUI.CursorController.navigationTarget == this)
			{
				this.xui.playerUI.RefreshNavigationTarget();
			}
			if (!value && this.isOver)
			{
				this.OnHover(true);
			}
			this.Controller.OnEnabledChanged(this.enabled);
			this.SetDirty();
		}
	}

	// Token: 0x1700107D RID: 4221
	// (get) Token: 0x06008CA1 RID: 36001 RVA: 0x003582CF File Offset: 0x003564CF
	// (set) Token: 0x06008CA2 RID: 36002 RVA: 0x003582D7 File Offset: 0x003564D7
	[XuiXmlAttribute("collider_scale", false)]
	public float ColliderScale
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get;
		[PublicizedFrom(EAccessModifier.Protected)]
		set;
	} = 1f;

	// Token: 0x1700107E RID: 4222
	// (get) Token: 0x06008CA3 RID: 36003 RVA: 0x003582E0 File Offset: 0x003564E0
	// (set) Token: 0x06008CA4 RID: 36004 RVA: 0x003582E8 File Offset: 0x003564E8
	[XuiXmlAttribute("collider_padding", false)]
	public Vector2i ColliderPadding
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get;
		[PublicizedFrom(EAccessModifier.Protected)]
		set;
	} = Vector2i.zero;

	// Token: 0x1700107F RID: 4223
	// (get) Token: 0x06008CA5 RID: 36005 RVA: 0x003582F1 File Offset: 0x003564F1
	// (set) Token: 0x06008CA6 RID: 36006 RVA: 0x003582F9 File Offset: 0x003564F9
	[XuiXmlAttribute("size", false)]
	public Vector2i Size
	{
		get
		{
			return this.size;
		}
		set
		{
			if (this.size == value)
			{
				return;
			}
			this.size = value;
			this.SetDirty();
		}
	}

	// Token: 0x17001080 RID: 4224
	// (get) Token: 0x06008CA7 RID: 36007 RVA: 0x00358317 File Offset: 0x00356517
	// (set) Token: 0x06008CA8 RID: 36008 RVA: 0x00358324 File Offset: 0x00356524
	[XuiXmlAttribute("width", false)]
	public int Width
	{
		get
		{
			return this.Size.x;
		}
		set
		{
			if (this.Width == value)
			{
				return;
			}
			Vector2i vector2i = this.Size;
			vector2i.x = value;
			this.Size = vector2i;
		}
	}

	// Token: 0x17001081 RID: 4225
	// (get) Token: 0x06008CA9 RID: 36009 RVA: 0x00358351 File Offset: 0x00356551
	// (set) Token: 0x06008CAA RID: 36010 RVA: 0x00358360 File Offset: 0x00356560
	[XuiXmlAttribute("height", false)]
	public int Height
	{
		get
		{
			return this.Size.y;
		}
		set
		{
			if (this.Height == value)
			{
				return;
			}
			Vector2i vector2i = this.Size;
			vector2i.y = value;
			this.Size = vector2i;
		}
	}

	// Token: 0x17001082 RID: 4226
	// (get) Token: 0x06008CAB RID: 36011 RVA: 0x0035838D File Offset: 0x0035658D
	// (set) Token: 0x06008CAC RID: 36012 RVA: 0x00358395 File Offset: 0x00356595
	[XuiXmlAttribute("pos", false)]
	public Vector2i Position
	{
		get
		{
			return this.position;
		}
		set
		{
			if (this.position == value)
			{
				return;
			}
			this.position = value;
			this.SetDirty();
			this.positionDirty = true;
		}
	}

	// Token: 0x17001083 RID: 4227
	// (get) Token: 0x06008CAD RID: 36013 RVA: 0x003583BA File Offset: 0x003565BA
	// (set) Token: 0x06008CAE RID: 36014 RVA: 0x003583C2 File Offset: 0x003565C2
	[XuiXmlAttribute("ignoreparentpadding", false)]
	public bool IgnoreParentPadding
	{
		get
		{
			return this.ignoreParentPadding;
		}
		set
		{
			if (this.ignoreParentPadding == value)
			{
				return;
			}
			this.ignoreParentPadding = value;
			this.SetDirty();
		}
	}

	// Token: 0x17001084 RID: 4228
	// (get) Token: 0x06008CAF RID: 36015 RVA: 0x003583DC File Offset: 0x003565DC
	public Vector2i PaddedPosition
	{
		get
		{
			Vector2i vector2i = this.position;
			Vector2i vector2i3;
			if (!this.ignoreParentPadding)
			{
				XUiController parent = this.Controller.Parent;
				Vector2i? vector2i2;
				if (parent == null)
				{
					vector2i2 = null;
				}
				else
				{
					XUiView viewComponent = parent.ViewComponent;
					vector2i2 = ((viewComponent != null) ? new Vector2i?(viewComponent.InnerPosition) : null);
				}
				vector2i3 = vector2i2 ?? Vector2i.zero;
			}
			else
			{
				vector2i3 = Vector2i.zero;
			}
			return vector2i + vector2i3;
		}
	}

	// Token: 0x17001085 RID: 4229
	// (get) Token: 0x06008CB0 RID: 36016 RVA: 0x00358453 File Offset: 0x00356653
	public virtual Vector2i InnerSize
	{
		get
		{
			return new Vector2i(this.size.x - this.padding.SumLeftRight, this.size.y - this.padding.SumTopBottom);
		}
	}

	// Token: 0x17001086 RID: 4230
	// (get) Token: 0x06008CB1 RID: 36017 RVA: 0x00358488 File Offset: 0x00356688
	public Vector2i InnerPosition
	{
		get
		{
			return new Vector2i(this.padding.Left, -this.padding.Top);
		}
	}

	// Token: 0x17001087 RID: 4231
	// (get) Token: 0x06008CB2 RID: 36018 RVA: 0x003584A6 File Offset: 0x003566A6
	// (set) Token: 0x06008CB3 RID: 36019 RVA: 0x003584AE File Offset: 0x003566AE
	[XuiXmlAttribute("rotation", false)]
	public float Rotation
	{
		get
		{
			return this.rotation;
		}
		set
		{
			if (Mathf.Approximately(this.rotation, value))
			{
				return;
			}
			this.rotation = value;
			this.SetDirty();
			this.rotationDirty = true;
		}
	}

	// Token: 0x17001088 RID: 4232
	// (get) Token: 0x06008CB4 RID: 36020 RVA: 0x003584D3 File Offset: 0x003566D3
	// (set) Token: 0x06008CB5 RID: 36021 RVA: 0x003584DB File Offset: 0x003566DB
	public int Depth
	{
		get
		{
			return this.depth;
		}
		set
		{
			if (this.depth == value)
			{
				return;
			}
			this.depth = value;
			this.SetDirty();
		}
	}

	// Token: 0x06008CB6 RID: 36022 RVA: 0x003584F4 File Offset: 0x003566F4
	[XuiXmlAttribute("depth", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeDepth(int _value)
	{
		XUiView viewComponent = this.controller.Parent.ViewComponent;
		this.Depth = _value + ((viewComponent != null) ? viewComponent.Depth : 0);
	}

	// Token: 0x06008CB7 RID: 36023 RVA: 0x0035851A File Offset: 0x0035671A
	[XuiXmlAttribute("padding_left", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributePaddingLeft(int _value)
	{
		this.padding = this.padding.SetLeft(_value);
	}

	// Token: 0x06008CB8 RID: 36024 RVA: 0x0035852E File Offset: 0x0035672E
	[XuiXmlAttribute("padding_right", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributePaddingRight(int _value)
	{
		this.padding = this.padding.SetRight(_value);
	}

	// Token: 0x06008CB9 RID: 36025 RVA: 0x00358542 File Offset: 0x00356742
	[XuiXmlAttribute("padding_top", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributePaddingTop(int _value)
	{
		this.padding = this.padding.SetTop(_value);
	}

	// Token: 0x06008CBA RID: 36026 RVA: 0x00358556 File Offset: 0x00356756
	[XuiXmlAttribute("padding_bottom", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributePaddingBottom(int _value)
	{
		this.padding = this.padding.SetBottom(_value);
	}

	// Token: 0x06008CBB RID: 36027 RVA: 0x0035856A File Offset: 0x0035676A
	[XuiXmlAttribute("padding", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributePadding(string _value)
	{
		XUiSideSizes.TryParse(_value, out this.padding, "padding");
	}

	// Token: 0x06008CBC RID: 36028 RVA: 0x0035857E File Offset: 0x0035677E
	[XuiXmlAttribute("sound", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeSound(string _value)
	{
		this.xui.LoadData<AudioClip>(_value, delegate(AudioClip _o)
		{
			this.xuiSound = _o;
		});
	}

	// Token: 0x06008CBD RID: 36029 RVA: 0x00358598 File Offset: 0x00356798
	[XuiXmlAttribute("sound_play_on_hover", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeSoundOnHover(string _value)
	{
		this.xui.LoadData<AudioClip>(_value, delegate(AudioClip _o)
		{
			this.xuiHoverSound = _o;
		});
	}

	// Token: 0x17001089 RID: 4233
	// (get) Token: 0x06008CBE RID: 36030 RVA: 0x003585B2 File Offset: 0x003567B2
	// (set) Token: 0x06008CBF RID: 36031 RVA: 0x003585BA File Offset: 0x003567BA
	[XuiXmlAttribute("sound_play_on_press", false)]
	public bool SoundPlayOnClick { get; set; }

	// Token: 0x1700108A RID: 4234
	// (get) Token: 0x06008CC0 RID: 36032 RVA: 0x003585C3 File Offset: 0x003567C3
	// (set) Token: 0x06008CC1 RID: 36033 RVA: 0x003585CB File Offset: 0x003567CB
	public bool SoundPlayOnHover { get; set; }

	// Token: 0x1700108B RID: 4235
	// (get) Token: 0x06008CC2 RID: 36034 RVA: 0x003585D4 File Offset: 0x003567D4
	// (set) Token: 0x06008CC3 RID: 36035 RVA: 0x003585DC File Offset: 0x003567DC
	[XuiXmlAttribute("sound_play_on_open", false)]
	public bool SoundPlayOnOpen { get; set; }

	// Token: 0x1700108C RID: 4236
	// (get) Token: 0x06008CC4 RID: 36036 RVA: 0x003585E5 File Offset: 0x003567E5
	// (set) Token: 0x06008CC5 RID: 36037 RVA: 0x003585ED File Offset: 0x003567ED
	[XuiXmlAttribute("sound_volume", false)]
	public float SoundVolume { get; set; }

	// Token: 0x1700108D RID: 4237
	// (get) Token: 0x06008CC6 RID: 36038 RVA: 0x003585F6 File Offset: 0x003567F6
	public bool HasHoverSound
	{
		get
		{
			return this.xuiHoverSound != null;
		}
	}

	// Token: 0x1700108E RID: 4238
	// (get) Token: 0x06008CC7 RID: 36039 RVA: 0x00358604 File Offset: 0x00356804
	// (set) Token: 0x06008CC8 RID: 36040 RVA: 0x0035860C File Offset: 0x0035680C
	[XuiXmlAttribute("hold_delay", false)]
	public float HoldDelay
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = 0.5f;

	// Token: 0x1700108F RID: 4239
	// (get) Token: 0x06008CC9 RID: 36041 RVA: 0x00358615 File Offset: 0x00356815
	// (set) Token: 0x06008CCA RID: 36042 RVA: 0x0035861D File Offset: 0x0035681D
	[XuiXmlAttribute("hold_timed_initial_interval", false)]
	public float HoldEventIntervalInitial
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = 0.5f;

	// Token: 0x17001090 RID: 4240
	// (get) Token: 0x06008CCB RID: 36043 RVA: 0x00358626 File Offset: 0x00356826
	// (set) Token: 0x06008CCC RID: 36044 RVA: 0x0035862E File Offset: 0x0035682E
	[XuiXmlAttribute("hold_timed_final_interval", false)]
	public float HoldEventIntervalFinal
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = 0.06f;

	// Token: 0x17001091 RID: 4241
	// (get) Token: 0x06008CCD RID: 36045 RVA: 0x00358637 File Offset: 0x00356837
	// (set) Token: 0x06008CCE RID: 36046 RVA: 0x0035863F File Offset: 0x0035683F
	[XuiXmlAttribute("hold_timed_step_acceleration", false)]
	public float HoldEventIntervalAcceleration
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = 0.015f;

	// Token: 0x17001092 RID: 4242
	// (get) Token: 0x06008CCF RID: 36047 RVA: 0x00358648 File Offset: 0x00356848
	// (set) Token: 0x06008CD0 RID: 36048 RVA: 0x00358650 File Offset: 0x00356850
	[XuiXmlAttribute("on_hover", false)]
	public bool EventOnHover
	{
		get
		{
			return this.eventOnHover;
		}
		set
		{
			if (this.eventOnHover == value)
			{
				return;
			}
			this.eventOnHover = value;
			this.SetDirty();
		}
	}

	// Token: 0x17001093 RID: 4243
	// (get) Token: 0x06008CD1 RID: 36049 RVA: 0x00358669 File Offset: 0x00356869
	// (set) Token: 0x06008CD2 RID: 36050 RVA: 0x00358671 File Offset: 0x00356871
	[XuiXmlAttribute("on_press", false)]
	public bool EventOnPress
	{
		get
		{
			return this.eventOnPress;
		}
		set
		{
			if (this.eventOnPress == value)
			{
				return;
			}
			this.eventOnPress = value;
			this.SetDirty();
		}
	}

	// Token: 0x17001094 RID: 4244
	// (get) Token: 0x06008CD3 RID: 36051 RVA: 0x0035868A File Offset: 0x0035688A
	// (set) Token: 0x06008CD4 RID: 36052 RVA: 0x00358692 File Offset: 0x00356892
	[XuiXmlAttribute("on_doubleclick", false)]
	public bool EventOnDoubleClick
	{
		get
		{
			return this.eventOnDoubleClick;
		}
		set
		{
			if (this.eventOnDoubleClick == value)
			{
				return;
			}
			this.eventOnDoubleClick = value;
			this.SetDirty();
		}
	}

	// Token: 0x17001095 RID: 4245
	// (get) Token: 0x06008CD5 RID: 36053 RVA: 0x003586AB File Offset: 0x003568AB
	// (set) Token: 0x06008CD6 RID: 36054 RVA: 0x003586B3 File Offset: 0x003568B3
	[XuiXmlAttribute("on_held", false)]
	public bool EventOnHeld
	{
		get
		{
			return this.eventOnHeld;
		}
		set
		{
			if (this.eventOnHeld == value)
			{
				return;
			}
			this.eventOnHeld = value;
			this.SetDirty();
		}
	}

	// Token: 0x17001096 RID: 4246
	// (get) Token: 0x06008CD7 RID: 36055 RVA: 0x003586CC File Offset: 0x003568CC
	// (set) Token: 0x06008CD8 RID: 36056 RVA: 0x003586D4 File Offset: 0x003568D4
	[XuiXmlAttribute("on_scroll", false)]
	public bool EventOnScroll
	{
		get
		{
			return this.eventOnScroll;
		}
		set
		{
			if (this.eventOnScroll == value)
			{
				return;
			}
			this.eventOnScroll = value;
			this.SetDirty();
		}
	}

	// Token: 0x17001097 RID: 4247
	// (get) Token: 0x06008CD9 RID: 36057 RVA: 0x003586ED File Offset: 0x003568ED
	// (set) Token: 0x06008CDA RID: 36058 RVA: 0x003586F5 File Offset: 0x003568F5
	[XuiXmlAttribute("on_drag", false)]
	public bool EventOnDrag
	{
		get
		{
			return this.eventOnDrag;
		}
		set
		{
			if (this.eventOnDrag == value)
			{
				return;
			}
			this.eventOnDrag = value;
			this.SetDirty();
		}
	}

	// Token: 0x17001098 RID: 4248
	// (get) Token: 0x06008CDB RID: 36059 RVA: 0x0035870E File Offset: 0x0035690E
	// (set) Token: 0x06008CDC RID: 36060 RVA: 0x00358716 File Offset: 0x00356916
	[XuiXmlAttribute("on_select", false)]
	public bool EventOnSelect
	{
		get
		{
			return this.eventOnSelect;
		}
		set
		{
			if (this.eventOnSelect == value)
			{
				return;
			}
			this.eventOnSelect = value;
			this.SetDirty();
		}
	}

	// Token: 0x17001099 RID: 4249
	// (get) Token: 0x06008CDD RID: 36061 RVA: 0x00358730 File Offset: 0x00356930
	public virtual bool HasAnyEvent
	{
		get
		{
			return this.EventOnPress || this.EventOnDoubleClick || this.EventOnHover || this.EventOnHeld || this.EventOnDrag || this.EventOnScroll || this.EventOnSelect || !string.IsNullOrEmpty(this.ToolTip);
		}
	}

	// Token: 0x1700109A RID: 4250
	// (get) Token: 0x06008CDE RID: 36062 RVA: 0x00358785 File Offset: 0x00356985
	public bool IsHovered
	{
		get
		{
			return this.isOver;
		}
	}

	// Token: 0x1700109B RID: 4251
	// (get) Token: 0x06008CDF RID: 36063 RVA: 0x0035878D File Offset: 0x0035698D
	// (set) Token: 0x06008CE0 RID: 36064 RVA: 0x003587AF File Offset: 0x003569AF
	public bool IsNavigatable
	{
		get
		{
			return this.isNavigatable && this.Enabled && this.IsVisible && this.IsActiveInHierarchy;
		}
		set
		{
			this.isNavigatable = value;
		}
	}

	// Token: 0x06008CE1 RID: 36065 RVA: 0x003587B8 File Offset: 0x003569B8
	[XuiXmlAttribute("gamepad_selectable", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeGamepadSelectable(bool _value)
	{
		this.IsNavigatable = _value;
		this.gamepadSelectableSetFromAttributes = true;
	}

	// Token: 0x1700109C RID: 4252
	// (get) Token: 0x06008CE2 RID: 36066 RVA: 0x003587C8 File Offset: 0x003569C8
	// (set) Token: 0x06008CE3 RID: 36067 RVA: 0x003587D0 File Offset: 0x003569D0
	public bool IsSnappable { get; set; } = true;

	// Token: 0x06008CE4 RID: 36068 RVA: 0x003587D9 File Offset: 0x003569D9
	[XuiXmlAttribute("snap", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeGamepadSnappable(bool _value)
	{
		this.IsSnappable = _value;
		this.gamepadSnappableSetFromAttributes = true;
	}

	// Token: 0x1700109D RID: 4253
	// (get) Token: 0x06008CE5 RID: 36069 RVA: 0x003587E9 File Offset: 0x003569E9
	// (set) Token: 0x06008CE6 RID: 36070 RVA: 0x003587F1 File Offset: 0x003569F1
	[XuiXmlAttribute("use_selection_box", false)]
	public bool UseSelectionBox { get; set; } = true;

	// Token: 0x1700109E RID: 4254
	// (get) Token: 0x06008CE7 RID: 36071 RVA: 0x003587FA File Offset: 0x003569FA
	// (set) Token: 0x06008CE8 RID: 36072 RVA: 0x00358802 File Offset: 0x00356A02
	[XuiXmlAttribute("nav_up", false)]
	public string NavUpTargetString
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.navUpTargetString;
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			if (this.navUpTargetString == value)
			{
				return;
			}
			this.navUpTargetString = value;
			this.SetDirty();
		}
	}

	// Token: 0x1700109F RID: 4255
	// (get) Token: 0x06008CE9 RID: 36073 RVA: 0x00358820 File Offset: 0x00356A20
	// (set) Token: 0x06008CEA RID: 36074 RVA: 0x00358828 File Offset: 0x00356A28
	public XUiView NavUpTarget
	{
		get
		{
			return this.navUpTarget;
		}
		set
		{
			this.navUpTarget = value;
			foreach (XUiController xuiController in this.Controller.Children)
			{
				xuiController.ViewComponent.NavUpTarget = value;
			}
		}
	}

	// Token: 0x170010A0 RID: 4256
	// (get) Token: 0x06008CEB RID: 36075 RVA: 0x0035888C File Offset: 0x00356A8C
	// (set) Token: 0x06008CEC RID: 36076 RVA: 0x00358894 File Offset: 0x00356A94
	[XuiXmlAttribute("nav_down", false)]
	public string NavDownTargetString
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.navDownTargetString;
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			if (this.navDownTargetString == value)
			{
				return;
			}
			this.navDownTargetString = value;
			this.SetDirty();
		}
	}

	// Token: 0x170010A1 RID: 4257
	// (get) Token: 0x06008CED RID: 36077 RVA: 0x003588B2 File Offset: 0x00356AB2
	// (set) Token: 0x06008CEE RID: 36078 RVA: 0x003588BC File Offset: 0x00356ABC
	public XUiView NavDownTarget
	{
		get
		{
			return this.navDownTarget;
		}
		set
		{
			this.navDownTarget = value;
			foreach (XUiController xuiController in this.Controller.Children)
			{
				xuiController.ViewComponent.navDownTarget = value;
			}
		}
	}

	// Token: 0x170010A2 RID: 4258
	// (get) Token: 0x06008CEF RID: 36079 RVA: 0x00358920 File Offset: 0x00356B20
	// (set) Token: 0x06008CF0 RID: 36080 RVA: 0x00358928 File Offset: 0x00356B28
	[XuiXmlAttribute("nav_left", false)]
	public string NavLeftTargetString
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.navLeftTargetString;
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			if (this.navLeftTargetString == value)
			{
				return;
			}
			this.navLeftTargetString = value;
			this.SetDirty();
		}
	}

	// Token: 0x170010A3 RID: 4259
	// (get) Token: 0x06008CF1 RID: 36081 RVA: 0x00358946 File Offset: 0x00356B46
	// (set) Token: 0x06008CF2 RID: 36082 RVA: 0x00358950 File Offset: 0x00356B50
	public XUiView NavLeftTarget
	{
		get
		{
			return this.navLeftTarget;
		}
		set
		{
			this.navLeftTarget = value;
			foreach (XUiController xuiController in this.Controller.Children)
			{
				xuiController.ViewComponent.navLeftTarget = value;
			}
		}
	}

	// Token: 0x170010A4 RID: 4260
	// (get) Token: 0x06008CF3 RID: 36083 RVA: 0x003589B4 File Offset: 0x00356BB4
	// (set) Token: 0x06008CF4 RID: 36084 RVA: 0x003589BC File Offset: 0x00356BBC
	[XuiXmlAttribute("nav_right", false)]
	public string NavRightTargetString
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.navRightTargetString;
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			if (this.navRightTargetString == value)
			{
				return;
			}
			this.navRightTargetString = value;
			this.SetDirty();
		}
	}

	// Token: 0x170010A5 RID: 4261
	// (get) Token: 0x06008CF5 RID: 36085 RVA: 0x003589DA File Offset: 0x00356BDA
	// (set) Token: 0x06008CF6 RID: 36086 RVA: 0x003589E4 File Offset: 0x00356BE4
	public XUiView NavRightTarget
	{
		get
		{
			return this.navRightTarget;
		}
		set
		{
			this.navRightTarget = value;
			foreach (XUiController xuiController in this.Controller.Children)
			{
				xuiController.ViewComponent.navRightTarget = value;
			}
		}
	}

	// Token: 0x170010A6 RID: 4262
	// (get) Token: 0x06008CF7 RID: 36087 RVA: 0x00358A48 File Offset: 0x00356C48
	// (set) Token: 0x06008CF8 RID: 36088 RVA: 0x00358A50 File Offset: 0x00356C50
	[XuiXmlAttribute("tooltip", false)]
	public string ToolTip
	{
		get
		{
			return this.toolTip;
		}
		set
		{
			if (this.toolTip == value)
			{
				return;
			}
			if (GameManager.Instance.GameIsFocused && this.enabled && this.isOver && this.xui.ToolTipWindow != null && this.xui.ToolTipWindow.ToolTip == this.toolTip)
			{
				this.xui.ToolTipWindow.ToolTip = value;
			}
			this.toolTip = value;
		}
	}

	// Token: 0x170010A7 RID: 4263
	// (get) Token: 0x06008CF9 RID: 36089 RVA: 0x00358ACA File Offset: 0x00356CCA
	// (set) Token: 0x06008CFA RID: 36090 RVA: 0x00358AD4 File Offset: 0x00356CD4
	[XuiXmlAttribute("disabled_tooltip", false)]
	public string DisabledToolTip
	{
		get
		{
			return this.disabledToolTip;
		}
		set
		{
			if (this.disabledToolTip == value)
			{
				return;
			}
			if (GameManager.Instance.GameIsFocused && this.enabled && this.isOver && this.xui.ToolTipWindow != null && this.xui.ToolTipWindow.ToolTip == this.disabledToolTip)
			{
				this.xui.ToolTipWindow.ToolTip = value;
			}
			this.disabledToolTip = value;
		}
	}

	// Token: 0x06008CFB RID: 36091 RVA: 0x00358B4E File Offset: 0x00356D4E
	[XuiXmlAttribute("tooltip_key", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeTooltipKey(string _value)
	{
		this.ToolTip = Localization.Get(_value, false, null);
	}

	// Token: 0x06008CFC RID: 36092 RVA: 0x00358B5E File Offset: 0x00356D5E
	[XuiXmlAttribute("disabled_tooltip_key", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeDisabledTooltipKey(string _value)
	{
		this.DisabledToolTip = Localization.Get(_value, false, null);
	}

	// Token: 0x06008CFD RID: 36093 RVA: 0x00358B70 File Offset: 0x00356D70
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiView(XUi _xui, string _id)
	{
		this.xui = _xui;
		this.id = _id;
	}

	// Token: 0x06008CFE RID: 36094 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void createComponents(GameObject _go)
	{
	}

	// Token: 0x06008CFF RID: 36095 RVA: 0x00358C34 File Offset: 0x00356E34
	[PublicizedFrom(EAccessModifier.Private)]
	public void buildView()
	{
		Type type = base.GetType();
		GameObject gameObject;
		if (XUiView.componentTemplates.TryGetValue(type, out gameObject))
		{
			this.uiTransform = global::UnityEngine.Object.Instantiate<GameObject>(gameObject).transform;
			return;
		}
		if (XUiView.templatesParent == null)
		{
			Transform transform = this.xui.playerUI.uiCamera.transform;
			GameObject gameObject2 = new GameObject("_ViewTemplates");
			gameObject2.SetActive(false);
			XUiView.templatesParent = gameObject2.transform;
			XUiView.templatesParent.parent = transform;
		}
		GameObject gameObject3 = new GameObject(type.Name)
		{
			layer = 12,
			transform = 
			{
				parent = XUiView.templatesParent
			}
		};
		gameObject3.AddComponent<BoxCollider>().enabled = false;
		UIEventListener.Get(gameObject3);
		this.createComponents(gameObject3);
		XUiView.componentTemplates[type] = gameObject3;
		this.uiTransform = global::UnityEngine.Object.Instantiate<GameObject>(gameObject3).transform;
	}

	// Token: 0x06008D00 RID: 36096 RVA: 0x00358D0D File Offset: 0x00356F0D
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void captureComponents()
	{
		this.collider = this.uiTransform.gameObject.GetComponent<BoxCollider>();
	}

	// Token: 0x06008D01 RID: 36097 RVA: 0x00358D28 File Offset: 0x00356F28
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void addTweeners()
	{
		for (int i = 0; i < this.Tweeners.Count; i++)
		{
			this.Tweeners[i].CreateTween(this.uiTransform.gameObject);
		}
	}

	// Token: 0x06008D02 RID: 36098 RVA: 0x00358D68 File Offset: 0x00356F68
	public virtual void InitView()
	{
		this.buildView();
		this.uiTransform.name = this.id;
		this.captureComponents();
		this.addTweeners();
		XUiController parent = this.controller.Parent;
		if (((parent != null) ? parent.ViewComponent : null) != null)
		{
			XUiView viewComponent = this.controller.Parent.ViewComponent;
			this.uiTransform.parent = viewComponent.uiTransform;
			this.uiTransform.localScale = Vector3.one;
			this.uiTransform.localPosition = new Vector3((float)this.PaddedPosition.x, (float)this.PaddedPosition.y, 0f);
			this.uiTransform.localEulerAngles = new Vector3(0f, 0f, this.rotation);
		}
		if (this.HasAnyEvent)
		{
			this.collider.enabled = true;
			this.refreshBoxCollider();
			UIEventListener uieventListener = UIEventListener.Get(this.uiTransform.gameObject);
			uieventListener.onHover = (UIEventListener.BoolDelegate)Delegate.Combine(uieventListener.onHover, new UIEventListener.BoolDelegate(delegate(GameObject _, bool _state)
			{
				this.OnHover(_state);
			}));
			uieventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uieventListener.onClick, new UIEventListener.VoidDelegate(this.OnClick));
			uieventListener.onDoubleClick = (UIEventListener.VoidDelegate)Delegate.Combine(uieventListener.onDoubleClick, new UIEventListener.VoidDelegate(this.OnDoubleClick));
			uieventListener.onDrag = (UIEventListener.VectorDelegate)Delegate.Combine(uieventListener.onDrag, new UIEventListener.VectorDelegate(this.OnDrag));
			uieventListener.onPress = (UIEventListener.BoolDelegate)Delegate.Combine(uieventListener.onPress, new UIEventListener.BoolDelegate(this.OnPress));
			uieventListener.onScroll = (UIEventListener.FloatDelegate)Delegate.Combine(uieventListener.onScroll, new UIEventListener.FloatDelegate(this.OnScroll));
			uieventListener.onSelect = (UIEventListener.BoolDelegate)Delegate.Combine(uieventListener.onSelect, new UIEventListener.BoolDelegate(this.OnSelect));
			uieventListener.onDragOut = (UIEventListener.VoidDelegate)Delegate.Combine(uieventListener.onDragOut, new UIEventListener.VoidDelegate(this.OnHeldDragOut));
		}
		if (this.uiTransform.gameObject.activeSelf != this.isVisible)
		{
			this.uiTransform.gameObject.SetActive(this.isVisible);
		}
		if (!this.gamepadSelectableSetFromAttributes)
		{
			this.IsNavigatable = this.EventOnPress;
		}
		if (!this.gamepadSnappableSetFromAttributes)
		{
			this.IsSnappable = this.EventOnPress;
		}
	}

	// Token: 0x06008D03 RID: 36099 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void SetRepeatContentTemplateParams(Dictionary<string, object> _templateParams, int _curRepeatNum)
	{
	}

	// Token: 0x06008D04 RID: 36100 RVA: 0x00358FC2 File Offset: 0x003571C2
	public virtual void Cleanup()
	{
		this.destroyed = true;
	}

	// Token: 0x06008D05 RID: 36101 RVA: 0x00358FCC File Offset: 0x003571CC
	public virtual void Update(float _dt)
	{
		if (this.isOver && !this.UiTransformIsHovered)
		{
			using (this.pmUpdateHover.Auto())
			{
				this.OnHover(false);
			}
		}
		if (this.isPressed && this.enabled)
		{
			using (this.pmUpdatePress.Auto())
			{
				float unscaledTime = Time.unscaledTime;
				if (!this.isHold)
				{
					this.isHold = unscaledTime - this.pressStartTime >= this.HoldDelay;
					if (this.isHold)
					{
						this.holdStartTime = unscaledTime;
						this.controller.Held(EHoldType.HoldStart, 0f, -1f);
						this.holdEventNextTime = 0f;
						this.holdEventLastTime = unscaledTime;
						this.holdEventIntervalChangeSpeed = 0f;
						this.holdEventIntervalCurrent = this.HoldEventIntervalInitial;
					}
				}
				else
				{
					this.controller.Held(EHoldType.Hold, unscaledTime - this.holdStartTime, -1f);
				}
				if (this.isHold && unscaledTime >= this.holdEventNextTime)
				{
					this.holdEventIntervalCurrent = Mathf.SmoothDamp(this.holdEventIntervalCurrent, this.HoldEventIntervalFinal, ref this.holdEventIntervalChangeSpeed, this.HoldEventIntervalAcceleration, float.PositiveInfinity, _dt);
					this.holdEventNextTime = unscaledTime + this.holdEventIntervalCurrent;
					this.controller.Held(EHoldType.HoldTimed, unscaledTime - this.holdStartTime, unscaledTime - this.holdEventLastTime);
					this.holdEventLastTime = unscaledTime;
				}
			}
		}
		if (this.isDirty)
		{
			using (this.pmUpdateData.Auto())
			{
				this.updateData();
				this.isDirty = false;
			}
		}
	}

	// Token: 0x06008D06 RID: 36102 RVA: 0x00359194 File Offset: 0x00357394
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void updateData()
	{
		if (this.positionDirty)
		{
			this.uiTransform.localPosition = new Vector3((float)this.PaddedPosition.x, (float)this.PaddedPosition.y, this.uiTransform.localPosition.z);
			this.positionDirty = false;
		}
		if (this.rotationDirty)
		{
			this.uiTransform.localEulerAngles = new Vector3(0f, 0f, this.rotation);
			this.rotationDirty = false;
		}
		this.parseAnchors();
		this.parseNavigationTargets();
	}

	// Token: 0x06008D07 RID: 36103 RVA: 0x00359224 File Offset: 0x00357424
	public void TryUpdatePosition()
	{
		if (this.positionDirty)
		{
			this.uiTransform.localPosition = new Vector3((float)this.PaddedPosition.x, (float)this.PaddedPosition.y, this.uiTransform.localPosition.z);
		}
	}

	// Token: 0x06008D08 RID: 36104 RVA: 0x00359274 File Offset: 0x00357474
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void refreshBoxCollider()
	{
		this.collider.center = new Vector3((float)this.size.x * 0.5f, 0f - (float)this.size.y * 0.5f, 0f);
		this.collider.size = new Vector3((float)this.size.x * this.ColliderScale + (float)(2 * this.ColliderPadding.x), (float)this.size.y * this.ColliderScale + (float)(2 * this.ColliderPadding.y), 0f);
	}

	// Token: 0x06008D09 RID: 36105 RVA: 0x00359319 File Offset: 0x00357519
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void SetDirty()
	{
		this.isDirty = true;
	}

	// Token: 0x170010A8 RID: 4264
	// (get) Token: 0x06008D0A RID: 36106 RVA: 0x00359322 File Offset: 0x00357522
	// (set) Token: 0x06008D0B RID: 36107 RVA: 0x0035932A File Offset: 0x0035752A
	[XuiXmlAttribute("anchor_left", false)]
	public string AnchorLeft
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.anchorLeft;
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			if (this.anchorLeft == value)
			{
				return;
			}
			this.anchorLeft = value;
			this.SetDirty();
		}
	}

	// Token: 0x170010A9 RID: 4265
	// (get) Token: 0x06008D0C RID: 36108 RVA: 0x00359348 File Offset: 0x00357548
	// (set) Token: 0x06008D0D RID: 36109 RVA: 0x00359350 File Offset: 0x00357550
	[XuiXmlAttribute("anchor_right", false)]
	public string AnchorRight
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.anchorRight;
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			if (this.anchorRight == value)
			{
				return;
			}
			this.anchorRight = value;
			this.SetDirty();
		}
	}

	// Token: 0x170010AA RID: 4266
	// (get) Token: 0x06008D0E RID: 36110 RVA: 0x0035936E File Offset: 0x0035756E
	// (set) Token: 0x06008D0F RID: 36111 RVA: 0x00359376 File Offset: 0x00357576
	[XuiXmlAttribute("anchor_bottom", false)]
	public string AnchorBottom
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.anchorBottom;
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			if (this.anchorBottom == value)
			{
				return;
			}
			this.anchorBottom = value;
			this.SetDirty();
		}
	}

	// Token: 0x170010AB RID: 4267
	// (get) Token: 0x06008D10 RID: 36112 RVA: 0x00359394 File Offset: 0x00357594
	// (set) Token: 0x06008D11 RID: 36113 RVA: 0x0035939C File Offset: 0x0035759C
	[XuiXmlAttribute("anchor_top", false)]
	public string AnchorTop
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.anchorTop;
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			if (this.anchorTop == value)
			{
				return;
			}
			this.anchorTop = value;
			this.SetDirty();
		}
	}

	// Token: 0x170010AC RID: 4268
	// (get) Token: 0x06008D12 RID: 36114 RVA: 0x003593BA File Offset: 0x003575BA
	public bool HasAnchorStringLeft
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return !string.IsNullOrEmpty(this.anchorLeft);
		}
	}

	// Token: 0x170010AD RID: 4269
	// (get) Token: 0x06008D13 RID: 36115 RVA: 0x003593CA File Offset: 0x003575CA
	public bool HasAnchorStringRight
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return !string.IsNullOrEmpty(this.anchorRight);
		}
	}

	// Token: 0x170010AE RID: 4270
	// (get) Token: 0x06008D14 RID: 36116 RVA: 0x003593DA File Offset: 0x003575DA
	public bool HasAnchorStringTop
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return !string.IsNullOrEmpty(this.anchorTop);
		}
	}

	// Token: 0x170010AF RID: 4271
	// (get) Token: 0x06008D15 RID: 36117 RVA: 0x003593EA File Offset: 0x003575EA
	public bool HasAnchorStringBottom
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return !string.IsNullOrEmpty(this.anchorBottom);
		}
	}

	// Token: 0x170010B0 RID: 4272
	// (get) Token: 0x06008D16 RID: 36118 RVA: 0x003593FA File Offset: 0x003575FA
	public bool HasAnchorStringLeftAndRight
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.HasAnchorStringLeft && this.HasAnchorStringRight;
		}
	}

	// Token: 0x170010B1 RID: 4273
	// (get) Token: 0x06008D17 RID: 36119 RVA: 0x0035940C File Offset: 0x0035760C
	public bool HasAnchorStringTopAndBottom
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.HasAnchorStringTop && this.HasAnchorStringBottom;
		}
	}

	// Token: 0x06008D18 RID: 36120 RVA: 0x00359420 File Offset: 0x00357620
	[PublicizedFrom(EAccessModifier.Protected)]
	public void parseAnchors()
	{
		UIRect uiRect = this.UiRect;
		if (uiRect == null)
		{
			return;
		}
		if (this.<parseAnchors>g__ParseAnchorString|281_0(this.anchorLeft, ref this.anchorLeftParsed, uiRect, uiRect.leftAnchor) | this.<parseAnchors>g__ParseAnchorString|281_0(this.anchorRight, ref this.anchorRightParsed, uiRect, uiRect.rightAnchor) | this.<parseAnchors>g__ParseAnchorString|281_0(this.anchorBottom, ref this.anchorBottomParsed, uiRect, uiRect.bottomAnchor) | this.<parseAnchors>g__ParseAnchorString|281_0(this.anchorTop, ref this.anchorTopParsed, uiRect, uiRect.topAnchor))
		{
			this.anchoredLeftAndRight = uiRect.leftAnchor.target && uiRect.rightAnchor.target;
			this.anchoredTopAndBottom = uiRect.bottomAnchor.target && uiRect.topAnchor.target;
			this.SetDirty();
			uiRect.ResetAnchors();
		}
		this.anchorsParsed();
	}

	// Token: 0x06008D19 RID: 36121 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void anchorsParsed()
	{
	}

	// Token: 0x06008D1A RID: 36122 RVA: 0x0035950F File Offset: 0x0035770F
	public virtual void OnOpen()
	{
		if (this.xuiSound != null && this.SoundPlayOnOpen)
		{
			Manager.PlayXUiSound(this.xuiSound, this.SoundVolume);
		}
		this.isPressed = false;
		this.isHold = false;
	}

	// Token: 0x06008D1B RID: 36123 RVA: 0x00359548 File Offset: 0x00357748
	public virtual void OnClose()
	{
		this.isPressed = false;
		this.isHold = false;
		if (!GameManager.Instance.IsQuitting)
		{
			CursorControllerAbs cursorController = this.xui.playerUI.CursorController;
			if (cursorController.HoverTarget == this)
			{
				cursorController.HoverTarget = null;
			}
			if (cursorController.navigationTarget == this)
			{
				this.controller.Hovered(false);
				cursorController.SetNavigationTarget(null);
			}
			if (cursorController.lockNavigationToView == this)
			{
				cursorController.SetNavigationLockView(null, null);
			}
		}
	}

	// Token: 0x06008D1C RID: 36124 RVA: 0x003595BD File Offset: 0x003577BD
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnScroll(GameObject _go, float _delta)
	{
		if (this.EventOnScroll)
		{
			this.controller.Scrolled(_delta);
		}
	}

	// Token: 0x06008D1D RID: 36125 RVA: 0x003595D3 File Offset: 0x003577D3
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnSelect(GameObject _go, bool _selected)
	{
		if (this.EventOnSelect && this.enabled)
		{
			this.controller.Selected(_selected);
		}
	}

	// Token: 0x06008D1E RID: 36126 RVA: 0x003595F4 File Offset: 0x003577F4
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDrag(GameObject _go, Vector2 _delta)
	{
		if (this.EventOnDrag && this.enabled)
		{
			EDragType edragType = (this.wasDragging ? EDragType.Dragging : EDragType.DragStart);
			this.wasDragging = true;
			this.controller.Dragged(_delta, edragType);
		}
	}

	// Token: 0x06008D1F RID: 36127 RVA: 0x00359634 File Offset: 0x00357834
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnPress(GameObject _go, bool _pressed)
	{
		if (this.EventOnPress && this.enabled)
		{
			this.controller.MouseUpDown(_pressed);
		}
		if (this.EventOnDrag && this.enabled && !_pressed && this.wasDragging)
		{
			this.wasDragging = false;
			this.controller.Dragged(default(Vector2), EDragType.DragEnd);
		}
		if (this.EventOnHeld)
		{
			if (_pressed && !this.isPressed && this.enabled)
			{
				this.isPressed = true;
				this.isHold = false;
				this.pressStartTime = Time.unscaledTime;
				this.holdStartTime = -1f;
				return;
			}
			if (!_pressed)
			{
				this.isPressed = false;
				bool flag = this.isHold;
				this.isHold = false;
				if (flag)
				{
					this.controller.Held(EHoldType.HoldEnd, Time.unscaledTime - this.holdStartTime, -1f);
				}
			}
		}
	}

	// Token: 0x06008D20 RID: 36128 RVA: 0x0035970C File Offset: 0x0035790C
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnHeldDragOut(GameObject _go)
	{
		if (this.EventOnHeld && this.isPressed)
		{
			this.isPressed = false;
			bool flag = this.isHold;
			this.isHold = false;
			if (flag)
			{
				this.controller.Held(EHoldType.HoldEnd, Time.unscaledTime - this.holdStartTime, -1f);
			}
		}
	}

	// Token: 0x06008D21 RID: 36129 RVA: 0x0035975C File Offset: 0x0035795C
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnClick(GameObject _go)
	{
		if (this.EventOnPress && this.enabled)
		{
			if (this.xuiSound != null && this.SoundPlayOnClick && UICamera.currentTouchID == -1)
			{
				Manager.PlayXUiSound(this.xuiSound, this.SoundVolume);
			}
			this.controller.Pressed(UICamera.currentTouchID);
		}
	}

	// Token: 0x06008D22 RID: 36130 RVA: 0x003597B8 File Offset: 0x003579B8
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDoubleClick(GameObject _go)
	{
		if (this.EventOnDoubleClick && this.enabled)
		{
			if (this.xuiSound != null && this.SoundPlayOnClick && UICamera.currentTouchID == -1)
			{
				Manager.PlayXUiSound(this.xuiSound, this.SoundVolume);
			}
			this.controller.DoubleClicked(UICamera.currentTouchID);
		}
	}

	// Token: 0x06008D23 RID: 36131 RVA: 0x00359814 File Offset: 0x00357A14
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnHover(bool _isOver)
	{
		PlayerActionsLocal playerInput = this.xui.playerUI.playerInput;
		if (playerInput != null && playerInput.LastDeviceClass == InputDeviceClass.Keyboard && !Cursor.visible)
		{
			_isOver = false;
		}
		bool flag = _isOver;
		bool flag2 = _isOver && !this.enabled && !string.IsNullOrEmpty(this.DisabledToolTip);
		_isOver &= this.enabled;
		bool flag3 = _isOver && !string.IsNullOrEmpty(this.ToolTip);
		if (_isOver != this.isOver && _isOver)
		{
			this.PlayHoverSound();
		}
		this.isOver = _isOver;
		if (this.EventOnHover)
		{
			this.controller.Hovered(_isOver);
		}
		if (this.xui.ToolTipWindow != null)
		{
			if (flag3)
			{
				this.xui.ToolTipWindow.ToolTip = this.ToolTip;
			}
			else if (flag2)
			{
				this.xui.ToolTipWindow.ToolTip = this.DisabledToolTip;
			}
			else
			{
				this.xui.ToolTipWindow.ToolTip = "";
			}
		}
		this.xui.playerUI.CursorController.HoverTarget = (flag ? this : null);
	}

	// Token: 0x06008D24 RID: 36132 RVA: 0x0035992F File Offset: 0x00357B2F
	public void PlayHoverSound()
	{
		if (this.xuiHoverSound != null && this.SoundPlayOnHover && this.enabled && GameManager.Instance.GameIsFocused)
		{
			Manager.PlayXUiSound(this.xuiHoverSound, this.SoundVolume);
		}
	}

	// Token: 0x06008D25 RID: 36133 RVA: 0x0035996C File Offset: 0x00357B6C
	public void PlayClickSound()
	{
		if (this.xuiSound != null)
		{
			Manager.PlayXUiSound(this.xuiSound, this.SoundVolume);
		}
	}

	// Token: 0x06008D26 RID: 36134 RVA: 0x0035998D File Offset: 0x00357B8D
	public Rect GetXUiRect()
	{
		return this.GetXUiRect(Vector2.zero);
	}

	// Token: 0x06008D27 RID: 36135 RVA: 0x0035999C File Offset: 0x00357B9C
	public Rect GetXUiRect(Vector2 _padding)
	{
		XUiController parent = this.controller;
		Vector3 vector = this.uiTransform.localPosition;
		vector.x -= _padding.x;
		vector.y += _padding.y;
		while (parent.Parent != null && parent.Parent.ViewComponent != null)
		{
			parent = parent.Parent;
			vector += parent.ViewComponent.uiTransform.localPosition;
		}
		vector += parent.ViewComponent.uiTransform.parent.localPosition;
		Vector2 vector2 = this.Size.AsVector2() + 2f * _padding;
		XUiV_Window xuiV_Window = parent.ViewComponent as XUiV_Window;
		if (xuiV_Window != null && xuiV_Window.IsInStackPanel)
		{
			Transform parent2 = xuiV_Window.uiTransform.parent.parent;
			vector *= parent2.localScale.x;
			vector2 *= parent2.localScale.x;
			vector += parent2.localPosition;
		}
		return new Rect(new Vector2(vector.x, vector.y - vector2.y), vector2);
	}

	// Token: 0x06008D28 RID: 36136 RVA: 0x00359ACA File Offset: 0x00357CCA
	public string GetXuiHierarchy()
	{
		return XUiUtils.GetXuiHierarchy(this.Controller);
	}

	// Token: 0x06008D29 RID: 36137 RVA: 0x00359AD7 File Offset: 0x00357CD7
	public Vector2 GetClosestPoint(Vector3 _point)
	{
		return this.collider.ClosestPointOnBounds(_point);
	}

	// Token: 0x06008D2A RID: 36138 RVA: 0x00359AEC File Offset: 0x00357CEC
	public void ClearNavigationTargets()
	{
		this.NavUpTarget = (this.NavDownTarget = (this.NavLeftTarget = (this.NavRightTarget = null)));
	}

	// Token: 0x06008D2B RID: 36139 RVA: 0x00359B1C File Offset: 0x00357D1C
	[PublicizedFrom(EAccessModifier.Private)]
	public void parseNavigationTargets()
	{
		if (!string.IsNullOrEmpty(this.navUpTargetString))
		{
			this.NavUpTarget = this.<parseNavigationTargets>g__FindView|310_0(this.navUpTargetString);
		}
		if (!string.IsNullOrEmpty(this.navDownTargetString))
		{
			this.NavDownTarget = this.<parseNavigationTargets>g__FindView|310_0(this.navDownTargetString);
		}
		if (!string.IsNullOrEmpty(this.navLeftTargetString))
		{
			this.NavLeftTarget = this.<parseNavigationTargets>g__FindView|310_0(this.navLeftTargetString);
		}
		if (!string.IsNullOrEmpty(this.navRightTargetString))
		{
			this.NavRightTarget = this.<parseNavigationTargets>g__FindView|310_0(this.navRightTargetString);
		}
	}

	// Token: 0x06008D2C RID: 36140 RVA: 0x00359BA8 File Offset: 0x00357DA8
	public virtual void SetDefaults(XUiController _parent)
	{
		this.RepeatContent = false;
		this.RepeatCount = 1;
		this.Size = Vector2i.min;
		int? num;
		if (_parent == null)
		{
			num = null;
		}
		else
		{
			XUiView viewComponent = _parent.ViewComponent;
			num = ((viewComponent != null) ? new int?(viewComponent.Depth) : null);
		}
		int? num2 = num;
		this.Depth = num2.GetValueOrDefault();
		this.ToolTip = "";
		this.SoundPlayOnClick = true;
		this.SoundPlayOnOpen = false;
		this.SoundPlayOnHover = true;
		this.SoundVolume = 1f;
	}

	// Token: 0x06008D2D RID: 36141 RVA: 0x00359C34 File Offset: 0x00357E34
	public virtual void SetPostParsingDefaults(XUiController _parent)
	{
		XUiView xuiView = ((_parent != null) ? _parent.ViewComponent : null);
		Vector2i vector2i = this.Size;
		if (vector2i.x == -2147483648)
		{
			vector2i.x = (this.ignoreParentPadding ? ((xuiView != null) ? xuiView.Size.x : 0) : ((xuiView != null) ? xuiView.InnerSize.x : 0));
		}
		if (vector2i.y == -2147483648)
		{
			vector2i.y = (this.ignoreParentPadding ? ((xuiView != null) ? xuiView.Size.y : 0) : ((xuiView != null) ? xuiView.InnerSize.y : 0));
		}
		this.Size = vector2i;
	}

	// Token: 0x06008D2E RID: 36142 RVA: 0x00359CDC File Offset: 0x00357EDC
	public void ParseInitialAttributeValue(string _attribute, string _value)
	{
		if (_value.Contains("{"))
		{
			BindingsManager.CreateBinding(this, _attribute, _value);
			return;
		}
		if (ParsingMethodCache.Instance.TryParseDirect(this, _attribute, _value))
		{
			return;
		}
		if (this.Controller != null && ParsingMethodCache.Instance.TryParseDirect(this.Controller, _attribute, _value))
		{
			return;
		}
		this.ParseAttributeViewAndController(_attribute, _value);
	}

	// Token: 0x06008D2F RID: 36143 RVA: 0x00359D38 File Offset: 0x00357F38
	public bool ParseAttributeViewAndController(string _attribute, string _value)
	{
		if (_value.Contains("{") && XUiFromXml.DebugXuiLoading == XUiFromXml.DebugLevel.Verbose)
		{
			Log.Warning(string.Concat(new string[] { "[XUi] Refreshed binding contained '{': ", _attribute, "='", _value, "' on ", this.id }));
		}
		if (this.Controller != null)
		{
			if (!this.Controller.ParseAttribute(_attribute, _value))
			{
				this.Controller.CustomAttributes[_attribute] = _value;
			}
			return true;
		}
		return false;
	}

	// Token: 0x06008D30 RID: 36144 RVA: 0x00359DBF File Offset: 0x00357FBF
	public virtual void OnVisibilityChanged(bool _visibleInScene)
	{
		if (this.isOver && !_visibleInScene)
		{
			this.OnHover(false);
		}
	}

	// Token: 0x06008D35 RID: 36149 RVA: 0x00359DFC File Offset: 0x00357FFC
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public bool <parseAnchors>g__ParseAnchorString|281_0(string _anchorString, ref string _parsedString, UIRect _uiRect, UIRect.AnchorPoint _anchor)
	{
		if (string.IsNullOrEmpty(_anchorString))
		{
			return false;
		}
		if (_anchorString == _parsedString && _anchor.target != null)
		{
			return false;
		}
		_parsedString = _anchorString;
		int num = _anchorString.IndexOf(',');
		if (num < 0)
		{
			throw new ArgumentException("Invalid anchor string '" + _anchorString + "', expected '<target>,<relative>,<absolute>'");
		}
		string text = _anchorString.Substring(0, num);
		int num2 = _anchorString.IndexOf(',', num + 1);
		if (num2 < 0)
		{
			throw new ArgumentException("Invalid anchor string '" + _anchorString + "', expected '<target>,<relative>,<absolute>'");
		}
		float num3 = StringParsers.ParseFloat(_anchorString, num + 1, num2 - 1, NumberStyles.Any);
		int num4 = StringParsers.ParseSInt32(_anchorString, num2 + 1, -1, NumberStyles.Integer);
		if (text.Length == 0)
		{
			throw new ArgumentException("Invalid anchor string '" + _anchorString + "', expected '<target>,<relative>,<absolute>'");
		}
		if (text.EqualsCaseInsensitive("#parent"))
		{
			_anchor.target = this.uiTransform.parent;
		}
		else if (text.EqualsCaseInsensitive("#cam"))
		{
			UICamera componentInParent = this.uiTransform.gameObject.GetComponentInParent<UICamera>();
			if (componentInParent == null)
			{
				throw new Exception("UICamera not found");
			}
			_anchor.target = componentInParent.transform;
		}
		else if (text[0] == '#')
		{
			string text2 = text.Substring(1);
			UIAnchor.Side side;
			if (!EnumUtils.TryParse<UIAnchor.Side>(text2, out side, true))
			{
				throw new ArgumentException("Invalid anchor side name '" + text2 + "', expected any of '\tBottomLeft,Left,TopLeft,Top,TopRight,Right,BottomRight,Bottom,Center'");
			}
			_anchor.target = this.xui.GetAnchor(side).transform;
		}
		else
		{
			XUiView xuiView = XUiUtils.FindHierarchyClosestView(this.Controller, text);
			if (xuiView == null)
			{
				throw new ArgumentException(string.Concat(new string[]
				{
					"Invalid anchor string '",
					_anchorString,
					"', view component with name '",
					text,
					"' not found.\nOn: ",
					this.GetXuiHierarchy()
				}));
			}
			_anchor.target = xuiView.UiTransform;
			XUiV_Grid xuiV_Grid = xuiView as XUiV_Grid;
			if (xuiV_Grid != null)
			{
				xuiV_Grid.OnSizeChanged += delegate(Vector2Int _, Vector2 _)
				{
					_uiRect.UpdateAnchors();
				};
			}
		}
		_anchor.relative = num3;
		_anchor.absolute = num4;
		return true;
	}

	// Token: 0x06008D36 RID: 36150 RVA: 0x0035A018 File Offset: 0x00358218
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiView <parseNavigationTargets>g__FindView|310_0(string _name)
	{
		if (string.IsNullOrEmpty(_name))
		{
			return null;
		}
		XUiView xuiView = XUiUtils.FindHierarchyClosestView(this.Controller, _name);
		if (xuiView != null)
		{
			return xuiView;
		}
		XUiController childById = this.controller.WindowGroup.Controller.GetChildById(_name);
		xuiView = ((childById != null) ? childById.ViewComponent : null);
		if (xuiView != null)
		{
			return xuiView;
		}
		Log.Error("Invalid navigation target, view component with name '" + _name + "' not found.\nOn: " + this.GetXuiHierarchy());
		return null;
	}

	// Token: 0x040067CE RID: 26574
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Dictionary<Type, GameObject> componentTemplates = new Dictionary<Type, GameObject>();

	// Token: 0x040067CF RID: 26575
	[PublicizedFrom(EAccessModifier.Private)]
	public static Transform templatesParent;

	// Token: 0x040067D0 RID: 26576
	public readonly XUi xui;

	// Token: 0x040067D1 RID: 26577
	[PublicizedFrom(EAccessModifier.Private)]
	public bool destroyed;

	// Token: 0x040067D2 RID: 26578
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiController controller;

	// Token: 0x040067D3 RID: 26579
	[PublicizedFrom(EAccessModifier.Protected)]
	public Transform uiTransform;

	// Token: 0x040067D4 RID: 26580
	[PublicizedFrom(EAccessModifier.Protected)]
	public BoxCollider collider;

	// Token: 0x040067D5 RID: 26581
	public readonly List<XUiTweenAbs> Tweeners = new List<XUiTweenAbs>();

	// Token: 0x040067D8 RID: 26584
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isDirty;

	// Token: 0x040067D9 RID: 26585
	[PublicizedFrom(EAccessModifier.Protected)]
	public string id;

	// Token: 0x040067DA RID: 26586
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isVisible = true;

	// Token: 0x040067DB RID: 26587
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool enabled = true;

	// Token: 0x040067DE RID: 26590
	[PublicizedFrom(EAccessModifier.Protected)]
	public Vector2i size;

	// Token: 0x040067DF RID: 26591
	[PublicizedFrom(EAccessModifier.Protected)]
	public Vector2i position;

	// Token: 0x040067E0 RID: 26592
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool positionDirty;

	// Token: 0x040067E1 RID: 26593
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool ignoreParentPadding;

	// Token: 0x040067E2 RID: 26594
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiSideSizes padding;

	// Token: 0x040067E3 RID: 26595
	[PublicizedFrom(EAccessModifier.Protected)]
	public float rotation;

	// Token: 0x040067E4 RID: 26596
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool rotationDirty;

	// Token: 0x040067E5 RID: 26597
	[PublicizedFrom(EAccessModifier.Protected)]
	public int depth;

	// Token: 0x040067E6 RID: 26598
	[PublicizedFrom(EAccessModifier.Private)]
	public AudioClip xuiSound;

	// Token: 0x040067E7 RID: 26599
	[PublicizedFrom(EAccessModifier.Private)]
	public AudioClip xuiHoverSound;

	// Token: 0x040067F0 RID: 26608
	[PublicizedFrom(EAccessModifier.Private)]
	public bool eventOnHover;

	// Token: 0x040067F1 RID: 26609
	[PublicizedFrom(EAccessModifier.Private)]
	public bool eventOnPress;

	// Token: 0x040067F2 RID: 26610
	[PublicizedFrom(EAccessModifier.Private)]
	public bool eventOnDoubleClick;

	// Token: 0x040067F3 RID: 26611
	[PublicizedFrom(EAccessModifier.Private)]
	public bool eventOnHeld;

	// Token: 0x040067F4 RID: 26612
	[PublicizedFrom(EAccessModifier.Private)]
	public bool eventOnScroll;

	// Token: 0x040067F5 RID: 26613
	[PublicizedFrom(EAccessModifier.Private)]
	public bool eventOnDrag;

	// Token: 0x040067F6 RID: 26614
	[PublicizedFrom(EAccessModifier.Private)]
	public bool eventOnSelect;

	// Token: 0x040067F7 RID: 26615
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isNavigatable = true;

	// Token: 0x040067FA RID: 26618
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool gamepadSelectableSetFromAttributes;

	// Token: 0x040067FB RID: 26619
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool gamepadSnappableSetFromAttributes;

	// Token: 0x040067FC RID: 26620
	[PublicizedFrom(EAccessModifier.Private)]
	public string navUpTargetString;

	// Token: 0x040067FD RID: 26621
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiView navUpTarget;

	// Token: 0x040067FE RID: 26622
	[PublicizedFrom(EAccessModifier.Private)]
	public string navDownTargetString;

	// Token: 0x040067FF RID: 26623
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiView navDownTarget;

	// Token: 0x04006800 RID: 26624
	[PublicizedFrom(EAccessModifier.Private)]
	public string navLeftTargetString;

	// Token: 0x04006801 RID: 26625
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiView navLeftTarget;

	// Token: 0x04006802 RID: 26626
	[PublicizedFrom(EAccessModifier.Private)]
	public string navRightTargetString;

	// Token: 0x04006803 RID: 26627
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiView navRightTarget;

	// Token: 0x04006804 RID: 26628
	[PublicizedFrom(EAccessModifier.Private)]
	public string toolTip;

	// Token: 0x04006805 RID: 26629
	[PublicizedFrom(EAccessModifier.Private)]
	public string disabledToolTip;

	// Token: 0x04006806 RID: 26630
	[PublicizedFrom(EAccessModifier.Private)]
	public ProfilerMarker pmUpdateHover = new ProfilerMarker("XV.U-Hover");

	// Token: 0x04006807 RID: 26631
	[PublicizedFrom(EAccessModifier.Private)]
	public ProfilerMarker pmUpdatePress = new ProfilerMarker("XV.U-Press");

	// Token: 0x04006808 RID: 26632
	[PublicizedFrom(EAccessModifier.Private)]
	public ProfilerMarker pmUpdateData = new ProfilerMarker("XV.U-UpdateData");

	// Token: 0x04006809 RID: 26633
	[PublicizedFrom(EAccessModifier.Private)]
	public string anchorLeft;

	// Token: 0x0400680A RID: 26634
	[PublicizedFrom(EAccessModifier.Private)]
	public string anchorRight;

	// Token: 0x0400680B RID: 26635
	[PublicizedFrom(EAccessModifier.Private)]
	public string anchorBottom;

	// Token: 0x0400680C RID: 26636
	[PublicizedFrom(EAccessModifier.Private)]
	public string anchorTop;

	// Token: 0x0400680D RID: 26637
	[PublicizedFrom(EAccessModifier.Private)]
	public string anchorLeftParsed;

	// Token: 0x0400680E RID: 26638
	[PublicizedFrom(EAccessModifier.Private)]
	public string anchorRightParsed;

	// Token: 0x0400680F RID: 26639
	[PublicizedFrom(EAccessModifier.Private)]
	public string anchorBottomParsed;

	// Token: 0x04006810 RID: 26640
	[PublicizedFrom(EAccessModifier.Private)]
	public string anchorTopParsed;

	// Token: 0x04006811 RID: 26641
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool anchoredLeftAndRight;

	// Token: 0x04006812 RID: 26642
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool anchoredTopAndBottom;

	// Token: 0x04006813 RID: 26643
	[PublicizedFrom(EAccessModifier.Private)]
	public bool wasDragging;

	// Token: 0x04006814 RID: 26644
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isPressed;

	// Token: 0x04006815 RID: 26645
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isHold;

	// Token: 0x04006816 RID: 26646
	[PublicizedFrom(EAccessModifier.Private)]
	public float pressStartTime;

	// Token: 0x04006817 RID: 26647
	[PublicizedFrom(EAccessModifier.Private)]
	public float holdStartTime;

	// Token: 0x04006818 RID: 26648
	[PublicizedFrom(EAccessModifier.Private)]
	public float holdEventIntervalCurrent;

	// Token: 0x04006819 RID: 26649
	[PublicizedFrom(EAccessModifier.Private)]
	public float holdEventIntervalChangeSpeed;

	// Token: 0x0400681A RID: 26650
	[PublicizedFrom(EAccessModifier.Private)]
	public float holdEventNextTime;

	// Token: 0x0400681B RID: 26651
	[PublicizedFrom(EAccessModifier.Private)]
	public float holdEventLastTime;

	// Token: 0x0400681C RID: 26652
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool isOver;
}

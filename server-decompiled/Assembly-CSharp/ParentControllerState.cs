using System;

// Token: 0x0200111E RID: 4382
public class ParentControllerState
{
	// Token: 0x06008ABB RID: 35515 RVA: 0x0034F280 File Offset: 0x0034D480
	public ParentControllerState(XUiController parentController)
	{
		if (parentController == null)
		{
			return;
		}
		if (parentController.ViewComponent == null)
		{
			this.m_parentControllers = new XUiController[parentController.Children.Count];
			for (int i = 0; i < parentController.Children.Count; i++)
			{
				this.m_parentControllers[i] = parentController.Children[i];
			}
		}
		else
		{
			this.m_parentControllers = new XUiController[] { parentController };
		}
		this.m_isVisible = new bool[this.m_parentControllers.Length];
		this.m_isEscClosable = new bool[this.m_parentControllers.Length];
		for (int j = 0; j < this.m_parentControllers.Length; j++)
		{
			this.m_isVisible[j] = this.m_parentControllers[j].ViewComponent.IsVisible;
			this.m_isEscClosable[j] = this.m_parentControllers[j].WindowGroup.isEscClosable;
		}
	}

	// Token: 0x06008ABC RID: 35516 RVA: 0x0034F360 File Offset: 0x0034D560
	public void Hide()
	{
		if (this.m_parentControllers == null)
		{
			return;
		}
		for (int i = 0; i < this.m_parentControllers.Length; i++)
		{
			this.m_parentControllers[i].ViewComponent.IsVisible = false;
			this.m_parentControllers[i].WindowGroup.isEscClosable = false;
		}
	}

	// Token: 0x06008ABD RID: 35517 RVA: 0x0034F3B0 File Offset: 0x0034D5B0
	public void Restore()
	{
		if (this.m_parentControllers == null)
		{
			return;
		}
		for (int i = 0; i < this.m_parentControllers.Length; i++)
		{
			this.m_parentControllers[i].ViewComponent.IsVisible = this.m_isVisible[i];
			this.m_parentControllers[i].WindowGroup.isEscClosable = this.m_isEscClosable[i];
		}
	}

	// Token: 0x0400670F RID: 26383
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiController[] m_parentControllers;

	// Token: 0x04006710 RID: 26384
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly bool[] m_isVisible;

	// Token: 0x04006711 RID: 26385
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly bool[] m_isEscClosable;
}

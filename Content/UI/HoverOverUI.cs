using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using Runeforge.Content.SkillTree;
using ReLogic.Content;

namespace Runeforge.Content.UI
{
	public class HoverOverUI : UIElement
	{
		private SkillTreePanel mainPanel;
		public UIImage hoverOverUIImage;
        public ConnectionDirection direction_from_node;
		public Asset<Texture2D> activeConnectionImage;
		public Asset<Texture2D> inActiveConnectionImage;
		public Asset<Texture2D> transparentImage;
		public UIText description;
		public bool active;
		private int id;
		private static int global_id = 0;
		public HoverOverUI(SkillTreePanel panel, Asset<Texture2D> inactive, Asset<Texture2D> active, Asset<Texture2D> transparentImage)
		{
			mainPanel = panel;
			hoverOverUIImage = new UIImage(transparentImage);
			this.description = new UIText("test", 0.7f);
			inActiveConnectionImage = inactive;
			this.transparentImage = transparentImage;
			activeConnectionImage = active;
			id = global_id;
			global_id++;
		}
		public int GetID()
		{
			return id;
		}

		public bool ChangeDescription(string newDescription)
		{
			if (description != null)
			{
				description.SetText(newDescription);
				return true;
			}
			return false;
		}

		public void SetActive()
		{
			hoverOverUIImage.SetImage(activeConnectionImage);
			active = true;
		}
		public void SetInActive()
		{
			hoverOverUIImage.SetImage(inActiveConnectionImage);
			active = false;
		}

		public void SetTransparent()
		{
			hoverOverUIImage.SetImage(transparentImage);
		}

		public override void OnInitialize()
		{
			base.OnInitialize();
			Width.Set(50, 0f);
			Height.Set(50, 0f);
			description.PaddingTop = 15;
			description.PaddingLeft = 15;
			description.SetText("");
			Append(hoverOverUIImage);
			hoverOverUIImage.Append(description);
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
		}

        public override int GetHashCode()
        {
			return hoverOverUIImage.GetHashCode();
        }
	}
}
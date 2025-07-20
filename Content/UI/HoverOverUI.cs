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
		public UIImage hoverOverUIImage;
        public ConnectionDirection direction_from_node;
		public Asset<Texture2D> activeConnectionImage;
		public Asset<Texture2D> inActiveConnectionImage;
		public Asset<Texture2D> transparentImage;
		public UIText description;
		public bool active;
		private int id;
		private static int global_id = 0;
		public HoverOverUI(TextureManager textureManager)
		{
			transparentImage = textureManager.transparent_box;
			(Asset<Texture2D> active, Asset<Texture2D> inactive) asset = textureManager.GetUI(UIType.hoverOver);
			hoverOverUIImage = new UIImage(transparentImage);
			this.description = new UIText("test", 0.7f); // DO NOT SET "test" to "" when changing font size, if font size is 1 then it can be ""
			inActiveConnectionImage = asset.inactive;
			activeConnectionImage = asset.active;
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
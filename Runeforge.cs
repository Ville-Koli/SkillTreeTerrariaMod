using Microsoft.Xna.Framework.Input;
using Terraria.ModLoader;

namespace Runeforge
{
	// Please read https://github.com/tModLoader/tModLoader/wiki/Basic-tModLoader-Modding-Guide#mod-skeleton-contents for more information about the various files in a mod.
	public class Runeforge : Mod
	{
		public static ModKeybind ToggleMyUIKeybind;
		public override void Load()
		{
			ToggleMyUIKeybind = KeybindLoader.RegisterKeybind(this, "Toggle Skilltree", Keys.G);
		}
        public override void Unload()
        {
            ToggleMyUIKeybind = null;
        }
	}
}

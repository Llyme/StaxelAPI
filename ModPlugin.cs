using Harmony;
using Plukit.Base;
using Staxel.Items;
using Staxel.Logic;
using Staxel.Tiles;
using System.IO;
using System.Reflection;

namespace StaxelAPI
{
	public abstract class ModPlugin : Staxel.Modding.IModHookV4
	{
		public readonly INI ini;
		public static Universe Universe { get; internal set; }
		public static Blob UniverseSettings { get; internal set; }

		public ModPlugin()
		{
			string name = Assembly.GetCallingAssembly().GetName().Name;
			ini = new INI(Path.Combine(Directory.GetCurrentDirectory(), "PluginConfig", $"{name}.ini"));
		}

		public virtual bool CanInteractWithEntity(Entity entity, Entity lookingAtEntity)
		{
			return true;
		}

		public virtual bool CanInteractWithTile(Entity entity, Vector3F location, Tile tile)
		{
			return true;
		}

		public virtual bool CanPlaceTile(Entity entity, Vector3I location, Tile tile, TileAccessFlags accessFlags)
		{
			return true;
		}

		public virtual bool CanRemoveTile(Entity entity, Vector3I location, TileAccessFlags accessFlags)
		{
			return true;
		}

		public virtual bool CanReplaceTile(Entity entity, Vector3I location, Tile tile, TileAccessFlags accessFlags)
		{
			return true;
		}

		public virtual void CleanupOldSession()
		{
		}

		public virtual void ClientContextDeinitialize()
		{
		}

		public virtual void ClientContextInitializeAfter()
		{
		}

		public virtual void ClientContextInitializeBefore()
		{
		}

		public virtual void ClientContextInitializeInit()
		{
		}

		public virtual void ClientContextReloadAfter()
		{
		}

		public virtual void ClientContextReloadBefore()
		{
		}

		public virtual void Dispose()
		{
		}

		public virtual void GameContextDeinitialize()
		{
		}

		public virtual void GameContextInitializeAfter()
		{
		}

		public virtual void GameContextInitializeBefore()
		{
		}

		public virtual void GameContextInitializeInit()
		{
		}

		public virtual void GameContextReloadAfter()
		{
		}

		public virtual void GameContextReloadBefore()
		{
		}

		public virtual void UniverseUpdateAfter()
		{
		}

		public virtual void UniverseUpdateBefore(Universe universe, Timestep step)
		{
			if (Universe != universe)
			{
				Universe = universe;
				UniverseSettings = new Traverse(universe).Field("_universeSettings").GetValue<Blob>();
			}

			ini.Update();

			if (!universe.Server)
				InputHelper.Update();
		}
	}
}

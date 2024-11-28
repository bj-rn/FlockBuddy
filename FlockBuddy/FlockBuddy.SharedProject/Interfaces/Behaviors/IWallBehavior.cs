using CollisionBuddy;
using Stride.Core.Mathematics;

namespace FlockBuddy.Interfaces.Behaviors
{
	/// <summary>
	/// This is a behavior that uses the walls.
	/// </summary>
	public interface IWallBehavior
	{
		List<ILine> Walls { set; }

		List<Vector2> Feelers { get; }
	}
}
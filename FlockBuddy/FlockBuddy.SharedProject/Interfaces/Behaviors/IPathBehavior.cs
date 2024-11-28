﻿using Stride.Core.Mathematics;

namespace FlockBuddy.Interfaces.Behaviors
{
	/// <summary>
	/// This is a behavior that includes a set of points
	/// </summary>
	public interface IPathBehavior
	{
		List<Vector2> Path { set; }
	}
}

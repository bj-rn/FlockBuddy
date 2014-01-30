using Microsoft.Xna.Framework;

namespace FlockBuddy
{
	/// <summary>
	/// this behavior is similar to seek but it attempts to arrive at the target position with a zero velocity
	/// </summary>
	public class Arrive : BaseBehavior
	{
		#region Members

		#endregion //Members

		#region Methods

		/// <summary>
		/// Initializes a new instance of the <see cref="FlockBuddy.Evade"/> class.
		/// </summary>
		public Arrive(Boid dude)
			: base(dude, EBehaviorType.arrive)
		{
		}

		/// <summary>
		/// Called every fram to get the steering direction from this behavior
		/// </summary>
		/// <param name="time"></param>
		/// <returns></returns>
		protected override Vector2 GetSteering()
		{
			return Vector2.Zero * Weight;
		}

		#endregion //Methods
	}
}
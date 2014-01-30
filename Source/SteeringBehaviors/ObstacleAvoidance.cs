using Microsoft.Xna.Framework;

namespace FlockBuddy
{
	/// <summary>
	/// this returns a steering force which will attempt to keep the agent away from any obstacles it may encounter
	/// </summary>
	public class ObstacleAvoidance : BaseBehavior
	{
		#region Members

		//length of the 'detection box' utilized in obstacle avoidance
		const float DBoxLength = 100.0f;

		#endregion //Members

		#region Methods

		/// <summary>
		/// Initializes a new instance of the <see cref="FlockBuddy.ObstacleAvoidance"/> class.
		/// </summary>
		public ObstacleAvoidance(Boid dude)
			: base(dude, EBehaviorType.obstacle_avoidance)
		{
		}

		/// <summary>
		/// Called every fram to get the steering direction from this behavior
		/// </summary>
		/// <param name="time"></param>
		/// <returns></returns>
		protected override Vector2 GetSteering()
		{
			  //the detection box length is proportional to the agent's velocity
			  float boxLength = DBoxLength + (Owner.Speed / Owner.MaxSpeed) * DBoxLength;

			  //tag all obstacles within range of the box for processing
			  Owner.MyFlock.ta->TagObstaclesWithinViewRange(Onwer, boxLength);

			  //this will keep track of the closest intersecting obstacle (CIB)
			  BaseGameEntity* ClosestIntersectingObstacle = NULL;
 
			  //this will be used to track the distance to the CIB
			  double DistToClosestIP = MaxDouble;

			  //this will record the transformed local coordinates of the CIB
			  Vector2D LocalPosOfClosestObstacle;

			  std::vector<BaseGameEntity*>::const_iterator curOb = obstacles.begin();

			  while(curOb != obstacles.end())
			  {
				//if the obstacle has been tagged within range proceed
				if ((*curOb)->IsTagged())
				{
				  //calculate this obstacle's position in local space
				  Vector2D LocalPos = PointToLocalSpace((*curOb)->Pos(),
														 Owner.Heading(),
														 Owner.Side(),
														 Owner.Pos());

				  //if the local position has a negative x value then it must lay
				  //behind the agent. (in which case it can be ignored)
				  if (LocalPos.x >= 0)
				  {
					//if the distance from the x axis to the object's position is less
					//than its radius + half the width of the detection box then there
					//is a potential intersection.
					double ExpandedRadius = (*curOb)->BRadius() + Owner.BRadius();

					if (fabs(LocalPos.y) < ExpandedRadius)
					{
					  //now to do a line/circle intersection test. The center of the 
					  //circle is represented by (cX, cY). The intersection points are 
					  //given by the formula x = cX +/-sqrt(r^2-cY^2) for y=0. 
					  //We only need to look at the smallest positive value of x because
					  //that will be the closest point of intersection.
					  double cX = LocalPos.x;
					  double cY = LocalPos.y;
          
					  //we only need to calculate the sqrt part of the above equation once
					  double SqrtPart = sqrt(ExpandedRadius*ExpandedRadius - cY*cY);

					  double ip = cX - SqrtPart;

					  if (ip <= 0.0)
					  {
						ip = cX + SqrtPart;
					  }

					  //test to see if this is the closest so far. If it is keep a
					  //record of the obstacle and its local coordinates
					  if (ip < DistToClosestIP)
					  {
						DistToClosestIP = ip;

						ClosestIntersectingObstacle = *curOb;

						LocalPosOfClosestObstacle = LocalPos;
					  }         
					}
				  }
				}

				++curOb;
			  }

			  //if we have found an intersecting obstacle, calculate a steering 
			  //force away from it
			  Vector2D SteeringForce;

			  if (ClosestIntersectingObstacle)
			  {
				//the closer the agent is to an object, the stronger the 
				//steering force should be
				double multiplier = 1.0 + (m_dDBoxLength - LocalPosOfClosestObstacle.x) /
									m_dDBoxLength;

				//calculate the lateral force
				SteeringForce.y = (ClosestIntersectingObstacle->BRadius()-
								   LocalPosOfClosestObstacle.y)  * multiplier;   

				//apply a braking force proportional to the obstacles distance from
				//the vehicle. 
				const double BrakingWeight = 0.2;

				SteeringForce.x = (ClosestIntersectingObstacle->BRadius() - 
								   LocalPosOfClosestObstacle.x) * 
								   BrakingWeight;
			  }

			  //finally, convert the steering vector from local to world space
			  return VectorToWorldSpace(SteeringForce,
										Owner.Heading(),
										Owner.Side());
		}

		#endregion //Methods
	}
}
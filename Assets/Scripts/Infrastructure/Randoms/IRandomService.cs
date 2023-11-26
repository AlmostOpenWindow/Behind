using System.Collections.Generic;
using Services;
using UnityEngine;

namespace Infrastructure.Randoms
{
	public interface IRandomService : IService
	{ 
		Vector2 InsideUnitCircle { get; }
		/// <summary>
		/// [0,Max)
		/// </summary>
		int NextInt(int max);
		/// <summary>
		/// [0,Max]
		/// </summary>
		float NextFloat(float max = 1);
		T RandomByHeight<T>(List<WeightObject<T>> weightObjects);
		List<T> RandomByHeight<T>(List<WeightObject<T>> weightObjects, int count);
	}

	public class WeightObject<T>
	{
		public T Container { get; }
		public int Weight { get; set; }
		
		public WeightObject(T container, int weight)
		{
			Weight = weight;
			Container = container;
		}
	}
}
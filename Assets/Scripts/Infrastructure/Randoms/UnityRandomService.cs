using System;
using System.Collections.Generic;
using System.Linq;
using Services.Debug;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Infrastructure.Randoms
{
	public class UnityRandomService : IRandomService
	{
		private readonly ILogService _logService;
		
		public UnityRandomService(ILogService logService)
		{
			_logService = logService;
		}
		
		public int NextInt(int max)
			=> Random.Range(0, max);
		
		public float NextFloat(float max = 1)
			=> Random.Range(0, max);

		public T RandomByHeight<T>(List<WeightObject<T>> weightObjects)
		{
			if (weightObjects == null || weightObjects.Count == 0)
			{
				_logService.Error("weightObjects == null || weightObjects.Count == 0");
				throw new Exception("weightObjects == null || weightObjects.Count == 0");
			}
			
			var totalWeight = weightObjects.Sum(weightObject => weightObject.Weight);
			var randomPoint = NextFloat() * totalWeight;
			
			foreach (var currentWeightObject in weightObjects)
			{
				randomPoint -= currentWeightObject.Weight;
				if (randomPoint < 0)
				{
					return currentWeightObject.Container;
				}
			}
			
			return weightObjects[weightObjects.Count-1].Container;
		}
		
		public List<T> RandomByHeight<T>(List<WeightObject<T>> weightObjects, int count)
		{
			var results = new List<T>();
			while (count > 0 && weightObjects.Count > 0)
			{
				var stepResult = RandomByHeight(weightObjects);
				var weightStepResult = weightObjects.FirstOrDefault(weight => weight.Container.Equals(stepResult));
				if (weightStepResult != null)
				{
					if (weightStepResult.Weight > 0)
						weightStepResult.Weight--;
					else
						weightObjects.Remove(weightStepResult);
				}
				results.Add(stepResult);
				count--;
			}
			return results;
		}

		public Vector2 InsideUnitCircle
			=> Random.insideUnitCircle;
	}
}
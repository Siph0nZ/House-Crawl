using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="RandomWalkParameters_",menuName = "PCG/RandomWalkData")] // PCG - procedural content generation
public class RandomWalkSO : ScriptableObject
{
    public int iterations = 10, walkLength = 10;
    public bool startRandomPerIteration = true;

}

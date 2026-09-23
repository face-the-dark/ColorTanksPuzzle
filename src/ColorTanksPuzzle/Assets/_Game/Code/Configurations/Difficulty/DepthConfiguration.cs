using System;
using UnityEngine;

namespace _Game.Code.Configurations.Difficulty
{
    [Serializable]
    public class DepthConfiguration
    {
        [SerializeField] private int _depthValue;
        [SerializeField] private int _depthWeight;

        public int DepthValue => _depthValue;
        public int DepthWeight => _depthWeight;
    }
}
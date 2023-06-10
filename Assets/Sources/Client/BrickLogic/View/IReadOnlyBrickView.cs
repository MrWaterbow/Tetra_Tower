using UnityEngine;

namespace Client.BrickLogic
{
    internal interface IReadOnlyBrickView
    {
        MeshFilter MeshFilter { get; }
    }
}
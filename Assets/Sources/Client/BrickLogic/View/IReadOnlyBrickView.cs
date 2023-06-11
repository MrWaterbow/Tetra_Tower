using UnityEngine;

namespace Client.BrickLogic
{
    internal interface IReadOnlyBrickView
    {
        Mesh GetMesh();
        Color GetColor();
    }
}
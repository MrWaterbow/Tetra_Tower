using Sources.Factories;
using UnityEngine;

namespace Sources.BlockLogic
{
    public interface IBlockVisualization
    {
        void SetPosition(Vector3 position);
        void SetVisualization(VisualizationType type);
        void SetRenderQueue(int value);

        void Show(Mesh mesh, Color color);
        void Hide();
    }
}
using Server.GhostLogic;
using UnityEngine;

namespace Client.GhostLogic
{
    internal sealed class GhostView : MonoBehaviour
    {
        [SerializeField] private Transform _transform;
        [SerializeField] private MeshFilter _meshFilter;

        private IGhostViewPresenter _presenter;

        public void SetCallbacks(IGhostViewPresenter presenter)
        {
            presenter.OnPositionChanged += ChangePosition;

            _presenter = presenter;
        }

        public void SetCallbacks()
        {
            _presenter.OnPositionChanged += ChangePosition;
        }

        public void DisposeCallbacks()
        {
            _presenter.OnPositionChanged -= ChangePosition;
        }

        /// <summary>
        /// Изменяет позицию блока ( использует DOTween )
        /// </summary>
        /// <param name="newPosition"></param>
        public void ChangePosition(Vector3 newPosition)
        {
            _transform.position = newPosition;
        }

        public void SetMesh(Mesh mesh)
        {
            _meshFilter.mesh = mesh;
        }
    }
}
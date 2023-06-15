using Server.GhostLogic;
using UnityEngine;

namespace Client.GhostLogic
{
    internal sealed class GhostView : MonoBehaviour
    {
        [SerializeField] private Transform _transform;
        [SerializeField] private MeshFilter _meshFilter;
        [SerializeField] private MeshRenderer _meshRenderer;

        private IGhostViewPresenter _presenter;

        /// <summary>
        /// Подписывается на ивенты от презентера при этом назначая его
        /// </summary>
        /// <param name="presenter"></param>
        public void SetCallbacks(IGhostViewPresenter presenter)
        {
            presenter.OnPositionChanged += ChangePosition;

            _presenter = presenter;
        }

        /// <summary>
        /// Подписывается на ивенты от презентера.
        /// </summary>
        public void SetCallbacks()
        {
            _presenter.OnPositionChanged += ChangePosition;
        }

        /// <summary>
        /// Отписывается от ивентов от презентера.
        /// </summary>
        public void DisposeCallbacks()
        {
            _presenter.OnPositionChanged -= ChangePosition;
        }

        /// <summary>
        /// Изменяет позицию блока (использует DOTween).
        /// </summary>
        /// <param name="newPosition"></param>
        public void ChangePosition(Vector3 newPosition)
        {
            _transform.position = newPosition;
        }

        /// <summary>
        /// Меняет меш призрака.
        /// </summary>
        /// <param name="mesh"></param>
        public void SetMesh(Mesh mesh)
        {
            _meshFilter.mesh = mesh;
        }

        /// <summary>
        /// Меняет цвет призрака.
        /// </summary>
        /// <param name="color"></param>
        public void SetColor(Color color)
        {
            _meshRenderer.material.color = color;
        }
    }
}
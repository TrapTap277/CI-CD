using UnityEngine;

namespace FPS.Scripts.Math._1_Lesson.BitTogglingAndUnitySpecifics
{
    public class PlayerRaycaster : MonoBehaviour
    {
        // ReSharper disable once InconsistentNaming
        private static int CubeLayerMaskId = 1 << 12;
        private static int SphereLayerMaskId = CubeLayerMaskId << 2;

        private long _board;
        
        private readonly RaycastHit[] _results = new RaycastHit[128];

        private void FixedUpdate()
        {
            var layerMask = CubeLayerMaskId | SphereLayerMaskId;

            if(Physics.Raycast(GetRay(), out RaycastHit raycast, Mathf.Infinity, layerMask))
            {
                if(raycast.collider)
                    Debug.Log($"Raycast hit: {raycast.collider.name}");
                
                DrawRay(Color.yellow);
            }
            else
            {
                DrawRay(Color.red);
            }
        }

        private bool GetCellState(int row, int column)
        {
            var currentCell = 1L << row * 8 + column;

            return (_board & currentCell) != 0;
        }

        private int GetCellCount()
        {
            int cellCount = 0;
            var board = _board;
            
            while (board != 0)
            {
                board &= board - 1;
                cellCount++;
            }

            return cellCount;
        }

        private void TrySetCellState(int row, int column)
        {
            if(GetCellState(row, column))
                return;
            
            var currentCell = row | column;
            _board |= (uint)currentCell;
        }
        
        private void DrawRay(Color color) =>
            Debug.DrawRay(GetRay().origin, GetRay().direction * 100, color, 0);

        private Ray GetRay() =>
            Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
    }
}
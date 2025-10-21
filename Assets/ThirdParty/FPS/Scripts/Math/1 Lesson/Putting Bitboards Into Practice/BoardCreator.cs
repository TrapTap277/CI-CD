using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace FPS.Scripts.Math._1_Lesson.Putting_Bitboards_Into_Practice
{
    public class BoardCreator : MonoBehaviour
    {
        private const string GrainConstant = "Grain";
        private const string TreeKey = "Tree";
        private const string HouseKey = nameof(HouseKey);

        private readonly Dictionary<string, long> _board = new();

        [SerializeField] private GameObject[] _titlePrefabs;
        [SerializeField] private GameObject _house;
        [SerializeField] private GameObject _tree;
        [SerializeField] private Text _score;
        [SerializeField] private int _row;
        [SerializeField] private int _column;

        private GameObject[] _tiles;
        
        private void Start()
        {
            _tiles = new GameObject[_row * _column];
            
            for (int row = 0; row < _row; row++)
            {
                for (int column = 0; column < _column; column++)
                {
                    var newTile = Instantiate(GetRandomTitle(), new Vector3(column, 0, row), Quaternion.identity);
                    newTile.name = $"{newTile.tag}_{row}_{column}";

                    AddCellToBoard(newTile.tag, row, column);
                    _tiles[row * _row + column] = newTile;
                }
            }

            InvokeRepeating(nameof(PlantTrees), 0, 0.25f);
        }

        private void Update()
        {
            if(Input.GetMouseButtonDown(0))
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if(Physics.Raycast(ray, out var hitInfo, Mathf.Infinity))
                {
                    var hitTransform = hitInfo.transform;

                    var alreadyHasHouse = HasHouse((int)hitTransform.position.z, (int)hitTransform.position.x);
                    var hasTree = HasTree((int)hitTransform.position.z, (int)hitTransform.position.x);

                    if(hitTransform.tag == GrainConstant && alreadyHasHouse == false && hasTree == false)
                    {
                        var newHouse = Instantiate(_house, Vector3.zero, Quaternion.identity);
                        newHouse.transform.parent = hitTransform;
                        newHouse.transform.localPosition = Vector3.zero;
                        AddCellToBoard(HouseKey, (int)hitTransform.position.z, (int)hitTransform.position.x);
                    }
                }
            }
        }

        private bool HasTree(int row, int column) =>
            IsCellActive(TreeKey, row, column);

        private bool HasHouse(int row, int column) =>
            IsCellActive(HouseKey, row, column);

        private void PlantTrees()
        {
            var randomRow = Random.Range(0, _row);
            var randomColumn = Random.Range(0, _column);

            if(IsCellActive(GrainConstant, randomRow, randomColumn) && HasTree(randomRow, randomColumn) == false && HasHouse(randomRow, randomColumn) == false)
            {
                var tileTransform = _tiles[randomRow * _row + randomColumn].transform;

                var newTree = Instantiate(_tree, Vector3.zero, Quaternion.identity);
                newTree.transform.parent = tileTransform;
                newTree.transform.localPosition = Vector3.zero;

                AddCellToBoard(TreeKey, randomRow, randomColumn);
            }
        }

        private bool IsCellActive(string tag, int row, int column) =>
            _board.TryGetValue(tag, out var value) && (value & 1L << row * _row + column) != 0;

        private int GetCellCountByTag(string tag)
        {
            var board = _board[tag];
            var cellCount = 0;
            
            while (board != 0)
            {
                board &= board - 1;
                cellCount++;
            }

            return cellCount;
        }

        private void AddCellToBoard(string titleTag, int row, int column)
        {
            var newBit = 1L << row * _row + column;

            if(_board.TryAdd(titleTag, newBit) == false)
                _board[titleTag] = _board[titleTag] |= newBit;

            UpdateScore();
        }

        private void UpdateScore() =>
            _score.text = string.Join('\n', _board.Select(x => $"{x.Key}, Value: {GetCellCountByTag(x.Key)}"));

        private GameObject GetRandomTitle() =>
            _titlePrefabs[Random.Range(0, _titlePrefabs.Length)];

        private void LogCellsCount()
        {
            foreach (var boardValuePair in _board)
            {
                var printedValue = Convert.ToString(boardValuePair.Value, 2).PadLeft(64, '0');
                Debug.Log($"Key: {boardValuePair.Key}, Value { GetCellCountByTag(boardValuePair.Key)}");
            }
        }
    }
}
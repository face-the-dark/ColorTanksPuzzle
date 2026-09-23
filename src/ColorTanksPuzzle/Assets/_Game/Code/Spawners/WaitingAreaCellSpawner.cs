using System;
using System.Collections.Generic;
using _Game.Code.Configurations;
using _Game.Code.Configurations.Difficulty;
using _Game.Code.Infrastructure.Assets;
using _Game.Code.Providers;
using _Game.Code.WaitingAreaComponents;
using UnityEngine;
using Object = UnityEngine.Object;

namespace _Game.Code.Spawners
{
    public class WaitingAreaCellSpawner
    {
        private const float DistanceBetweenCells = 1f;
        private const float Divisor = 2f;
        private const int MaxAreaCellsInRow = 5;

        private readonly DifficultyConfigurationProvider _difficultyConfigurationProvider;
        private readonly LevelConfigurationProvider _levelConfigurationProvider;

        private WaitingAreaCell _waitingAreaCellPrefab;
        private WaitingAreaSpawnBoundaries _waitingAreaSpawnBoundaries;

        private float _currentPositionByX;
        private float _currentPositionByZ;

        private List<WaitingAreaCell> _currentWaitingAreaCells = new();

        public WaitingAreaCellSpawner
        (
            WaitingAreaSpawnBoundaries waitingAreaSpawnBoundaries,
            LoadService loadService,
            DifficultyConfigurationProvider difficultyConfigurationProvider,
            LevelConfigurationProvider levelConfigurationProvider
        )
        {
            _waitingAreaSpawnBoundaries = waitingAreaSpawnBoundaries;
            _difficultyConfigurationProvider = difficultyConfigurationProvider;
            _levelConfigurationProvider = levelConfigurationProvider;

            _waitingAreaCellPrefab = loadService.LoadWaitingAreaCell();
        }

        public event Action<List<WaitingAreaCell>> CellsSpawned;

        public void SpawnStartCells()
        {
            DifficultyMode difficultyMode = _levelConfigurationProvider.LevelConfiguration.DifficultyMode;
            DifficultyConfiguration difficultyConfiguration = _difficultyConfigurationProvider.Get(difficultyMode);

            if (difficultyConfiguration == null)
                throw new ArgumentNullException();

            int startMaxTanksCount = difficultyConfiguration.MaxTanksCount;

            _currentPositionByX = _waitingAreaSpawnBoundaries.LeftUpBoundary.position.x;
            _currentPositionByX += _waitingAreaCellPrefab.transform.localScale.x / Divisor;

            _currentPositionByZ = _waitingAreaSpawnBoundaries.LeftUpBoundary.position.z;
            _currentPositionByZ -= _waitingAreaCellPrefab.transform.localScale.z / Divisor;
            
            for (int i = 0; i < startMaxTanksCount; i++)
            {
                SpawnCell();

                UpdateNextPosition();
            }

            AlignCellsInCenterByRows();

            CellsSpawned?.Invoke(_currentWaitingAreaCells);
        }

        public void AddCell()
        {
            SpawnCell();
            UpdateNextPosition();
            AlignCellsInCenterByRows();
        }

        private void SpawnCell()
        {
            Vector3 position = new Vector3(_currentPositionByX, 0.5f, _currentPositionByZ);

            WaitingAreaCell waitingAreaCell = Object.Instantiate(_waitingAreaCellPrefab, position, Quaternion.identity);

            _currentWaitingAreaCells.Add(waitingAreaCell);
        }

        private void UpdateNextPosition()
        {
            _currentPositionByX += _waitingAreaCellPrefab.transform.localScale.x;
            _currentPositionByX += DistanceBetweenCells;
                
            if (_currentPositionByX > _waitingAreaSpawnBoundaries.RightUpBoundary.position.x)
            {
                _currentPositionByX = _waitingAreaSpawnBoundaries.LeftUpBoundary.position.x;
                _currentPositionByX += _waitingAreaCellPrefab.transform.localScale.x / Divisor;

                _currentPositionByZ -= DistanceBetweenCells;
                _currentPositionByZ -= _waitingAreaCellPrefab.transform.localScale.z;
            }
        }

        private void AlignCellsInCenterByRows()
        {
            List<WaitingAreaCell> currentWaitingAreaCells = new List<WaitingAreaCell>(_currentWaitingAreaCells);

            while (currentWaitingAreaCells.Count % MaxAreaCellsInRow > 0)
            {
                List<WaitingAreaCell> partOfCurrentWaitingAreaCells = new List<WaitingAreaCell>();
                
                int cellsCount = MaxAreaCellsInRow;
                
                if (currentWaitingAreaCells.Count < MaxAreaCellsInRow)
                    cellsCount = currentWaitingAreaCells.Count;
                
                for (int i = 0; i < cellsCount; i++)
                {
                    partOfCurrentWaitingAreaCells.Add(currentWaitingAreaCells[i]);
                }
                
                currentWaitingAreaCells.RemoveRange(0, partOfCurrentWaitingAreaCells.Count);
                
                AlignCellsInCenter(partOfCurrentWaitingAreaCells);
            }
        }

        private void AlignCellsInCenter(List<WaitingAreaCell> waitingAreaCells)
        {
            if (waitingAreaCells.Count % 2 != 0)
            {
                int centerCellIndex = waitingAreaCells.Count / 2;
                WaitingAreaCell cell = waitingAreaCells[centerCellIndex];

                cell.transform.localPosition =
                    new Vector3(0f, cell.transform.localPosition.y, cell.transform.localPosition.z);

                float leftPositionByX = cell.transform.localPosition.x;

                for (int i = centerCellIndex - 1; i >= 0; i--)
                {
                    leftPositionByX -= DistanceBetweenCells;
                    leftPositionByX -= _waitingAreaCellPrefab.transform.localScale.x;

                    waitingAreaCells[i].transform.localPosition = new Vector3
                    (
                        leftPositionByX, cell.transform.localPosition.y, cell.transform.localPosition.z
                    );
                }

                float rightPositionByX = cell.transform.localPosition.x;

                for (int i = centerCellIndex + 1; i < waitingAreaCells.Count; i++)
                {
                    rightPositionByX += DistanceBetweenCells;
                    rightPositionByX += _waitingAreaCellPrefab.transform.localScale.x;

                    waitingAreaCells[i].transform.localPosition = new Vector3
                    (
                        rightPositionByX, cell.transform.localPosition.y, cell.transform.localPosition.z
                    );
                }
            }
            else
            {
                float halfDistanceBetweenCells = DistanceBetweenCells / 2f;
                int halfCellsCount = waitingAreaCells.Count / 2;

                float firstDistanceByX = _waitingAreaCellPrefab.transform.localScale.x / 2f;

                float leftPositionByX = -firstDistanceByX - halfDistanceBetweenCells;

                for (int i = halfCellsCount - 1; i >= 0; i--)
                {
                    Vector3 cellLocalPosition = waitingAreaCells[i].transform.localPosition;

                    waitingAreaCells[i].transform.localPosition =
                        new Vector3(leftPositionByX, cellLocalPosition.y, cellLocalPosition.z);

                    leftPositionByX -= DistanceBetweenCells;
                    leftPositionByX -= _waitingAreaCellPrefab.transform.localScale.x;
                }

                float rightPositionByX = firstDistanceByX + halfDistanceBetweenCells;

                for (int i = halfCellsCount; i < waitingAreaCells.Count; i++)
                {
                    Vector3 cellLocalPosition = waitingAreaCells[i].transform.localPosition;

                    waitingAreaCells[i].transform.localPosition =
                        new Vector3(rightPositionByX, cellLocalPosition.y, cellLocalPosition.z);

                    rightPositionByX += DistanceBetweenCells;
                    rightPositionByX += _waitingAreaCellPrefab.transform.localScale.x;
                }
            }
        }
    }
}
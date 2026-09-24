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
        private readonly WaitingAreaSpawnBoundaries _waitingAreaSpawnBoundaries;

        private readonly List<WaitingAreaCell> _currentWaitingAreaCells = new();

        private WaitingAreaCell _waitingAreaCellPrefab;

        private float _currentPositionByX;
        private float _currentPositionByZ;

        public WaitingAreaCellSpawner
        (
            WaitingAreaSpawnBoundaries waitingAreaSpawnBoundaries,
            LoadService loadService,
            DifficultyConfigurationProvider difficultyConfigurationProvider,
            LevelConfigurationProvider levelConfigurationProvider
        )
        {
            _waitingAreaSpawnBoundaries = waitingAreaSpawnBoundaries ??
                                          throw new ArgumentNullException(nameof(waitingAreaSpawnBoundaries));

            _difficultyConfigurationProvider = difficultyConfigurationProvider ??
                                               throw new ArgumentNullException(nameof(difficultyConfigurationProvider));

            _levelConfigurationProvider = levelConfigurationProvider ??
                                          throw new ArgumentNullException(nameof(levelConfigurationProvider));

            if (loadService == null)
                throw new ArgumentNullException(nameof(loadService));

            _waitingAreaCellPrefab = loadService.LoadWaitingAreaCell();
        }

        public event Action<List<WaitingAreaCell>> CellsSpawned;
        public event Action<WaitingAreaCell> CellAdded;

        public void SpawnStartCells()
        {
            DifficultyConfiguration difficultyConfiguration = _levelConfigurationProvider.GetDifficultyConfiguration();

            if (difficultyConfiguration == null)
                throw new InvalidOperationException(nameof(difficultyConfiguration));

            InitializeStartPosition();

            for (int i = 0; i < difficultyConfiguration.StartMaxTanksCount; i++)
            {
                SpawnCell();
                UpdateNextPosition();
            }

            AlignCellsInCenterByRows();

            CellsSpawned?.Invoke(_currentWaitingAreaCells);
        }

        public void AddCell()
        {
            WaitingAreaCell waitingAreaCell = SpawnCell();

            UpdateNextPosition();

            AlignCellsInCenterByRows();

            CellAdded?.Invoke(waitingAreaCell);
        }

        private void InitializeStartPosition()
        {
            _currentPositionByX = _waitingAreaSpawnBoundaries.LeftUpBoundary.position.x;
            _currentPositionByX += _waitingAreaCellPrefab.transform.localScale.x / Divisor;

            _currentPositionByZ = _waitingAreaSpawnBoundaries.LeftUpBoundary.position.z;
            _currentPositionByZ -= _waitingAreaCellPrefab.transform.localScale.z / Divisor;
        }

        private WaitingAreaCell SpawnCell()
        {
            Vector3 position = new(_currentPositionByX, 0.5f, _currentPositionByZ);

            WaitingAreaCell waitingAreaCell = Object.Instantiate(_waitingAreaCellPrefab, position, Quaternion.identity);

            _currentWaitingAreaCells.Add(waitingAreaCell);

            return waitingAreaCell;
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
            for (int i = 0; i < _currentWaitingAreaCells.Count; i += MaxAreaCellsInRow)
            {
                int cellsInRow = Mathf.Min(MaxAreaCellsInRow, _currentWaitingAreaCells.Count - i);

                List<WaitingAreaCell> row = _currentWaitingAreaCells.GetRange(i, cellsInRow);

                AlignCellsInCenter(row);
            }
        }

        private void AlignCellsInCenter(List<WaitingAreaCell> cells)
        {
            float step = _waitingAreaCellPrefab.transform.localScale.x + DistanceBetweenCells;

            float startX = -(cells.Count - 1) * step / Divisor;

            for (int i = 0; i < cells.Count; i++)
            {
                Transform cellTransform = cells[i].transform;

                float positionByX = startX + i * step;
                
                cellTransform.localPosition = 
                    new Vector3(positionByX, cellTransform.localPosition.y, cellTransform.localPosition.z);
            }
        }
    }
}
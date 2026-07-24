using System;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public enum LevelState
    {
        WaitingToStart,
        Preparation,
        Wave,
        Victory,
        GameOver
    }

    [SerializeField]
    private WaveManager waveManager;

    [Header("Preparation")]
    [SerializeField]
    private float preparationDuration = 30f;

    private float preparationTimeRemaining;

    public LevelState CurrentState { get; private set; }
    public float PreparationTimeRemaining => preparationTimeRemaining;

    private void Awake()
    {
        ChangeState(LevelState.WaitingToStart);
    }

    private void Update()
    {
        if (CurrentState == LevelState.Preparation)
        {
            UpdatePreparationTimer();
        }
        Debug.Log(CurrentState);
    }

    public void StartLevel()
    {
        if (CurrentState != LevelState.WaitingToStart)
        {
            return;
        }

        StartPreparation();
    }

    private void StartPreparation()
    {
        preparationTimeRemaining = preparationDuration;

        ChangeState(LevelState.Preparation);
    }

    private void UpdatePreparationTimer()
    {
        preparationTimeRemaining -= Time.deltaTime;

        if (preparationTimeRemaining < 0f)
        {
            preparationTimeRemaining = 0f;
        }

        if (preparationTimeRemaining <= 0f)
        {
            StartWave();
        }
    }

    public void SkipPreparation()
    {
        if (CurrentState != LevelState.Preparation)
        {
            return;
        }

        preparationTimeRemaining = 0f;
    }

    public void CompleteWave()
    {
        StartPreparation();
    }

    private void StartWave()
    {
        ChangeState(LevelState.Wave);
        waveManager.StartWave();
    }


    private void ChangeState(LevelState newState)
    {
        CurrentState = newState;

        Debug.Log($"Level state: {CurrentState}");
    }
}
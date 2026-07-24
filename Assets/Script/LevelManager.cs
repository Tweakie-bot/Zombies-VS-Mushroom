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

    [Header("Preparation")]
    [SerializeField]
    private float preparationDuration = 30f;

    private float preparationTimeRemaining;

    public LevelState CurrentState { get; private set; }
    public float PreparationTimeRemaining => preparationTimeRemaining;

    public event Action<LevelState> OnStateChanged;
    public event Action<float> OnPreparationTimeChanged;
    public event Action OnWaveRequested;

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
        OnPreparationTimeChanged?.Invoke(preparationTimeRemaining);
    }

    private void UpdatePreparationTimer()
    {
        preparationTimeRemaining -= Time.deltaTime;

        if (preparationTimeRemaining < 0f)
        {
            preparationTimeRemaining = 0f;
        }

        OnPreparationTimeChanged?.Invoke(preparationTimeRemaining);

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
        OnPreparationTimeChanged?.Invoke(preparationTimeRemaining);

        StartWave();
    }

    private void StartWave()
    {
        ChangeState(LevelState.Wave);
        OnWaveRequested?.Invoke();
    }

    public void NotifyWaveCompleted()
    {
        if (CurrentState != LevelState.Wave)
        {
            return;
        }

        StartPreparation();
    }

    public void NotifyVictory()
    {
        ChangeState(LevelState.Victory);
    }

    public void NotifyGameOver()
    {
        ChangeState(LevelState.GameOver);
    }

    private void ChangeState(LevelState newState)
    {
        CurrentState = newState;
        OnStateChanged?.Invoke(CurrentState);

        Debug.Log($"Level state: {CurrentState}");
    }
}
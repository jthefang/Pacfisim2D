using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour, ILoadableScript
{
    int _score;
    public int Score {
        get {
            return this._score;
        } 
        set {
            this._score = value;
            OnScoreChange?.Invoke(this);
        }
    }

    int _scoreMultiplier;
    public int ScoreMultiplier {
        get {
            return this._scoreMultiplier;
        }
        set {
            this._scoreMultiplier = value;
            OnScoreMultiplierChange?.Invoke(this);
        }
    }

    [SerializeField]
    GameManager gameManager;
    [SerializeField]
    List<String> events;
    [SerializeField]
    List<int> pointWorths;
    Dictionary<String, int> eventToPointWorth;

    #region Singleton
    public static ScoreManager Instance;
    //reference this only version as ScoreManager.Instance.Score = 100;

    private void Awake() {
        Instance = this;
    }
    #endregion

    bool _isInitialized = false;
    bool isInitialized {
        get {
            return this._isInitialized;
        }
        set {
            this._isInitialized = value;
            if (this._isInitialized) {
                OnScriptInitialized?.Invoke(this);
            }
        }   
    }
    public bool IsInitialized () {
        return isInitialized;
    }
    public event Action<ILoadableScript> OnScriptInitialized;
    public event Action<ScoreManager> OnScoreChange;
    public event Action<ScoreManager> OnScoreMultiplierChange;

    // Start is called before the first frame update
    void Start()
    {
        gameManager.OnGameStart += OnGameStart;
        GeneratePointDict();

        isInitialized = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnGameStart(GameManager gameManager) {
        Score = 0;
        ScoreMultiplier = 1;
    }

    void GeneratePointDict() {
        if (events == null || pointWorths == null) {
            Debug.Log("No current score point values for any events");
        }
        if (events.Count != pointWorths.Count) {
            Debug.LogError("There must be one event for every point worth and vice versa.");
        }

        eventToPointWorth = new Dictionary<string, int>();
        for (int i = 0; i < events.Count; i++) {
            eventToPointWorth[events[i]] = pointWorths[i];
        }
    }

    public void UpdateScoreWithEvent(String eventString) {
        if (!eventToPointWorth.ContainsKey(eventString)) {
            Debug.LogError("No point score for event: " + events);
        }

        Score += ScoreMultiplier * eventToPointWorth[eventString];
    }

}

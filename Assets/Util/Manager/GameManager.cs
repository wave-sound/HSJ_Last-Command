using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Util.Manager;

public class GameManager : MonoBehaviour
{
    [SerializeField] private ObjectManager _objectManagerPrefab;
    [SerializeField] private UIManager _uiManagerPrefab;
    [SerializeField] private PatternManager _patternManagerPrefab;
    [SerializeField] private StageManager _stageManagerPrefab;

    private ObjectManager _objectManager;
    private UIManager _uiManager;
    private PatternManager _patternManager;
    private StageManager _stageManager;

    private void Awake()
    {
        InitSettings();
        _uiManager.SetUI(_objectManager._player.HP(), _objectManager._boss);
        BindEvents();
    }
    private void OnDestroy()
    {
        UnbindEvents();
    }
    private void InitSettings()
    {
        _objectManager = Instantiate(_objectManagerPrefab);
        _objectManager.Init(this);
        _uiManager = Instantiate(_uiManagerPrefab);
        _uiManager.Init(this);
        _patternManager = Instantiate(_patternManagerPrefab);
        _patternManager.Init(this);
        _stageManager = Instantiate(_stageManagerPrefab);
        _stageManager.Init(this);
    }

    private void BindEvents()
    {
        _objectManager._boss.OnDie -= _stageManager.GameClear;
        _objectManager._boss.OnDie += _stageManager.GameClear;
        _objectManager._player.OnDie -= _stageManager.GameOver;
        _objectManager._player.OnDie += _stageManager.GameOver;
        _objectManager._boss.OnAttackSuccess -= _uiManager.BossHpDecrease;
        _objectManager._boss.OnAttackSuccess += _uiManager.BossHpDecrease;
        _objectManager._player.OnHpDecrease -= _uiManager.PlayerHpDecrease;
        _objectManager._player.OnHpDecrease += _uiManager.PlayerHpDecrease;
        _objectManager._bossGroggy.OnGroggyEnd -= _uiManager.ResetFill;
        _objectManager._bossGroggy.OnGroggyEnd += _uiManager.ResetFill;
        _objectManager._boss.OnTempChange -= _uiManager.ChangeTemp;
        _objectManager._boss.OnTempChange += _uiManager.ChangeTemp;
        _objectManager._boss.OnConfChange -= _uiManager.ChangeConf;
        _objectManager._boss.OnConfChange += _uiManager.ChangeConf;
        _objectManager._boss.OnGroggy -= _uiManager.EnterGroggy;
        _objectManager._boss.OnGroggy += _uiManager.EnterGroggy;
        _objectManager._boss.OnAttackSuccess -= _patternManager.ChangePattern;
        _objectManager._boss.OnAttackSuccess += _patternManager.ChangePattern;
    }

    private void UnbindEvents()
    {
        _objectManager._boss.OnDie -= _stageManager.GameClear;
        _objectManager._player.OnDie -= _stageManager.GameOver;
        _objectManager._boss.OnAttackSuccess -= _uiManager.BossHpDecrease;
        _objectManager._player.OnHpDecrease -= _uiManager.PlayerHpDecrease;
        _objectManager._bossGroggy.OnGroggyEnd -= _uiManager.ResetFill;
        _objectManager._boss.OnTempChange -= _uiManager.ChangeTemp;
        _objectManager._boss.OnConfChange -= _uiManager.ChangeConf;
        _objectManager._boss.OnGroggy -= _uiManager.EnterGroggy;
        _objectManager._boss.OnAttackSuccess -= _patternManager.ChangePattern;
    }

}

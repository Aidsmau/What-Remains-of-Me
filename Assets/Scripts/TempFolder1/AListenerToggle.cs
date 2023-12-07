using UnityEngine;

/// <summary>
/// An abstract class for scripts that toggle gameobjects under certain conditions when observing an AControllable.
/// Switches what callback to hook into depending on the ListenType.
/// Implemented by EnergyToggle and PacificationToggle, both of which are very similar to each other. Abstract.
/// </summary>
public abstract class AListenerToggle : AEventToggle
{
    private enum ListenType
    {
        Energy,
        Virus // add more if necessary
    }

    [Space(20)]
    [Header("Listener Fields")]
    [SerializeField] protected AControllable _observed;
    [SerializeField] private ListenType _controllableListenerType;

    private bool _isToggleActive;
    private bool _wasToggleActive;

    protected abstract bool IsToggleActive();

    private void Awake()
    {
        switch (_controllableListenerType)
        {
            case ListenType.Energy:
                _observed.OnEnergyChange.AddListener(ProcessChange);
                break;

            case ListenType.Virus:
                _observed.OnVirusChange.AddListener(ProcessChange);
                break;

            default:
                throw new System.NotImplementedException($"Trigger listener callback for type {_controllableListenerType} has not been implemented!");
        }
    }

    private void ProcessChange(float _)
    {
        _isToggleActive = IsToggleActive(); // should we be active?

        // if the state is different than the frame before it...
        if (_isToggleActive != _wasToggleActive)
        {
            // fire the event with the new state and record this new state
            FireEvent(_isToggleActive);
            _wasToggleActive = _isToggleActive;
        }
    }
}

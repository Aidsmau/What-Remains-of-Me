using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles virus and low health warnings for the player
/// </summary>
public class WarningController : MonoBehaviour
{
    private bool _runLightBlink;
    private bool _runLowHealthAnimation;
    [SerializeField] private GameObject lowHealthWarning;
    [SerializeField] private GameObject virusEyes;
    [SerializeField] private GameObject headlightA;
    [SerializeField] private GameObject headlightB;
    [SerializeField] private Color targetLightColorA;
    [SerializeField] private Color targetLightColorB;
    private float time;
    private Color initLightColorA;
    private Color initLightColorB;
    private Color eyeColor;

    /// <summary>
    /// Initializes animation to not run, initializes time and eye/light colors
    /// </summary>
    void Start()
    {
        _runLowHealthAnimation = false;
        time = -1.0f;
        initLightColorA = headlightA.GetComponent<UnityEngine.Rendering.Universal.Light2D>().color;
        initLightColorB = headlightB.GetComponent<UnityEngine.Rendering.Universal.Light2D>().color;
        eyeColor = virusEyes.GetComponent<SpriteRenderer>().color;
        virusEyes.GetComponent<SpriteRenderer>().color = new Color(eyeColor.r, eyeColor.g, eyeColor.b, 0.0f);

    }

    /// <summary>
    /// If the virus warning should be active, updates the headlight to blink
    /// </summary>
    void Update()
    {
        if (_runLightBlink)
        {
            
            if (time <= 0.1)   // Since time is set to a negative number in Start and StopAnimation, 
                                // this line is always true on first update when _runAnimation is true
            {
                headlightA.SetActive(!headlightA.activeSelf);
                headlightB.SetActive(!headlightB.activeSelf);
                time = 0.1f;
            }
            else if (time >= 0.3)
            {
                time = -1.0f;
            }
            time += Time.deltaTime;
        }
    }

    /// <summary>
    /// Starts the virus warning animation (ie, blinking headlight)
    /// </summary>
    public void StartLightBlinking()
    {
        if (!_runLightBlink)
        {
            headlightA.SetActive(false);
            headlightB.SetActive(false);
        }
        _runLightBlink = true;
    }

    /// <summary>
    /// Ends the virus animation. Resets time and sets headlights to active
    /// </summary>
    public void StopLightBlinking()
    {
        time = -1.0f;
        if (_runLightBlink)
        {
            headlightA.SetActive(true);
            headlightB.SetActive(true);
        }
        _runLightBlink = false;
    }

    /// <summary>
    /// Starts the low health warning animation, turning the warning image an animation on
    /// </summary>
    public void StartLowHealthWarning()
    {
        if (lowHealthWarning != null)
        {
            lowHealthWarning.GetComponent<UnityEngine.UI.Image>().enabled = true;
            lowHealthWarning.GetComponent<Animator>().enabled = true;
        }
    }

    /// <summary>
    /// Stops the low health warning animation, turning the warning image an animation off
    /// </summary>
    public void StopLowHealthWarning()
    {
        if (lowHealthWarning != null)
        {
            lowHealthWarning.GetComponent<UnityEngine.UI.Image>().enabled = false;
            lowHealthWarning.GetComponent<Animator>().enabled = false;
        }
    }

    /// <summary>
    /// Changes Atlas' eye and headlamp color based on virus percentage.
    /// Headlight becomes closer to the target light colors (purple) as virus percentage increases
    /// Virus eyes become less transparent as virus percentage increases (in practice, this makes Atlas' eyes look more purple)
    /// </summary>
    public void VirusControl(float percentVirus)
    {
       // Debug.Log(percentVirus);
        headlightA.GetComponent<UnityEngine.Rendering.Universal.Light2D>().color =
            new Color(initLightColorA.r + (percentVirus * (targetLightColorA.r - initLightColorA.r)),
            initLightColorA.g + (percentVirus * (targetLightColorA.g - initLightColorA.g)),
            initLightColorA.b + (percentVirus * (targetLightColorA.b - initLightColorA.b)),
            initLightColorA.a);
        headlightB.GetComponent<UnityEngine.Rendering.Universal.Light2D>().color = new Color(initLightColorB.r + (percentVirus * (targetLightColorB.r - initLightColorB.r)),
            initLightColorB.g + (percentVirus * (targetLightColorB.g - initLightColorB.g)),
            initLightColorB.b + (percentVirus * (targetLightColorA.b - initLightColorB.b)),
            initLightColorB.a);
        virusEyes.GetComponent<SpriteRenderer>().color = new Color(eyeColor.r, eyeColor.g, eyeColor.b, percentVirus);

    }

    /// <summary>
    /// Resets Atlas' eye and headlamp color when there is no virus
    /// Headlight becomes original yellow color and stays on
    /// Virus eyes become fully transparent (in practice, this makes Atlas' eyes black
    /// </summary>
    public void ResetVirus()
    {
        headlightA.GetComponent<UnityEngine.Rendering.Universal.Light2D>().color = initLightColorA;
        headlightB.GetComponent<UnityEngine.Rendering.Universal.Light2D>().color = initLightColorB;
        headlightA.SetActive(true);
        headlightB.SetActive(true);
        virusEyes.GetComponent<SpriteRenderer>().color = new Color(eyeColor.r, eyeColor.g, eyeColor.b, 0.0f);
    }


}

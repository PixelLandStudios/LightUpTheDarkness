using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class FlashlightController : MonoBehaviour
{
    public Light flashlight;
    public float activeDuration = 3f; // Duration for which the flashlight remains active
    public float cooldownDuration = 3f; // Cooldown duration between flashlight uses
    public Slider batterySlider; // Reference to the battery slider UI component
    public Image batterySliderImage; // Reference to the battery slider UI component

    private bool isFlashlightOn = false;
    private float lastActivationTime = 0f;
    bool isInCooldown = false;

    // Input action for toggling the flashlight
    private InputAction triggerAction;

    private float timeElapsedWhileOn = 0f;
    private float timeElapsedWhileOff = 0f;

    bool isRecharging;

    void OnEnable()
    {
        // Enable the input action
        triggerAction = new InputAction("Trigger", InputActionType.Button, "<XRController>{RightHand}/{TriggerButton}");
        triggerAction.Enable();

        // Register callbacks for trigger press and release
        triggerAction.started += OnTriggerStarted;
        triggerAction.canceled += OnTriggerCanceled;
    }

    void OnDisable()
    {
        // Disable the input action
        triggerAction.Disable();

        // Unregister callbacks
        triggerAction.started -= OnTriggerStarted;
        triggerAction.canceled -= OnTriggerCanceled;
    }

    void Update()
    {
        // If flashlight is on and duration has exceeded active duration
        if (isFlashlightOn && timeElapsedWhileOn >= activeDuration)
        {
            isInCooldown = true;
            ToggleFlashlight(false); // Turn off flashlight

            // Reset timer when turning off flashlight
            timeElapsedWhileOff = 0f;
            batterySliderImage.color = Color.black;
        }
        // If flashlight is off and duration has exceeded cooldown duration
        else if (!isFlashlightOn && timeElapsedWhileOff >= cooldownDuration)
        {
            batterySliderImage.color = Color.white;
            isInCooldown = false;
        }

        // Update timers
        if (isFlashlightOn)
        {
            timeElapsedWhileOn += Time.deltaTime;
            timeElapsedWhileOff = 0f;

            // Update battery slider based on the remaining active duration
            float remainingBattery = Mathf.Clamp01(1f - (timeElapsedWhileOn / activeDuration));
            batterySlider.value = remainingBattery;
        }
        else if (isInCooldown)
        {
            timeElapsedWhileOff += Time.deltaTime;
            timeElapsedWhileOn = 0f;

            // Update battery slider during cooldown
            float remainingCooldown = Mathf.Clamp01(timeElapsedWhileOff / cooldownDuration);
            batterySlider.value = remainingCooldown;
        }
        else if (isRecharging)
        {
            timeElapsedWhileOn -= Time.deltaTime;

            //Debug.Log(timeElapsedWhileOn);

            float remainingBattery = Mathf.Clamp01(1f - (timeElapsedWhileOn / activeDuration));
            batterySlider.value = remainingBattery;

            if (timeElapsedWhileOn <= 0)
                isRecharging = false;
        }
    }

    void OnTriggerStarted(InputAction.CallbackContext context)
    {
        if (!isInCooldown)
        {
            ToggleFlashlight(true);
        }
    }

    void OnTriggerCanceled(InputAction.CallbackContext context)
    {
        // If flashlight is turned off prematurely, store the premature off time
        if (isFlashlightOn)
        {
            isRecharging = true;
        }

        // Turn off flashlight when trigger is released
        ToggleFlashlight(false);
    }

    void ToggleFlashlight(bool toggle)
    {
        // Toggle the flashlight state
        isFlashlightOn = toggle;

        // Turn on/off the flashlight
        flashlight.enabled = isFlashlightOn;
    }
}
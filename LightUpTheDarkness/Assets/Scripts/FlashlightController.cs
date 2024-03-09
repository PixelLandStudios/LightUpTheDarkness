using UnityEngine;
using UnityEngine.InputSystem;

public class FlashlightController : MonoBehaviour
{
    public Light flashlight;
    public float activeDuration = 3f; // Duration for which the flashlight remains active
    public float cooldownDuration = 2f; // Cooldown duration between flashlight uses

    private bool isFlashlightOn = false;
    private float lastActivationTime = 0f;
    bool isInCooldown = false;

    // Input action for toggling the flashlight
    private InputAction triggerAction;

    private float timeElapsedWhileOn = 0f;
    private float timeElapsedWhileOff = 0f;

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
        }
        // If flashlight is off and duration has exceeded cooldown duration
        else if (!isFlashlightOn && timeElapsedWhileOff >= cooldownDuration)
        {
            isInCooldown = false;
        }

        // Update timers
        if (isFlashlightOn)
        {
            timeElapsedWhileOn += Time.deltaTime;
            timeElapsedWhileOff = 0f;
        }
        else
        {
            timeElapsedWhileOff += Time.deltaTime;
            timeElapsedWhileOn = 0f;
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

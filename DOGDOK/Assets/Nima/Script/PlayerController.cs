using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    public GameObject mainCamera;
    public AimPlayer aimPlayer;
    public CharacterMovement characterMovement;
    //public PlayerMouseLook playerMouseLook;
    public Animator animator;
    public PlayerEnergyController playerEnergyController;

    [Header("States")]
    public bool isAiming;
    public bool isShooting;
    public bool isMoving;
    public bool isRunning;
    public bool isWalking = true;
    public bool isCrouching;
    public bool canRun= true;

    [Header("CameraObjects")]
    [SerializeField] GameObject combatCamera;
    [SerializeField] GameObject basicCamera;
    [SerializeField] Cinemachine3rdPersonFollow activeCinemachine;
    public PlayerStates currentState = PlayerStates.Basic;
    [SerializeField] Vector2 lastMouseValue;

    [Header("Animation Weights")]
    Rig[] rigLayers;
    public float aimLayerWeightSpeed = 3f;
    [SerializeField] int aimWeightLayerIndex = 1;
    private float currentWeight;
    private float newWeight;

    [Header("Other")]
    [SerializeField] Material weaponMat;
    [SerializeField] float aimDistance;
    [SerializeField] TextMeshProUGUI interactionText;

    [Header("dof Effect")]
    [SerializeField] Volume volume;
    [SerializeField] VolumeProfile volProf;
    public DepthOfField dofEffect;
    [SerializeField] bool isDepthing;
    [SerializeField] LayerMask focusLayers;
    [SerializeField] float dofEffectSpeed;
    [SerializeField] float currentFocusValue;
    [SerializeField] float focusDistanceToleranse;
    [SerializeField] float focuseDistanceRange = 3f;
    [SerializeField] float focusCastSphereRadius;
    [SerializeField] float maxFocusRange;
    [SerializeField] float farDof = 2f;
    [SerializeField] float nearDef = 2f;

    [Header("Switch Camera Side")]
    [SerializeField] float currentShoulderSideValue = 1;
    [SerializeField] float targetShoulderSideValue = 1;
    [SerializeField] float shoulderSwitchDuration = 0.3f;
    [SerializeField] bool isSwitchingShoulder = false;

    public enum PlayerStates
    {
        Basic,
        Combat,
        Building
    }

    private void Awake()
    {
        characterMovement = GetComponent<CharacterMovement>();
        aimPlayer = GetComponent<AimPlayer>();
        playerEnergyController = GetComponent<PlayerEnergyController>();
    }

    void Start()
    {
        volume.profile = volProf;
        volProf.TryGet<DepthOfField>(out dofEffect);

        Cursor.lockState = CursorLockMode.Locked;
        ChangePlayerState(PlayerStates.Basic);
        //playerMouseLook = GetComponent<PlayerMouseLook>();
    }

    public void SetInteractionText(bool isActivate, string _text = "")
    {
        interactionText.gameObject.SetActive(isActivate);
        interactionText.text = _text;
    }

    float GetFocusDistance()
    {
        if (Physics.SphereCast(mainCamera.transform.position, focusCastSphereRadius, mainCamera.transform.forward, out RaycastHit hitInfo, maxFocusRange, focusLayers))
        {
            return hitInfo.distance;
        }

        return maxFocusRange;
    }



    public void ChangePlayerState(PlayerStates _state)
    {
        if (_state == PlayerStates.Basic)
        {
            dofEffect.active = false;
            combatCamera.SetActive(false);
            basicCamera.SetActive(true);
            activeCinemachine = basicCamera.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<Cinemachine3rdPersonFollow>();
            activeCinemachine.CameraSide = currentShoulderSideValue;

            canRun = true;
            aimPlayer.isAiming = false;
            currentWeight = animator.GetLayerWeight(aimWeightLayerIndex);
            newWeight = 0f;//end Aim Animation
            if (characterMovement.isRunning)
            {
                characterMovement.ToggleRunState(CharacterMovement.MoveStates.Run);
            }
            else
            {
                characterMovement.ToggleRunState(CharacterMovement.MoveStates.Walk);
            }
        }
        if (_state == PlayerStates.Combat)
        {
            dofEffect.active = true;
            basicCamera.SetActive(false);
            combatCamera.SetActive(true);
            activeCinemachine = combatCamera.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<Cinemachine3rdPersonFollow>();//Optimize Edilebilir
            activeCinemachine.CameraSide = currentShoulderSideValue;
            characterMovement.ToggleRunState(CharacterMovement.MoveStates.Crouched);
            //chande animaton weight
            currentWeight = animator.GetLayerWeight(aimWeightLayerIndex);
            newWeight = 1f;//Start Aim Animation
            aimPlayer.isAiming = true;
            canRun = false;
        }
        if (_state == PlayerStates.Building)
        {
            dofEffect.active = false;
            basicCamera.SetActive(false);
            combatCamera.SetActive(true);
            activeCinemachine = combatCamera.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<Cinemachine3rdPersonFollow>();//Optimize Edilebilir
            activeCinemachine.CameraSide = currentShoulderSideValue;
            characterMovement.ToggleRunState(CharacterMovement.MoveStates.Walk);

            aimPlayer.isAiming = false;
            canRun = false;

            currentWeight = animator.GetLayerWeight(aimWeightLayerIndex);
            //newWeight = 1f;//Start Aim Animation

        }
        currentState = _state;
        
    }

    void SwitchAimShoulder()
    {
        if (!isSwitchingShoulder)
        {
            currentShoulderSideValue = activeCinemachine.CameraSide;
            if (targetShoulderSideValue == 1)
            {
                targetShoulderSideValue = 0;
            }
            else
            {
                targetShoulderSideValue = 1;
            }
            isSwitchingShoulder = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SwitchAimShoulder();
        }
        if (isSwitchingShoulder)
        {
            currentShoulderSideValue = Mathf.MoveTowards(currentShoulderSideValue, targetShoulderSideValue,(1/shoulderSwitchDuration) * Time.deltaTime);
            activeCinemachine.CameraSide = currentShoulderSideValue;
            if (currentShoulderSideValue == targetShoulderSideValue)
            {
                isSwitchingShoulder = false;
            }
        }

        if (currentState == PlayerStates.Combat)
        {
            isDepthing = Mathf.Abs(currentFocusValue - GetFocusDistance()) >= focusDistanceToleranse;
            if (isDepthing)
            {

                currentFocusValue = Mathf.Lerp(currentFocusValue, GetFocusDistance(), dofEffectSpeed * Time.deltaTime);
                dofEffect.farFocusStart.value = currentFocusValue + focuseDistanceRange;
                dofEffect.nearFocusEnd.value = currentFocusValue - focuseDistanceRange;
                //dofEffect.nearFocusStart.value = currentFocusValue / nearDef;
                dofEffect.farFocusEnd.value = currentFocusValue * farDof;
            }
            
        }

        if (characterMovement.currentVelocity != characterMovement.speedToMove)//when player's changing speed
        {
            animator.SetFloat("MovementSpeed", characterMovement.currentVelocity / characterMovement.runningSpeed);
        }
        //animator.SetFloat("VelocityDir", characterMovement.speedToMove - characterMovement.currentVelocity);

        if (newWeight != currentWeight)
        {
            //currentWeight = Mathf.MoveTowards(currentWeight,newWeight,aimLayerWeightSpeed * Time.deltaTime);
            //rigLayers[0].weight = currentWeight;
            //animator.SetLayerWeight(aimWeightLayerIndex,currentWeight);
            //weaponMat.SetFloat("_Dissolve",1 - currentWeight);
        }
    }
}

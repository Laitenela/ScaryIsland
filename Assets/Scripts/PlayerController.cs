using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 10f;
    [SerializeField] private GameObject torch;
    [SerializeField] private GameObject lightOfTorch;
    [SerializeField] private GameObject stick;
    [SerializeField] private GameObject stickInHead;
    [SerializeField] private GameObject chest;
    [SerializeField] private AudioClip[] clips;
    private float getVertical;
    private float getHorizontal;
    private Animator animator;
    private AudioSource audioSource;
    private bool isMovementFree = true;
    private bool withoutStick = true;
    private bool hasTorch = false;
    private bool hasChest = false;
    public float timeWithoutLight = 0f;


    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        audioSource = gameObject.GetComponent<AudioSource>();
    }
    // Update is called once per frame
    void Update()
    {
        getVertical = Input.GetAxis("Vertical");
        getHorizontal = Input.GetAxis("Horizontal");
        float currentSpeed = Mathf.Abs(getVertical) + Mathf.Abs(getHorizontal) > 1 ? 1 : Mathf.Abs(getVertical) + Mathf.Abs(getHorizontal);
        animator.SetFloat("Speed", currentSpeed);
        audioSource.pitch = currentSpeed;

        if (!hasTorch)
        {
            timeWithoutLight += Time.deltaTime;
        }
        else
        {
            timeWithoutLight = 0f;
        }

        if (isMovementFree)
        {
            transform.LookAt(new Vector3(transform.position.x + getVertical, transform.position.y, transform.position.z + -getHorizontal));
            transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime * movementSpeed);


            if ((Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Joystick1Button0)) && !withoutStick)
            {
                animator.SetTrigger("IsPickup");
                isMovementFree = false;
                withoutStick = true;
                Invoke(nameof(PutStick), 0.5f);
            }
        }
    }

    void PutStick()
    {
        Instantiate(stick, transform.position, stick.transform.rotation);
        isMovementFree = true;
        stickInHead.SetActive(false);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Light"))
        {
            timeWithoutLight = 0f;
        }

        if (isMovementFree)
        {
            if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Joystick1Button0))
            {
                if (other.name == "CampFire" && !hasTorch)
                {
                    animator.SetTrigger("IsPickup");
                    isMovementFree = false;
                    Invoke(nameof(TorchPickup), 0.5f);
                    audioSource.PlayOneShot(clips[0], 0.1f);
                }
                else if ((other.CompareTag("TorchStick") || other.CompareTag("StickOnGround")) && withoutStick)
                {
                    animator.SetTrigger("IsPickup");
                    withoutStick = false;
                    isMovementFree = false;
                    Invoke(nameof(StickPickup), 0.5f);
                    other.gameObject.GetComponent<TorchStickController>().SelfDestroy();
                }
                if (other.CompareTag("Chest") && !hasChest)
                {
                    animator.SetTrigger("IsPickup");
                    hasChest = true;
                    isMovementFree = false;
                    Invoke(nameof(ChestPickup), 0.5f);
                    other.gameObject.GetComponent<ChestController>().Destroy();
                }
            }
            if (other.CompareTag("StickOnGround") && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Joystick1Button1))&& hasTorch)
            {
                TorchStickController torchController = other.gameObject.GetComponent<TorchStickController>();
                if (!torchController.isFireActive)
                {
                    torchController.SetActiveFire();
                    animator.SetTrigger("IsBurn");
                    isMovementFree = false;
                    Invoke(nameof(MovementToFree), 0.5f);
                }
            }
        }
    }

    void ChestPickup() 
    {
        MovementToFree();
        chest.SetActive(true);
    }

    void StickPickup()
    {
        isMovementFree = true;
        stickInHead.SetActive(true);
    }

    void TorchPickup()
    {
        animator.SetFloat("Speed", 0);
        torch.SetActive(true);
        lightOfTorch.SetActive(true);
        hasTorch = true;
        MovementToFree();
        Invoke(nameof(TorchDrop), 25f);
    }

    void MovementToFree()
    {
        isMovementFree = true;
    }

    void TorchDrop()
    {
        torch.SetActive(false);
        lightOfTorch.SetActive(false);
        hasTorch = false;
    }

    public float GetTimeWithoutLight()
    {
        return timeWithoutLight;
    }
}

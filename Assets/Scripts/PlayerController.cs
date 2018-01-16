using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum MorphStatus { INIT, HUMAN, DEFAULT_FISH };

public class PlayerController : MonoBehaviour
{
    //Referenzen
    private Rigidbody2D _rigidBody;
    private GameObject _carryRef;
    private GameObject _touchedAccepterRef = null;
    private GameObject _touchedCarryableRef = null;

    public Animator animatorHuman;
    public Animator animatorFish;

    public Transform groundFish;
    public Transform groundFishBack;
    public Transform groundHuman;

    public GameObject bubbles;
    public GameObject airMeterCanvasHolder;
    public GameObject[] airMeter;
    public GameObject hurtWarning;

    public float humanSpeedFactor = 0.75f;
    public float humanJumpHeightBase = 25f;
    private float humanJumpHeightCurrent;


    public float maxAirTime = 15f;
    private float airTime;
    private float timePerImage;


    //Audio
    public AudioSource audioSrc;
    public AudioClip sfxGrabItem;
    public AudioClip sfxPlaceItem;
    public AudioClip sfxTakeItem;
    public AudioClip sfxWaterSplash;
    public AudioClip sfxWaterPlay;
    public AudioClip sfxJump;
    public AudioClip sfxBoostJump;

    //Maximale Geschwindigkeit als Fisch
    public float maxFishSpeed = 4f;
    private float sqrMaxFishSpeed;

    //Beschleunigung als Fisch
    public float fishAccel = 0.4f;

    //"MorphStatus" - also der Zustand, in dem sich der Spieler befindet.
    private MorphStatus _morphStatus = MorphStatus.INIT;
    private MorphStatus _newStatus = MorphStatus.INIT;
    public MorphStatus initStatus = MorphStatus.INIT;

    public GameObject FishForm;
    public GameObject FishCollider;
    public GameObject HumanForm;

    //Beispiel für eine "erlernbare Fertigkeit". Sprechen mit Leertaste, wenn TRUE.
    public bool canSpeakUnderwater = false;

    //Derzeit default auf true, eventuell auch erlernbar
    public bool canGrabItems = true;
    public GameObject grabbedItemOrigin;
    public GameObject grabbedItemOriginHuman;

    //Blickrichtung
    private float facingAngle = 0;

    private bool _isGrounded;
    private bool _isInWater;

    private bool isHooked = false;
    private float hookFree = 0f;
    private float hookJerk = 0f;
    private Hook hookRef;


    void Start()
    {
        timePerImage = maxAirTime / airMeter.Length;
        airTime = maxAirTime;

        humanJumpHeightCurrent = humanJumpHeightBase;

        //debug
        if (Application.isEditor && (GameObject.Find("GameController") == null))
        {
            Debug.Log(SceneManager.GetActiveScene().name);
            SceneManager.LoadScene("MainMenu");
        }
        //end debug

        _rigidBody = GetComponent<Rigidbody2D>();
        sqrMaxFishSpeed = maxFishSpeed * maxFishSpeed;

        SetMorphStatus(initStatus);
        _newStatus = initStatus;

        Vector3 warpPos;
        if (GameController.Instance == null) warpPos = new Vector3(0, 0);
        else warpPos = new Vector3(GameController.Instance.warpX, GameController.Instance.warpY);

        //using 0/0 as "default start position"
        if (warpPos.sqrMagnitude != 0)
        {
            transform.SetPositionAndRotation(warpPos, transform.rotation);
        }
    }


    void Update()
    {
        CheckGameOver();

        _isGrounded = Physics2D.Linecast(transform.position, groundHuman.position, 1 << LayerMask.NameToLayer("Ground"));

        if (!_isInWater)
        {
            _isGrounded = _isGrounded ||
                   Physics2D.Linecast(transform.position, groundFish.position, 1 << LayerMask.NameToLayer("Ground")) ||
                   Physics2D.Linecast(transform.position, groundFishBack.position, 1 << LayerMask.NameToLayer("Ground"));
            _rigidBody.gravityScale = 4;
            airTime -= Time.deltaTime;

        }
        else
        {
            airTime += Time.deltaTime * 4.5f;
            airTime = Mathf.Min(airTime, maxAirTime);

            _rigidBody.gravityScale = 0;
        }

        if (airTime >= maxAirTime)
        {
            airMeterCanvasHolder.SetActive(false);
        }
        else
        {
            airMeterCanvasHolder.SetActive(true);
            // var hideAt = timePerImage;
            for (int i = 0; i < airMeter.Length; i++)
            {
                if (timePerImage * i >= airTime)
                {
                    airMeter[i].SetActive(false);
                }
                else
                {
                    airMeter[i].SetActive(true);
                }
            }
        }

        if (_isGrounded)
        {
            //Debug.Break();

            _newStatus = MorphStatus.HUMAN;
            /*if (animatorHuman != null)
                animatorHuman.SetBool("isHuman", true);*/
        }
        // Morphe character, when notwendig
        SetMorphStatus(_newStatus);

        // Wähle aus den Keycommands, je nach Morphstatus
        switch (_newStatus)
        {
            case MorphStatus.DEFAULT_FISH:
                if (!isHooked) { DefaultFishControls(); }
                else { FishOnHookControls(); }
                break;
            case MorphStatus.HUMAN:
                DefaultHumanControls();
                break;
        }

        // Update carried obj. position
        if (_carryRef != null)
        {
            var targetTransform = _morphStatus == MorphStatus.DEFAULT_FISH ? grabbedItemOrigin : grabbedItemOriginHuman;
            _carryRef.transform.position = targetTransform.transform.position;
        }

        // Code für animator
        if (animatorHuman != null)
        {
            //animator.SetFloat("speed", Mathf.Abs(_rigidBody.velocity.x / maxFishSpeed));
        }
    }

    void CheckGameOver()
    {
        bool isOver = false;

        if (airTime <= 0)
            isOver = true;

        if (isOver)
        {
            GameController.Instance.LoadLevel("GameOver");
        }
    }

    void SetMorphStatus(MorphStatus newStatus)
    {
        if (_morphStatus == newStatus) return;
        _morphStatus = newStatus;

        switch (newStatus)
        {
            default:
            case MorphStatus.DEFAULT_FISH:
                FishForm.SetActive(true);
                FishCollider.SetActive(true);
                HumanForm.SetActive(false);
                //_rigidBody.velocity = new Vector2(_rigidBody.velocity.x * .5f, _rigidBody.velocity.y * .5f);
                _rigidBody.drag = 3;
                break;
            case MorphStatus.HUMAN:
                FishForm.SetActive(false);
                FishCollider.SetActive(false);
                HumanForm.SetActive(true);
                //
                _rigidBody.drag = 8;
                break;
        }
    }

    void DefaultFishControls()
    {
        if (_isInWater)
        {

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space)) _rigidBody.velocity += new Vector2(0, fishAccel);
            if (Input.GetKey(KeyCode.S)) _rigidBody.velocity -= new Vector2(0, fishAccel);

            if (Input.GetKey(KeyCode.A))
            {
                _rigidBody.velocity -= new Vector2(fishAccel, 0);
                facingAngle = 180;
            }
            if (Input.GetKey(KeyCode.D))
            {
                _rigidBody.velocity += new Vector2(fishAccel, 0);
                facingAngle = 0;
            }

            if (_rigidBody.velocity.sqrMagnitude > sqrMaxFishSpeed)
            {
                _rigidBody.velocity = _rigidBody.velocity.normalized * maxFishSpeed;
            }

            animatorFish.SetBool("grounded", true);
            float animationSpeed = 0;
            if (_rigidBody.velocity.magnitude > 1)
            {
                animationSpeed = Mathf.Abs(_rigidBody.velocity.magnitude) / maxFishSpeed * 3;
            }

            animatorFish.SetFloat("velocityX", animationSpeed);
        }
        else
        {
            if (Input.GetKey(KeyCode.A)) _rigidBody.velocity -= new Vector2(fishAccel, 0);
            if (Input.GetKey(KeyCode.D)) _rigidBody.velocity += new Vector2(fishAccel, 0);

        }

        if (Input.GetKey(KeyCode.Space))
        {
            if (canSpeakUnderwater) { Debug.Log("I am a fish."); }
            else { Debug.Log("Blub."); }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            HandleCarryableObject();
        }

        transform.eulerAngles = new Vector3(0, facingAngle, (_rigidBody.velocity.y * 8f));
        FishCollider.transform.eulerAngles = new Vector3(0, 0, 90);

    }

    void FishOnHookControls()
    {
        //Debug.Log(hookFree); // log escape status

        //Spam SPACE key to escape
        hookFree = Mathf.Max(hookFree - 2f * Time.deltaTime, -5);
        if (Input.GetKeyUp(KeyCode.Space))
        {
            hookFree += 1;
            hookJerk = Random.Range(-15, 15);
        }

        if (hookFree > 0)
        {
            isHooked = false;
            hookRef.Deactivate();
        }
        transform.position = new Vector3(hookRef.transform.position.x - 0.25f, hookRef.transform.position.y - 0.4f);
        transform.eulerAngles = new Vector3(0, 0, hookJerk + 90 + Mathf.Sin(transform.position.y * 4) * 10);
    }

    void DefaultHumanControls()
    {
        if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space)) && _isGrounded)
        {
            animatorHuman.SetBool("jump", true);
            _isGrounded = false;
            _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, humanJumpHeightCurrent);
            audioSrc.Stop();
            if (humanJumpHeightBase == humanJumpHeightCurrent)
            {
                audioSrc.pitch = Random.Range(0.95f, 1.15f);
                audioSrc.PlayOneShot(sfxJump, 0.2f);
            }
            else
            {
                audioSrc.pitch = 1.0f;
                audioSrc.PlayOneShot(sfxBoostJump, 0.4f);
            }
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            animatorHuman.SetBool("jump", false);
            if (_rigidBody.velocity.y > 0)
            {
                _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, _rigidBody.velocity.y * 0.5f);
            }
        }
        if (Input.GetKey(KeyCode.A))
        {
            _rigidBody.velocity -= new Vector2(humanSpeedFactor, 0);
            facingAngle = 180;
        }
        if (Input.GetKey(KeyCode.D))
        {
            _rigidBody.velocity += new Vector2(humanSpeedFactor, 0);
            facingAngle = 0;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            HandleCarryableObject();
        }

        animatorHuman.SetBool("grounded", _isGrounded);
        animatorHuman.SetFloat("velocityX", Mathf.Abs(_rigidBody.velocity.x));

        transform.eulerAngles = new Vector3(0, facingAngle, 0);
    }


    void HandleCarryableObject()
    {
        audioSrc.pitch = 1;
        if (!canGrabItems) return;

        if (_touchedAccepterRef != null)
        {
            if (_carryRef != null)
                audioSrc.PlayOneShot(sfxPlaceItem, 0.22f);
            else
                audioSrc.PlayOneShot(sfxTakeItem, 0.22f);

            GameObject prevAccepterItem = _touchedAccepterRef.gameObject.GetComponent<ObjectAccepter>().GetItemRefBeforeRelease();



            _touchedAccepterRef.gameObject.GetComponent<ObjectAccepter>().Release();
            _touchedAccepterRef.gameObject.GetComponent<ObjectAccepter>().Hold(_carryRef);


            if (_carryRef != null)
            {

            }


            _carryRef = prevAccepterItem;


        }

        if (_touchedCarryableRef && !_touchedAccepterRef)
        {
            if (_carryRef == null)
            {
                _carryRef = _touchedCarryableRef.gameObject;
              
                audioSrc.PlayOneShot(sfxGrabItem, 0.1f);
            }
            else
            {
  
                _carryRef = null;
            }
        }
 
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Powerup"))
        {
            Powerup powerUp = (Powerup)other.GetComponent("Powerup");
            powerUp.Collect();
            switch (powerUp.pType)
            {
                case PowerupType.LEARN_SPEECH:
                    canSpeakUnderwater = true;
                    break;
                case PowerupType.SHRINK:
                    transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
                    break;
                case PowerupType.AIR:
                    airTime = maxAirTime;
                    break;
                case PowerupType.NONE:
                default:
                    Debug.Log("Powerup didn't have any effect");
                    break;
            }
        }
        else if (other.gameObject.CompareTag("Trigger"))
        {
            Trigger trigger = (Trigger)other.GetComponent("Trigger");
            trigger.Collect();
        }
        else if (other.gameObject.CompareTag("Hook") && !isHooked)
        {
            hookRef = (Hook)(other.GetComponent("Hook"));
            hookRef.Activate();
            isHooked = true;
            hookFree = -5;
        }
        else if (other.gameObject.CompareTag("Warp"))
        {
            var warpObj = (WarpArrow)(other.GetComponent(typeof(WarpArrow)));

            GameController.Instance.warpX = warpObj.warpX;
            GameController.Instance.warpY = warpObj.warpY;
            GameController.Instance.LoadLevel(warpObj.LevelName);
        }

        if (other.gameObject.CompareTag("AcceptsObject")) _touchedAccepterRef = other.gameObject;
        if (other.gameObject.CompareTag("Carryable")) _touchedCarryableRef = other.gameObject;

        if (other.gameObject.CompareTag("WaterLevel"))
        {
            bubbles.SetActive(true);
            _isInWater = true;
            _newStatus = MorphStatus.DEFAULT_FISH;

            //animator.SetBool("isHuman", false);
            if (Mathf.Abs(_rigidBody.velocity.y) > 5)
            {
                audioSrc.Stop();
                audioSrc.pitch = Random.Range(0.95f, 1.1f);
                audioSrc.PlayOneShot(sfxWaterSplash, -0.03f * _rigidBody.velocity.y);
            }
        }
        if (other.gameObject.CompareTag("JumpPad"))
        {
            humanJumpHeightCurrent = humanJumpHeightBase * 3;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {

        if (other.gameObject.CompareTag("WaterLevel") && _isInWater)
        {
            bubbles.SetActive(false);
            _isInWater = false;
            _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, Mathf.Min(15, _rigidBody.velocity.y * 3.5f));
            audioSrc.Stop();
            audioSrc.pitch = Random.Range(0.95f, 1.1f);
            audioSrc.PlayOneShot(sfxWaterPlay, 0.07f);
        }

        if (other.gameObject.CompareTag("AcceptsObject")) _touchedAccepterRef = null;
        if (other.gameObject.CompareTag("Carryable")) _touchedCarryableRef = null;
        if (_carryRef != null) _touchedCarryableRef = _carryRef;

        if (other.gameObject.CompareTag("JumpPad"))
        {
            humanJumpHeightCurrent = humanJumpHeightBase;
        }

        if (other.gameObject.CompareTag("DamageSource"))
        {
            hurtWarning.SetActive(false);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("DamageSource"))
        {
            //Debug.Log(Random.value);
            airTime -= 10f * Time.deltaTime;
            hurtWarning.SetActive(true);
        }
    }
}
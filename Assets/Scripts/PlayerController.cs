using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
// ReSharper disable All

public enum MorphStatus { INIT, HUMAN, DEFAULT_FISH };

public class PlayerController : MonoBehaviour
{
    //Referenz auf Rigidbody
    private Rigidbody2D rigidBody;

    //Maximale Geschwindigkeit als Fisch
    public float maxFishSpeed = 4f;
    private float sqrMaxFishSpeed;

    //Beschleunigung als Fisch
    public float fishAccel = 0.4f;

    //Derzeitiger "MorphStatus" - also der Zustand, in dem sich der Spieler befindet.
    private MorphStatus _morphStatus = MorphStatus.INIT;
    public MorphStatus initStatus = MorphStatus.INIT;

    public GameObject FishForm;
    public GameObject HumanForm;

    //Beispiel für eine "erlernbare Fertigkeit". Sprechen mit Leertaste, wenn TRUE.
    public bool canSpeakUnderwater = false;

    //Derzeit default auf true, eventuell auch erlernbar
    public bool canGrabItems = true;
    public GameObject grabbedItemOrigin;

    private GameObject _carryRef;
    private GameObject _touchedAccepterRef = null;
    private GameObject _touchedCarryableRef = null;

    //Animator
    [CanBeNull] public Animator animator = null;

    //Blickrichtung
    private float facingAngle = 0;

    private bool isHooked = false;
    private float hookFree = 0f;
    private float hookJerk = 0f;
    private Hook hookRef;


    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        sqrMaxFishSpeed = maxFishSpeed * maxFishSpeed;

        SetMorphStatus(initStatus);
    }

    void Update()
    {
        // Morphe character, when notwendig
        SetMorphStatus(_morphStatus);

        // Wähle aus den Keycommands, je nach Morphstatus
        switch (_morphStatus)
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
            _carryRef.transform.position = grabbedItemOrigin.transform.position;
        }

        // Code für animator
        if (animator != null)
        {
            animator.SetFloat("speed", Mathf.Abs(rigidBody.velocity.x / maxFishSpeed));
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
                HumanForm.SetActive(false);
                break;
            case MorphStatus.HUMAN:
                FishForm.SetActive(false);
                HumanForm.SetActive(true);
                break;
        }
    }

    void DefaultFishControls()
    {

        if (Input.GetKey(KeyCode.W)) rigidBody.velocity += new Vector2(0, fishAccel);
        if (Input.GetKey(KeyCode.S)) rigidBody.velocity -= new Vector2(0, fishAccel);
        if (Input.GetKey(KeyCode.A))
        {
            rigidBody.velocity -= new Vector2(fishAccel, 0);
            facingAngle = 180;
        }
        if (Input.GetKey(KeyCode.D))
        {
            rigidBody.velocity += new Vector2(fishAccel, 0);
            facingAngle = 0;
        }

        if (rigidBody.velocity.sqrMagnitude > sqrMaxFishSpeed)
        {
            rigidBody.velocity = rigidBody.velocity.normalized * maxFishSpeed;
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

        transform.eulerAngles = new Vector3(0, facingAngle, (rigidBody.velocity.y * 8f));

        //"morph test" - einfach ab einer gewissen höhe status auf "Mensch" setzen
        if (transform.position.y > 3.3)
           SetMorphStatus(MorphStatus.HUMAN);
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
        if (Input.GetKey(KeyCode.Space)) Debug.Log("I am human.");

        //nur test code... einfach mit "S" wieder nach unten
        if (Input.GetKey(KeyCode.S))
            rigidBody.velocity -= new Vector2(0, 0.1f);

        //wenn man wieder "unter die grenze kommt", dann wird man wieder zum Fisch
        if (transform.position.y < 3.2)
            SetMorphStatus(MorphStatus.DEFAULT_FISH);
            
        if (Input.GetKeyDown(KeyCode.E))
        {
            HandleCarryableObject();
        }
    }

    void HandleCarryableObject()
    {
        if (!canGrabItems) return;

        if (_touchedAccepterRef != null)
        {
            GameObject prevAccepterItem = _touchedAccepterRef.gameObject.GetComponent<ObjectAccepter>().GetItemRefBeforeRelease();
            _touchedAccepterRef.gameObject.GetComponent<ObjectAccepter>().Release();
            _touchedAccepterRef.gameObject.GetComponent<ObjectAccepter>().Hold(_carryRef);
            _carryRef = prevAccepterItem;
        }

        if (_touchedCarryableRef && !_touchedAccepterRef)
        {
            if (_carryRef == null)
            {
                _carryRef = _touchedCarryableRef.gameObject;
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

        if (other.gameObject.CompareTag("AcceptsObject")) _touchedAccepterRef = other.gameObject;
        if (other.gameObject.CompareTag("Carryable")) _touchedCarryableRef = other.gameObject;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("WaterLevel"))
        {
            _morphStatus = MorphStatus.DEFAULT_FISH;
            animator.SetBool("isHuman", false);
        }
        else if (other.gameObject.CompareTag("GroundLevel"))
        {
            _morphStatus = MorphStatus.HUMAN;
            animator.SetBool("isHuman", true);

        }

        if (other.gameObject.CompareTag("AcceptsObject")) _touchedAccepterRef = null;
        if (other.gameObject.CompareTag("Carryable")) _touchedCarryableRef = null;
        if (_carryRef != null) _touchedCarryableRef = _carryRef;
    }
}
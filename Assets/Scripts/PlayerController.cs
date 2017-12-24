﻿using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
// ReSharper disable All

public enum MorphStatus { INIT, HUMAN, DEFAULT_FISH };

public class PlayerController : MonoBehaviour
{
    //Referenzen
    private Rigidbody2D _rigidBody;
    private GameObject _carryRef;
    private GameObject _touchedAccepterRef = null;
    private GameObject _touchedCarryableRef = null;
    public Transform groundFish;
    public Transform groundHuman;

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
    public GameObject HumanForm;

    //Beispiel für eine "erlernbare Fertigkeit". Sprechen mit Leertaste, wenn TRUE.
    public bool canSpeakUnderwater = false;

    //Derzeit default auf true, eventuell auch erlernbar
    public bool canGrabItems = true;
    public GameObject grabbedItemOrigin;



    //Animator
    [CanBeNull] public Animator animator = null;

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
        _rigidBody = GetComponent<Rigidbody2D>();
        sqrMaxFishSpeed = maxFishSpeed * maxFishSpeed;

        SetMorphStatus(initStatus);
    }

    void Update()
    {
        _isGrounded = Physics2D.Linecast(transform.position, groundHuman.position, 1 << LayerMask.NameToLayer("Ground"));
        if (_isInWater)
        {
            _rigidBody.gravityScale = 0;
        }
        else
        {
            _rigidBody.gravityScale = 4;
        }

        if (_isGrounded)
        {
            _newStatus = MorphStatus.HUMAN;
            if (animator != null)
                animator.SetBool("isHuman", true);
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
            _carryRef.transform.position = grabbedItemOrigin.transform.position;
        }

        // Code für animator
        if (animator != null)
        {
            animator.SetFloat("speed", Mathf.Abs(_rigidBody.velocity.x / maxFishSpeed));
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
                //_rigidBody.velocity = new Vector2(_rigidBody.velocity.x * .5f, _rigidBody.velocity.y * .5f);
                _rigidBody.drag = 3;
                break;
            case MorphStatus.HUMAN:
                FishForm.SetActive(false);
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
            if (Input.GetKey(KeyCode.W)) _rigidBody.velocity += new Vector2(0, fishAccel);
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
        }
        else
        {
            if (Input.GetKey(KeyCode.A)) _rigidBody.velocity -= new Vector2(fishAccel / 5, 0);
            if (Input.GetKey(KeyCode.D)) _rigidBody.velocity += new Vector2(fishAccel / 5, 0);

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
        if (Input.GetKey(KeyCode.Space) && _isGrounded)
        {
            _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, 20);
        }//Debug.Log("I am human.");

        if (Input.GetKey(KeyCode.A))
        {
            _rigidBody.velocity -= new Vector2(0.6f, 0);
            facingAngle = 180;
        }
        if (Input.GetKey(KeyCode.D))
        {
            _rigidBody.velocity += new Vector2(0.6f, 0);
            facingAngle = 0;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            HandleCarryableObject();
        }

        transform.eulerAngles = new Vector3(0, facingAngle, 0);
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

        if (other.gameObject.CompareTag("WaterLevel"))
        {
            _isInWater = true;
            _newStatus = MorphStatus.DEFAULT_FISH;
            if (animator != null)
                animator.SetBool("isHuman", false);

        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("WaterLevel"))
        {
            _isInWater = false;
            _rigidBody.velocity = new Vector2(_rigidBody.velocity.x * 1.3f, Mathf.Min(12, _rigidBody.velocity.y * 2f));
        }
        if (other.gameObject.CompareTag("AcceptsObject")) _touchedAccepterRef = null;
        if (other.gameObject.CompareTag("Carryable")) _touchedCarryableRef = null;
        if (_carryRef != null) _touchedCarryableRef = _carryRef;
    }
}
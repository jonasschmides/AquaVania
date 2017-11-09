using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MorphStatus { HUMAN, DEFAULT_FISH };

public class PlayerController : MonoBehaviour
{
    //Referenz auf Rigidbody
    private Rigidbody2D rigidBody;

    //Maximale Geschwindigkeit als Fisch
    public float maxFishSpeed = 4f;
    private float sqrMaxFishSpeed;

    //Beschleunigung als Fisch
    public float fishAccel = 0.5f;

    //Derzeitiger "MorphStatus" - also der Zustand, in dem sich der Spieler befindet.
    public MorphStatus morphStatus = MorphStatus.DEFAULT_FISH;

    //Beispiel für eine "erlernbare Fertigkeit". Sprechen mit Leertaste, wenn TRUE.
    public bool canSpeakUnderwater = false;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        sqrMaxFishSpeed = maxFishSpeed * maxFishSpeed;
    }

    void Update()
    {
        //Wähle aus den Keycommands, je nach Morphstatus
        switch (morphStatus)
        {
            case MorphStatus.DEFAULT_FISH:
                DefaultFishControls();
                break;
            case MorphStatus.HUMAN:
                DefaultHumanControls();
                break;
        }
    }



    void DefaultFishControls()
    {

        float facingAngle = 0;

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
            if (canSpeakUnderwater)
            {
                Debug.Log("I am a fish.");
            }
            else
            {
                Debug.Log("Blub.");
            }
        }

        transform.eulerAngles = new Vector3(0, facingAngle, (rigidBody.velocity.y * 8f));

        //"morph test" - einfach ab einer gewissen höhe status auf "Mensch" setzen
        if (transform.position.y > 3)
            morphStatus = MorphStatus.HUMAN;
    }

    void DefaultHumanControls()
    {
        if (Input.GetKey(KeyCode.Space)) Debug.Log("I am human.");

        //nur test code... einfach mit "S" wieder nach unten
        if (Input.GetKey(KeyCode.S))
            rigidBody.velocity -= new Vector2(0, 0.1f);

        //wenn man wieder "unter die grenze kommt", dann wird man wieder zum Fisch
        if (transform.position.y < 2.9)
            morphStatus = MorphStatus.DEFAULT_FISH;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Powerup"))
        {
            Powerup powerUp = (Powerup)other.GetComponent("Powerup");
            powerUp.Collect();
            //Destroy(other.gameObject);
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
    }
}

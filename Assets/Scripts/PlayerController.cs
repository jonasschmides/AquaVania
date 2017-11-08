using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MorphStatus { HUMAN, DEFAULT_FISH };

public class PlayerController : MonoBehaviour
{
    //Referenz auf Rigidbody
    public Rigidbody2D rigidBody;

    //Maximale Geschwindigkeit als Fisch
    public float maxFishSpeed = 5f;
    private float sqrMaxFishSpeed;

    //Beschleunigung als Fisch
    public float fishAccel = 0.6f;

    //Derzeitiger "MorphStatus" - also der Zustand, in dem sich der Spieler befindet.
    public MorphStatus morphStatus = MorphStatus.DEFAULT_FISH;

    //Beispiel für eine "erlernbare Fertigkeit". Sprechen mit Leertaste, wenn TRUE.
    public bool canSpeakUnderwater = false;

    void Start()
    {
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

        if (Input.GetKey(KeyCode.W)) rigidBody.velocity += new Vector2(0, fishAccel);
        if (Input.GetKey(KeyCode.S)) rigidBody.velocity -= new Vector2(0, fishAccel);
        if (Input.GetKey(KeyCode.A))
        {
            rigidBody.velocity -= new Vector2(fishAccel, 0);
            if (transform.localScale.x > 0) transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, 1);
        }
        if (Input.GetKey(KeyCode.D))
        {
            rigidBody.velocity += new Vector2(fishAccel, 0);
            if (transform.localScale.x < 0) transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, 1);
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

        transform.eulerAngles = new Vector3(0, 0, rigidBody.velocity.y * 5 * Mathf.Sign(rigidBody.velocity.x));

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
            Powerup type = (Powerup)other.GetComponent("Powerup");
            Destroy(other.gameObject);
            switch (type.pType)
            {
                case PowerupType.LEARN_SPEECH:
                    canSpeakUnderwater = true;
                    break;
                case PowerupType.SHRINK:
                    transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                    break;
                case PowerupType.NONE:
                default:
                    Debug.Log("Powerup didn't have any effect");
                    break;
            }
        }
    }
}

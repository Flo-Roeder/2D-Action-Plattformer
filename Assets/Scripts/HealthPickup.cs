using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{

    public int healAmount = 20;
    public Vector3 spinRotationSpeed = new Vector3(0,180,0);

    AudioSource pickUpAudioSource;

    private void Awake()
    {
        pickUpAudioSource=GetComponent<AudioSource>();
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        transform.eulerAngles+= spinRotationSpeed*Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damageable damageable= collision.GetComponent<Damageable>();

        if (damageable)
        {
            damageable.Heal(healAmount);
            if (pickUpAudioSource)
            {
                AudioSource.PlayClipAtPoint(pickUpAudioSource.clip, gameObject.transform.position, pickUpAudioSource.volume);
            }
            Destroy(gameObject);
        }
    }
}

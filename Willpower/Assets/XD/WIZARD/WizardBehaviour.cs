using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WizardBehaviour : MonoBehaviour
{
    [SerializeField] WizardSpawner wizspa;
    [SerializeField] Transform spawnLocation;
    [SerializeField] GameObject playerObj;
    [SerializeField] ConvertoTo2D ConvertoTo2D;


    PlayerMovement pmmammam;

    private Vector3 newLocation;
    private float speed = 3f;

    private float step;

    // Start is called before the first frame update
    void Start()
    {

        pmmammam = FindObjectOfType<PlayerMovement>();
        wizspa = FindObjectOfType<WizardSpawner>();
        ConvertoTo2D = GetComponent<ConvertoTo2D>();
        step = speed * Time.deltaTime;
        newLocation = spawnLocation.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(playerObj.transform.position);

        transform.position = Vector3.MoveTowards(transform.position, newLocation, step);

        if (Vector3.Distance(transform.position, newLocation) < 0.5f) NewRandomLocation();
    }

    private void NewRandomLocation()
    {
        newLocation = new Vector3(Random.Range(-5.0f, 5.0f)+spawnLocation.position.x, Random.Range(-2.0f, 2.0f)+spawnLocation.position.y, Random.Range(-5.0f, 5.0f)+spawnLocation.position.z);
    }

    private void OnCollisionEnter(Collision collision)
    {
        wizspa.wizCur--;
        if (collision.gameObject.CompareTag("Bullet"))
        {

            pmmammam.audiothing.sound(pmmammam.audiothing.ripSo);
            ConvertoTo2D.ConvertTo2DSprite();
        }
            Destroy(this);
    }
}

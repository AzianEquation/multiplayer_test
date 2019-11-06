using UnityEngine;
using System.Collections;
using UnityEngine.Networking;


public class Bomb : NetworkBehaviour {

    [Range(-50f, 100f)]
    public float Power = 10f;
    [Range(-5f, 8f)]
    public float upPower = 4f;

    [Range(1f, 20f)]
    public float Radius = 5f;

    [Range(0f, 10f)]
    public float Timer = 5f;

    [Range(0f, 1000f)]
    public float Damage = 200f;

    [SerializeField]
    private bool ExplodeOnImpact = false;

    public GameObject shooter;

    public GameObject effect;

    private Health _hp;



    private void Start()
    {
        if(isServer)
        {
            RpcStart();
        }

        StartCoroutine(tick());
    }
    [ClientRpc]
    void RpcStart()
    {
        StartCoroutine(tick());
    }
    IEnumerator tick()
    {
        gameObject.GetComponent<Collider>().enabled = true;
        yield return new WaitForSeconds(Timer + Random.Range(-0.5f, 0.5f));
        if(isServer)
        {
            RpcExplode();
        }

    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject == shooter)
        {
            Physics.IgnoreCollision(shooter.GetComponent<Collider>(), GetComponent<Collider>(), true);
        }
        if(ExplodeOnImpact)
        {
            if(isServer)
            {
                RpcExplode();
            }

        }
    }

    [ClientRpc]
    void RpcExplode()
    {
        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, Radius);
        foreach(Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if(rb != null)
            {
                rb.AddExplosionForce(Power, explosionPos, Radius, upPower, ForceMode.Impulse);
            }

            _hp = hit.GetComponent<Health>();
            if(_hp != null)
            {
                _hp.health -= (Damage / ((Vector3.Distance(explosionPos, _hp.transform.position)) +1) * 1.5f);
                _hp.Refresh();//
                _hp.lastDamager = shooter;
            }
        }
        GameObject _effect = Instantiate(effect, transform.position, transform.rotation);
        //NetworkServer.Spawn(_effect);
        Destroy(_effect, 2f);
        Destroy(gameObject, 0.1f);

    }





 
}

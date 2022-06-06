using System.Collections;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] float _duration = 10f;
    [SerializeField] float _delayMultiplier = 0.5f;
    [SerializeField] float _cooldown = 10f;

    [SerializeField] Transform[] _spawnPoints;
    [SerializeField] bool _spread;

    public float DelayMultiplier => _delayMultiplier;

    public bool SpreadShot => _spread;

    void OnTriggerEnter(Collider other)
    {
        var playerWeapon = other.GetComponent<PlayerWeapon>();
        if(playerWeapon)
        {
            playerWeapon.AddPowerUp(this);
            StartCoroutine(DisableAfterDelay(playerWeapon));
            GetComponent<Collider>().enabled = false;
            GetComponent<Renderer>().enabled = false;
        }
    }
    IEnumerator DisableAfterDelay(PlayerWeapon playerWeapon)
    {
        yield return new WaitForSeconds(_duration);
        playerWeapon.RemovePowerUp(this);
        yield return new WaitForSeconds(_cooldown);
        
        int randomIndex = UnityEngine.Random.Range(0, _spawnPoints.Length);
        transform.position = _spawnPoints[randomIndex].position;
        GetComponent<Collider>().enabled = true;
        GetComponent<Renderer>().enabled = true;
    }
}

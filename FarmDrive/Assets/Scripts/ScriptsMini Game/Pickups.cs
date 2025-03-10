using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickups : MonoBehaviour
{
    public GameObject burstParticlesPrefab;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
        
            GameManager1.instance.IncrementScore(2);
            MoneyController.Instance.AddMoney(100);
            Instantiate(burstParticlesPrefab, transform.position, Quaternion.identity);
            this.gameObject.SetActive(false);
        }
    }
}
